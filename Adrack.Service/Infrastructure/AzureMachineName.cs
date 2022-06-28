// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AzureMachineName.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Service.Infrastructure
{
    /// <summary>
    /// Represents a Azure Machine Name
    /// Implements the <see cref="Adrack.Service.Infrastructure.IMachineName" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Infrastructure.IMachineName" />
    public class AzureMachineName : IMachineName
    {
        #region Methods

        /// <summary>
        /// Get Machine Name
        /// </summary>
        /// <returns>String Item</returns>
        public string GetMachineName()
        {
            //use the code below if run on Windows Azure cloud services (web roles)
            //return Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment.CurrentRoleInstance.Id;

            var name = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID");

            if (String.IsNullOrEmpty(name))
                name = Environment.MachineName;

            return name;
        }

        #endregion Methods
    }
}