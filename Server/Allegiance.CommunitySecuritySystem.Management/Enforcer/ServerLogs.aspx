<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="ServerLogs.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Enforcer.ServerLogs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

Enter Search Text (Wildcard: %)<br />
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

	<asp:GridView ID="gvGames" runat="server" AutoGenerateColumns="False" EnableModelValidation="True" CssClass="fullWidthGrid">
		<Columns>
			<asp:BoundField DataField="GameID" HeaderText="ID" 
				ItemStyle-CssClass="rightAligned" >
<ItemStyle CssClass="rightAligned"></ItemStyle>
			</asp:BoundField>
			<asp:BoundField DataField="GameName" HeaderText="Name" 
				ItemStyle-CssClass="primaryColumn" >
				<ItemStyle CssClass="primaryColumn"></ItemStyle>
			</asp:BoundField>
			<asp:BoundField DataField="GameServer" HeaderText="Server" />
			<asp:BoundField DataField="GameStartTime" HeaderText="Start" />
			
			<asp:TemplateField HeaderText="Details">
				<ItemTemplate>
					<a href="GameDetails.aspx?gameIdentID=<%# Eval("GameIdentID") %>&searchText=<%= HttpContext.Current.Server.UrlEncode(txtSearch.Text) %>">Details</a>
				</ItemTemplate>
			</asp:TemplateField>
			
		</Columns>
	</asp:GridView>
	<br />

</asp:Content>
