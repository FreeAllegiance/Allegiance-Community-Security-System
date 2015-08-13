using System;
using System.Collections.Generic;
using System.Text;
using Allegiance.CommunitySecuritySystem.Client.Utility;
using Allegiance.CommunitySecuritySystem.Client.Integration;

namespace Allegiance.CommunitySecuritySystem.Client.Service
{
	static class Authentication
	{
		//private static ClientService.ClientService GetNewClientService()
		//{
		//    ClientService.ClientService clientService = new ClientService.ClientService();
		//    clientService.Url = AllegianceRegistry.ClientService;

		//    return clientService;
		//}

		public static void GetCaptchaAsync(int width, int height, ClientService.GetCaptchaCompletedEventHandler onGetCaptchaComplete)
		{
			ClientService.ClientService clientService = ServiceHandler.Service;

			clientService.GetCaptchaCompleted += new ClientService.GetCaptchaCompletedEventHandler(onGetCaptchaComplete);

			clientService.GetCaptchaAsync(width, true, height, true);
		}

		public static void CreateNewAccount(string username, string password, string email, string captchaToken, string captchaAnswer, ClientService.CreateLoginCompletedEventHandler onCreateLoginCompleted)
		{
			ClientService.ClientService clientService = ServiceHandler.Service;

			clientService.CreateLoginCompleted += onCreateLoginCompleted;

			clientService.CreateLoginAsync(username, password, email, captchaToken, captchaAnswer);
		}
	}
}
