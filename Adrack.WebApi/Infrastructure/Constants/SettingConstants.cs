using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Infrastructure.Constants
{
    public class SettingConstants
    {
        public const string CACHE_AUTO_CACHE_MODE = "System.AutoCacheMode";
        public const string CACHE_AUTO_CACHE_URLS = "System.AutoCacheUrls";
        public const string CACHE_APP_SETTING_URL = "AppSetting.Url";
        public const string CACHE_APP_SETTING_URL_VALUE = "/";

        public const int CACHE_AUTO_CACHE_MODE_ENABLED = 1;

        public static string AutoCacheMode => CACHE_AUTO_CACHE_MODE;
        public static string AutoCacheUrls => CACHE_AUTO_CACHE_URLS;
        public static string AppSettingUrl => CACHE_APP_SETTING_URL;
        public static string AppSettingUrlValue => CACHE_APP_SETTING_URL_VALUE;

        public static int IsAutoCacheModeEnabled => CACHE_AUTO_CACHE_MODE_ENABLED;

    }
}