// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IUserService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Domain.Membership;
using System;
using System.Collections.Generic;

namespace Adrack.Service.Membership
{
    /// <summary>
    /// Represents a User Service
    /// </summary>
    public partial interface IUserService
    {
        #region Methods

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="userId">User Identifier</param>
        /// <returns>User Item</returns>
        User GetUserById(long userId);


        /// <summary>
        /// Gets the user by parent identifier.
        /// </summary>
        /// <param name="parentid">The parentid.</param>
        /// <param name="usertypeid">The usertypeid.</param>
        /// <returns>User.</returns>
        User GetUserByParentId(long parentid, UserTypes userType);

        /// <summary>
        /// Gets the users by parent identifier.
        /// </summary>
        /// <param name="parentid">The parentid.</param>
        /// <param name="usertypeid">The usertypeid.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;User&gt;.</returns>
        IList<User> GetUsersByParentId(long parentid, UserTypes userType, short deleted = 0);

        /// <summary>
        /// Get User By User GuId
        /// </summary>
        /// <param name="userGuId">User Globally Unique Identifier</param>
        /// <returns>User Item</returns>
        User GetUserByUserGuId(string userGuId);

        /// <summary>
        /// Get User By Username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User Item</returns>
        User GetUserByUsername(string username);

        /// <summary>
        /// Get User By Email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User Item</returns>
        User GetUserByEmail(string email);

        /// <summary>
        /// Check Global Attribute
        /// </summary>
        /// <param name="key">Global Attribute's Key</param>
        /// <param name="value">Global Attribute's Value</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool CheckGlobalAttribute(string key, string value);

        /// <summary>
        /// Get User By Built In Name
        /// </summary>
        /// <param name="builtInName">Built In Name</param>
        /// <returns>User Item</returns>
        User GetUserByBuiltInName(string builtInName);

        /// <summary>
        /// Gets the user by remote login unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>User.</returns>
        User GetUserByRemoteLoginGuid(Guid guid);

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <param name="pageIndex">Page Index</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>User Collection Item</returns>
        IPagination<User> GetAllUsers(short deleted = 0, int pageIndex = 0, int pageSize = int.MaxValue);


        /// <summary>
        /// Get Users By Email
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="pageIndex">Page Index</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>User Collection Item</returns>
        IList<User> GetUsersByEmail(string email = "");

        /// <summary>
        /// Gets the system users.
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>IPagination&lt;User&gt;.</returns>
        IPagination<User> GetSuperUsers(short deleted = 0, int pageIndex = 0, int pageSize = int.MaxValue);

        IPagination<User> GetNetworkUsers(short deleted = 0, int pageIndex = 0, int pageSize = int.MaxValue);


        /// <summary>
        /// Gets the users by role identifier.
        /// </summary>
        /// <param name="RoleId">The role identifier.</param>
        /// <returns>IList&lt;User&gt;.</returns>
        IList<User> GetUsersByRoleId(long RoleId);

        /// <summary>
        /// Get Users profile information
        /// </summary>
        /// <param name="RoleId">long</param>
        /// <returns></returns>
        IList<UserProfileResult> GetUsersWithRolesByRoleId(long RoleId);

