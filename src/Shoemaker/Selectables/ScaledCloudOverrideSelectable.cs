using Newtonsoft.Json.Linq;
using PatchManager.SassyPatching;
using PatchManager.SassyPatching.Interfaces;
using PatchManager.SassyPatching.Selectables;
using Shoemaker.Modifiables;
using Shoemaker.Overrides;

namespace Shoemaker.Selectables;

public sealed class ScaledCloudOverrideSelectable : BaseSelectable
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

    public readonly JObject ScaledCloudOverrideObject;

    public ScaledCloudOverrideSelectable(JObject scaledCloudOverrideObject)
    {
        Children = [];
        Classes = [];
        ElementType = "scaled_cloud_override";
        ScaledCloudOverrideObject = scaledCloudOverrideObject;
        Name = scaledCloudOverrideObject["PlanetName"].Value<string>();
        foreach (var (key, child) in scaledCloudOverrideObject)
        {
            Classes.Add(key);
        }

        foreach (var (key, child) in (JObject)scaledCloudOverrideObject["LayerOverrides"])
        {
            Classes.Add(key);
            Children.Add(new JTokenSelectable(SetModified, child, key, "layer_override"));
        }
    }
    
    /// <inheritdoc />
    public override List<ISelectable> Children { get; }

    /// <inheritdoc />
    public override string Name { get; }

    /// <inheritdoc />
    public override List<string> Classes { get; }

    public override bool MatchesClass(string @class, out DataValue classValue)
    {
        if (((JObject)ScaledCloudOverrideObject["LayerOverrides"])!.TryGetValue(@class, out var token))
        {
            classValue = DataValue.FromJToken(token);
            return true;
        }

        classValue = DataValue.Null;
        return false;
    }

    /// <inheritdoc />
    public override bool IsSameAs(ISelectable other) =>
        other is ScaledCloudOverrideSelectable atmosphereOverrideSelectable && atmosphereOverrideSelectable.ScaledCloudOverrideObject == ScaledCloudOverrideObject;
    
    /// <inheritdoc />
    public override IModifiable OpenModification() => new ScaledCloudOverrideModifiable(this);

    public override ISelectable AddElement(string elementType)
    {
        var newLayer = new LayerOverride();
        var obj = JObject.FromObject(newLayer);
        ((JObject)ScaledCloudOverrideObject["LayerOverrides"])!.Add(elementType, obj);
        obj = (JObject)ScaledCloudOverrideObject["LayerOverrides"]![elementType];
        var selectable = new JTokenSelectable(SetModified, obj, elementType, "layer_override");
        Children.Add(selectable);
        Classes.Add(elementType);
        return selectable;
    }
    /// <inheritdoc />
    public override string Serialize() => _deleted ? "" : ScaledCloudOverrideObject.ToString();

    /// <inheritdoc />
    public override DataValue GetValue() => DataValue.FromJToken(ScaledCloudOverrideObject);
    public override string ElementType { get; }
}