namespace Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202001Alpha
{
    using static Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Runtime.Extensions;

    /// <summary>Resource tags.</summary>
    public partial class DppTrackedResourceTags :
        Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202001Alpha.IDppTrackedResourceTags,
        Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202001Alpha.IDppTrackedResourceTagsInternal
    {

        /// <summary>Creates an new <see cref="DppTrackedResourceTags" /> instance.</summary>
        public DppTrackedResourceTags()
        {

        }
    }
    /// Resource tags.
    public partial interface IDppTrackedResourceTags :
        Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Runtime.IJsonSerializable,
        Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Runtime.IAssociativeArray<string>
    {

    }
    /// Resource tags.
    internal partial interface IDppTrackedResourceTagsInternal

    {

    }
}