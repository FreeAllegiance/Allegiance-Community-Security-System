// Further information at:
// http://support.microsoft.com/default.aspx?scid=kb;en-us;105763

using System;
using System.IO;

namespace AlternateDataStreams {
   /// <summary>
   /// A class with static methods for reading and writing to "hidden" streams in
   /// a normal file.  We have to drop down to interop here because MS managed code
   /// will not allow colons in the filename, which specify the stream within the
   /// file to access.  This only works on NTFS drives, not FAT or FAT32 drives.
   /// 
   /// Sean Michael Murphy, 2005.
   /// </summary>
	public class ADSFile {
      #region Win32 Constants
      private  const    uint     GENERIC_WRITE                 = 0x40000000;
      private  const    uint     GENERIC_READ                  = 0x80000000;

      private  const    uint     FILE_SHARE_READ               = 0x00000001;
      private  const    uint     FILE_SHARE_WRITE              = 0x00000002;

      private  const    uint     CREATE_NEW                    = 1;
      private  const    uint     CREATE_ALWAYS                 = 2;
      private  const    uint     OPEN_EXISTING                 = 3;
      private  const    uint     OPEN_ALWAYS                   = 4;
      #endregion

      #region Win32 API Defines
      [System.Runtime.InteropServices.DllImport("kernel32", SetLastError=true)]
      static extern  uint GetFileSize(uint   handle,
                                      IntPtr size);

      [System.Runtime.InteropServices.DllImport("kernel32", SetLastError=true)]
      static extern  uint ReadFile(uint   handle,
                                   byte[] buffer,
                                   uint   byteToRead,
                               ref uint   bytesRead,
                                   IntPtr lpOverlapped);

      [System.Runtime.InteropServices.DllImport("kernel32", SetLastError=true)]
      static extern  uint CreateFile(string  filename,
                                     uint    desiredAccess,
                                     uint    shareMode,
                                     IntPtr  attributes,
                                     uint    creationDisposition,
                                     uint    flagsAndAttributes,
                                     IntPtr  templateFile);

      [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
      static extern bool WriteFile(uint   hFile,
                                   byte[] lpBuffer,
                                   uint   nNumberOfBytesToWrite,
                               ref uint   lpNumberOfBytesWritten,
                                   IntPtr lpOverlapped);

      [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
      static extern bool CloseHandle(uint hFile);
      #endregion

      #region ctors
      /// <summary>
      /// Private constructor.  No instances of the class can be created.
      /// </summary>
      private ADSFile() {}
      #endregion

      #region Public Static Methods
      /// <summary>
      /// Method called when an alternate data stream must be read from.
      /// </summary>
      /// <param name="file">The fully qualified name of the file from which
      /// the ADS data will be read.</param>
      /// <param name="stream">The name of the stream within the "normal" file
      /// from which to read.</param>
      /// <returns>The contents of the file as a string.  It will always return
      /// at least a zero-length string, even if the file does not exist.</returns>
      public static string Read
                          (string      file,
                           string      stream) {
         uint     fHandle  = CreateFile(file + ":" + stream,   // Filename
                                        GENERIC_READ,          // Desired access
                                        FILE_SHARE_READ,       // Share more
                                        IntPtr.Zero,           // Attributes
                                        OPEN_EXISTING,         // Creation attributes
                                        0,                     // Flags and attributes
                                        IntPtr.Zero);          // Template file

         // if the handle returned is uint.MaxValue, the stream doesn't exist.
         if (fHandle != uint.MaxValue) {
            // A handle to the stream within the file was created successfully.
            uint     size           = GetFileSize(fHandle, IntPtr.Zero);
            byte[]   buffer         = new byte[size];
            uint     read           = uint.MinValue;

            uint     result        = ReadFile(fHandle,         // Handle
                                              buffer,          // Data buffer
                                              size,            // Bytes to read
                                          ref read,            // Bytes actually read
                                              IntPtr.Zero);    // Overlapped
            
            CloseHandle(fHandle);
            
            // Convert the bytes read into an ASCII string and return it to the caller.
            return System.Text.Encoding.ASCII.GetString(buffer);
         } else
			   throw new AlternateDataStreams.StreamNotFoundException(file, stream);
      }

      /// <summary>
      /// The static method to call when data must be written to a stream.
      /// </summary>
      /// <param name="data">The string data to embed in the stream in the file</param>
      /// <param name="file">The fully qualified name of the file with the
      /// stream into which the data will be written.</param>
      /// <param name="stream">The name of the stream within the normal file to
      /// write the data.</param>
      /// <returns>An unsigned integer of how many bytes were actually written.</returns>
      public static uint   Write
                          (string      data, 
                           string      file,
                           string      stream) {
         // Convert the string data to be written to an array of ascii characters.
         byte[]   barData           = System.Text.Encoding.ASCII.GetBytes(data);
         uint     nReturn           = 0;

         uint fHandle = CreateFile(file + ":" + stream,        // File name
                                   GENERIC_WRITE,              // Desired access
                                   FILE_SHARE_WRITE,           // Share mode
                                   IntPtr.Zero,                // Attributes
                                   CREATE_ALWAYS,              // Creation disposition
                                   0,                          // Flags and attributes
                                   IntPtr.Zero);               // Template file

         bool  bOK = WriteFile(fHandle,                        // Handle
                               barData,                        // Data buffer
                         (uint)barData.Length,                 // Buffer size
                           ref nReturn,                        // Bytes written
                               IntPtr.Zero);                   // Overlapped

         CloseHandle(fHandle);

         // Throw an exception if the data wasn't written successfully.
         if (!bOK)
            throw new System.ComponentModel.Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error());

         return nReturn;
      }
      #endregion
	}
	
   /// <summary>
   /// Class to allow stream read operations to raise specific errors if the stream
   /// is not found in the file.
   /// </summary>
	public	class	StreamNotFoundException		:	System.IO.FileNotFoundException {
      #region Private Members
		private		string		_stream		= string.Empty;
      #endregion

      #region ctors
      /// <summary>
      /// Constructor called with the name of the file and stream which was
      /// unsuccessfully opened.
      /// </summary>
      /// <param name="file">Fully qualified name of the file in which the stream
      /// was supposed to reside.</param>
      /// <param name="stream">Stream within the file to open.</param>
		public	StreamNotFoundException
			     (string		file,
			      string		stream) : base(string.Empty, file) {
			_stream = stream;
		}
      #endregion

      #region Public Properties
      /// <summary>
      /// Read-only property to allow the user to query the exception to determine
      /// the name of the stream that couldn't be found.
      /// </summary>
      public	string	Stream {
         get {
            return _stream;
         }
      }
      #endregion

      #region Overridden Properties
      /// <summary>
      /// Overridden Message property to return a concise string describing the
      /// exception.
      /// </summary>
		public override string Message {
			get {
				return "Stream \"" + _stream + "\" not found in \"" + base.FileName + "\"";
			}
		}
      #endregion
	}
}