        /// <summary>
        /// Gets the affiliate users.
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>IPagination&lt;User&gt;.</returns>
        IPagination<User> GetAffiliateUsers(long? affiliateId = null, short deleted = 0, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets the buyer users.
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>IPagination&lt;User&gt;.</returns>
        IPagination<User> GetBuyerUsers(long? buyerId = null, short deleted = 0, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets the users by affiliate identifier.
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>IPagination&lt;User&gt;.</returns>
        IPagination<User> GetUsersByAffiliateId(long affiliateId, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets the users by buyer identifier.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>IPagination&lt;User&gt;.</returns>
        IPagination<User> GetUsersByBuyerId(long buyerId, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets the user buyer channels.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;UserBuyerChannel&gt;.</returns>
        IList<UserBuyerChannel> GetUserBuyerChannels(long Id);

        /// <summary>
        /// Gets the buyer channel users.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;UserBuyerChannel&gt;.</returns>
        IList<UserBuyerChannel> GetBuyerChannelUsers(long Id);

        /// <summary>
        /// Attaches the buyer channel.
        /// </summary>
        /// <param name="userBuyerChannel">The user buyer channel.</param>
        void AttachBuyerChannel(UserBuyerChannel userBuyerChannel);

        /// <summary>
        /// Detaches the buyer channel.
        /// </summary>
        /// <param name="userBuyerChannel">The user buyer channel.</param>
        void DetachBuyerChannel(UserBuyerChannel userBuyerChannel);

        /// <summary>
        /// Get User Search
        /// </summary>
        /// <param name="id">User Identifier</param>
        /// <param name="username">User Name</param>
        /// <param name="email">Email</param>
        /// <returns>User Collection Item</returns>
        IList<User> GetUserSearch(long id, string username = null, string email = null);

        /// <summary>
        /// Gets all UserIds of Organization
        /// </summary>
        /// <returns>List of UserIds</returns>
        IList<long> GetOrganizationUserIds();

        /// <summary>
        /// Gets the users by type identifier.
        /// </summary>
        /// <param name="typeId">The type identifier.</param>
        /// <returns>IList&lt;User&gt;.</returns>
        IList<User> GetUsersByType(UserTypes userType);

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns>IList&lt;User&gt;.</returns>
        IList<User> GetUsers();

        /// <summary>
        /// Insert User
        /// </summary>
        /// <param name="user">User</param>
        void InsertUser(User user);

        /// <summary>
        /// Insert User Role
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        void AddUserRole(long roleId, long userId);

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="user">User</param>
        void UpdateUser(User user);

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="isPermanently">Is Permanently</param>
        void DeleteUser(User user, bool isPermanently = false);

        /// <summary>
        /// Restore User By Email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User Item</returns>
        User RestoreUserByEmail(string email);

        #endregion Methods

        #region User Type

        /// <summary>
        /// Get User Type By Id
        /// </summary>
        /// <param name="userTypeId">User Type Identifier</param>
        /// <returns>User Type Item</returns>
        UserType GetUserTypeById(long userTypeId);

        /// <summary>
        /// Get User Type By Name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>User Type Item</returns>
        UserType GetUserTypeByName(string name);

        /// <summary>
        /// Get All User Types
        /// </summary>
        /// <returns>User Type Collection Item</returns>
        IList<UserType> GetAllUserTypes();

        /// <summary>
        /// Insert User Type
        /// </summary>
        /// <param name="userType">User Type</param>
        void InsertUserType(UserType userType);

        /// <summary>
        /// Update User Type
        /// </summary>
        /// <param name="userType">User Type</param>
        void UpdateUserType(UserType userType);

        /// <summary>
        /// Delete User Type
        /// </summary>
        /// <param name="userType">User Type</param>
        /// <param name="isPermanently">Is Permanently</param>
        void DeleteUserType(UserType userType, bool isPermanently = false);

        #endregion User Type

        #region Verify Account

        /// <summary>
        /// Get Verify Account By Id
        /// </summary>
        /// <param name="verifyAccountId">Verify Account Identifier</param>
        /// <returns>Verify Account Item</returns>
        VerifyAccount GetVerifyAccountById(long verifyAccountId);

        /// <summary>
        /// Get Verify Account By User Id
        /// </summary>
        /// <param name="userId">User Identifier</param>
        /// <returns>Verify Account Item</returns>
        IList<VerifyAccount> GetVerifyAccountByUserId(long userId);

        /// <summary>
        /// Get All Verify Accounts
        /// </summary>
        /// <returns>Verify Account Collection Item</returns>
        IList<VerifyAccount> GetAllVerifyAccounts();

        /// <summary>
        /// Insert Verify Account
        /// </summary>
        /// <param name="verifyAccount">Verify Account</param>
        void InsertVerifyAccount(VerifyAccount verifyAccount);

        /// <summary>
        /// Update Verify Account
        /// </summary>
        /// <param name="verifyAccount">Verify Account</param>
        void UpdateVerifyAccount(VerifyAccount verifyAccount);

        /// <summary>
        /// Delete Verify Account
        /// </summary>
        /// <param name="verifyAccount">Verify Account</param>
        void DeleteVerifyAccount(VerifyAccount verifyAccount);

        #endregion Verify Account

        #region Verify Security

        /// <summary>
        /// Get Verify Security By Id
        /// </summary>
        /// <param name="verifySecurityId">Verify Security Identifier</param>
        /// <returns>Verify Security Item</returns>
        VerifySecurity GetVerifySecurityById(long verifySecurityId);

        /// <summary>
        /// Get Verify Security By User Id
        /// </summary>
        /// <param name="userId">User Identifier</param>
        /// <returns>Verify Security Item</returns>
        IList<VerifySecurity> GetVerifySecurityByUserId(long userId);

        /// <summary>
        /// Get All Verify Securitys
        /// </summary>
        /// <returns>Verify Security Collection Item</returns>
        IList<VerifySecurity> GetAllVerifySecuritys();

        /// <summary>
        /// Insert Verify Security
        /// </summary>
        /// <param name="verifySecurity">Verify Security</param>
        void InsertVerifySecurity(VerifySecurity verifySecurity);

        /// <summary>
        /// Update Verify Security
        /// </summary>
        /// <param name="verifySecurity">Verify Security</param>
        void UpdateVerifySecurity(VerifySecurity verifySecurity);

        /// <summary>
        /// Delete Verify Security
        /// </summary>
        /// <param name="verifySecurity">Verify Security</param>
        void DeleteVerifySecurity(VerifySecurity verifySecurity);

        #endregion Verify Security

        #region Entity ownership

        IList<long> GetUserEntityIds(long? userId, string entityName, Guid accountGuid);

        void InsertEntityOwnership(EntityOwnership entityOwnership);

        void DeleteEntityOwnership(long? userId, string entityName, Guid accountGuid);

        void DeleteEntityOwnership(long userId, string entityName, long entityId);

        void ClearEntityOwnership(long userId);

        bool IsEntityOwnershipApproved(long userId, string entityName, long entityId);



        List<EntityOwnership> GetEntityOwnership(string entityName, long entityId);
        #endregion

        #region Add User Extra Functions

        /// <summary>
        /// Validate Create Opposite User
        /// </summary>
        bool ValidateCreateOppositeUser(UserTypes currentUserType, UserTypes addedUserType);

        /// <summary>
        /// Validate User Access Section
        /// </summary>
        bool ValidateUserAccessSection(
            UserTypes userType,
            List<long> buyers,
            List<long> buyerChannels,
            List<long> affiliates,
            List<long> affiliateChannels);

        /// <summary>
        /// AddEntity Ownerships
        /// </summary>
        void AddEntityOwnerships(List<long> campaigns,
            List<long> buyers,
            List<long> buyerChannels,
            List<long> affiliates,
            List<long> affiliateChannels,
            long id);

        #endregion
    }
}