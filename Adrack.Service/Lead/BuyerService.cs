// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="BuyerService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Helpers;
using Adrack.Service.Infrastructure.ApplicationEvent;
using Adrack.Service.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc.Html;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Class BuyerService.
    /// Implements the <see cref="Adrack.Service.Lead.IBuyerService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IBuyerService" />
    public partial class BuyerService : IBuyerService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_BUYER_BY_ID_KEY = "App.Cache.Buyer.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_BUYER_ALL_KEY = "App.Cache.Buyer.All";

        private const string CACHE_BUYER_GetBuyerByName = "App.Cache.Buyer.GetBuyerByName-{0}-{1}";

        private const string CACHE_BUYER_GetAllBuyers = "App.Cache.Buyer.GetBuyerByName-{0}";

        private const string CACHE_BUYER_GetAllBuyers2 = "App.Cache.Buyer.GetBuyerByName-{0}-{1}";

        private const string CACHE_BUYER_GetBuyerByAccountId = "App.Cache.Buyer.GetBuyerByAccountId-{0}";
        private const string CACHE_Buyer_GetAllBuyersByStatus = "App.Cache.Buyer.GetAllBuyersByStatus-{0}-{1}";
        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_BUYER_PATTERN_KEY = "App.Cache.Buyer.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<Buyer> _buyerRepository;
        private readonly IRepository<User> _userRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        private readonly IAppContext _appContext;

        private readonly IUserService _userService;

        private readonly IRepository<BuyerInvitation> _buyerInvitationRepository;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="buyerRepository">The buyer repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public BuyerService(IRepository<Buyer> buyerRepository,
            ICacheManager cacheManager,
            IAppEventPublisher appEventPublisher,
            IUserService userService,
            IRepository<BuyerInvitation> buyerInvitationRepository,
            IRepository<User> userRepository,
            IAppContext appContext)
        {
            this._buyerRepository = buyerRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._appContext = appContext;
            this._userService = userService;
            this._buyerInvitationRepository = buyerInvitationRepository;
            this._userRepository = userRepository;
        }

        #endregion Constructor

        #region Methods

        public IQueryable<Buyer> FilterAllowed(IQueryable<Buyer> query, User user = null)
        { 
            //return query;

            if (user == null)
                user = _appContext.AppUser;

            var userService = AppEngineContext.Current.Resolve<IUserService>();

            List<long> allowedBuyerIds = (List<long>)userService.GetUserEntityIds(user.Id, "buyer", Guid.Empty);

            if (user.UserType == SharedData.AffiliateUserTypeId)
            {
                query = query.Where(x => x.Id == -1);
            }
            else if (user.UserType == SharedData.BuyerUserTypeId)
            {
                query = query.Where(x => allowedBuyerIds.Contains(x.Id));
            }
            else if (user.UserType == SharedData.NetowrkUserTypeId)
            {
                query = query.Where(x => x.ManagerId == user.Id || allowedBuyerIds.Contains(x.Id));
            }

            return query;
        }

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual Buyer GetBuyerById(long buyerId, bool cached = false)
        {
            if (buyerId == 0)
                return null;

            if (!cached)
                return (from x in _buyerRepository.Table
                        where x.Id == buyerId &&
                              (!x.Deleted.HasValue || x.Deleted == false)
                        select x).FirstOrDefault();

            string key = string.Format(CACHE_BUYER_BY_ID_KEY, buyerId);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _buyerRepository.Table
                        where x.Id == buyerId &&
                              (!x.Deleted.HasValue || x.Deleted == false)
                        select x).FirstOrDefault();
            });
        }

        public virtual Buyer GetBuyerByIdWithDeletedStatus(long buyerId, bool considerDeleted = false)
        {
            if (buyerId == 0)
                return null;

           if(considerDeleted)
                return (from x in _buyerRepository.Table
                        where x.Id == buyerId
                        select x).FirstOrDefault();
           else
                return (from x in _buyerRepository.Table
                            where x.Id == buyerId &&
                                  (!x.Deleted.HasValue || x.Deleted == false)
                            select x).FirstOrDefault();

            
        }

        /// <summary>
        /// Gets the name of the buyer by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="exceptId">The except identifier.</param>
        /// <returns>Buyer.</returns>
        public virtual Buyer GetBuyerByName(string name, long exceptId)
        {
            string key = string.Format(CACHE_BUYER_GetBuyerByName, name, exceptId);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _buyerRepository.Table
                        where x.Name == name && x.Id != exceptId
                        select x).FirstOrDefault();
            });
        }

        /// <summary>
        /// Gets the buyer by account id.
        /// </summary>
        /// <param name="accountid">The account id.</param>
        /// <returns>Buyer.</returns>
        public virtual Buyer GetBuyerByAccountId(int accountId)
        {
            string key = string.Format(CACHE_BUYER_GetBuyerByAccountId, accountId);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _buyerRepository.Table
                        where x.AccountId == accountId
                        select x).FirstOrDefault();
            });
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<Buyer> GetAllBuyers(short deleted = 0)
        {
            string key = string.Format(CACHE_BUYER_GetAllBuyers, deleted);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _buyerRepository.Table
                            where (deleted == -1 || (deleted == 0 && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)) || (deleted == 1 && x.Deleted.HasValue && x.Deleted.Value))
                            orderby x.Id descending
                            select x;

                query = (IOrderedQueryable<Buyer>)FilterAllowed(query);

                return query.ToList();
            });
        }

        public virtual IList<Buyer> GetAllBuyersByStatus(EntityFilterByStatus status = EntityFilterByStatus.Active,
                                                 EntityFilterByDeleted deleted = EntityFilterByDeleted.NotDeleted)
        {
            var key = string.Format(CACHE_Buyer_GetAllBuyersByStatus, status, deleted);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _buyerRepository.Table
                            where
                            (deleted == EntityFilterByDeleted.All
                            || (deleted == EntityFilterByDeleted.NotDeleted && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue))
                            || (deleted == EntityFilterByDeleted.Deleted && x.Deleted.HasValue && x.Deleted.Value))
                            &&
                            (status == EntityFilterByStatus.All
                            || (status == EntityFilterByStatus.Active && x.Status == (short)EntityStatus.Active)
                            || (status == EntityFilterByStatus.Inactive && x.Status == (short)EntityStatus.Inactive)
                            )
                            orderby x.Id descending
                            select x;

                query = (IOrderedQueryable<Buyer>)FilterAllowed(query);

                return query.ToList();
            });
        }

        /// <summary>
        /// Gets all buyers.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;Buyer&gt;.</returns>
        public virtual IList<Buyer> GetAllBuyers(User user, short deleted = 0)
        {
            if (user == null)
                user = _appContext.AppUser;

            string key = string.Format(CACHE_BUYER_GetAllBuyers2, user.Id, deleted);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _buyerRepository.Table
                            where (deleted == -1 || (deleted == 0 && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue)) || (deleted == 1 && x.Deleted.HasValue && x.Deleted.Value))
                            orderby x.Id descending
                            select x;

                query = (IOrderedQueryable<Buyer>)FilterAllowed(query, user);

                return query.ToList();
            });
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="buyer">The buyer.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">buyer</exception>
        public virtual long InsertBuyer(Buyer buyer)
        {
            if (buyer == null)
                throw new ArgumentNullException("buyer");

            _buyerRepository.SetCanTrackChanges(true);

            _buyerRepository.Insert(buyer);

            _cacheManager.RemoveByPattern(CACHE_BUYER_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(buyer);

            return buyer.Id;
        }

        /// <summary>
        /// Update Buyer
        /// </summary>
        /// <param name="buyer">The buyer.</param>
        /// <exception cref="ArgumentNullException">buyer</exception>
        public virtual void UpdateBuyer(Buyer buyer)
        {
            if (buyer == null)
                throw new ArgumentNullException("buyer");

            _buyerRepository.SetCanTrackChanges(true);

            _buyerRepository.Update(buyer);

            _cacheManager.RemoveByPattern(CACHE_BUYER_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(buyer);
        }

        /// <summary>
        /// Update Buyers by List
        /// </summary>
        /// <param name="buyerList">The buyers list</param>
        /// <exception cref="ArgumentNullException">buyer</exception>
        public virtual void UpdateBuyerList(IEnumerable<Buyer> buyerList)
        {
            if (buyerList == null)
                throw new ArgumentNullException("buyer");

            _buyerRepository.SetCanTrackChanges(true);

            _buyerRepository.Update(buyerList);

            _cacheManager.RemoveByPattern(CACHE_BUYER_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="buyer">The buyer.</param>
        /// <exception cref="ArgumentNullException">buyer</exception>
        public virtual void DeleteBuyer(Buyer buyer)
        {
            if (buyer == null)
                throw new ArgumentNullException("buyer");

            _buyerRepository.SetCanTrackChanges(true);

            _buyerRepository.Delete(buyer);

            _cacheManager.RemoveByPattern(CACHE_BUYER_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(buyer);
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <returns>Profile Collection Item</returns>
        public virtual List<BuyerInvitation> GetBuyerInvitations(long buyerId)
        {
            if (buyerId == 0)
                throw new ArgumentNullException("BuyerId");
            var query = (from x in _buyerInvitationRepository.Table
                         where x.BuyerId == buyerId
                         orderby x.Id descending
                         select x).ToList();

            var acceptedUsers = _userService.GetEntityOwnership("buyerInvitation", buyerId);
            var acceptedUsersResult = new List<BuyerInvitation>();

            if (acceptedUsers != null && acceptedUsers.Any())
            {
                acceptedUsersResult = (from au in acceptedUsers
                                       join u in _userRepository.Table on au.UserId equals u.Id
                                       join inv in _buyerInvitationRepository.Table.Where(x => x.BuyerId == buyerId) on u.Email.ToLower() equals inv.RecipientEmail.ToLower()

                                       select new BuyerInvitation()
                                       {
                                           BuyerId = buyerId,
                                           Id = inv.Id,
                                           InvitationDate = inv.InvitationDate,
                                           RecipientEmail = u.Email,
                                           Role = inv.Role,
                                           Status = inv.Status
                                       }).ToList();
            }
            var acceptedUsersResultEmails = acceptedUsersResult.Select(x => x.RecipientEmail.ToLower());
            query = query.Where(q => !acceptedUsersResultEmails.Contains(q.RecipientEmail.ToLower())).ToList();

            query.AddRange(acceptedUsersResult);

            return query;
        }

        public BuyerInvitation GetBuyerInvitationByEmail(string email)
        {
            var query = from x in _buyerInvitationRepository.Table
                where x.RecipientEmail == email
                select x;

            return query.FirstOrDefault();
        }
        public List<BuyerInvitation> GetBuyerInvitationsByEmail(string email)
        {
            var query = from x in _buyerInvitationRepository.Table
                        where x.RecipientEmail == email
                        select x;

            return query.ToList();
        }

        public virtual List<BuyerInvitation> GetAllBuyerInvitations()
        {

            var query = (from x in _buyerInvitationRepository.Table
                select x).ToList();

            return query;
        }

        public virtual long InsertBuyerInvitation(BuyerInvitation buyerInvitation)
        {
            if (buyerInvitation == null)
                throw new ArgumentNullException("buyerInvitation");

            _buyerInvitationRepository.Insert(buyerInvitation);


            _appEventPublisher.EntityInserted(buyerInvitation);

            return buyerInvitation.Id;
        }


        /// Update Status
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">buyerInvitationEmail</exception>
        public virtual void UpdateInvitationStatus(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("buyerInvitationEmail");

            var invitation = (from x in _buyerInvitationRepository.Table
                where x.RecipientEmail == email
                select x).FirstOrDefault();

            if (invitation != null)
            {
                invitation.Status = BuyerInvitationStatuses.Accepted;
                _buyerInvitationRepository.Update(invitation);
            }
        }


        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="affiliateInvitation">The affiliate invitation.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">affiliate</exception>
        public virtual void UpdateBuyerInvitation(BuyerInvitation buyerInvitation)
        {
            if (buyerInvitation == null)
                throw new ArgumentNullException("buyerInvitation");

            var invitation = _buyerInvitationRepository.GetById(buyerInvitation.Id);
            var existingInvitationStatus = invitation.Status;
            if (invitation != null)
            {
                invitation.Status = buyerInvitation.Status;
                invitation.RecipientEmail = buyerInvitation.RecipientEmail;
                invitation.InvitationDate = DateTime.UtcNow;

                _buyerInvitationRepository.Update(invitation);

                if (existingInvitationStatus == BuyerInvitationStatuses.Pending
                    && buyerInvitation.Status == BuyerInvitationStatuses.Accepted)
                {
                    _userService.InsertEntityOwnership(new Core.Domain.Common.EntityOwnership()
                    {
                        Id = 0,
                        UserId = _userService.GetUserByEmail(invitation.RecipientEmail.Trim()).Id,
                        EntityId = invitation.BuyerId,
                        EntityName = "buyerInvitation"
                    });
                } //else 
                //if (existingInvitationStatus == (short)AffiliateInvitationStatus.Accepted
                //    && affiliateInvitation.Status != (short)AffiliateInvitationStatus.Accepted)
                //{
                //    var user = _userService.GetUserByEmail(invitation.RecipientEmail);
                //    if (user != null)
                //    {
                //        _userService.DeleteEntityOwnership(user.Id, "affiliate", new Guid());
                //    }
                //}
            }

            _appEventPublisher.EntityInserted(buyerInvitation);
        }

        public void DeleteBuyerInvitation(long invitatationId)
        {
            try
            {
                var invitation = _buyerInvitationRepository.GetById(invitatationId);
                if (invitation != null)
                {

                    _buyerInvitationRepository.Delete(invitation);

                    var user = _userService.GetUserByEmail(invitation.RecipientEmail);
                    if (user != null)
                    {
                        _userService.DeleteEntityOwnership(user.Id, EntityType.BuyerInvitation.ToString(), Guid.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Methods
    }
}