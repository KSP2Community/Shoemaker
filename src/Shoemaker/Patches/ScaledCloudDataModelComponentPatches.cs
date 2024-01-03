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
}