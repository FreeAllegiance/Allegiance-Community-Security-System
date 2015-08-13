<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.ForgotPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<asp:Panel ID="pErrorMessage" runat="server">
	<br />
	<br />
	<center><asp:Label CssClass="errorText" ID="lblErrorMessage" runat="server" Text=""></asp:Label></center>
	<br />
	<br />
	</asp:Panel>
	

	<div class="AspNet-PasswordRecovery" id="divPasswordRecovery" runat="server">
		<div class="AspNet-PasswordRecovery-UserName-TitlePanel">
			<span>Forgot Your Password?</span>
		</div>
		<div class="AspNet-PasswordRecovery-UserName-InstructionPanel">
			<span>Enter your User Name to receive your password.</span>
		</div>
		<div class="AspNet-PasswordRecovery-UserName-UserPanel">
			<label for="ctl00_ContentPlaceHolder1_PasswordRecovery1_UserNameContainerID_UserName"><em>U</em>ser Name:</label>
			<asp:TextBox ID="txtUsername" runat="server" CausesValidation="True" 
				ValidationGroup="Validation"></asp:TextBox>
			<asp:CustomValidator ID="cvUsername"
				runat="server" ErrorMessage="Your username was not found." Text="*" 
				ControlToValidate="txtUsername" Display="Dynamic" ValidationGroup="Validation" 
				onservervalidate="cvUsername_ServerValidate" SetFocusOnError="True" ></asp:CustomValidator><asp:RequiredFieldValidator
				ID="rfvUsername"  runat="server" ErrorMessage="Please enter your user name." ValidationGroup="Validation"  ControlToValidate="txtUsername" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
			<br />
			<asp:ValidationSummary ID="vsValidationSummary" runat="server" 
				ValidationGroup="Validation" />
			<br />
		</div>
		<div class="AspNet-PasswordRecovery-UserName-SubmitPanel">
			<asp:Button ID="btnSubmit" runat="server" Text="Send Reset Link" 
				onclick="btnSubmit_Click" ValidationGroup="Validation" />
		</div>
	</div>

	<asp:Label ID="lblSuccessMessage" runat="server" Text=""></asp:Label>
</asp:Content>
