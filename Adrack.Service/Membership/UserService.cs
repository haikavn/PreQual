// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="UserService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Common;
using Adrack.Service.Helpers;
using Adrack.Service.Infrastructure.ApplicationEvent;
using Adrack.Service.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Adrack.Service.Membership
{
    /// <summary>
    /// Represents a User Service
    /// Implements the <see cref="Adrack.Service.Membership.IUserService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Membership.IUserService" />
    public partial class UserService : IUserService
    {
        #region Constants

        private const string CACHE_ENTITY_OWNERSHIP_IDS_BY_USER_AND_NAME = "App.Cache.Entity.Ownership.By.User.And.Name-{0}-{1}";

        private const string CACHE_ENTITY_OWNERSHIP_IsEntityOwnershipApproved = "App.Cache.Entity.Ownership.IsEntityOwnershipApproved-{0}-{1}-{2}";

        private const string CACHE_ENTITY_OWNERSHIP_PATTERN_KEY = "App.Cache.Entity.Ownership.";



        #endregion

        #region Fields

        /// <summary>
        /// User Setting
        /// </summary>
        private readonly UserSetting _userSetting;

        /// <summary>
        /// User
        /// </summary>
        private readonly IRepository<User> _userRepository;

        /// <summary>
        /// User Type
        /// </summary>
        private readonly IRepository<UserType> _userTypeRepository;

        /// <summary>
        /// Verify Account
        /// </summary>
        private readonly IRepository<VerifyAccount> _verifyAccountRepository;

        /// <summary>
        /// Verify Security
        /// </summary>
        private readonly IRepository<VerifySecurity> _verifySecurityRepository;

        /// <summary>
        /// The user buyer channel repository
        /// </summary>
        private readonly IRepository<UserBuyerChannel> _userBuyerChannelRepository;

        private readonly IRepository<EntityOwnership> _entityOwnershipRepository;


        /// <summary>
        /// Role Service
        /// </summary>
        private readonly IRoleService _roleService;

        /// <summary>
        /// Data Provider
        /// </summary>
        private readonly IDataProvider _dataProvider;

        /// <summary>
        /// Db Context
        /// </summary>
        //private readonly IDbContext _dbContext;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        private readonly IAppContext _appContext;

        private readonly long _organizationId = 0;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// User Service
        /// </summary>
        /// <param name="userSetting">User Setting</param>
        /// <param name="userRepository">User Repository</param>
        /// <param name="userTypeRepository">User Type Repository</param>
        /// <param name="verifyAccountRepository">Verify Account Repository</param>
        /// <param name="verifySecurityRepository">Verify AccountSecurity Repository</param>
        /// <param name="userBuyerChannelRepository">The user buyer channel repository.</param>
        /// <param name="roleService">Role Service</param>
        /// <param name="dataProvider">Data Provider</param>
        /// <param name="dbContext">Db Context</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="appContext"></param>
        public UserService(
            UserSetting userSetting,
            IRepository<User> userRepository,
            IRepository<UserType> userTypeRepository,
            IRepository<VerifyAccount> verifyAccountRepository,
            IRepository<VerifySecurity> verifySecurityRepository,
            IRepository<UserBuyerChannel> userBuyerChannelRepository,
            IRepository<EntityOwnership> entityOwnershipRepository,
            IRoleService roleService,
            IDataProvider dataProvider,
            //IDbContext dbContext,
            ICacheManager cacheManager,
            IAppEventPublisher appEventPublisher)
        {
            this._userSetting = userSetting;
            this._userRepository = userRepository;
            this._userTypeRepository = userTypeRepository;
            this._verifyAccountRepository = verifyAccountRepository;
            this._verifySecurityRepository = verifySecurityRepository;
            this._userBuyerChannelRepository = userBuyerChannelRepository;
            this._entityOwnershipRepository = entityOwnershipRepository;
            this._roleService = roleService;
            this._dataProvider = dataProvider;
            //this._dbContext = dbContext;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;


            //if (appContext.AppUser != null)
            //  this._organizationId = appContext.AppUser.OrganizationId;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="userId">User Identifier</param>
        /// <returns>User Item</returns>
        public virtual User GetUserById(long userId)
        {
            if (userId == 0)
                return null;

            return _userRepository.GetById(userId);
        }


        /// <summary>
        /// Gets the user by parent identifier.
        /// </summary>
        /// <param name="parentid">The parentid.</param>
        /// <param name="usertypeid">The usertypeid.</param>
        /// <returns>User.</returns>
        public virtual User GetUserByParentId(long parentid, UserTypes userType)
        {
            var query = from x in _userRepository.Table
                        where x.ParentId == parentid && x.UserType == userType
                        select x;

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Gets the users by parent identifier.
        /// </summary>
        /// <param name="parentid">The parentid.</param>
        /// <param name="usertypeid">The usertypeid.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;User&gt;.</returns>
        public virtual IList<User> GetUsersByParentId(long parentid, UserTypes userType, short deleted = 0)
        {
            var query = from x in _userRepository.Table
                        where x.ParentId == parentid && x.UserType == userType && (deleted == -1 || (deleted == 0 && !x.Deleted) || (deleted == 1 && x.Deleted))
                        select x;

            return query.ToList();
        }

        /// <summary>
        /// Get User By User GuId
        /// </summary>
        /// <param name="guId">Globally Unique Identifier</param>
        /// <returns>User Item</returns>
        public virtual User GetUserByUserGuId(string guId)
        {
            if (string.IsNullOrWhiteSpace(guId))
                return null;

            var query = from x in _userRepository.Table
                        orderby x.Id
                        where x.GuId == guId
                        select x;

            var user = query.FirstOrDefault();

            return user;
        }

        /// <summary>
        /// Get User By Username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User Item</returns>
        public virtual User GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var query = from x in _userRepository.Table
                        orderby x.Id
                        where x.Username == username || x.Email == username
                        select x;

            var user = query.FirstOrDefault();

            return user;
        }

        /// <summary>
        /// Get User By Email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User Item</returns>
        public virtual User GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var query = from x in _userRepository.Table
                        orderby x.Id
                        where x.Email == email && !x.Deleted
                        select x;

            var user = query.FirstOrDefault();

            return user;
        }

        /// <summary>
        /// Check Global Attribute
        /// </summary>
        /// <param name="key">Global Attribute's Key</param>
        /// <param name="value">Global Attribute's Value</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool CheckGlobalAttribute(string key, string value)
        {
            bool result = false;
            
            IGlobalAttributeService _globalAttributeService = AppEngineContext.Current.Resolve<IGlobalAttributeService>();
            
            var globalAttributes = _globalAttributeService.GetAllGlobalAttributes();
            foreach (var globalAttribute in globalAttributes)
            {
                if (globalAttribute.Key == key && globalAttribute.Value == value)
                {
                    _globalAttributeService.DeleteGlobalAttribute(globalAttribute);
                    result = true;
                }
            }

            return result;
        }



        /// <summary>
        /// Get User By Built In Name
        /// </summary>
        /// <param name="builtInName">Built In Name</param>
        /// <returns>User Item</returns>
        public virtual User GetUserByBuiltInName(string builtInName)
        {
            if (string.IsNullOrWhiteSpace(builtInName))
                return null;

            var query = from x in _userRepository.Table
                        orderby x.Id
                        where x.BuiltInName == builtInName
                        select x;

            var user = query.FirstOrDefault();

            return user;
        }

        /// <summary>
        /// Gets the user by remote login unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>User.</returns>
        public virtual User GetUserByRemoteLoginGuid(Guid guid)
        {
            var query = from x in _userRepository.Table
                        where x.RemoteLoginGuid == guid
                        select x;

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <param name="pageIndex">Page Index</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>User Collection Item</returns>
        public virtual IPagination<User> GetAllUsers(short deleted = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from x in _userRepository.Table
                        where (deleted == -1 || (deleted == 0 && !x.Deleted) || (deleted == 1 && x.Deleted))
                        orderby x.Username, x.LoginDate descending
                        select x;

            var users = new Pagination<User>(query, pageIndex, pageSize);

            return users;
        }

        /// <summary>
        /// Gets the system users.
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>IPagination&lt;User&gt;.</returns>
        public virtual IPagination<User> GetSuperUsers(short deleted = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from x in _userRepository.Table
                        where x.UserType == UserTypes.Super && (deleted == -1 || (deleted == 0 && !x.Deleted) || (deleted == 1 && x.Deleted))
                        orderby x.Username, x.LoginDate descending
                        select x;

            var users = new Pagination<User>(query, pageIndex, pageSize);

            return users;
        }

        public virtual IPagination<User> GetNetworkUsers(short deleted = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from x in _userRepository.Table
                        where x.UserType == UserTypes.Network && (deleted == -1 || (deleted == 0 && !x.Deleted) || (deleted == 1 && x.Deleted))
                        orderby x.Username, x.LoginDate descending
                        select x;

            var users = new Pagination<User>(query, pageIndex, pageSize);

            return users;
        }

        /// <summary>
        /// Gets the users by role identifier.
        /// </summary>
        /// <param name="RoleId">The role identifier.</param>
        /// <returns>IList&lt;User&gt;.</returns>
        public virtual IList<User> GetUsersByRoleId(long RoleId)
        {
            var RoleIdParam = _dataProvider.GetParameter();

            RoleIdParam.ParameterName = "RoleId";
            RoleIdParam.Value = RoleId;
            RoleIdParam.DbType = DbType.Int64;           

//            IList<User> UsersList = _dbContext.SqlQuery<User>("EXECUTE [dbo].[GetUsersByRoleId] @RoleId", RoleIdParam).ToList();
            IList<User> UsersList = _userRepository.GetDbClientContext().SqlQuery<User>("EXECUTE [dbo].[GetUsersByRoleId] @RoleId", RoleIdParam).ToList();

            return UsersList;
        }
        /// <summary>
        /// Get Users profile information
        /// </summary>
        /// <param name="RoleId">long</param>
        /// <returns></returns>

        public virtual IList<UserProfileResult> GetUsersWithRolesByRoleId(long RoleId)
        {
            var RoleIdParam = _dataProvider.GetParameter();

            RoleIdParam.ParameterName = "RoleId";
            RoleIdParam.Value = RoleId;
            RoleIdParam.DbType = DbType.Int64;

            //           return _dbContext.SqlQuery<UserProfileResult>("EXECUTE [dbo].[GetUsersWithRolesByRoleId] @RoleId", RoleIdParam).ToList();
            return _userRepository.GetDbClientContext().SqlQuery<UserProfileResult>("EXECUTE [dbo].[GetUsersWithRolesByRoleId] @RoleId", RoleIdParam).ToList();
        }


        /// <summary>
        /// Gets the affiliate users.
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>IPagination&lt;User&gt;.</returns>
        public virtual IPagination<User> GetAffiliateUsers(long? affiliateId = null, short deleted = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            List<long?> userIds = new List<long?>();

            if (affiliateId.HasValue)
            {
                userIds = (from x in _entityOwnershipRepository.Table
                           where x.EntityName.ToLower() == "affiliate" && (!affiliateId.HasValue || (affiliateId.HasValue && x.EntityId == affiliateId.Value))
                           select x.UserId).ToList();
            }

            var query = from x in _userRepository.Table
                        where
                        x.UserType == UserTypes.Affiliate &&
                        (deleted == -1 || (deleted == 0 && !x.Deleted) || (deleted == 1 && x.Deleted)) &&
                        ((affiliateId.HasValue && userIds.Contains(x.Id)) || !affiliateId.HasValue)
                        orderby x.Username, x.LoginDate descending
                        select x;

            var users = new Pagination<User>(query, pageIndex, pageSize);

            return users;
        }

        /// <summary>
        /// Gets the buyer users.
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>IPagination&lt;User&gt;.</returns>
        public virtual IPagination<User> GetBuyerUsers(long? buyerId = null, short deleted = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            List<long?> userIds = new List<long?>();

            if (buyerId.HasValue)
            {
                userIds = (from x in _entityOwnershipRepository.Table
                           where x.EntityName.ToLower() == "buyer" && (!buyerId.HasValue || (buyerId.HasValue && x.EntityId == buyerId.Value))
                           select x.UserId).ToList();
            }

            var query = from x in _userRepository.Table
                        where
                        x.UserType == UserTypes.Buyer &&
                        (deleted == -1 || (deleted == 0 && !x.Deleted) || (deleted == 1 && x.Deleted)) &&
                        ((buyerId.HasValue && userIds.Contains(x.Id)) || !buyerId.HasValue)
                        orderby x.Username, x.LoginDate descending
                        select x;

            var users = new Pagination<User>(query, pageIndex, pageSize);

            return users;
        }

        /// <summary>
        /// Gets the users by affiliate identifier.
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>IPagination&lt;User&gt;.</returns>
        public virtual IPagination<User> GetUsersByAffiliateId(long affiliateId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var userIds = (from x in _entityOwnershipRepository.Table
                           where x.EntityName.ToLower() == "affiliate" && x.EntityId == affiliateId
                           select x.UserId).ToList();

            var query = from x in _userRepository.Table
                        where userIds.Contains(x.Id) && x.UserType == UserTypes.Affiliate
                        orderby x.Username, x.LoginDate descending
                        select x;

            var users = new Pagination<User>(query, pageIndex, pageSize);

            return users;
        }

        /// <summary>
        /// Gets the users by buyer identifier.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>IPagination&lt;User&gt;.</returns>
        public virtual IPagination<User> GetUsersByBuyerId(long buyerId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var userIds = (from x in _entityOwnershipRepository.Table
                           where x.EntityName.ToLower() == "buyer" && x.EntityId == buyerId
                           select x.UserId).ToList();

            var query = from x in _userRepository.Table
                        where userIds.Contains(x.Id) && x.UserType == UserTypes.Buyer
                        orderby x.Username, x.LoginDate descending
                        select x;

            var users = new Pagination<User>(query, pageIndex, pageSize);

            return users;
        }

        /// <summary>
        /// Gets the user buyer channels.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;UserBuyerChannel&gt;.</returns>
        public virtual IList<UserBuyerChannel> GetUserBuyerChannels(long Id)
        {
            var query = from x in _userBuyerChannelRepository.Table
                        where x.UserId == Id
                        select x;

            return query.ToList();
        }

        /// <summary>
        /// Gets the buyer channel users.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;UserBuyerChannel&gt;.</returns>
        public virtual IList<UserBuyerChannel> GetBuyerChannelUsers(long Id)
        {
            var query = from x in _userBuyerChannelRepository.Table
                        where x.BuyerChannelId == Id
                        select x;

            return query.ToList();
        }

        /// <summary>
        /// Attaches the buyer channel.
        /// </summary>
        /// <param name="userBuyerChannel">The user buyer channel.</param>
        /// <exception cref="ArgumentNullException">userBuyerChannel</exception>
        public virtual void AttachBuyerChannel(UserBuyerChannel userBuyerChannel)
        {
            if (userBuyerChannel == null)
                throw new ArgumentNullException("userBuyerChannel");

            _userBuyerChannelRepository.Insert(userBuyerChannel);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(userBuyerChannel);
        }

        /// <summary>
        /// Detaches the buyer channel.
        /// </summary>
        /// <param name="userBuyerChannel">The user buyer channel.</param>
        public virtual void DetachBuyerChannel(UserBuyerChannel userBuyerChannel)
        {
            _userBuyerChannelRepository.Delete(userBuyerChannel);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(userBuyerChannel);
        }

        /// <summary>
        /// Get User Search
        /// </summary>
        /// <param name="id">User Identifier</param>
        /// <param name="username">User Name</param>
        /// <param name="email">Email</param>
        /// <returns>User Collection Item</returns>
        public virtual IList<User> GetUserSearch(long id, string username = null, string email = null)
        {
            var userId = _dataProvider.GetParameter();

            userId.ParameterName = "Id";
            userId.Value = id;
            userId.DbType = DbType.Int64;

            //var result = _dbContext.SqlQuery<User>("EXECUTE [dbo].[usp_membership_user_search] @Id", userId).ToList();
            var result = _userRepository.GetDbClientContext().SqlQuery<User>("EXECUTE [dbo].[usp_membership_user_search] @Id", userId).ToList();

            return result;
        }

        /// <summary>
        /// Gets all UserIds of Organization
        /// </summary>
        /// <returns>List of UserIds</returns>
        public IList<long> GetOrganizationUserIds()
        {
            //var query = _userRepository.Table.Where(x => x.OrganizationId == _organizationId).Select(y => y.OrganizationId);
            //return query.ToList();

            return null;
        }


        /// <summary>
        /// Get Users By Email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User Collection Item</returns>
        public virtual IList<User> GetUsersByEmail(string email = "")
        {
            var query = from x in _userRepository.Table
                        where (email == "" || (email != "" && x.Email.ToLower().StartsWith(email)) && !x.Deleted)
                        orderby x.Username, x.LoginDate descending
                        select x;

            return query.ToList();
        }

        /// <summary>
        /// Gets the users by type identifier.
        /// </summary>
        /// <param name="typeId">The type identifier.</param>
        /// <returns>IList&lt;User&gt;.</returns>
        public virtual IList<User> GetUsersByType(UserTypes userType)
        {
            var query = from x in _userRepository.Table
                        where x.UserType == userType
                        orderby x.Username, x.LoginDate descending
                        select x;

            return query.ToList();
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns>IList&lt;User&gt;.</returns>
        public virtual IList<User> GetUsers()
        {
            var query = from x in _userRepository.Table
                orderby x.Username, x.LoginDate descending
                select x;

            return query.ToList();
        }

        /// <summary>
        /// Insert User
        /// </summary>
        /// <param name="user">User</param>
        /// <exception cref="ArgumentNullException">user</exception>
        public virtual void InsertUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.SetCanTrackChanges(true);

            _userRepository.Insert(user);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(user);
        }

        /// <summary>
        /// Isert User Role
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        public virtual void AddUserRole(long roleId, long userId)
        {
            var roleIdParam = _dataProvider.GetParameter();
            roleIdParam.ParameterName = "roleid";
            roleIdParam.Value = roleId;
            roleIdParam.DbType = DbType.Int64;

            var userIdParam = _dataProvider.GetParameter();
            userIdParam.ParameterName = "userid";
            userIdParam.Value = userId;
            userIdParam.DbType = DbType.Int64;

            //_dbContext.SqlQuery<int>("EXECUTE [dbo].[AddUserRole] @roleid,@userid", roleIdParam, userIdParam).FirstOrDefault();
            _userRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[AddUserRole] @roleid,@userid", roleIdParam, userIdParam).FirstOrDefault();

        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="user">User</param>
        /// <exception cref="ArgumentNullException">user</exception>
        public virtual void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.SetCanTrackChanges(false);

            _userRepository.Update(user);
            _appEventPublisher.EntityUpdated(user);
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="isPermanently">Is Permanently</param>
        /// <exception cref="ArgumentNullException">user</exception>
        /// <exception cref="AppException">Buil-In user account ({0}) could not be deleted</exception>
        public virtual void DeleteUser(User user, bool isPermanently = false)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.SetCanTrackChanges(true);

            if (user.BuiltIn)
                throw new AppException("Buil-In user account ({0}) could not be deleted", user.BuiltInName);

            if (!isPermanently)
            {
                user.Active = false;
                user.Deleted = true;

                if (!String.IsNullOrEmpty(user.Email))
                    user.Email += "-DELETED";

                if (!String.IsNullOrEmpty(user.Username))
                    user.Username += "-DELETED";

                UpdateUser(user);
            }
            else
            {
                _userRepository.Delete(user);
                _cacheManager.ClearRemoteServersCache();
                _appEventPublisher.EntityDeleted(user);
            }
        }

        /// <summary>
        /// Restore User By Email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User Item</returns>
        public virtual User RestoreUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var query = from x in _userRepository.Table
                orderby x.Id
                where x.Email == email && x.Deleted
                select x;

            var user = query.FirstOrDefault();
            if (user == null)
                throw new ArgumentNullException("deleted user");

            var duplicateUser = (from x in _userRepository.Table
                orderby x.Id
                where x.Email == email && !x.Deleted
                select x).FirstOrDefault();

            if (duplicateUser != null)
                throw new ArgumentException("duplicate user");

            user.Active = true;//??
            user.Deleted = false;

            UpdateUser(user);

            return user;
        }

        #endregion Methods

        #region User Type

        /// <summary>
        /// Get User Type By Id
        /// </summary>
        /// <param name="userTypeId">User Type Identifier</param>
        /// <returns>User Type Item</returns>
        public virtual UserType GetUserTypeById(long userTypeId)
        {
            if (userTypeId == 0)
                return null;

            return _userTypeRepository.GetById(userTypeId);
        }

        /// <summary>
        /// Get User Type By Name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>User Type Item</returns>
        public virtual UserType GetUserTypeByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var query = from x in _userTypeRepository.Table
                        orderby x.Name
                        where x.Name == name
                        select x;

            var user = query.FirstOrDefault();

            return user;
        }

        /// <summary>
        /// Get All User Types
        /// </summary>
        /// <returns>User Type Collection Item</returns>
        public virtual IList<UserType> GetAllUserTypes()
        {
            var query = from x in _userTypeRepository.Table
                        orderby x.DisplayOrder, x.Id
                        select x;

            var userTypes = query.ToList();

            return userTypes;
        }

        /// <summary>
        /// Insert User Type
        /// </summary>
        /// <param name="userType">User Type</param>
        /// <exception cref="ArgumentNullException">userType</exception>
        public virtual void InsertUserType(UserType userType)
        {
            if (userType == null)
                throw new ArgumentNullException("userType");

            _userTypeRepository.Insert(userType);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(userType);
        }

        /// <summary>
        /// Update User Type
        /// </summary>
        /// <param name="userType">User Type</param>
        /// <exception cref="ArgumentNullException">userType</exception>
        public virtual void UpdateUserType(UserType userType)
        {
            if (userType == null)
                throw new ArgumentNullException("userType");

            _userTypeRepository.Update(userType);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(userType);
        }

        /// <summary>
        /// Delete User Type
        /// </summary>
        /// <param name="userType">User Type</param>
        /// <param name="isPermanently">Is Permanently</param>
        /// <exception cref="ArgumentNullException">userType</exception>
        public virtual void DeleteUserType(UserType userType, bool isPermanently = false)
        {
            if (userType == null)
                throw new ArgumentNullException("userType");

            if (!isPermanently)
            {
                userType.Published = false;
                userType.Deleted = true;

                if (!String.IsNullOrEmpty(userType.Name))
                    userType.Name += "-DELETED";

                UpdateUserType(userType);
            }
            else
            {
                _userTypeRepository.Delete(userType);
                _cacheManager.ClearRemoteServersCache();
                _appEventPublisher.EntityDeleted(userType);
            }
        }

        #endregion User Type

        #region Verify Account

        /// <summary>
        /// Get Verify Account By Id
        /// </summary>
        /// <param name="verifyAccountId">Verify Account Identifier</param>
        /// <returns>Verify Account Item</returns>
        public virtual VerifyAccount GetVerifyAccountById(long verifyAccountId)
        {
            if (verifyAccountId == 0)
                return null;

            return _verifyAccountRepository.GetById(verifyAccountId);
        }

        /// <summary>
        /// Get Verify Account By User Id
        /// </summary>
        /// <param name="userId">User Identifier</param>
        /// <returns>Verify Account Item</returns>
        public virtual IList<VerifyAccount> GetVerifyAccountByUserId(long userId)
        {
            if (userId == 0)
                return null;

            var query = from x in _verifyAccountRepository.Table
                        orderby x.UserId
                        where x.UserId == userId
                        select x;

            var verifyAccount = query.ToList();

            return verifyAccount;
        }

        /// <summary>
        /// Get All Verify Accounts
        /// </summary>
        /// <returns>Verify Account Collection Item</returns>
        public virtual IList<VerifyAccount> GetAllVerifyAccounts()
        {
            var query = from x in _verifyAccountRepository.Table
                        orderby x.Id
                        select x;

            var verifyAccounts = query.ToList();

            return verifyAccounts;
        }

        /// <summary>
        /// Insert Verify Account
        /// </summary>
        /// <param name="verifyAccount">Verify Account</param>
        /// <exception cref="ArgumentNullException">verifyAccount</exception>
        public virtual void InsertVerifyAccount(VerifyAccount verifyAccount)
        {
            if (verifyAccount == null)
                throw new ArgumentNullException("verifyAccount");

            _verifyAccountRepository.Insert(verifyAccount);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(verifyAccount);
        }

        /// <summary>
        /// Update Verify Account
        /// </summary>
        /// <param name="verifyAccount">Verify Account</param>
        /// <exception cref="ArgumentNullException">verifyAccount</exception>
        public virtual void UpdateVerifyAccount(VerifyAccount verifyAccount)
        {
            if (verifyAccount == null)
                throw new ArgumentNullException("verifyAccount");

            _verifyAccountRepository.Update(verifyAccount);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(verifyAccount);
        }

        /// <summary>
        /// Delete Verify Account
        /// </summary>
        /// <param name="verifyAccount">Verify Account</param>
        /// <exception cref="ArgumentNullException">verifyAccount</exception>
        public virtual void DeleteVerifyAccount(VerifyAccount verifyAccount)
        {
            if (verifyAccount == null)
                throw new ArgumentNullException("verifyAccount");

            _verifyAccountRepository.Delete(verifyAccount);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(verifyAccount);
        }

        #endregion Verify Account

        #region Verify Security

        /// <summary>
        /// Get Verify Security By Id
        /// </summary>
        /// <param name="verifySecurityId">Verify Security Identifier</param>
        /// <returns>Verify Security Item</returns>
        public virtual VerifySecurity GetVerifySecurityById(long verifySecurityId)
        {
            if (verifySecurityId == 0)
                return null;

            return _verifySecurityRepository.GetById(verifySecurityId);
        }

        /// <summary>
        /// Get Verify Security By User Id
        /// </summary>
        /// <param name="userId">User Identifier</param>
        /// <returns>Verify Security Item</returns>
        public virtual IList<VerifySecurity> GetVerifySecurityByUserId(long userId)
        {
            if (userId == 0)
                return null;

            var query = from x in _verifySecurityRepository.Table
                        orderby x.UserId
                        where x.UserId == userId
                        select x;

            var verifySecurity = query.ToList();

            return verifySecurity;
        }

        /// <summary>
        /// Get All Verify Securitys
        /// </summary>
        /// <returns>Verify Security Collection Item</returns>
        public virtual IList<VerifySecurity> GetAllVerifySecuritys()
        {
            var query = from x in _verifySecurityRepository.Table
                        orderby x.Id
                        select x;

            var verifySecuritys = query.ToList();

            return verifySecuritys;
        }

        /// <summary>
        /// Insert Verify Security
        /// </summary>
        /// <param name="verifySecurity">Verify Security</param>
        /// <exception cref="ArgumentNullException">verifySecurity</exception>
        public virtual void InsertVerifySecurity(VerifySecurity verifySecurity)
        {
            if (verifySecurity == null)
                throw new ArgumentNullException("verifySecurity");

            _verifySecurityRepository.Insert(verifySecurity);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(verifySecurity);
        }

        /// <summary>
        /// Update Verify Security
        /// </summary>
        /// <param name="verifySecurity">Verify Security</param>
        /// <exception cref="ArgumentNullException">verifySecurity</exception>
        public virtual void UpdateVerifySecurity(VerifySecurity verifySecurity)
        {
            if (verifySecurity == null)
                throw new ArgumentNullException("verifySecurity");

            _verifySecurityRepository.Update(verifySecurity);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(verifySecurity);
        }

        /// <summary>
        /// Delete Verify Security
        /// </summary>
        /// <param name="verifySecurity">Verify Security</param>
        /// <exception cref="ArgumentNullException">verifySecurity</exception>
        public virtual void DeleteVerifySecurity(VerifySecurity verifySecurity)
        {
            if (verifySecurity == null)
                throw new ArgumentNullException("verifySecurity");

            _verifySecurityRepository.Delete(verifySecurity);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(verifySecurity);
        }

        #endregion Verify Security

        #region Entity ownership

        public IList<long> GetUserEntityIds(long? userId, string entityName, Guid accountGuid)
        {
            string key = string.Format(CACHE_ENTITY_OWNERSHIP_IDS_BY_USER_AND_NAME, userId, entityName, accountGuid.ToString());

            return _cacheManager.Get(key, () => {
                return (from x in _entityOwnershipRepository.Table
                    where (!userId.HasValue || (userId.HasValue && x.UserId == userId)) && x.EntityName.ToLower() == entityName.ToLower() //&& x.AccountGuid == accountGuid
                        select x.EntityId).ToList();
            });
        }

        public void InsertEntityOwnership(EntityOwnership entityOwnership)
        {
            _entityOwnershipRepository.Insert(entityOwnership);
            
            _cacheManager.RemoveByPattern(CACHE_ENTITY_OWNERSHIP_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(entityOwnership);
        }

        public void DeleteEntityOwnership(long? userId, string entityName, Guid accountGuid)
        {
            var entityOwnership = (from x in _entityOwnershipRepository.Table
                where (!userId.HasValue || (userId.HasValue && x.UserId == userId)) &&
                      (string.IsNullOrEmpty(entityName) || (!string.IsNullOrEmpty(entityName) && x.EntityName.ToLower() == entityName.ToLower())) //&& x.AccountGuid == accountGuid
                select x).FirstOrDefault();
            if (entityOwnership != null)
            {
                _entityOwnershipRepository.Delete(entityOwnership);

                _cacheManager.RemoveByPattern(CACHE_ENTITY_OWNERSHIP_PATTERN_KEY);
                _cacheManager.ClearRemoteServersCache();
                _appEventPublisher.EntityDeleted(entityOwnership);
            }
        }

        public void DeleteEntityOwnership(long userId, string entityName, long entityId)
        {
            var entityOwnership = (from x in _entityOwnershipRepository.Table
                                   where x.UserId == userId &&
                                         x.EntityName.ToLower() == entityName.ToLower() &&
                                         x.EntityId == entityId //&& x.AccountGuid == accountGuid
                                   select x).FirstOrDefault();
            if (entityOwnership != null)
            {
                _entityOwnershipRepository.Delete(entityOwnership);

                _cacheManager.RemoveByPattern(CACHE_ENTITY_OWNERSHIP_PATTERN_KEY);
                _cacheManager.ClearRemoteServersCache();
                _appEventPublisher.EntityDeleted(entityOwnership);
            }
        }

        public void ClearEntityOwnership(long userId)
        {
            var startParam = _dataProvider.GetParameter();
            startParam.ParameterName = "userid";
            startParam.Value = userId;
            startParam.DbType = DbType.Int64;

            //_dbContext.SqlQuery<int>("EXECUTE [dbo].[ClearEntityOwnershipByUserId] @userId", startParam).FirstOrDefault();

            _userRepository.GetDbClientContext().SqlQuery<int>("EXECUTE [dbo].[ClearEntityOwnershipByUserId] @userId", startParam).FirstOrDefault();


            _cacheManager.RemoveByPattern(CACHE_ENTITY_OWNERSHIP_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
        }

        public bool IsEntityOwnershipApproved(long userId, string entityName, long entityId)
        {
            string key = string.Format(CACHE_ENTITY_OWNERSHIP_IsEntityOwnershipApproved, userId, entityName, entityId);

            var entityOwnership =  _cacheManager.Get(key, () => {
                return (from x in _entityOwnershipRepository.Table
                    where x.UserId == userId && x.EntityName.ToLower() == entityName.ToLower() && x.EntityId == entityId //&& x.AccountGuid == accountGuid
                    select x).FirstOrDefault();
            });

            if (entityOwnership == null) return false;

            return entityOwnership.IsApproved;
        }

        public List<EntityOwnership> GetEntityOwnership(string entityName, long entityId)
        {
            var entityOwnership = (from x in _entityOwnershipRepository.Table
                                   where (x.EntityName.ToLower() == entityName.ToLower() && x.EntityId == entityId)
                                   select x);

            return entityOwnership.ToList();
        }

        #endregion

        #region Add User Extra Functions
        /// <summary>
        /// Validate Create Opposite User
        /// </summary>
        public bool ValidateCreateOppositeUser(UserTypes currentUserType, UserTypes addedUserType)
        {
            if ((currentUserType == UserTypes.Affiliate && addedUserType == UserTypes.Buyer) ||
                (currentUserType == UserTypes.Buyer && addedUserType == UserTypes.Affiliate))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate User Access Section
        /// </summary>
        public bool ValidateUserAccessSection(
                    UserTypes userType,
                    List<long> buyers,
                    List<long> buyerChannels,
                    List<long> affiliates,
                    List<long> affiliateChannels)
        {
            if (userType == UserTypes.Affiliate)
            {
                if ((buyers != null && buyers.Count > 0) || (buyerChannels != null && buyerChannels.Count > 0))
                    return false;
            }
            else if (userType == UserTypes.Buyer)
            {
                if ((affiliates != null && affiliates.Count > 0) || (affiliateChannels != null && affiliateChannels.Count > 0))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// AddEntity Ownerships
        /// </summary>
        public void AddEntityOwnerships(List<long> campaigns,
                                List<long> buyers,
                                List<long> buyerChannels,
                                List<long> affiliates,
                                List<long> affiliateChannels,
                                long id)
        {
            if (campaigns.Any(x => x != 0))
            {
                foreach (var campaign in campaigns.Select(x => x).Distinct().ToList())
                {
                    var access = new EntityOwnership
                    {
                        Id = 0,
                        UserId = id,
                        EntityId = campaign,
                        EntityName = EntityType.Campaign.ToString()
                    };
                    InsertEntityOwnership(access);
                }
            }

            if (buyers.Any(x => x != 0))
            {
                foreach (var buyer in buyers.Select(x => x).Distinct().ToList())
                {
                    var access = new EntityOwnership
                    {
                        Id = 0,
                        UserId = id,
                        EntityId = buyer,
                        EntityName = EntityType.Buyer.ToString()
                    };
                    InsertEntityOwnership(access);
                }
            }

            if (buyerChannels.Any(x => x != 0))
            {
                foreach (var buyerChannel in buyerChannels.Select(x => x).Distinct().ToList())
                {
                    var access = new EntityOwnership
                    {
                        Id = 0,
                        UserId = id,
                        EntityId = buyerChannel,
                        EntityName = EntityType.BuyerChannel.ToString()
                    };
                    InsertEntityOwnership(access);
                }
            }

            if (affiliates.Any(x => x != 0))
            {
                foreach (var affiliate in affiliates.Select(x => x).Distinct().ToList())
                {
                    var access = new EntityOwnership
                    {
                        Id = 0,
                        UserId = id,
                        EntityId = affiliate,
                        EntityName = EntityType.Affiliate.ToString()
                    };
                    InsertEntityOwnership(access);
                }
            }

            if (affiliateChannels.Any(x => x != 0))
            {
                foreach (var affiliateChannel in affiliateChannels.Select(x => x).Distinct().ToList())
                {
                    var access = new EntityOwnership
                    {
                        Id = 0,
                        UserId = id,
                        EntityId = affiliateChannel,
                        EntityName = EntityType.AffiliateChannel.ToString()
                    };
                    InsertEntityOwnership(access);
                }
            }
        }


        #endregion


    }
}