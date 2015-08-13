using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Allegiance.CommunitySecuritySystem.Common.Envelopes.AuthInfo;

namespace Allegiance.CommunitySecuritySystem.Common.Utility
{
	public class EnumBinder
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="enumTypeToBind">Because of a limitation in the ObjectDataSource control, this is passed as a string. This parameter should be the full typename of the enum to bind.</param>
		/// <returns></returns>
		public Dictionary<int, string> GetAllValues(string enumTypeToBind)
		{
			Dictionary<int, string> returnValue = new Dictionary<int, string>();

			Type enumType = Type.GetType(enumTypeToBind);

			foreach (int key in Enum.GetValues(enumType))
				returnValue.Add(key, Enum.GetName(enumType, key));
		
			return returnValue;
		}
	}
}