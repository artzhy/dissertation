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

namespace ComputeAndroidApp.AuthWS {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="BasicHttpBinding_IAuthSvc", Namespace="http://tempuri.org/")]
    public partial class AuthSvc : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetAuthTokenOperationCompleted;
        
        private System.Threading.SendOrPostCallback AuthenticateUserOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public AuthSvc() {
            this.Url = "http://192.168.1.65:58041/AuthSvc.svc";
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
        public event GetAuthTokenCompletedEventHandler GetAuthTokenCompleted;
        
        /// <remarks/>
        public event AuthenticateUserCompletedEventHandler AuthenticateUserCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IAuthSvc/GetAuthToken", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string GetAuthToken([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string username, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string password, int deviceId, [System.Xml.Serialization.XmlIgnoreAttribute()] bool deviceIdSpecified) {
            object[] results = this.Invoke("GetAuthToken", new object[] {
                        username,
                        password,
                        deviceId,
                        deviceIdSpecified});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetAuthTokenAsync(string username, string password, int deviceId, bool deviceIdSpecified) {
            this.GetAuthTokenAsync(username, password, deviceId, deviceIdSpecified, null);
        }
        
        /// <remarks/>
        public void GetAuthTokenAsync(string username, string password, int deviceId, bool deviceIdSpecified, object userState) {
            if ((this.GetAuthTokenOperationCompleted == null)) {
                this.GetAuthTokenOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAuthTokenOperationCompleted);
            }
            this.InvokeAsync("GetAuthToken", new object[] {
                        username,
                        password,
                        deviceId,
                        deviceIdSpecified}, this.GetAuthTokenOperationCompleted, userState);
        }
        
        private void OnGetAuthTokenOperationCompleted(object arg) {
            if ((this.GetAuthTokenCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAuthTokenCompleted(this, new GetAuthTokenCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IAuthSvc/AuthenticateUser", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void AuthenticateUser([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string username, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string password, out bool AuthenticateUserResult, [System.Xml.Serialization.XmlIgnoreAttribute()] out bool AuthenticateUserResultSpecified) {
            object[] results = this.Invoke("AuthenticateUser", new object[] {
                        username,
                        password});
            AuthenticateUserResult = ((bool)(results[0]));
            AuthenticateUserResultSpecified = ((bool)(results[1]));
        }
        
        /// <remarks/>
        public void AuthenticateUserAsync(string username, string password) {
            this.AuthenticateUserAsync(username, password, null);
        }
        
        /// <remarks/>
        public void AuthenticateUserAsync(string username, string password, object userState) {
            if ((this.AuthenticateUserOperationCompleted == null)) {
                this.AuthenticateUserOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAuthenticateUserOperationCompleted);
            }
            this.InvokeAsync("AuthenticateUser", new object[] {
                        username,
                        password}, this.AuthenticateUserOperationCompleted, userState);
        }
        
        private void OnAuthenticateUserOperationCompleted(object arg) {
            if ((this.AuthenticateUserCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AuthenticateUserCompleted(this, new AuthenticateUserCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetAuthTokenCompletedEventHandler(object sender, GetAuthTokenCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAuthTokenCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAuthTokenCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void AuthenticateUserCompletedEventHandler(object sender, AuthenticateUserCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AuthenticateUserCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal AuthenticateUserCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool AuthenticateUserResult {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public bool AuthenticateUserResultSpecified {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[1]));
            }
        }
    }
}

#pragma warning restore 1591