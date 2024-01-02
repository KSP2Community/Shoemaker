using Newtonsoft.Json.Linq;
using PatchManager.SassyPatching;
using PatchManager.SassyPatching.Attributes;
using PatchManager.SassyPatching.Interfaces;
using PatchManager.SassyPatching.NewAssets;
using Shoemaker.Overrides;
using Shoemaker.Selectables;

namespace Shoemaker.Rulesets;
[PatcherRuleset("scaled-cloud-override","scaled_cloud_overrides")]
public class ScaledCloudOverrideRuleset : IPatcherRuleSet
{
    public bool Matches(string label) => label == "scaled_cloud_overrides";

    public ISelectable ConvertToSelectable(string type, string name, string jsonData) => new ScaledCloudOverrideSelectable(JObject.Parse(jsonData));

    public INewAsset CreateNew(List<DataValue> dataValues)
    {
        var planetName = dataValues[0].String.ToLowerInvariant();
        var data = new ScaledCloudOverride
        {
            PlanetName = planetName
        };
        return new NewGenericAsset("scaled_cloud_overrides", $"scaled_cloud_override_{planetName}",
            new ScaledCloudOverrideSelectable(JObject.FromObject(data)));
    }
}