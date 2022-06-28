using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Infrastructure.Enums
{
    public enum UserRoleType
    {
        GlobalAdministrators = 1,
        AccountManager = 2,
        NetworkUsers = 3,
        Admin = 4,
        AffiliateUser = 5,
        BuyerUser = 6,
        TestRole = 7
    }
}