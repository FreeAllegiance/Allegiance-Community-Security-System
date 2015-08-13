using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace Allegiance.CommunitySecuritySystem.Server.Providers
{
    public class HostFactory : ServiceHostFactory
    {
        #region Methods

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var customServiceHost = new ServiceHost(serviceType, GetBaseAddress(serviceType));
            return customServiceHost;
        }

        private static Uri[] GetBaseAddress(Type serviceType)
        {
            var addresses = new List<Uri>();
            AddBaseAddress(addresses, serviceType);
            return addresses.ToArray();
        }

        private static void AddBaseAddress(List<Uri> addresses, Type serviceType)
        {
            var url = ConfigurationManager.AppSettings["serviceBaseAddress"];

            if (!url.EndsWith("/"))
                url = string.Concat(url, "/");

            url = string.Concat(url, serviceType.Name, ".svc");

            addresses.Add(new Uri(url));
        }

        #endregion
    }
}