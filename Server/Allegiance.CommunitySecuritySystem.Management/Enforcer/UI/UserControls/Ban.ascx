<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Ban.ascx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Enforcer.UI.UserControls.Ban" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" >
	<Services>
		<asp:ServiceReference Path="~/AjaxProviders/Enforcer.svc" />
	</Services>
</asp:ScriptManagerProxy>
	
<asp:GridView ID="gvPlayers" runat="server" AutoGenerateColumns="False" 
	CellPadding="4" ForeColor="#333333" GridLines="None">
	<RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
	<Columns>
		<asp:BoundField DataField="LoginId" DataFormatString="{0}" HeaderText="LoginId" 
			Visible="False" />
		<asp:BoundField DataField="Callsign" HeaderText="Player Callsign">
		<ItemStyle Width="250px" />
		</asp:BoundField>
		<asp:ImageField DataAlternateTextField="IsBanned" DataImageUrlField="BanImage" 
			HeaderText="Banned" ItemStyle-CssClass="markerImage">
			<ItemStyle Width="50px" />
		</asp:ImageField>
		
		<asp:TemplateField HeaderText="Ban">
			<ItemTemplate>
				<asp:HyperLink ID="hlBan" runat="server" 
					NavigateUrl='<%# String.Format("javascript:showBan(\"{1}\", {2})", Eval("LoginId"), Eval("Callsign"), ((bool) Eval("IsBanned")).ToString().ToLower()) %>' 
					Text='<%# String.Format("{0}", ((bool) Eval("IsBanned")) == true ? ((bool) Eval("CanBeUnbanned")) == true ? "Unban" : "" : "Ban") %>'></asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		
		<asp:TemplateField HeaderText="Info">
			<ItemTemplate>
				<asp:HyperLink ID="hlInfo" runat="server" 
					NavigateUrl='<%# String.Format("javascript:showInfo(\"{0}\")", Eval("Callsign")) %>' 
					Text='Info'></asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
	
		<asp:TemplateField HeaderText="IP" Visible="false">
			<ItemTemplate>
				<a href="IPReport.aspx?loginID=<%# Eval("LoginId") %>&searchText=<%= HttpContext.Current.Server.UrlEncode(SearchText) %>">IPs</a>
			</ItemTemplate>
		</asp:TemplateField>
			
	</Columns>
	<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
	<PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
	<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
	<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
	<EditRowStyle BackColor="#999999" />
	<AlternatingRowStyle BackColor="White" ForeColor="#284775" />
	
</asp:GridView>

<asp:Panel id="pUnban" runat="server" style="display: none; border: 1px solid black; background: white;">

		<table cellpadding="5" style="width: 500px; height: 230px;">
			<tr><td colspan="2" class="modalHeader">Unban: <span id="spanUnbanUsername"></span></td></tr>
			<tr><td colspan="2">&nbsp;</td></tr>
			<tr>
				<td nowrap>Ban Reason:
				</td>
				<td style="width: 100%">
					<span id="spanUnbanReason"></span>
				</td>
			</tr>
			<tr>
				<td nowrap>Ban Duration:
				</td>
				<td style="width: 100%">
					<span id="spanUnbanDuration"></span>
				</td>
			</tr>
			<tr>
				<td nowrap>Ban Time Remaining:
				</td>
				<td style="width: 100%">
					<span id="spanUnbanTimeRemaining"></span>
				</td>
			</tr>
			<tr valign="bottom">
				<td colspan="2" style="height: 100%;">
				<center><asp:Button runat="server" Text="Remove Ban" id="btnUnban" 
						onclick="btnUnban_Click"/>
				&nbsp;&nbsp;&nbsp;
				<input type="button" onclick="onUnbanCancel()" value="Cancel" /></center>
				</td>
			</tr>
		</table>
</asp:Panel>


