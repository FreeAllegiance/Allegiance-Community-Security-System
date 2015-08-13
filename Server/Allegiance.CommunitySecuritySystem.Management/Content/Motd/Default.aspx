<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Content.Motd.Default" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" >
	</asp:ScriptManagerProxy>
	
	<%--<script src="../../Scripts/jquery-1.4.2.min.js" type="text/javascript" language="javascript"></script>--%>
	<script src="../../Scripts/FAZ-glowtext.js" type="text/javascript" language="javascript"></script>
	
	<table class="fullWidthTable" style="width: 100%; border-spacing: 5px;">
	<%-- 
		<tr>
			<td colspan="2">
				<div style="width: 100%; overflow-x: scroll;">
					<asp:Repeater ID="rptImagePick" runat="server" 
						onitemdatabound="rptImagePick_ItemDataBound" >
						<HeaderTemplate><table><tr></HeaderTemplate>
						<ItemTemplate><td><asp:Image runat="server" ID="imgBackground" OnDataBinding="imgBackground_OnDataBinding" Height="50px" Width="50px" /></td></ItemTemplate>
						<FooterTemplate></tr></table></FooterTemplate>
					</asp:Repeater>
				</div>
			</td>
		</tr>
	--%>
		<tr style="vertical-align: middle;">
			<td class="label">
				Editing MOTD for Lobby
			</td>
			<td>
			
				<asp:DropDownList ID="ddlLobby" runat="server" EnableViewState="true" AutoPostBack="true" onchange="if (modificationCheck(this) == false) return false;" 
					onselectedindexchanged="ddlLobby_SelectedIndexChanged"></asp:DropDownList>
			&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:Label ID="lblUpdateStatus" runat="server" CssClass="errorText" 
					Text="MOTD for lobby was updated." Visible="False"></asp:Label>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<hr />
			</td>
		</tr>
		<tr style="vertical-align: middle;">
			<td class="label">
				Logo Image
			</td>
			<td>
				<table><tr style="vertical-align: middle;"><td>
				<asp:DropDownList ID="ddlLogo" EnableViewState="true" runat="server" onchange="updatePreviewPicture(this)" 
						onkeyup="updatePreviewPicture(this)">
				</asp:DropDownList>
				</td>
				<td style="height: 110px; padding-left: 5px;">
					<img id="imgLogo" style="width: 100px; height: 100px;" alt="Logo"/>
				</td></tr></table>
			</td>
		</tr>
		<tr>
			<td class="label">
				Banner Text
			</td>
			<td>
				<asp:TextBox ID="txtBanner" runat="server" MaxLength="50" Width="500px" Height="100px" onchange="setModificationFlag()" TextMode="MultiLine" ></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td class="label">
				Primary Heading
			</td>
			<td>
				<asp:TextBox ID="txtPrimaryHeading" runat="server" MaxLength="255" Width="500px" Height="100px" TextMode="MultiLine" onchange="setModificationFlag()"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td class="label">
				Primary Text
			</td>
			<td>
				<asp:TextBox ID="txtPrimaryText" runat="server" Width="500px" Height="100px" TextMode="MultiLine" onchange="setModificationFlag()"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td class="label">
				Secondary Heading
			</td>
			<td>
				<asp:TextBox ID="txtSecondaryHeading" runat="server" MaxLength="255" Width="500px" Height="100px" TextMode="MultiLine" onchange="setModificationFlag()"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td class="label">
				Secondary Text
			</td>
			<td>
				<asp:TextBox ID="txtSecondaryText" runat="server" Width="500px" Height="300px" TextMode="MultiLine" onchange="setModificationFlag()"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td class="label">
				Details
			</td>
			<td>
				<asp:TextBox ID="txtDetails" runat="server" Width="500px" Height="100px" TextMode="MultiLine" onchange="setModificationFlag()"></asp:TextBox>
			</td>
		</tr>
		<tr valign="middle">
			<td class="label">
				Blank Lines At End Of MOTD
			</td>
			<td>
				<span style="position: relative; top: 0px;">
					<asp:TextBox ID="txtPaddingCrCount" runat="server" TextMode="SingleLine"  onchange="setModificationFlag()"></asp:TextBox>
				</span>
				<cc1:NumericUpDownExtender ID="txtPaddingCrCount_NumericUpDownExtender"  Width="50"
					runat="server" TargetControlID="txtPaddingCrCount" Minimum="0" Maximum="100">
				</cc1:NumericUpDownExtender>
			</td>
		</tr>
		<tr>
			<td></td>
			<td><asp:RangeValidator ID="rvPaddingCount" runat="server"  
					ControlToValidate="txtPaddingCrCount" MinimumValue="1" MaximumValue="100" 
					Text="" ErrorMessage="Please specify a value between 1 and 100." 
					EnableTheming="True" Type="Integer"></asp:RangeValidator></td>
		</tr>
		
		<tr>
			<td colspan="2">
				<center>
					<hr />
					<asp:Button ID="btnSave" runat="server" OnClientClick="clearModificationFlag()" onclick="btnSave_Click" Text="Save" />
					&nbsp;&nbsp;&nbsp;
					<input type="button" value="Preview" onclick="showPreview()" />
					&nbsp;&nbsp;&nbsp;
					<input type="button" value="Download" onclick="downloadCurrent()" />
				</center>
			</td>
		</tr>
				
	</table>
	
	<br />
	<br />

	<table id="tblPreview" runat="server" style="display: none; width: 680px; height: 550px; background: white; border: 1px solid black;">
		<tr>
			<td class="modalHeader">MOTD Preview</td>
		</tr>
		<tr>
			<td style="padding: 10px;">
				<div style="position: relative; width: 660px; height: 500px; background-color: black; ">
					<table style="width: 660px; height: 500px; z-index: 5000; position: absolute;">
						<tr valign="middle">
							<td>
								<center><img id="imgPreviewLogo" style="width: 400px; height: 400px;" src="" alt="" /></center>
							</td>
						</tr>
					</table>
					<div style="width: 640px; height: 480px; z-index: 6000; position: absolute; top: 0px;
						left: 0px; color: White; font-family: Verdana; overflow-y: scroll; padding: 10px;">
						<center>
							<span class="glowText h1" id="spanBanner"></span>
							<span class="glowTextOrange h4" id="spanUpdated"></span>
							<br />
							<span class="glowTextOrange h2" id="spanPrimaryHeading"></span>
							<span class="glowText h3" id="spanPrimaryText"></span>
							<span class="glowTextOrange h4" id="spanSecondaryHeading"></span>
							<span class="glowText h5" id="spanSecondaryText"></span>
						</center>
						<br />
						<br />
						<span class="glowText h5" id="spanDetails"></span>
						<span id="spanPadding"></span>
					</div>
				</div>
			</td>
		</tr>
		<tr valign="middle">
			<td style="padding-bottom: 10px;">
				<center>
					<asp:Button ID="btnClosePreview" runat="server" Text="Close Window" />
				</center>
			</td>
		</tr>
	</table>

	<cc1:modalpopupextender id="mpePreview" runat="server" dynamicservicepath="" enabled="True"
		targetcontrolid="txtPreviewPopupControl" dropshadow="true" popupcontrolid="tblPreview"
		backgroundcssclass="modalBackground" CancelControlID="btnClosePreview" >
	</cc1:modalpopupextender>
	
	<asp:HiddenField ID="txtPreviewPopupControl" runat="server" />


