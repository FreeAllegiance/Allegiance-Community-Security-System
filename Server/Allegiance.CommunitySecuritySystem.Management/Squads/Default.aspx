<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Squads.Default" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<script src="<%= Page.ResolveUrl("~/Scripts/jquery.maxlength.min.js") %>" type="text/javascript"></script>
	<script src="<%= Page.ResolveUrl("~/Scripts/jquery.watermark.min.js") %>" type="text/javascript"></script>

	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" >
		<Services>
			<asp:ServiceReference Path="~/AjaxProviders/Squads.svc" />
		</Services>
	</asp:ScriptManagerProxy>

	<asp:Label ID="lblErrorMessage" runat="server" Text="" CssClass="errorText"></asp:Label>

	<asp:Panel ID="pNoSquadsAvailable" runat="server">
		<h3>You have not been assigned to any squads yet.</h3>
	</asp:Panel>

	<asp:Panel ID="pSquadList" runat="server">
		<table style="width: 100%"><tr><td style="width: 100%">Members</td><td style="white-space: nowrap;">Available Squads: 
		<asp:DropDownList ID="ddlSquads" runat="server" AutoPostBack="true" 
			onselectedindexchanged="ddlSquads_SelectedIndexChanged" >
		</asp:DropDownList></td></tr></table>
		<hr />
		<asp:GridView ID="gvMembers" runat="server" AutoGenerateColumns="False" 
			CssClass="fullWidthGrid" onrowdatabound="gvMembers_RowDataBound">
			<Columns>
				<asp:TemplateField HeaderText="Callsign">
					<ItemTemplate>
						<div style="width: 15px; text-align: right; float: left;">&nbsp;<asp:Label ID="Label2" runat="server" Text='<%# Bind("Token") %>'></asp:Label></div><asp:Label ID="Label1" runat="server" Text='<%# Bind("Callsign") %>'></asp:Label>
					</ItemTemplate>
					<ItemStyle CssClass="primaryColumn" />
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Role Name">
					<ItemTemplate>
						<asp:DropDownList ID="ddlRoles" runat="server" DataTextField="Name" 
							DataValueField="Id" AutoPostBack="True" 
							onselectedindexchanged="ddlRoles_SelectedIndexChanged" >
						</asp:DropDownList>
						<asp:Label ID="lblRoleName" runat="server" Text="" Visible="false"></asp:Label>
						<asp:HiddenField ID="txtGroupID" runat="server" />
						<asp:HiddenField ID="txtAliasID" runat="server" />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Remove">
					<ItemTemplate>
						<asp:Panel runat="server" ID="pRemoveLink">
							<a href="javascript:removeCallsign('<%# Eval("Callsign") %>')">Remove</a>
						</asp:Panel>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Message">
					<ItemTemplate>
						<asp:Panel runat="server" ID="pMessageLink">
							<a href="javascript:messageSingleUser('<%# Eval("Callsign") %>')">Message</a>
						</asp:Panel>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Select">
					<ItemTemplate>
						<asp:Panel runat="server" ID="pSelectMember">
							<input type="checkbox" name="chkSelectMember" onchange="setSelectedCallsign(this, '<%# Eval("Callsign") %>')" />
						</asp:Panel>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>
		<br />

		<asp:Panel runat="server" ID="pAslOptions">
			<input type="button" name="btnMessageSelected" id="btnMessageSelected" value="Message Selected Callsigns" onclick="openMessageDialog()" />
			<input type="button" name="btnMessageAll" id="btnMessageAll" value="Message Squad" onclick="messageAllUsers()" />
			<input type="button" name="btnAddCallsign" id="btnAddCallsign" value="Add Callsign" onclick="addCallsign()" />
		</asp:Panel>
	</asp:Panel>
	
	<table id="tblMessage" runat="server" class="modalWindow" style="display: none;">
		<tr>
			<td class="modalHeader">Send Message</td></tr><tr>
			<td style="height: 300px; padding-right: 10px;">
					<table style="width: 100%; height: 100%; padding: 10px;">
						<tr>
							<td colspan="5"><asp:TextBox ID="txtSubject" runat="server" MaxLength="50" style="width: 100%;"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td style="height: 100%;" colspan="5"><asp:TextBox TextMode="MultiLine" ID="txtMessage" runat="server" MaxLength="255" style="height: 250px; width: 100%;"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class="label" style="white-space:nowrap;">
								Send Date
							</td>
							<td style="white-space:nowrap;">
								<asp:TextBox ID="txtSendDate" runat="server"></asp:TextBox><asp:ImageButton style="position: relative; top: 3px;" ID="btnSendDate" runat="server" ImageUrl="~/Images/calendar.png" />
								<cc1:CalendarExtender ID="ceSendDate" runat="server" TargetControlID="txtSendDate" PopupButtonID="btnSendDate" OnClientShown="calendarShown">
								</cc1:CalendarExtender>
							</td>
							<td style="width: 100%;"></td>
							<td class="label" style="white-space:nowrap;">
								Expiration Date
							</td>
							<td style="white-space:nowrap;">
								<asp:TextBox ID="txtExpirationDate" runat="server"></asp:TextBox><asp:ImageButton style="position: relative; top: 3px;" ID="btnExpirationDate" runat="server" ImageUrl="~/Images/calendar.png" />
								<cc1:CalendarExtender ID="ceExpirationDate" runat="server" TargetControlID="txtExpirationDate"  PopupButtonID="btnExpirationDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
							</td>
						</tr>
						<tr>
							<td colspan="5" id="maxLengthTarget" style="padding-top: 10px;"></td>
						</tr>
					</table>
			</td>
		</tr>
		<tr>
			<td style="height: 50px;">
				<center>
					<input id="btnSendMessage" type="button" value="Send Message" onclick="sendMessage()" />
					&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<input id="btnCancelMessage" type="button" value="Cancel Message" onclick="cancelMessage()" />
				</center>
			</td>
		</tr>
	</table>
	
	<cc1:ModalPopupExtender ID="mpeSendMessage" runat="server" DynamicServicePath="" 
		Enabled="True" TargetControlID="txtSendMessagePopupControl" DropShadow="true" 
		PopupControlID="tblMessage" BackgroundCssClass="modalBackground">
	</cc1:ModalPopupExtender>
	
	
	<%--<asp:TextBox ID="testbox" runat="server"></asp:TextBox>
	<cc1:CalendarExtender ID="ceExpirationDate" runat="server" TargetControlID="txtExpirationDate" PopupButtonID="btnExpirationDate"></cc1:CalendarExtender>
