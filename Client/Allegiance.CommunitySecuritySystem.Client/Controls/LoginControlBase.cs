using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Allegiance.CommunitySecuritySystem.Client.Controls
{
	public class LoginControlBase : UserControl
	{
		public delegate void StatusChangedHandler(string statusMessage, int percentage);
		public event StatusChangedHandler StatusChanged;

		protected void SetStatusBar(string statusMessage)
		{
			SetStatusBar(statusMessage, 0);
		}

		protected void SetStatusBar(string statusMessage, int percentage)
		{
			if (StatusChanged != null)
				StatusChanged(statusMessage, percentage);
		}

		protected bool ValidateLogin(string userName, string password)
		{
			return !string.IsNullOrEmpty(userName)
				&& !string.IsNullOrEmpty(password);
		}
	}
}
