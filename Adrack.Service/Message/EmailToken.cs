// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="EmailToken.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Service.Message
{
    /// <summary>
    /// Represents a Email Token
    /// </summary>
    public sealed class EmailToken
    {
        #region Fields

        /// <summary>
        /// Key
        /// </summary>
        private readonly string _key;

        /// <summary>
        /// Value
        /// </summary>
        private readonly string _value;

        /// <summary>
        /// Html Encoded
        /// </summary>
        private readonly bool _htmlEncoded;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Email Token
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public EmailToken(string key, string value) : this(key, value, false)
        {
        }

        /// <summary>
        /// Email Token
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="htmlEncoded">Html Encoded</param>
        public EmailToken(string key, string value, bool htmlEncoded)
        {
            this._key = key;
            this._value = value;
            this._htmlEncoded = htmlEncoded;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// To String
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return string.Format("{0}: {1}", Key, Value);
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Key
        /// </summary>
        /// <value>The key.</value>
        public string Key
        {
            get { return _key; }
        }

        /// <summary>
        /// Gets or Sets the Value
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Gets or Sets the Html Encoded
        /// </summary>
        /// <value><c>true</c> if [HTML encoded]; otherwise, <c>false</c>.</value>
        public bool HtmlEncoded
        {
            get { return _htmlEncoded; }
        }

        #endregion Properties
    }
}