<asp:Panel id="pBan" runat="server" style="display: none; border: 1px solid black; background: white;">
	<table style="width: 500px;">
	<tr><td class="modalHeader">Ban: <span id="spanUsername"></span></td></tr>
	<tr>
	<td>
	<cc1:TabContainer ID="tcBanTypes" runat="server" style="padding: 10px;"  Height="250px"
		ActiveTabIndex="0">
		<cc1:TabPanel HeaderText="Auto" runat="server" ID="tpAuto" >
			<ContentTemplate>
				
					<table cellpadding="5" style="width: 100%; height: 176px; ">
						<tr>
							<td nowrap>Ban Reason:
							</td>
							<td style="width: 100%">
							<asp:DropDownList runat="server" id="ddlAutoBanReason" onChange="onAutoBanReasonChange(this)"></asp:DropDownList>
							</td>
						</tr>
						<tr id="trBanDuration" style="visibility: hidden;">
							<td nowrap>Ban Duration:
							</td>
							<td style="width: 100%">
								<span id="spanBanDuration"></span>
							</td>
						</tr>
						<tr valign="bottom">
							<td colspan="2" style="height: 100%;">
							<center><asp:Button runat="server" Text="Apply Ban" id="btnApplyAutoBan" 
									onclick="btnApplyAutoBan_Click" />
							&nbsp;&nbsp;&nbsp;
							<input type="button" onclick="onBanCancel()" value="Cancel" /></center>
							</td>
						</tr>
					</table>
				
			</ContentTemplate>
		</cc1:TabPanel>
		
		<cc1:TabPanel HeaderText="Manual" runat="server" ID="tpManual" TabIndex="1"  >
			<ContentTemplate>
				
					<table cellpadding="5" style="width: 100%; height: 120px;">
						<tr>
							<td nowrap>Ban Reason:
							</td>
							<td style="width: 100%;">
							<asp:TextBox ID="txtBanReason" runat="server" Width="300px" ValidationGroup="vgManualBan" MaxLength="100"></asp:TextBox><asp:RequiredFieldValidator
								ID="rfvBanReason" runat="server" ErrorMessage="Please specify a ban reason." ValidationGroup="vgManualBan" Text="*" ControlToValidate="txtBanReason"></asp:RequiredFieldValidator>
							</td>
						</tr>
						<tr valign="middle">
							<td nowrap>Ban Duration:
							</td>
							<td>
								<table cellpadding="5">
									<tr>
										<td>
											Years
										</td>
										<td>
											Months
										</td>
										<td>
											Days
										</td>
										<td>
											Hours
										</td>
										<td>
											Minutes
										</td>
										<td></td>
									</tr>
									<tr>
										<td>
											<asp:DropDownList ID="ddlManualBanYears" runat="server" onchange="updateManualTimeSelection()">
												<asp:ListItem Text="0" Value="0" Selected="True" />
												<asp:ListItem Text="1" Value="1" />
												<asp:ListItem Text="2" Value="2" />
												<asp:ListItem Text="3" Value="3" />
												<asp:ListItem Text="4" Value="4" />
												<asp:ListItem Text="5" Value="5" />
												<asp:ListItem Text="6" Value="6" />
												<asp:ListItem Text="7" Value="7" />
												<asp:ListItem Text="8" Value="8" />
												<asp:ListItem Text="9" Value="9" />
												<asp:ListItem Text="10" Value="10" />
											</asp:DropDownList>
										</td>
										<td>
											<asp:DropDownList ID="ddlManualBanMonths" runat="server" onchange="updateManualTimeSelection()">
												<asp:ListItem Text="0" Value="0" Selected="True" />
												<asp:ListItem Text="1" Value="1" />
												<asp:ListItem Text="2" Value="2" />
												<asp:ListItem Text="3" Value="3" />
												<asp:ListItem Text="4" Value="4" />
												<asp:ListItem Text="5" Value="5" />
												<asp:ListItem Text="6" Value="6" />
												<asp:ListItem Text="7" Value="7" />
												<asp:ListItem Text="8" Value="8" />
												<asp:ListItem Text="9" Value="9" />
												<asp:ListItem Text="10" Value="10" />
												<asp:ListItem Text="11" Value="11" />
											</asp:DropDownList>
										</td>
										<td>
											<asp:DropDownList ID="ddlManualBanDays" runat="server" onchange="updateManualTimeSelection()">
												<asp:ListItem Text="0" Value="0" Selected="True" />
												<asp:ListItem Text="1" Value="1" />
												<asp:ListItem Text="2" Value="2" />
												<asp:ListItem Text="3" Value="3" />
												<asp:ListItem Text="4" Value="4" />
												<asp:ListItem Text="5" Value="5" />
												<asp:ListItem Text="6" Value="6" />
												<asp:ListItem Text="7" Value="7" />
												<asp:ListItem Text="8" Value="8" />
												<asp:ListItem Text="9" Value="9" />
												<asp:ListItem Text="10" Value="10" />
												<asp:ListItem Text="11" Value="11" />
												<asp:ListItem Text="12" Value="12" />
												<asp:ListItem Text="13" Value="13" />
												<asp:ListItem Text="14" Value="14" />
												<asp:ListItem Text="15" Value="15" />
												<asp:ListItem Text="16" Value="16" />
												<asp:ListItem Text="17" Value="17" />
												<asp:ListItem Text="18" Value="18" />
												<asp:ListItem Text="19" Value="19" />
												<asp:ListItem Text="20" Value="20" />
												<asp:ListItem Text="21" Value="21" />
												<asp:ListItem Text="22" Value="22" />
												<asp:ListItem Text="23" Value="23" />
												<asp:ListItem Text="24" Value="24" />
												<asp:ListItem Text="25" Value="25" />
												<asp:ListItem Text="26" Value="26" />
												<asp:ListItem Text="27" Value="27" />
												<asp:ListItem Text="28" Value="28" />
												<asp:ListItem Text="29" Value="29" />
												<asp:ListItem Text="30" Value="30" />
											</asp:DropDownList>
										</td>
										<td>
											<asp:DropDownList ID="ddlManualBanHours" runat="server" onchange="updateManualTimeSelection()">
												<asp:ListItem Text="0" Value="0" Selected="True" />
												<asp:ListItem Text="1" Value="1" />
												<asp:ListItem Text="2" Value="2" />
												<asp:ListItem Text="3" Value="3" />
												<asp:ListItem Text="4" Value="4" />
												<asp:ListItem Text="5" Value="5" />
												<asp:ListItem Text="6" Value="6" />
												<asp:ListItem Text="7" Value="7" />
												<asp:ListItem Text="8" Value="8" />
												<asp:ListItem Text="9" Value="9" />
												<asp:ListItem Text="10" Value="10" />
												<asp:ListItem Text="11" Value="11" />
												<asp:ListItem Text="12" Value="12" />
												<asp:ListItem Text="13" Value="13" />
												<asp:ListItem Text="14" Value="14" />
												<asp:ListItem Text="15" Value="15" />
												<asp:ListItem Text="16" Value="16" />
												<asp:ListItem Text="17" Value="17" />
												<asp:ListItem Text="18" Value="18" />
												<asp:ListItem Text="19" Value="19" />
												<asp:ListItem Text="20" Value="20" />
												<asp:ListItem Text="21" Value="21" />
												<asp:ListItem Text="22" Value="22" />
												<asp:ListItem Text="23" Value="23" />
											</asp:DropDownList>
										</td>
										<td>
											<asp:DropDownList ID="ddlManualBanMinutes" runat="server" onchange="updateManualTimeSelection()">
												<asp:ListItem Text="0" Value="0" Selected="True" />
												<asp:ListItem Text="5" Value="5" />
												<asp:ListItem Text="10" Value="10" />
												<asp:ListItem Text="15" Value="15" />
												<asp:ListItem Text="20" Value="20" />
												<asp:ListItem Text="25" Value="25" />
												<asp:ListItem Text="30" Value="30" />
												<asp:ListItem Text="35" Value="35" />
												<asp:ListItem Text="40" Value="40" />
												<asp:ListItem Text="45" Value="45" />
												<asp:ListItem Text="50" Value="50" />
												<asp:ListItem Text="55" Value="55" />
											</asp:DropDownList>
										</td>
										<%--<td><asp:RequiredFieldValidator ID="rfvTimeComposite" runat="server" ErrorMessage="Please select a time from the drop down lists."
									Text="*" ValidationGroup="vgManualBan" ControlToValidate="txtManualTimeComposite"></asp:RequiredFieldValidator></td>--%>
									</tr>
								</table>
								<asp:TextBox ID="txtManualTimeComposite" runat="server" ValidationGroup="vgManualBan" style="display: none;"></asp:TextBox>
							</td>
						</tr>
						<tr><td colspan="2" class="errorText">
							<div style="height: 90px;">
							&nbsp;<asp:ValidationSummary ID="vsManualBanValidationSummary" ValidationGroup="vgManualBan" runat="server" />
							</div>
						</td></tr>
						<tr valign="bottom">
							<td colspan="2">
							<center><asp:Button runat="server" Text="Apply Ban" id="btnApplyManualBan" 
									ValidationGroup="vgManualBan" onclick="btnApplyManualBan_Click" />
							&nbsp;&nbsp;&nbsp;
							<input type="button" onclick="onBanCancel()" value="Cancel" /></center>
							</td>
						</tr>
					</table>
					<br />
					<br />
					
				
			</ContentTemplate>
		</cc1:TabPanel>
		</cc1:TabContainer>
		</td>
		</tr>
		</table>
	
	
	<%-- 
	<cc1:ComboBox ID="ccbBanReason" runat="server" CssClass="comboBox" style="z-index: 200000;">
	</cc1:ComboBox>
	--%>