<script type="text/javascript" language="javascript">

	var g_hasModifications = false;
	var g_currentLobby = -1;
	
	function setModificationFlag()
	{
		g_hasModifications = true;
	}

	function updatePreviewPicture(ddlLogo)
	{
		var imgLogo = $get("imgLogo");
		imgLogo.src = "<%= Page.ResolveUrl("~/Images/motd") %>/" + ddlLogo.value + ".png";
		
		setModificationFlag();
	}

	function clearModificationFlag()
	{
		g_hasModifications = false;	
	}

	function modificationCheck(ddlLobby)
	{
		if(g_hasModifications == true)
		{
			if(confirm("There are changes on this screen, are you sure you wish to lose these changes?") == false)
			{
				$get("<%= ddlLobby.ClientID %>").value = g_currentLobby;
				return false;
			}
		}
		
		g_currentLobby = $get("<%= ddlLobby.ClientID %>").value;
		
		return true;
	}
	
	function showPreview()
	{
		var tblPreview = $get("<%= tblPreview.ClientID %>");
		tblPreview.style.display = "";
		
		var txtBanner = $get("<%= txtBanner.ClientID %>");
		var txtPrimaryText = $get("<%= txtPrimaryText.ClientID %>");
		var txtPrimaryHeading = $get("<%= txtPrimaryHeading.ClientID %>");
		var txtPrimaryText = $get("<%= txtPrimaryText.ClientID %>");
		var txtSecondaryHeading = $get("<%= txtSecondaryHeading.ClientID %>");
		var txtSecondaryText = $get("<%= txtSecondaryText.ClientID %>");
		var txtDetails = $get("<%= txtDetails.ClientID %>");
		var txtPaddingCrCount = $get("<%= txtPaddingCrCount.ClientID %>");
		
		var spanBanner = $get("spanBanner");
		var spanUpdated = $get("spanUpdated");
		var spanPrimaryHeading = $get("spanPrimaryHeading");
		var spanPrimaryText = $get("spanPrimaryText");
		var spanSecondaryHeading = $get("spanSecondaryHeading");
		var spanSecondaryText = $get("spanSecondaryText");
		var spanDetails = $get("spanDetails");
		var spanPadding = $get("spanPadding");
		
		spanBanner.innerHTML = txtBanner.value.replace(/\n/g, "<br />");
		spanUpdated.innerHTML = "Last Updated: " + new Date().toDateString();
		spanPrimaryHeading.innerHTML = txtPrimaryHeading.value.replace(/\n/g, "<br />");
		spanPrimaryText.innerHTML = txtPrimaryText.value.replace(/\n/g, "<br />");
		spanSecondaryHeading.innerHTML = txtSecondaryHeading.value.replace(/\n/g, "<br />");
		spanSecondaryText.innerHTML = txtSecondaryText.value.replace(/\n/g, "<br />");
		spanDetails.innerHTML = txtDetails.value.replace(/\n/g, "<br />");
		
		var paddingCount = parseInt(txtPaddingCrCount.value);
		
		var padding = "";
		for(var i = 0; i < paddingCount; i++)
			padding += "<br />";
		
		spanPadding.innerHTML = padding;
		
		var ddlLogo = $get("<%= ddlLogo.ClientID %>");
		var imgPreviewLogo = $get("imgPreviewLogo");
		imgPreviewLogo.src = "<%= Page.ResolveUrl("~/Images/motd") %>/" + ddlLogo.value + ".png";
		
		var txtPreviewPopupControl = $get("<%= txtPreviewPopupControl.ClientID %>");
		txtPreviewPopupControl.ModalPopupBehavior.show();
		
		// Must come after the popup show, or centering won't format properly.
		FAZScripts.addGlow(".glowText", "glowForeground", "glowBackground", 1);
		FAZScripts.addGlow(".glowTextOrange", "glowForegroundOrange", "glowBackground", 1);
	}
	
	function downloadCurrent()
	{
		if(g_hasModifications == true)
		{
			if(confirm("There are changes on this screen that will not be included in the download. Continue?") == false)
				return;
		}
		
		var lobbyID = $get("<%= ddlLobby.ClientID %>").value;
		
		document.location.href = "MotdDownloaderIframe.aspx?lobbyID=" + lobbyID;
	}

	function pageLoad()
	{
		updatePreviewPicture($get("<%= ddlLogo.ClientID %>"));
		
		g_currentLobby = $get("<%= ddlLobby.ClientID %>").value;
		g_hasModifications = false;
	}
</script>

</asp:Content>
