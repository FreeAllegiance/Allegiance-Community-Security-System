<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Packages.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.Packages" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" >
		<Services>
			<asp:ServiceReference Path="~/AjaxProviders/AutoUpdate.svc" />
		</Services>
	</asp:ScriptManagerProxy>
	
Current Packages<br /> 
<hr />
	<div class="packages">
		<asp:GridView ID="gvPackages" runat="server" AutoGenerateColumns="False" CssClass="fullWidthGrid nowrapCells nowrapHeaders">
			<Columns>
				<asp:BoundField DataField="Name" HeaderText="Package Name" ItemStyle-CssClass="primaryColumn"/>
				<asp:BoundField DataField="DateCreated" HeaderText="Date Created" />
				<asp:BoundField DataField="LastModified" HeaderText="Last Modified" />
				<asp:TemplateField HeaderText="Edit">
					<ItemTemplate>
						<a href="EditPackage.aspx?target=<%# Server.UrlEncode((String) Eval("Name", "{0}")) %>">Edit</a>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Delete">
				<ItemTemplate>
					<a href="javascript:deletePackage('<%# Eval("Name").ToString() %>')">Delete</a>
				</ItemTemplate>
			</asp:TemplateField>
			</Columns>
		</asp:GridView>
	</div>
	<br />
	<asp:Button ID="btnNewPackage" runat="server" Text="Create New Package" 
		onclick="btnNewPackage_Click" />

	<script type="text/javascript" language="javascript">
		var g_autoUpdate = new AjaxProviders.AutoUpdate();
		
		function deletePackage(packageName)
		{
			if (confirm("Are you sure you want to delete " + packageName + "?") == false)
				return;

			g_autoUpdate.DeletePackage(packageName, onDeletePackageSuccess, onDeletePackageFail);
		}

		function onDeletePackageSuccess(result)
		{
			window.location.reload();
		}

		function onDeletePackageFail(result, errorMsg, thrown)
		{
			alert('fail: ' + errorMsg);
		}
	
	</script>

</asp:Content>
