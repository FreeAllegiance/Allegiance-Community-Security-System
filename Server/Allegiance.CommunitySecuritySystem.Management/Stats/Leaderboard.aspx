<%@ Page Title="" EnableViewState="false" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Leaderboard.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Stats.Leaderboard" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<!-- Enable leader board table to render correctly with table sort JS applied. -->
	<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	
	<%--<script src="<%= ResolveUrl("~/Scripts/jquery-1.4.2.min.js") %>" type="text/javascript" language="javascript"></script>--%>
	<!--<script src="../../Scripts/jquery.fixedtableheader-1-0-2.min.js" type="text/javascript" language="javascript"></script>-->
	<!--<script src="../../Scripts/jquery.fixedheadertable.1.0.js" type="text/javascript" language="javascript"></script>-->
	<script src="<%= ResolveUrl("~/Scripts/sorttable.js") %>" type="text/javascript" language="javascript"></script>
	<!--<script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>-->
	<%--<script src="../Scripts/jquery.ingrid.js" type="text/javascript"></script>--%>
	<%--<script src="../Scripts/jquery.jqGrid.min.js" type="text/javascript"></script>--%>
	<%--
		<table class="table2"></table>
	--%>
	
		<!-- If the first row is getting hidden in the results, adjust: .scrollableTable tbody tr:first-child td in the IE specific styles. -->
		<div class="scrollableTableWrapper">
			<asp:GridView ID="gvLeaderboard" runat="server" AutoGenerateColumns="False" CssClass="AspNet-GridView sortable scrollableTable" >
				<Columns>
					<asp:TemplateField HeaderText="#" ControlStyle-CssClass="hiddenColumn">
						<ItemStyle CssClass="hiddenColumn" />
						<ItemTemplate><div id="OrderIndexDiv"><%# Eval("Order") %></div></ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="Place" HeaderText="Place" />
					<asp:BoundField DataField="Callsign" HeaderText="Callsign" >
						<ItemStyle CssClass="primaryColumn" />
					</asp:BoundField>
					<asp:BoundField DataField="MuString" HeaderText="Mu" />
					<asp:BoundField DataField="SigmaString" HeaderText="Sigma" />
					<asp:BoundField DataField="RankString" HeaderText="Rank" />
                    
					<asp:BoundField DataField="Wins" HeaderText="Wins" />
					<asp:BoundField DataField="Losses" HeaderText="Losses" />
					<asp:BoundField DataField="Draws" HeaderText="Draws" />
					<asp:BoundField DataField="Defects" HeaderText="Defects" />
					<asp:BoundField DataField="StackRatingString" HeaderText="Stack Rating" />
					<asp:BoundField DataField="CommandMuString" HeaderText="Cmd Mu" />
					<asp:BoundField DataField="CommandSigmaString" HeaderText="Cmd Sigma" />
					<asp:BoundField DataField="CommandRankString" HeaderText="Cmd Rank" />
					<asp:BoundField DataField="CommandWins" HeaderText="Cmd Wins" />
					<asp:BoundField DataField="CommandLosses" HeaderText="Cmd Losses" />
					<asp:BoundField DataField="CommandDraws" HeaderText="Cmd Draws" />
					<asp:BoundField DataField="Kills" HeaderText="Kills" />
					<asp:BoundField DataField="Ejects" HeaderText="Ejects" />
					<asp:BoundField DataField="DroneKills" HeaderText="Drn Kills" />
					<asp:BoundField DataField="StationKills" HeaderText="Stn Kills" />
					<asp:BoundField DataField="StationCaptures" HeaderText="Stn Caps" />
					<asp:BoundField DataField="KillsEjectsRatio" HeaderText="Kills / Ejects" />
					<asp:BoundField DataField="HoursPlayedString" HeaderText="Hrs. Played" />
					<asp:BoundField DataField="KillsPerHour" HeaderText="Kills / Hr." />
                    <asp:BoundField DataField="PRank" HeaderText="P-Rank" />
                    <asp:BoundField DataField="Xp" HeaderText="Xp" />
				</Columns>
			</asp:GridView>
		</div>
		
		
	
	<script type="text/javascript" language="javascript">
		$(document).ready(function () {

			$("td.primaryColumn").each(function (index) {
				$(this).attr("sorttable_customkey", $(this).html().toLowerCase());
			});

		});

		//jquery.fixedtableheader-1-0-2.min.js
//		$(document).ready(function() {
//			$('.fullWidthGrid').fixedtableheader();
		//		});



		//jquery.fixedheadertable.1.0.js
//		$(document).ready(function()
//		{
//			$(".sortable").tablesorter();
//			$('.table1').fixedHeaderTable();
		//		});

//		$(document).ready(function()
//		{
//			$(".table2").jqGrid({
//				height: 350,
//				datatype: 'json',
//				loadonce: true,
//				url: "LeaderboardData.aspx",
//				colModel: [
//					{ name: 'Order', index: 'Order', width: 55 },
//					{ name: 'Place', index: 'Place', width: 55 },
//					{ name: 'Callsign', index: 'Callsign', width: 100 }
//					],
//				rownumbers: true
//			});

//		}); 
//	
		
		

	</script>
</asp:Content>
