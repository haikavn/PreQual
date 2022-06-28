// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IFormTemplateService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IVerticalService
    /// </summary>
    public partial interface IVerticalService
    {
        #region Methods

        /// <summary>
        /// Get Vertical By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        Vertical GetVerticalById(long Id);

        /// <summary>
        /// Get Vertical By Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Vertical GetVerticalByName(string name);

        /// <summary>
        /// Get Vertical Field By Id
        /// </summary>
        /// <param name="Id">long</param>
        /// <returns>VerticalField</returns>
        VerticalField GetVerticalFieldById(long Id);

        /// <summary>
        /// Get All Verticals
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<Vertical> GetAllVerticals();

        /// <summary>
        /// Insert Vertical
        /// </summary>
        /// <param name="vertical">The vertical.</param>
        /// <returns>System.Int64.</returns>
        long InsertVertical(Vertical vertical);

        /// <summary>
        /// Update Vertical
        /// </summary>
        /// <param name="vertical">The vertical.</param>
        void UpdateVertical(Vertical vertical);

        /// <summary>
        /// Delete Vertical
        /// </summary>
        /// <param name="vertical">The vertical.</param>
        void DeleteVertical(Vertical vertical);

        /// <summary>
        /// Get Vertical Fields
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<VerticalField> GetAllVerticalFields();

        IList<VerticalField> GetAllVerticalFields(long verticalId);


        /// <summary>
        /// Insert Vertical Field
        /// </summary>
        /// <param name="verticalField">The verticalField.</param>
        /// <returns>System.Int64.</returns>
        long InsertVerticalField(VerticalField verticalField);

        /// <summary>
        /// Update Vertical Field
        /// </summary>
        /// <param name="verticalField">The verticalField.</param>
        void UpdateVerticalField(VerticalField verticalField);

        /// <summary>
        /// Delete Vertical Field
        /// </summary>
        /// <param name="verticalField">The verticalField.</param>
        void DeleteVerticalField(VerticalField verticalField);

        /// <summary>
        /// Load Vertical Field Names From Xml
        /// </summary>
        /// <param name="xml"></param>
        List<string> LoadFieldNamesFromXml(string xml);

        #endregion Methods
    }
}