// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="User.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Attributes;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adrack.Core.Domain.Membership
{
    /// <summary>
    /// Represents a User
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    /// 
    [Tracked]
    public class User : BaseEntity
    {
        #region Fields

        /// <summary>
        /// Role
        /// </summary>
        private ICollection<Role> _roles;

        /// <summary>
        /// Permission
        /// </summary>
        private ICollection<Permission> _permissions;

        /// <summary>
        /// Authentication
        /// </summary>
        private ICollection<Authentication> _authentications;



        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or Sets the Parent Identifier
        /// </summary>
        /// <value>The parent identifier.</value>
        public long ParentId { get; set; }

        /// <summary>
        /// Gets or Sets the Globally Unique Identifier
        /// </summary>
        /// <value>The gu identifier.</value>
        public string GuId { get; set; }

        /// <summary>
        /// Gets or Sets the Username
        /// </summary>
        /// <value>The username.</value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or Sets the Email
        /// </summary>
        /// <value>The email.</value>
        /// 
        [Tracked]
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets the contact Email
        /// </summary>
        /// <value>The contact email.</value>
        public string ContactEmail { get; set; }

        /// <summary>
        /// Gets or Sets the Password
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or Sets the Salt Key
        /// </summary>
        /// <value>The salt key.</value>
        public string SaltKey { get; set; }

        /// <summary>
        /// Gets or Sets the Active
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        /// 
        [Tracked]
        public bool Active { get; set; }

        /// <summary>
        /// Gets or Sets the Lock Out
        /// </summary>
        /// <value><c>true</c> if [locked out]; otherwise, <c>false</c>.</value>
        public bool LockedOut { get; set; }

        /// <summary>
        /// Gets or Sets the Deleted
        /// </summary>
        /// <value><c>true</c> if deleted; otherwise, <c>false</c>.</value>
        /// 
        [Tracked]
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or Sets the Built In
        /// </summary>
        /// <value><c>true</c> if [built in]; otherwise, <c>false</c>.</value>
        public bool BuiltIn { get; set; }

        /// <summary>
        /// Gets or Sets the Built In Name
        /// </summary>
        /// <value>The name of the built in.</value>
        public string BuiltInName { get; set; }

        /// <summary>
        /// Gets or Sets the Registration Date
        /// </summary>
        /// <value>The registration date.</value>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Gets or Sets the Login Date
        /// </summary>
        /// <value>The login date.</value>
        public DateTime LoginDate { get; set; }

        /// <summary>
        /// Gets or Sets the Activity Date
        /// </summary>
        /// <value>The activity date.</value>
        public DateTime ActivityDate { get; set; }

        /// <summary>
        /// Gets or Sets the Password Changed Date
        /// </summary>
        /// <value>The password changed date.</value>
        public DateTime? PasswordChangedDate { get; set; }

        /// <summary>
        /// Gets or Sets the Lockout Date
        /// </summary>
        /// <value>The lockout date.</value>
        public DateTime? LockoutDate { get; set; }

        /// <summary>
        /// Gets or Sets the Ip Address
        /// </summary>
        /// <value>The ip address.</value>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or Sets the Failed Password Attempt Count
        /// </summary>
        /// <value>The failed password attempt count.</value>
        public int? FailedPasswordAttemptCount { get; set; }

        /// <summary>
        /// Gets or Sets the Comment
        /// </summary>
        /// <value>The comment.</value>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or Sets the Department Identifier
        /// </summary>
        /// <value>The department identifier.</value>ff
        public long? DepartmentId { get; set; }

        /// <summary>
        /// Gets or Sets the Department Identifier
        /// </summary>
        /// <value>The type of the menu.</value>
        public short? MenuType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [mask email].
        /// </summary>
        /// <value><c>true</c> if [mask email]; otherwise, <c>false</c>.</value>
        public bool MaskEmail { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [validate on login].
        /// </summary>
        /// <value><c>true</c> if [validate on login]; otherwise, <c>false</c>.</value>
        public bool ValidateOnLogin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [change pass on login].
        /// </summary>
        /// <value><c>null</c> if [change pass on login] contains no value, <c>true</c> if [change pass on login]; otherwise, <c>false</c>.</value>
        public bool? ChangePassOnLogin { get; set; }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets the remote login unique identifier.
        /// </summary>
        /// <value>The remote login unique identifier.</value>
        public Guid? RemoteLoginGuid { get; set; }

        /// <summary>
        /// Gets or sets the Profile Picture Path.
        /// </summary>
        /// <value>The Profile Picture Path.</value>
        public string ProfilePicturePath { get; set; }

        public UserTypes UserType { get; set; }

        public long UserTypeId { get; set; }


        /// <summary>
        /// Gets or sets the PlanId identifier.
        /// </summary>
        /// <value>The PlanId identifier.</value>
        public long? PlanId { get; set; }

        #endregion Properties

        #region Navigation Properties

        /// <summary>
        /// Gets or sets the department.
        /// </summary>
        /// <value>The department.</value>
        public virtual Department Department { get; set; }

        /// <summary>
        /// Gets or Sets the Role
        /// </summary>
        /// <value>The roles.</value>
        public virtual ICollection<Role> Roles
        {
            get { return _roles ?? (_roles = new List<Role>()); }
            protected set { _roles = value; }
        }

        /// <summary>
        /// Gets or Sets the Permission
        /// </summary>
        /// <value>The permissions.</value>
        public virtual ICollection<Permission> Permissions
        {
            get { return _permissions ?? (_permissions = new List<Permission>()); }
            protected set { _permissions = value; }
        }

        /// <summary>
        /// Gets or Sets the Authentication
        /// </summary>
        /// <value>The authentications.</value>
        public virtual ICollection<Authentication> Authentications
        {
            get { return _authentications ?? (_authentications = new List<Authentication>()); }
            protected set { _authentications = value; }
        }

        #endregion Navigation Properties
    }
}