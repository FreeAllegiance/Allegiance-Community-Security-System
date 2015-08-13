using System;
using System.Collections.Generic;
using System.Management;
using System.Text;
using Microsoft.Win32;

namespace Allegiance.CommunitySecuritySystem.PrototypeClient
{
    public static class Fingerprint
    {
        #region Methods

        /// <summary>
        /// Gathers all unique hardware information
        /// </summary>
        public static void GatherAll()
        {
            var macs    = RetrieveMAC();
            var edids   = RetrieveEDIDs();
            var disks   = RetrieveDisks();

            //DEBUG
            macs.Insert(0, "MAC Addresses:");
            macs.Insert(0, string.Empty);
            edids.Insert(0, "Monitors:");
            edids.Insert(0, string.Empty);
            disks.Insert(0, "Disks:");
            disks.Insert(0, string.Empty);
            PrintAll(macs, edids, disks);
        }

        //DEBUG
        private static void PrintAll(params IEnumerable<string>[] values)
        {
            foreach (var value in values)
            {
                foreach (var str in value)
                    Console.WriteLine(str);
            }
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
            return GetWMIObjects("Win32_DiskDrive", "Signature", "SerialNumber", "Index", "Model");
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
                foreach (var subkeyName in key.GetSubKeyNames())
                {
                    using (var subkey = key.OpenSubKey(subkeyName))
                    {
                        var value = RetrieveMonitorEDIDs(subkey);
                        if (value != null && value.Count > 0)
                            results.AddRange(value);
                    }
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