using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using Allegiance.CommunitySecuritySystem.Client.Integration;
using Allegiance.CommunitySecuritySystem.Client.Utility;
using System.IO;

namespace Allegiance.CommunitySecuritySystem.Client.Install
{
	/// <summary>
	/// If you need to upgrade things on the end users system during an auto-update push, put them in here.
	/// 
	/// PerformUpgradeTasks will be called as the main form loads. Make sure you put checks to see if the 
	/// upgrade task needs to be run as this code will get executed on each load.
	/// </summary>
	public static class Upgrade
	{
		public static bool PerformUpgradeTasks(out string returnMessage)
		{
			returnMessage = string.Empty;

			if (UpgradeRegistryToUse12Keys() == false)
				return false;

			if (UpgradeRegistryToUseDiscreetBetaAndProdPaths(out returnMessage) == false)
				return false;

			return true;
		}

		private static bool UpgradeRegistryToUseDiscreetBetaAndProdPaths(out string returnMessage)
		{
			returnMessage = String.Empty;

			if (String.IsNullOrEmpty(AllegianceRegistry.ProductionArtPath) == true)
			{
				var productionArtworkPath = Path.Combine(Path.Combine(AllegianceRegistry.LobbyPath, LobbyType.Production.ToString()), "Artwork");

				var copyProgress = new CopyProgress();
				copyProgress.TopMost = true;
				//copyProgress.MinimizeBox = false;
				copyProgress.SourceDirectory = AllegianceRegistry.ArtPath;
				copyProgress.TargetDirectory = productionArtworkPath;
				copyProgress.LobbyType = LobbyType.Production;

				if (copyProgress.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				{
					returnMessage = "Artwork file copy was canceled. Launcher update incomplete, please restart ACSS Launcher to try again.";
					return false;
				}

				AllegianceRegistry.ProductionArtPath = productionArtworkPath;
			}

			if (String.IsNullOrEmpty(AllegianceRegistry.BetaArtPath) == true)
				AllegianceRegistry.BetaArtPath = AllegianceRegistry.ArtPath;

			if (String.IsNullOrEmpty(AllegianceRegistry.ProductionCfgFile) == true)
				AllegianceRegistry.ProductionCfgFile = "http://acss.alleg.net/allegiance.txt";

			if (String.IsNullOrEmpty(AllegianceRegistry.BetaCfgFile) == true)
				AllegianceRegistry.BetaCfgFile = "http://acss.alleg.net/allegiance-beta.txt";

			return true;
		}

		/// <summary>
		/// Upgrade the registry to use 1.2 for the allegiance keys instead of 1.1
		/// </summary>
		private static bool UpgradeRegistryToUse12Keys()
		{
			RegistryKey allegianceRoot = Registry.LocalMachine.OpenSubKey(AllegianceRegistry.AllegianceRootRegistry);

			if (allegianceRoot == null)
			{
				RegistryKey allegiance11Root = Registry.LocalMachine.OpenSubKey(AllegianceRegistry.Root + "1.1");
				if (allegiance11Root == null)
					throw new Exception("Couldn't open allegiance 1.1 subkey, upgrade failed, please re-install from the latest installer package.");

				Log.Write("Upgrading registry keys from 1.1 to 1.2.");

				allegianceRoot = Registry.LocalMachine.CreateSubKey(AllegianceRegistry.AllegianceRootRegistry);

				RecurseCopyKey(allegiance11Root, allegianceRoot);

				allegiance11Root.Close();
			}

			allegianceRoot.Close();

			return true;
		}

		// Uses sample from: http://www.codeproject.com/KB/cs/RenameRegistryKey.aspx
		private static void RecurseCopyKey(RegistryKey sourceKey, RegistryKey destinationKey)
		{
			//copy all the values
			foreach (string valueName in sourceKey.GetValueNames())
			{
				object objValue = sourceKey.GetValue(valueName);
				RegistryValueKind valKind = sourceKey.GetValueKind(valueName);
				destinationKey.SetValue(valueName, objValue, valKind);
			}

			//For Each subKey 
			//Create a new subKey in destinationKey 
			//Call myself 
			foreach (string sourceSubKeyName in sourceKey.GetSubKeyNames())
			{
				RegistryKey sourceSubKey = sourceKey.OpenSubKey(sourceSubKeyName);
				RegistryKey destSubKey = destinationKey.CreateSubKey(sourceSubKeyName);
				RecurseCopyKey(sourceSubKey, destSubKey);
				sourceSubKey.Close();
				destSubKey.Close();
			}
		}
	}
}
