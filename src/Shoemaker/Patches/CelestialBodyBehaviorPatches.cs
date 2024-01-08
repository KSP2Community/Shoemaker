using HarmonyLib;
using KSP;
using KSP.Game;
using KSP.Rendering.Planets;
using KSP.Sim.impl;
using UnityEngine;

namespace Shoemaker.Patches;

[HarmonyPatch(typeof(CelestialBodyBehavior))]
internal static class CelestialBodyBehaviourPatches
{
    private const string Ksp2UnityToolsLocalPath = "KSP2/Planets/Local";
    private const string Ksp2UnityToolsScaledPath = "KSP2/Planets/Scaled";
    private const string Ksp2LocalPath = "KSP2/Environment/CelestialBody/CelestialBody_Local";
    private const string Ksp2ScaledPath = "KSP2/Environment/CelestialBody/CelestialBody_Scaled";
    
    [HarmonyPatch(nameof(CelestialBodyBehavior.OnScaledSpaceViewInstantiated))]
    [HarmonyPrefix]
    internal static void MergeData(CelestialBodyBehavior __instance, GameObject instance)
    {
        foreach (var renderer in instance.GetComponents<MeshRenderer>())
        {
            if (renderer.material.shader.name != Ksp2UnityToolsScaledPath) continue;
            var material = renderer.material;
            var mat = new Material(Shader.Find(Ksp2ScaledPath))
            {
                name = material.name
            };
            mat.CopyPropertiesFromMaterial(material);
            renderer.material = mat;
        }
        
        var data = instance.GetComponent<CoreCelestialBodyData>();
        var oldRadius = data.Data.radius;
        var name = data.Data.bodyName;
        var newData =  GameManager.Instance.Game.CelestialBodies.Get(name);
        var newRadius = newData.data.radius;
        OverrideManager.Scales.TryAdd(name.ToLowerInvariant(), newRadius / oldRadius);
        data.core = newData;
    }

    [HarmonyPatch(nameof(CelestialBodyBehavior.OnLocalSpaceViewInstantiated))]
    [HarmonyPrefix]
    internal static void ChangeMaterials(CelestialBodyBehavior __instance, GameObject obj)
    {
        foreach (var pqs in obj.GetComponents<PQS>())
        {
            var data = pqs.data;
            if (data.materialSettings.surfaceMaterial.shader.name == Ksp2UnityToolsLocalPath)
            {
                var material = data.materialSettings.surfaceMaterial;
                var mat = new Material(Shader.Find(Ksp2LocalPath))
                {
                    name = material.name
                };
                mat.CopyPropertiesFromMaterial(material);
                data.materialSettings.surfaceMaterial = mat;
            }
            
            if (data.materialSettings.scaledSpaceMaterial.shader.name == Ksp2UnityToolsScaledPath)
            {
                var material = data.materialSettings.scaledSpaceMaterial;
                var mat = new Material(Shader.Find(Ksp2ScaledPath))
                {
                    name = material.name
                };
                mat.CopyPropertiesFromMaterial(material);
                data.materialSettings.scaledSpaceMaterial = mat;
            }
        }
    }
    /*
    [HarmonyPatch(nameof(CelestialBodyBehavior.OnLocalSpaceViewInstantiated))]
    [HarmonyPrefix]
    internal static void LogStuff(CelestialBodyBehavior __instance, GameObject obj)
    {
        LogInfo(
            $"Following info is from the local space load of {__instance.CelestialBodyData.Data.bodyName}");
        LogInfo($"The celestial body has a radius of {__instance.CelestialBodyData.Data.radius}");
        LogInfo("The local space object has the following components:\n");
        foreach (var component in obj.GetComponents(typeof(UnityObject)))
        {
            LogInfo($"- {component.GetType()}");
        }
        
        // LogInfo($"The local space has the following prefabs\n");
        // if (obj.TryGetComponent(out NestedPrefabSpawner nestedPrefabSpawner))
        // {
        //     foreach (var prefab in nestedPrefabSpawner.Prefabs)
        //     {
        //         LogInfo($"- {prefab.PrefabAssetReference.AssetGUID}");
        //         if (prefab.tgtTransform != null)
        //         {
        //             LogInfo($"\t- {prefab.tgtTransform.name}");
        //             var operation = prefab.PrefabAssetReference.InstantiateAsync();
        //             operation.Completed += x =>
        //             {
        //                 LogInfo($"From previous async handle for {prefab.PrefabAssetReference.AssetGUID}");
        //                 var surfaceObject = x.Result.GetComponent<PQSSurfaceObject>();
        //                 LogInfo($"Latitude - {surfaceObject.Latitude}");
        //                 LogInfo($"Longitude - {surfaceObject.Longitude}");
        //                 LogInfo($"Radial Offset - {surfaceObject.RadialOffset}");
        //                 LogInfo($"Degrees around radial normal - {surfaceObject.DegreesAroundRadialNormal}");
        //                 x.Result.DestroyGameObject();
        //             };
        //         }
        //     }
        // }
        // var scale = OverrideManager.Scales[__instance.CelestialBodyData.Data.bodyName];
        
        // obj.transform.localScale *= (float)scale;
    }
    */
}