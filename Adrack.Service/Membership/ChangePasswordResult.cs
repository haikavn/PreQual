// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ChangePasswordResult.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;

namespace Adrack.Service.Membership
{
    /// <summary>
    /// Represents a Change Password Result
    /// </summary>
    public class ChangePasswordResult
    {
        #region Constructor

        /// <summary>
        /// Change Password Result
        /// </summary>
        public ChangePasswordResult()
        {
            this.Errors = new List<string>();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Add Error
        /// </summary>
        /// <param name="error">Error</param>
        public void AddError(string error)
        {
            this.Errors.Add(error);
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Success
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success
        {
            get { return this.Errors.Count == 0; }
        }

        /// <summary>
        /// Gets or Sets the Errors
        /// </summary>
        /// <value>The errors.</value>
        public IList<string> Errors { get; set; }

        #endregion Properties
    }
}