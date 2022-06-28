// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IEncryptionService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Service.Security
{
    /// <summary>
    /// Represents a Encryption Service
    /// </summary>
    public partial interface IEncryptionService
    {
        #region Methods

        /// <summary>
        /// Create Salt Key
        /// </summary>
        /// <param name="size">Size</param>
        /// <returns>String Item</returns>
        string CreateSaltKey(int size);

        /// <summary>
        /// Create Password Hash
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="saltKey">Salt Key</param>
        /// <param name="passwordFormat">Password Format</param>
        /// <returns>String Item</returns>
        string CreatePasswordHash(string password, string saltKey, string passwordFormat = "SHA1");

        /// <summary>
        /// Encrypt Text
        /// </summary>
        /// <param name="plainText">Plain Text</param>
        /// <param name="encryptionPrivateKey">Encryption Private Key</param>
        /// <returns>String Item</returns>
        string EncryptText(string plainText, string encryptionPrivateKey = "");

        /// <summary>
        /// Decrypt Text
        /// </summary>
        /// <param name="cipherText">Cipher Text</param>
        /// <param name="encryptionPrivateKey">Encryption Private Key</param>
        /// <returns>String Item</returns>
        string DecryptText(string cipherText, string encryptionPrivateKey = "");

        string SimpleEncrypt(string encryptString, string key);

        string SimpleDecrypt(string cipherText, string key);


        #endregion Methods
    }
}