namespace Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101
{
    using Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Runtime.PowerShell;

    /// <summary>AzureBackup Job Resource Class</summary>
    [System.ComponentModel.TypeConverter(typeof(AzureBackupJobResourceTypeConverter))]
    public partial class AzureBackupJobResource
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
        /// Deserializes a <see cref="global::System.Collections.IDictionary" /> into a new instance of <see cref="Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.AzureBackupJobResource"
        /// />.
        /// </summary>
        /// <param name="content">The global::System.Collections.IDictionary content that should be used.</param>
        internal AzureBackupJobResource(global::System.Collections.IDictionary content)
        {
            bool returnNow = false;
            BeforeDeserializeDictionary(content, ref returnNow);
            if (returnNow)
            {
                return;
            }
            // actually deserialize
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).Property = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJob) content.GetValueForProperty("Property",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).Property, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.AzureBackupJobTypeConverter.ConvertFrom);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Name = (string) content.GetValueForProperty("Name",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Name, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Type = (string) content.GetValueForProperty("Type",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Type, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Id = (string) content.GetValueForProperty("Id",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Id, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).SystemData = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.ISystemData) content.GetValueForProperty("SystemData",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).SystemData, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.SystemDataTypeConverter.ConvertFrom);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfo = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IJobExtendedInfo) content.GetValueForProperty("ExtendedInfo",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfo, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.JobExtendedInfoTypeConverter.ConvertFrom);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ActivityId = (string) content.GetValueForProperty("ActivityId",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ActivityId, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).BackupInstanceFriendlyName = (string) content.GetValueForProperty("BackupInstanceFriendlyName",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).BackupInstanceFriendlyName, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).BackupInstanceId = (string) content.GetValueForProperty("BackupInstanceId",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).BackupInstanceId, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceId = (string) content.GetValueForProperty("DataSourceId",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceId, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceLocation = (string) content.GetValueForProperty("DataSourceLocation",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceLocation, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceName = (string) content.GetValueForProperty("DataSourceName",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceName, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceSetName = (string) content.GetValueForProperty("DataSourceSetName",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceSetName, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceType = (string) content.GetValueForProperty("DataSourceType",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceType, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).Duration = (string) content.GetValueForProperty("Duration",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).Duration, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).EndTime = (global::System.DateTime?) content.GetValueForProperty("EndTime",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).EndTime, (v) => v is global::System.DateTime _v ? _v : global::System.Xml.XmlConvert.ToDateTime( v.ToString() , global::System.Xml.XmlDateTimeSerializationMode.Unspecified));
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ErrorDetail = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IUserFacingError[]) content.GetValueForProperty("ErrorDetail",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ErrorDetail, __y => TypeConverterExtensions.SelectToArray<Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IUserFacingError>(__y, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.UserFacingErrorTypeConverter.ConvertFrom));
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).IsUserTriggered = (bool) content.GetValueForProperty("IsUserTriggered",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).IsUserTriggered, (__y)=> (bool) global::System.Convert.ChangeType(__y, typeof(bool)));
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).Operation = (string) content.GetValueForProperty("Operation",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).Operation, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).OperationCategory = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Support.JobOperationCategory) content.GetValueForProperty("OperationCategory",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).OperationCategory, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Support.JobOperationCategory.CreateFrom);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).PolicyId = (string) content.GetValueForProperty("PolicyId",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).PolicyId, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).PolicyName = (string) content.GetValueForProperty("PolicyName",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).PolicyName, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ProgressEnabled = (bool) content.GetValueForProperty("ProgressEnabled",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ProgressEnabled, (__y)=> (bool) global::System.Convert.ChangeType(__y, typeof(bool)));
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ProgressUrl = (string) content.GetValueForProperty("ProgressUrl",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ProgressUrl, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).RestoreType = (string) content.GetValueForProperty("RestoreType",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).RestoreType, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SourceResourceGroup = (string) content.GetValueForProperty("SourceResourceGroup",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SourceResourceGroup, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SourceSubscriptionId = (string) content.GetValueForProperty("SourceSubscriptionId",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SourceSubscriptionId, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).StartTime = (global::System.DateTime) content.GetValueForProperty("StartTime",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).StartTime, (v) => v is global::System.DateTime _v ? _v : global::System.Xml.XmlConvert.ToDateTime( v.ToString() , global::System.Xml.XmlDateTimeSerializationMode.Unspecified));
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).Status = (string) content.GetValueForProperty("Status",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).Status, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SubscriptionId = (string) content.GetValueForProperty("SubscriptionId",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SubscriptionId, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SupportedAction = (string[]) content.GetValueForProperty("SupportedAction",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SupportedAction, __y => TypeConverterExtensions.SelectToArray<string>(__y, global::System.Convert.ToString));
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).VaultName = (string) content.GetValueForProperty("VaultName",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).VaultName, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoSourceRecoverPoint = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IRestoreJobRecoveryPointDetails) content.GetValueForProperty("ExtendedInfoSourceRecoverPoint",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoSourceRecoverPoint, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.RestoreJobRecoveryPointDetailsTypeConverter.ConvertFrom);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoTargetRecoverPoint = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IRestoreJobRecoveryPointDetails) content.GetValueForProperty("ExtendedInfoTargetRecoverPoint",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoTargetRecoverPoint, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.RestoreJobRecoveryPointDetailsTypeConverter.ConvertFrom);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoAdditionalDetail = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IJobExtendedInfoAdditionalDetails) content.GetValueForProperty("ExtendedInfoAdditionalDetail",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoAdditionalDetail, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.JobExtendedInfoAdditionalDetailsTypeConverter.ConvertFrom);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoBackupInstanceState = (string) content.GetValueForProperty("ExtendedInfoBackupInstanceState",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoBackupInstanceState, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoDataTransferredInByte = (double?) content.GetValueForProperty("ExtendedInfoDataTransferredInByte",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoDataTransferredInByte, (__y)=> (double) global::System.Convert.ChangeType(__y, typeof(double)));
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoRecoveryDestination = (string) content.GetValueForProperty("ExtendedInfoRecoveryDestination",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoRecoveryDestination, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoSubTask = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IJobSubTask[]) content.GetValueForProperty("ExtendedInfoSubTask",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoSubTask, __y => TypeConverterExtensions.SelectToArray<Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IJobSubTask>(__y, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.JobSubTaskTypeConverter.ConvertFrom));
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SourceRecoverPointRecoveryPointId = (string) content.GetValueForProperty("SourceRecoverPointRecoveryPointId",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SourceRecoverPointRecoveryPointId, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SourceRecoverPointRecoveryPointTime = (global::System.DateTime?) content.GetValueForProperty("SourceRecoverPointRecoveryPointTime",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SourceRecoverPointRecoveryPointTime, (v) => v is global::System.DateTime _v ? _v : global::System.Xml.XmlConvert.ToDateTime( v.ToString() , global::System.Xml.XmlDateTimeSerializationMode.Unspecified));
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).TargetRecoverPointRecoveryPointId = (string) content.GetValueForProperty("TargetRecoverPointRecoveryPointId",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).TargetRecoverPointRecoveryPointId, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).TargetRecoverPointRecoveryPointTime = (global::System.DateTime?) content.GetValueForProperty("TargetRecoverPointRecoveryPointTime",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).TargetRecoverPointRecoveryPointTime, (v) => v is global::System.DateTime _v ? _v : global::System.Xml.XmlConvert.ToDateTime( v.ToString() , global::System.Xml.XmlDateTimeSerializationMode.Unspecified));
            AfterDeserializeDictionary(content);
        }

        /// <summary>
        /// Deserializes a <see cref="global::System.Management.Automation.PSObject" /> into a new instance of <see cref="Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.AzureBackupJobResource"
        /// />.
        /// </summary>
        /// <param name="content">The global::System.Management.Automation.PSObject content that should be used.</param>
        internal AzureBackupJobResource(global::System.Management.Automation.PSObject content)
        {
            bool returnNow = false;
            BeforeDeserializePSObject(content, ref returnNow);
            if (returnNow)
            {
                return;
            }
            // actually deserialize
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).Property = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJob) content.GetValueForProperty("Property",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).Property, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.AzureBackupJobTypeConverter.ConvertFrom);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Name = (string) content.GetValueForProperty("Name",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Name, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Type = (string) content.GetValueForProperty("Type",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Type, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Id = (string) content.GetValueForProperty("Id",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).Id, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).SystemData = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.ISystemData) content.GetValueForProperty("SystemData",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IDppResourceInternal)this).SystemData, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.SystemDataTypeConverter.ConvertFrom);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfo = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IJobExtendedInfo) content.GetValueForProperty("ExtendedInfo",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfo, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.JobExtendedInfoTypeConverter.ConvertFrom);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ActivityId = (string) content.GetValueForProperty("ActivityId",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ActivityId, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).BackupInstanceFriendlyName = (string) content.GetValueForProperty("BackupInstanceFriendlyName",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).BackupInstanceFriendlyName, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).BackupInstanceId = (string) content.GetValueForProperty("BackupInstanceId",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).BackupInstanceId, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceId = (string) content.GetValueForProperty("DataSourceId",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceId, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceLocation = (string) content.GetValueForProperty("DataSourceLocation",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceLocation, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceName = (string) content.GetValueForProperty("DataSourceName",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceName, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceSetName = (string) content.GetValueForProperty("DataSourceSetName",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceSetName, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceType = (string) content.GetValueForProperty("DataSourceType",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).DataSourceType, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).Duration = (string) content.GetValueForProperty("Duration",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).Duration, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).EndTime = (global::System.DateTime?) content.GetValueForProperty("EndTime",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).EndTime, (v) => v is global::System.DateTime _v ? _v : global::System.Xml.XmlConvert.ToDateTime( v.ToString() , global::System.Xml.XmlDateTimeSerializationMode.Unspecified));
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ErrorDetail = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IUserFacingError[]) content.GetValueForProperty("ErrorDetail",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ErrorDetail, __y => TypeConverterExtensions.SelectToArray<Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IUserFacingError>(__y, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.UserFacingErrorTypeConverter.ConvertFrom));
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).IsUserTriggered = (bool) content.GetValueForProperty("IsUserTriggered",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).IsUserTriggered, (__y)=> (bool) global::System.Convert.ChangeType(__y, typeof(bool)));
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).Operation = (string) content.GetValueForProperty("Operation",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).Operation, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).OperationCategory = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Support.JobOperationCategory) content.GetValueForProperty("OperationCategory",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).OperationCategory, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Support.JobOperationCategory.CreateFrom);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).PolicyId = (string) content.GetValueForProperty("PolicyId",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).PolicyId, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).PolicyName = (string) content.GetValueForProperty("PolicyName",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).PolicyName, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ProgressEnabled = (bool) content.GetValueForProperty("ProgressEnabled",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ProgressEnabled, (__y)=> (bool) global::System.Convert.ChangeType(__y, typeof(bool)));
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ProgressUrl = (string) content.GetValueForProperty("ProgressUrl",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ProgressUrl, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).RestoreType = (string) content.GetValueForProperty("RestoreType",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).RestoreType, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SourceResourceGroup = (string) content.GetValueForProperty("SourceResourceGroup",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SourceResourceGroup, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SourceSubscriptionId = (string) content.GetValueForProperty("SourceSubscriptionId",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SourceSubscriptionId, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).StartTime = (global::System.DateTime) content.GetValueForProperty("StartTime",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).StartTime, (v) => v is global::System.DateTime _v ? _v : global::System.Xml.XmlConvert.ToDateTime( v.ToString() , global::System.Xml.XmlDateTimeSerializationMode.Unspecified));
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).Status = (string) content.GetValueForProperty("Status",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).Status, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SubscriptionId = (string) content.GetValueForProperty("SubscriptionId",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SubscriptionId, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SupportedAction = (string[]) content.GetValueForProperty("SupportedAction",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SupportedAction, __y => TypeConverterExtensions.SelectToArray<string>(__y, global::System.Convert.ToString));
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).VaultName = (string) content.GetValueForProperty("VaultName",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).VaultName, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoSourceRecoverPoint = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IRestoreJobRecoveryPointDetails) content.GetValueForProperty("ExtendedInfoSourceRecoverPoint",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoSourceRecoverPoint, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.RestoreJobRecoveryPointDetailsTypeConverter.ConvertFrom);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoTargetRecoverPoint = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IRestoreJobRecoveryPointDetails) content.GetValueForProperty("ExtendedInfoTargetRecoverPoint",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoTargetRecoverPoint, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.RestoreJobRecoveryPointDetailsTypeConverter.ConvertFrom);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoAdditionalDetail = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IJobExtendedInfoAdditionalDetails) content.GetValueForProperty("ExtendedInfoAdditionalDetail",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoAdditionalDetail, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.JobExtendedInfoAdditionalDetailsTypeConverter.ConvertFrom);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoBackupInstanceState = (string) content.GetValueForProperty("ExtendedInfoBackupInstanceState",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoBackupInstanceState, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoDataTransferredInByte = (double?) content.GetValueForProperty("ExtendedInfoDataTransferredInByte",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoDataTransferredInByte, (__y)=> (double) global::System.Convert.ChangeType(__y, typeof(double)));
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoRecoveryDestination = (string) content.GetValueForProperty("ExtendedInfoRecoveryDestination",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoRecoveryDestination, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoSubTask = (Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IJobSubTask[]) content.GetValueForProperty("ExtendedInfoSubTask",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).ExtendedInfoSubTask, __y => TypeConverterExtensions.SelectToArray<Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IJobSubTask>(__y, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.JobSubTaskTypeConverter.ConvertFrom));
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SourceRecoverPointRecoveryPointId = (string) content.GetValueForProperty("SourceRecoverPointRecoveryPointId",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SourceRecoverPointRecoveryPointId, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SourceRecoverPointRecoveryPointTime = (global::System.DateTime?) content.GetValueForProperty("SourceRecoverPointRecoveryPointTime",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).SourceRecoverPointRecoveryPointTime, (v) => v is global::System.DateTime _v ? _v : global::System.Xml.XmlConvert.ToDateTime( v.ToString() , global::System.Xml.XmlDateTimeSerializationMode.Unspecified));
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).TargetRecoverPointRecoveryPointId = (string) content.GetValueForProperty("TargetRecoverPointRecoveryPointId",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).TargetRecoverPointRecoveryPointId, global::System.Convert.ToString);
            ((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).TargetRecoverPointRecoveryPointTime = (global::System.DateTime?) content.GetValueForProperty("TargetRecoverPointRecoveryPointTime",((Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResourceInternal)this).TargetRecoverPointRecoveryPointTime, (v) => v is global::System.DateTime _v ? _v : global::System.Xml.XmlConvert.ToDateTime( v.ToString() , global::System.Xml.XmlDateTimeSerializationMode.Unspecified));
            AfterDeserializePSObject(content);
        }

        /// <summary>
        /// Deserializes a <see cref="global::System.Collections.IDictionary" /> into an instance of <see cref="Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.AzureBackupJobResource"
        /// />.
        /// </summary>
        /// <param name="content">The global::System.Collections.IDictionary content that should be used.</param>
        /// <returns>
        /// an instance of <see cref="Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResource"
        /// />.
        /// </returns>
        public static Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResource DeserializeFromDictionary(global::System.Collections.IDictionary content)
        {
            return new AzureBackupJobResource(content);
        }

        /// <summary>
        /// Deserializes a <see cref="global::System.Management.Automation.PSObject" /> into an instance of <see cref="Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.AzureBackupJobResource"
        /// />.
        /// </summary>
        /// <param name="content">The global::System.Management.Automation.PSObject content that should be used.</param>
        /// <returns>
        /// an instance of <see cref="Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResource"
        /// />.
        /// </returns>
        public static Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResource DeserializeFromPSObject(global::System.Management.Automation.PSObject content)
        {
            return new AzureBackupJobResource(content);
        }

        /// <summary>
        /// Creates a new instance of <see cref="AzureBackupJobResource" />, deserializing the content from a json string.
        /// </summary>
        /// <param name="jsonText">a string containing a JSON serialized instance of this model.</param>
        /// <returns>an instance of the <see cref="className" /> model class.</returns>
        public static Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202101.IAzureBackupJobResource FromJsonString(string jsonText) => FromJson(Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Runtime.Json.JsonNode.Parse(jsonText));

        /// <summary>Serializes this instance to a json string.</summary>

        /// <returns>a <see cref="System.String" /> containing this model serialized to JSON text.</returns>
        public string ToJsonString() => ToJson(null, Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Runtime.SerializationMode.IncludeAll)?.ToString();
    }
    /// AzureBackup Job Resource Class
    [System.ComponentModel.TypeConverter(typeof(AzureBackupJobResourceTypeConverter))]
    public partial interface IAzureBackupJobResource

    {

    }
}