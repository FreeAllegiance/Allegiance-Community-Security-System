<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AliasIframe.aspx.cs" Inherits="Allegiance.CommunitySecuritySystem.Management.Users.AliasIframe" %>

<%@ Register src="UI/UserControls/AliasDetail.ascx" tagname="AliasDetail" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

	<asp:ScriptManager ID="ScriptManager1" runat="server">
		<Scripts>
			<asp:ScriptReference Path="~/Scripts/jquery-1.4.2.min.js" ScriptMode="Auto" />
		</Scripts>
	</asp:ScriptManager>

    <div>
    	<uc1:AliasDetail ID="ucAliasDetail" runat="server" />
    </div>
    </form>
</body>
</html>
