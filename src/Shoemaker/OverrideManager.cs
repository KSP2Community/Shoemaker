using JetBrains.Annotations;
using Shoemaker.Overrides;

namespace Shoemaker;

[PublicAPI]
public static class OverrideManager
{
    public static Dictionary<string, AtmosphereOverride> AtmosphereOverrides = new();
    public static Dictionary<string, ScaledCloudOverride> ScaledCloudOverrides = new();
    public static Dictionary<string, double> Scales = new();
}