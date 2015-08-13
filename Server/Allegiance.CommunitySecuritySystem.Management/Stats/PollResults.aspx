<%@ Page Title="" Language="C#" MasterPageFile="~/UI/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="PollResults.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Stats.PollResults" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<h3>Recent Poll Results</h3>
<hr />
	<asp:GridView ID="gvPolls" runat="server" AutoGenerateColumns="False" 
		DataSourceID="ldsPolls" EnableModelValidation="True">
		<Columns>
			<asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" 
				SortExpression="Id" />
			<asp:BoundField DataField="Question" HeaderText="Question" 
				ReadOnly="True" SortExpression="Question" />
			<asp:BoundField DataField="DateCreated" HeaderText="Date" 
				ReadOnly="True" SortExpression="DateCreated" DataFormatString="{0:MMM-dd-yyyy}" />
			<asp:TemplateField HeaderText="PollOptions" SortExpression="PollOptions" >
				<ItemTemplate>
					<asp:Repeater DataSource='<%# Eval("PollOptions") %>' runat="server" ID="rptPollOptions">
						<HeaderTemplate>
							<table style="width: 100%;">
						</HeaderTemplate>
						<ItemTemplate>
								<tr>
									<td><%# Eval("Option") %></td>
									<td style="width: 10px;"><%# Eval("VoteCount") %></td>
									<td style="white-space: nowrap; height: 40px;  width: 100px; text-align: center;">
										<div style="position: relative;">
											<div style="position: absolute; top: 0px; left: 0px; display: block; width: <%#Eval("VotePercentageInt32")%>%; height: 100%; background-color: Pink;"></div>
											<div style="border: 1px solid black; position: relative; color: Black;"><%#Eval("VotePercentage")%></div>
										</div>
								</tr>
						</ItemTemplate>
						<FooterTemplate>
							</table>
						</FooterTemplate>
					</asp:Repeater>
				

					<%--<asp:GridView ID="gvPollOptions" runat="server" AutoGenerateColumns="False" 
						DataSource='<%# Eval("PollOptions") %>' EnableModelValidation="True">
						<Columns>
							<asp:BoundField DataField="Option" HeaderText="Option" />
							<asp:BoundField DataField="VoteCount" HeaderText="Count" />
						</Columns>
					</asp:GridView>--%>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>



	<asp:LinqDataSource ID="ldsPolls" runat="server" 
		ContextTypeName="Allegiance.CommunitySecuritySystem.DataAccess.CSSDataContext" 
		OrderBy="DateCreated desc" 
		Select="new (Question, Id, DateCreated, PollOptions)" TableName="Polls" 
		Where="DateExpires.AddMonths(3) &gt; DateTime.Now">
	</asp:LinqDataSource>



</asp:Content>
