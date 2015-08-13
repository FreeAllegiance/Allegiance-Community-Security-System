<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="GlobalMessage.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Users.GlobalMessage" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<script src="<%= Page.ResolveUrl("~/Scripts/jquery.maxlength.min.js") %>" type="text/javascript"></script>
	<script src="<%= Page.ResolveUrl("~/Scripts/jquery.watermark.min.js") %>" type="text/javascript"></script>

	<input id="btnNewMessage" type="button" value="New Global Message" />
	<hr />
	<asp:GridView ID="gvGlobalMessages" runat="server" AutoGenerateColumns="False" 
		EnableModelValidation="True" CssClass="fullWidthGrid">
		<Columns>
			<asp:BoundField DataField="DateCreated" DataFormatString="{0:MMM-dd-yyyy}" 
				HeaderText="Date Created" />
			<asp:BoundField DataField="DateToSend" DataFormatString="{0:MMM-dd-yyyy}" 
				HeaderText="Date To Send" />
			<asp:BoundField DataField="DateExpires" DataFormatString="{0:MMM-dd-yyyy}" 
				HeaderText="Expiration Date" />
			<asp:BoundField DataField="ShortSubject" HeaderText="Subject" 
				ItemStyle-CssClass="primaryColumn" >
<ItemStyle CssClass="primaryColumn"></ItemStyle>
			</asp:BoundField>
			<asp:TemplateField HeaderText="Edit">
				<ItemTemplate>
					<asp:HyperLink ID="HyperLink1" runat="server" 
						NavigateUrl='<%# String.Format("javascript:editMessage({0}, \"{1}\", \"{2}\", \"{3}\", \"{4}\")", Eval("Id"), Eval("Subject"), Eval("Message"), DateTime.Parse(Eval("DateToSend").ToString()).ToShortDateString(), DateTime.Parse(Eval("DateExpires").ToString()).ToShortDateString())  %>'   Text="Edit"></asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Delete">
				<ItemTemplate>
					<asp:HyperLink ID="HyperLink2" runat="server" 
						NavigateUrl='<%# String.Format("javascript:deleteMessage({0}, \"{1}\")", Eval("Id"), Eval("Subject")) %>' Text="Delete"></asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>

	</asp:GridView>

	<table id="tblMessage" class="modalWindow" style="display: none;">
		<tr>
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
								<asp:TextBox ID="txtSendDate" runat="server"></asp:TextBox>
							</td>
							<td style="width: 100%;"></td>
							<td class="label" style="white-space:nowrap;">
								&nbsp;&nbsp;&nbsp;Expiration Date
							</td>
							<td style="white-space:nowrap;">
								<asp:TextBox ID="txtExpirationDate" runat="server"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td colspan="5" id="maxLengthTarget" style="padding-top: 10px;"></td>
						</tr>
					</table>
			</td>
		</tr>
	</table>

	<input id="txtGlobalMessageID" runat="server" type="hidden" />
	<asp:Button ID="btnSaveMessage" runat="server" Text="Save Message" 
		style="display: none;" onclick="btnSaveMessage_Click"></asp:Button>
	<asp:Button ID="btnDeleteMessage" runat="server" Text="Delete Message" 
		style="display: none;" onclick="btnDeleteMessage_Click"></asp:Button>

	<script type="text/javascript" language="javascript">

	$(document).ready(function()
	{
		$("#<%= txtMessage.ClientID %>").maxlength(
		{
			max: 255,
			feedbackTarget: $("#maxLengthTarget")
		});

		$("#<%= txtSubject.ClientID %>").watermark("Subject (50 chars max)");
		$("#<%= txtMessage.ClientID %>").watermark("Message (255 chars max)");

	});
	
	function editMessage(messageID, subject, message, sendDate, expirationDate)
	{
		$("#<%= txtGlobalMessageID.ClientID %>").val(messageID);

		$("#<%= txtSubject.ClientID %>").val(subject);
		$("#<%= txtMessage.ClientID %>").val(message);
		$("#<%= txtSendDate.ClientID %>").val(sendDate);
		$("#<%= txtExpirationDate.ClientID %>").val(expirationDate);

		$("#tblMessage").dialog("open");
	}

	function deleteMessage(messageID, subject)
	{
		if(confirm("Are you sure you want to delete this message: \r\n\r\n" + subject) == true)
		{
			$("#<%= txtGlobalMessageID.ClientID %>").val(messageID);
			$("#<%= btnDeleteMessage.ClientID %>").click();
		}
	}

		$(function ()
		{
			$("#<%= txtSendDate.ClientID %>").datepicker({
				showOn: "both",
				buttonImage: "<%= Page.ResolveClientUrl("~/Images/calendar.png") %>",
				buttonImageOnly: true,
				showButtonPanel: true
			});

			$("#<%= txtExpirationDate.ClientID %>").datepicker({
				showOn: "both",
				buttonImage: "<%= Page.ResolveClientUrl("~/Images/calendar.png") %>",
				buttonImageOnly: true,
				showButtonPanel: true
			});

			$("#tblMessage").dialog({
				autoOpen: false,
				height: 300,
				width: 600,
				modal: true,
				title: "Editing Global Message",
				buttons: {
					"Save Message": function() {
						$( this ).dialog( "close" );
						$( "#<%= btnSaveMessage.ClientID %>").click();
					},
					Cancel: function() {
						$( this ).dialog( "close" );
					}
				},
				close: function() {
					//alert($("#<%= txtSubject.ClientID %>").val());
				}
			});

			// Enables the dialog to host asp.net controls.
			$("#tblMessage").parent().appendTo(jQuery("form:first"));

			$("#btnNewMessage").click(
				function () 
				{
					$("#<%= txtGlobalMessageID.ClientID %>").val("");
					$("#<%= txtSubject.ClientID %>").val("");
					$("#<%= txtMessage.ClientID %>").val("");
					$("#<%= txtSendDate.ClientID %>").val("<%= DateTime.Now.ToString("MM/dd/yyyy") %>");
					$("#<%= txtExpirationDate.ClientID %>").val("<%= DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy") %>");

					$("#tblMessage").dialog("open");
				}
			);
		});
	</script>

</asp:Content>
