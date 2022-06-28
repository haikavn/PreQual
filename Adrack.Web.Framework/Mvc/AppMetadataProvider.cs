// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="AppMetadataProvider.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Adrack.Web.Framework.Mvc
{
    /// <summary>
    /// Represents a Application Metadata Provider
    /// Implements the <see cref="System.Web.Mvc.DataAnnotationsModelMetadataProvider" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.DataAnnotationsModelMetadataProvider" />
    public class AppMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        #region Utilities

        /// <summary>
        /// Create Metadata
        /// </summary>
        /// <param name="attributes">Attribute</param>
        /// <param name="containerType">Container Type</param>
        /// <param name="modelAccessor">Model Accessor</param>
        /// <param name="modelType">Model Type</param>
        /// <param name="propertyName">Property Name</param>
        /// <returns>Model Metadata Item</returns>
        /// <exception cref="AppException">There is already an attribute with the name of \"" + additionalValue.Name + "\" on this model.</exception>
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            var additionalValues = attributes.OfType<IModelAttribute>().ToList();

            foreach (var additionalValue in additionalValues)
            {
                if (metadata.AdditionalValues.ContainsKey(additionalValue.Name))
                    throw new AppException("There is already an attribute with the name of \"" + additionalValue.Name + "\" on this model.");

                metadata.AdditionalValues.Add(additionalValue.Name, additionalValue);
            }

            return metadata;
        }

        #endregion Utilities
    }
}