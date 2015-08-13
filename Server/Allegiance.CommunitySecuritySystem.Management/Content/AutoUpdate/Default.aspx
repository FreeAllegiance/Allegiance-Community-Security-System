<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<center>
		<table cellpadding="5" style="text-align: left;">
			<tr>
				<td style="width: 200px; text-align: right;">
					Available Packages:
				</td>
				<td  style="width: 50px;">
					<asp:Label ID="lblAvailablePackageCount" runat="server" Text=""></asp:Label>
				</td>
				<td>
					<a href="Packages.aspx">Manage</a>
				</td>
			</tr>
			<tr>
				<td style="width: 200px; text-align: right;">
					Publication Destinations:
				</td>
				<td>
					<asp:Label ID="lblPublicationDestinationCount" runat="server" Text=""></asp:Label>
				</td>
				<td>
					<a href="Publish.aspx">Publish</a>
				</td>
			</tr>
			<tr>
				<td style="width: 200px; text-align: right;">
					Backups:
				</td>
				<td>
					<asp:Label ID="lblBackupCount" runat="server" Text=""></asp:Label>
				</td>
				<td>
					<a href="Backups.aspx">Backup / Restore</a>
				</td>
			</tr>
		</table>
	</center>


</asp:Content>
