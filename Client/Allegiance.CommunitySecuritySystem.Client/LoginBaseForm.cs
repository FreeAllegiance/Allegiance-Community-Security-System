using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Allegiance.CommunitySecuritySystem.Client
{
	public class LoginBaseForm : Form
	{
		protected delegate void ShowOfflineLaunchOptionDelegate(string accountAttemptingToLogin, string oldestLinkedAccount, bool closeParentWindow);
		protected void ShowOfflineLaunchOption(string accountAttemptingToLogin, string oldestLinkedAccount, bool closeParentWindow)
		{
			if (this.InvokeRequired == true)
			{
				this.Invoke(new ShowOfflineLaunchOptionDelegate(ShowOfflineLaunchOption), accountAttemptingToLogin, oldestLinkedAccount, closeParentWindow);
				return;
			}

			var offlineLaunch = new OfflineLaunch("Your account: " + accountAttemptingToLogin + " has been linked with another account: " + oldestLinkedAccount + ". Please log in with this account instead. If you need assistance, please use the below help link.", "http://www.freeallegiance.org/FAW/index.php/Linked_account", "Get More Information Online.");
			offlineLaunch.TopMost = true;
			offlineLaunch.ShowDialog();

			if (closeParentWindow == true)
			{
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				this.Close();
				//this.CloseForm();
			}
		}

		protected delegate void ShowVirtualMachineInfoDelegate(bool closeParentWindow);
		protected void ShowVirtualMachineInfo(bool closeParentWindow)
		{
			if (this.InvokeRequired == true)
			{
				this.Invoke(new ShowVirtualMachineInfoDelegate(ShowVirtualMachineInfo), closeParentWindow);
				return;
			}

			var virtualMachineInfo = new VirtualMachineInfo();
			virtualMachineInfo.TopMost = true;
			virtualMachineInfo.ShowDialog();

			if (closeParentWindow == true)
			{
				this.DialogResult = System.Windows.Forms.DialogResult.Abort;
				//this.CloseForm();
			}
		}

		//protected delegate void CloseFormDelegate();
		//protected void CloseForm()
		//{
		//    if (this.InvokeRequired == true)
		//    {
		//        this.Invoke(new CloseFormDelegate(CloseForm));
		//        return;
		//    }

		//    try
		//    {
		//        this.Close();
		//    }
		//    catch
		//    {
		//        // The form was already closed.
		//    }
		//}
	}
}
