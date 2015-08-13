<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="CreateUser.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.CreateUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:CreateUserWizard ID="CreateUserWizard1" runat="server" 
	CancelDestinationPageUrl="~/Default.aspx" 
	ContinueDestinationPageUrl="~/Default.aspx" 
	MembershipProvider="CssMembershipProvider" >
	<WizardSteps>
		<asp:CreateUserWizardStep runat="server">
			<ContentTemplate>
				<div class="AspNet-CreateUserWizard-CreateStepPanel">
					<center>
					<div class="AspNet-CreateUserWizard-StepTitlePanel">Sign Up for Your New Account</div>
					<table border="0" class="AspNet-CreateUserWizard-UserInfoTable">
						
						<tr>
							<td align="right">
								<asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
							</td>
							<td>
								<asp:TextBox ID="UserName" runat="server" MaxLength="17" ></asp:TextBox>
								<asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
									ControlToValidate="UserName" ErrorMessage="User Name is required." 
									ToolTip="User Name is required." ValidationGroup="CreateUserWizard1" Display="Dynamic">*</asp:RequiredFieldValidator>
								<asp:CustomValidator ID="userNameCustomValidator" runat="server" 
									ClientValidationFunction="validateUserName" ControlToValidate="UserName" 
									ErrorMessage="Please enter a user name between 3 and 17 characters." 
									onservervalidate="userNameCustomValidator_ServerValidate" ValidationGroup="CreateUserWizard1" 
									Display="Dynamic" EnableClientScript="False">*</asp:CustomValidator>
								<asp:CustomValidator ID="cvLegacyAliasCheck" runat="server" ValidationGroup="CreateUserWizard1" 
									ControlToValidate="UserName" Display="Dynamic" 
									ErrorMessage="This callsign is already registered on ASGS (the old security system). Please use your original password to create this account and claim this callsign. If you don't remeber your password, please contact the system adminstration via the forums." 
									onservervalidate="cvLegacyAliasCheck_ServerValidate">*</asp:CustomValidator>
							</td>
						</tr>
						<tr>
							<td align="right">
								<asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
							</td>
							<td>
								<asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
								<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
									ControlToValidate="Password" ErrorMessage="Password is required." 
									ToolTip="Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
							</td>
						</tr>
						<tr>
							<td align="right">
								<asp:Label ID="ConfirmPasswordLabel" runat="server" 
									AssociatedControlID="ConfirmPassword">Confirm Password:</asp:Label>
							</td>
							<td>
								<asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
								<asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" 
									ControlToValidate="ConfirmPassword" 
									ErrorMessage="Confirm Password is required." 
									ToolTip="Confirm Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
							</td>
						</tr>
						<tr>
							<td align="right">
								<asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>
							</td>
							<td>
								<asp:TextBox ID="Email" runat="server"></asp:TextBox>
								<asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
									ControlToValidate="Email" ErrorMessage="E-mail is required." 
									ToolTip="E-mail is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
							</td>
						</tr>
						<tr>
							<td align="right">
								Validation Code</td>
							<td>
								<asp:TextBox ID="txtValidationCode" runat="server"></asp:TextBox>
								<asp:CustomValidator ID="cvCaptcha" runat="server" Display="Dynamic" 
									ErrorMessage="Please enter the validation code displayed to create your account." 
									ControlToValidate="txtValidationCode" 
									onservervalidate="cvCaptcha_ServerValidate" ValidationGroup="CreateUserWizard1">*</asp:CustomValidator>
								<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
									Display="Dynamic" 
									ErrorMessage="Please enter the validation code displayed to create your account." 
									ControlToValidate="txtValidationCode" ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
							</td>
						</tr>
						<tr>
							<td align="right">
								&nbsp;</td>
							<td>
								<asp:Image ID="imgValidationImage" runat="server" 
									onload="imgValidationImage_Load" onprerender="imgValidationImage_PreRender" />
							</td>
						</tr>
						<tr>
							<td align="right">
								&nbsp;</td>
							<td>
								&nbsp;</td>
						</tr>
						<tr>
							<td align="right">
								&nbsp;</td>
							<td>
								&nbsp;</td>
						</tr>
						<tr>
							<td align="center" colspan="2">
								<asp:CompareValidator ID="PasswordCompare" runat="server" 
									ControlToCompare="Password" ControlToValidate="ConfirmPassword" 
									Display="Dynamic" 
									ErrorMessage="The Password and Confirmation Password must match." 
									ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
							</td>
						</tr>
						<tr>
							<td align="center" colspan="2" style="color:Red;">
								<asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
								<asp:ValidationSummary ID="vsValidationSummary" runat="server" 
									ValidationGroup="CreateUserWizard1" />
							</td>
						</tr>
					</table>
					</center>
				
			</ContentTemplate>
		</asp:CreateUserWizardStep>
		
<asp:CompleteWizardStep runat="server"></asp:CompleteWizardStep>
		
	</WizardSteps>
</asp:CreateUserWizard>

<%--<asp:HiddenField ID="txtUserNameUniqueID" runat="server" />

<script type="text/javascript" language="javascript">

	function validateUserName(source, args)
	{
		var txtUserNameUniqueID = document.getElementById("<%= txtUserNameUniqueID.UniqueID %>");

		// Using this to expose the underlying client id for the UserName textbox to the server side 
		// code on postback. There doesn't seem to be any good way to get it from the MS web control
		// or the template.
		txtUserNameUniqueID.value = source.controltovalidate;

		/* The microsoft membership provider framework does not allow the validation summary to work correctly for client side custom validators.
		var username = document.getElementById(source.controltovalidate).value;

		if(username.length < <%= Allegiance.CommunitySecuritySystem.DataAccess.Alias.MinAliasLength %> || username.length > <%= Allegiance.CommunitySecuritySystem.DataAccess.Alias.MaxAliasLength %>)
			args.IsValid = false;
		*/
	}

</script>--%>

</asp:Content>
