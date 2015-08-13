<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopNavigation.ascx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.UI.UserControls.TopNavigation" %>

<%--<style type="text/css">
	.mainNav li.AspNet-Menu-WithChildren
	{
		background-image: url(<%= Page.ResolveUrl("~/Images/submenu_indicator.png") %>);
	}
</style>--%>

	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
		<Scripts>
			<asp:ScriptReference Path="~/Scripts/jquery-1.4.2.min.js" ScriptMode="Auto" />
			<asp:ScriptReference Path="~/Scripts/jquery-ui-1.8.1.custom.min.js" ScriptMode="Auto" />
			<asp:ScriptReference Path="~/Scripts/jquery.hoverIntent.minified.js" ScriptMode="Auto" />
			<asp:ScriptReference Path="~/Scripts/superfish.js" ScriptMode="Auto" />
		</Scripts>
	</asp:ScriptManagerProxy>
	
	<%--
<script type="text/javascript" src="<%= Page.ResolveUrl("~/Scripts/jquery-1.4.2.min.js") %>"></script> 
<script type="text/javascript" src="<%= Page.ResolveUrl("~/Scripts/jquery.hoverIntent.minified.js") %>"></script> 
<script type="text/javascript" src="<%= Page.ResolveUrl("~/Scripts/superfish.js") %>"></script> 
 --%>

<ul class="sf-menu sf-js-enabled sf-shadow" id="sample-menu-1">
	<li>
		<a href="<%= Page.ResolveUrl("~/Default.aspx") %>">Home</a>
	</li>
	<li>
		<a href="#">Stats</a>
		<ul style="display: none; visibility: hidden;">
			<li><a href="<%= Page.ResolveUrl("~/Stats/Leaderboard.aspx") %>">Leaderboard</a></li>
			<li><a href="<%= Page.ResolveUrl("~/Stats/Faction.aspx") %>">Faction Statistics</a></li>
			<li><a href="<%= Page.ResolveUrl("~/Stats/SquadRoster.aspx") %>">Squad Roster</a></li>
			<li><a href="<%= Page.ResolveUrl("~/Stats/BanList.aspx?type=mostRecent") %>">Recent Bans</a></li>
			<li><a href="<%= Page.ResolveUrl("~/Stats/BanList.aspx") %>">Ban Countdown</a></li>
			<li><a href="<%= Page.ResolveUrl("~/Stats/PollResults.aspx") %>">Poll Results</a></li>
		</ul>
	</li>
	<li class="moderatorVisible">
		<a href="#">Enforcer</a>
		<ul style="display: none; visibility: hidden;">
			<li><a href="<%= Page.ResolveUrl("~/Enforcer/Default.aspx") %>">Active Users</a></li>
			<li><a href="<%= Page.ResolveUrl("~/Enforcer/Search.aspx") %>">Search All Users</a></li>
			<li class="zoneLeadVisible"><a href="<%= Page.ResolveUrl("~/Enforcer/ServerLogs.aspx") %>">Game Logs</a></li>
		</ul>
	</li>
	<li  class="administratorVisible">
		<a href="#">Content</a>
		<ul style="display: none; visibility: hidden;">
			<li><a href="<%= Page.ResolveUrl("~/Content/AutoUpdate/Default.aspx") %>">Auto Update</a></li>
			<li><a href="<%= Page.ResolveUrl("~/Content/Motd/Default.aspx") %>">Message Of The Day</a></li>
			<li><a href="<%= Page.ResolveUrl("~/Content/Polls/Default.aspx") %>">Manage Polls</a></li>
			<li><a href="<%= Page.ResolveUrl("~/Content/MachineRecordExclusions/Default.aspx") %>">Machine Record Exclusions</a></li>
			<li><a href="<%= Page.ResolveUrl("~/Content/VirtualMachineMarkers/Default.aspx") %>">Virtual Machine Markers</a></li>
			<li><a href="<%= Page.ResolveUrl("~/Content/Groups/Default.aspx") %>">Squads and Groups</a></li>
		</ul>
	</li>
	<li  class="authenticatedVisible">
		<a href="<%= Page.ResolveUrl("~/Squads/Default.aspx") %>">Squads</a>
	</li>
	<li  class="administratorVisible">
		<a href="#">Users</a>
		<ul style="display: none; visibility: hidden;">
			<li><a href="<%= Page.ResolveUrl("~/Users/Default.aspx") %>">Manage Users</a></li>
			<li><a href="<%= Page.ResolveUrl("~/Users/GlobalMessage.aspx") %>">Global Message</a></li>
		</ul>
		
	</li>
	<li  class="authenticatedVisible">
		<a href="#">My Account</a>
		<ul style="display: none; visibility: hidden;">
			<li><a href="<%= Page.ResolveUrl("~/ChangePassword.aspx") %>">Change Password</a></li>
		</ul>
	</li>
