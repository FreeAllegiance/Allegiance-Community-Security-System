<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Backups.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.Backups" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" >
	<Services>
		<asp:ServiceReference Path="~/AjaxProviders/AutoUpdate.svc" />
	</Services>
</asp:ScriptManagerProxy>

	<table style="width: 100%">
		<tr>
			<td style="width: 100%">
				Backups
			</td>
			<td>
				<asp:Button ID="btnOpenCreateDialog" runat="server" Text="Create Backup" />
			</td>
		</tr>
	</table>
	<hr />
	<asp:Label ID="lblNoBackups" runat="server" Text="No backups have been created yet." Visible='false'></asp:Label>
	<asp:GridView ID="gvBackups" runat="server" AutoGenerateColumns="False" 
		CssClass="fullWidthGrid" AllowPaging="True" 
		onpageindexchanging="gvBackups_PageIndexChanging" PageSize="20">
		<Columns>
			<asp:BoundField DataField="Name" HeaderText="Backup Name" 
				ItemStyle-CssClass="primaryColumn" >
<ItemStyle CssClass="primaryColumn"></ItemStyle>
			</asp:BoundField>
			<asp:BoundField DataField="DateCreated" HeaderText="Backup Date" />
			<asp:TemplateField>
				<ItemTemplate>
					<a href="BackupDetails.aspx?target=<%# Server.UrlEncode(Eval("Name").ToString()) %>&markAll=1">Restore</a>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					<a href="BackupDetails.aspx?target=<%# Server.UrlEncode(Eval("Name").ToString()) %>">View</a>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
	<br />
	<br />
	
	
	<table ID="tblCreateBackup" runat="server" style="width: 500px; height: 300px; background: white; border: 1px solid black;">
		<tr>
			<td class="modalHeader">Create Backup</td>
		</tr>
		<tr>
			<td style="height: 100%;">
				<table style="width: 100%; height: 100%;">
					<tr>
						<td class="label">Backup Name</td>
						<td class="">
							<asp:TextBox ID="txtBackupName" runat="server" style="width: 300px;"></asp:TextBox></td>
					</tr>
				</table>
			</td>
		</tr>
		<tr><td style="text-align: center;">
			<span id="spanErrorMessage" class="errorText" style="visibility: hidden;">Please specify a backup name.</span>
			</td></tr>
		<tr><td style="text-align: center; height: 75px;">
			<span id="spanCreateBackupButtons">
			<input type="button" onclick="createBackup()" value="Create Backup" />
			&nbsp;&nbsp;&nbsp;
			<asp:Button ID="btnCancelCreateBackup" runat="server" Text="Cancel"/>
			</span><span id="spanThrobber" style="display: none;">
				<img src="<%= Page.ResolveUrl("~/Images/throbber.gif") %>" alt="Copying..." />
			</span></td></tr>
	</table>

	<cc1:ModalPopupExtender ID="mpeCreateBackup" runat="server" DynamicServicePath="" 
		Enabled="True" TargetControlID="btnOpenCreateDialog" DropShadow="true" 
		PopupControlID="tblCreateBackup" BackgroundCssClass="modalBackground" 
		CancelControlID="btnCancelCreateBackup"
		>
	</cc1:ModalPopupExtender>
	
	
<asp:HiddenField ID="txtCreateBackupPopupControl" runat="server" />

<script type="text/javascript" language="javascript">
	var g_autoUpdate = new AjaxProviders.AutoUpdate();
	
	function createBackup()
	{
		var txtBackupName = $get("<%= txtBackupName.ClientID %>");
		if (txtBackupName.value.length == 0)
		{
			$get("spanErrorMessage").style.visibility = "";
			return;
		}

		$get("spanErrorMessage").style.visibility = "hidden";

		var spanCreateBackupButtons = $get("spanCreateBackupButtons");
		var spanThrobber = $get("spanThrobber");

		spanCreateBackupButtons.style.display = "none";
		spanThrobber.style.display = "";

		g_autoUpdate.CreateBackup(txtBackupName.value, onCreateBackupSucceeded, onCreateBackupFailed);
		

	}
	
	function onCreateBackupSucceeded(result)
	{
		if(result == true)
		{
			var btnOpenCreateDialog = $get("<%= btnOpenCreateDialog.ClientID %>");
			btnOpenCreateDialog.ModalPopupBehavior.hide();

			window.location.reload();
		}
		else
		{
			var spanErrorMessage = $get("spanErrorMessage");
			spanErrorMessage.style.visibility = "";
			spanErrorMessage.innerHTML = "Could not create backup. Please check backup name and try again.";

			var spanCreateBackupButtons = $get("spanCreateBackupButtons");
			var spanThrobber = $get("spanThrobber");
			
			spanCreateBackupButtons.style.display = "";
			spanThrobber.style.display = "none";
		}
	}
	
	function onCreateBackupFailed(result)
	{
		alert("There was an error creating the backup. Please reload the page and try again.");
	}


</script>

</asp:Content>
