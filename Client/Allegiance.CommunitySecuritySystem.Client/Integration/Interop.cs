using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Allegiance.CommunitySecuritySystem.Client.Integration
{
	// http://www.codekeep.net/CSharp/code/snippets/18461/EnvironmentIs64Bit/view.aspx
	[Obsolete("No!", true)]
	class Interop
	{
		public static Boolean Is64Bit()
		{
			var isWow64 = false;
			//This only works for NT 5.1 and above.
			if (System.Environment.OSVersion.Version.Major < 5 ||
				(System.Environment.OSVersion.Version.Major == 5 &
				 System.Environment.OSVersion.Version.Minor < 1))
				return isWow64;

			var processHandle = GetProcessHandle(Process.GetCurrentProcess().Handle);

			Boolean retVal;
			if (!NativeMethods.IsWow64Process(processHandle, out retVal))
				throw new Win32Exception(Marshal.GetLastWin32Error());

			isWow64 = retVal;

			return isWow64;
		}

		//private static SafeProcessHandle GetProcessHandle(UInt32 processId)
		private static SafeProcessHandle GetProcessHandle(IntPtr handle)
		{
			return new SafeProcessHandle(handle, true);
		}

		private sealed class SafeProcessHandle : SafeHandle
		{
			public SafeProcessHandle()
				: base(IntPtr.Zero, true) { }
			public SafeProcessHandle(IntPtr handle, Boolean ownsHandle)
				: base(IntPtr.Zero, ownsHandle)
			{
				base.handle = handle;
			}

			public override bool IsInvalid
			{
				get { return base.handle == IntPtr.Zero; }
			}

			protected override bool ReleaseHandle()
			{
				return NativeMethods.CloseHandle(base.handle);
			}
		}

		private static class NativeMethods
		{
			[DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern Boolean IsWow64Process(
				[In] SafeProcessHandle hProcess,
				[Out, MarshalAs(UnmanagedType.Bool)] out Boolean wow64Process);

			[DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern Boolean IsWow64Process(
				[In] IntPtr processHandle,
				[Out, MarshalAs(UnmanagedType.Bool)] out Boolean wow64Process);

			[DllImport("kernel32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern Boolean CloseHandle(IntPtr handle);
		}
	}
}
