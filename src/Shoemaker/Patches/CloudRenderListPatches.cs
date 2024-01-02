using HarmonyLib;
using KSP.Rendering;
using KSP.VolumeCloud;

namespace Shoemaker.Patches;

[HarmonyPatch(typeof(CloudRenderList))]
internal static class CloudRenderListPatches
{
    [HarmonyPatch(nameof(CloudRenderList.SyncCloudsConfig))]
    [HarmonyPostfix]
    public static void SyncCloudsConfig(ScaledCloudDataModelComponent scaledCloudModel,
        VolumeCloudConfiguration config)
    {
        if (OverrideManager.ScaledCloudOverrides.TryGetValue(scaledCloudModel.PlanetName.ToLowerInvariant(),
                out var scaledCloudOverride))
        {
            scaledCloudOverride.ApplyTo(scaledCloudModel.ScaledCloudConfiguration);
            
            LogInfo($"Applied override to the config for {scaledCloudModel.PlanetName} after syncing");
            LogInfo("Layers:");
            foreach (var layer in scaledCloudModel.ScaledCloudConfiguration.scaledCloudLayers)
            {
                LogInfo(layer.CloudLayerName);
                LogInfo($"- Radius: {layer.Radius}");
            }
        }
    }
}