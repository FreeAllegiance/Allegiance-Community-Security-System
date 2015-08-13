<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Users.Default" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" >
</asp:ScriptManagerProxy>

Enter Callsign, Email, or Login Username (Wildcard: %)<br />
	<table>
		<tr>
			<td>
				<asp:TextBox ID="txtSearch" runat="server" Style="width: 250px;" 
					ontextchanged="txtSearch_TextChanged"></asp:TextBox>
			</td>
			<td>
				<asp:Button ID="btnSearch" runat="server" Text="Search" 
					onclick="btnSearch_Click" />
			</td>
			<td>
			</td>
			<td>
			</td>
		</tr>
	</table>
	<br />
	<asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" 
		Visible="False" CssClass="fullWidthGrid">
		<Columns>
			<asp:BoundField DataField="Username" HeaderText="User Name" />
			<asp:BoundField DataField="Email" HeaderText="Email" />
			<asp:BoundField DataField="DateCreated" DataFormatString="{0:d} - {0:T}" 
				HeaderText="Date Created" />
			<asp:BoundField DataField="LastLogin" DataFormatString="{0:d} - {0:T}" 
				HeaderText="Last Login" />
			<asp:TemplateField HeaderText="Edit">
				<ItemTemplate>
					<a href="EditUser.aspx?loginID=<%# Eval("Id") %>&searchText=<%= HttpContext.Current.Server.UrlEncode(txtSearch.Text) %>">Edit</a>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Linking">
				<ItemTemplate>
					<a href="ManageLinks.aspx?loginID=<%# Eval("Id") %>&searchText=<%= HttpContext.Current.Server.UrlEncode(txtSearch.Text) %>"><%# Eval("LinkManagementLabel")%></a>
				</ItemTemplate>
			</asp:TemplateField>
			
		</Columns>
	</asp:GridView>
	
</asp:Content>
