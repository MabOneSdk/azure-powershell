namespace Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101
{
    using Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Runtime.PowerShell;

    /// <summary>Azure backup recoveryPoint resource</summary>
    [System.ComponentModel.TypeConverter(typeof(AzureBackupRecoveryPointResourceTypeConverter))]
    public partial class AzureBackupRecoveryPointResource
    {

        /// <summary>
        /// <c>AfterDeserializeDictionary</c> will be called after the deserialization has finished, allowing customization of the
        /// object before it is returned. Implement this method in a partial class to enable this behavior
        /// </summary>
        /// <param name="content">The global::System.Collections.IDictionary content that should be used.</param>

        partial void AfterDeserializeDictionary(global::System.Collections.IDictionary content);

        /// <summary>
        /// <c>AfterDeserializePSObject</c> will be called after the deserialization has finished, allowing customization of the object
        /// before it is returned. Implement this method in a partial class to enable this behavior
        /// </summary>
        /// <param name="content">The global::System.Management.Automation.PSObject content that should be used.</param>

        partial void AfterDeserializePSObject(global::System.Management.Automation.PSObject content);

        /// <summary>
        /// <c>BeforeDeserializeDictionary</c> will be called before the deserialization has commenced, allowing complete customization
        /// of the object before it is deserialized.
        /// If you wish to disable the default deserialization entirely, return <c>true</c> in the <see "returnNow" /> output parameter.
        /// Implement this method in a partial class to enable this behavior.
        /// </summary>
        /// <param name="content">The global::System.Collections.IDictionary content that should be used.</param>
        /// <param name="returnNow">Determines if the rest of the serialization should be processed, or if the method should return
        /// instantly.</param>

        partial void BeforeDeserializeDictionary(global::System.Collections.IDictionary content, ref bool returnNow);

        /// <summary>
        /// <c>BeforeDeserializePSObject</c> will be called before the deserialization has commenced, allowing complete customization
        /// of the object before it is deserialized.
        /// If you wish to disable the default deserialization entirely, return <c>true</c> in the <see "returnNow" /> output parameter.
        /// Implement this method in a partial class to enable this behavior.
        /// </summary>
        /// <param name="content">The global::System.Management.Automation.PSObject content that should be used.</param>
        /// <param name="returnNow">Determines if the rest of the serialization should be processed, or if the method should return
        /// instantly.</param>

        partial void BeforeDeserializePSObject(global::System.Management.Automation.PSObject content, ref bool returnNow);

        /// <summary>
        /// Deserializes a <see cref="global::System.Collections.IDictionary" /> into a new instance of <see cref="Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.AzureBackupRecoveryPointResource"
        /// />.
        /// </summary>
        /// <param name="content">The global::System.Collections.IDictionary content that should be used.</param>
        internal AzureBackupRecoveryPointResource(global::System.Collections.IDictionary content)
        {
            bool returnNow = false;
            BeforeDeserializeDictionary(content, ref returnNow);
            if (returnNow)
            {
                return;
            }
            // actually deserialize
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupRecoveryPointResourceInternal)this).Property = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupRecoveryPoint) content.GetValueForProperty("Property",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupRecoveryPointResourceInternal)this).Property, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.AzureBackupRecoveryPointTypeConverter.ConvertFrom);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Name = (string) content.GetValueForProperty("Name",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Name, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Type = (string) content.GetValueForProperty("Type",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Type, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Id = (string) content.GetValueForProperty("Id",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Id, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).SystemData = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.ISystemData) content.GetValueForProperty("SystemData",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).SystemData, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.SystemDataTypeConverter.ConvertFrom);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupRecoveryPointResourceInternal)this).ObjectType = (string) content.GetValueForProperty("ObjectType",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupRecoveryPointResourceInternal)this).ObjectType, global::System.Convert.ToString);
            AfterDeserializeDictionary(content);
        }

        /// <summary>
        /// Deserializes a <see cref="global::System.Management.Automation.PSObject" /> into a new instance of <see cref="Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.AzureBackupRecoveryPointResource"
        /// />.
        /// </summary>
        /// <param name="content">The global::System.Management.Automation.PSObject content that should be used.</param>
        internal AzureBackupRecoveryPointResource(global::System.Management.Automation.PSObject content)
        {
            bool returnNow = false;
            BeforeDeserializePSObject(content, ref returnNow);
            if (returnNow)
            {
                return;
            }
            // actually deserialize
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupRecoveryPointResourceInternal)this).Property = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupRecoveryPoint) content.GetValueForProperty("Property",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupRecoveryPointResourceInternal)this).Property, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.AzureBackupRecoveryPointTypeConverter.ConvertFrom);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Name = (string) content.GetValueForProperty("Name",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Name, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Type = (string) content.GetValueForProperty("Type",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Type, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Id = (string) content.GetValueForProperty("Id",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Id, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).SystemData = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.ISystemData) content.GetValueForProperty("SystemData",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).SystemData, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.SystemDataTypeConverter.ConvertFrom);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupRecoveryPointResourceInternal)this).ObjectType = (string) content.GetValueForProperty("ObjectType",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupRecoveryPointResourceInternal)this).ObjectType, global::System.Convert.ToString);
            AfterDeserializePSObject(content);
        }

        /// <summary>
        /// Deserializes a <see cref="global::System.Collections.IDictionary" /> into an instance of <see cref="Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.AzureBackupRecoveryPointResource"
        /// />.
        /// </summary>
        /// <param name="content">The global::System.Collections.IDictionary content that should be used.</param>
        /// <returns>
        /// an instance of <see cref="Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupRecoveryPointResource"
        /// />.
        /// </returns>
        public static Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupRecoveryPointResource DeserializeFromDictionary(global::System.Collections.IDictionary content)
        {
            return new AzureBackupRecoveryPointResource(content);
        }

        /// <summary>
        /// Deserializes a <see cref="global::System.Management.Automation.PSObject" /> into an instance of <see cref="Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.AzureBackupRecoveryPointResource"
        /// />.
        /// </summary>
        /// <param name="content">The global::System.Management.Automation.PSObject content that should be used.</param>
        /// <returns>
        /// an instance of <see cref="Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupRecoveryPointResource"
        /// />.
        /// </returns>
        public static Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupRecoveryPointResource DeserializeFromPSObject(global::System.Management.Automation.PSObject content)
        {
            return new AzureBackupRecoveryPointResource(content);
        }

        /// <summary>
        /// Creates a new instance of <see cref="AzureBackupRecoveryPointResource" />, deserializing the content from a json string.
        /// </summary>
        /// <param name="jsonText">a string containing a JSON serialized instance of this model.</param>
        /// <returns>an instance of the <see cref="className" /> model class.</returns>
        public static Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupRecoveryPointResource FromJsonString(string jsonText) => FromJson(Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Runtime.Json.JsonNode.Parse(jsonText));

        /// <summary>Serializes this instance to a json string.</summary>

        /// <returns>a <see cref="System.String" /> containing this model serialized to JSON text.</returns>
        public string ToJsonString() => ToJson(null, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Runtime.SerializationMode.IncludeAll)?.ToString();
    }
    /// Azure backup recoveryPoint resource
    [System.ComponentModel.TypeConverter(typeof(AzureBackupRecoveryPointResourceTypeConverter))]
    public partial interface IAzureBackupRecoveryPointResource

    {

    }
}