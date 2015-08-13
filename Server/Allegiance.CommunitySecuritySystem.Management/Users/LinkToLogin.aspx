<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="LinkToLogin.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Users.LinkToLogin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<h3>Linking To Login: 
	<asp:Label ID="lblTargetLogin" runat="server" Text=""></asp:Label></h3>

Enter Login Name (Wildcard: %)<br />
	<table>
		<tr>
			<td>
				<asp:TextBox ID="txtSearch" runat="server" Style="width: 250px;" 
					ontextchanged="txtSearch_TextChanged"></asp:TextBox>
			</td>
			<td>
				<asp:Button ID="btnSearch" runat="server" Text="Search" 
					onclick="btnSearch_Click" />
			</td>
			<td>
			</td>
			<td>
			</td>
		</tr>
	</table>
	<br />
	<asp:GridView ID="gvLogins" runat="server" AutoGenerateColumns="False" 
		Visible="False" CssClass="fullWidthGrid">
		<Columns>
			<asp:BoundField DataField="Username" HeaderText="User Name" />
			<asp:BoundField DataField="Email" HeaderText="Email" />
			<asp:BoundField DataField="DateCreated" DataFormatString="{0:d} - {0:T}" 
				HeaderText="Date Created" />
			<asp:BoundField DataField="LastLogin" DataFormatString="{0:d} - {0:T}" 
				HeaderText="LastLogin" />
			<asp:TemplateField HeaderText="Select">
				<ItemTemplate>
					<asp:LinkButton ID="lnkSelect" runat="server" CommandName="linkSelect" CommandArgument='<%# Eval("Id") %>' 
						oncommand="lnkSelect_Command">Link</asp:LinkButton>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
	<br />
	<br />
	<hr />
	<center>
		<asp:Button ID="btnCancel" runat="server" Text="Cancel" 
			onclick="btnCancel_Click" />
	</center>

</asp:Content>
