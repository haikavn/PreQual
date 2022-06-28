// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="FakeHttpSessionState.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;

namespace Adrack.Core.Fakes
{
    /// <summary>
    /// Represents a Fake Http Session State
    /// Implements the <see cref="System.Web.HttpSessionStateBase" />
    /// </summary>
    /// <seealso cref="System.Web.HttpSessionStateBase" />
    public class FakeHttpSessionState : HttpSessionStateBase
    {
        #region Fields

        /// <summary>
        /// Session State Item Collection
        /// </summary>
        private readonly SessionStateItemCollection _sessionStateItemCollection;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Fake Http Session State
        /// </summary>
        /// <param name="sessionStateItemCollection">Session State Item Collection</param>
        public FakeHttpSessionState(SessionStateItemCollection sessionStateItemCollection)
        {
            _sessionStateItemCollection = sessionStateItemCollection;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Exists
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Boolean Item</returns>
        public bool Exists(string key)
        {
            return _sessionStateItemCollection[key] != null;
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        public override void Add(string name, object value)
        {
            _sessionStateItemCollection[name] = value;
        }

        /// <summary>
        /// Enumerator
        /// </summary>
        /// <returns>Enumerator Item Collection</returns>
        public override IEnumerator GetEnumerator()
        {
            return _sessionStateItemCollection.GetEnumerator();
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="name">Name</param>
        public override void Remove(string name)
        {
            _sessionStateItemCollection.Remove(name);
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Count
        /// </summary>
        /// <value>The count.</value>
        public override int Count
        {
            get { return _sessionStateItemCollection.Count; }
        }

        /// <summary>
        /// Gets or Sets the Keys
        /// </summary>
        /// <value>The keys.</value>
        public override NameObjectCollectionBase.KeysCollection Keys
        {
            get { return _sessionStateItemCollection.Keys; }
        }

        /// <summary>
        /// Gets or Sets the Object Name
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Session State Item</returns>
        public override object this[string name]
        {
            get { return _sessionStateItemCollection[name]; }
            set { _sessionStateItemCollection[name] = value; }
        }

        /// <summary>
        /// Gets or Sets the Object Index
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Session State Item</returns>
        public override object this[int index]
        {
            get { return _sessionStateItemCollection[index]; }
            set { _sessionStateItemCollection[index] = value; }
        }

        #endregion Properties
    }
}