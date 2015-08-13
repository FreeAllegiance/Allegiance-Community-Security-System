<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="GameDetails.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Enforcer.GameDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<h3>Game Details</h3>
<div style="width: 800px; overflow-x: scroll;">
	<asp:GridView ID="gvGame" runat="server" AutoGenerateColumns="True" EnableModelValidation="True" CssClass="fullWidthGrid">
	</asp:GridView>
</div>

<br />
<br />

<asp:Repeater ID="rptTeams" runat="server" onitemdatabound="rptTeams_ItemDataBound">
	<ItemTemplate>
		<h3>Team <%# Eval("GameTeamName") %></h3>
		
		<asp:GridView ID="gvTeam" runat="server" AutoGenerateColumns="True" EnableModelValidation="True" CssClass="fullWidthGrid wrapLastColumn300">
		</asp:GridView>
		
	</ItemTemplate>
</asp:Repeater>

<br />
<br />

<h3>Chat Log</h3>
<div>
	<asp:GridView ID="gvChatLog" runat="server" AutoGenerateColumns="True" EnableModelValidation="True" CssClass="fullWidthGrid wrapLastColumn300">
	</asp:GridView>
</div>

</asp:Content>
