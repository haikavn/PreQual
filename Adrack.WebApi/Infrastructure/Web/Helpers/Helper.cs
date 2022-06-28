using Adrack.WebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Adrack.WebApi.Infrastructure.Web.Helpers
{
    public static class Helper
    {
        #region methods

        /// <summary>
        /// Gets the unique key.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="ArgumentException">Length must be between 1 and " + guidResult.Length</exception>
        public static string GetUniqueKey(int length)
        {
            string guidResult = string.Empty;

            while (guidResult.Length < length)
            {
                // Get the GUID.
                guidResult += $"{guidResult}{Guid.NewGuid().ToString().GetHashCode():x}";
            }

            // Make sure length is valid.
            if (length <= 0 || length > guidResult.Length)
            {
                throw new ArgumentException($"Length must be between 1 and {guidResult.Length}", nameof(guidResult));
            }

            // Return the first length bytes.
            return guidResult.Substring(0, length);
        }

        /// <summary>
        /// Initializes paging data
        /// </summary>
        /// <param name="pageData"></param>
        /// <param name="start"></param>
        /// <param name="total"></param>
        /// <param name="filtered"></param>
        public static void SetInstanceValues(this IPagedData pageData, int start = 1, int total = 3, int filtered = 3)
        {
            pageData.RecordsStart = start;
            pageData.RecordsTotal = total;
            pageData.RecordsFiltered = filtered;
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>System.String.</returns>
        public static string Decrypt(string cipherText)
        {
            try
            {
                var encryptionKey = "abc123";
                cipherText = cipherText.Replace(" ", "+");
                var cipherBytes = Convert.FromBase64String(cipherText);
                using (var aes = Aes.Create())
                {
                    var pdb = new Rfc2898DeriveBytes(encryptionKey,
                        new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
                    aes.Key = pdb.GetBytes(32);
                    aes.IV = pdb.GetBytes(16);
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }

                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception)
            {
            }
            return cipherText;
        }

        public static void Copy(object sourceObject, object targetObject)
        {
            var sourceProperties = sourceObject.GetType().GetProperties();
            var targetProperties = targetObject.GetType().GetProperties();

            foreach (var property in targetProperties)
            {
                var sourceProperty = sourceProperties.FirstOrDefault(x =>
                    x.Name == property.Name &&
                    x.PropertyType == property.PropertyType);
                if (sourceProperty != null)
                {
                    property.SetValue(targetObject, sourceProperty.GetValue(sourceObject));
                }
            }
        }
        #endregion methods
    }
}