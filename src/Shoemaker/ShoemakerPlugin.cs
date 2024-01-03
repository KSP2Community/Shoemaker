using BepInEx;
using HarmonyLib;
using JetBrains.Annotations;
using KSP.Game;
using KSP.IO;
using Newtonsoft.Json;
using PatchManager;
using Shoemaker.Overrides;
using SpaceWarp;
using SpaceWarp.API.Mods;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Shoemaker;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]
[BepInDependency(PatchManagerPlugin.ModGuid, PatchManagerPlugin.ModVer)]
public class ShoemakerPlugin : BaseSpaceWarpPlugin
{
    // Useful in case some other mod wants to use this mod a dependency
    [PublicAPI] public const string ModGuid = MyPluginInfo.PLUGIN_GUID;
    [PublicAPI] public const string ModName = MyPluginInfo.PLUGIN_NAME;
    [PublicAPI] public const string ModVer = MyPluginInfo.PLUGIN_VERSION;

    // Singleton instance of the plugin class
    [PublicAPI] public static ShoemakerPlugin Instance { get; set; }

    public void Awake()
    {
        Instance = this;
        Harmony.CreateAndPatchAll(GetType().Assembly);
    }

    private static void RegisterAtmosphereOverride(TextAsset atmosphereOverride)
    {
        LogInfo($"Loading atmosphere override: {atmosphereOverride.name}");
        var atmosphere = JsonConvert.DeserializeObject<AtmosphereOverride>(atmosphereOverride.text);
        OverrideManager.AtmosphereOverrides[atmosphere.PlanetName] = atmosphere;
    }

    private static void RegisterVolumeCloudOverride(TextAsset volumeCloudOverride)
    {
        LogInfo($"Loading volume cloud override: {volumeCloudOverride.name}");
        var volumeCloud = IOProvider.FromJson<VolumeCloudConfigurationOverride>(volumeCloudOverride.text);
        OverrideManager.VolumeCloudOverrides[volumeCloud.bodyName.ToLowerInvariant()] = volumeCloud;
    }
    
    /// <summary>
    /// Runs when the mod is first initialized.
    /// </summary>
    public override void OnInitialized()
    {
        GameManager.Instance.Assets.LoadByLabel("atmosphere_overrides", RegisterAtmosphereOverride,
            delegate(IList<TextAsset> assetLocations) { Addressables.Release(assetLocations); });
        GameManager.Instance.Assets.LoadByLabel("volume_cloud_overrides", RegisterVolumeCloudOverride, 
            delegate(IList<TextAsset> assetLocations) { Addressables.Release(assetLocations); });
        base.OnInitialized();
    }
}