// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="BaseAppModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.Framework.Mvc
{
    /// <summary>
    /// Represents a Base Application Model
    /// </summary>
    [ModelBinder(typeof(AppModelBinder))]
    public partial class BaseAppModel
    {
        #region Constructor

        /// <summary>
        /// Base Application Model
        /// </summary>
        public BaseAppModel()
        {
            this.CustomProperties = new Dictionary<string, object>();

            PostInitialize();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Bind Model
        /// </summary>
        /// <param name="controllerContext">Controller Context</param>
        /// <param name="modelBindingContext">Model Binding Context</param>
        public virtual void BindModel(ControllerContext controllerContext, ModelBindingContext modelBindingContext)
        {
        }

        /// <summary>
        /// Developers can override this method in custom partial classes in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Custom Properties
        /// </summary>
        /// <value>The custom properties.</value>
        public Dictionary<string, object> CustomProperties { get; set; }

        #endregion Properties
    }

    /// <summary>
    /// Represents a Base Entity Application Model
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public partial class BaseAppEntityModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Entity Identifier
        /// </summary>
        /// <value>The identifier.</value>
        public virtual long Id { get; set; }

        #endregion Properties
    }
}