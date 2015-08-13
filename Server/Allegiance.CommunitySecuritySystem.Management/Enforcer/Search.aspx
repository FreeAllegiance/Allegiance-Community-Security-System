<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Enforcer.Search" %>
<%@ Register src="UI/UserControls/Ban.ascx" tagname="Ban" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

Enter Callsign (Wildcard: %)<br />
	<table>
		<tr>
			<td>
				<asp:TextBox ID="txtSearch" runat="server" Style="width: 250px;" 
					ontextchanged="txtSearch_TextChanged"></asp:TextBox>
			</td>
			<td>
				<asp:Button ID="btnSearch" runat="server" Text="Search" />
			</td>
			<td>
			</td>
			<td>
			</td>
		</tr>
	</table>
	<br />
	<uc1:Ban ID="ucBan" runat="server" />
	
<br />


</asp:Content>
