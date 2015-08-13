<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AliasDetail.ascx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Users.UI.UserControls.AliasDetail" %>

<asp:Panel ID="pGroups" runat="server">
	<br />
	<asp:Panel ID="pAddGroup" runat="server">
		Add Group: 
		<asp:DropDownList ID="ddlGroup" runat="server" Width="200px">
		</asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Role: <asp:DropDownList ID="ddlRole" runat="server" Width="200px">
		</asp:DropDownList>&nbsp;<asp:Button ID="btnAddGroupRole" runat="server" 
			Text="Add" onclick="btnAddGroupRole_Click" />
		<br /><br />
	</asp:Panel>
	<asp:Label ID="lblErrorMessage" runat="server" CssClass="errorText"></asp:Label>
	<asp:GridView ID="gvGroups" runat="server" AutoGenerateColumns="False" 
		onrowdatabound="gvGroups_RowDataBound" CssClass="fullWidthGrid">
		
		<Columns>
			<asp:BoundField DataField="GroupName" HeaderText="Group Name" ItemStyle-CssClass="primaryColumn" />
			<asp:TemplateField HeaderText="Role Name">
				<ItemTemplate>
					<asp:DropDownList ID="ddlRoles" runat="server" DataTextField="Name" 
						DataValueField="Id" AutoPostBack="True" 
						onselectedindexchanged="ddlRoles_SelectedIndexChanged" >
					</asp:DropDownList>
					<asp:HiddenField ID="txtGroupID" runat="server" />
					<asp:HiddenField ID="txtAliasID" runat="server" />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:BoundField DataField="Token" HeaderText="Token" />
			<asp:BoundField DataField="Tag" HeaderText="Tag" />
			
			<asp:TemplateField HeaderText="Delete">
				<ItemTemplate>
					<a href="javascript:<%= ClientID %>_onDeleteGroupRole(<%# Eval("GroupID") %>, <%# Eval("AliasID") %>, '<%# Eval("GroupName") %>')">Delete</a>
				</ItemTemplate>
			</asp:TemplateField>
		 </Columns>
		
	</asp:GridView>
</asp:Panel>
<asp:Panel ID="pNoGroupRolesAssigned" runat="server">This alias is not assigned to any groups.</asp:Panel>
<br />

<% if (CanShowDeleteButton == true) { %>
	<center>
		<input type="button" name="btnDeleteAlias" id="btnDeleteAlias" value="Delete Alias" onclick="<%= ClientID %>_onDeleteAlias(<%= AliasID %>)" />
	</center>
<% } %>

<asp:HiddenField ID="txtDeleteAliasFlag" runat="server" />
<asp:HiddenField ID="txtDeleteFlag" runat="server" />
<asp:HiddenField ID="txtGroupID" runat="server" />
<asp:HiddenField ID="txtAliasID" runat="server" />

<script type="text/javascript" language="javascript">

	function <%= ClientID %>_onDeleteGroupRole(groupID, aliasID, groupName)
	{
		if (confirm("Are you sure you want to delete " + groupName + " from this user?") == false)
			return;
			
		document.getElementById('<%= txtDeleteFlag.ClientID %>').value = "1";
		document.getElementById('<%= txtGroupID.ClientID %>').value = groupID;
		document.getElementById('<%= txtAliasID.ClientID %>').value = aliasID;
		
		__doPostBack("<%= ClientID %>_onDeleteGroupRole", '');
	}

	function <%= ClientID %>_onDeleteAlias(aliasID)
	{
		if(confirm("Are you sure you want to delete this alias?") == false)
			return;

		document.getElementById('<%= txtDeleteAliasFlag.ClientID %>').value = "1";
		document.getElementById('<%= txtAliasID.ClientID %>').value = aliasID;

		//__doPostBack("<%= ClientID %>_onDeleteAlias", '');
		document.forms[0].submit();
	}
	
	$(document).ready(function () 
	{
		//alert('pageload.');

		document.getElementById('<%= txtDeleteAliasFlag.ClientID %>').value = "";
		document.getElementById('<%= txtDeleteFlag.ClientID %>').value = "";
		document.getElementById('<%= txtGroupID.ClientID %>').value = "";
		document.getElementById('<%= txtAliasID.ClientID %>').value = "";

		<% if(ForcePageReload == true) { %>
			self.parent.location.reload(true);
		<% } %>
	});

</script>