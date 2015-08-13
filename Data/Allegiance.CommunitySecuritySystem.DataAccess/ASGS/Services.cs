﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

// 
// This source code was auto-generated by wsdl, Version=4.0.30319.1.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Web.Services.WebServiceBindingAttribute(Name="ServicesSoap", Namespace="http://ASGS.Alleg.net/ASGS/ASGS")]
public partial class Services : System.Web.Services.Protocols.SoapHttpClientProtocol {
    
    private System.Threading.SendOrPostCallback AuthenticateOperationCompleted;
    
    private System.Threading.SendOrPostCallback AuthenticateTicketOperationCompleted;
    
    private System.Threading.SendOrPostCallback RegisterServerOperationCompleted;
    
    private System.Threading.SendOrPostCallback PostGameStatisticsOperationCompleted;
    
    private System.Threading.SendOrPostCallback GetPlayerRankOperationCompleted;
    
    /// <remarks/>
    public Services() {
        this.Url = "http://asgs.alleg.net/ASGS/Services.asmx";
    }
    
    /// <remarks/>
    public event AuthenticateCompletedEventHandler AuthenticateCompleted;
    
    /// <remarks/>
    public event AuthenticateTicketCompletedEventHandler AuthenticateTicketCompleted;
    
    /// <remarks/>
    public event RegisterServerCompletedEventHandler RegisterServerCompleted;
    
    /// <remarks/>
    public event PostGameStatisticsCompletedEventHandler PostGameStatisticsCompleted;
    
    /// <remarks/>
    public event GetPlayerRankCompletedEventHandler GetPlayerRankCompleted;
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ASGS.Alleg.net/ASGS/ASGS/Authenticate", RequestNamespace="http://ASGS.Alleg.net/ASGS/ASGS", ResponseNamespace="http://ASGS.Alleg.net/ASGS/ASGS", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public int Authenticate(string Callsign, string PasswordHash) {
        object[] results = this.Invoke("Authenticate", new object[] {
                    Callsign,
                    PasswordHash});
        return ((int)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginAuthenticate(string Callsign, string PasswordHash, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("Authenticate", new object[] {
                    Callsign,
                    PasswordHash}, callback, asyncState);
    }
    
    /// <remarks/>
    public int EndAuthenticate(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }
    
    /// <remarks/>
    public void AuthenticateAsync(string Callsign, string PasswordHash) {
        this.AuthenticateAsync(Callsign, PasswordHash, null);
    }
    
    /// <remarks/>
    public void AuthenticateAsync(string Callsign, string PasswordHash, object userState) {
        if ((this.AuthenticateOperationCompleted == null)) {
            this.AuthenticateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAuthenticateOperationCompleted);
        }
        this.InvokeAsync("Authenticate", new object[] {
                    Callsign,
                    PasswordHash}, this.AuthenticateOperationCompleted, userState);
    }
    
