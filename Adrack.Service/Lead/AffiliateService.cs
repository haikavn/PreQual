// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AffiliateService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Common;
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

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Represents a Profile Service
    /// Implements the <see cref="Adrack.Service.Lead.IAffiliateService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IAffiliateService" />
    public partial class AffiliateService : IAffiliateService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_AFFILIATE_BY_ID_KEY = "App.Cache.Affiliate.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_AFFILIATE_ALL_KEY = "App.Cache.Affiliate.All";

        private const string CACHE_AFFILIATE_GetAffiliateByName = "App.Cache.Affiliate.GetAffiliateByName-{0}-{1}";

        private const string CACHE_AFFILIATE_GetAllAffiliates = "App.Cache.Affiliate.GetAllAffiliates-{0}";

        private const string CACHE_AFFILIATE_GetAllAffiliatesByStatus = "App.Cache.Affiliate.GetAllAffiliates-{0}-{1}-{2}";

        private const string CACHE_AFFILIATE_GetAffiliatesByUser = "App.Cache.Affiliate.GetAllAffiliates-{0}-{1}-{2}";

        private const string CACHE_AFFILIATE_GetAllAffiliates2 = "App.Cache.Affiliate.GetAllAffiliates-{0}-{1}";

        private const string CACHE_AFFILIATE_GetAffiliateInvitations = "App.Cache.Affiliate.GetAffiliateInvitations-{0}-{1}";

        private const string CACHE_AFFILIATE_GetAffiliateInvitation = "App.Cache.Affiliate.GetAffiliateInvitation-{0}-{1}";

        private const string CACHE_AFFILIATE_GetAffiliateInvitationById = "App.Cache.Affiliate.GetAffiliateInvitationById-{0}";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_AFFILIATE_PATTERN_KEY = "App.Cache.Affiliate.";
        private const string CACHE_AFFILIATE_INVITATION_PATTERN_KEY = "App.Cache.AffiliateInvitation.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<Affiliate> _affiliateRepository;


        private readonly IRepository<User> _userRepository;

        /// <summary>
        /// AffiliateChannel
        /// </summary>
        private readonly IRepository<AffiliateChannel> _affiliateChannelRepository;

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<AffiliateInvitation> _affiliateInvitationRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        private readonly IAppContext _appContext;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="affiliateRepository">The affiliate repository.</param>
        /// <param name="affiliateInvitationRepository">The affiliate invitation repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userService">The user service.</param>
        public AffiliateService(
            IRepository<Affiliate> affiliateRepository,
            IRepository<AffiliateChannel> affiliateChannelRepository,
            IRepository<AffiliateInvitation> affiliateInvitationRepository,
            IRepository<User> userRepository,
            ICacheManager cacheManager, 
            IAppEventPublisher appEventPublisher, 
            IUserService userService,
            IAppContext appContext)
        {
            this._affiliateRepository = affiliateRepository;
            this._affiliateChannelRepository = affiliateChannelRepository;
            this._affiliateInvitationRepository = affiliateInvitationRepository;
            this._userRepository = userRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._userService = userService;
            this._appContext = appContext;
        }

        #endregion Constructor

        #region Methods



        public AffiliateInvitation GetAffiliateInvitation(long affiliateId, string email)
        {
            string key = string.Format(CACHE_AFFILIATE_GetAffiliateInvitation, affiliateId, email);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _affiliateInvitationRepository.Table
                            where x.AffiliateId == affiliateId && x.RecipientEmail == email
                            orderby x.Id descending
                            select x;

                return query.FirstOrDefault();
            });
        }


        public AffiliateInvitation GetAffiliateInvitationByEmail(string email)
        {
            var query = from x in _affiliateInvitationRepository.Table
                where x.RecipientEmail == email
                select x;

            return query.FirstOrDefault();
        }

        public List<AffiliateInvitation> GetAffiliateInvitationsByEmail(string email)
        {
            var query = from x in _affiliateInvitationRepository.Table
                        where x.RecipientEmail == email
                        select x;

            return query.ToList();
        }


        public IQueryable<Affiliate> FilterAllowed(IQueryable<Affiliate> query, User user = null)
        {
            //eturn query;

            if (user == null)
                user = _appContext.AppUser;

            List<long> allowedAffiliateIds = (List<long>)_userService.GetUserEntityIds(user.Id, "affiliate", Guid.Empty);

            if (user.UserType == SharedData.AffiliateUserTypeId)
            {
                query = query.Where(x => allowedAffiliateIds.Contains(x.Id));
            }
            else if (user.UserType == SharedData.BuyerUserTypeId)
            {
                //var affiliateChannelService = AppEngineContext.Current.Resolve<IAffiliateChannelService>();
                //var affiliateIds = affiliateChannelService.GetAllAffiliateChannels().Select(x => x.AffiliateId).ToList();
                //query = query.Where(x => affiliateIds.Contains(x.Id) && allowedAffiliateIds.Contains(x.Id));
                query = query.Where(x => x.Id == -1);
            }
            else if (user.UserType == SharedData.NetowrkUserTypeId)
            {
                query = query.Where(x => x.ManagerId == user.Id || allowedAffiliateIds.Contains(x.Id));
            }

            return query;
        }

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual Affiliate GetAffiliateById(long affiliateId, bool cache)
        {
            if (affiliateId == 0)
                return null;

            if (!cache)
                return _affiliateRepository.GetById(affiliateId);

            string key = string.Format(CACHE_AFFILIATE_BY_ID_KEY, affiliateId);

            return _cacheManager.Get(key, () => { return _affiliateRepository.GetById(affiliateId); });
        }

        /// <summary>
        /// Gets the name of the affiliate by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="exceptId">The except identifier.</param>
        /// <returns>Affiliate.</returns>
        public virtual Affiliate GetAffiliateByName(string name, long exceptId)
        {
            string key = string.Format(CACHE_AFFILIATE_GetAffiliateByName, name, exceptId);

            return _cacheManager.Get(key, () =>
            {
                return (from x in _affiliateRepository.Table
                        where x.Name == name && x.Id != exceptId
                        select x).FirstOrDefault();
            });
        }


        /// <summary>
        /// Check Affiliate Name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Affiliate.</returns>
        public virtual Affiliate CheckAffiliateName(string name)
        {
            return (from x in _affiliateRepository.Table
                where x.Name == name 
                select x).FirstOrDefault();
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<Affiliate> GetAllAffiliates(short deleted = 0)
        {
            string key = string.Format(CACHE_AFFILIATE_GetAllAffiliates, deleted);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _affiliateRepository.Table
                            where 
                            (deleted == -1 || (deleted == 0 && ((x.IsDeleted.HasValue && !x.IsDeleted.Value) || !x.IsDeleted.HasValue)) ||
                            (deleted == 1 && x.IsDeleted.HasValue && x.IsDeleted.Value))
                            orderby x.Id descending
                            select x;

                query = (IOrderedQueryable<Affiliate>)FilterAllowed(query);

                return query.ToList();
            });
        }

        /// <summary>
        /// Gets all affiliates.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="deleted">The deleted.</param>
        /// <param name="status">The status.</param>
        /// <returns>IList&lt;Affiliate&gt;.</returns>
        public virtual IList<Affiliate> GetAllAffiliates(string name
            , EntityFilterByStatus status = EntityFilterByStatus.Active
            , EntityFilterByDeleted deleted = EntityFilterByDeleted.NotDeleted)
        {
            string key = string.Format(CACHE_AFFILIATE_GetAllAffiliatesByStatus,name,status, deleted);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _affiliateRepository.Table
                            //join ac in _affiliateChannelRepository.Table.Where(x=>x.Status == (short)EntityFilterByStatus.Active && x.Deleted.HasValue && x.Deleted.Value == false) on x.Id equals ac.AffiliateId
                            //join allChannel in _affiliateChannelRepository.Table.Where(x => x.Deleted.HasValue && x.Deleted.Value == false) on x.Id  equals allChannel.AffiliateId
                            where 
                            (deleted == EntityFilterByDeleted.All 
                            || (deleted == EntityFilterByDeleted.NotDeleted && ((x.IsDeleted.HasValue && !x.IsDeleted.Value) || !x.IsDeleted.HasValue))
                            || (deleted == EntityFilterByDeleted.Deleted && x.IsDeleted.HasValue && x.IsDeleted.Value))
                            && 
                            (status == EntityFilterByStatus.All
                            || (status==EntityFilterByStatus.Active && x.Status==(short)EntityStatus.Active)
                            || (status == EntityFilterByStatus.Inactive && x.Status==(short)EntityStatus.Inactive)
                            )
                            &&
                            (name == null || name == string.Empty
                            || (!(name == null || name == string.Empty) && x.Name.ToLower().Contains(name))
                            )
                            orderby x.Id descending
                            select x;

                query = (IOrderedQueryable<Affiliate>)FilterAllowed(query);

                return query.ToList();
            });
        }

        /// <summary>
        /// Gets all affiliates.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;Affiliate&gt;.</returns>
        public virtual IList<Affiliate> GetAllAffiliates(User user, short deleted = 0)
        {
            if (user == null)
                user = _appContext.AppUser;

            string key = string.Format(CACHE_AFFILIATE_GetAllAffiliates2, user.Id, deleted);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _affiliateRepository.Table
                            where 
                            (deleted == -1 || (deleted == 0 && ((x.IsDeleted.HasValue && !x.IsDeleted.Value) || !x.IsDeleted.HasValue)) || 
                            (deleted == 1 && x.IsDeleted.HasValue && x.IsDeleted.Value))
                            orderby x.Id descending
                            select x;

                query = (IOrderedQueryable<Affiliate>)FilterAllowed(query, user);

                return query.ToList();
            });
        }







        /// <summary>
        /// Get Affiliates By User.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="status">The status.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;Affiliate&gt;.</returns>
        public virtual IList<Affiliate> GetAffiliatesByUser(string name
            , EntityFilterByStatus status = EntityFilterByStatus.Active
            , EntityFilterByDeleted deleted = EntityFilterByDeleted.NotDeleted)
        {
            string key = string.Format(CACHE_AFFILIATE_GetAffiliatesByUser, name, status, deleted);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _affiliateRepository.Table
                            join u in _userRepository.Table on x.ManagerId equals u.Id
                            where x.UserId == _appContext.AppUser.Id &&
                            (deleted == EntityFilterByDeleted.All
                            || (deleted == EntityFilterByDeleted.NotDeleted && ((x.Deleted.HasValue && !x.Deleted.Value) || !x.Deleted.HasValue))
                            || (deleted == EntityFilterByDeleted.Deleted && x.Deleted.HasValue && x.Deleted.Value))
                            &&
                            (status == EntityFilterByStatus.All
                            || (status == EntityFilterByStatus.Active && x.Status == (short)EntityStatus.Active)
                            || (status == EntityFilterByStatus.Inactive && x.Status == (short)EntityStatus.Inactive)
                            )
                            &&
                            (name == null || name == string.Empty
                            || (!(name == null || name == string.Empty) && x.Name.ToLower().Contains(name))
                            )
                            orderby x.Id descending
                            select x;

                query = (IOrderedQueryable<Affiliate>)FilterAllowed(query);

                return query.ToList();
            });
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="affiliate">The affiliate.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">affiliate</exception>
        public virtual long InsertAffiliate(Affiliate affiliate)
        {
            if (affiliate == null)
                throw new ArgumentNullException("affiliate");

            _affiliateRepository.SetCanTrackChanges(true);

            _affiliateRepository.Insert(affiliate);

            _cacheManager.RemoveByPattern(CACHE_AFFILIATE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(affiliate);

            return affiliate.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="affiliate">The affiliate.</param>
        /// <exception cref="ArgumentNullException">affiliate</exception>
        public virtual void UpdateAffiliate(Affiliate affiliate)
        {
            if (affiliate == null)
                throw new ArgumentNullException("affiliate");

            _affiliateRepository.SetCanTrackChanges(true);

            _affiliateRepository.Update(affiliate);
            _cacheManager.ClearRemoteServersCache();
            _cacheManager.RemoveByPattern(CACHE_AFFILIATE_PATTERN_KEY);

            _appEventPublisher.EntityUpdated(affiliate);
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="affiliateInvitation">The affiliate invitation.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">affiliate</exception>
        public virtual long InsertAffiliateInvitation(AffiliateInvitation affiliateInvitation)
        {
            if (affiliateInvitation == null)
                throw new ArgumentNullException("affiliateInvitation");

            _affiliateInvitationRepository.Insert(affiliateInvitation);

            _cacheManager.RemoveByPattern(CACHE_AFFILIATE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(affiliateInvitation);

            return affiliateInvitation.Id;
        }

        public void DeleteAffiliateInvitation(long invitatationId)
        {
            try
            {
                var invitation = _affiliateInvitationRepository.GetById(invitatationId);
                if (invitation != null)
                {
                    
                    _affiliateInvitationRepository.Delete(invitation);

                    var user = _userService.GetUserByEmail(invitation.RecipientEmail);
                    if (user != null)
                    {
                        _userService.DeleteEntityOwnership(user.Id, EntityType.AffiliateInvitation.ToString(), Guid.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Update Status
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">affiliateInvitationEmail</exception>
        public virtual void UpdateInvitationStatus(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("affiliateInvitationEmail");

            var invitation = (from x in _affiliateInvitationRepository.Table
                where x.RecipientEmail == email
                select x).FirstOrDefault();

            if (invitation != null)
            {
                invitation.Status = AffiliateInvitationStatuses.Accepted;
                _affiliateInvitationRepository.Update(invitation);
            }
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="affiliateInvitation">The affiliate invitation.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">affiliate</exception>
        public virtual void UpdateAffiliateInvitation(AffiliateInvitation affiliateInvitation)
        {
            if (affiliateInvitation == null)
                throw new ArgumentNullException("affiliateInvitation");
            
            var invitation = _affiliateInvitationRepository.GetById(affiliateInvitation.Id);
            var existingInvitationStatus = invitation.Status;
            if (invitation != null)
            {
                invitation.Status = affiliateInvitation.Status;
                invitation.RecipientEmail = affiliateInvitation.RecipientEmail;
                invitation.InvitationDate = DateTime.UtcNow;
                _affiliateInvitationRepository.Update(invitation);

                if (existingInvitationStatus == AffiliateInvitationStatuses.Pending
                    && affiliateInvitation.Status == AffiliateInvitationStatuses.Accepted)
                {
                    _userService.InsertEntityOwnership(new Core.Domain.Common.EntityOwnership()
                    {
                        Id = 0,
                        UserId = _userService.GetUserByEmail(invitation.RecipientEmail.Trim()).Id,
                        EntityId = invitation.AffiliateId,
                        EntityName = "affiliateInvitation"
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

            _cacheManager.RemoveByPattern(CACHE_AFFILIATE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(affiliateInvitation);
        }

        
        public virtual List<AffiliateInvitation> GetAffiliateInvitations(long affiliateId)
        {
            if (affiliateId == 0)
                throw new ArgumentNullException("AffiliateId");
            var query = (from x in _affiliateInvitationRepository.Table
                        where x.AffiliateId == affiliateId
                        orderby x.Id descending
                        select x).ToList();

            var acceptedUsers = _userService.GetEntityOwnership("affiliateInvitation", affiliateId);
            var acceptedUsersResult = new List<AffiliateInvitation>();

            if (acceptedUsers != null && acceptedUsers.Any())
            {
                 acceptedUsersResult = (from au in acceptedUsers
                                          join u in _userRepository.Table on au.UserId equals u.Id
                                          join inv in _affiliateInvitationRepository.Table.Where(x => x.AffiliateId == affiliateId) on u.Email.ToLower() equals inv.RecipientEmail.ToLower()

                                          select new AffiliateInvitation()
                                          {
                                              AffiliateId = affiliateId,
                                              Id = inv.Id,
                                              InvitationDate = inv.InvitationDate,
                                              RecipientEmail = u.Email,
                                              Role=inv.Role,
                                              Status = inv.Status
                                          }).ToList();
            }
            var acceptedUsersResultEmails = acceptedUsersResult.Select(x => x.RecipientEmail.ToLower());
            query = query.Where(q => !acceptedUsersResultEmails.Contains(q.RecipientEmail.ToLower())).ToList();

            query.AddRange(acceptedUsersResult);

            return query;
        }


        public virtual List<AffiliateInvitation> GetAllAffiliateInvitations()
        {

            var query = (from x in _affiliateInvitationRepository.Table
                select x).ToList();

            return query;
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="affiliate">The affiliate.</param>
        /// <exception cref="ArgumentNullException">affiliate</exception>
        public void DeleteAffiliate(Affiliate affiliate)
        {
            if (affiliate == null)
                throw new ArgumentNullException("affiliate");

            _affiliateRepository.SetCanTrackChanges(true);

            _affiliateRepository.Delete(affiliate);

            _cacheManager.RemoveByPattern(CACHE_AFFILIATE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(affiliate);
        }

        /// <summary>
        /// GetAffiliatesStatusCounts
        /// </summary>
        /// <returns>IList&lt;StatusCountsClass&gt;.</returns>
        /// <return> List KeyValuePair </return>
        public virtual IList<StatusCountsClass> GetAffiliatesStatusCounts()
        {
            List<StatusCountsClass> res = _affiliateRepository.GetDbClientContext().SqlQuery<StatusCountsClass>("EXECUTE [dbo].[GetAffiliatesStatusCounts]").ToList();

            return res;
        }

        #endregion Methods
    }
}