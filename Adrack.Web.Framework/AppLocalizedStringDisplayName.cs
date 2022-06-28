// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="AppLocalizedStringDisplayName.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Infrastructure;
using Adrack.Service.Localization;
using Adrack.Web.Framework.Mvc;

namespace Adrack.Web.Framework
{
    /// <summary>
    /// Represents a Application Localized String Display Name
    /// Implements the <see cref="System.ComponentModel.DisplayNameAttribute" />
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.IModelAttribute" />
    /// </summary>
    /// <seealso cref="System.ComponentModel.DisplayNameAttribute" />
    /// <seealso cref="Adrack.Web.Framework.Mvc.IModelAttribute" />
    public class AppLocalizedStringDisplayName : System.ComponentModel.DisplayNameAttribute, IModelAttribute
    {
        #region Fields

        /// <summary>
        /// Localized String Value
        /// </summary>
        private string _localizedStringValue = string.Empty;

        #endregion Fields



        #region Constructor

        /// <summary>
        /// Application Localized String Display Name
        /// </summary>
        /// <param name="localizedStringKey">The localized string key.</param>
        public AppLocalizedStringDisplayName(string localizedStringKey)
            : base(localizedStringKey)
        {
            LocalizedStringKey = localizedStringKey;
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Localized String Key
        /// </summary>
        /// <value>The localized string key.</value>
        public string LocalizedStringKey { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return "AppLocalizedStringDisplayName"; }
        }

        /// <summary>
        /// Display Name
        /// </summary>
        /// <value>The display name.</value>
        public override string DisplayName
        {
            get
            {
                var appLanguageId = AppEngineContext.Current.Resolve<IAppContext>().AppLanguage.Id;

                _localizedStringValue = AppEngineContext.Current.Resolve<ILocalizedStringService>().GetLocalizedString(appLanguageId, LocalizedStringKey, LocalizedStringKey, true);

                return _localizedStringValue;
            }
        }

        #endregion Properties
    }
}