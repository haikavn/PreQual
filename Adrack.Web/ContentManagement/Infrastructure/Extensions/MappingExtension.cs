// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="MappingExtension.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Audit;
using Adrack.Core.Domain.Message;
using Adrack.Web.ContentManagement.Models.Audit;
using Adrack.Web.ContentManagement.Models.Message;
using AutoMapper;

namespace Adrack.Web.ContentManagement.Infrastructure.Extensions
{
    /// <summary>
    /// Represents a Mapping Extension
    /// </summary>
    public static class MappingExtension
    {
        #region Methods

        /// <summary>
        /// Type Destination
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
        /// Type Destination
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

        #region Audit

        /// <summary>
        /// Log Model
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Log Model Item</returns>
        public static LogModel ToModel(this Log entity)
        {
            return entity.MapTo<Log, LogModel>();
        }

        /// <summary>
        /// To Entity
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Log Item</returns>
        public static Log ToEntity(this LogModel model)
        {
            return model.MapTo<LogModel, Log>();
        }

        /// <summary>
        /// To Entity
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="destination">Destination</param>
        /// <returns>Log Item</returns>
        public static Log ToEntity(this LogModel model, Log destination)
        {
            return model.MapTo(destination);
        }

        #endregion Audit

        #region Message

        /// <summary>
        /// Smtp Account Model
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Smtp Account Model Item</returns>
        public static SmtpAccountModel ToModel(this SmtpAccount entity)
        {
            return entity.MapTo<SmtpAccount, SmtpAccountModel>();
        }

        /// <summary>
        /// To Entity
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Smtp Account Item</returns>
        public static SmtpAccount ToEntity(this SmtpAccountModel model)
        {
            return model.MapTo<SmtpAccountModel, SmtpAccount>();
        }

        /// <summary>
        /// To Entity
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="destination">Destination</param>
        /// <returns>Smtp Account Item</returns>
        public static SmtpAccount ToEntity(this SmtpAccountModel model, SmtpAccount destination)
        {
            return model.MapTo(destination);
        }

        #endregion Message

        #endregion Methods
    }
}