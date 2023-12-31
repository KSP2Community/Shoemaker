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
        var newData = GameManager.Instance.Game.CelestialBodies.Get(model.PlanetName) ??
                      GameManager.Instance.Game.CelestialBodies.Get(char.ToUpper(model.PlanetName[0]) +
                                                                    model.PlanetName[1..]);
        var origRadius = model.BottomRadius;
        model.BottomRadius = (float)(newData.data.radius / 1000.0);
        // this will slightly expand out kerbins clouds as well
        model.AtmosphereHeight = (float)(newData.data.atmosphereDepth / 1000.0);
        model.AbsorptionHeightMinMax *= model.BottomRadius/origRadius;
    }
}