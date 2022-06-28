//------------------------------------------------------------------------------
// Company: Adrack
//------------------------------------------------------------------------------
// Developer: Zarzand Papikyan
// Description:	Application Context
//------------------------------------------------------------------------------

using Adrack.Core.Domain.Directory;
using Adrack.Core.Domain.Localization;
using Adrack.Core.Domain.Membership;
using System;
using System.Web;

namespace Adrack.Core
{
    /// <summary>
    /// Represents a Application Context
    /// </summary>
    public interface IAppContext
    {
        #region Methods

        void ClearUserCookie();
        HttpCookie GetUserCookie();
        void SetUserCookie(Guid userGuid);

        void SetUserExpire(long id);
        bool CheckUserExpire(long id);

        void SetBackLoginUser(User user);
        User GetBackLoginUser(bool clear = true);
        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the Application User
        /// </summary>
        User AppUser { get; set; }

        /// <summary>
        /// Gets or Sets the Application User Impersonate
        /// </summary>
        User AppUserImpersonate { get; }

        /// <summary>
        /// Gets or Sets the Application Language
        /// </summary>
        Language AppLanguage { get; set; }

        /// <summary>
        /// Gets or Sets the Application Currency
        /// </summary>
        Currency AppCurrency { get; set; }

        /// <summary>
        /// Gets or Sets the Global Administrator (Built-in global administrators have complete and unrestricted access to the full website)
        /// </summary>
        bool IsGlobalAdministrator { get; set; }

        /// <summary>
        /// Gets or Sets the Content Manager (Built-in content managers have restricted access to the website)
        /// </summary>
        bool IsContentManager { get; set; }

        /// <summary>
        /// Gets or Sets the Nework User (Built-in network user are registered users)
        /// </summary>
        bool IsNetworkUser { get; set; }

        #endregion
    }
}