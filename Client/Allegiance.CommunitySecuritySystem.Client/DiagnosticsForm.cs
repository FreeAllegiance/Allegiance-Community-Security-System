using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Management;
using System.Management.Instrumentation;
using System.Threading;
using System.IO;
using System.Diagnostics;
using Allegiance.CommunitySecuritySystem.Client.Utility;

namespace Allegiance.CommunitySecuritySystem.Client
{
    public partial class DiagnosticsForm : Form
    {
        public DiagnosticsForm()
        {
            InitializeComponent();
        }

        private void DiagnosticsForm_Load(object sender, EventArgs e)
        {
            //Populate the fields
            //OS
            string servicePack = System.Environment.OSVersion.ServicePack;
            if (String.IsNullOrEmpty(servicePack))
            {
                servicePack = "None";
            }
            _winVerTextBox.Text = System.Environment.OSVersion.ToString() + " Service Packs: " + servicePack;

            //Processor
            RegistryKey Rkey = Registry.LocalMachine;
            Rkey = Rkey.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0");
            _procTextBox.Text = (string)Rkey.GetValue("ProcessorNameString");

            //RAM
            long memSize = 0;
            ManagementObjectSearcher search = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory");
            foreach (ManagementObject mObject in search.Get())
            {
                memSize += Convert.ToInt64(mObject["Capacity"]);
            }
            _ramTextBox.Text = Convert.ToString(memSize / 1024 / 1024);

            //Video Card(s) 
            //_vidCardTextBox.Text = GetWMI("Name", "Win32_VideoController");

            //Sound Card(s) 
            //_soundCardTextBox.Text = GetWMI("ProductName", "Win32_SoundDevice");

            //AntiVirus - Detection is buggy for Vista/Win7 - maybe review this later!
			string[] scopes = new string[] { @"\root\SecurityCenter", @"\root\SecurityCenter2" };
			foreach (string scope in scopes)
			{
				_antiVirusTextBox.Text += GetWMI(scope, "DisplayName", "AntiVirusProduct");
			}
			if (String.IsNullOrEmpty(_antiVirusTextBox.Text))
			{
				_antiVirusTextBox.Text = "Anti-Virus Package";
			}

            //DXDIAG
            
            Thread thread = new Thread(new ThreadStart(GetDxDiag));
            thread.Start();
        }

        private string GetWMI(string select, string from)
        {
            return GetWMI(String.Empty, select, from);
        }

        private string GetWMI(string scope, string select, string from)
        {
            string result = String.Empty;
            string wmipathstr = @"\\" + Environment.MachineName + scope;
            ManagementObjectSearcher search = new ManagementObjectSearcher(wmipathstr, String.Format("SELECT {0} FROM {1}", select, from));

			if (search.Scope.IsConnected == false)
				return String.Empty;

            foreach (ManagementObject mObject in search.Get())
            {
                string found = mObject[select] as string;
                if (found != "Invalid namespace")
                {
                    result += mObject[select] + "; ";
                }
            }
            return result.TrimEnd(';', ' ');
        }

		//private void SetDxDiagTextBox(string text)
		//{
		//    if (_dxDiagTextBox.InvokeRequired == true)
		//        _dxDiagTextBox.Invoke(new MethodInvoker(delegate
		//            {
		//                _dxDiagTextBox.Text = text;
		//            }));
		//    else
		//        _dxDiagTextBox.Text = text.ToString();
		//}

		private delegate void InvokeHelperDelegate();
		private void InvokeHelper(InvokeHelperDelegate invokeHelperDelegate)
		{
			if(this.InvokeRequired == true)
			{
				this.Invoke(new MethodInvoker(invokeHelperDelegate));
			}
			else
			{
				invokeHelperDelegate();
			}
		}

        private void GetDxDiag()
        {
			//SetDxDiagTextBox("Working...");

			InvokeHelper(delegate
			{
				_dxDiagTextBox.Text = "Working, please wait (this may take a minute or two).";
				_copyToClipboardButton.UseWaitCursor = true;
				linkLabel1.UseWaitCursor = true;
				_dxDiagThrobber.Visible = true;

				_copyToClipboardButton.Enabled = false;
				linkLabel1.Enabled = false;
			});

			try
			{
				string path = Application.StartupPath + @"\TempDxDiag.txt";
				if (File.Exists(path))
					File.Delete(path);

				Process p = new Process();
				p.StartInfo.UseShellExecute = false;
				p.StartInfo.RedirectStandardOutput = true;
				p.StartInfo.FileName = "dxdiag.exe";
				p.StartInfo.Arguments = "/t " + path;
				p.Start();

				while (true)
				{
					if (File.Exists(Application.StartupPath + @"\TempDxDiag.txt"))
					{
						StreamReader reader = new StreamReader(Application.StartupPath + @"\TempDxDiag.txt");
						StringBuilder sb = new StringBuilder();
						while (!reader.EndOfStream)
						{
							string line = reader.ReadLine();
							if (!line.Contains("Caps="))
							{
								sb.AppendLine(line);
							}
						}

						InvokeHelper(delegate
						{
							_dxDiagTextBox.Text = sb.ToString();
						});

						//SetDxDiagTextBox(sb.ToString());
						
						reader.Close();
						File.Delete(Application.StartupPath + @"\TempDxDiag.txt");
						break;
					}
					else
					{
						Thread.Sleep(200);
					}
				}

				p.WaitForExit();
			}
			finally
			{
				InvokeHelper(delegate
				{
					_copyToClipboardButton.UseWaitCursor = false;
					linkLabel1.UseWaitCursor = false;

					this._dxDiagThrobber.Visible = false;

					_copyToClipboardButton.Enabled = true;
					linkLabel1.Enabled = true;
				});
			}
        }

        private void _copyToClipboardButton_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(new String('-',115));
            sb.AppendLine("[b]Windows Version (and Service Packs):[/b] " + _winVerTextBox.Text);
            sb.AppendLine("[b]Processor Speed:[/b] " + _procTextBox.Text);
            sb.AppendLine("[b]RAM:[/b] " + _ramTextBox.Text + "MB");
            //sb.AppendLine("[b]Video Card (Make and Model#):[/b] " + _vidCardTextBox.Text);
            //sb.AppendLine("[b]Sound Card (Make and Model#):[/b] " + _soundCardTextBox.Text);
            sb.AppendLine("[b]Connection Type (Dialup, Cable, DSL, other):[/b] " + _connectionComboBox.Text);
            sb.AppendLine("[b]Modem (Make and Model#):[/b] " + _modemTextBox.Text);
            sb.AppendLine("[b]Router (Make and Model#):[/b] " + _routerTextBox.Text);
            sb.AppendLine("[b]Internet Service Provider(company name):[/b] " + _ispTextBox.Text);
            //sb.AppendLine("[b]Installed Mods:[/b] ");
            sb.AppendLine("[b]Antivirus:[/b] " + _antiVirusTextBox.Text);
            sb.AppendLine();
            sb.AppendLine("DxDiag:[codebox]" + _dxDiagTextBox.Text + "[/codebox]");
            Clipboard.SetText(sb.ToString());
        }

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			_copyToClipboardButton_Click(this, EventArgs.Empty);

			MessageBox.Show("The diagnostic form was copied to your clipboard. Please paste it into your help request after the browser window opens.");

			Process.Start("http://www.freeallegiance.org/forums/index.php?act=post&do=new_post&f=5");
		}
    }
}
