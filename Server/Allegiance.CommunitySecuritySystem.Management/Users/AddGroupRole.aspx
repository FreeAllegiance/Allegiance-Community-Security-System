<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="AddGroupRole.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Users.AddGroupRole" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
Adding Group Role to: 
	<asp:Label ID="lblCallsign" runat="server"></asp:Label>
	<br />
	<br />
	<table>
		<tr>
			<td>Group: </td>
			<td>
				<asp:DropDownList ID="ddlGroup" runat="server">
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>Role: </td>
			<td>
				<asp:DropDownList ID="ddlRole" runat="server">
				</asp:DropDownList>
			</td>
		</tr>
	
	</table>
	<br />
	<asp:Label ID="lblErrorMessage" runat="server" CssClass="errorText"></asp:Label>
	<br />
	<br />
	<asp:Button ID="btnSave" runat="server" Text="Save Assignment" 
		onclick="btnSave_Click" />
	<asp:Button ID="btnCancel" runat="server" Text="Cancel" />
	<hr />
	Alias is a memeber of the following Group Roles:
	<asp:Repeater ID="rptGroupRoles" runat="server">
	<HeaderTemplate><table><tr><td>Group</td><td>Role</td></tr></HeaderTemplate>
	<ItemTemplate><tr><td></td><td></td></tr></ItemTemplate>
	<FooterTemplate></table></FooterTemplate>
	</asp:Repeater>
</asp:Content>
