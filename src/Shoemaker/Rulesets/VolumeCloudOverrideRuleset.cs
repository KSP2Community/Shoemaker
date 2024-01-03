using Newtonsoft.Json.Linq;
using PatchManager.SassyPatching;
using PatchManager.SassyPatching.Attributes;
using PatchManager.SassyPatching.Interfaces;
using PatchManager.SassyPatching.NewAssets;
using Shoemaker.Overrides;
using Shoemaker.Selectables;

namespace Shoemaker.Rulesets;

[PatcherRuleset("volume-cloud-override","volume_cloud_overrides")]
public class VolumeCloudOverrideRuleset : IPatcherRuleSet
{
    public bool Matches(string label) => label == "volume_cloud_overrides";

    public ISelectable ConvertToSelectable(string type, string name, string jsonData) => new VolumeCloudSelectable(JObject.Parse(jsonData));

    public INewAsset CreateNew(List<DataValue> dataValues)
    {
        var planetName = dataValues[0].String.ToLowerInvariant();
        var data = new VolumeCloudConfigurationOverride
        {
            bodyName = planetName
        };
        return new NewGenericAsset("volume_cloud_overrides", $"volume_cloud_override_{planetName}",
            new VolumeCloudSelectable(JObject.FromObject(data)));
    }
}