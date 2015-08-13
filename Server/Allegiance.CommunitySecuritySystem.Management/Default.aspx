<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:LoginView ID="LoginView1" runat="server">
		<LoggedInTemplate>
											
		</LoggedInTemplate>
		<AnonymousTemplate>
			
		</AnonymousTemplate>
	</asp:LoginView>
</asp:Content>
