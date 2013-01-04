﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.17929.
// 
#pragma warning disable 1591

namespace ComputeAndroidApp.UserWS {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="BasicHttpBinding_IUserSvc", Namespace="http://tempuri.org/")]
    public partial class UserSvc : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback AddUserOperationCompleted;
        
        private System.Threading.SendOrPostCallback ModifyUserOperationCompleted;
        
        private System.Threading.SendOrPostCallback AddUserDeviceOperationCompleted;
        
        private System.Threading.SendOrPostCallback DeleteUserDeviceOperationCompleted;
        
        private System.Threading.SendOrPostCallback ModifyUserDeviceOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetDeviceIdOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public UserSvc() {
            this.Url = "http://192.168.1.65:58041/UserSvc.svc";
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event AddUserCompletedEventHandler AddUserCompleted;
        
        /// <remarks/>
        public event ModifyUserCompletedEventHandler ModifyUserCompleted;
        
        /// <remarks/>
        public event AddUserDeviceCompletedEventHandler AddUserDeviceCompleted;
        
        /// <remarks/>
        public event DeleteUserDeviceCompletedEventHandler DeleteUserDeviceCompleted;
        
        /// <remarks/>
        public event ModifyUserDeviceCompletedEventHandler ModifyUserDeviceCompleted;
        
        /// <remarks/>
        public event GetDeviceIdCompletedEventHandler GetDeviceIdCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IUserSvc/AddUser", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public User AddUser([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string forename, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string surname, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string username, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string password) {
            object[] results = this.Invoke("AddUser", new object[] {
                        forename,
                        surname,
                        username,
                        password});
            return ((User)(results[0]));
        }
        
        /// <remarks/>
        public void AddUserAsync(string forename, string surname, string username, string password) {
            this.AddUserAsync(forename, surname, username, password, null);
        }
        
        /// <remarks/>
        public void AddUserAsync(string forename, string surname, string username, string password, object userState) {
            if ((this.AddUserOperationCompleted == null)) {
                this.AddUserOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddUserOperationCompleted);
            }
            this.InvokeAsync("AddUser", new object[] {
                        forename,
                        surname,
                        username,
                        password}, this.AddUserOperationCompleted, userState);
        }
        
        private void OnAddUserOperationCompleted(object arg) {
            if ((this.AddUserCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddUserCompleted(this, new AddUserCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IUserSvc/ModifyUser", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void ModifyUser([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string authUsername, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string authPassword, int userId, [System.Xml.Serialization.XmlIgnoreAttribute()] bool userIdSpecified, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string forename, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string surname, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string password) {
            this.Invoke("ModifyUser", new object[] {
                        authUsername,
                        authPassword,
                        userId,
                        userIdSpecified,
                        forename,
                        surname,
                        password});
        }
        
        /// <remarks/>
        public void ModifyUserAsync(string authUsername, string authPassword, int userId, bool userIdSpecified, string forename, string surname, string password) {
            this.ModifyUserAsync(authUsername, authPassword, userId, userIdSpecified, forename, surname, password, null);
        }
        
        /// <remarks/>
        public void ModifyUserAsync(string authUsername, string authPassword, int userId, bool userIdSpecified, string forename, string surname, string password, object userState) {
            if ((this.ModifyUserOperationCompleted == null)) {
                this.ModifyUserOperationCompleted = new System.Threading.SendOrPostCallback(this.OnModifyUserOperationCompleted);
            }
            this.InvokeAsync("ModifyUser", new object[] {
                        authUsername,
                        authPassword,
                        userId,
                        userIdSpecified,
                        forename,
                        surname,
                        password}, this.ModifyUserOperationCompleted, userState);
        }
        
        private void OnModifyUserOperationCompleted(object arg) {
            if ((this.ModifyUserCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ModifyUserCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IUserSvc/AddUserDevice", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public UserDevice AddUserDevice([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string authUsername, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string authPassword, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string deviceType, int deviceMemoryResource, [System.Xml.Serialization.XmlIgnoreAttribute()] bool deviceMemoryResourceSpecified, int deviceProcRating, [System.Xml.Serialization.XmlIgnoreAttribute()] bool deviceProcRatingSpecified, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string gcmCode) {
            object[] results = this.Invoke("AddUserDevice", new object[] {
                        authUsername,
                        authPassword,
                        deviceType,
                        deviceMemoryResource,
                        deviceMemoryResourceSpecified,
                        deviceProcRating,
                        deviceProcRatingSpecified,
                        gcmCode});
            return ((UserDevice)(results[0]));
        }
        
        /// <remarks/>
        public void AddUserDeviceAsync(string authUsername, string authPassword, string deviceType, int deviceMemoryResource, bool deviceMemoryResourceSpecified, int deviceProcRating, bool deviceProcRatingSpecified, string gcmCode) {
            this.AddUserDeviceAsync(authUsername, authPassword, deviceType, deviceMemoryResource, deviceMemoryResourceSpecified, deviceProcRating, deviceProcRatingSpecified, gcmCode, null);
        }
        
        /// <remarks/>
        public void AddUserDeviceAsync(string authUsername, string authPassword, string deviceType, int deviceMemoryResource, bool deviceMemoryResourceSpecified, int deviceProcRating, bool deviceProcRatingSpecified, string gcmCode, object userState) {
            if ((this.AddUserDeviceOperationCompleted == null)) {
                this.AddUserDeviceOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddUserDeviceOperationCompleted);
            }
            this.InvokeAsync("AddUserDevice", new object[] {
                        authUsername,
                        authPassword,
                        deviceType,
                        deviceMemoryResource,
                        deviceMemoryResourceSpecified,
                        deviceProcRating,
                        deviceProcRatingSpecified,
                        gcmCode}, this.AddUserDeviceOperationCompleted, userState);
        }
        
        private void OnAddUserDeviceOperationCompleted(object arg) {
            if ((this.AddUserDeviceCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddUserDeviceCompleted(this, new AddUserDeviceCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IUserSvc/DeleteUserDevice", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void DeleteUserDevice([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string authUsername, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string authPassword, int deviceId, [System.Xml.Serialization.XmlIgnoreAttribute()] bool deviceIdSpecified) {
            this.Invoke("DeleteUserDevice", new object[] {
                        authUsername,
                        authPassword,
                        deviceId,
                        deviceIdSpecified});
        }
        
        /// <remarks/>
        public void DeleteUserDeviceAsync(string authUsername, string authPassword, int deviceId, bool deviceIdSpecified) {
            this.DeleteUserDeviceAsync(authUsername, authPassword, deviceId, deviceIdSpecified, null);
        }
        
        /// <remarks/>
        public void DeleteUserDeviceAsync(string authUsername, string authPassword, int deviceId, bool deviceIdSpecified, object userState) {
            if ((this.DeleteUserDeviceOperationCompleted == null)) {
                this.DeleteUserDeviceOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteUserDeviceOperationCompleted);
            }
            this.InvokeAsync("DeleteUserDevice", new object[] {
                        authUsername,
                        authPassword,
                        deviceId,
                        deviceIdSpecified}, this.DeleteUserDeviceOperationCompleted, userState);
        }
        
        private void OnDeleteUserDeviceOperationCompleted(object arg) {
            if ((this.DeleteUserDeviceCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteUserDeviceCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IUserSvc/ModifyUserDevice", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void ModifyUserDevice([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string authUsername, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string authPassword, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string gcmCode, int deviceId, [System.Xml.Serialization.XmlIgnoreAttribute()] bool deviceIdSpecified) {
            this.Invoke("ModifyUserDevice", new object[] {
                        authUsername,
                        authPassword,
                        gcmCode,
                        deviceId,
                        deviceIdSpecified});
        }
        
        /// <remarks/>
        public void ModifyUserDeviceAsync(string authUsername, string authPassword, string gcmCode, int deviceId, bool deviceIdSpecified) {
            this.ModifyUserDeviceAsync(authUsername, authPassword, gcmCode, deviceId, deviceIdSpecified, null);
        }
        
        /// <remarks/>
        public void ModifyUserDeviceAsync(string authUsername, string authPassword, string gcmCode, int deviceId, bool deviceIdSpecified, object userState) {
            if ((this.ModifyUserDeviceOperationCompleted == null)) {
                this.ModifyUserDeviceOperationCompleted = new System.Threading.SendOrPostCallback(this.OnModifyUserDeviceOperationCompleted);
            }
            this.InvokeAsync("ModifyUserDevice", new object[] {
                        authUsername,
                        authPassword,
                        gcmCode,
                        deviceId,
                        deviceIdSpecified}, this.ModifyUserDeviceOperationCompleted, userState);
        }
        
        private void OnModifyUserDeviceOperationCompleted(object arg) {
            if ((this.ModifyUserDeviceCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ModifyUserDeviceCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IUserSvc/GetDeviceId", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void GetDeviceId([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string authUsername, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string authPassword, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string gcmId, out int GetDeviceIdResult, [System.Xml.Serialization.XmlIgnoreAttribute()] out bool GetDeviceIdResultSpecified) {
            object[] results = this.Invoke("GetDeviceId", new object[] {
                        authUsername,
                        authPassword,
                        gcmId});
            GetDeviceIdResult = ((int)(results[0]));
            GetDeviceIdResultSpecified = ((bool)(results[1]));
        }
        
        /// <remarks/>
        public void GetDeviceIdAsync(string authUsername, string authPassword, string gcmId) {
            this.GetDeviceIdAsync(authUsername, authPassword, gcmId, null);
        }
        
        /// <remarks/>
        public void GetDeviceIdAsync(string authUsername, string authPassword, string gcmId, object userState) {
            if ((this.GetDeviceIdOperationCompleted == null)) {
                this.GetDeviceIdOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetDeviceIdOperationCompleted);
            }
            this.InvokeAsync("GetDeviceId", new object[] {
                        authUsername,
                        authPassword,
                        gcmId}, this.GetDeviceIdOperationCompleted, userState);
        }
        
        private void OnGetDeviceIdOperationCompleted(object arg) {
            if ((this.GetDeviceIdCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetDeviceIdCompleted(this, new GetDeviceIdCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/BusinessLayer")]
    public partial class User {
        
        private string forenamek__BackingFieldField;
        
        private string passwordk__BackingFieldField;
        
        private string surnamek__BackingFieldField;
        
        private UserDevice[] userDevicesk__BackingFieldField;
        
        private int userIdk__BackingFieldField;
        
        private string usernamek__BackingFieldField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("<Forename>k__BackingField", IsNullable=true)]
        public string Forenamek__BackingField {
            get {
                return this.forenamek__BackingFieldField;
            }
            set {
                this.forenamek__BackingFieldField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("<Password>k__BackingField", IsNullable=true)]
        public string Passwordk__BackingField {
            get {
                return this.passwordk__BackingFieldField;
            }
            set {
                this.passwordk__BackingFieldField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("<Surname>k__BackingField", IsNullable=true)]
        public string Surnamek__BackingField {
            get {
                return this.surnamek__BackingFieldField;
            }
            set {
                this.surnamek__BackingFieldField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute("<UserDevices>k__BackingField", IsNullable=true)]
        public UserDevice[] UserDevicesk__BackingField {
            get {
                return this.userDevicesk__BackingFieldField;
            }
            set {
                this.userDevicesk__BackingFieldField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("<UserId>k__BackingField")]
        public int UserIdk__BackingField {
            get {
                return this.userIdk__BackingFieldField;
            }
            set {
                this.userIdk__BackingFieldField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("<Username>k__BackingField", IsNullable=true)]
        public string Usernamek__BackingField {
            get {
                return this.usernamek__BackingFieldField;
            }
            set {
                this.usernamek__BackingFieldField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/BusinessLayer")]
    public partial class UserDevice {
        
        private int deviceIdk__BackingFieldField;
        
        private int deviceMemoryResourcek__BackingFieldField;
        
        private int deviceProcRatingk__BackingFieldField;
        
        private string deviceTypek__BackingFieldField;
        
        private string gCMCodek__BackingFieldField;
        
        private User userk__BackingFieldField;
        
        private string usernamek__BackingFieldField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("<DeviceId>k__BackingField")]
        public int DeviceIdk__BackingField {
            get {
                return this.deviceIdk__BackingFieldField;
            }
            set {
                this.deviceIdk__BackingFieldField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("<DeviceMemoryResource>k__BackingField")]
        public int DeviceMemoryResourcek__BackingField {
            get {
                return this.deviceMemoryResourcek__BackingFieldField;
            }
            set {
                this.deviceMemoryResourcek__BackingFieldField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("<DeviceProcRating>k__BackingField")]
        public int DeviceProcRatingk__BackingField {
            get {
                return this.deviceProcRatingk__BackingFieldField;
            }
            set {
                this.deviceProcRatingk__BackingFieldField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("<DeviceType>k__BackingField", IsNullable=true)]
        public string DeviceTypek__BackingField {
            get {
                return this.deviceTypek__BackingFieldField;
            }
            set {
                this.deviceTypek__BackingFieldField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("<GCMCode>k__BackingField", IsNullable=true)]
        public string GCMCodek__BackingField {
            get {
                return this.gCMCodek__BackingFieldField;
            }
            set {
                this.gCMCodek__BackingFieldField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("<User>k__BackingField", IsNullable=true)]
        public User Userk__BackingField {
            get {
                return this.userk__BackingFieldField;
            }
            set {
                this.userk__BackingFieldField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("<Username>k__BackingField", IsNullable=true)]
        public string Usernamek__BackingField {
            get {
                return this.usernamek__BackingFieldField;
            }
            set {
                this.usernamek__BackingFieldField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void AddUserCompletedEventHandler(object sender, AddUserCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AddUserCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal AddUserCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public User Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((User)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void ModifyUserCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void AddUserDeviceCompletedEventHandler(object sender, AddUserDeviceCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AddUserDeviceCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal AddUserDeviceCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public UserDevice Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((UserDevice)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void DeleteUserDeviceCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void ModifyUserDeviceCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetDeviceIdCompletedEventHandler(object sender, GetDeviceIdCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetDeviceIdCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetDeviceIdCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int GetDeviceIdResult {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public bool GetDeviceIdResultSpecified {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[1]));
            }
        }
    }
}

#pragma warning restore 1591