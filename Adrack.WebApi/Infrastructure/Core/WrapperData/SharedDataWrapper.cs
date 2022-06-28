using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core;
using Adrack.Service.Helpers;

namespace Adrack.WebApi.Infrastructure.Core.WrapperData
{
    public class SharedDataWrapper : ISharedDataWrapper
    {
        public UserTypes GetBuyerUserTypeId()
        {
            return SharedData.BuyerUserTypeId;

        }

        public UserTypes GetAffiliateUserTypeId()
        {
            return SharedData.AffiliateUserTypeId;

        }

        public UserTypes GetBuiltInUserTypeId()
        {
            return SharedData.BuiltInUserTypeId;

        }

        public UserTypes GetNetworkUserTypeId()
        {
            return SharedData.NetowrkUserTypeId;

        }
    }
}