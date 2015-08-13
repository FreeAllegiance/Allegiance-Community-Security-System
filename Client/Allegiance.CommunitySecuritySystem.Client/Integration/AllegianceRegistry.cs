using System.Collections.Generic;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System;
using Allegiance.CommunitySecuritySystem.Client.Properties;
using System.Configuration;
using Allegiance.CommunitySecuritySystem.Client.Utility;

namespace Allegiance.CommunitySecuritySystem.Client.Integration
{
    public static class AllegianceRegistry
    {
		//[DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
		//[return: MarshalAs(UnmanagedType.Bool)]
		//public static extern bool IsWow64Process([In] IntPtr processHandle, [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process);

		[DllImport("kernel32.dll")]
		static extern IntPtr GetCurrentProcess();

        #region Fields

        private static Dictionary<string, object> _valueCache = null;

        private static string _root = @"Software\Microsoft\Microsoft Games\Allegiance\";
		private static string _64BitRoot = @"Software\Wow6432Node\Microsoft\Microsoft Games\Allegiance\";

		/*TODO: Readd this as we get closer to production release.
#if !DEBUG
        private static string _version = "1.0";
#else
        private static string _version = "1.1";
#endif
		 */

		private static string _version = "1.2";


        #endregion

        #region Public Properties

        public static bool LogChat
        {
            get { return RetrieveValue<string>(AllegianceRootRegistry, "LogChat") == "1"; }
            set
            {
                if (value == true)
                    SetValue(AllegianceRootRegistry, "LogChat", "1");
                else
                    SetValue(AllegianceRootRegistry, "LogChat", "0");
            }
        }

		public static bool OutputDebugString
		{
			get { return RetrieveValue<string>(AllegianceRootRegistry, "OutputDebugString") == "1"; }
			set
			{
				if (value == true)
					SetValue(AllegianceRootRegistry, "OutputDebugString", "1");
				else
					SetValue(AllegianceRootRegistry, "OutputDebugString", "0");
			}
		}

		public static bool LogToFile
		{
			get { return RetrieveValue<string>(AllegianceRootRegistry, "LogToFile") == "1"; }
			set
			{
				if (value == true)
					SetValue(AllegianceRootRegistry, "LogToFile", "1");
				else
					SetValue(AllegianceRootRegistry, "LogToFile", "0");
			}
		}
        
        public static string LobbyPath
        {
            get { return RetrieveValue<string>(AllegianceRootRegistry, "Lobby Path"); }
            set { SetValue(AllegianceRootRegistry, "Lobby Path", value); }
        }

		public static string EXEPath
		{
			get { return RetrieveValue<string>(AllegianceRootRegistry, "EXE Path"); }
			set { SetValue(AllegianceRootRegistry, "EXE Path", value); }
		}

        public static string ArtPath
        {
            get { return RetrieveValue<string>(AllegianceRootRegistry, "ArtPath"); }
            set { SetValue(AllegianceRootRegistry, "ArtPath", value); }
        }

		public static string BetaArtPath
		{
			get { return RetrieveValue<string>(AllegianceRootRegistry, "BetaArtPath"); }
			set { SetValue(AllegianceRootRegistry, "BetaArtPath", value); }
		}

		public static string ProductionArtPath
		{
			get { return RetrieveValue<string>(AllegianceRootRegistry, "ProductionArtPath"); }
			set { SetValue(AllegianceRootRegistry, "ProductionArtPath", value); }
		}

        public static uint Allow3DAcceleration
        {
            get { return RetrieveValue<uint>(AllegianceRootRegistry, "Allow3DAcceleration", 1); }
            set { SetValue(AllegianceRootRegistry, "Allow3DAcceleration", value); }
        }

        public static string CfgFile
        {
            get { return RetrieveValue<string>(AllegianceRootRegistry, "CfgFile"); }
            set { SetValue(AllegianceRootRegistry, "CfgFile", value); }
        }


		public static string ProductionCfgFile
		{
			get { return RetrieveValue<string>(AllegianceRootRegistry, "ProductionCfgFile"); }
			set { SetValue(AllegianceRootRegistry, "ProductionCfgFile", value); }
		}


