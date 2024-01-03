using Newtonsoft.Json.Linq;
using PatchManager.SassyPatching;
using PatchManager.SassyPatching.Interfaces;
using PatchManager.SassyPatching.Selectables;
using Shoemaker.Modifiables;

namespace Shoemaker.Selectables;

public sealed class CelestialBodySelectable : BaseSelectable
{
#pragma warning disable CS0414 // Field is assigned but its value is never used
    private bool _modified;
#pragma warning restore CS0414 // Field is assigned but its value is never used
    private bool _deleted;

    /// <summary>
    /// Marks this part selectable as having been modified any level down
    /// </summary>
    public void SetModified()
    {
        _modified = true;
    }

    /// <summary>
    /// Marks this part as goneso
    /// </summary>
    public void SetDeleted()
    {
        SetModified();
        _deleted = true;
    }

    public JObject CelestialBodyObject;
    public JObject DataObject;

    public CelestialBodySelectable(JObject celestialBodyObject)
    {
        ElementType = "celestial_body";
        CelestialBodyObject = celestialBodyObject;
        DataObject = (JObject)celestialBodyObject["data"];
        Classes = [];
        Children = [];
        foreach (var subToken in DataObject)
        {
            Classes.Add(subToken.Key);
            Children.Add(new JTokenSelectable(SetModified,subToken.Value,subToken.Key));
        }
    }
    
    public override bool MatchesClass(string @class, out DataValue classValue)
    {
        classValue = null;
        if (!MatchesClass(@class))
        {
            return false;
        }

        classValue = DataValue.FromJToken(DataObject[@class]);
        return true;
    }

    public override bool IsSameAs(ISelectable other) => other is CelestialBodySelectable selectable &&
                                                        selectable.CelestialBodyObject == CelestialBodyObject;

    public override IModifiable OpenModification() => new CelestialBodyModifiable(this);

    public override ISelectable AddElement(string elementType)
    {
        var obj = new JObject();
        DataObject[elementType] = obj;
        var n = new JTokenSelectable(SetModified, obj, elementType);
        Children.Add(n);
        return n;
    }

    public override string Serialize() => _deleted ? "" : CelestialBodyObject.ToString();

    public override DataValue GetValue() => DataValue.FromJToken(DataObject);

    public override List<ISelectable> Children { get; }
    public override string Name => DataObject["bodyName"]!.Value<string>();
    public override List<string> Classes { get; }
    public override string ElementType { get; }
}