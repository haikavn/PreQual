// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="EmailTokenizer.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Web;

namespace Adrack.Service.Message
{
    /// <summary>
    /// Represents a Email Tokenizer
    /// Implements the <see cref="Adrack.Service.Message.IEmailTokenizer" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Message.IEmailTokenizer" />
    public partial class EmailTokenizer : IEmailTokenizer
    {
        #region Fields

        /// <summary>
        /// String Comparison
        /// </summary>
        private readonly StringComparison _stringComparison;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Replace
        /// </summary>
        /// <param name="original">original</param>
        /// <param name="pattern">Pattern</param>
        /// <param name="replacement">Replacement</param>
        /// <returns>String Item</returns>
        private string Replace(string original, string pattern, string replacement)
        {
            if (_stringComparison == StringComparison.Ordinal)
            {
                return original.Replace(pattern, replacement);
            }

            int count, position0, position1;

            count = position0 = position1 = 0;

            int inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);

            var chars = new char[original.Length + Math.Max(0, inc)];

            while ((position1 = original.IndexOf(pattern, position0, _stringComparison)) != -1)
            {
                for (int i = position0; i < position1; ++i)
                    chars[count++] = original[i];

                for (int i = 0; i < replacement.Length; ++i)
                    chars[count++] = replacement[i];

                position0 = position1 + pattern.Length;
            }

            if (position0 == 0) return original;

            for (int i = position0; i < original.Length; ++i)
                chars[count++] = original[i];

            return new string(chars, 0, count);
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Email Tokenizer
        /// </summary>
        public EmailTokenizer()
        {
            _stringComparison = StringComparison.Ordinal;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Replace
        /// </summary>
        /// <param name="emailTemplate">Email Template</param>
        /// <param name="emailToken">Email Token</param>
        /// <param name="htmlEncode">HTML Code</param>
        /// <returns>String ITEM</returns>
        /// <exception cref="ArgumentNullException">template
        /// or
        /// emailToken</exception>
        public string Replace(string emailTemplate, IEnumerable<EmailToken> emailToken, bool htmlEncode)
        {
            if (string.IsNullOrWhiteSpace(emailTemplate))
                throw new ArgumentNullException("template");

            if (emailToken == null)
                throw new ArgumentNullException("emailToken");

            foreach (var token in emailToken)
            {
                string tokenValue = token.Value;

                if (htmlEncode && !token.HtmlEncoded)
                    tokenValue = HttpUtility.HtmlEncode(tokenValue);

                emailTemplate = Replace(emailTemplate, String.Format(@"%{0}%", token.Key), tokenValue);
            }

            return emailTemplate;
        }

        #endregion Methods
    }
}