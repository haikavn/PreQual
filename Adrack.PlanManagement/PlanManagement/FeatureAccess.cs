using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Adrack.PlanManagement
{
    public class FeatureAccess
    {
        public string ServiceName;
        public string MethodName;
        public bool Enabled;
        public object CallProcedure(Object service, params object[] list) //dynamic parameters
        {
            if (!Enabled) throw new Exception(ServiceName+"."+MethodName+" | Access denied");
            MethodInfo method=service.GetType().GetMethod(MethodName, BindingFlags.Public);
            return method.Invoke(service, list);
        }
    }
}