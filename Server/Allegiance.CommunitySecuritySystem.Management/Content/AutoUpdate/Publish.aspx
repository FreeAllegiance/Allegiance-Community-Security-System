<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Publish.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Content.AutoUpdate.Publish" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

Current Publications<br /> 
<hr />
	<asp:GridView ID="gvPublications" runat="server" AutoGenerateColumns="False" CssClass="fullWidthGrid">
		<Columns>
			<asp:BoundField DataField="Id" HeaderText="Id" />
			<asp:BoundField DataField="Name" HeaderText="Name" 
				ItemStyle-CssClass="primaryColumn" >
			</asp:BoundField>
			<asp:BoundField DataField="Host" HeaderText="Host" />
			<asp:BoundField DataField="BasePath" HeaderText="Base Path" />
			
			
			<asp:CheckBoxField DataField="IsRestrictive" HeaderText="Restrictive" 
				ReadOnly="True" />
			<asp:CheckBoxField DataField="IsEnabled" HeaderText="Enabled" ReadOnly="True" />
			
			
			<asp:TemplateField HeaderText="Manage">
				<ItemTemplate>
					<a href="EditPublication.aspx?target=<%# Eval("Id") %>">Manage</a>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Download">
				<ItemTemplate>
					<a href="DownloadOrDeployPublication.aspx?target=<%# Eval("Id") %>">Download</a>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
	<br />

</asp:Content>



