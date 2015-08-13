<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="FileManager.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.FileManager" %>

<%@ Register Assembly="IZ.WebFileManager" Namespace="IZ.WebFileManager" TagPrefix="iz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div>
		<center>
			<asp:Label ID="lblErrorMessage" runat="server" CssClass="errorText"></asp:Label>
			<iz:FileManager ID="wcFileManager" runat="server" Height="500" Width="780" style="text-align: left;"
				FileViewMode="Details" onfileuploading="wcFileManager_FileUploading" 
				onitemrenamed="wcFileManager_ItemRenamed" 
				onitemrenaming="wcFileManager_ItemRenaming" 
				onnewfoldercreating="wcFileManager_NewFolderCreating" 
				onselecteditemsaction="wcFileManager_SelectedItemsAction" 
				onselecteditemsactioncomplete="wcFileManager_SelectedItemsActionComplete" 
				ontoolbarcommand="wcFileManager_ToolbarCommand" >
<FileViewStyle BackColor="White" BorderColor="#ACA899" BorderWidth="1px" BorderStyle="Solid" Font-Names="Tahoma,Verdana,Geneva,Arial,Helvetica,sans-serif" Font-Size="11px" ForeColor="Black" Height="400px" Width="600px"></FileViewStyle>

<FolderTreeStyle BackColor="White" BorderColor="#ACA899" BorderWidth="1px" BorderStyle="Solid" Font-Names="Tahoma,Verdana,Geneva,Arial,Helvetica,sans-serif" Font-Size="11px" ForeColor="Black" Height="400px" Width="600px"></FolderTreeStyle>
			</iz:FileManager>
			<br />
			<asp:Button ID="btnReturnToPackageManager" runat="server" Text="Save Changes and Return To Package Manager" onclick="btnReturnToPackageManager_Click" />
			<asp:Button ID="btnCancel" runat="server" Text="Cancel Changes" 
				onclick="btnCancel_Click" />
		</center>
	</div>
	
	<script type="text/javascript" language="javascript">

		var uploadControl = document.getElementById('<%= wcFileManager.ClientID %>_Upload');
		if (uploadControl != null)
			uploadControl.size = 80;

	</script>

</asp:Content>

