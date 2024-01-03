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
        var firstTargetType = asm.MainModule.Types.First(t => t.Name == "CloudRenderHelper");
        var targetMethod = firstTargetType.Methods.First(t => t.Name == "LoadingConfigCompleted");
        var dir = new DirectoryInfo(Paths.PluginPath);
        var shoemakerAssembly =
            AssemblyDefinition.ReadAssembly(dir.EnumerateFiles("Shoemaker.dll", SearchOption.AllDirectories).First().FullName);
        var prefixMethod = shoemakerAssembly.MainModule.Types.First(t => t.Name == "CloudRenderHelperPatches").Methods
            .First(m => m.Name == "UpdateConfiguration");
        var imported = targetMethod.Module.ImportReference(prefixMethod);
        targetMethod.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, imported));
        targetMethod.Body.Instructions.Insert(0,Instruction.Create(OpCodes.Ldarg_1));

        // asm.Write("TestOutput.dll");
    }
}