<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="EditPackage.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.EditPackage" %>

<%@ Register src="UI/UserControls/PackageContents.ascx" tagname="PackageContents" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	
	<table style="width: 100%; white-space: nowrap; padding: 0px; border-spacing: 0px;">
		<tr>
			<td class="label">
				Name:
			</td>
			<td>
				<asp:TextBox ID="txtPackageName" runat="server"></asp:TextBox><asp:RequiredFieldValidator
					ID="rfvPackageName" runat="server" ErrorMessage="Please specify a package name."
					Text="*" ControlToValidate="txtPackageName" Display="Dynamic"></asp:RequiredFieldValidator>
				<asp:CustomValidator ID="cvPackageExists" runat="server" ErrorMessage="A package with this name already exists."
					Text="*" Display="Dynamic" OnServerValidate="cvPackageExists_ServerValidate"
					ControlToValidate="txtPackageName"></asp:CustomValidator>
				<asp:Button ID="btnSavePackageDetails" runat="server" Text="Save" OnClick="btnSavePackageDetails_Click" />
			</td>
			<td class="label">
				Created:
			</td>
			<td>
				<asp:TextBox ID="txtCreateDate" runat="server" ReadOnly="true"></asp:TextBox>
			</td>
			<td class="label">
				Modified:
			</td>
			<td>
				<asp:TextBox ID="txtLastModifiedDate" runat="server" ReadOnly="true"></asp:TextBox>
			</td>
		</tr>
		<tr><td colspan="6"><asp:ValidationSummary ID="vsSummary" runat="server" CssClass="validationSummary" /></td></tr>
		<tr>
			<td colspan="6"><br />
				<table class="modalHeader" style="width: 100%; border: 1px solid black;">
					<tr >
						<td>&nbsp;&nbsp;&nbsp;Quick Deploy:&nbsp;</td>
						<td id="tdQuickDeploy" runat="server" style="width: 100%; text-align: left;" >
							<asp:Repeater ID="rptQuickDeploy" runat="server">
								<HeaderTemplate>
									<table style="width: 100%">
										<tr>
								</HeaderTemplate>
								<ItemTemplate>
											<td>
												<input style="width: 150px;" id="btnQuickDeploy" type="button" value="<%# DataBinder.Eval(Container.DataItem, "Name") %>" onclick="deployPackage('<%# DataBinder.Eval(Container.DataItem, "Id") %>')" />
											</td>
								</ItemTemplate>
								<FooterTemplate>
										</tr> 
									</table>
								</FooterTemplate>
							</asp:Repeater>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<br />
	<table runat="server" id="tblPackageContents" class="packageContents" >
		<tr>
			<td class="modalHeader">
				Package Contents
			</td>
		</tr>
		<tr>
			<td class="fileUploadSection">
				<asp:FileUpload ID="fuFileUpload" runat="server" CssClass="fileUpload" size="50" Height="23px" />
				<asp:Button ID="btnAddFile" runat="server" Text="Upload Package Zip File" OnClick="btnAddFile_Click" Width="170px" Height="23px" />
				<asp:Button ID="btnFileManager" runat="server" Text="File Manager" Height="23px" onclick="btnFileManager_Click"/>
				<br />
				<asp:Label ID="lblUploadStatus" runat="server" Text="" CssClass="errorText"></asp:Label>
			</td>
		</tr>
		<tr>
			<td class="packageFilelistSection">
				
				<uc1:PackageContents ID="ucPackageContents" runat="server" />
				
			</td>
		</tr>
	</table>
	
	<script>

		function deployPackage(publicationID)
		{
			if (confirm("All files in this publication will be deployed, not just the ones in this package. Continue?") == true)
			{
				window.location.href = "DownloadOrDeployPublication.aspx?target=" + publicationID + "&deployPublication=1&packageName=<%= System.Web.HttpUtility.UrlEncode(Target) %>"
			}
		}

		function pageLoad()
		{
			if(<%= PublicationDeployed.ToString().ToLower() %> == true)
			{
				alert("Publication was deployed successfully.");
				window.location.href = window.location.href.toString().replace("publicationDeployed=1", "publicationDeployed=0");
			}

			if(<%= PublicationDeployFailed.ToString().ToLower() %> == true)
			{
				alert("Publication deployment failed.");
				window.location.href = window.location.href.toString().replace("publicationDeployFailed=1", "publicationDeployFailed=0");
			}
		}
	
	</script>	
		
</asp:Content>
