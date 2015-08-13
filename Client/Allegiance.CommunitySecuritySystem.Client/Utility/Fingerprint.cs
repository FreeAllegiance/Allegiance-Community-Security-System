using System;
using System.Collections.Generic;
using System.Management;
using System.Text;
using Microsoft.Win32;

namespace Allegiance.CommunitySecuritySystem.Client.Utility
{
    internal static class Fingerprint
    {
        #region Methods

        /// <summary>
        /// Gathers all unique hardware information
        /// </summary>
        public static void GatherAll(out List<string> macs, out List<string> edids, out List<string> disks)
        {
            macs    = RetrieveMAC();
            edids   = RetrieveEDIDs();
            disks   = RetrieveDisks();
        }

        /// <summary>
        /// Retrieve machine MAC address
        /// </summary>
        /// <returns></returns>
        private static List<string> RetrieveMAC()
        {
            return GetWMIObjects("Win32_NetworkAdapterConfiguration",  "MacAddress", "Description");
        }

        /// <summary>
        /// Retrieves list of harddisks.
        /// </summary>
        private static List<string> RetrieveDisks()
        {
            List<string> diskInfos = GetWMIObjects("Win32_DiskDrive", "Signature", "Caption", "Index", "Model");

			diskInfos.AddRange(GetLogicalDiskInfos());

			return diskInfos;
        }

		private static List<string> GetLogicalDiskInfos()
		{
			const string root = @"HARDWARE\DEVICEMAP\Scsi";

			List<string> returnValue = new List<string>();

			using (var key = Registry.LocalMachine.OpenSubKey(root))
			{
				Log.Write("opened: " + root);

				if (key == null)
					return returnValue;

				foreach (string scsiPortKeyName in key.GetSubKeyNames())
				{
					Log.Write(scsiPortKeyName);

					using(var scsiPortKey = key.OpenSubKey(scsiPortKeyName))
					{
						if (scsiPortKey == null)
							continue;

						foreach (string scsiBusKeyName in scsiPortKey.GetSubKeyNames())
						{
							Log.Write(scsiBusKeyName);

							using (var scsiBusKey = scsiPortKey.OpenSubKey(scsiBusKeyName))
							{
								if (scsiBusKey == null)
									continue;

								foreach (string scsiBusObjectName in scsiBusKey.GetSubKeyNames())
								{
									Log.Write(scsiBusObjectName);

									if (scsiBusObjectName.StartsWith("Target") == true)
									{
										using (var scsiBusObject = scsiBusKey.OpenSubKey(scsiBusObjectName))
										{
											if (scsiBusObject == null)
												continue;

											foreach (string logicalUnitName in scsiBusObject.GetSubKeyNames())
											{
												Log.Write(logicalUnitName);

												using (var logicalUnit = scsiBusObject.OpenSubKey(logicalUnitName))
												{
													if (logicalUnit == null)
														continue;

													var value = logicalUnit.GetValue("Identifier");
													if (value != null)
													{
														string cleanValue = String.Empty;
														foreach (var letter in value.ToString().ToCharArray())
														{
															if (Char.IsLetterOrDigit(letter) == true)
																cleanValue += letter;
															else
																cleanValue += " ";
														}

														Log.Write("value: " + value + ", cleanValue: " + cleanValue);
														returnValue.Add("LogicalDisk:" + cleanValue.ToString());
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}

			return returnValue;
		}

        /// <summary>
        /// Retrieve all EDIDs from the monitors attached to this machine
        /// </summary>
        /// <returns></returns>
        private static List<string> RetrieveEDIDs()
        {
            const string root = @"System\CurrentControlSet\Enum\DISPLAY";

            var results = new List<string>();

            using (var key = Registry.LocalMachine.OpenSubKey(root))
            {
				if (key != null)
				{
					foreach (var subkeyName in key.GetSubKeyNames())
					{
						using (var subkey = key.OpenSubKey(subkeyName))
						{
							if (subkey != null)
							{
								var value = RetrieveMonitorEDIDs(subkey);
								if (value != null && value.Count > 0)
									results.AddRange(value);
							}
						}
					}
				}
            }

			// This will detect VirtualBox v4.0, maybe other versions as well.
			using (var key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System"))
			{
				if (key != null)
				{
					var videoBiosVersion = key.GetValue("VideoBiosVersion") as String[];
					if (videoBiosVersion != null && videoBiosVersion.Length > 0)
						results.Add(videoBiosVersion[0]);
				}
			}

			// This will detect VMWare 7+
			using (var key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS"))
			{
				if (key != null)
				{
					var systemManufacturer = key.GetValue("SystemManufacturer") as String;
					if (systemManufacturer != null && systemManufacturer.Length > 0)
						results.Add("BIOS-SystemManufacturer: " + systemManufacturer);
				}
			}

            return results;
        }

        /// <summary>
        /// Retrieves a monitor's distinct EDIDs
        /// </summary>
        /// <returns></returns>
        private static List<string> RetrieveMonitorEDIDs(RegistryKey displayKey)
        {
            var results = new List<string>();

            var subkeys = displayKey.GetSubKeyNames();
            foreach (var subkeyName in subkeys)
            {
                using (var subkey = displayKey.OpenSubKey(subkeyName))
                {
                    if (subkey == null)
                        continue;

                    using (var deviceParameters = subkey.OpenSubKey("Device Parameters"))
                    {
                        if (deviceParameters == null)
                            continue;

                        var result = deviceParameters.GetValue("EDID") as byte[];
                        if (result != null && result.Length > 0)
                        {
                            var value = Convert.ToBase64String(result);
                            if(!results.Contains(value))
                                results.Add(value);
                        }
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Retrieves hardware identifiers.
        /// </summary>
        private static List<string> GetWMIObjects(string wmiClass, string required, params string[] wmiProperties)
        {
            var result = new List<string>();

            using (var managementClass      = new ManagementClass(wmiClass))
            using (var managementCollection = managementClass.GetInstances())
            {
                foreach (var mo in managementCollection)
                {
                    var sb = new StringBuilder();

                    var required_property = mo[required];
                    if (required_property != null)
                        sb.AppendFormat("{0}|", required_property);
                    else
                        continue;

					//if (mo.ToString().Contains("Disk") == true)
					//{
					//    foreach (var wmiProperty in mo.Properties)
					//    {
					//        System.Diagnostics.Debug.WriteLine(wmiProperty.Name + ": " + mo[wmiProperty.Name]);
					//    }
					//}

                    foreach (var wmiProperty in wmiProperties)
                    {
                        var property = mo[wmiProperty];
                        if (property != null)
                            sb.AppendFormat("{0}|", property);
                    }

                    var value = sb.ToString();
                    if (!string.IsNullOrEmpty(value))
                        result.Add(value.Substring(0, value.Length - 1));
                }

                return result;
            }
        }

        #endregion
    }
}
