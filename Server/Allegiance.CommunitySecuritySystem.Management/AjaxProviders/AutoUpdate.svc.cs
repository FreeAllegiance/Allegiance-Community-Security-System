using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Allegiance.CommunitySecuritySystem.Management.Business;
using System.Security.Principal;
using System.Web;
using System.IO;

namespace Allegiance.CommunitySecuritySystem.Management.AjaxProviders
{
	[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
	[ServiceContract(Namespace = "AjaxProviders")]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class AutoUpdate
	{
		private void CheckAccess()
		{
			IPrincipal principal = HttpContext.Current.User;

			if (Business.Authorization.IsAdminOrSuperAdmin(HttpContext.Current.User) == false)
				throw new Exception("Access is denied.");
		}

		[OperationContract]
		public void SetFileInclusionForPackage(string packageName, string relativeDirectory, string fileName, bool isIncluded)
		{
			CheckAccess();

			AutoUpdateManager.SetFileInclusionForPackage(packageName, relativeDirectory, fileName, isIncluded);
		}

		[OperationContract]
		public void SetFileProtectionForPackage(string packageName, string relativeDirectory, string fileName, bool isProtected)
		{
			CheckAccess();

			AutoUpdateManager.SetFileProtectionForPackage(packageName, relativeDirectory, fileName, isProtected);
		}

		[OperationContract]
		public void SetPackageInclusionForPublication(int publicationID, string packageName, bool isIncluded)
		{
			CheckAccess();

			AutoUpdateManager.SetPackageInclusionForPublication(publicationID, packageName, isIncluded);
		}

		[OperationContract]
		public bool CreateBackup(string backupName)
		{
			CheckAccess();

			return AutoUpdateManager.CreateBackup(backupName);
		}

		[OperationContract]
		public bool DeletePackage(string packageName)
		{
			CheckAccess();

			return AutoUpdateManager.DeletePackage(packageName);
		}
	}
}