</asp:Panel>


<asp:Panel id="pInfo" runat="server" style="display: none; border: 1px solid black; width: 600px; height1: 320px; background: white;">
	<div class="modalHeader">Player Info: <span id="spanPlayerInfoCallsign"></span></div>
	<br />
	<div style="height1: 300px; padding-left: 10px; padding-right: 10px; padding-bottom: 10px;">
		<table style="width: 100%;">
			<tr class="infocardHeader">
				<td style="width: 200;">Callsign</td>
				<td></td>
				<td>Token</td>
				<td>Tag</td>
			</tr>
			<tr>
				<td id="tdInfoCallsign"></td>
				<td></td>
				<td id="tdInfoToken"></td>
				<td id="tdInfoTag"></td>
			</tr>
			<tr>
				<td colspan="4">&nbsp;</td>
			</tr>
			<tr class="infocardHeader">
				<td colspan="2">Last Login</td>
				<td>Status</td>
				<td>Aliases</td>
			</tr>
			<tr valign="top">
				<td id="tdInfoLastLogin"  colspan="2"></td>
				<td id="tdInfoStatus"></td>
				<td id="tdInfoAliases"></td>
			</tr>
			<tr>
				<td colspan="4">&nbsp;</td>
			</tr>
			<tr class="infocardHeader">
				<td colspan="4">Last Ban</td>
			</tr>
			<tr>
				<td colspan="4"><hr /></td>
			</tr>
			<tr valign="top">
				<td  id="tdInfoLastBanReason" colspan="2" style="padding-right: 10px;"></td>
				<td colspan="2" id="tdInfoBanDetails">Banned On: <span id="spanInfoLastBanTime"></span><br />Duration: <span id="spanInfoLastBanDuration"></span><br />Banned By: <span id="spanInfoLastBanReasonUser"></span></td>
			</tr>
			<tr>
				<td colspan="4">&nbsp;</td>
			</tr>
			<tr valign="bottom">
				<td colspan="4" style="height: 100%; text-align: center;">
					<asp:Button id="btnClosePlayerInfo" Text="Close" runat="server" />
				</td>
			</tr>
		</table>
	</div>
	
