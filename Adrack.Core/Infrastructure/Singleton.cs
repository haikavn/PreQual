// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Singleton.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Adrack.Core.Infrastructure
{
    /// <summary>
    /// Represents a Singleton (A statically compiled "singleton" used to store objects throughout the lifetime of the app domain. Not so much singleton in the pattern's sense of the word as a standardized way to store single instances)
    /// Implements the <see cref="Adrack.Core.Infrastructure.Singleton" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Adrack.Core.Infrastructure.Singleton" />
    public class Singleton<T> : Singleton
    {
        #region Constants

        /// <summary>
        /// The instance
        /// </summary>
        private static T instance;

        #endregion Constants

        #region Properties

        /// <summary>
        /// The singleton instance for the specified type T. Only one instance (at the time) of this object for each type of T
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get { return instance; }
            set
            {
                instance = value;
                AllSingletons[typeof(T)] = value;
            }
        }

        #endregion Properties
    }

    /// <summary>
    /// Represents a Singleton (Provides a singleton list for a certain type)
    /// Implements the <see cref="Adrack.Core.Infrastructure.Singleton{System.Collections.Generic.IList{T}}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Adrack.Core.Infrastructure.Singleton{System.Collections.Generic.IList{T}}" />
    public class SingletonList<T> : Singleton<IList<T>>
    {
        #region Constructor

        /// <summary>
        /// Singleton List
        /// </summary>
        static SingletonList()
        {
            Singleton<IList<T>>.Instance = new List<T>();
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// The singleton instance for the specified type T. Only one instance (at the time) of this list for each type of T
        /// </summary>
        /// <value>The instance.</value>
        public new static IList<T> Instance
        {
            get { return Singleton<IList<T>>.Instance; }
        }

        #endregion Properties
    }

    /// <summary>
    /// Represents a Singleton (Provides a singleton dictionary for a certain key and vlaue type)
    /// Implements the <see cref="Adrack.Core.Infrastructure.Singleton{System.Collections.Generic.IDictionary{TKey, TValue}}" />
    /// </summary>
    /// <typeparam name="TKey">The type of the t key.</typeparam>
    /// <typeparam name="TValue">The type of the t value.</typeparam>
    /// <seealso cref="Adrack.Core.Infrastructure.Singleton{System.Collections.Generic.IDictionary{TKey, TValue}}" />
    public class SingletonDictionary<TKey, TValue> : Singleton<IDictionary<TKey, TValue>>
    {
        #region Constructor

        /// <summary>
        /// Singleton Dictionary
        /// </summary>
        static SingletonDictionary()
        {
            Singleton<Dictionary<TKey, TValue>>.Instance = new Dictionary<TKey, TValue>();
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Dictionary
        /// </summary>
        /// <value>The instance.</value>
        public new static IDictionary<TKey, TValue> Instance
        {
            get { return Singleton<Dictionary<TKey, TValue>>.Instance; }
        }

        #endregion Properties
    }

    /// <summary>
    /// Represents a Singleton (Provides access to all "singletons")
    /// Implements the <see cref="Adrack.Core.Infrastructure.Singleton" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.Singleton" />
    public class Singleton
    {
        #region Fields

        /// <summary>
        /// All singletons
        /// </summary>
        private static readonly IDictionary<Type, object> allSingletons;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Singleton
        /// </summary>
        static Singleton()
        {
            allSingletons = new Dictionary<Type, object>();
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Dictionary of type to singleton instances
        /// </summary>
        /// <value>All singletons.</value>
        public static IDictionary<Type, object> AllSingletons
        {
            get { return allSingletons; }
        }

        #endregion Properties
    }
}