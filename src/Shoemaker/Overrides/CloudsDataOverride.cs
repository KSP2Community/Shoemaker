// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

using JetBrains.Annotations;
using KSP.VolumeCloud;
using UnityEngine;

namespace Shoemaker.Overrides;

[PublicAPI]
public class CloudsDataOverride : IOverride<VolumeCloudConfiguration.CloudsData>
{
    public string layerName;
    public bool? isEnable;
    public bool? castShadow;
    public float? bakeCloudMipmap;
    public float? currentBakedCloudMipMap;
    public VolumeCloudConfiguration.CloudsLayerType? cloudsType;
    public Vector2? cloudHeightRange;
    public float? bakedCloudHeight;
    public Vector3? cloudsLayerRotate;
    public bool? enableWind;
    public Vector2? windDirection;
    public float? movementSpeed;
    public float? evolveSpeed;
    public float? topOffset;
    public bool? isFold;
    
    public void ApplyTo(VolumeCloudConfiguration.CloudsData obj)
    {
        isEnable.Apply(ref obj.isEnable);
        castShadow.Apply(ref obj.castShadow);
        bakeCloudMipmap.Apply(ref obj.bakeCloudMipmap);
        currentBakedCloudMipMap.Apply(ref obj.currentBakedCloudMipmap);
        cloudsType.Apply(ref obj.cloudsType);
        cloudHeightRange.Apply(ref obj.cloudHeightRange);
        bakedCloudHeight.Apply(ref obj.bakedCloudHeight);
        cloudsLayerRotate.Apply(ref obj.cloudsLayerRotate);
        enableWind.Apply(ref obj.enableWind);
        windDirection.Apply(ref obj.windDirection);
        movementSpeed.Apply(ref obj.movementSpeed);
        evolveSpeed.Apply(ref obj.evolveSpeed);
        topOffset.Apply(ref obj.topOffset);
        isFold.Apply(ref obj.isFold);
    }
}