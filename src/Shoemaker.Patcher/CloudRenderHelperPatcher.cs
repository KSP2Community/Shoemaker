using BepInEx;
using JetBrains.Annotations;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Shoemaker.Patcher;

[UsedImplicitly]
public class CloudRenderHelperPatcher
{
    [UsedImplicitly]
    public static IEnumerable<string> TargetDLLs => new[] { "Assembly-CSharp.dll"};

    [UsedImplicitly]
    public static void Patch(ref AssemblyDefinition asm)
    {
        var dir = new DirectoryInfo(Paths.PluginPath);
        var shoemakerAssembly =
            AssemblyDefinition.ReadAssembly(dir.EnumerateFiles("Shoemaker.dll", SearchOption.AllDirectories).First().FullName);
        PatchCloudRenderHelper(ref asm, shoemakerAssembly);
        PatchCelestialBodyProvider(ref asm, shoemakerAssembly);
        PrefixMap3DView(ref asm, shoemakerAssembly);
        // asm.Write("TestOutput.dll");
    }

    private static void PatchCloudRenderHelper(ref AssemblyDefinition asm, AssemblyDefinition shoemakerAssembly)
    {
        var firstTargetType = asm.MainModule.Types.First(t => t.Name == "CloudRenderHelper");
        var targetMethod = firstTargetType.Methods.First(t => t.Name == "LoadingConfigCompleted");
        var prefixMethod = shoemakerAssembly.MainModule.Types.First(t => t.Name == "CloudRenderHelperPatches").Methods
            .First(m => m.Name == "UpdateConfiguration");
        var imported = targetMethod.Module.ImportReference(prefixMethod);
        targetMethod.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, imported));
        targetMethod.Body.Instructions.Insert(0,Instruction.Create(OpCodes.Ldarg_1));
    }

    private static void PatchCelestialBodyProvider(ref AssemblyDefinition asm, AssemblyDefinition shoemakerAssembly)
    {
        var firstTargetType = asm.MainModule.Types.First(t => t.Name == "CelestialBodyProvider");
        var targetMethod = firstTargetType.Methods.First(t => t.Name == "RegisterBodyFromData");
        var replaceMethod = shoemakerAssembly.MainModule.Types.First(t => t.Name == "CelestialBodyProviderPatches")
            .Methods.First(m => m.Name == "RegisterBodyFromData");
        var imported = targetMethod.Module.ImportReference(replaceMethod);
        targetMethod.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, imported));
        targetMethod.Body.Instructions.Insert(0,Instruction.Create(OpCodes.Ldarg_1));
        targetMethod.Body.Instructions.Insert(0,Instruction.Create(OpCodes.Ldarg_0));
    }

    private static void PrefixMap3DView(ref AssemblyDefinition asm, AssemblyDefinition shoemakerAssembly)
    {
        var firstTargetType = asm.MainModule.Types.First(t => t.Name == "Map3DView");
        var targetMethod = firstTargetType.Methods.First(t => t.Name == "OnMapScaledSpaceCelestialBodyInstantiated");
        var replaceMethod = shoemakerAssembly.MainModule.Types.First(t => t.Name == "Map3DViewPatches")
            .Methods.First(m => m.Name == "OnMapScaledSpaceCelestialBodyInstantiated");
        var imported = targetMethod.Module.ImportReference(replaceMethod);
        targetMethod.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, imported));
        targetMethod.Body.Instructions.Insert(0,Instruction.Create(OpCodes.Ldarg_1));
    }
}