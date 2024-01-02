using JetBrains.Annotations;
using KSP.Game;
using UnityEngine;

namespace Shoemaker.Overrides;

[UsedImplicitly]
[PublicAPI]
public struct LayerOverride
{
    public bool? UseScaleCloudsOnly;
    public bool? IsEnable;
    public float? Radius;
    [CanBeNull] public string CloudsTexture;
    [CanBeNull] public string CloudsNormalTexture;
    [CanBeNull] public string CloudsDetailMask;
    [CanBeNull] public string CloudsDetailMaskNormal;
    public float? CloudsDetailMaskNormalTileRate;
    public float? NormalFactor;
    public Quaternion? Rotation;
    public bool? EnableWind;
    public Vector3? WindDirection;
    public float? MovementSpeed;
    public bool? EnableColorTexture;
    public float? CloudColorIntensity;
    [CanBeNull] public string CloudColorTexture;
    [CanBeNull] public string CloudDetailTexture;
    public float? CloudDetailTile;
    public float? CloudDetailStrength;
    public Color? ScaleCloudColor;

    public void ApplyTo(ScaledCloudConfiguration.scaledCloudMaterialData scaledCloudMaterialData)
    {
        if (UseScaleCloudsOnly.HasValue) scaledCloudMaterialData.UseScaleCloudsOnly = UseScaleCloudsOnly.Value;
        if (IsEnable.HasValue) scaledCloudMaterialData.IsEnable = IsEnable.Value;
        if (Radius.HasValue) scaledCloudMaterialData.Radius = Radius.Value;
        if (!string.IsNullOrEmpty(CloudsTexture))
        {
            GameManager.Instance.Assets.TryLoad<Cubemap>(CloudsTexture,
                asset => scaledCloudMaterialData.CloudsTexture = asset);
        }
        if (!string.IsNullOrEmpty(CloudsNormalTexture))
        {
            GameManager.Instance.Assets.TryLoad<Cubemap>(CloudsNormalTexture,
                asset => scaledCloudMaterialData.CloudsNormalTexture = asset);
        }
        if (!string.IsNullOrEmpty(CloudsDetailMask))
        {
            GameManager.Instance.Assets.TryLoad<Texture2D>(CloudsDetailMask,
                asset => scaledCloudMaterialData.CloudsDetialMask = asset);
        }
        if (!string.IsNullOrEmpty(CloudsDetailMaskNormal))
        {
            GameManager.Instance.Assets.TryLoad<Texture2D>(CloudsDetailMaskNormal,
                asset => scaledCloudMaterialData.CloudsDetailMaskNormal = asset);
        }
        if (CloudsDetailMaskNormalTileRate.HasValue)
            scaledCloudMaterialData.CloudsDetailMaskNormalTileRate = CloudsDetailMaskNormalTileRate.Value;
        if (NormalFactor.HasValue) scaledCloudMaterialData.NormalFactor = NormalFactor.Value;
        if (Rotation.HasValue) scaledCloudMaterialData.Rotation = Rotation.Value;
        if (EnableWind.HasValue) scaledCloudMaterialData.EnableWind = EnableWind.Value;
        if (WindDirection.HasValue) scaledCloudMaterialData.WindDirection = WindDirection.Value;
        if (MovementSpeed.HasValue) scaledCloudMaterialData.MovmentSpeed = MovementSpeed.Value;
        if (EnableColorTexture.HasValue) scaledCloudMaterialData.EnableColorTexture = EnableColorTexture.Value;
        if (CloudColorIntensity.HasValue) scaledCloudMaterialData.CloudColorIntensity = CloudColorIntensity.Value;
        if (!string.IsNullOrEmpty(CloudColorTexture))
        {
            GameManager.Instance.Assets.TryLoad<Cubemap>(CloudColorTexture,
                asset => scaledCloudMaterialData.CloudColorTexture = asset);
        }
        if (!string.IsNullOrEmpty(CloudDetailTexture))
        {
            GameManager.Instance.Assets.TryLoad<Texture3D>(CloudDetailTexture,
                asset => scaledCloudMaterialData.CloudDetailTexture = asset);
        }
        if (CloudDetailTile.HasValue) scaledCloudMaterialData.CloudDetailTile = CloudDetailTile.Value;
        if (CloudDetailStrength.HasValue) scaledCloudMaterialData.CloudDetailStrenth = CloudDetailStrength.Value;
        if (ScaleCloudColor.HasValue) scaledCloudMaterialData.ScaleCloudColor = ScaleCloudColor.Value;
    }
}