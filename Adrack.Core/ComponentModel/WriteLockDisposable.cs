// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="WriteLockDisposable.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Threading;

namespace Adrack.Core.ComponentModel
{
    /// <summary>
    /// Represents a Write Lock Disposable
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class WriteLockDisposable : IDisposable
    {
        #region Fields

        /// <summary>
        /// Reader Writer Lock Slim
        /// </summary>
        private readonly ReaderWriterLockSlim _readerWriterLockSlim;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Write Lock Disposable
        /// </summary>
        /// <param name="readerWriterLockSlim">Reader Writer Lock Slim</param>
        public WriteLockDisposable(ReaderWriterLockSlim readerWriterLockSlim)
        {
            _readerWriterLockSlim = readerWriterLockSlim;
            _readerWriterLockSlim.EnterWriteLock();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _readerWriterLockSlim.ExitWriteLock();
        }

        /// <summary>
        /// Disposable
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Methods
    }
}