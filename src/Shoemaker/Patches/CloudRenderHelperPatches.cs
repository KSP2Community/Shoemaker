using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using KSP.VolumeCloud;
using Premonition.Core.Attributes;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Shoemaker.Patches;

[PremonitionAssembly("Assembly-CSharp")]
[PremonitionType("KSP.VolumeCloud.CloudRenderHelper")]
public static class CloudRenderHelperPatches
{
    // [HarmonyPatch(typeof(CloudRenderHelper))]
    // [HarmonyPatch(nameof(CloudRenderHelper.LoadingConfigCompleted))]
    // [HarmonyTranspiler]
    // public static IEnumerable<CodeInstruction> ModifyFunction(IEnumerable<CodeInstruction> instructions)
    // {
    //     yield return new CodeInstruction(OpCodes.Ldarg_1);
    //     yield return new CodeInstruction(OpCodes.Call,typeof(CloudRenderHelper).GetMethod(nameof(UpdateConfiguration),BindingFlags.Public | BindingFlags.Static));
    //     foreach (var instruction in instructions)
    //     {
    //         yield return instruction;
    //     }
    // }

    [PremonitionMethod("LoadingConfigCompleted")]
    [PremonitionPrefix]
    [UsedImplicitly]
    public static void UpdateConfiguration(AsyncOperationHandle<VolumeCloudConfiguration> handle)
    {
        if (handle.Status != AsyncOperationStatus.Succeeded) return;
        var bodyName = handle.Result.bodyName;
        if (!OverrideManager.VolumeCloudOverrides.TryGetValue(bodyName.ToLowerInvariant(),
                out var volumeCloudConfigurationOverride)) return;
        LogInfo($"Found a configuration override for {bodyName}");
        volumeCloudConfigurationOverride.ApplyTo(handle.Result);
    }
}