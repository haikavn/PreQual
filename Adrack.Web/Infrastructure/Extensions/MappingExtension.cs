// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="MappingExtension.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using AutoMapper;

namespace Adrack.Web.Infrastructure.Extensions
{
    /// <summary>
    ///     Represents a Mapping Extension
    /// </summary>
    public static class MappingExtension
    {
        #region Methods

        /// <summary>
        ///     Type Destination
        /// </summary>
        /// <typeparam name="TSource">Type Source</typeparam>
        /// <typeparam name="TDestination">Type Destination</typeparam>
        /// <param name="source">Source</param>
        /// <returns>Type Destination</returns>
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        /// <summary>
        ///     Type Destination
        /// </summary>
        /// <typeparam name="TSource">Type Source</typeparam>
        /// <typeparam name="TDestination">Type Destination</typeparam>
        /// <param name="source">Source</param>
        /// <param name="destination">Destination</param>
        /// <returns>Type Destination</returns>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return Mapper.Map(source, destination);
        }

        #endregion Methods
    }
}