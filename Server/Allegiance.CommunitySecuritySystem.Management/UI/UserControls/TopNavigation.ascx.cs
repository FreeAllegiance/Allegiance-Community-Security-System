using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Allegiance.CommunitySecuritySystem.Management.UI.UserControls
{
	public partial class TopNavigation : System.Web.UI.UserControl
	{
		protected bool ShowAuthenticatedOptions = false;
		protected bool ShowModeratorOptions = false;
		protected bool ShowAdministratorOptions = false;
		protected bool ShowZoneLeadOptions = false;


		protected void Page_Load(object sender, EventArgs e)
		{
			ShowAuthenticatedOptions = Business.Authorization.IsLoggedIn(Page.User);
			ShowModeratorOptions = Business.Authorization.IsModeratorOrZoneLeadOrAdminOrSuperAdmin(Page.User);
			ShowAdministratorOptions = Business.Authorization.IsAdminOrSuperAdmin(Page.User);
			ShowZoneLeadOptions = Business.Authorization.IsZoneLeadOrAdminOrSuperAdmin(Page.User);

			//foreach (MenuItem menuItem in wcMainMenu.Items)
			//{
			//    if (menuItem.Value == "Anonymous")
			//        continue;

			//    if (Business.Authorization.IsLoggedIn(Page.User) == false)
			//    {
			//        menuItem.Enabled = false;
			//    }
			//    else if (menuItem.Value.Equals("Moderator", StringComparison.InvariantCultureIgnoreCase) == true)
			//    {
			//        if (Business.Authorization.IsModeratorOrZoneLeadOrAdminOrSuperAdmin(Page.User) == false)
			//            menuItem.Enabled = false;
			//    }
			//    else if (menuItem.Value.Equals("Administrator", StringComparison.InvariantCultureIgnoreCase) == true)
			//    {
			//        if (Business.Authorization.IsAdminOrSuperAdmin(Page.User) == false)
			//            menuItem.Enabled = false;
			//    }
			//}
		}
	}
}