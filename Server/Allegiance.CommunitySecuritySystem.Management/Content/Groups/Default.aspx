<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Content.Groups.Default" %>

<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
	Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
		<Scripts>
			<asp:ScriptReference Path="http://ajax.microsoft.com/ajax/jquery.validate/1.7/jquery.validate.min.js" />
		</Scripts>
	</asp:ScriptManagerProxy>

	<asp:Label ID="lblErrorText" CssClass="errorText" runat="server" Text=""></asp:Label>

	<asp:GridView ID="gvGroups" runat="server"
		DataSourceID="ldsGroups" EnableModelValidation="True" 
		AutoGenerateColumns="False" DataKeyNames="Id" CssClass="fullWidthGrid">
		<Columns>

			<asp:TemplateField HeaderText="Id" InsertVisible="False" SortExpression="Id">
				<EditItemTemplate>
					<asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
				</EditItemTemplate>
				<ItemTemplate>
					<asp:Label ID="Label3" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Name" SortExpression="Name">
				<EditItemTemplate>
					<asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
					<asp:RequiredFieldValidator ID="rfvName" runat="server" 
						ErrorMessage="<br />Name is required." ControlToValidate="txtName" Display="Dynamic"></asp:RequiredFieldValidator>
					<asp:CustomValidator ID="cvName" runat="server" ErrorMessage="<br />Name is already in use." 
						ControlToValidate="txtName" EnableClientScript="False" 
						onservervalidate="cvName_ServerValidate" Display="Dynamic"></asp:CustomValidator>
				</EditItemTemplate>
				<ItemTemplate>
					<asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
				</ItemTemplate>
				<ItemStyle CssClass="primaryColumn" />
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Tag" SortExpression="Tag">
				<EditItemTemplate>
					
					<asp:TextBox ID="txtTag" runat="server" Text='<%# Bind("Tag") %>'></asp:TextBox>
					<asp:RequiredFieldValidator ID="rfvTag" runat="server" 
						ErrorMessage="<br />Tag is required." ControlToValidate="txtTag" Display="Dynamic"></asp:RequiredFieldValidator>
					<asp:CustomValidator ID="cvTag" runat="server" ErrorMessage="<br />Tag is already in use." 
						ControlToValidate="txtTag" EnableClientScript="False" 
						onservervalidate="cvTag_ServerValidate" Display="Dynamic"></asp:CustomValidator>
				</EditItemTemplate>
				<ItemTemplate>
					<asp:Label ID="Label2" runat="server" Text='<%# Bind("Tag") %>'></asp:Label>
				</ItemTemplate>
			</asp:TemplateField>

			<asp:CheckBoxField DataField="IsSquad" HeaderText="Is Squad" />

			<asp:BoundField DataField="DateCreated" HeaderText="Date Created" 
				ReadOnly="True" DataFormatString="{0:MM/dd/yyyy}" />

			<asp:CommandField ShowEditButton="True">
			<ItemStyle Width="100px" />
			</asp:CommandField>
			<asp:TemplateField ShowHeader="False">
				 <ItemTemplate>
				   <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
								OnClientClick='return confirm("Are you sure you want to delete this entry?");'
								Text="Delete" />
				 </ItemTemplate>
		   	  <ItemStyle Width="100px" />
		   </asp:TemplateField>

		</Columns>
	</asp:GridView>
	<hr />
	<input id="btnAddNew" type="button" value="Add New Group or Squad" onclick="$('#tblNewGroup').dialog('open')" />

	<table style="display: none;" id="tblNewGroup">
		<tr>
			<td>Group or Squad Name:</td>
			<td>
				<asp:TextBox ID="txtName" runat="server" CssClass="required"></asp:TextBox></td>
		</tr>
		<tr>
			<td>Tag:</td>
			<td>
				<asp:TextBox ID="txtTag" runat="server" CssClass="required"></asp:TextBox></td>
		</tr>
		<tr>
			<td><asp:Label ID="lblIsSquad" runat="server" AssociatedControlID="chkIsSquad" Text="Is Squad:"></asp:Label></td>
			<td>
				<asp:CheckBox ID="chkIsSquad" runat="server" /></td>
		</tr>
	</table>

	<asp:Button ID="btnDummy" runat="server" Text="Do Nothing On Enter Press On GridView textboxes" style="display: none;" />

	<asp:Button ID="btnSaveNewGroup" runat="server" Text="Save" style="display: none;"
		 onclick="btnSaveNewGroup_Click" />
	
	<asp:LinqDataSource ID="ldsGroups" runat="server" 
		ContextTypeName="Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext"
		TableName="Groups"
		EnableUpdate="True"
		EnableDelete="True" onupdating="ldsGroups_Updating">
	</asp:LinqDataSource>	
	
	<script type="text/javascript" language="javascript">

		$(function ()
		{
			$("#tblNewGroup").dialog({
				autoOpen: false,
				height: 300,
				width: 550,
				modal: true,
				title: "New Virtual Machine Marker",
				buttons: {
					"Save": function ()
					{
						var validator = jQuery("form:first").validate();
						validator.element("#<%= txtName.ClientID %>");
						validator.element("#<%= txtTag.ClientID %>");

						//alert(validator.numberOfInvalids());

						if (validator.numberOfInvalids() == 0)
						{
							$(this).dialog("close");
							$("#<%= btnSaveNewGroup.ClientID %>").click();
						}
					},
					Cancel: function ()
					{
						$(this).dialog("close");
					}
				},
				close: function ()
				{
					//allFields.val( "" ).removeClass( "ui-state-error" );
				}
			});

			// Enables the dialog to host asp.net controls.
			$("#tblNewGroup").parent().appendTo(jQuery("form:first"));

			var litErrorText = '<%= ValidationMessage %>';
			if (litErrorText.length > 0)
				alert(litErrorText);

		});


	</script>

</asp:Content>

