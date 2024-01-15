using HarmonyLib;
using JetBrains.Annotations;
using KSP.Map;
using KSP.Sim.impl;
using Premonition.Core.Attributes;
using UnityEngine;

namespace Shoemaker.Patches;

[PremonitionAssembly("Assembly-CSharp")]
[PremonitionType("KSP.Map.Map3DView")]
internal static class Map3DViewPatches
{
    private const string Ksp2UnityToolsScaledPath = "KSP2/Planets/Scaled";
    private const string Ksp2ScaledPath = "KSP2/Environment/CelestialBody/CelestialBody_Scaled";
    
    [PremonitionMethod("OnMapScaledSpaceCelestialBodyInstantiated")]
    [PremonitionPrefix]
    [UsedImplicitly]
    private static void OnMapScaledSpaceCelestialBodyInstantiated(GameObject instance)
    {
        LogInfo($"Instantiated {instance.name}");
        foreach (var renderer in instance.GetComponents<MeshRenderer>())
        {
            if (renderer.material.shader.name != Ksp2UnityToolsScaledPath) continue;
            LogInfo($"Replacing a renderer: {renderer}");
            var material = renderer.material;
            var mat = new Material(Shader.Find(Ksp2ScaledPath))
            {
                name = material.name
            };
            mat.CopyPropertiesFromMaterial(material);
            renderer.material = mat;
        }
    }
}