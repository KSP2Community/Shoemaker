using JetBrains.Annotations;
using KSP.Game;
using UnityEngine;

namespace Shoemaker.Overrides;

[PublicAPI]
[UsedImplicitly]
public class ScaledCloudOverride
{

    public string PlanetName;
    // ReSharper disable file InconsistentNaming
    public float? brightness;
    [CanBeNull] public string cloudsNormalTexture;
    public float? diffuseFactor;
    public float? ambientIntensity;
    public float? AmbientTint;
    public float? ambientFalloff;
    public float? sunsetStart;
    public float? sunsetEnd;
    public Color? sunsetColor;
    public bool? UseScaleCloudsOnly;
    public float? ShadowDensity;
    public float? ShadowOpacity;
    public float? DepthOffset;
    public Dictionary<string, LayerOverride> LayerOverrides = new();

    public void ApplyTo(ScaledCloudConfiguration scaledCloudConfiguration)
    {
        if (brightness.HasValue) scaledCloudConfiguration.brightness = brightness.Value;
        if (!string.IsNullOrEmpty(cloudsNormalTexture))
        {
            GameManager.Instance.Assets.TryLoad<Cubemap>(cloudsNormalTexture,
                asset => scaledCloudConfiguration.cloudsNormalTexture = asset);
        }
        if (diffuseFactor.HasValue) scaledCloudConfiguration.diffuseFactor = diffuseFactor.Value;
        if (ambientIntensity.HasValue) scaledCloudConfiguration.ambientIntensity = ambientIntensity.Value;
        if (AmbientTint.HasValue) scaledCloudConfiguration.AmbientTint = AmbientTint.Value;
        if (ambientFalloff.HasValue) scaledCloudConfiguration.ambientFalloff = ambientFalloff.Value;
        if (sunsetStart.HasValue) scaledCloudConfiguration.sunsetStart = sunsetStart.Value;
        if (sunsetEnd.HasValue) scaledCloudConfiguration.sunsetEnd = sunsetEnd.Value;
        if (sunsetColor.HasValue) scaledCloudConfiguration.sunsetColor = sunsetColor.Value;
        if (UseScaleCloudsOnly.HasValue) scaledCloudConfiguration.UseScaleCloudsOnly = UseScaleCloudsOnly.Value;
        if (ShadowDensity.HasValue) scaledCloudConfiguration.ShadowDensity = ShadowDensity.Value;
        if (ShadowOpacity.HasValue) scaledCloudConfiguration.ShadowOpacity = ShadowOpacity.Value;
        if (DepthOffset.HasValue) scaledCloudConfiguration.DepthOffset = DepthOffset.Value;
        foreach (var (key, value) in LayerOverrides)
        {
            var first = scaledCloudConfiguration.scaledCloudLayers.FirstOrDefault(x => x.CloudLayerName == key);
            if (first is not null)
            {
                value.ApplyTo(first);
            }
        }
    }
}