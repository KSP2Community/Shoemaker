using HarmonyLib;
using KSP;
using KSP.Game;
using KSP.Rendering;
using UnityEngine;

namespace Shoemaker.Patches;

[HarmonyPatch(typeof(ScaledCloudDataModelComponent))]
internal static class ScaledCloudDataModelComponentPatches
{
    [HarmonyPatch(nameof(ScaledCloudDataModelComponent.Initialize))]
    [HarmonyPrefix]
    public static void UpdateCoreCelestialBodyData(ScaledCloudDataModelComponent __instance)
    {
        var origData = __instance.GetComponent<CoreCelestialBodyData>();
        var name = origData.Data.bodyName;
        var newData =  GameManager.Instance.Game.CelestialBodies.Get(name);
        origData.core = newData;
    }

    [HarmonyPatch(nameof(ScaledCloudDataModelComponent.CreateScaledCloudLayers))]
    [HarmonyPrefix]
    public static void CreateScaledCloudLayers(ScaledCloudDataModelComponent __instance)
    {
        LogInfo($"Loaded a scaled cloud configuration for {__instance.PlanetName}");
        LogInfo("It has the following layers");
        var model = __instance.ScaledCloudConfiguration;
        foreach (var layer in model.scaledCloudLayers)
        {
            LogInfo(layer.CloudLayerName);
            LogInfo($"- Radius: {layer.Radius}");
        }

        if (OverrideManager.ScaledCloudOverrides.TryGetValue(__instance.PlanetName.ToLowerInvariant(), out var scaledCloudOverride))
        {
            scaledCloudOverride.ApplyTo(model);
            LogInfo("Applied override to the config");
            LogInfo("New layers:");
            foreach (var layer in model.scaledCloudLayers)
            {
                LogInfo(layer.CloudLayerName);
                LogInfo($"- Radius: {layer.Radius}");
            }
        }
    }
}