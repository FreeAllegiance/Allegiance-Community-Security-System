<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterPages/NoChrome.Master" CodeBehind="PublicationDetailsIFrame.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.PublicationDetailsIFrame" %>


<%@ Register src="UI/UserControls/PackageContents.ascx" tagname="PackageContents" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<asp:ScriptManager ID="ScriptManager1" runat="server" LoadScriptsBeforeUI="true">
	</asp:ScriptManager>

	<div>
    	<uc1:PackageContents ID="ucPackageContents" ShowDelete="False" runat="server" />
    </div>
</asp:Content>
