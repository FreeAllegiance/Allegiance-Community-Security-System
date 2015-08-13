<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Users.EditUser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" >
	</asp:ScriptManagerProxy>

	<center>
		<table class="fullWidthTable" style="text-align: left;">
			<tr>
				<td class="label">
					Username:
				</td>
				<td>
					<asp:TextBox ID="txtUsername" runat="server" 
						ontextchanged="OnDataChanged"></asp:TextBox>
				</td>
				<td  class="label">
					Email:
				</td>
				<td>
					<asp:TextBox ID="txtEmail" runat="server" ontextchanged="OnDataChanged"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td class="seperator" colspan="4">&nbsp;</td>
			</tr>
			<tr valign="top">
				<td class="label">Login Roles:</td>
				<td>
					<asp:CheckBoxList ID="cblLoginRoles" runat="server" 
						onselectedindexchanged="OnDataChanged">
					</asp:CheckBoxList>
				</td>
				<td class="label">
					<asp:Label ID="lblAllowVirtualMachine" runat="server" Text="Allow Virtual Machine:" AssociatedControlID="chkAllowVirtualMachine"></asp:Label></td>
				<td><asp:CheckBox ID="chkAllowVirtualMachine" runat="server" OnCheckedChanged="OnDataChanged" /></td>
				
			</tr>
			<tr>
				<td class="seperator" colspan="4">&nbsp;</td>
			</tr>
			<tr>
				<td colspan="4">
					<table cellpadding="5" cellspacing="0" style="width: 100%; border: 1px solid black;">
						<tr>
							<td class="modalHeader">
								Alias Management
							</td>
						</tr>
						<tr>
							<td>
								<cc1:TabContainer ID="tcAliases" runat="server" ActiveTabIndex="0">
								</cc1:TabContainer>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<br />
		<asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button 
			ID="btnCancel" runat="server" Text="Cancel" onclick="btnCancel_Click" />
			<br />
			<br />
			<asp:Label ID="lblSaveMessage" class="errorText" Text="" runat="server"></asp:Label>
	</center>
</asp:Content>
