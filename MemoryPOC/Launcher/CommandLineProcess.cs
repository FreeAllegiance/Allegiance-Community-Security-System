using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Launcher
{
	public class CommandLineProcess
	{

		public delegate void OnStandardOutHandler(string data);
		public event OnStandardOutHandler OnStandardOut;

		public delegate void OnStandardErrorHandler(string data);
		public event OnStandardErrorHandler OnStandardError;

		public Process Start(string command, string args)
		{
			return Start(command, args, Directory.GetCurrentDirectory());
		}

		public Process Start(string command, string args, string workingDirectory)
		{
			ProcessStartInfo psi = new ProcessStartInfo(command, args);
			psi.WorkingDirectory = workingDirectory;
			psi.UseShellExecute = false;
			psi.RedirectStandardOutput = true;
			psi.RedirectStandardError = true;
			
			Process p = new Process();
			p.StartInfo = psi;

			p.OutputDataReceived += new DataReceivedEventHandler(OutputDataReceived);
			p.ErrorDataReceived += new DataReceivedEventHandler(ErrorDataReceived);

			p.Start();
			p.BeginOutputReadLine();
			p.BeginErrorReadLine();

			return p;
		}

		void ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (OnStandardError != null)
				OnStandardError(e.Data);
		}

		void OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (OnStandardOut != null)
				OnStandardOut(e.Data);
		}
	}
}

