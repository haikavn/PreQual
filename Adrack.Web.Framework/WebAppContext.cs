//------------------------------------------------------------------------------
// Company: Adrack
//------------------------------------------------------------------------------
// Developer: Zarzand Papikyan
// Description:	Web Application Context
//------------------------------------------------------------------------------

using Adrack.Core;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Domain.Localization;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Security;
using Adrack.Service.Common;
using Adrack.Service.Configuration;
using Adrack.Service.Directory;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Web.Framework.Localization;
using System;
using System.Linq;
using System.Web;

namespace Adrack.Web.Framework
{
    /// <summary>
    /// Represents a Web Application Context
    /// </summary>
    public partial class WebAppContext : IAppContext
    {
        #region Constants

        /// <summary>
        /// Cache User Cookie Key
        /// </summary>
        private const string CACHE_USER_COOKIE_KEY = "App.Cache.User.Cookie";

        #endregion

        #region Fields

        /// <summary>
        /// Localization Setting
        /// </summary>
        private readonly LocalizationSetting _localizationSetting;

        /// <summary>
        /// Currency Setting
        /// </summary>
        private readonly CurrencySetting _currencySetting;

        /// <summary>
        /// User Setting
        /// </summary>
        private readonly UserSetting _userSetting;

        /// <summary>
        /// Http Context Base
        /// </summary>
        private readonly HttpContextBase _httpContextBase;

        /// <summary>
        /// User Service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Authentication Service
        /// </summary>
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// Language Service
        /// </summary>
        private readonly ILanguageService _languageService;

        /// <summary>
        /// Currency Service
        /// </summary>
        private readonly ICurrencyService _currencyService;

        /// <summary>
        /// Global Attribute Service
        /// </summary>
        private readonly IGlobalAttributeService _globalAttributeService;

        private readonly ISettingService _settingService;

        /// <summary>
        /// Application User Cached
        /// </summary>
        private User _appUserCached;

        /// <summary>
        /// Application User Impersonate
        /// </summary>
        private User _appUserImpersonate;

        /// <summary>
        /// Application Language Cached
        /// </summary>
        private Language _appLanguageCached;

        /// <summary>
        /// Application Currency Cached
        /// </summary>
        private Currency _appCurrencyCached;

        HttpCookie _httpCookie;

        #endregion

        #region Utilities



        public void SetUserExpire(long id)
        {
            _httpContextBase.Session["UserExpire-" + id.ToString()] = DateTime.Now;
        }

        public bool CheckUserExpire(long id)
        {
            object o = _httpContextBase.Session["UserExpire-" + id.ToString()];
            if (o != null)
            {
                Setting set = this._settingService.GetSetting("UserSetting.CookieExpireMinutes");

                double cookieExpires = 0;// _userSetting.CookieExpireMinutes;

                if (set != null)
                    double.TryParse(set.Value, out cookieExpires);

                if (cookieExpires == 0) cookieExpires = 30;

                if ((DateTime.Now - (DateTime)o).TotalMinutes > cookieExpires) return true;
            }

            return false;
        }

        public virtual void SetBackLoginUser(User user)
        {
            _httpContextBase.Session["UserBackLogin"] = user.Id;
        }
        public virtual User GetBackLoginUser(bool clear = true)
        {
            long userId = 0;

            try
            {
                userId = (long)_httpContextBase.Session["UserBackLogin"];
            }
            catch
            {
                userId = 0;
            }

            User user = _userService.GetUserById(userId);

            if (clear)
                _httpContextBase.Session["UserBackLogin"] = 0;

            return user;
        }


        public virtual void ClearUserCookie()
        {
            if (_httpContextBase == null || _httpContextBase.Request == null)
                return;

            var currentUserCookie = _httpContextBase.Request.Cookies[CACHE_USER_COOKIE_KEY];

            if (currentUserCookie != null)
            {
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                _httpContextBase.Request.Cookies.Set(currentUserCookie);
                //HttpContext.Current.Response.SetCookie(currentUserCookie);
            }
            //            _httpContextBase.Request.Cookies.Remove(CACHE_USER_COOKIE_KEY);
            SetUserCookie(new Guid());

            //_httpContextBase.Request.Cookies.Clear();
        }

        /// <summary>
        /// Get User Cookie
        /// </summary>
        /// <returns>Http Cookie Item</returns>
        public virtual HttpCookie GetUserCookie()
        {
            if (_httpContextBase == null || _httpContextBase.Request == null)
                return null;

            return _httpContextBase.Request.Cookies[CACHE_USER_COOKIE_KEY];
        }

