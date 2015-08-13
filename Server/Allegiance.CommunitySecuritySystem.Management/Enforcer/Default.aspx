<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Enforcer.Default" %>

<%@ Register src="UI/UserControls/Ban.ascx" tagname="Ban" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	
	
	Active Callsigns<br />
	<hr />
	<uc1:Ban ID="ucBan" runat="server" />
	
</asp:Content>
