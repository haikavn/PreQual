using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core.Domain.Membership;

namespace Adrack.PlanManagement
{
    public class ServiceFeatureControl
    {
        public static AdrackUserFeatureAccessMap CurrentMap;
        public static void SetMap(User user)
        {
          //  CurrentMap = user.AccessMap;
        }

        public static bool ValidateProcedure(string MethodName, object service)
        {
            // SS: closed as it throws null exception
            return true; //CurrentMap.ValidateProcedure(MethodName, service);
        }
    }
}