// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IVerticalService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IFormTemplateService
    /// </summary>
    public partial interface IFormTemplateService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        FormTemplate GetFormTemplateById(long Id);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<FormTemplate> GetAllFormTemplates();

        IList<FormTemplate> GetAllFormTemplates(long affiliateChannelId);


        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="formTemplate">The form template.</param>
        /// <returns>System.Int64.</returns>
        long InsertFormTemplate(FormTemplate formTemplate);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="formTemplate">The form template.</param>
        void UpdateFormTemplate(FormTemplate formTemplate);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="formTemplate">The form template.</param>
        void DeleteFormTemplate(FormTemplate formTemplate);


        FormTemplateItem GetFormTemplateItemById(long Id);
        IList<FormTemplateItem> GetFormTemplateItemsByTemplateId(long Id);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="formTemplate">The form template.</param>
        /// <returns>System.Int64.</returns>
        long InsertFormTemplateItem(FormTemplateItem formTemplateItem);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="formTemplate">The form template.</param>
        void UpdateFormTemplateItem(FormTemplateItem formTemplate);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="formTemplate">The form template.</param>
        void DeleteFormTemplateItem(FormTemplateItem formTemplate);

        void DeleteFormTemplateItems(FormTemplate formTemplate);

        #endregion Methods
    }
}