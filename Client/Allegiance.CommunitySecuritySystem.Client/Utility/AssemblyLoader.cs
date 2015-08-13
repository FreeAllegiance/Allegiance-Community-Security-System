using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.IO;

namespace Allegiance.CommunitySecuritySystem.Client.Utility
{
    [Serializable]
    internal class AssemblyLoader : MarshalByRefObject
    {
        #region Fields

        private static Assembly _latest = null;

        #endregion

        #region Methods

        public byte[] ValidateEntryAssembly(byte[] blackboxData)
        {
            //Create permissions
            var permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(new FileIOPermission(
                FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, 
                Assembly.GetExecutingAssembly().Location));
			permissions.AddPermission(new FileIOPermission(
				FileIOPermissionAccess.AllAccess | FileIOPermissionAccess.PathDiscovery,
				Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Log.OutputFile)));

			Log.Write("AssemblyLoader::ValidateEntryAssembly() - data length: " + ((blackboxData != null) ? blackboxData.Length : -1).ToString());

            //Gather machine information
            List<string> macs = null, edids = null, disks = null;
            if (blackboxData != null)
                Fingerprint.GatherAll(out macs, out edids, out disks);

            try
            {
                permissions.PermitOnly();

                Assembly assembly;
                if (blackboxData != null)
                {
                    assembly = Assembly.Load(blackboxData, null);
                    _latest = assembly;
                }
                else
                    assembly = _latest;

				Log.Write("AssemblyLoader::ValidateEntryAssembly() loaded assembly: " + ((assembly != null) ? assembly.GetType().ToString() : "null"));

                var validatorType   = assembly.GetType("Allegiance.CommunitySecuritySystem.Blackbox.Validator");
                var machineInfoType = assembly.GetType("Allegiance.CommunitySecuritySystem.Blackbox.MachineInformation");
                var deviceInfoType  = assembly.GetType("Allegiance.CommunitySecuritySystem.Blackbox.DeviceInfo");
                var deviceTypeType  = assembly.GetType("Allegiance.CommunitySecuritySystem.Blackbox.DeviceType");
                var machineInfo     = Activator.CreateInstance(machineInfoType);

				Log.Write("AssemblyLoader::ValidateEntryAssembly() machine info created.");

                //Fill MachineInfo
                if (macs != null && edids != null && disks != null)
                {
                    AppendDeviceInfo(macs, machineInfo, "Network", machineInfoType, deviceInfoType, deviceTypeType);
                    AppendDeviceInfo(edids, machineInfo, "EDID", machineInfoType, deviceInfoType, deviceTypeType);
                    AppendDeviceInfo(disks, machineInfo, "HardDisk", machineInfoType, deviceInfoType, deviceTypeType);
                }

				Log.Write("AssemblyLoader::ValidateEntryAssembly() calling checkin.");

                //Perform initial check in
                var method = validatorType.GetMethod("Check", BindingFlags.Static | BindingFlags.Public);
                return method.Invoke(null, new object[] { machineInfo }) as byte[];
            }
            finally
            {
                //Revert permission changes
                SecurityPermission.RevertPermitOnly();

				Log.Write("AssemblyLoader::ValidateEntryAssembly() calling checkin.");
            }
        }

        private void AppendDeviceInfo(List<string> inputList, object machineInfo, 
            string type, Type machineInfoType, Type deviceInfoType, Type deviceTypeType)
        {
            var machineValuesProperty   = machineInfoType.GetProperty("MachineValues");
            var machineValues           = machineValuesProperty.GetValue(machineInfo, null);
            var addMethod               = machineValuesProperty.PropertyType.GetMethod("Add");
            var enumType                = Enum.Parse(deviceTypeType, type);

            foreach (var value in inputList)
            {
                var deviceInfo = Activator.CreateInstance(deviceInfoType);
                SetProperty(deviceInfo, "Type", enumType);
                SetProperty(deviceInfo, "Name", type);
                SetProperty(deviceInfo, "Value", value);

                addMethod.Invoke(machineValues, new object[] { deviceInfo });
            }
        }

        private void SetProperty(object owner, string property, object value)
        {
            var type = owner.GetType();
            var prop = type.GetProperty(property);
            prop.SetValue(owner, value, null);
        }

        #endregion
    }
}