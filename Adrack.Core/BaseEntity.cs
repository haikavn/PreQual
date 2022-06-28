// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BaseEntity.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core
{
    /// <summary>
    /// Represents a Base Entity
    /// </summary>
    public abstract partial class BaseEntity
    {
        #region Utilities

        /// <summary>
        /// Is Transient
        /// </summary>
        /// <param name="baseEntity">Base Entity</param>
        /// <returns>Boolean</returns>
        private static bool IsTransient(BaseEntity baseEntity)
        {
            return baseEntity != null && Equals(baseEntity.Id, default(long));
        }

        /// <summary>
        /// Get Unproxied Type
        /// </summary>
        /// <returns>Type</returns>
        private Type GetUnproxiedType()
        {
            return GetType();
        }

        #endregion Utilities

        #region Properties

        /// <summary>
        /// Gets or Sets the Entity Identifier
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="baseEntity">Base Entity</param>
        /// <returns>Boolean</returns>
        public virtual bool Equals(BaseEntity baseEntity)
        {
            if (baseEntity == null)
                return false;

            if (ReferenceEquals(this, baseEntity))
                return true;

            if (!IsTransient(this) && !IsTransient(baseEntity) && Equals(Id, baseEntity.Id))
            {
                var otherType = baseEntity.GetUnproxiedType();

                var thisType = GetUnproxiedType();

                return thisType.IsAssignableFrom(otherType) || otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        /// <summary>
        /// Get Hash Code
        /// </summary>
        /// <returns>Integer</returns>
        public override int GetHashCode()
        {
            if (Equals(Id, default(long)))
                return base.GetHashCode();

            return Id.GetHashCode();
        }

        /// <summary>
        /// Operator
        /// </summary>
        /// <param name="x">Base Entity</param>
        /// <param name="y">Base Entity</param>
        /// <returns>Boolean</returns>
        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        /// <summary>
        /// Operator
        /// </summary>
        /// <param name="x">Base Entity</param>
        /// <param name="y">Base Entity</param>
        /// <returns>Boolean</returns>
        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !(x == y);
        }

        #endregion Methods
    }
}