    private void OnAuthenticateOperationCompleted(object arg) {
        if ((this.AuthenticateCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.AuthenticateCompleted(this, new AuthenticateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ASGS.Alleg.net/ASGS/ASGS/AuthenticateTicket", RequestNamespace="http://ASGS.Alleg.net/ASGS/ASGS", ResponseNamespace="http://ASGS.Alleg.net/ASGS/ASGS", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public int AuthenticateTicket(string Callsign, string IP, string Ticket) {
        object[] results = this.Invoke("AuthenticateTicket", new object[] {
                    Callsign,
                    IP,
                    Ticket});
        return ((int)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginAuthenticateTicket(string Callsign, string IP, string Ticket, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("AuthenticateTicket", new object[] {
                    Callsign,
                    IP,
                    Ticket}, callback, asyncState);
    }
    
    /// <remarks/>
    public int EndAuthenticateTicket(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }
    
    /// <remarks/>
    public void AuthenticateTicketAsync(string Callsign, string IP, string Ticket) {
        this.AuthenticateTicketAsync(Callsign, IP, Ticket, null);
    }
    
    /// <remarks/>
    public void AuthenticateTicketAsync(string Callsign, string IP, string Ticket, object userState) {
        if ((this.AuthenticateTicketOperationCompleted == null)) {
            this.AuthenticateTicketOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAuthenticateTicketOperationCompleted);
        }
        this.InvokeAsync("AuthenticateTicket", new object[] {
                    Callsign,
                    IP,
                    Ticket}, this.AuthenticateTicketOperationCompleted, userState);
    }
    
    private void OnAuthenticateTicketOperationCompleted(object arg) {
        if ((this.AuthenticateTicketCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.AuthenticateTicketCompleted(this, new AuthenticateTicketCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ASGS.Alleg.net/ASGS/ASGS/RegisterServer", RequestNamespace="http://ASGS.Alleg.net/ASGS/ASGS", ResponseNamespace="http://ASGS.Alleg.net/ASGS/ASGS", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public int RegisterServer(string Callsign, string ServerName, string IPAddress) {
        object[] results = this.Invoke("RegisterServer", new object[] {
                    Callsign,
                    ServerName,
                    IPAddress});
        return ((int)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginRegisterServer(string Callsign, string ServerName, string IPAddress, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("RegisterServer", new object[] {
                    Callsign,
                    ServerName,
                    IPAddress}, callback, asyncState);
    }
    
    /// <remarks/>
    public int EndRegisterServer(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }
    
    /// <remarks/>
    public void RegisterServerAsync(string Callsign, string ServerName, string IPAddress) {
        this.RegisterServerAsync(Callsign, ServerName, IPAddress, null);
    }
    
    /// <remarks/>
    public void RegisterServerAsync(string Callsign, string ServerName, string IPAddress, object userState) {
        if ((this.RegisterServerOperationCompleted == null)) {
            this.RegisterServerOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRegisterServerOperationCompleted);
        }
        this.InvokeAsync("RegisterServer", new object[] {
                    Callsign,
                    ServerName,
                    IPAddress}, this.RegisterServerOperationCompleted, userState);
    }
    
    private void OnRegisterServerOperationCompleted(object arg) {
        if ((this.RegisterServerCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.RegisterServerCompleted(this, new RegisterServerCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ASGS.Alleg.net/ASGS/ASGS/PostGameStatistics", RequestNamespace="http://ASGS.Alleg.net/ASGS/ASGS", ResponseNamespace="http://ASGS.Alleg.net/ASGS/ASGS", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public int PostGameStatistics(string CompressedGameData, out string Result) {
        object[] results = this.Invoke("PostGameStatistics", new object[] {
                    CompressedGameData});
        Result = ((string)(results[1]));
        return ((int)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginPostGameStatistics(string CompressedGameData, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("PostGameStatistics", new object[] {
                    CompressedGameData}, callback, asyncState);
    }
    
    /// <remarks/>
    public int EndPostGameStatistics(System.IAsyncResult asyncResult, out string Result) {
        object[] results = this.EndInvoke(asyncResult);
        Result = ((string)(results[1]));
        return ((int)(results[0]));
    }
    
    /// <remarks/>
    public void PostGameStatisticsAsync(string CompressedGameData) {
        this.PostGameStatisticsAsync(CompressedGameData, null);
    }
    
    /// <remarks/>
    public void PostGameStatisticsAsync(string CompressedGameData, object userState) {
        if ((this.PostGameStatisticsOperationCompleted == null)) {
            this.PostGameStatisticsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPostGameStatisticsOperationCompleted);
        }
        this.InvokeAsync("PostGameStatistics", new object[] {
                    CompressedGameData}, this.PostGameStatisticsOperationCompleted, userState);
    }
    
    private void OnPostGameStatisticsOperationCompleted(object arg) {
        if ((this.PostGameStatisticsCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.PostGameStatisticsCompleted(this, new PostGameStatisticsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ASGS.Alleg.net/ASGS/ASGS/GetPlayerRank", RequestNamespace="http://ASGS.Alleg.net/ASGS/ASGS", ResponseNamespace="http://ASGS.Alleg.net/ASGS/ASGS", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string GetPlayerRank(string Callsign) {
        object[] results = this.Invoke("GetPlayerRank", new object[] {
                    Callsign});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginGetPlayerRank(string Callsign, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("GetPlayerRank", new object[] {
                    Callsign}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndGetPlayerRank(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void GetPlayerRankAsync(string Callsign) {
        this.GetPlayerRankAsync(Callsign, null);
    }
    
    /// <remarks/>
    public void GetPlayerRankAsync(string Callsign, object userState) {
        if ((this.GetPlayerRankOperationCompleted == null)) {
            this.GetPlayerRankOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetPlayerRankOperationCompleted);
        }
        this.InvokeAsync("GetPlayerRank", new object[] {
                    Callsign}, this.GetPlayerRankOperationCompleted, userState);
    }
    
    private void OnGetPlayerRankOperationCompleted(object arg) {
        if ((this.GetPlayerRankCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.GetPlayerRankCompleted(this, new GetPlayerRankCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    public new void CancelAsync(object userState) {
        base.CancelAsync(userState);
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
public delegate void AuthenticateCompletedEventHandler(object sender, AuthenticateCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class AuthenticateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal AuthenticateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public int Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((int)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
public delegate void AuthenticateTicketCompletedEventHandler(object sender, AuthenticateTicketCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class AuthenticateTicketCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal AuthenticateTicketCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public int Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((int)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
public delegate void RegisterServerCompletedEventHandler(object sender, RegisterServerCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class RegisterServerCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal RegisterServerCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public int Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((int)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
public delegate void PostGameStatisticsCompletedEventHandler(object sender, PostGameStatisticsCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class PostGameStatisticsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal PostGameStatisticsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public int Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((int)(this.results[0]));
        }
    }
    

}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
public delegate void GetPlayerRankCompletedEventHandler(object sender, GetPlayerRankCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class GetPlayerRankCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal GetPlayerRankCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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