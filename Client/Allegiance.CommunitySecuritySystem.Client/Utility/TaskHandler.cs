using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Allegiance.CommunitySecuritySystem.Client.Utility
{
    internal static class TaskHandler
    {
        #region Fields

        private static Semaphore _semaphore     = null;
        private static List<Thread> _threads    = new List<Thread>();

        #endregion

        #region Properties

        public static int MaxThreads { get; set; }

        #endregion

        #region Methods

#if !DEBUG
        [DebuggerStepThrough]
#endif
		public static Thread RunTask(ThreadStart task)
        {
            if (_semaphore == null)
            {
                if (MaxThreads == 0)
                    MaxThreads = 10;

                _semaphore = new Semaphore(MaxThreads, MaxThreads);
            }

            _semaphore.WaitOne();

            var t = new Thread(delegate()
            {
                try
                {
                    task.Invoke();
                }
                finally
                {
                    _semaphore.Release();

                    //Remove thread from _thread list
                    lock (_threads)
                    {
                        if (_threads.Contains(Thread.CurrentThread))
                            _threads.Remove(Thread.CurrentThread);
                    }
                }
            });
            t.IsBackground = true;

            lock (_threads)
                _threads.Add(t);

            t.Start();

            return t;
        }

#if !DEBUG
        [DebuggerStepThrough]
#endif
		public static Thread RunTask(ParameterizedThreadStart task, params object[] parameters)
        {
            if (_semaphore == null)
            {
                if (MaxThreads == 0) 
                    MaxThreads = 10;

                _semaphore = new Semaphore(MaxThreads, MaxThreads);
            }

            _semaphore.WaitOne();

            var t = new Thread(delegate()
            {
                try
                {
                    task.Invoke(parameters);
                }
                finally
                {
                    _semaphore.Release();
					
                    //Remove thread from _thread list
                    lock (_threads)
                    {
                        if(_threads.Contains(Thread.CurrentThread))
                            _threads.Remove(Thread.CurrentThread);
                    }
                }
            });
            t.IsBackground = true;

            lock (_threads)
                _threads.Add(t);

            t.Start();

            return t;
        }

        public static void WaitEnd()
        {
            foreach (var t in _threads)
                t.Join();
        }

        public static void WaitEnd(Thread thread)
        {
            thread.Join();
        }

        #endregion
    }
}