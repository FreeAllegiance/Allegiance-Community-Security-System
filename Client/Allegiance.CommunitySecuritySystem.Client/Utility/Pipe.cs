using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Allegiance.CommunitySecuritySystem.Client.Utility
{
    internal class Pipe : IDisposable
    {
        #region Interop Functions

        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern SafeFileHandle CreateNamedPipe(
           String pipeName,
           uint dwOpenMode,
           uint dwPipeMode,
           uint nMaxInstances,
           uint nOutBufferSize,
           uint nInBufferSize,
           uint nDefaultTimeOut,
           IntPtr lpSecurityAttributes);

        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern int ConnectNamedPipe(
           SafeFileHandle hNamedPipe,
           IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern int DisconnectNamedPipe(int hNamedPipe);

        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern SafeFileHandle CreateFile(
           String pipeName,
           uint dwDesiredAccess,
           uint dwShareMode,
           IntPtr lpSecurityAttributes,
           uint dwCreationDisposition,
           uint dwFlagsAndAttributes,
           IntPtr hTemplate);

        #endregion

        #region Fields

        protected const uint BufferSize         = 64;
        protected const uint FileFlagOverlap    = 0x40000000;
        protected const uint Duplex             = 0x00000003;
        protected const uint GenericRead        = 0x80000000;
        protected const uint GenericWrite       = 0x40000000;
        protected const uint Existing           = 3;

        protected SafeFileHandle _pipeHandle;

        #endregion

        #region Properties

        public string PipeName { get; protected set; }

        public bool Connected { get; protected set; }

        #endregion

        #region Constructors

        public Pipe(string pipeName)
        {
            this.PipeName = pipeName;
        }

        #endregion

        #region Methods

        public void Create()
        {
            _pipeHandle = CreateNamedPipe(
                PipeName,
                Duplex | FileFlagOverlap,
                0,
                255,
                BufferSize,
                BufferSize,
                0,
                IntPtr.Zero);

            if (_pipeHandle.IsInvalid)
                throw new Exception("Could not create named pipe.");
        }

        public void OpenExisting()
        {
            _pipeHandle = CreateFile(
                PipeName,
                GenericRead | GenericWrite,
                0,
                IntPtr.Zero,
                Existing,
                FileFlagOverlap,
                IntPtr.Zero);

            if (_pipeHandle.IsInvalid)
                throw new Exception("Could not open named pipe.");
        }

        /// <summary>
        /// Waits for a connection
        /// </summary>
        public bool Connect()
        {
            return (Connected = ConnectNamedPipe(_pipeHandle, IntPtr.Zero) == 1);
        }

        public void Send(string data)
        {
            using (var fs = new FileStream(_pipeHandle, FileAccess.Write))
            using (var sw = new StreamWriter(fs))
                sw.Write(data);
        }

        public string Read()
        {
            var buffer = new byte[BufferSize];

            using (var fs = new FileStream(_pipeHandle, FileAccess.Read))
            using (var sr = new StreamReader(fs))
                return sr.ReadToEnd();
        }

        public void Disconnect()
        {
            if (Connected)
            {
                _pipeHandle.Close();
                Connected = false;
            }
        }

        public void Dispose()
        {
            Disconnect();
        }

        #endregion
    }
}