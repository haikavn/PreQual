// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="AppModelBinder.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Web.Mvc;

namespace Adrack.Web.Framework.Mvc
{
    /// <summary>
    /// Represents a Application Model Binder
    /// Implements the <see cref="System.Web.Mvc.DefaultModelBinder" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.DefaultModelBinder" />
    public class AppModelBinder : DefaultModelBinder
    {
        #region Methods

        /// <summary>
        /// Bind Model
        /// </summary>
        /// <param name="controllerContext">Controller Context</param>
        /// <param name="modelBindingContext">Model Binding Context</param>
        /// <returns>Object Item</returns>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext modelBindingContext)
        {
            var bindModel = base.BindModel(controllerContext, modelBindingContext);

            if (bindModel is BaseAppModel)
            {
                ((BaseAppModel)bindModel).BindModel(controllerContext, modelBindingContext);
            }

            return bindModel;
        }

        #endregion Methods
    }
}