<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div style="padding-top: 50px;">
		<center>
			<asp:Login ID="wcLogin" runat="server" MembershipProvider="CssMembershipProvider" CreateUserText="Create New Account" CreateUserUrl="CreateUser.aspx" PasswordRecoveryText="Forgot Password" PasswordRecoveryUrl="ForgotPassword.aspx" DestinationPageUrl="Default.aspx" TextLayout="TextOnLeft"></asp:Login>
		</center>
	</div>
</asp:Content>
