using HarmonyLib;
using KSP.Game;
using KSP.Rendering;
using KSP.Rendering.Planets;

namespace Shoemaker.Patches;

[HarmonyPatch(typeof(AtmosphereScatterManager))]
internal static class AtmosphereScatterManagerPatches
{
    [HarmonyPatch(nameof(AtmosphereScatterManager.OnAtmosphereModelLoaded))]
    [HarmonyPrefix]
    internal static void OnAtmosphereModelLoaded(AtmosphereScatterManager __instance, AtmosphereModel model)
    {
        if (model == null)
            return;
        LogInfo($"The atmosphere model for {model.PlanetName} has been loaded");
        LogInfo($"Bottom radius: {model.BottomRadius} km");
        LogInfo($"Height: {model.AtmosphereHeight} km");
        LogInfo($"Absorption Height min-max: {model.AbsorptionHeightMinMax.x} - {model.AbsorptionHeightMinMax.y}");
        if (OverrideManager.AtmosphereOverrides.TryGetValue(model.PlanetName, out var @override))
        {
            @override.ApplyTo(model);
            LogInfo($"After overrides applied: ");
            LogInfo($"Bottom radius: {model.BottomRadius} km");
            LogInfo($"Height: {model.AtmosphereHeight} km");
            LogInfo($"Absorption Height Min max: {model.AbsorptionHeightMinMax.x} - {model.AbsorptionHeightMinMax.y}");
        }
    }
}