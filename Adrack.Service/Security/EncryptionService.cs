// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="EncryptionService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Security;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Adrack.Service.Security
{
    /// <summary>
    /// Represents a Encryption Service
    /// Implements the <see cref="Adrack.Service.Security.IEncryptionService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Security.IEncryptionService" />
    public partial class EncryptionService : IEncryptionService
    {
        #region Fields

        /// <summary>
        /// Security Setting
        /// </summary>
        private readonly SecuritySetting _securitySetting;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Encrypt Text To Memory
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="key">Key</param>
        /// <param name="iv">IV</param>
        /// <returns>Byte Item</returns>
        private byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
        {
            var memoryStream = new MemoryStream();
            using (var cs = new CryptoStream(memoryStream, new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                byte[] toEncrypt = new UnicodeEncoding().GetBytes(data);

                cs.Write(toEncrypt, 0, toEncrypt.Length);
                cs.FlushFinalBlock();
            }

            return memoryStream.ToArray();
        }

        /// <summary>
        /// Decrypt Text From Memory
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="key">Key</param>
        /// <param name="iv">IV</param>
        /// <returns>String Item</returns>
        private string DecryptTextFromMemory(byte[] data, byte[] key, byte[] iv)
        {
            var memoryStream = new MemoryStream(data);

            using (var cryptoStream = new CryptoStream(memoryStream, new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv), CryptoStreamMode.Read))
            {
                var streamReader = new StreamReader(cryptoStream, new UnicodeEncoding());

                return streamReader.ReadLine();
            }
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Encryption Service
        /// </summary>
        /// <param name="securitySetting">Security Setting</param>
        public EncryptionService(SecuritySetting securitySetting)
        {
            this._securitySetting = securitySetting;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Create Salt Key
        /// </summary>
        /// <param name="size">Size</param>
        /// <returns>String Item</returns>
        public virtual string CreateSaltKey(int size)
        {
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var byteSize = new byte[size];

            rngCryptoServiceProvider.GetBytes(byteSize);

            return Convert.ToBase64String(byteSize);
        }

        /// <summary>
        /// Create Password Hash
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="saltKey">Salt Key</param>
        /// <param name="passwordFormat">Password Format</param>
        /// <returns>String Item</returns>
        /// <exception cref="ArgumentException">Unrecognized hash name {0}</exception>
        public virtual string CreatePasswordHash(string password, string saltKey, string passwordFormat = "SHA1")
        {
            if (String.IsNullOrEmpty(passwordFormat))
                passwordFormat = "SHA1";

            string saltAndPassword = String.Concat(password, saltKey);

            var hashAlgorithm = HashAlgorithm.Create(passwordFormat);

            if (hashAlgorithm == null)
                throw new ArgumentException("Unrecognized hash name {0}", passwordFormat);

            var hashByteArray = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndPassword));

            return BitConverter.ToString(hashByteArray).Replace("-", "");
        }

        /// <summary>
        /// Encrypt Text
        /// </summary>
        /// <param name="plainText">Plain Text</param>
        /// <param name="encryptionPrivateKey">Encryption Private Key</param>
        /// <returns>String Item</returns>
        public virtual string EncryptText(string plainText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            if (String.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = _securitySetting.EncryptionKey;

            var tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
            {
                Key = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(0, 16)),
                IV = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(8, 8))
            };

            byte[] encryptTextToMemory = EncryptTextToMemory(plainText, tripleDESCryptoServiceProvider.Key, tripleDESCryptoServiceProvider.IV);

            return Convert.ToBase64String(encryptTextToMemory);
        }

        /// <summary>
        /// Decrypt Text
        /// </summary>
        /// <param name="cipherText">Cipher Text</param>
        /// <param name="encryptionPrivateKey">Encryption Private Key</param>
        /// <returns>String Item</returns>
        public virtual string DecryptText(string cipherText, string encryptionPrivateKey = "")
        {
            if (String.IsNullOrEmpty(cipherText))
                return cipherText;

            if (String.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = _securitySetting.EncryptionKey;

            var tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
            {
                Key = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(0, 16)),
                IV = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(8, 8))
            };

            byte[] bufferByte = Convert.FromBase64String(cipherText);

            return DecryptTextFromMemory(bufferByte, tripleDESCryptoServiceProvider.Key, tripleDESCryptoServiceProvider.IV);
        }

        public string SimpleEncrypt(string encryptString, string key)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public string SimpleDecrypt(string cipherText, string key)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        #endregion Methods
    }
}