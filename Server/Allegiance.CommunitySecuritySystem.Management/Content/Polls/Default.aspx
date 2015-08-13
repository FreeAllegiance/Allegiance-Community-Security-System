<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Content.Polls.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<br />
<asp:Button ID="btnAddPoll" runat="server" Text="Add New Poll" 
		onclick="btnAddPoll_Click" />
<hr />
	<asp:GridView ID="gvPolls" runat="server" AutoGenerateColumns="False" 
		Visible="True" CssClass="fullWidthGrid">
		<Columns>
			<asp:BoundField DataField="ShortQuestion" HeaderText="Question" />
			<asp:BoundField DataField="DateCreated" HeaderText="Date Created" DataFormatString="{0:d} - {0:T}" />
			<asp:BoundField DataField="DateExpires" HeaderText="Date Expires" DataFormatString="{0:d} - {0:T}" />
			<asp:TemplateField HeaderText="Edit">
				<ItemTemplate>
					<a href="EditPoll.aspx?pollID=<%# Eval("Id") %>">Edit</a>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Edit">
				<ItemTemplate>
					<a href="javascript:deletePoll(<%# Eval("Id") %>, '<%# Eval("Question") %>')">Delete</a>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>

	<script type="text/javascript" language="javascript">

		function deletePoll(pollID, question)
		{
			if (confirm("Are you sure you want to delete this poll: \n\n" + question) == true)
			{
				document.location.href = "Default.aspx?deletePoll=" + pollID;
			}
		}

	</script>

</asp:Content>
