using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Allegiance.CommunitySecuritySystem.DataAccess
{
	public partial class Captcha
	{
		/// <summary>
		/// The time in seconds before a captcha becomes invalid.
		/// </summary>
		public const int CaptchaExpirationTimeInSeconds = 300;

		/// <summary>
		/// The number of characters to return for the randomly generated captcha.
		/// </summary>
		public const int CaptchaLength = 6;

		public static void RemoveExpiredCaptchas()
		{
			using (CSSDataContext db = new CSSDataContext())
			{
				var timeout = DateTime.Now.AddSeconds(-CaptchaExpirationTimeInSeconds);
				var oldCaptchas = db.Captchas.Where(p => p.DateCreated < timeout);
				db.Captchas.DeleteAllOnSubmit(oldCaptchas);
				db.SubmitChanges();
			}
		}

		public static bool CheckCaptcha(Guid captchaToken, string captchaAnswer)
		{
			bool isCaptchaValid = false;

			using (CSSDataContext db = new CSSDataContext())
			{
				var timeout = DateTime.Now.AddSeconds(-CaptchaExpirationTimeInSeconds);
				var captcha = db.Captchas.FirstOrDefault(p => p.Id == captchaToken && p.Answer == captchaAnswer && p.DateCreated > timeout);
				if (captcha != null)
				{
					isCaptchaValid = true;
					db.Captchas.DeleteOnSubmit(captcha);
					db.SubmitChanges();
				}
			}

			return isCaptchaValid;
		}

		public static void GetNewCaptchaAnswer(string requestorIpAddress, out Guid captchaToken, out string captchaAnswer)
		{
			captchaToken = Guid.NewGuid();
			captchaAnswer = Path.GetRandomFileName().Substring(0, CaptchaLength);

			using (CSSDataContext db = new CSSDataContext())
			{
				db.Captchas.InsertOnSubmit(new Captcha()
					{
						DateCreated = DateTime.Now,
						Id = captchaToken,
						Answer = captchaAnswer,
						IpAddress = requestorIpAddress
					});

				db.SubmitChanges();
			}
		}
	}
}
