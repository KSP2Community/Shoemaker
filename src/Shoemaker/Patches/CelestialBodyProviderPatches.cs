using HarmonyLib;
using JetBrains.Annotations;
using KSP.Game;
using KSP.Sim.Definitions;
using Premonition.Core.Attributes;

namespace Shoemaker.Patches;

[PremonitionAssembly("Assembly-CSharp")]
[PremonitionType("KSP.Game.CelestialBodyProvider")]
internal static class CelestialBodyProviderPatches
{
    
    [PremonitionMethod("RegisterBodyFromData")]
    [PremonitionPrefix]
    [UsedImplicitly]
    public static void RegisterBodyFromData(CelestialBodyProvider __instance, CelestialBodyCore jsonData)
    {
        LogInfo($"[CelestialBodyProvider] Loading {jsonData.data.bodyName}");
    }
}