<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="BackupDetails.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.BackupDetails" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" >
	</asp:ScriptManagerProxy>
	<div id="divErrorMessage" class="errorText"></div>
	Details for Backup: <b><%= Target %></b>, Created On: <%= BackupCreationDate %>
	<hr />
	<asp:GridView ID="gvBackupFiles" runat="server" AutoGenerateColumns="False" 
		CssClass="fullWidthGrid" >
		<Columns>
			<asp:ImageField DataImageUrlField="IncludedImage" HeaderText="Incl.">
			</asp:ImageField>
			<asp:BoundField DataField="Type" HeaderText="Type" ></asp:BoundField>
			<asp:BoundField DataField="Container" HeaderText="Container" ></asp:BoundField>
			<asp:BoundField DataField="RelativeDirectory" HeaderText="Sub Folder" ></asp:BoundField>
			<asp:TemplateField HeaderText="File Name">
				<ItemTemplate>
					<a href="DownloadItem.aspx?backup=<%# Page.Server.UrlEncode(Target) %>&type=<%# Page.Server.UrlEncode(Eval("Type").ToString()) %>&container=<%# Page.Server.UrlEncode(Eval("Container").ToString()) %>&rel=<%# Server.UrlEncode(Eval("RelativeDirectory").ToString()) %>&file=<%# Server.UrlEncode(Eval("Name").ToString()) %>"><%# Eval("Name") %></a>
				</ItemTemplate>
				<ItemStyle CssClass="primaryColumn" />
			</asp:TemplateField>
			<asp:BoundField DataField="Name" HeaderText="HiddenName" ItemStyle-CssClass="hiddenColumn">
<ItemStyle CssClass="hiddenColumn"></ItemStyle>
			</asp:BoundField>
			<asp:BoundField DataField="LastModified" HeaderText="Modified" />
			<asp:TemplateField HeaderText="Restore">
				<ItemTemplate>
					<asp:CheckBox ID="chkRestore" runat="server" OnCheckedChanged="chkRestore_OnCheckedChanged" OnPreRender="chkRestore_OnPreRender" />
					<input type="hidden" id="txtPath" value="<%# Eval("Path") %>" />
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
	<br />
	<asp:Button ID="btnRecoverSelectedFiles" runat="server" Text="Recover Selected Files" OnClientClick="return confirmRestore()" />

	<script type="text/javascript" language="javascript">
		var g_allCheckboxes = [<%= AllCheckboxUniqueIDsToSelect.ToString() %>];		
		
		function presetAllCheckboxes()
		{
			for(var i = 0; i < g_allCheckboxes.length; i++)
			{
				$get(g_allCheckboxes[i]).checked = true;
			}
		}
		
		function confirmRestore()
		{
			if(confirm("Are you sure you want to restore all selected files over the current versions? This action cannot be undone.") == false)
				return false;
				
			var btnRecoverSelectedFiles = $get("<%= btnRecoverSelectedFiles.ClientID %>");
			btnRecoverSelectedFiles.value = "Recovering...";
			btnRecoverSelectedFiles.disabled = true;
			
			return true;		
		}
	
		function pageLoad()
		{
			presetAllCheckboxes();
			
			if(<%= FilesWereRestored.ToString().ToLower() %> == true)
			{
				//var divErrorMessage = $get("divErrorMessage");
				//divErrorMessage.innerHTML = "File restore complete.<br /><br />";
				alert("File restore complete. Please check that all publications have the right packages included.");
			}
		}
	
	</script>
	
</asp:Content>
