<%@ Page Title="" EnableViewState="false" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="BanList.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Stats.BanList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	
	<asp:GridView ID="gvBanList" CssClass="fullWidthGrid" runat="server" 
		AutoGenerateColumns="False">
		<Columns>
			<asp:BoundField DataField="Username" HeaderText="Userame" />
			<asp:BoundField DataField="Reason" HeaderText="Reason" ItemStyle-CssClass="primaryColumn" />
			<asp:BoundField DataField="BannedBy" HeaderText="Banned By" />
			<asp:BoundField DataField="DateCreated" HeaderText="Ban Date" />
			<asp:BoundField DataField="Duration" HeaderText="Duration" ItemStyle-CssClass="rightAligned" />
			<asp:BoundField DataField="TimeLeft" HeaderText="Time Left" ItemStyle-CssClass="rightAligned" />
		</Columns>
	</asp:GridView>
	
	<asp:Panel ID="pNoBannedUsers" runat="server" Visible="false">
		<br />
		<center>
			<h3>There are no banned users at this time.</h3>
		</center>
	</asp:Panel>

</asp:Content>
