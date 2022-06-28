using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Infrastructure.Constants
{
    public static class UserRoleKeys
    {
        #region user role keys

        public static string GlobalAdministratorsKey => "GlobalAdministrators";
        public static string AccountManagerKey => "AccountManager";
        public static string NetworkUsersKey => "NetworkUsers";
        public static string AdministratorKey => "Adminstrator";
        public static string AffiliateUserKey => "AffilateUser";
        public static string BuyerUserKey => "BuyerUser";
        public static string TestRoleKey => "TestRole";

        #endregion user role keys

    }
}