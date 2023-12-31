using HarmonyLib;
using KSP.Rendering.Planets;

namespace Shoemaker.Patches;

[HarmonyPatch(typeof(VolumeCloudManager))]
internal static class VolumeCloudManagerPatches
{
    [HarmonyPatch(nameof(VolumeCloudManager.OnScaledCloudModelLoaded))]
    [HarmonyPrefix]
    public static void ScaleClouds(VolumeCloudManager __instance, string bodyName, ScaledCloudConfiguration model)
    {
        LogInfo($"Loaded a scaled cloud configuration for {bodyName}");
        LogInfo("It has the following layers");
        foreach (var layer in model.scaledCloudLayers)
        {
            LogInfo(layer.CloudLayerName);
            LogInfo($"- Radius: {layer.Radius}");
        }
    }
}