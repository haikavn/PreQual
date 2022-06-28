// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IEmailTokenizer.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;

namespace Adrack.Service.Message
{
    /// <summary>
    /// Represents a Email Tokenizer
    /// </summary>
    public partial interface IEmailTokenizer
    {
        #region Methods

        /// <summary>
        /// Replace
        /// </summary>
        /// <param name="emailTemplate">Email Template</param>
        /// <param name="emailToken">Email Token</param>
        /// <param name="htmlEncode">HTML Code</param>
        /// <returns>String ITEM</returns>
        string Replace(string emailTemplate, IEnumerable<EmailToken> emailToken, bool htmlEncode);

        #endregion Methods
    }
}