</asp:Panel>


<cc1:ModalPopupExtender ID="mpeBan" runat="server" DynamicServicePath="" 
	Enabled="True" TargetControlID="txtBanPopupControl" DropShadow="true" 
	PopupControlID="pBan" BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>

<cc1:ModalPopupExtender ID="mpeUnban" runat="server" DynamicServicePath="" 
	Enabled="True" TargetControlID="txtUnbanPopupControl" DropShadow="true" 
	PopupControlID="pUnban" BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>

<cc1:ModalPopupExtender ID="mpeInfo" runat="server" DynamicServicePath="" 
	Enabled="True" TargetControlID="txtInfoPopupControl" DropShadow="true" 
	PopupControlID="pInfo" BackgroundCssClass="modalBackground" CancelControlID="btnClosePlayerInfo">
</cc1:ModalPopupExtender>

<asp:HiddenField ID="txtBanPopupControl" runat="server" />
<asp:HiddenField ID="txtUnbanPopupControl" runat="server" />
<asp:HiddenField ID="txtInfoPopupControl" runat="server" />
<asp:HiddenField ID="txtCallsign" runat="server" />

<%--  

TODO: BT 5/27/2010 - Convert all ASP.Net modal popup extenders to jqueryUI modal dialogs.
Sample code below:

	<div id="test" title="I am a TITLE!" style="display: none;">AAA</div>
	<script src="../Scripts/jquery-ui-1.8.1.custom.min.js" type="text/javascript"></script>
	
	<script type="text/javascript" language="javascript">
	
		$("#test").dialog({
			modal: true,
			draggable: false,
			resizable: false,
			buttons: {
				Ok: function()
				{
					$(this).dialog('close');
				}
			}
		});

	</script>

