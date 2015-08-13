<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.ResetPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<div class="AspNet-PasswordRecovery" id="divPasswordRecovery" runat="server">
		<div class="AspNet-PasswordRecovery-UserName-TitlePanel">
			<span>Reset your password</span>
		</div>
		
		<div class="AspNet-PasswordRecovery-UserName-UserPanel" runat="server" id="divResetPassword">
			<div class="AspNet-PasswordRecovery-UserName-InstructionPanel">
				<span>Please enter and verify your password below.</span>
			</div>

			<center>
				<table cellpadding="5">
					<tr>
						<td>New Password</td>
						<td>
							<asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
							<asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Please specify a password." Text="*" Display="Dynamic" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
						</td>
					</tr>
					<tr>
						<td>Verify Password</td>
						<td>
							<asp:TextBox ID="txtVerifyPassword" runat="server" TextMode="Password" 
								CausesValidation="True"></asp:TextBox>
							<asp:RequiredFieldValidator ID="rfvVerifyPassword" runat="server" ErrorMessage="Please verify your password." Text="*" Display="Dynamic" ControlToValidate="txtVerifyPassword"></asp:RequiredFieldValidator>
							<asp:CustomValidator ID="cvValidatePasswords" runat="server" 
								ControlToValidate="txtVerifyPassword" Display="Dynamic" 
								ErrorMessage="Your passwords do not match, please try again." 
								onservervalidate="cvValidatePasswords_ServerValidate">*</asp:CustomValidator>
						</td>
					</tr>
				</table>
			</center>
			<asp:ValidationSummary ID="vsValidationSummary" runat="server" />
			<br />
			<br />
			<asp:Button ID="btnSubmit" runat="server" Text="Change Password" 
				onclick="btnSubmit_Click" />
		</div>

		<div class="AspNet-PasswordRecovery-UserName-UserPanel" runat="server" id="divResetLinkInvalid">
			<center>
				<h3 class="errorText">Your password reset link is no longer valid.</h3>
				<br />
				<a href="~/ForgotPassword.aspx" id="lnkForgotPassword" runat="server">Please click here to try again.</a>
			</center>
		</div>

		<div id="divResetSuccess" class="AspNet-PasswordRecovery-UserName-UserPanel" runat="server">
			<center>
				<h3 class="errorText">Your password has been reset.</h3>
				<br />
				<a href="~/Login.aspx" id="A1" runat="server">Please click here to login.</a>
			</center>
		</div>
	</div>

</asp:Content>
