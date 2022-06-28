// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="EventPublisher.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;

namespace Adrack.Core.Domain.Message
{
    /// <summary>
    /// Represents a Email Subscribe Event Publisher
    /// </summary>
    public class EmailSubscribeEventPublisher
    {
        #region Fields

        /// <summary>
        /// Email
        /// </summary>
        private readonly string _email;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Email Subscribe Event Publisher
        /// </summary>
        /// <param name="email">Email</param>
        public EmailSubscribeEventPublisher(string email)
        {
            this._email = email;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="other">Other</param>
        /// <returns>Boolean Item</returns>
        public bool Equals(EmailSubscribeEventPublisher other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Equals(other._email, _email);
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Boolean Item</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != typeof(EmailSubscribeEventPublisher))
                return false;

            return Equals((EmailSubscribeEventPublisher)obj);
        }

        /// <summary>
        /// Get Hash Code
        /// </summary>
        /// <returns>Integer Item</returns>
        public override int GetHashCode()
        {
            return (_email != null ? _email.GetHashCode() : 0);
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Email
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            get { return _email; }
        }

        #endregion Properties
    }

    /// <summary>
    /// Represents a Email Unsubscribe Event Publisher
    /// </summary>
    public class EmailUnsubscribeEventPublisher
    {
        #region Fields

        /// <summary>
        /// Email
        /// </summary>
        private readonly string _email;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Email Subscribe Event Publisher
        /// </summary>
        /// <param name="email">Email</param>
        public EmailUnsubscribeEventPublisher(string email)
        {
            this._email = email;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="other">Other</param>
        /// <returns>Boolean Item</returns>
        public bool Equals(EmailUnsubscribeEventPublisher other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Equals(other._email, _email);
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Boolean Item</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != typeof(EmailUnsubscribeEventPublisher))
                return false;

            return Equals((EmailUnsubscribeEventPublisher)obj);
        }

        /// <summary>
        /// Get Hash Code
        /// </summary>
        /// <returns>Integer Item</returns>
        public override int GetHashCode()
        {
            return (_email != null ? _email.GetHashCode() : 0);
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Email
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            get { return _email; }
        }

        #endregion Properties
    }

    /// <summary>
    /// Represents a Entity Token Added Event
    /// </summary>
    /// <typeparam name="T">Entity Type</typeparam>
    /// <typeparam name="U">Token</typeparam>
    public class EntityTokenAddedEvent<T, U> where T : BaseEntity
    {
        #region Fields

        /// <summary>
        /// Entity Type
        /// </summary>
        private readonly T _entityType;

        /// <summary>
        /// Tokens
        /// </summary>
        private readonly IList<U> _tokens;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Entity Token Added Event
        /// </summary>
        /// <param name="entityType">Entity Type</param>
        /// <param name="tokens">Tokens Value</param>
        public EntityTokenAddedEvent(T entityType, IList<U> tokens)
        {
            this._entityType = entityType;
            this._tokens = tokens;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or Sets the Entity Type
        /// </summary>
        /// <value>The entity.</value>
        public T Entity
        {
            get { return _entityType; }
        }

        /// <summary>
        /// Gets or Sets the Tokens
        /// </summary>
        /// <value>The tokens.</value>
        public IList<U> Tokens
        {
            get { return _tokens; }
        }

        #endregion Properties
    }

    /// <summary>
    /// Represents a Email Template Token Added Event
    /// </summary>
    /// <typeparam name="U">Token Value</typeparam>
    public class EmailTemplateTokenAddedEvent<U>
    {
        #region Fields

        /// <summary>
        /// Email Template
        /// </summary>
        private readonly EmailTemplate _emailTemplate;

        /// <summary>
        /// Tokens
        /// </summary>
        private readonly IList<U> _tokens;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Email Template Token Added Event
        /// </summary>
        /// <param name="emailTemplate">Email Template</param>
        /// <param name="tokens">Tokens</param>
        public EmailTemplateTokenAddedEvent(EmailTemplate emailTemplate, IList<U> tokens)
        {
            this._emailTemplate = emailTemplate;
            this._tokens = tokens;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or Sets the Email Template
        /// </summary>
        /// <value>The email template.</value>
        public EmailTemplate EmailTemplate
        {
            get { return _emailTemplate; }
        }

        /// <summary>
        /// Gets or Sets the Tokens
        /// </summary>
        /// <value>The tokens.</value>
        public IList<U> Tokens
        {
            get { return _tokens; }
        }

        #endregion Properties
    }
}