--%>


<script language="javascript" type="text/javascript">

	var g_enforcer = new AjaxProviders.Enforcer();
	//var g_currentLoginId = 0;
	
	function updateManualTimeSelection()
	{
		var ddlManualBanYears = $get("<%= ddlManualBanYears.ClientID %>");
		var ddlManualBanMonths = $get("<%= ddlManualBanMonths.ClientID %>");
		var ddlManualBanDays = $get("<%= ddlManualBanDays.ClientID %>");
		var ddlManualBanHours = $get("<%= ddlManualBanHours.ClientID %>");
		var ddlManualBanMinutes = $get("<%= ddlManualBanMinutes.ClientID %>");
		var txtManualTimeComposite = $get("<%= txtManualTimeComposite.ClientID %>");
		
		txtManualTimeComposite.value = "";
		
		if(parseInt(ddlManualBanYears.value) > 0)
			txtManualTimeComposite.value = "1";
			
		if(parseInt(ddlManualBanMonths.value) > 0)
			txtManualTimeComposite.value = "1";
			
		if(parseInt(ddlManualBanDays.value) > 0)
			txtManualTimeComposite.value = "1";
			
		if(parseInt(ddlManualBanHours.value) > 0)
			txtManualTimeComposite.value = "1";
			
		if(parseInt(ddlManualBanMinutes.value) > 0)
			txtManualTimeComposite.value = "1";
	}

	function showBan(callsign, isBanned)
	{
		//g_currentLoginId = loginId;
		
		var txtCallsign = $get("<%= txtCallsign.ClientID %>");
		txtCallsign.value = callsign;
		
		if(isBanned == false)
		{
			var txtBanPopupControl = $get("<%= txtBanPopupControl.ClientID %>");
			txtBanPopupControl.ModalPopupBehavior.show();

			var spanUsername = $get("spanUsername");
			spanUsername.innerHTML = callsign;

			var ddlAutoBanReason = $get("<%= ddlAutoBanReason.ClientID %>");
			onAutoBanReasonChange(ddlAutoBanReason);
		}
		else
		{
			var spanUnbanUsername = $get("spanUnbanUsername");
			spanUnbanUsername.innerHTML = callsign;

			var txtUnbanPopupControl = $get("<%= txtUnbanPopupControl.ClientID %>");
			txtUnbanPopupControl.ModalPopupBehavior.show();
			
			updateUnbanInfo();
		}
		
<%-- 
		var ccbBanReason = $get("<%= ccbBanReason.ClientID %>");
		ccbBanReason.control.get_optionListControl().PopupBehavior.add_shown(fixComboboxZOrder);
--%>
	}
	
	function onBanCancel()
	{
		var txtBanPopupControl = $get("<%= txtBanPopupControl.ClientID %>");
		txtBanPopupControl.ModalPopupBehavior.hide();
	}
	
	function onUnbanCancel()
	{
		var txtUnbanPopupControl = $get("<%= txtUnbanPopupControl.ClientID %>");
		txtUnbanPopupControl.ModalPopupBehavior.hide();
	}
	
	function showInfo(callsign)
	{
		g_enforcer.GetPlayerInfo(callsign, onGetPlayerInfoSuccess, onGetPlayerInfoFail);
	}
	
	function onGetPlayerInfoSuccess(result)
	{
		var spanPlayerInfoCallsign = $get("spanPlayerInfoCallsign");
		spanPlayerInfoCallsign.innerHTML = result.Callsign;
		
		var tdInfoCallsign = $get("tdInfoCallsign");
		var tdInfoToken = $get("tdInfoToken");
		var tdInfoTag = $get("tdInfoTag");
		var tdInfoStatus = $get("tdInfoStatus");
		var tdInfoLastLogin = $get("tdInfoLastLogin");
		var spanInfoLastBanTime = $get("spanInfoLastBanTime");
		var tdInfoLastBanReason = $get("tdInfoLastBanReason");
		var spanInfoLastBanReasonUser = $get("spanInfoLastBanReasonUser");
		var spanInfoLastBanDuration = $get("spanInfoLastBanDuration");
		var tdInfoBanDetails = $get("tdInfoBanDetails");
		var tdInfoAliases = $get("tdInfoAliases");
		
		tdInfoCallsign.innerHTML = result.Callsign;
		tdInfoToken.innerHTML = result.Token;
		tdInfoTag.innerHTML = result.Tag;
		tdInfoStatus.innerHTML = result.Status;
		tdInfoLastLogin.innerHTML = result.LastLogin;
		spanInfoLastBanTime.innerHTML = result.LastBanTime;
		tdInfoLastBanReason.innerHTML = result.LastBanReason;
		spanInfoLastBanReasonUser.innerHTML = result.LastBanUser;
		spanInfoLastBanDuration.innerHTML = result.LastBanDuration;
		
		var aliases = "";
		for(var i = 0; i < result.Aliases.length; i++)
		{
			var alias = result.Aliases[i];
			if(result.DefaultAlias != null && alias.toLowerCase() == result.DefaultAlias.toLowerCase())
				alias += " (Primary)";
		
			aliases += alias + "<br />";
		}
		
		tdInfoAliases.innerHTML = aliases;
		
		if(result.LastBanReason == "None")
			tdInfoBanDetails.style.visibility = "hidden";
		else
			tdInfoBanDetails.style.visibility = "";
		
		var txtInfoPopupControl = $get("<%= txtInfoPopupControl.ClientID %>");
		txtInfoPopupControl.ModalPopupBehavior.show();
	}
	
	function onGetPlayerInfoFail()
	{
		alert("Couldn't retrieve info for callsign!");
	}