</ul>



<script type="text/javascript">

	$(document).ready(function()
	{
		$('#sample-menu-1').superfish({
			autoArrows: true // emulate default behavior for this example
		});

		var showAuthenticatedOptions = <%= ShowAuthenticatedOptions.ToString().ToLower() %>;
		var showModeratorOptions = <%= ShowModeratorOptions.ToString().ToLower() %>;
		var showAdministratorOptions = <%= ShowAdministratorOptions.ToString().ToLower() %>;
		var showZoneLeadOptions = <%= ShowZoneLeadOptions.ToString().ToLower() %>;
		
		if(showAuthenticatedOptions == false)
			$(".authenticatedVisible").hide();
			
		if(showModeratorOptions == false)
			$(".moderatorVisible").hide();
			
		if(showAdministratorOptions == false)
			$(".administratorVisible").hide();

		if(showZoneLeadOptions == false)
			$(".zoneLeadVisible").hide();
	}); 
 
</script>

<%--
<asp:Menu ID="wcMainMenu" runat="server" CssClass="topnavMenu1" 
	Orientation="Horizontal" Width="500px" BackColor="#F7F6F3" 
	DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="0.8em" 
	ForeColor="#7C6F57" StaticSubMenuIndent="10px">
	<StaticSelectedStyle BackColor="#5D7B9D" />
	<StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
	<DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
	<DynamicMenuStyle BackColor="#F7F6F3" />
	<DynamicSelectedStyle BackColor="#5D7B9D" />
	<DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
	<StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
	<Items>
		<asp:MenuItem NavigateUrl="~/Default.aspx" Text="Home" Value="Anonymous">
		</asp:MenuItem>
		<asp:MenuItem Text="Stats"  Value="Anonymous">
			<asp:MenuItem NavigateUrl="~/Stats/Leaderboard.aspx" Text="Leaderboard"></asp:MenuItem>
		</asp:MenuItem>
		<asp:MenuItem Text="Enforcer" Value="Moderator">
			<asp:MenuItem NavigateUrl="~/Enforcer/Default.aspx" Text="Active Users"></asp:MenuItem>
			<asp:MenuItem NavigateUrl="~/Enforcer/Search.aspx" Text="Search All Users"></asp:MenuItem>
		</asp:MenuItem>
		<asp:MenuItem Text="Content" Value="Administrator">
			<asp:MenuItem NavigateUrl="~/Content/AutoUpdate/Default.aspx" Text="Auto Update" Value="Administrator"></asp:MenuItem>
			<asp:MenuItem NavigateUrl="~/Content/Motd/Default.aspx" Text="Message Of The Day" Value="Administrator"></asp:MenuItem>
		</asp:MenuItem>
		<asp:MenuItem NavigateUrl="~/Squads/Default.aspx" Text="Squads">
		</asp:MenuItem>
		<asp:MenuItem NavigateUrl="~/Users/Default.aspx" Text="Users" Value="Administrator"></asp:MenuItem>
		<asp:MenuItem Text="My Account">
			<asp:MenuItem Text="Change Password" NavigateUrl="~/ChangePassword.aspx"></asp:MenuItem>
		</asp:MenuItem>
		<asp:MenuItem NavigateUrl="~/Logout.aspx" Text="Log Out">
		</asp:MenuItem>
	</Items>
</asp:Menu>
--%>