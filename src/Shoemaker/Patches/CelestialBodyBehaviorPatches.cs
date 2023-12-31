﻿using HarmonyLib;
using KSP;
using KSP.Game;
using KSP.Sim.impl;
using UnityEngine;
using Object = System.Object;

namespace Shoemaker.Patches;

[HarmonyPatch(typeof(CelestialBodyBehavior))]
internal static class CelestialBodyBehaviourPatches
{
    [HarmonyPatch(nameof(CelestialBodyBehavior.OnScaledSpaceViewInstantiated))]
    [HarmonyPrefix]
    internal static void MergeData(CelestialBodyBehavior __instance, GameObject instance)
    {
        var data = instance.GetComponent<CoreCelestialBodyData>();
        var name = data.Data.bodyName;
        var newData =  GameManager.Instance.Game.CelestialBodies.Get(name);
        data.core = newData;
        // var name = __instance.CelestialBodyData.Data.bodyName;
        // var newData = GameManager.Instance.Game.CelestialBodies.Get(name);
        // __instance._coreCelestialBodyData.core = newData;
        
        LogInfo(
            $"Following info is from the scaled space load of {data.Data.bodyName}");
        LogInfo("The scaled space object has the following components:\n");
        foreach (var component in instance.GetComponents(typeof(Object)))
        {
            LogInfo($"- {component.GetType()}");
        }
    }
    
    [HarmonyPatch(nameof(CelestialBodyBehavior.OnLocalSpaceViewInstantiated))]
    [HarmonyPrefix]
    internal static void LogStuff(CelestialBodyBehavior __instance, GameObject obj)
    {
        LogInfo(
            $"Following info is from the local space load of {__instance.CelestialBodyData.Data.bodyName}");
        LogInfo($"The celestial body has a radius of {__instance.CelestialBodyData.Data.radius}");
        LogInfo("The local space object has the following components:\n");
        foreach (var component in obj.GetComponents(typeof(Object)))
        {
            LogInfo($"- {component.GetType()}");
        }
        
        LogInfo($"The local space has the following prefabs\n");
        if (obj.TryGetComponent(out NestedPrefabSpawner nestedPrefabSpawner))
        {
            foreach (var prefab in nestedPrefabSpawner.Prefabs)
            {
                LogInfo($"- {prefab.PrefabAssetReference.AssetGUID}");
                if (prefab.tgtTransform != null)
                {
                    LogInfo($"\t- {prefab.tgtTransform.name}");
                    var operation = prefab.PrefabAssetReference.InstantiateAsync();
                    operation.Completed += x =>
                    {
                        LogInfo($"From previous async handle for {prefab.PrefabAssetReference.AssetGUID}");
                        var surfaceObject = x.Result.GetComponent<PQSSurfaceObject>();
                        LogInfo($"Latitude - {surfaceObject.Latitude}");
                        LogInfo($"Longitude - {surfaceObject.Longitude}");
                        LogInfo($"Radial Offset - {surfaceObject.RadialOffset}");
                        LogInfo($"Degrees around radial normal - {surfaceObject.DegreesAroundRadialNormal}");
                        x.Result.DestroyGameObject();
                    };
                }
            }
        }
    }
}