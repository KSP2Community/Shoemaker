using BepInEx;
using HarmonyLib;
using JetBrains.Annotations;
using KSP.Game;
using Newtonsoft.Json;
using PatchManager;
using Shoemaker.Overrides;
using Shoemaker.Utility;
using SpaceWarp;
using SpaceWarp.API.Mods;
using SpaceWarp.API.Loading;
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

    private static void RegisterScaledCloudOverride(TextAsset scaledCloudOverride)
    {
        LogInfo($"Loading scaled cloud override: {scaledCloudOverride.name}");
        var scaledCloud = JsonConvert.DeserializeObject<ScaledCloudOverride>(scaledCloudOverride.text);
        OverrideManager.ScaledCloudOverrides[scaledCloud.PlanetName] = scaledCloud;
    }

    /// <summary>
    /// Runs when the mod is first initialized.
    /// </summary>
    public override void OnInitialized()
    {
        GameManager.Instance.Assets.LoadByLabel("atmosphere_overrides", RegisterAtmosphereOverride,
            delegate(IList<TextAsset> assetLocations) { Addressables.Release(assetLocations); });
        GameManager.Instance.Assets.LoadByLabel("scaled_cloud_overrides", RegisterScaledCloudOverride,
            delegate(IList<TextAsset> assetLocations) { Addressables.Release(assetLocations); });
        base.OnInitialized();
    }
}