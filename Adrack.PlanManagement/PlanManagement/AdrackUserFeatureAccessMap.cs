using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Adrack.Core.Domain.Membership;

namespace Adrack.PlanManagement
{
    public class AdrackUserFeatureAccessMap
    {
        List<FeatureAccess> Features;
        AdrackManagementPlan plan;
        User user;

        public void SetUser(User user)
        {            
            this.user = user;
            //user.AccessMap = this;
        }

        public void AddFeaturesFromService(object service)
        {
            var servicename = service.GetType().Name;
            MethodInfo[] methods=service.GetType().GetMethods();

            for (var i=0; i<methods.Length; i++)
            {
                FeatureAccess access = new FeatureAccess();
                access.ServiceName = servicename;
                access.MethodName = methods[i].Name;
                access.Enabled = true;
                this.Features.Add(access);
            }
        }

        public object CallProcedure(string MethodName, Object service, params object[] list)
        {
            var servicename = service.GetType().Name;
            FeatureAccess access=Features.Find(a => a.ServiceName==servicename && a.MethodName == MethodName);
            if (access!=null)
                return access.CallProcedure(service, list);
            else
                throw new Exception(servicename + "." + MethodName + " | Access denied");
        }

        public bool ValidateProcedure(string MethodName, Object service)
        {
            var servicename = service.GetType().Name;
            FeatureAccess access = Features.Find(a => a.ServiceName == servicename && a.MethodName == MethodName);
            if (access != null && access.Enabled)
                return true;
            else
                throw new Exception(servicename + "." + MethodName + " | Access denied");
        }


    }
}