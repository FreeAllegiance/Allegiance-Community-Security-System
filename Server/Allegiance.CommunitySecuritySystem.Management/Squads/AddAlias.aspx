<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="AddAlias.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Squads.AddAlias" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" >
	</asp:ScriptManagerProxy>
	Adding Callsigns to <asp:Label ID="lblSquadName" runat="server" Text="Label"></asp:Label>
	<hr />
	Enter Callsign (Wildcard: %)<br />
	<table>
		<tr>
			<td>
				<asp:TextBox ID="txtSearch" runat="server" Style="width: 250px;" 
					ontextchanged="txtSearch_TextChanged"></asp:TextBox>
			</td>
			<td>
				&nbsp;<asp:Button ID="btnSearch" runat="server" Text="Search" 
					onclick="btnSearch_Click" />
					&nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" 
					onclick="btnCancel_Click"  />
			</td>
			<td>
			</td>
			<td>
			</td>
		</tr>
	</table>
	<br />
	<asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" CssClass="fullWidthGrid">
		<Columns>
			<asp:BoundField DataField="Callsign" HeaderText="Callsign" />
			<asp:TemplateField HeaderText="Select">
				<ItemTemplate>
					<a href="javascript:addUserToGroup('<%# Eval("Callsign") %>')">Select</a>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
	
	<script language="javascript" type="text/javascript">

		function addUserToGroup(callsign)
		{
			var group = "<%= Group %>";
			var groupID = "<%= GroupID %>";

			if (confirm("Would you like to add " + callsign + " to " + group + "?") == false)
				return;

			window.location.href = "Default.aspx?action=add&callsign=" + escape(callsign) + "&groupID=" + groupID;
		}
	
	
	</script>
</asp:Content>
