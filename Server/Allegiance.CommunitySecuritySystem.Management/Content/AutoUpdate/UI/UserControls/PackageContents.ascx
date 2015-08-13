<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PackageContents.ascx.cs"
	Inherits="Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.UI.UserControls.PackageContents" %>
	
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
	
	
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" >
	<Services>
		<asp:ServiceReference Path="~/AjaxProviders/AutoUpdate.svc" />
	</Services>
</asp:ScriptManagerProxy>
	
<asp:GridView ID="gvPackageFiles" runat="server" AutoGenerateColumns="False" CssClass="fullWidthGrid">
	<Columns>
		<asp:BoundField DataField="RelativeDirectory" HeaderText="Sub Folder" />
		<asp:TemplateField HeaderText="File Name">
			<ItemTemplate>
				<a href="DownloadItem.aspx?type=<%# Page.Server.UrlEncode("Packages") %>&container=<%# Page.Server.UrlEncode(Target) %>&rel=<%# Server.UrlEncode(Eval("RelativeDirectory").ToString()) %>&file=<%# Server.UrlEncode(Eval("Name").ToString()) %>">
					<%# Eval("Name") %></a>
			</ItemTemplate>
			<ItemStyle CssClass="primaryColumn" />
		</asp:TemplateField>
		<asp:BoundField DataField="LastModified" HeaderText="Last Modified" 
			DataFormatString="{0:MM/dd/yyyy HH:mm}" />
		<asp:TemplateField HeaderText="Delete">
			<ItemTemplate>
				<a href="javascript:deleteFile('<%# Eval("RelativeDirectory") %>', '<%# HttpContext.Current.Server.UrlEncode(Convert.ToString(Eval("Name"))) %>')">
					Delete</a>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Include">
			<ItemTemplate>
				<input type="checkbox" <%# Convert.ToBoolean(Eval("IsIncluded")) == true ? "checked" : "" %>
					onchange="onIncludeInPackageChange('<%# Eval("RelativeDirectory") %>', '<%# Eval("Name") %>', this.checked)" />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Protected">
			<ItemTemplate>
				<input type="checkbox" <%# Convert.ToBoolean(Eval("IsProtected")) == true ? "checked" : "" %>
					<%# Convert.ToBoolean(Eval("IsIncluded")) == false ? "disabled='disabled'" : "" %>
					onchange="onProtectInPackageChange('<%# Eval("RelativeDirectory") %>', '<%# Eval("Name") %>', this.checked)" id="protected_<%# Eval("Name") %>" />
			</ItemTemplate>
		</asp:TemplateField>
	</Columns>
</asp:GridView>

<asp:HiddenField ID="txtFileToDelete" runat="server" EnableViewState="False" onvaluechanged="txtFileToDelete_ValueChanged" />
<asp:HiddenField ID="txtFileToDeleteRelativeDirectory" runat="server" EnableViewState="False" />
		
<script language="javascript" type="text/javascript">

	var g_autoUpdate = new AjaxProviders.AutoUpdate();
	
	function deleteFile(relativeDirectory, filename)
	{
		if (confirm("Are you sure you want to delete " + filename) == true)
		{
			var txtFileToDelete = document.getElementById("<%= txtFileToDelete.ClientID %>");
			var txtFileToDeleteRelativeDirectory = document.getElementById("<%= txtFileToDeleteRelativeDirectory.ClientID %>");
			
			txtFileToDelete.value = filename;
			txtFileToDeleteRelativeDirectory.value = relativeDirectory;
			
			__doPostBack("deleteFile", '');
		}
	}
	
	function onIncludeInPackageChange(relativeDirectory, fileName, isIncluded)
	{
		g_autoUpdate.SetFileInclusionForPackage('<%= Target %>', relativeDirectory, fileName, isIncluded, onIncludeInPackageSuccess, onIncludeInPackageFail);

		var protectedCheckbox = document.getElementById("protected_" + fileName);
		
		if (isIncluded == false)
		{	
			protectedCheckbox.checked = false;
			protectedCheckbox.disabled = "disabled";
		}
		else
		{
			protectedCheckbox.disabled = "";
		}
	}

	function onProtectInPackageChange(relativeDirectory, fileName, isProtected)
	{
		g_autoUpdate.SetFileProtectionForPackage('<%= Target %>', relativeDirectory, fileName, isProtected, onProtectInPackageSuccess, onProtectInPackageFail);
	}

	function onIncludeInPackageSuccess(result)
	{

	}

	function onIncludeInPackageFail(result)
	{
		alert("Include or Exclude status was not updated. Please refresh this window.");
	}

	function onProtectInPackageSuccess(result)
	{

	}

	function onProtectInPackageFail(result)
	{
		alert("Protect or Unprotect status was not updated. Please refresh this window.");
	}
	

</script>