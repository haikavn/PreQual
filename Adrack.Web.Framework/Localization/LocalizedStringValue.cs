// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="LocalizedStringValue.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Web;

namespace Adrack.Web.Framework.Localization
{
    /// <summary>
    /// Represents a Localized String Value
    /// Implements the <see cref="System.MarshalByRefObject" />
    /// Implements the <see cref="System.Web.IHtmlString" />
    /// </summary>
    /// <seealso cref="System.MarshalByRefObject" />
    /// <seealso cref="System.Web.IHtmlString" />
    public class LocalizedStringValue : MarshalByRefObject, IHtmlString
    {
        #region Fields

        /// <summary>
        /// Localized
        /// </summary>
        private readonly string _localized;

        /// <summary>
        /// Scope
        /// </summary>
        private readonly string _scope;

        /// <summary>
        /// Text Hint
        /// </summary>
        private readonly string _textHint;

        /// <summary>
        /// Arguments
        /// </summary>
        private readonly object[] _args;

        #endregion Fields



        #region Constructor

        /// <summary>
        /// Localized String Value
        /// </summary>
        /// <param name="localized">Localized</param>
        public LocalizedStringValue(string localized)
        {
            this._localized = localized;
        }

        /// <summary>
        /// Localized String Value
        /// </summary>
        /// <param name="localized">Localized</param>
        /// <param name="scope">Scope</param>
        /// <param name="textHint">Text Hint</param>
        /// <param name="args">Arguments</param>
        public LocalizedStringValue(string localized, string scope, string textHint, object[] args)
        {
            this._localized = localized;
            this._scope = scope;
            this._textHint = textHint;
            this._args = args;
        }

        /// <summary>
        /// Text Or Default
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="defaultValue">Default Value</param>
        /// <returns>Localized String Value Item</returns>
        public static LocalizedStringValue TextOrDefault(string text, LocalizedStringValue defaultValue)
        {
            if (string.IsNullOrEmpty(text))
                return defaultValue;

            return new LocalizedStringValue(text);
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Hash Code
        /// </summary>
        /// <returns>Integer Item</returns>
        public override int GetHashCode()
        {
            var hashCode = 0;

            if (_localized != null)
                hashCode ^= _localized.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Boolean Item</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var thatValue = (LocalizedStringValue)obj;

            return string.Equals(_localized, thatValue._localized);
        }

        /// <summary>
        /// To String
        /// </summary>
        /// <returns>String Item</returns>
        public override string ToString()
        {
            return _localized;
        }

        /// <summary>
        /// To Html String
        /// </summary>
        /// <returns>String Item</returns>
        public string ToHtmlString()
        {
            return _localized;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Scope
        /// </summary>
        /// <value>The scope.</value>
        public string Scope
        {
            get { return _scope; }
        }

        /// <summary>
        /// Gets or Sets the Text Hint
        /// </summary>
        /// <value>The text hint.</value>
        public string TextHint
        {
            get { return _textHint; }
        }

        /// <summary>
        /// Gets or Sets the Arguments
        /// </summary>
        /// <value>The arguments.</value>
        public object[] Args
        {
            get { return _args; }
        }

        /// <summary>
        /// Gets or Sets the Text
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return _localized; }
        }

        #endregion Properties
    }
}