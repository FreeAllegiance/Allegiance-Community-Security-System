<%@ Page Title="" EnableViewState="false" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Faction.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Stats.Faction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript" src="<%= Page.ResolveUrl("~/Scripts/jquery.columnhover.pack.js") %>"></script> 

Faction Statistics Overview
<hr />
<br />
<div class="AspNet-GridView">
	<table class="fullWidthGrid highlightRows nowrapCells nowrapHeaders centerCells" id="tblFactionStats" cellpadding="0" cellspacing="0">
		<thead>
			<asp:Repeater ID="rptFactionHeaders" runat="server">
				<HeaderTemplate><th>&nbsp;</th></HeaderTemplate>
				<ItemTemplate><th><%# Eval("Name") %> (win)</th></ItemTemplate>
			</asp:Repeater>
		</thead>
		<asp:Repeater ID="rptFactionRow" runat="server">
			<HeaderTemplate></HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td style="text-align: right;"><%# Eval("Name") %> (loss)</td>
					<%# Eval("Stats") %>
				</tr>
			</ItemTemplate>
			<FooterTemplate></FooterTemplate>
		</asp:Repeater>
	</table>
</div>

<script type="text/javascript">

	$(document).ready(function()
	{
		$('#tblFactionStats').columnHover({ eachCell: true, hoverClass: 'highlightColumns' });
	});
	
</script>

</asp:Content>

