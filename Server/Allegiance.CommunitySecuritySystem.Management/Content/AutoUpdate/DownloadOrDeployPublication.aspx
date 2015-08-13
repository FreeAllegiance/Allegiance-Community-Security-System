<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="DownloadOrDeployPublication.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.DownloadOrDeployPublication" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:Panel ID="pFileCollisions" runat="server">
		There were <asp:Label ID="lblCollisionCount" runat="server" Text=""></asp:Label> filename collisons.
		<hr />
		<asp:GridView ID="gvCollisions" runat="server" AutoGenerateColumns="False" CssClass="fullWidthGrid">
			<Columns>
				<asp:BoundField DataField="CollidingFile" HeaderText="Colliding File" />
				<asp:BoundField DataField="PreferredFile" HeaderText="Preferred File" />
			</Columns>
		</asp:GridView>
		<br />
		<br />
		<asp:Panel ID="pDownloadMergedPublications" runat="server">
			<center>
				<a href="<%= PublicationDownloaderUrl %>">Download Merged Publication</a>
				&nbsp;&nbsp;
				<a href="EditPublication.aspx?target=<%= PublicationID %>">Cancel Download</a>
			</center>
		</asp:Panel>
		<asp:Panel ID="pPublishMergedPublications" runat="server">
			<center>
				<asp:Button ID="btnDeployMergedPublication" runat="server" 
					Text="Deploy Merged Publication" onclick="btnDeployPublication_Click" />
				&nbsp;&nbsp;
				<asp:Button ID="btnCancelMergedPublicationDeployment" runat="server" Text="Cancel Deployment" 
				onclick="btnCancel_Click" />
			</center>
		</asp:Panel>
	</asp:Panel>
	<asp:Panel ID="pNoFileCollisions" runat="server">
		<br />
		<h3>No filename collisions detected.</h3>
		<br />
		Your download will begin shortly. <a href="<%= PublicationDownloaderUrl %>">Click here</a> if it does not.
		<hr />
		<a href="EditPublication.aspx?target=<%= PublicationID %>">Return to Publication</a>
		
		<iframe style="display: none;" id="iframeDownloader" src="<%= PublicationDownloaderUrl %>"></iframe>

	</asp:Panel>
	

</asp:Content>
