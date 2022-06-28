// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="LogModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Web.Framework;
using Adrack.Web.Framework.Mvc;
using System;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Models.Audit
{
    /// <summary>
    /// Represents a Log Model
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppEntityModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppEntityModel" />
    public partial class LogModel : BaseAppEntityModel
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Log Level
        /// </summary>
        /// <value>The log level.</value>
        [AllowHtml]
        public string LogLevel { get; set; }

        /// <summary>
        /// Gets or Sets the Message
        /// </summary>
        /// <value><c>true</c> if message; otherwise, <c>false</c>.</value>
        [AllowHtml]
        [AppLocalizedStringDisplayName("Admin.Log.Message")]
        public bool Message { get; set; }

        /// <summary>
        /// Gets or Sets the Exception
        /// </summary>
        /// <value><c>true</c> if exception; otherwise, <c>false</c>.</value>
        [AllowHtml]
        [AppLocalizedStringDisplayName("Admin.Log.Exception")]
        public bool Exception { get; set; }

        /// <summary>
        /// Gets or Sets the IpAddress
        /// </summary>
        /// <value><c>true</c> if [ip address]; otherwise, <c>false</c>.</value>
        [AllowHtml]
        [AppLocalizedStringDisplayName("Admin.Log.IpAddress")]
        public bool IpAddress { get; set; }

        /// <summary>
        /// Gets or Sets the Page Url
        /// </summary>
        /// <value><c>true</c> if [page URL]; otherwise, <c>false</c>.</value>
        [AllowHtml]
        [AppLocalizedStringDisplayName("Admin.Log.PageUrl")]
        public bool PageUrl { get; set; }

        /// <summary>
        /// Gets or Sets the Referrer Url
        /// </summary>
        /// <value><c>true</c> if [referrer URL]; otherwise, <c>false</c>.</value>
        [AllowHtml]
        [AppLocalizedStringDisplayName("Admin.Log.ReferrerUrl")]
        public bool ReferrerUrl { get; set; }

        /// <summary>
        /// Gets or Sets the Created On
        /// </summary>
        /// <value>The created on.</value>
        [AllowHtml]
        [AppLocalizedStringDisplayName("Admin.Log.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion Properties
    }
}