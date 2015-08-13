using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Allegiance.CommunitySecuritySystem.Client.Integration;

namespace Allegiance.CommunitySecuritySystem.Client.Utility
{
    internal class DebugDetector
    {
        #region Fields

        private const int CheckInterval = 50;

        private readonly Thread _running;

        private static DebugDetector Instance { get; set; }

        #endregion

        #region Properties

        private static Thread RunningThread
        {
            get
            {
                if (Instance != null)
                    return Instance._running;
                return null;
            }
        }

        private static bool IsCheckRunning
        {
            get { return RunningThread != null && RunningThread.IsAlive; }
        }

        #endregion

        #region Constructors

#if !DEBUG
        [DebuggerStepThrough]
#endif
		private DebugDetector(Thread thread)
        {
            _running = thread;
        }

        #endregion

        #region Methods

        [DllImport("Kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int IsDebuggerPresent();

        [DebuggerStepThrough]
        public static bool Detect()
        {
#if DEBUG
            return false;
#else
            //Simple anti-debugger
            if (Debugger.IsAttached)
                return true;

            if (IsDebuggerPresent() != 0)
                return true;

            if (IsCheckRunning)
                return false;

            Instance = new DebugDetector(TaskHandler.RunTask(delegate()
            {
                while (true)
                {
                    if (Debugger.IsAttached)
                        break;

                    if (IsDebuggerPresent() > 0)
                        break;

                    Thread.Sleep(CheckInterval);
                }

                KillProcess();

            }));

            return false;
#endif
		}

#if !DEBUG
        [DebuggerStepThrough]
#endif
		private static void KillProcess()
        {
            while (true)
            {
                try
                {
                    Process.GetCurrentProcess().Kill();
                    Process.GetCurrentProcess().WaitForExit();
                }
                catch
                {
                    Thread.CurrentThread.Priority = ThreadPriority.Highest;
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
                }
            }
        }

#if !DEBUG
        [DebuggerStepThrough]
#endif
		public static void AssertCheckRunning()
        {
#if !DEBUG
            if (Debugger.IsAttached || !IsCheckRunning)
            {
                //Exit Allegiance if it is running
                AllegianceLoader.ExitAllegiance();

                KillProcess();
                throw new Exception("Debug check not running.");
            }
#endif
        }

        #endregion
    }
}