		public static string BetaCfgFile
		{
			get { return RetrieveValue<string>(AllegianceRootRegistry, "BetaCfgFile"); }
			set { SetValue(AllegianceRootRegistry, "BetaCfgFile", value); }
		}

		public static string ClientService
		{
			get 
			{
#if DEBUG
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["ClientServiceOverride"]) == true)
					return RetrieveValue<string>(AllegianceRootRegistry, "ClientService", Settings.Default.Launcher_ClientService_ClientService);
				else
					return ConfigurationManager.AppSettings["ClientServiceOverride"];
#else 
				return RetrieveValue<string>(AllegianceRootRegistry, "ClientService", Settings.Default.Launcher_ClientService_ClientService);
#endif
			}
		}

		public static string ManagementWebRoot
		{
			get
			{
#if DEBUG
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["ManagementWebRootOverride"]) == true)
					return RetrieveValue<string>(AllegianceRootRegistry, "ManagementWebRoot", Settings.Default.ManagementWebRoot);
				else
					return ConfigurationManager.AppSettings["ManagementWebRootOverride"];
#else 
				return RetrieveValue<string>(AllegianceRootRegistry, "ManagementWebRoot", Settings.Default.ManagementWebRoot);
#endif
			}
		}

		// Fix for #61 	Win8 x64: ACSS trips on Wow6432Node redirection
		private static bool? _force64BitRoot = null;
        public static string Root
        {
            get 
			{
				if (_force64BitRoot == null)
				{
					RegistryKey key = Registry.LocalMachine.OpenSubKey(_root);
					if (key != null)
					{
						Log.Write("Using: " + _root + " for registry root.");
						_force64BitRoot = false;
						key.Close();
					}
					else
					{
						RegistryKey key64 = Registry.LocalMachine.OpenSubKey(_64BitRoot);
						if (key64 != null)
						{
							Log.Write("Using: " + _64BitRoot + " for registry root.");
							_force64BitRoot = true;
							key64.Close();
						}
						else
						{
							throw new Exception("Couldn't open: " + _root + ", or " + _64BitRoot + "!");
						}
					}
				}

				if (_force64BitRoot == true)
					return _64BitRoot;
				else
					return _root;
			}
        }

        public static string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        #endregion

        #region Private Properties

        public static string AllegianceRootRegistry
        {
            get { return string.Concat(Root, Version); }
        }

        private static Dictionary<string, object> ValueCache
        {
            get 
            { 
                if(_valueCache == null)
                    _valueCache = new Dictionary<string, object>();
                return _valueCache;
            }
        }

        #endregion

        #region Methods

		public static string GetManagementWebPath(string subPath)
		{
			string rootPath = AllegianceRegistry.ManagementWebRoot;
			if(rootPath.EndsWith("/") == false && subPath.StartsWith("/") == false)
				rootPath += "/";

			return rootPath + subPath;
		}

        private static T RetrieveValue<T>(string key, string value)
        {
            return RetrieveValue<T>(key, value, default(T));
        }

        private static T RetrieveValue<T>(string key, string value, T defaultValue)
        {
            string keyValue = string.Concat(key, value);
            if (ValueCache.ContainsKey(keyValue))
                return (T)ValueCache[keyValue];

            using (var registryKey = Registry.LocalMachine.OpenSubKey(key))
            {
				if (registryKey == null)
					throw new Exception("Couldn't open registry key: " + key + " to read value: " + value);
				
                var result = (T)registryKey.GetValue(value, defaultValue);
                ValueCache.Add(keyValue, result);

                return result;
            }
        }

        private static void SetValue(string key, string value, object parameter)
        {
            string keyValue = string.Concat(key, value);
            
            if (ValueCache.ContainsKey(keyValue))
                ValueCache.Remove(keyValue);

            using (var registryKey = Registry.LocalMachine.OpenSubKey(key, true))
            {
                registryKey.SetValue(value, parameter);
                ValueCache.Add(keyValue, parameter);
            }
        }

        #endregion

        
    }
}