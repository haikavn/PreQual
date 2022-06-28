// ***********************************************************************
// Assembly         : Arack.Encryption
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ADLEncryptionManager.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Adrack.Core.Infrastructure;
using Adrack.Service.Audit;

namespace Arack.Encryption
{
    /// <summary>
    ///     Implements top level layer for encryption access
    /// </summary>
    public class ADLEncryptionManager
    {
        /// <summary>
        ///     Starts asynchronous hashing of file
        ///     For the moment not used in project, will be included in future services
        /// </summary>
        /// <param name="hasher">The hasher.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="handler">The handler.</param>
        /// <returns>Thread.</returns>
        public static Thread HashFile(FileHashing hasher, string fileName, FileHashing.HashingProgressHandler handler)
        {
            try
            {
                var stream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite);

                hasher.FileHashingProgress += handler;

                var t = new Thread(
                    delegate() { hasher.ComputeHash(stream); }
                )
                {
                    Priority = ThreadPriority.Lowest
                };

                return t;
            }
            catch (Exception ex)
            {
                //Arman Handle Exception
                var logService = AppEngineContext.Current.Resolve<ILogService>();
                if (logService != null)
                    logService.Error(ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        ///     Hashes the file.
        ///     For the moment not used in project, will be included in future services
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] HashFile(string fileName)
        {
            var stream = (Stream) File.Open(fileName, FileMode.Open, FileAccess.ReadWrite);
            var hasher = new FileHashing(SHA1.Create());
            return hasher.ComputeHash(stream);
        }

        /// <summary>
        ///     Gets the string sha256 hash.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>System.String.</returns>
        public static string GetStringSha256Hash(string text)
        {
            //HashAlgorithm alg = SHA1.Create();

            if (string.IsNullOrEmpty(text))
                return string.Empty;

            using (var sha = new SHA256Managed())
            {
                var textData = Encoding.UTF8.GetBytes(text);
                var hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }

        /// <summary>
        ///     Encrypts the specified clear text.
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <returns>System.String.</returns>
        public static string Encrypt(string clearText)
        {
            try
            {
                var EncryptionKey = "abc123";
                var clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (var encryptor = Aes.Create())
                {
                    var pdb = new Rfc2898DeriveBytes(EncryptionKey,
                        new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }

                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch
            {
            }

            return clearText.Replace("+", "0xyz1").Replace("=", "0xyz2").Replace("/", "0xyz3");
        }

        /// <summary>
        ///     Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>System.String.</returns>
        public static string Decrypt(string cipherText)
        {
            try
            {
                var EncryptionKey = "abc123";
                cipherText = cipherText.Replace(" ", "+").Replace("0xyz1", "+").Replace("0xyz2", "=").Replace("0xyz3", "/");
                var cipherBytes = Convert.FromBase64String(cipherText);
                using (var encryptor = Aes.Create())
                {
                    var pdb = new Rfc2898DeriveBytes(EncryptionKey,
                        new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }

                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch
            {
            }

            return cipherText;
        }
    }
}