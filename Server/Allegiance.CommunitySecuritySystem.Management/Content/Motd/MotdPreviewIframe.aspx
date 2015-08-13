<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/NoChrome.Master"
	AutoEventWireup="true" CodeBehind="MotdPreviewIframe.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Content.Motd.MotdPreviewIframe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<%--<script src="../../Scripts/jquery-1.4.2.min.js" type="text/javascript" language="javascript"></script>--%>
	<script src="../../Scripts/FAZ-glowtext.js" type="text/javascript" language="javascript"></script>

	<asp:ScriptManager ID="ScriptManager1" runat="server">
	</asp:ScriptManager>
	
	<div style="position: relative; width: 660px; height: 500px; background-color: Black;">
		<table style="width: 660px; height: 500px; z-index: 5000; position: absolute;">
			<tr valign="middle">
				<td>
					<center><asp:Image ID="imgLogo" runat="server" style="width: 400px; height: 400px;" /></center>
				</td>
			</tr>
		</table>
		<div style="width: 640px; height: 480px; z-index: 6000; position: absolute; top: 0px;
			left: 0px; color: White; font-family: Verdana; overflow-y: scroll; padding: 10px;">
			<center>
				<span class="glowText h1"><asp:Label ID="lblBanner" runat="server" Text=""></asp:Label></span>
				<span class="glowTextOrange h4">Last Updated: <asp:Label ID="lblUpdated" runat="server" Text=""></asp:Label></span>
				<br />
				<span class="glowTextOrange h2"><asp:Label ID="lblPrimaryHeading" runat="server" Text=""></asp:Label></span>
				<span class="glowText h3"><asp:Label ID="lblPrimaryText" runat="server" Text=""></asp:Label></span>
				<span class="glowTextOrange h4"><asp:Label ID="lblSecondaryHeading" runat="server" Text=""></asp:Label></span>
				<span class="glowText h5"><asp:Label ID="lblSecondaryText" runat="server" Text=""></asp:Label></span>
			</center>
			<br />
			<br />
			<span class="glowText h5"><asp:Label ID="lblDetails" runat="server" Text=""></asp:Label></span>
			<asp:Label ID="lblPadding" runat="server" Text=""></asp:Label>
		</div>
	</div>
	
	<center>

	</center>
	
	
	<script type="text/javascript" language="javascript">
		FAZScripts.addGlow(".glowText", "glowForeground", "glowBackground", 1);
		FAZScripts.addGlow(".glowTextOrange", "glowForegroundOrange", "glowBackground", 1);
	</script>
</asp:Content>