        /// <summary>
        /// Set User Cookie
        /// </summary>
        /// <param name="userGuid">User Global Unique Identifier</param>
        public virtual void SetUserCookie(Guid userGuid)
        {
            if (_httpContextBase != null && _httpContextBase.Response != null)
            {
                var httpCookie = new HttpCookie(CACHE_USER_COOKIE_KEY);
                httpCookie.HttpOnly = true;
                httpCookie.Value = userGuid.ToString();

                if (userGuid == Guid.Empty)
                {
                    httpCookie.Expires = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    Setting set = this._settingService.GetSetting("UserSetting.CookieExpireMinutes");

                    int cookieExpires = 0;// _userSetting.CookieExpireMinutes;

                    if (set != null)
                        int.TryParse(set.Value, out cookieExpires);

                    if (cookieExpires == 0) cookieExpires = 30;

                    httpCookie.Expires = DateTime.Now.AddMinutes(cookieExpires);
                }

                _httpContextBase.Response.Cookies.Remove(CACHE_USER_COOKIE_KEY);
                _httpContextBase.Response.Cookies.Add(httpCookie);
            }
        }

        /// <summary>
        /// Get Language From Url
        /// </summary>
        /// <returns>Language Item</returns>
        protected virtual Language GetLanguageFromUrl()
        {
            if (_httpContextBase == null || _httpContextBase.Request == null)
                return null;

            string virtualPath = _httpContextBase.Request.AppRelativeCurrentExecutionFilePath;

            string applicationPath = _httpContextBase.Request.ApplicationPath;

            if (!virtualPath.IsLocalizedUrl(applicationPath, false))
                return null;

            var seoCulture = virtualPath.GetLanguageSeoCultureFromUrl(applicationPath, false);

            if (String.IsNullOrEmpty(seoCulture))
                return null;

            var language = _languageService.GetAllLanguages().FirstOrDefault(x => seoCulture.Equals(x.Culture, StringComparison.InvariantCultureIgnoreCase));

            if (language != null && language.Published)
            {
                return language;
            }

            return null;
        }