<%-- 
	// modal dialog extender does not play nice with other controls.
	function fixComboboxZOrder() 
	{
		var ccbBanReason = $get("<%= ccbBanReason.ClientID %>");
		ccbBanReason.control.get_optionListControl().style.zIndex = 200000;
		//test.style.zIndex
	}
--%>

	function onAutoBanReasonChange(ddlAutoBanReason)
	{
		var selectedValue = parseInt(ddlAutoBanReason.value);
		
		if(selectedValue > 0)
		{
			updateBanTimes();
		}
		else
		{
			var trBanDuration = $get("trBanDuration");
			trBanDuration.style.visibility = "hidden";
			$get("<%= btnApplyAutoBan.ClientID  %>").disabled = true;
		}
	}
	
	function updateBanTimes()
	{
		var ddlAutoBanReason = $get("<%= ddlAutoBanReason.ClientID %>");
		var selectedValue = parseInt(ddlAutoBanReason.value);
		var txtCallsign = $get("<%= txtCallsign.ClientID %>");
		
		if(selectedValue != 0)
		{
			g_enforcer.GetAutoBanDuration(txtCallsign.value, selectedValue, onUpdateBanTimesSuccess, onUpdateBanTimesFail);
		}
	}
	
	function onUpdateBanTimesSuccess(result)
	{
		var spanBanDuration = $get("spanBanDuration");
		spanBanDuration.innerHTML = result;
		
		var trBanDuration = $get("trBanDuration");
		trBanDuration.style.visibility = "";
		
		$get("<%= btnApplyAutoBan.ClientID  %>").disabled = false;
	}
	
	function onUpdateBanTimesFail(result)
	{
		var trBanDuration = $get("trBanDuration");
		trBanDuration.style.visibility = "hidden";
		
		$get("<%= btnApplyAutoBan.ClientID  %>").disabled = true;
		
		alert("There was a server error retrieving ban duration.");
	}
	
	function updateUnbanInfo()
	{
		var txtCallsign = $get("<%= txtCallsign.ClientID %>");
		
		g_enforcer.GetBanInfo(txtCallsign.value, onGetBanInfoSuccess, onGetBanInfoFail);
	}
	
	function onGetBanInfoSuccess(result)
	{
		var spanUnbanReason = $get("spanUnbanReason");
		var spanUnbanDuration = $get("spanUnbanDuration");
		var spanUnbanTimeRemaining = $get("spanUnbanTimeRemaining");
		
		spanUnbanReason.innerHTML = result.BanReason;
		spanUnbanDuration.innerHTML = result.TotalTime;
		spanUnbanTimeRemaining.innerHTML = result.TimeRemaining;
	}
	
	function onGetBanInfoFail(result)
	{
		alert("There was a server error retrieving ban information: " + result);
	}
</script>		