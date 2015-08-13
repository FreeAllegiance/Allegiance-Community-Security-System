using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Xml;

namespace Allegiance.CommunitySecuritySystem.PrototypeClient
{
    public sealed partial class DataStore
    {
        #region Properties

        public string Filename { get; set; }

        public string Password { get; set; }

        private XmlDocument Document { get; set; }

        private XmlElement Parent { get; set; }

        #endregion

        #region Constructors

        private DataStore()
        {
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
            if (value == null)
            {
                if (Parent[name] != null)
                    Parent.RemoveChild(Parent[name]);
                return this;
            }

            var serializer = new BinaryFormatter();
            string serializedData;
            using (var ms = new MemoryStream())
            {
                serializer.Serialize(ms, value);
                serializedData = Convert.ToBase64String(ms.ToArray());
            }

            //Check if element exists in document
            XmlElement element;
            if (Parent[name] != null)
                element = Parent[name];
            else
            {
                element = Document.CreateElement(name);
                Parent.AppendChild(element);
            }

            element.InnerText = serializedData.ToString();
            return this;
        }

        /// <summary>
        /// Retrieves a node from the store.
        /// </summary>
        public T LoadNode<T>(string name)
        {
            if (Parent[name] == null)
                return default(T);

            var value = Parent[name].InnerText;
            if (!string.IsNullOrEmpty(value))
            {
                var serializer = new BinaryFormatter();
                using(var ms = new MemoryStream(Convert.FromBase64String(value)))
                    return (T)serializer.Deserialize(ms);
            }

            return default(T);
        }

        #region IO

        /// <summary>
        /// Opens an encrypted & serialized DataStore file.
        /// </summary>
        public static DataStore Open(string filename, string password)
        {
            var doc = new XmlDocument();
            XmlElement parent;

            if (File.Exists(filename))
            {
                using (var stream = OpenDecryptionStream(filename, password))
                    doc.Load(stream);

                parent = doc["DataStore"];
            }
            else
            {
                parent = doc.CreateElement("DataStore");
                doc.AppendChild(parent);
            }

            return new DataStore()
            {
                Filename    = filename,
                Password    = password,
                Document    = doc,
                Parent      = parent
            };
        }

        /// <summary>
        /// Encrypts and saves to existing file.
        /// </summary>
        public void Save()
        {
            Save(Filename, Password);
        }

        /// <summary>
        /// Encrypts and saves to specified file.
        /// </summary>
        public void Save(string filename, string password)
        {
            using (var stream = OpenEncryptionStream(filename, password))
                Document.Save(stream);
        }

        /// <summary>
        /// Creates an encrypted & serialized DataStore file, replaces any existing file.
        /// </summary>
        public static DataStore Create(string filename, string password)
        {
            var doc     = new XmlDocument();
            var parent  = doc.CreateElement("DataStore");
            doc.AppendChild(parent);

            return new DataStore()
            {
                Filename    = filename,
                Password    = password,
                Document    = doc,
                Parent      = parent
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
            key = pdb.GetBytes(32);
            iv  = pdb.GetBytes(16);
        }

        #endregion

        #endregion
    }
}