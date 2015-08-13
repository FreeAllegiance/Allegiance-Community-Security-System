<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Content.VirtualMachineMarkers.Default" %>

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

	<asp:GridView ID="gvVirtualMachineMarkers" runat="server"
		DataSourceID="ldsVirtualMachineMarkers" EnableModelValidation="True" 
		AutoGenerateColumns="False" DataKeyNames="Id" CssClass="fullWidthGrid">
		<Columns>

			<asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" 
				ReadOnly="True" SortExpression="Id" />
			<asp:BoundField DataField="IdentifierMask" HeaderText="IdentifierMask" 
				SortExpression="IdentifierMask"> 
				<ItemStyle CssClass="primaryColumn" />
			</asp:BoundField>
			<asp:TemplateField HeaderText="DeviceType" SortExpression="DeviceType">
				<EditItemTemplate>
					<asp:DropDownList 
						ID="ddlDeviceType" 
						runat="server" 
						DataTextField="Name" 
						DataValueField="Id" 
						SelectedValue='<%# Bind("RecordTypeId") %>' 
						DataSourceID="ldsMachineRecordTypes">
					</asp:DropDownList>
				</EditItemTemplate>
				<ItemTemplate>
					<asp:Label ID="Label1" runat="server" Text='<%# Bind("DeviceType") %>'></asp:Label>
				</ItemTemplate>
			</asp:TemplateField>
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
	<input id="btnAddNew" type="button" value="Add New Virtual Machine Marker" onclick="$('#tblNewVirtualMachineMarker').dialog('open')" />

	<table style="display: none;" id="tblNewVirtualMachineMarker">
		<tr>
			<td>Identifier (use % for wildcards):</td>
			<td>
				<asp:TextBox ID="txtIdentifier" runat="server" CssClass="required"></asp:TextBox></td>
		</tr>
		<tr>
			<td>Record Type:</td>
			<td>
				<asp:DropDownList ID="ddlDeviceType" runat="server" AppendDataBoundItems="True" 
					DataTextField="Name" DataValueField="Id" AutoPostBack="False" SelectedValue='<%# 2 %>'
					DataSourceID="ldsMachineRecordTypes"></asp:DropDownList></td>
		</tr>
	</table>

	<asp:Button ID="btnSaveNewVirtualMachineMarker" runat="server" Text="Save" style="display: none;"
		 onclick="btnSaveNewVirtualMachineMarker_Click" />
	
	<asp:LinqDataSource ID="ldsVirtualMachineMarkers" runat="server" 
		ContextTypeName="Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext"
		TableName="VirtualMachineMarkers"
		EnableUpdate="True"
		EnableInsert="True"
		EnableDelete="True">
	</asp:LinqDataSource>
	

<%-- how to bind to an enumeration instead of a data table with a reuseable utility.	
	<asp:ObjectDataSource ID="odsDeviceType" runat="server" 
		SelectMethod="GetAllValues" 
		TypeName="Allegiance.CommunitySecuritySystem.Common.Utility.EnumBinder">
		<SelectParameters>
			<asp:Parameter DefaultValue="Allegiance.CommunitySecuritySystem.Common.Envelopes.AuthInfo.DeviceType" Name="enumTypeToBind" Type="String" />
		</SelectParameters>
	</asp:ObjectDataSource>--%>
	

	<asp:LinqDataSource ID="ldsMachineRecordTypes" runat="server" 
		ContextTypeName="Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext" 
		Select="new (Id, Name)" TableName="MachineRecordTypes">
	</asp:LinqDataSource>
	

	<script type="text/javascript" language="javascript">

		$(function ()
		{
			$("#tblNewVirtualMachineMarker").dialog({
				autoOpen: false,
				height: 300,
				width: 550,
				modal: true,
				title: "New Virtual Machine Marker",
				buttons: {
					"Save": function ()
					{
						var validator = jQuery("form:first").validate();
						validator.element("#<%= txtIdentifier.ClientID %>");

						//alert(validator.numberOfInvalids());

						if (validator.numberOfInvalids() == 0)
						{
							$(this).dialog("close");
							$("#<%= btnSaveNewVirtualMachineMarker.ClientID %>").click();
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
			$("#tblNewVirtualMachineMarker").parent().appendTo(jQuery("form:first"));

		});


	</script>

</asp:Content>

