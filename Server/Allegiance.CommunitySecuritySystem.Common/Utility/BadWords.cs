using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Allegiance.CommunitySecuritySystem.Common.Utility
{
	public class BadWords
	{
		private static List<String> _wordList = new List<string>();

		static BadWords()
		{
			// Taken from R5 clientlib\badwords.cpp in allegiance codebase.
			string badWords = "zone55|anus|a n u s|asshole|a s s h o l e|bastard|bitch|b i t c h|blowjob|b l o w j o b|clit|c l i t|cock|c0ck|cocksucker|c u m|cunt|c u n t|dick|dildo|d i l d o|faggot|f a g g o t|fatass|f uck|fugly|fux0r|fuckyou|fuck_you|fuck_u|fucku|fucka|fuckit|fuckthis|fuckme|fucker|fuckr|fucking|fuckin|fuckn|motherfucker|motherfuck|mutherfucker|fucked|f_u_c|f_ck|f_k|fahq|fck|fkyou|fu_k|fuc|fuck|f u c k|fuhk|fuk|fuq|f__c|f__k|f__u|fuh_q|gay|genital|hitler|jackoff|jism|l3itch|lesbian|masterbat|mofo|nazi|n a z i|niger|nigr|nigga|niggr|nigger|n i g g e r|nutsack|orgy|pecker|penis|p e n i s|phaq|phuc|phuk|phuck|phuq|phvc|phvk|phvq|phallus|pimp|prick|p r i c k|puss|pussy|p u s s y|s.o.b|scrotum|schit|sh1t|shit|s h i t|shlt|shyt|slut|testical|tits|t i t s|vagina|wank|whore|whoar";

			_wordList.Clear();

			foreach (string badWord in badWords.Split('|'))
			{
				if (String.IsNullOrEmpty(badWord))
					continue;

				_wordList.Add(badWord);
			}
		}

		public static bool ContainsBadWord(string stringToTest)
		{
			string lowerCaseStringToTest = stringToTest.ToLower();

			foreach (string badWord in _wordList)
			{
				if (lowerCaseStringToTest.Contains(badWord) == true)
					return true;
			}

			return false;
		}
	}
}
