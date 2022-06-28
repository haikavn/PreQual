using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adrack.PlanManagement
{
    public class Component
    {

        #region Properties


        public Guid ApplicationId;

        public int ComponentId;

      

        #endregion Properties


        #region Constructors


        public Component()
        {
          
        }

       

        #endregion Constructors


        #region Function 


        public virtual string GetSerializedVersion()
        {
            return "";
        }

        public virtual string GetComponentValidationRule()
        {
            return "";
        }


        #endregion Function

    }
}
