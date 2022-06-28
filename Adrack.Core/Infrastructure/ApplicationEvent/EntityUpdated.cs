// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="EntityUpdated.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Infrastructure.ApplicationEvent
{
    /// <summary>
    /// Represents a Entity Updated (A container for entities that are updated)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityUpdated<T> where T : BaseEntity
    {
        #region Constructor

        /// <summary>
        /// Entity Updated
        /// </summary>
        /// <param name="entity">Entity</param>
        public EntityUpdated(T entity)
        {
            this.Entity = entity;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or Sets the Entity
        /// </summary>
        /// <value>The entity.</value>
        public T Entity { get; private set; }

        #endregion Properties
    }
}