using Adrack.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.WebApi.Infrastructure.Core.WrapperData
{
    public interface ISharedDataWrapper
    {
        UserTypes GetBuyerUserTypeId();
        UserTypes GetAffiliateUserTypeId();
        UserTypes GetNetworkUserTypeId();
        UserTypes GetBuiltInUserTypeId();
    }
}
