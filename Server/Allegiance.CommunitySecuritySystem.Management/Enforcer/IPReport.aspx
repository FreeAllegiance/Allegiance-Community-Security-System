<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="IPReport.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Enforcer.IPReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" >
	</asp:ScriptManagerProxy>

	<h4>Users using same IP</h4>
	<asp:GridView ID="gvUserIpMatches" runat="server" AutoGenerateColumns="False" 
		EnableModelValidation="True"  CssClass="fullWidthGrid">
		<Columns>
			<asp:BoundField DataField="IPAddress" HeaderText="IP Address" />

			<asp:TemplateField HeaderText="User 1">
				<ItemTemplate>
					<a href="Search.aspx?searchText=<%# Eval("User1Encoded") %>"><%# Eval("User1") %></a>
				</ItemTemplate>
			</asp:TemplateField>

			<asp:BoundField DataField="User1Date" HeaderText="User 1 Login Date" />
			
			<asp:TemplateField HeaderText="User 2">
				<ItemTemplate>
					<a href="Search.aspx?searchText=<%# Eval("User2Encoded") %>"><%# Eval("User2") %></a>
				</ItemTemplate>
			</asp:TemplateField>

			<asp:BoundField DataField="User2Date" HeaderText="User 2 Login Date" />
		</Columns>
	</asp:GridView>
	<br />

	<h4>Users in same Class C subnet</h4>
	<asp:GridView ID="gvUserSubnetMatches" runat="server" AutoGenerateColumns="False" 
		EnableModelValidation="True"  CssClass="fullWidthGrid">
		<Columns>
			<asp:BoundField DataField="IPAddress" HeaderText="IP Address" />

			<asp:TemplateField HeaderText="User 1">
				<ItemTemplate>
					<a href="Search.aspx?searchText=<%# Eval("User1Encoded") %>"><%# Eval("User1") %></a>
				</ItemTemplate>
			</asp:TemplateField>

			<asp:BoundField DataField="User1Date" HeaderText="User 1 Login Date" />
			
			<asp:TemplateField HeaderText="User 2">
				<ItemTemplate>
					<a href="Search.aspx?searchText=<%# Eval("User2Encoded") %>"><%# Eval("User2") %></a>
				</ItemTemplate>
			</asp:TemplateField>

			<asp:BoundField DataField="User2Date" HeaderText="User 2 Login Date" />
		</Columns>
	</asp:GridView>
	<br />

</asp:Content>