        /// <summary>
        /// Get Language From Browser Setting
        /// </summary>
        /// <returns>Language Item</returns>
        protected virtual Language GetLanguageFromBrowserSetting()
        {
            if (_httpContextBase == null || _httpContextBase.Request == null || _httpContextBase.Request.UserLanguages == null)
                return null;

            var userLanguage = _httpContextBase.Request.UserLanguages.FirstOrDefault();

            if (String.IsNullOrEmpty(userLanguage))
                return null;

            var language = _languageService.GetAllLanguages().FirstOrDefault(x => userLanguage.Equals(x.Culture, StringComparison.InvariantCultureIgnoreCase));

            if (language != null && language.Published)
            {
                return language;
            }

            return null;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Web Application Context
        /// </summary>
        /// <param name="localizationSetting">Localization Setting</param>
        /// <param name="currencySetting">Currency Setting</param>
        /// <param name="userSetting">User Setting</param>
        /// <param name="httpContextBase">Http Context Base</param>
        /// <param name="userService">User Service</param>
        /// <param name="authenticationService">Authentication Service</param>
        /// <param name="languageService">Language Service</param>
        /// <param name="currencyService">Currency Service</param>
        /// <param name="globalAttributeService">Global Attribute Service</param>
        public WebAppContext(LocalizationSetting localizationSetting, CurrencySetting currencySetting, UserSetting userSetting, HttpContextBase httpContextBase, IUserService userService, IAuthenticationService authenticationService, ILanguageService languageService, ICurrencyService currencyService, IGlobalAttributeService globalAttributeService, ISettingService settingService)
        {
            this._localizationSetting = localizationSetting;
            this._currencySetting = currencySetting;
            this._userSetting = userSetting;
            this._httpContextBase = httpContextBase;
            this._userService = userService;
            this._authenticationService = authenticationService;
            this._languageService = languageService;
            this._currencyService = currencyService;
            this._globalAttributeService = globalAttributeService;
            this._settingService = settingService;
        }

        #endregion

        #region Methods
        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the Application User
        /// </summary>
        public virtual User AppUser
        {
            get
            {
                var user = HttpContext.Current.User;
                if (user is CustomPrincipal)
                {
                    return ((CustomPrincipal)user).User;
                }
                return null;
            }
            set
            {
                _appUserCached = value;
            }
        }

        /// <summary>
        /// Gets or Sets the Application User Impersonate
        /// </summary>
        public virtual User AppUserImpersonate
        {
            get
            {
                return _appUserImpersonate;
            }

        }

        /// <summary>
        /// Gets or Sets the Application Language
        /// </summary>
        public virtual Language AppLanguage
        {
            get
            {
                if (_appLanguageCached != null)
                    return _appLanguageCached;

                Language language = null;
                long languageId = 0;

                if (_localizationSetting.SeoFriendlyUrlsForLanguagesEnabled)
                {
                    // Get Language From Url
                    language = GetLanguageFromUrl();
                }

                if (language == null && _localizationSetting.AutomaticallyDetectLanguage)
                {
                    // Get Language From Browser Setting                    
                    if (!this.AppUser.GetGlobalAttribute<bool>(GlobalAttributeBuiltIn.LanguageAutomaticallyDetected, _globalAttributeService))
                    {
                        language = GetLanguageFromBrowserSetting();

                        if (language != null)
                        {
                            _globalAttributeService.SaveGlobalAttribute(this.AppUser, GlobalAttributeBuiltIn.LanguageAutomaticallyDetected, true);
                        }
                    }
                }

                if (language != null && AppUser != null)
                {
                    // The Language Is Detected. Now We Need To Save It
                    if (this.AppUser.GetGlobalAttribute<long>(GlobalAttributeBuiltIn.LanguageId, _globalAttributeService) != language.Id)
                    {
                        _globalAttributeService.SaveGlobalAttribute(this.AppUser, GlobalAttributeBuiltIn.LanguageId, language.Id);
                    }
                }

                var allLanguages = _languageService.GetAllLanguages();

                // Find Current User Language
                if (AppUser != null)
                    languageId = this.AppUser.GetGlobalAttribute<long>(GlobalAttributeBuiltIn.LanguageId, _globalAttributeService);

                var languageValue = allLanguages.FirstOrDefault(x => x.Id == languageId);

                if (languageValue == null)
                {
                    // If not specified, then return Primary Application Language Id
                    languageValue = allLanguages.FirstOrDefault(x => x.Id == _localizationSetting.PrimaryAppLanguageId);
                }

                if (languageValue == null)
                {
                    // If not specified, then return the first found one
                    languageValue = _languageService.GetAllLanguages().FirstOrDefault();
                }

                // Application Language Cache
                _appLanguageCached = languageValue;

                return _appLanguageCached;
            }
            set
            {
                var languageId = value != null ? value.Id : 0;

                _globalAttributeService.SaveGlobalAttribute(this.AppUser, GlobalAttributeBuiltIn.LanguageId, languageId);

                // Reset Application Language Cache
                _appLanguageCached = null;
            }
        }

        /// <summary>
        /// Gets or Sets the Application Currency
        /// </summary>
        public virtual Currency AppCurrency
        {
            get
            {
                if (_appCurrencyCached != null)
                    return _appCurrencyCached;

                if (this.IsGlobalAdministrator)
                {
                    var primaryExchangeRate = _currencyService.GetCurrencyById(_currencySetting.PrimaryExchangeRateCurrencyId);

                    if (primaryExchangeRate != null)
                    {
                        _appCurrencyCached = primaryExchangeRate;

                        return primaryExchangeRate;
                    }
                }

                var allCurrencies = _currencyService.GetAllCurrencies();

                // Find A Currency Previously Selected By A User
                var currencyId = this.AppUser.GetGlobalAttribute<long>(GlobalAttributeBuiltIn.CurrencyId, _globalAttributeService);

                var currency = allCurrencies.FirstOrDefault(x => x.Id == currencyId);

                if (currency == null)
                {
                    // If Not Found, Then Return The First Found One
                    currency = allCurrencies.FirstOrDefault();
                }

                if (currency == null)
                {
                    // If Not Specified, Then Return The First Found One
                    currency = _currencyService.GetAllCurrencies().FirstOrDefault();
                }

                // Application Currency Cache
                _appCurrencyCached = currency;

                return _appCurrencyCached;
            }
            set
            {
                var currencyId = value != null ? value.Id : 0;

                _globalAttributeService.SaveGlobalAttribute(this.AppUser, GlobalAttributeBuiltIn.CurrencyId, currencyId);

                // Reset Application Currency Cache
                _appCurrencyCached = null;
            }
        }

        /// <summary>
        /// Gets or Sets the Global Administrator (Built-in global administrators have complete and unrestricted access to the full website)
        /// </summary>
        public virtual bool IsGlobalAdministrator { get; set; }

        /// <summary>
        /// Gets or Sets the Content Manager (Built-in content managers have restricted access to the website)
        /// </summary>
        public virtual bool IsContentManager { get; set; }

        /// <summary>
        /// Gets or Sets the Nework User (Built-in network user are registered users)
        /// </summary>
        public virtual bool IsNetworkUser { get; set; }

        #endregion
    }
}