--%>
	<asp:HiddenField ID="txtSendMessagePopupControl" runat="server" />
		
	<script language="javascript" type="text/javascript">
		$(document).ready(function ()
		{
			$("#<%= txtMessage.ClientID %>").maxlength(
			{
				max: 255,
				feedbackTarget: $("#maxLengthTarget")
			});

			$("#<%= txtSubject.ClientID %>").watermark("Subject (50 chars max)");
			$("#<%= txtMessage.ClientID %>").watermark("Message (255 chars max)");

		});


		var g_squads = new AjaxProviders.Squads();
		var g_selectedCallsigns = new Array();
		var g_callsignsInArray = new Array();
		//var g_clearAllCheckboxesOnSend = false;
		var g_messageAllUsers = false;
		var g_messageSingleUserCallsign = null;

		// http://stackoverflow.com/questions/1632120/calendarextender-in-modalpopupextender/1956024#1956024
		function calendarShown(sender, args)
		{
			sender._popupBehavior._element.style.zIndex = 100005;
		}


		function pageLoad()
		{
			setAllSelectionCheckBoxes(false);
		}

		function setSelectedCallsign(checkbox, callsign)
		{
			g_selectedCallsigns[callsign] = { callsign: callsign, checkbox: checkbox };
			g_callsignsInArray.push(callsign);
		}

		function setAllSelectionCheckBoxes(checked)
		{
			var allSelectCheckboxes = document.getElementsByName("chkSelectMember");
			for (var i = 0; i < allSelectCheckboxes.length; i++)
			{
				allSelectCheckboxes[i].checked = checked;
				allSelectCheckboxes[i].onchange();
			}
		}

		function messageAllUsers()
		{
			//setAllSelectionCheckBoxes(true);

			//g_clearAllCheckboxesOnSend = true;

			g_messageAllUsers = true;
			
			openMessageDialog();
		}

		function messageSingleUser(callsign)
		{
			g_messageSingleUserCallsign = callsign;

			openMessageDialog();
		}

		function openMessageDialog()
		{
			var txtSendMessagePopupControl = $get("<%= txtSendMessagePopupControl.ClientID %>");
			txtSendMessagePopupControl.ModalPopupBehavior.show();
		}

		function cancelMessage()
		{
			//if (g_clearAllCheckboxesOnSend == true)
			//	setAllSelectionCheckBoxes(false);

			g_messageAllUsers = false;
		
			var txtSendMessagePopupControl = $get("<%= txtSendMessagePopupControl.ClientID %>");
			txtSendMessagePopupControl.ModalPopupBehavior.hide();
		}

		function sendMessage()
		{
			var txtSubject = $get("<%= txtSubject.ClientID %>");

			// || txtSubject.value == txtSubject.TextBoxWatermarkBehavior.get_WatermarkText()
			if (txtSubject.value.length == 0)
			{
				alert("Please specify a subject.");
				return;
			}

			// || txtMessage.value == txtMessage.TextBoxWatermarkBehavior.get_WatermarkText()
			var txtMessage = $get("<%= txtMessage.ClientID %>");
			if (txtMessage.value.length == 0)
			{
				alert("Please specify a message.");
				return;
			}

			if (txtMessage.value.length > 255)
			{
				alert("Message is over size limit: 255 characters max!.");
				return;
			}

			var selectedCallsigns = new Array();

			if (g_messageSingleUserCallsign != null)
			{
				selectedCallsigns.push(g_messageSingleUserCallsign);
				g_messageSingleUserCallsign = null;
			}
			else
			{
				for (var i = 0; i < g_callsignsInArray.length; i++)
				{
					if (g_selectedCallsigns[g_callsignsInArray[i]].checkbox.checked == true)
						selectedCallsigns.push(g_callsignsInArray[i]);
				}
			}

			var txtSendDate = $get("<%= txtSendDate.ClientID %>");
			var txtExpirationDate = $get("<%= txtExpirationDate.ClientID %>");
			var txtSubject = $get("<%= txtSubject.ClientID %>");
			var txtMessage = $get("<%= txtMessage.ClientID %>");

			var btnSendMessag = $get("btnSendMessage");
			btnSendMessag.disabled = true;

			var ddlSquads = $get("<%= ddlSquads.ClientID %>");

			if(g_messageAllUsers == false)
				g_squads.SendMessageToCallsigns(selectedCallsigns, txtSubject.value, txtMessage.value, txtSendDate.value, txtExpirationDate.value, onSendMessageSuccess, onSendMessageFail, null);
			else
				g_squads.SendMessageToGroup(ddlSquads.value, txtSubject.value, txtMessage.value, txtSendDate.value, txtExpirationDate.value, onSendMessageSuccess, onSendMessageFail, null);

			//if (g_clearAllCheckboxesOnSend == true)
			//	setAllSelectionCheckBoxes(false);
		}

		function onSendMessageSuccess(result)
		{
			var btnSendMessag = $get("btnSendMessage");
			btnSendMessag.disabled = false;

			alert("Message sent.");
			
			var txtSendMessagePopupControl = $get("<%= txtSendMessagePopupControl.ClientID %>");
			txtSendMessagePopupControl.ModalPopupBehavior.hide();
			g_messageAllUsers = false;
		}

		function onSendMessageFail(result, exception)
		{
			alert("message send failed: " + exception);
		
			var btnSendMessag = $get("btnSendMessage");
			btnSendMessag.disabled = false;
			g_messageAllUsers = false;
		}

		function addCallsign()
		{
			var ddlSquads = $get("<%= ddlSquads.ClientID %>");

			document.location.href = "AddAlias.aspx?groupID=" + ddlSquads.value;
		}

		function removeCallsign(callsign)
		{
			var ddlSquads = $get("<%= ddlSquads.ClientID %>");

			if (confirm("Are you sure you want to remove " + callsign + " from " + ddlSquads.options[ddlSquads.selectedIndex].text + "?") == false)
				return;

			document.location.href = "Default.aspx?action=delete&callsign=" + escape(callsign) + "&groupID=" + ddlSquads.value;				
		}
	</script>
</asp:Content>
