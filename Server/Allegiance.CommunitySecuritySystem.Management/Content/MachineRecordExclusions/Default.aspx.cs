using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allegiance.CommunitySecuritySystem.DataAccess;

namespace Allegiance.CommunitySecuritySystem.Management.Content.MachineRecordExclusions
{
	public partial class Default : UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			lblErrorText.Text = String.Empty;
		}

		protected void btnSaveNewMachineRecordExclusion_Click(object sender, EventArgs e)
		{
			using(CSSDataContext db = new CSSDataContext())
			{
				if (db.MachineRecordExclusions.FirstOrDefault(p => p.IdentifierMask == txtIdentifier.Text && p.RecordTypeId == Int32.Parse(ddlDeviceType.SelectedValue)) != null)
				{
					lblErrorText.Text = "The identifier you specified: " + txtIdentifier.Text + " already exists.";
					return;
				}

				db.MachineRecordExclusions.InsertOnSubmit(new MachineRecordExclusion()
				{
					IdentifierMask = txtIdentifier.Text,
					RecordTypeId = Int32.Parse(ddlDeviceType.SelectedValue)
				});

				db.SubmitChanges();

				this.DataBind();
			}
		}

		protected void ddlDeviceType_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

	}
}