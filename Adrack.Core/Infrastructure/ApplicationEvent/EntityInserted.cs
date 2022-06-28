// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="EntityInserted.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Infrastructure.ApplicationEvent
{
    /// <summary>
    /// Represents a Entity Inserted (A container for entities that have been inserted)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityInserted<T> where T : BaseEntity
    {
        #region Constructor

        /// <summary>
        /// Entity Inserted
        /// </summary>
        /// <param name="entity">Entity</param>
        public EntityInserted(T entity)
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