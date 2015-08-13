<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master"
	AutoEventWireup="true" CodeBehind="EditPublication.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.EditPublication" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" >
		<Services>
			<asp:ServiceReference Path="~/AjaxProviders/AutoUpdate.svc" />
		</Services>
	</asp:ScriptManagerProxy>
	
	Package Details
	<hr />
	<center>
		<table class="fullWidthTable">
			<tr>
				<td class="label">
					Lobby
				</td>
				<td>
					<asp:TextBox ID="txtLobbyName" runat="server" ReadOnly="true" Width="350"></asp:TextBox>
				</td>
				<td class="label">
					Host
				</td>
				<td>
					<asp:TextBox ID="txtHost" runat="server"></asp:TextBox><asp:RequiredFieldValidator
						ControlToValidate="txtHost" ID="rfvHost" runat="server" ErrorMessage="Please specify a host." Text="*"></asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td class="label">
					Base Path
				</td>
				<td colspan="1">
					<asp:TextBox ID="txtBasePath" runat="server" Width="350"></asp:TextBox><asp:RequiredFieldValidator
						ControlToValidate="txtBasePath" ID="rfvBasePath" runat="server" ErrorMessage="Please specify a base path." Text="*"></asp:RequiredFieldValidator>
				</td>
				<td class="label">
					<label for="<%= chkRestrictive.ClientID %>">Restrictive</label>
				</td>
				<td colspan="1">
					<asp:CheckBox ID="chkRestrictive" runat="server" />
				</td>
			</tr>
			<tr>
				<td class="label" colspan="3">
					<label for="<%= chkEnabled.ClientID %>">Enabled</label>
				</td>
				<td colspan="1">
					<asp:CheckBox ID="chkEnabled" runat="server" />
				</td>
			</tr>
		</table>
		<br />
		<asp:Button ID="btnSave" runat="server" Text="Save Details" 
			onclick="btnSave_Click" />
	&nbsp;&nbsp;&nbsp;&nbsp;
		<asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" 
			Text="Cancel" CausesValidation="false" />
			<table><tr><td>
			<asp:ValidationSummary ID="vsValidationSummary" runat="server" />
			</td></tr></table>
	</center>
	<br />
	<br />
	Available Packages
	<hr />
	<asp:GridView ID="gvPackages" runat="server" CssClass="fullWidthGrid" AutoGenerateColumns="False">
		<Columns>
			<asp:BoundField DataField="Name" HeaderText="Package Name" 
				ItemStyle-CssClass="primaryColumn" >
<ItemStyle CssClass="primaryColumn"></ItemStyle>
			</asp:BoundField>
			<asp:TemplateField HeaderText="Included">
				<ItemTemplate>
					<input type="checkbox" <%# Convert.ToBoolean(Eval("IsIncluded")) == true ? "checked" : "" %> 
						onchange="onIncludeInPublicationChange('<%# Eval("Name") %>', this.checked)" />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Info">
				<ItemTemplate>
					<a href="javascript:showPublicationFiles('<%# Eval("Name").ToString() %>')">
						Info</a>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Edit">
				<ItemTemplate>
					<a href="EditPackage.aspx?target=<%# Server.UrlEncode(Eval("Name").ToString()) %>">
						Edit</a>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Delete">
				<ItemTemplate>
					<a href="javascript:deletePackage('<%# Eval("Name").ToString() %>')">Delete</a>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
	<br />
	<center>
		<asp:Button ID="btnDownloadPublication" runat="server" 
			Text="Download Publication" onclick="btnDownloadPublication_Click" />
		&nbsp;&nbsp;&nbsp;
		<asp:Button ID="btnDeployPublication" runat="server" 
			Text="Deploy Publication" onclick="btnDeployPublication_Click" />
	</center>
	<table runat="server" id="tblPublicationDetails" style="border: 1px solid black; display: none; background-color: White;">
		<tr>
			<td class="modalHeader">
				Files in Publication: <span id="spanPackageName"></span>
			</td>
		</tr>
		<tr valign="top" style="width: 800px; height: 400px;">
			<td>
				<iframe id="iframePackageDetails" style="width: 800px; height: 400px; border: 0px;">
				</iframe>
			</td>
		</tr>
		<tr>
			<td style="text-align: center;">
				<asp:Button ID="btnClosePopup" runat="server" Text="Close" />
			</td>
		</tr>
	</table>
	
	<cc1:modalpopupextender id="mpePackageDetails" runat="server" dynamicservicepath="" enabled="True"
		targetcontrolid="txtPackageDetailsPopupControl" dropshadow="true" popupcontrolid="tblPublicationDetails"
		backgroundcssclass="modalBackground" CancelControlID="btnClosePopup" >
	</cc1:modalpopupextender>
	
	<asp:HiddenField ID="txtPackageDetailsPopupControl" runat="server" />

	<script type="text/javascript" language="javascript">

		var g_autoUpdate = new AjaxProviders.AutoUpdate();
		
		function showPublicationFiles(packageName)
		{
			var spanPackageName = document.getElementById("spanPackageName");
			spanPackageName.innerHTML = packageName;

			var iframePackageDetails = document.getElementById("iframePackageDetails");
			iframePackageDetails.src = '<%= Page.ResolveUrl("~/Content/AutoUpdate/PublicationDetailsIFrame.aspx?target=") %>' + escape(packageName);

			var txtPackageDetailsPopupControl = $get("<%= txtPackageDetailsPopupControl.ClientID %>");
			txtPackageDetailsPopupControl.ModalPopupBehavior.show();
		}
		
    	function onIncludeInPublicationChange(packageName, isIncluded)
    	{
    		g_autoUpdate.SetPackageInclusionForPublication(<%= Target %>, packageName, isIncluded, onIncludeInPublicationSuccess, onIncludeInPublicationFail);
    	}

    	function onIncludeInPublicationSuccess(result)
    	{

    	}

    	function onIncludeInPublicationFail(result)
    	{
    		alert("Include or Exclude status was not updated. Please refresh this window.");
    	}
    	
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

		function onDeletePackageFail(result, message)
		{
			alert('fail: ' + message);
		}
		
		if(<%= PublicationDeployed.ToString().ToLower() %> == true)
			alert("Publication was deployed.");
			
		if(<%= PublicationDeployFailed.ToString().ToLower() %> == true)
			alert("Publication deployment failed.");

	</script>

</asp:Content>
