using HarmonyLib;
using JetBrains.Annotations;
using KSP.Game;
using KSP.Sim.Definitions;
using PatchManager.Shared;

namespace Shoemaker.Patches;

internal static class CelestialBodyProviderPatches
{
    [UsedImplicitly]
    public static void RegisterBodyFromData(CelestialBodyProvider __instance, CelestialBodyCore jsonData)
    {
        Logging.LogInfo($"[CelestialBodyProvider] Loading {jsonData.data.bodyName}");
    }
}