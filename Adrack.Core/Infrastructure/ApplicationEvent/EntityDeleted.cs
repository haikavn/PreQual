// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="EntityDeleted.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Infrastructure.ApplicationEvent
{
    /// <summary>
    /// Represents a Entity Deleted (A container for passing entities that have been deleted. This is not used for entities that are deleted logicaly via a bit column)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityDeleted<T> where T : BaseEntity
    {
        #region Constructor

        /// <summary>
        /// Entity Deleted
        /// </summary>
        /// <param name="entity">Entity</param>
        public EntityDeleted(T entity)
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