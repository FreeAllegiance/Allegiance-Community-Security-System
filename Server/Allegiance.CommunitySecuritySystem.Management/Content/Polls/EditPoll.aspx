<%@ Page Title="" EnableEventValidation="true" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="EditPoll.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Content.Polls.EditPoll" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


	<asp:Label ID="lblErrorMessage" runat="server" Text="" CssClass="errorText"></asp:Label>

	<table>
		<tr>
			<td style="width: 150px;">
				Poll Expiration Date
			</td>
			<td>
				<asp:TextBox ID="txtPollExpirationDate" runat="server"></asp:TextBox>
				<asp:CustomValidator ID="cvExpirationDate" runat="server" 
					ControlToValidate="txtPollExpirationDate" Display="Dynamic" 
					ErrorMessage="Please enter a valid date that is greater than the current date." 
					onservervalidate="cvExpirationDate_ServerValidate" ValidateEmptyText="True" 
					ClientValidationFunction="validatePollExpirationDate">*</asp:CustomValidator>
			</td>
		</tr>
		<tr>
			<td>
				Poll Creation Date
			</td>
			<td>
				<asp:TextBox ID="txtPollCreationDate" runat="server" ReadOnly="true"></asp:TextBox>
			</td>
		</tr>

		<tr>
			<td colspan="2" style="padding-top: 20px;">
				Question
				<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
					ControlToValidate="txtQuestion" ErrorMessage="Please enter a question.">*</asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<asp:TextBox ID="txtQuestion" runat="server" TextMode="MultiLine" Height="200px" Width="500px"></asp:TextBox>
			</td>
		</tr>
	</table>

	<asp:Panel ID="pPollOptions" Visible="false" runat="server">
		<br />
		<input id="btnAddPollOption" type="button" value="Add Poll Option" onclick="editPollOption(0, '')" />
		<asp:Button ID="btnRecalculatePoll" runat="server" 
			Text="Recalculate Poll Results" onclick="btnRecalculatePoll_Click" />
		&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Last Poll Recalculation: <%= Allegiance.CommunitySecuritySystem.Management.Format.DateTime(Poll.LastRecalculation)%>
		<hr />

		<asp:GridView ID="gvPollOptions" runat="server" AutoGenerateColumns="False" 
			Visible="True" CssClass="fullWidthGrid">
			<Columns>
				<asp:BoundField DataField="VotePercentage" HeaderText="%"  />
				<asp:BoundField DataField="VoteCount" HeaderText="#"  />
				<asp:BoundField DataField="ShortOption" HeaderText="Option" ItemStyle-CssClass="primaryColumn" />
				<asp:TemplateField HeaderText="Edit">
					<ItemTemplate>
						<a href="javascript:editPollOption(<%# Eval("Id") %>, '<%# Eval("Option") %>')">Edit</a>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Delete">
					<ItemTemplate>
						<a href="javascript:deletePollOption(<%# Eval("Id") %>, '<%# Eval("Option") %>')">Delete</a>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>
	</asp:Panel>

	<hr />
	<asp:ValidationSummary ID="vsValidationSummary" runat="server" 
		ShowMessageBox="True" ShowSummary="False" />
	<asp:Button ID="btnSavePoll" runat="server" Text="Save" 
		onclick="btnSavePoll_Click" />

	<table id="tblEditPollOption" style="width: 100%; height: 100%;">
		<tr>
			<td>
				Poll Option (70 chars max)
			</td>
		</tr>
		<tr>
			<td>
				<textarea id="txtPollOption" style="width: 510px; height: 200px;"></textarea>
				<%--<asp:TextBox ID="txtPollOption" runat="server" TextMode="MultiLine" style="width: 510px; height: 200px;"></asp:TextBox>--%>
			</td>
		</tr>
	</table>
	<asp:Button  ID="btnSavePollOption" runat="server" Text="Save Poll Option" 
		onclick="btnSavePollOption_Click" style="display: none;"  />
	<asp:HiddenField ID="txtEditingPollOptionID" runat="server" />
	<asp:HiddenField ID="txtPollOptionValue" runat="server" />

	<script type="text/javascript" language="javascript">

		function deletePollOption(pollOptionID, option)
		{
			if (confirm("Are you sure you want to delete this poll option: \n\n" + option) == true)
			{
				document.location.href = "EditPoll.aspx?deletePollOptionID=" + pollOptionID + "&pollID=<%= PollID %>";
			}
		}

		function validatePollExpirationDate(source, args)
		{
			var txtPollExpirationDate = $get("<%= txtPollExpirationDate.ClientID %>");
			var txtPollCreationDate = $get("<%= txtPollCreationDate.ClientID %>");

			if(txtPollExpirationDate.value.length == 0)
			{
				args.IsValid = false;
				return;
			}

			var expirationDate = Date.parse(txtPollExpirationDate.value);
			var creationDate = Date.parse(txtPollCreationDate.value);

			if(expirationDate == null)
			{
				args.IsValid = false;
				return;
			}

			if(expirationDate < creationDate)
			{
				args.IsValid = false;
				return;
			}
		}

		function editPollOption(pollOptionID, optionText)
		{
			var txtPollOption = $get("txtPollOption");
			var txtEditingPollOptionID = $get("<%= txtEditingPollOptionID.ClientID  %>");
			
			txtPollOption.value = optionText;
			txtEditingPollOptionID.value = pollOptionID;

			$( "#tblEditPollOption" ).dialog("open");
		}

		$(function ()
		{
			$("#<%= txtPollExpirationDate.ClientID %>").datepicker({
				showOn: "both",
				buttonImage: "<%= Page.ResolveClientUrl("~/Images/calendar.png") %>",
				buttonImageOnly: true,
				showButtonPanel: true
			});

			$( "#tblEditPollOption" ).dialog({
				autoOpen: false,
				height: 300,
				width: 550,
				modal: true,
				title: "Editing Poll Option",
				buttons: {
					"Save Poll Option": function() {
						 // for some reason, the JQueryUI is hiding the value from the post back action.
						 var pollOptionText = $get("txtPollOption").value;

						if(pollOptionText.length == 0)
						{
							alert("Please enter a poll option.");
							return;
						}

						if(pollOptionText.length >= 70)
						{
							alert("Your poll option is more than 70 characters. Please enter a shorter poll option.");
							return;
						}

						 $get("<%= txtPollOptionValue.ClientID %>").value = pollOptionText;
						 
						 
						__doPostBack("<%= btnSavePollOption.UniqueID %>", "OnClick");
						//$get("<%= btnSavePollOption.ClientID %>").click();

						$( this ).dialog( "close" );
					},
					Cancel: function() {
						$( this ).dialog( "close" );
					}
				},
				close: function() {
					//allFields.val( "" ).removeClass( "ui-state-error" );
				}
			});



		});


	</script>

</asp:Content>
