<%@ Page Title="" EnableViewState="false" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="SquadRoster.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Stats.SquadRoster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--
<div class="AspNet-GridView">
	
</div>
	<table class="centerHeaders squadRoster">
		<thead>
			<tr>
<% 
	foreach (var squad in SquadList)
	{
%>
		<th class="centerHeaders"><%= squad.SquadName %></th>
<%
	} 
%>
			</tr>
		</thead>
	<tr valign="top">

<% 
	foreach (var squad in SquadList)
	{
%>
		<td>
			<table>
<%
		foreach(var member in squad.Members)
		{
%>
			<tr class="<%= member.ActiveCssClass %>">
				<td style="width: 10px; text-align: right;"><%= member.TokenHtml %></td>
				<td><%= member.Callsign %></td>
			</tr>
<%
		} 
%>	
			</table>
		</td>
<%
	} 
%>
	</tr>
</table>--%>

<div class="AspNet-GridView">
	<table class="centerHeaders squadRoster">
		<thead>
			<tr>
				<asp:Repeater ID="rptHeaders" runat="server">
					<ItemTemplate>
						<th class="centerHeaders"><%# Eval("SquadName") %></th>
					</ItemTemplate>
				</asp:Repeater>
			</tr>
		</thead>
		<tr valign="top">
			<asp:Repeater ID="rptSquads" runat="server" onitemdatabound="rptSquads_ItemDataBound">
				<ItemTemplate>
					<td>
						<table>
							<asp:Repeater ID="rptMembers" runat="server">
								<ItemTemplate>
									<tr class="<%# Eval("ActiveCssClass") %>">
										<td style="width: 10px; text-align: right;"><%# Eval("Token").ToString().Length == 0 ? "&nbsp;" : Eval("Token") %></td>
										<td><%# Eval("Callsign") %></td>
									</tr>
								</ItemTemplate>
							</asp:Repeater>
						</table>
					</td>
				</ItemTemplate>
			</asp:Repeater>
		</tr>
		<tfoot>
			<tr>
				<asp:Repeater ID="rptFooter" runat="server">
					<ItemTemplate>
						<td>
							<br />
							<hr />
							<table>
								<tr>
									<td style="text-align: right;">Active:</td>
									<td style="text-align: left; padding-left: 5px;"><%# Eval("ActiveMembers") %></td>
								</tr>
								<tr>
									<td style="text-align: right;">Inactive:</td>
									<td style="text-align: left; padding-left: 5px;"><%# Eval("InactiveMembers") %></td>
								</tr>
							</table>
						</td>
					</ItemTemplate>
				</asp:Repeater>
			</tr>
		</tfoot>
		
	</table>
</div>

</asp:Content>
