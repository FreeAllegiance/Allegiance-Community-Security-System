<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/MasterPages/Default.Master" CodeBehind="ManageLinks.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Users.ManageLinks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<asp:Panel ID="pLinkedLogins" runat="server">
		<h3>Linked Logins for: <asp:Label ID="lblPrimaryLogin" runat="server" Text="" CssClass="loginName"></asp:Label></h3>
		<hr />
		<asp:Label ID="lblErrorMessage" runat="server" Text="" CssClass="errorText"></asp:Label>
		
		<asp:GridView ID="gvLinkedLogins" runat="server" CssClass="fullWidthGrid nowrapHeaders nowrapCells" 
			AutoGenerateColumns="False">
			<Columns>
				<asp:BoundField DataField="Username" HeaderText="Login" ItemStyle-CssClass="primaryColumn" />
				<asp:BoundField DataField="Email" HeaderText="Email" />
				<asp:BoundField DataField="DateCreated" HeaderText="Date Created" DataFormatString="{0:d} - {0:T}" />
				<asp:TemplateField HeaderText="Unlink">
					<ItemTemplate>
						<asp:LinkButton ID="lnkUnlink" runat="server" CommandName="unlink" CommandArgument='<%# Eval("Id") %>'
							Text="Unlink" OnCommand="lnkUnlink_Click"></asp:LinkButton>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Merge">
					<ItemTemplate>
						<asp:LinkButton ID="lnkMerge" runat="server" CommandName="merge" CommandArgument='<%# Eval("Id") %>'
							Text="Merge" OnCommand="lnkMerge_Click" OnClientClick='return verifyMerge(this);'></asp:LinkButton>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Permanent Unlink">
					<ItemTemplate>
						<asp:LinkButton ID="lnkPermanentUnlink" runat="server" 
							Text="Permanent Unlink" OnCommand="lnkPermanentUnlink_Click" CommandName="permanentUnlink" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>
		<br />
		<br />
	</asp:Panel>
	
	<asp:Panel ID="pPermanentUnlinks" runat="server">
		<h3>Permanent Unlinks for: <asp:Label ID="lblPermanentUnlinkPrimaryLogin" runat="server" Text=""></asp:Label></h3>
		<hr />
		<asp:GridView ID="gvPermanentUnlinks" runat="server" CssClass="fullWidthGrid nowrapHeaders nowrapCells" AutoGenerateColumns="False" >
			<Columns>
				<asp:BoundField DataField="Username" HeaderText="Login" ItemStyle-CssClass="primaryColumn"/>
				<asp:BoundField DataField="Email" HeaderText="Email" />
				<asp:BoundField DataField="DateCreated" HeaderText="Date Created" DataFormatString="{0:d} - {0:T}" />
				<asp:TemplateField HeaderText="Remove Permanent Unlink">
					<ItemTemplate>
						<asp:LinkButton ID="lnkRemovePermanentUnlink" runat="server" CommandName="removePermanentunlink" CommandArgument='<%# Eval("Id") %>'
							Text="Remove Permanent Unlink" OnCommand="lnkRemovePermanentUnlink_Command"></asp:LinkButton>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>
		<br />
		<br />
	</asp:Panel>
	<hr />
	<center>
		<asp:Button ID="btnLinkAccount" runat="server" Text="Link To Another Login" 
			onclick="btnLinkAccount_Click" />
		&nbsp;&nbsp;&nbsp;
		<asp:Button ID="btnCancel" runat="server" Text="Cancel" 
			onclick="btnCancel_Click" />
	</center>

	<script type="text/javascript">
		function verifyMerge(source)
		{
			var usernameToMerge = $($(source).parent().parent().children()[0]).text();
			var primaryLoginName = $(".loginName").text();

			return confirm("Are you sure you wish to merge this login with " + usernameToMerge + "?\n\n All callsigns for " + usernameToMerge + " will become owned by " + primaryLoginName + ".");
		}
		


	</script>
	
</asp:Content>
