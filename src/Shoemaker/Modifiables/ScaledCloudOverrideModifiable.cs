using PatchManager.SassyPatching;
using PatchManager.SassyPatching.Modifiables;
using Shoemaker.Selectables;

namespace Shoemaker.Modifiables;

public class ScaledCloudOverrideModifiable : JTokenModifiable
{
    private readonly ScaledCloudOverrideSelectable _scaledCloudOverrideSelectable;

    /// <summary>
    /// Creates a new <see cref="GalaxyModifiable"/> for the given <see cref="GalaxyModifiable"/>.
    /// </summary>
    /// <param name="selectable">The selectable to modify.</param>
    public ScaledCloudOverrideModifiable(ScaledCloudOverrideSelectable selectable) : base(selectable.ScaledCloudOverrideObject, selectable.SetModified) => _scaledCloudOverrideSelectable = selectable;

    /// <inheritdoc/>
    public override void Set(DataValue dataValue)
    {
        if (dataValue.IsDeletion)
        {
            _scaledCloudOverrideSelectable.SetDeleted();
            return;
        }
        base.Set(dataValue);
    }
}