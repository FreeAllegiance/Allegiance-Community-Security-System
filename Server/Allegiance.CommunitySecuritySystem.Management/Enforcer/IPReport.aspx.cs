using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Allegiance.CommunitySecuritySystem.Management.Enforcer
{
	public partial class IPReport : UI.Page
	{
		private int LoginID
		{
			get
			{
				return Int32.Parse(Request.Params["loginID"]);
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Business.Authorization.IsZoneLeadOrAdminOrSuperAdmin(User) == false)
				Response.Redirect("~/Default.aspx");

			if (Master.Breadcrumb != null)
				Master.Breadcrumb.Visible = true;

			using (DataAccess.CSSDataContext db = new DataAccess.CSSDataContext())
			{
				var login = db.Logins.FirstOrDefault(p => p.Id == LoginID);

				if (login == null)
					throw new Exception("Couldn't look up login by id: " + LoginID);

				List<Business.IPReportItem> exactMatches = new List<Business.IPReportItem>();
				List<Business.IPReportItem> subnetMatches = new List<Business.IPReportItem>();

				foreach(var loggedIP in login.Identity.LogIPs)
				{
					foreach(var matchedUser in db.LogIPs.Where(p => p.IdentityId != login.IdentityId && p.IPAddress == loggedIP.IPAddress))
					{
						exactMatches.Add(new Business.IPReportItem()
						{
							IPAddress = loggedIP.IPAddress,
							User1 = login.Identity.Logins.OrderBy(p => p.DateCreated).FirstOrDefault().Username,
							User1Date = loggedIP.LastAccessed,
							User2 = matchedUser.Identity.Logins.OrderBy(p => p.DateCreated).FirstOrDefault().Username,
							User2Date = matchedUser.LastAccessed
						});
					}

					string subNetLabel = loggedIP.IPAddress.Substring(0, loggedIP.IPAddress.LastIndexOf('.')) + ".*";
					string subNetPart = loggedIP.IPAddress.Substring(0, loggedIP.IPAddress.LastIndexOf('.')) + ".%";

					foreach (var subnetMatch in db.LogIPs.Where(p => p.IdentityId != login.IdentityId && System.Data.Linq.SqlClient.SqlMethods.Like(p.IPAddress, subNetPart) == true))
					{
						subnetMatches.Add(new Business.IPReportItem()
						{
							IPAddress = subNetLabel,
							User1 = login.Identity.Logins.OrderBy(p => p.DateCreated).FirstOrDefault().Username,
							User1Date = loggedIP.LastAccessed,
							User2 = subnetMatch.Identity.Logins.OrderBy(p => p.DateCreated).FirstOrDefault().Username,
							User2Date = subnetMatch.LastAccessed
						});
					}
				}

				gvUserIpMatches.DataSource = exactMatches;
				gvUserIpMatches.DataBind();

				gvUserSubnetMatches.DataSource = subnetMatches;
				gvUserSubnetMatches.DataBind();
			}
		}
	}
}