using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Xml;

namespace Allegiance.CommunitySecuritySystem.Server.Providers
{
	public class WebServiceHostFactory : System.ServiceModel.Activation.WebServiceHostFactory
	{
		public WebServiceHostFactory() : base()
    {
    }

		public override System.ServiceModel.ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
		{
			var returnValue = base.CreateServiceHost(constructorString, baseAddresses);

			FixupConfig(returnValue.Description);

			return returnValue;
		}

		private void FixupConfig(System.ServiceModel.Description.ServiceDescription serviceDescription)
		{
			foreach (var endpoint in serviceDescription.Endpoints)
			{ 
				var binding = endpoint.Binding;
				var web = binding as WebHttpBinding;

				if (web != null)
				{
					web.MaxBufferSize = 2147483647;
					web.MaxReceivedMessageSize = 2147483647;
				}

				var myReaderQuotas = new XmlDictionaryReaderQuotas { MaxStringContentLength = 2147483647 };

				binding.GetType().GetProperty("ReaderQuotas").SetValue(binding, myReaderQuotas, null);
			}
		}

		protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
		{
			var returnValue = base.CreateServiceHost(serviceType, baseAddresses);

			FixupConfig(returnValue.Description);

			return returnValue;
		}

	//protected override void  OnOpening()
	//{
	//    base.OnOpening();

	//    foreach (var endpoint in this.Description.Endpoints)
	//    {
	//        var binding = endpoint.Binding;
	//        var web = binding as WebHttpBinding;

	//        if (web != null)
	//        {
	//            web.MaxBufferSize = 2147483647;
	//            web.MaxReceivedMessageSize = 2147483647;
	//        }

	//        var myReaderQuotas = new XmlDictionaryReaderQuotas { MaxStringContentLength = 2147483647 };

	//        binding.GetType().GetProperty("ReaderQuotas").SetValue(binding, myReaderQuotas, null);
	//    }
	//}

	}
}