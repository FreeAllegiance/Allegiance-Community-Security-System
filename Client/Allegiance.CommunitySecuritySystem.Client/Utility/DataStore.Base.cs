using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading;
using System.Xml;

namespace Allegiance.CommunitySecuritySystem.Client.Utility
{
    public sealed partial class DataStore
    {
        #region Fields

        private Mutex _lock = null;

        #endregion

        #region Properties

        public string Filename { get; set; }

        public string Key { get; set; }

        public Dictionary<string, object> Library { get; set; }

        #endregion

        #region Constructors

        private DataStore()
        {
            _lock = new Mutex();
        }

        #endregion

        #region Methods

        public object this[string name]
        {
            get { return LoadNode<object>(name); }
            set { AddNode(name, value); }
        }

        /// <summary>
        /// Adds or updates an existing node in the store.
        /// </summary>
        /// <param name="value">If this value is null, the node is removed</param>
        public DataStore AddNode(string name, object value)
        {
            _lock.WaitOne();

            if (value == null)
                Library.Remove(name);
            else
                Library[name] = value;

            _lock.ReleaseMutex();

            return this;
        }

        /// <summary>
        /// Retrieves a node from the store.
        /// </summary>
        public T LoadNode<T>(string name)
        {
            object data;
            if (Library.TryGetValue(name, out data))
                return (T)data;

            return default(T);
        }

        #region IO

        /// <summary>
        /// Opens an encrypted & serialized DataStore file.
        /// </summary>
        public static DataStore Open(string filename, string password)
        {
            Dictionary<string, object> library;
            if (File.Exists(filename))
            {
                var serializer = new BinaryFormatter();

                using (var stream = OpenDecryptionStream(filename, password))
                    library = serializer.Deserialize(stream) as Dictionary<string, object>;
            }
            else
                library = new Dictionary<string, object>();

            return new DataStore()
            {
                Filename    = filename,
                Key         = password,
                Library     = library
            };
        }

        /// <summary>
        /// Encrypts and saves to existing file.
        /// </summary>
        public void Save()
        {
            Save(Filename, Key);
        }

        /// <summary>
        /// Encrypts and saves to specified file.
        /// </summary>
        public void Save(string filename, string password)
        {
            _lock.WaitOne();

            var serializer = new BinaryFormatter();

            using (var stream = OpenEncryptionStream(filename, password))
                serializer.Serialize(stream, Library);

            _lock.ReleaseMutex();
        }

        /// <summary>
        /// Creates an encrypted & serialized DataStore file, replaces any existing file.
        /// </summary>
        public static DataStore Create(string filename, string password)
        {
            var doc = new XmlDocument();
            var parent = doc.CreateElement("DataStore");
            doc.AppendChild(parent);

            return new DataStore()
            {
                Filename    = filename,
                Key         = password,
                Library     = new Dictionary<string,object>()
            };
        }

        #endregion

        #region Utilities

        private static Stream OpenDecryptionStream(string filename, string password)
        {
            byte[] key, iv;
            GenerateKey(password, out key, out iv);

            var fs = File.OpenRead(filename);
            var cs = Rijndael.Create();

            var decryptor = cs.CreateDecryptor(key, iv);

            return new CryptoStream(fs, decryptor, CryptoStreamMode.Read);
        }

        private static Stream OpenEncryptionStream(string filename, string password)
        {
            byte[] key, iv;
            GenerateKey(password, out key, out iv);

            if (File.Exists(filename))
                File.Delete(filename);

            var fs = File.Open(filename, FileMode.CreateNew, FileAccess.Write, FileShare.None);
            var cs = Rijndael.Create();

            var decryptor = cs.CreateEncryptor(key, iv);

            return new CryptoStream(fs, decryptor, CryptoStreamMode.Write);
        }

        private static void GenerateKey(string password, out byte[] key, out byte[] iv)
        {
            var pdb = new PasswordDeriveBytes(password, new byte[] { 0, 5, 2, 63, 29 });
            key     = pdb.GetBytes(32);
            iv      = pdb.GetBytes(16);
        }

        #endregion

        #endregion
    }
}