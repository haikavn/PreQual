// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 02-06-2022
// ***********************************************************************
// <copyright file="AccountingService.cs" company="Adrack.com">
//     Copyright © 2022
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Audit;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Adrack.Service.Accounting
{
    /// <summary>
    /// Represents a Profile Service
    /// Implements the <see cref="Adrack.Service.Accounting.IAccountingService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Accounting.IAccountingService" />
    public partial class AccountingService : IAccountingService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_PROFILE_BY_ID_KEY = "App.Cache.Accounting.By.Id-{0}";

        /// <summary>
        /// The cache accounting getbuyerinvoices
        /// </summary>
        private const string CACHE_ACCOUNTING_GETBUYERINVOICES = "App.Cache.Accounting.GetBuyerInvoices-{0}-{1}-{2}-{3}";

        /// <summary>
        /// The cache accounting getallbuyerinvoices
        /// </summary>
        private const string CACHE_ACCOUNTING_GETALLBUYERINVOICES = "App.Cache.Accounting.GeAllBuyerInvoices-{0}-{1}";

        /// <summary>
        /// The cache accounting getbuyerinvoicebyid
        /// </summary>
        private const string CACHE_ACCOUNTING_GETBUYERINVOICEBYID = "App.Cache.Accounting.GetBuyerInvoiceById-{0}";

        /// <summary>
        /// The cache accounting getallbuyerpayments
        /// </summary>
        private const string CACHE_ACCOUNTING_GETALLBUYERPAYMENTS = "App.Cache.Accounting.GetAllBuyerPayments";

        /// <summary>
        /// The cache accounting getallbuyerpaymentsbybuyer
        /// </summary>
        private const string CACHE_ACCOUNTING_GETALLBUYERPAYMENTSBYBUYER = "App.Cache.Accounting.GetAllBuyerPaymentsByBuyer-{0}-{1}-{2}";

        /// <summary>
        /// The cache accounting getaffiliateinvoices
        /// </summary>
        private const string CACHE_ACCOUNTING_GETAFFILIATEINVOICES = "App.Cache.Accounting.GetAffiliateInvoices-{0}-{1}-{2}-{3}";

        /// <summary>
        /// The cache accounting getallaffiliateinvoices
        /// </summary>
        private const string CACHE_ACCOUNTING_GETALLAFFILIATEINVOICES = "App.Cache.Accounting.GeAllAffiliateInvoices-{0}-{1}";

        /// <summary>
        /// The cache accounting getaffiliateinvoicebyid
        /// </summary>
        private const string CACHE_ACCOUNTING_GETAFFILIATEINVOICEBYID = "App.Cache.Accounting.GetAffiliateInvoiceById-{0}";

        /// <summary>
        /// The cache accounting getallaffiliatepayments
        /// </summary>
        private const string CACHE_ACCOUNTING_GETALLAFFILIATEPAYMENTS = "App.Cache.Accounting.GetAllAffiliatePayments";

        /// <summary>
        /// The cache accounting getallrefundedleads
        /// </summary>
        private const string CACHE_ACCOUNTING_GETALLREFUNDEDLEADS = "App.Cache.Accounting.GetAllRefundedLeads";

        /// <summary>
        /// The cache accounting getallrefundedleadsbyid
        /// </summary>
        private const string CACHE_ACCOUNTING_GETALLREFUNDEDLEADSBYID = "App.Cache.Accounting.GetAllRefundedLeads-{0}";

        /// <summary>
        /// The cache accounting getaffiliaterefundedleadsbyid
        /// </summary>
        private const string CACHE_ACCOUNTING_GETAFFILIATEREFUNDEDLEADSBYID = "App.Cache.Accounting.GetAffiliateRefundedLeadsById-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_PROFILE_ALL_KEY = "App.Cache.Accounting.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_PROFILE_PATTERN_KEY = "App.Cache.Accounting.";

        #endregion Constants

        #region Fields

        //Arman Potential Bug
        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// Profile
        /// </summary>

        private readonly IRepository<BuyerInvoice> _BuyerInvoiceRepository;

        /// <summary>
        /// The affiliate invoice repository
        /// </summary>
        private readonly IRepository<AffiliateInvoice> _AffiliateInvoiceRepository;

        /// <summary>
        /// The affiliate invoice repository
        /// </summary>
        private readonly IRepository<CustomInvoice> _CustomInvoiceRepository;

        /// <summary>
        /// The affiliate invoice repository
        /// </summary>
        private readonly IRepository<CustomInvoiceRow> _CustomInvoiceRowRepository;

        /// <summary>
        /// The affiliate payment repository
        /// </summary>
        private readonly IRepository<AffiliatePayment> _AffiliatePaymentRepository;

        /// <summary>
        /// The buyer payment repository
        /// </summary>
        private readonly IRepository<BuyerPayment> _BuyerPaymentRepository;

        /// <summary>
        /// The refunded leads repository
        /// </summary>
        private readonly IRepository<RefundedLeads> _RefundedLeadsRepository;

        /// <summary>
        /// The buyer banace repository
        /// </summary>
        private readonly IRepository<BuyerBalance> _BuyerBanaceRepository;

        /// <summary>
        /// The buyer invoice adjustment repository
        /// </summary>
        private readonly IRepository<BuyerInvoiceAdjustment> _BuyerInvoiceAdjustmentRepository;

        /// <summary>
        /// The affiliate invoice adjustment repository
        /// </summary>
        private readonly IRepository<AffiliateInvoiceAdjustment> _AffiliateInvoiceAdjustmentRepository;

        /// <summary>
        /// The data provider
        /// </summary>
        private readonly IDataProvider _dataProvider;

        /// <summary>
        /// The buyer repository
        /// </summary>
        private readonly IRepository<Buyer> _BuyerRepository;

        /// <summary>
        /// The affiliate repository
        /// </summary>
        private readonly IRepository<Affiliate> _AffiliateRepository;

        /// <summary>
        /// The history repository
        /// </summary>
        private readonly IHistoryService _historyRepository;

        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;


        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly ILogService _logService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="buyerInvoiceRepository">The buyer invoice repository.</param>
        /// <param name="affiliateInvoiceRepository">The affiliate invoice repository.</param>
        /// <param name="affiliatePaymentRepository">The affiliate payment repository.</param>
        /// <param name="buyerPaymentRepository">The buyer payment repository.</param>
        /// <param name="refundedLeadsRepository">The refunded leads repository.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="buyerBanaceRepository">The buyer banace repository.</param>
        /// <param name="buyerInvoiceAdjustmentRepository">The buyer invoice adjustment repository.</param>
        /// <param name="affiliateInvoiceAdjustmentRepository">The affiliate invoice adjustment repository.</param>
        /// <param name="buyerRepository">The buyer repository.</param>
        /// <param name="affiliateRepository">The affiliate repository.</param>
        /// <param name="historyRepository">The history repository.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="appContext">The application context.</param>
        /// <param name="logService">Error logging service.</param>
        public AccountingService(
                                    IRepository<BuyerInvoice> buyerInvoiceRepository,
                                    IRepository<AffiliateInvoice> affiliateInvoiceRepository,
                                    IRepository<CustomInvoice> customInvoiceRepository,
                                    IRepository<CustomInvoiceRow> customInvoiceRowRepository,
                                    IRepository<AffiliatePayment> affiliatePaymentRepository,
                                    IRepository<BuyerPayment> buyerPaymentRepository,
                                    IRepository<RefundedLeads> refundedLeadsRepository,
                                    IAppContext appContext,
                                    ICacheManager cacheManager,
                                    IAppEventPublisher appEventPublisher,
                                    IDataProvider dataProvider,
                                    IRepository<BuyerBalance> buyerBanaceRepository,
                                    IRepository<BuyerInvoiceAdjustment> buyerInvoiceAdjustmentRepository,
                                    IRepository<AffiliateInvoiceAdjustment> affiliateInvoiceAdjustmentRepository,
                                    IRepository<Buyer> buyerRepository,
                                    IRepository<Affiliate> affiliateRepository,
                                    IHistoryService historyRepository,
                                    ISettingService settingService,
                                    ILogService logService)
        {
            this._BuyerInvoiceRepository = buyerInvoiceRepository;
            this._AffiliateInvoiceRepository = affiliateInvoiceRepository;
            this._AffiliatePaymentRepository = affiliatePaymentRepository;
            this._BuyerPaymentRepository = buyerPaymentRepository;
            this._RefundedLeadsRepository = refundedLeadsRepository;
            this._BuyerBanaceRepository = buyerBanaceRepository;
            this._CustomInvoiceRepository = customInvoiceRepository;
            this._CustomInvoiceRowRepository = customInvoiceRowRepository;

            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._appContext = appContext;

            this._dataProvider = dataProvider;
            this._BuyerInvoiceAdjustmentRepository = buyerInvoiceAdjustmentRepository;
            this._AffiliateInvoiceAdjustmentRepository = affiliateInvoiceAdjustmentRepository;
            this._BuyerRepository = buyerRepository;
            this._AffiliateRepository = affiliateRepository;
            this._historyRepository = historyRepository;
            this._settingService = settingService;
            this._logService = logService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get All Invoices of Buyers
        /// </summary>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<BuyerInvoice> GetAllBuyerInvoices(DateTime dateFrom, DateTime dateTo)
        {
            var query = from x in _BuyerInvoiceRepository.Table
                        where x.DateCreated >= dateFrom && x.DateCreated <= dateTo
                        orderby x.Id descending
                        select x;

            var bInvoices = query.ToList();

            return bInvoices;            
        }

        /// <summary>
        /// Get Invoices of Buyers
        /// </summary>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="status">The status.</param>
        /// <returns>BuyerInvoice Collection Item</returns>
        public virtual IList<BuyerInvoice> GetBuyerInvoices(long BuyerId, DateTime dateFrom, DateTime dateTo, int status)
        {
            string key = string.Format(CACHE_ACCOUNTING_GETBUYERINVOICES, BuyerId, dateFrom.ToString(), dateTo.ToString(), status);

            if (status == -3)
            {
                var query1 = from x in _BuyerInvoiceRepository.Table
                             orderby x.Id descending
                             where (BuyerId == 0 || x.BuyerId == BuyerId) && (x.Status >= 1) && x.DateCreated >= dateFrom && x.DateCreated <= dateTo
                             select x;
                var bInvoices1 = query1.ToList();
                return bInvoices1;
            }

            var query = from x in _BuyerInvoiceRepository.Table
                        orderby x.Id descending
                        where (BuyerId == 0 || x.BuyerId == BuyerId) && (status == -2 || x.Status == status) && x.DateCreated >= dateFrom && x.DateCreated <= dateTo
                        select x;
            var bInvoices = query.ToList();

            return bInvoices;
            //});
        }

        /// <summary>
        /// GetBuyerInvoiceById
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>BuyerInvoice</returns>
        public virtual BuyerInvoice GetBuyerInvoiceById(long Id)
        {
            return _BuyerInvoiceRepository.GetById(Id);
        }

        // <summary>
        /// <summary>
        /// GetAffiliateInvoiceById
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>AffiliateInvoices</returns>
        public virtual AffiliateInvoice GetAffiliateInvoiceById(long Id)
        {
            return _AffiliateInvoiceRepository.GetById(Id);
        }

        /// <summary>
        /// GetBuyerInvoiceDetails
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>BuyerInvoiceDetails</returns>
        public virtual List<BuyerInvoiceDetails> GetBuyerInvoiceDetails(long Id)
        {
            var IdParam = _dataProvider.GetParameter();
            IdParam.ParameterName = "InvoiceID";
            IdParam.Value = Id;
            IdParam.DbType = DbType.Int64;

            List<BuyerInvoiceDetails> result = _BuyerInvoiceRepository.GetDbClientContext().SqlQuery<BuyerInvoiceDetails>("EXECUTE [dbo].[GetBuyerInvoiceDetails] @InvoiceID", IdParam).ToList();
            return result;
        }

        /// <summary>
        /// GetAffiliateInvoiceDetails
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>BuyerInvoiceDetails</returns>
        public virtual List<AffiliateInvoiceDetails> GetAffiliateInvoiceDetails(long Id)
        {
            var IdParam = _dataProvider.GetParameter();
            IdParam.ParameterName = "InvoiceID";
            IdParam.Value = Id;
            IdParam.DbType = DbType.Int64;

            List<AffiliateInvoiceDetails> result = _AffiliateInvoiceRepository.GetDbClientContext().SqlQuery<AffiliateInvoiceDetails>("EXECUTE [dbo].[GetAffiliateInvoiceDetails] @InvoiceID", IdParam).ToList();
            return result;
        }

        /// <summary>
        /// Disable Buyer Invoice
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>true/false</returns>
        public int DisableBuyerInvoice(long Id)
        {
            var IdParam = _dataProvider.GetParameter();
            IdParam.ParameterName = "InvoiceId";
            IdParam.Value = Id;
            IdParam.DbType = DbType.Int64;

            var result = _BuyerInvoiceRepository.GetDbClientContext().SqlQuery<long>("EXECUTE [dbo].[DisableBuyerInvoice] @InvoiceId", IdParam).FirstOrDefault();

            return 1;
        }

        /// <summary>
        /// Disable Affiliate Invoice
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>true/false</returns>
        public int DisableAffiliateInvoice(long Id)
        {
            var IdParam = _dataProvider.GetParameter();
            IdParam.ParameterName = "InvoiceId";
            IdParam.Value = Id;
            IdParam.DbType = DbType.Int64;

            var result = _AffiliateInvoiceRepository.GetDbClientContext().SqlQuery<long>("EXECUTE [dbo].[DisableAffiliateInvoice] @InvoiceId", IdParam).FirstOrDefault();

            return 1;
        }

        /// <summary>
        /// Approve Buyer Invoice
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>true/false</returns>
        public int ApproveBuyerInvoice(long Id)
        {
            BuyerInvoice bi = _BuyerInvoiceRepository.GetById(Id);
            bi.Status = 1;
            _BuyerInvoiceRepository.Update(bi);
            return 1;
        }

        /// <summary>
        /// AffiliateInvoiceChangeStatus
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="Status">The status.</param>
        /// <returns>true/false</returns>
        public int AffiliateInvoiceChangeStatus(long Id, short Status)
        {
            AffiliateInvoice ai = _AffiliateInvoiceRepository.GetById(Id);
            ai.Status = Status;
            _AffiliateInvoiceRepository.Update(ai);
            return 1;
        }

        /// <summary>
        /// AffiliateInvoiceChangeStatus
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="Status">The status.</param>
        /// <returns>true/false</returns>
        public int BuyerInvoiceChangeStatus(long Id, short Status)
        {
            BuyerInvoice bi = _BuyerInvoiceRepository.GetById(Id);
            bi.Status = Status;
            _BuyerInvoiceRepository.Update(bi);
            return 1;
        }

        /// <summary>
        /// Approve Affiliate Invoice
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>true/false</returns>
        public int ApproveAffiliateInvoice(long Id)
        {
            AffiliateInvoice ai = _AffiliateInvoiceRepository.GetById(Id);
            ai.Status = 1;
            _AffiliateInvoiceRepository.Update(ai);
            return 1;
        }

        /// <summary>
        /// Approve Buyer Invoice
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="Amount">The amount.</param>
        /// <returns>true/false</returns>
        public int AddBuyerInvoicePayment(long Id, double Amount)
        {
            BuyerInvoice bi = _BuyerInvoiceRepository.GetById(Id);
            bi.Paid = Amount;
            _BuyerInvoiceRepository.Update(bi);
            return 1;
        }

        /// <summary>
        /// AddAffiliateInvoicePayment
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="Amount">The amount.</param>
        /// <returns>true/false</returns>
        public int AddAffiliateInvoicePayment(long Id, double Amount)
        {
            AffiliateInvoice ai = _AffiliateInvoiceRepository.GetById(Id);
            ai.Paid = Amount;
            _AffiliateInvoiceRepository.Update(ai);
            return 1;
        }

        /// <summary>
        /// GetBuyerDistrib
        /// </summary>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <returns>double</returns>
        public double GetBuyerDistrib(long BuyerId)
        {
            var buyerIdParam = _dataProvider.GetParameter();
            buyerIdParam.ParameterName = "BuyerId";
            buyerIdParam.Value = BuyerId;
            buyerIdParam.DbType = DbType.Int64;

            var result = _BuyerInvoiceRepository.GetDbClientContext().SqlQuery<double>("EXECUTE [dbo].[GetBuyerDistrib] @BuyerId", buyerIdParam).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Get All Invoices of Afiliates
        /// </summary>
        /// <param name="AffiliateId">The affiliate identifier.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="StatusFilter">The status filter.</param>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<AffiliateInvoice> GetAllAffiliateInvoices(long AffiliateId, DateTime dateFrom, DateTime dateTo, int StatusFilter)
        {
            var query = from x in _AffiliateInvoiceRepository.Table
                        orderby x.Id descending
                        where (AffiliateId == 0 || x.AffiliateId == AffiliateId) && (x.Status == StatusFilter || StatusFilter == -2) && (x.DateCreated >= dateFrom && x.DateCreated <= dateTo)
                        select x;

            var result = query.ToList();

            return result;            
        }

        /// <summary>
        /// GetAffiliateInvoices
        /// </summary>
        /// <param name="AffilaiteId">The affilaite identifier.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="status">The status.</param>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<AffiliateInvoice> GetAffiliateInvoices(long AffilaiteId, DateTime dateFrom, DateTime dateTo, int status)
        {            
            var query = from x in _AffiliateInvoiceRepository.Table
                        orderby x.Id descending
                        where (AffilaiteId == 0 || x.AffiliateId == AffilaiteId) && (x.Status >= 1) && x.DateCreated >= dateFrom && x.DateCreated <= dateTo
                        select x;

            var result = query.ToList();

            return result;            
        }

        /// <summary>
        /// InsertAffiliatePaymen
        /// </summary>
        /// <param name="affiliatePayment">affiliatePayment</param>
        /// <returns>Profile Collection Item</returns>
        /// <exception cref="ArgumentNullException">affiliatePayment</exception>
        public virtual long InsertAffiliatePayment(AffiliatePayment affiliatePayment)
        {
            if (affiliatePayment == null)
                throw new ArgumentNullException("affiliatePayment");

            _AffiliatePaymentRepository.Insert(affiliatePayment);

            _cacheManager.RemoveByPattern(CACHE_PROFILE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(affiliatePayment);
            return affiliatePayment.Id;
        }

        /// <summary>
        /// DeleteAffiliatePaymen
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>true/false</returns>
        /// <exception cref="ArgumentNullException">affiliatePayment</exception>
        public virtual int DeleteAffiliatePayment(long id)
        {
            if (id == 0)
                throw new ArgumentNullException("affiliatePayment");

            AffiliatePayment affiliatePayment = _AffiliatePaymentRepository.GetById(id);
            _AffiliatePaymentRepository.Delete(affiliatePayment);

            _cacheManager.RemoveByPattern(CACHE_PROFILE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(affiliatePayment);
            return 1;
        }

        /// <summary>
        /// Affiliate Payment
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<AffiliatePaymentView> GetAllAffiliatePayments()
        {
            return _AffiliatePaymentRepository.GetDbClientContext().SqlQuery<AffiliatePaymentView>("EXECUTE [dbo].[GetAllAffiliatePayments]").ToList();
        }

        /// <summary>
        /// Buyer Payment
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<BuyerPaymentView> GetAllBuyerPayments(short paymentMethod, string keyword)
        {            
            var paymentList = _BuyerPaymentRepository.GetDbClientContext().SqlQuery<BuyerPaymentView>("EXECUTE [dbo].[GetAllBuyerPayments]").ToList();
            if(paymentMethod != 0)
            {
                paymentList = paymentList.Where(a => a.PaymentMethod == paymentMethod)?.ToList();
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                paymentList = paymentList.Where(
                    a =>
                    (a.UserName != null && a.UserName.Contains(keyword)) ||
                    (a.BuyerName != null && a.BuyerName.Contains(keyword)) ||
                    (a.Note != null && a.Note.Contains(keyword))
                    )?.ToList();
            }

            return paymentList;
        }

        /// <summary>
        /// Buyer Payment
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="DateFrom">The date from.</param>
        /// <param name="DateTo">The date to.</param>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<BuyerPaymentView> GetAllBuyerPaymentsByBuyer(long buyerId, DateTime DateFrom, DateTime DateTo)
        {
            string key = string.Format(CACHE_ACCOUNTING_GETALLBUYERPAYMENTSBYBUYER, buyerId, DateFrom.ToString(), DateTo.ToString());

            return _cacheManager.Get(key, () =>
            {
                var buyerIdParam = _dataProvider.GetParameter();
                buyerIdParam.ParameterName = "BuyerId";
                buyerIdParam.Value = buyerId;
                buyerIdParam.DbType = DbType.Int64;

                var dateFromParam = _dataProvider.GetParameter();
                dateFromParam = _dataProvider.GetParameter();
                dateFromParam.ParameterName = "DateFrom";
                dateFromParam.Value = DateFrom;
                dateFromParam.DbType = DbType.DateTime;

                var dateToParam = _dataProvider.GetParameter();
                dateToParam.ParameterName = "DateTo";
                dateToParam.Value = DateTo;
                dateToParam.DbType = DbType.DateTime;

                var result = _BuyerPaymentRepository.GetDbClientContext().SqlQuery<BuyerPaymentView>("EXECUTE [dbo].[GetAllBuyerPaymentsByBuyer] @BuyerId, @DateFrom, @DateTo", buyerIdParam, dateFromParam, dateToParam).ToList();

                return result;
            });
        }

        /// <summary>
        /// InsertBuyerPaymen
        /// </summary>
        /// <param name="buyerPayment">buyerPayment</param>
        /// <returns>Profile Collection Item</returns>
        /// <exception cref="ArgumentNullException">buyerPayment</exception>
        public virtual long InsertBuyerPayment(BuyerPayment buyerPayment)
        {
            if (buyerPayment == null)
                throw new ArgumentNullException("buyerPayment");

            this._BuyerPaymentRepository.Insert(buyerPayment);

            _cacheManager.RemoveByPattern(CACHE_PROFILE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(buyerPayment);
            return buyerPayment.Id;
        }

        /// <summary>
        /// DeleteBuyerPaymen
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>true/false</returns>
        /// <exception cref="ArgumentNullException">buyerPayment</exception>
        public virtual int DeleteBuyerPayment(long id)
        {
            if (id == 0)
                throw new ArgumentNullException("buyerPayment");

            BuyerPayment buyerPayment = _BuyerPaymentRepository.GetById(id);
            if (buyerPayment != null)
            {
                _BuyerPaymentRepository.Delete(buyerPayment);

                _cacheManager.RemoveByPattern(CACHE_PROFILE_PATTERN_KEY);
                _cacheManager.ClearRemoteServersCache();
                _appEventPublisher.EntityInserted(buyerPayment);
            }
            return 1;
        }

        /// <summary>
        /// EditBuyerPaymen
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>true/false</returns>
        /// <exception cref="ArgumentNullException">buyerPayment</exception>
        public virtual int EditBuyerPayment(BuyerPayment buyerPaymentModel)
        {
            BuyerPayment buyerPayment = _BuyerPaymentRepository.GetById(buyerPaymentModel.Id);
            if (buyerPayment != null)
            {
                buyerPayment.Note = buyerPaymentModel.Note;
                buyerPayment.PaymentDate = buyerPaymentModel.PaymentDate;
                buyerPayment.PaymentMethod = buyerPaymentModel.PaymentMethod;
                buyerPayment.Amount = buyerPaymentModel.Amount;

                _BuyerPaymentRepository.Update(buyerPayment);

                _cacheManager.RemoveByPattern(CACHE_PROFILE_PATTERN_KEY);
                _cacheManager.ClearRemoteServersCache();
                _appEventPublisher.EntityInserted(buyerPayment);
            }
            else
            {
                throw new ArgumentException($"BuyerPayment with ID: '{buyerPaymentModel.Id}' does not exist");
            }

            return 1;
        }


        /// <summary>
        /// Buyer Payment
        /// </summary>
        /// <returns>IList BuyerPaymentView</returns>
        public virtual IList<RefundedLeads> GetAllRefundedLeads(int status, DateTime fromDate, DateTime toDate, string keyword)
        {
            var query = from x in _RefundedLeadsRepository.Table
                orderby x.Id descending
                where x.DateCreated >= fromDate && x.DateCreated <= toDate && (status == -1 || x.Approved == status) && (string.IsNullOrEmpty(keyword) || x.LeadId.ToString() == keyword || x.Reason.Contains(keyword) || x.ReviewNote.Contains(keyword))
                select x;

            var refLeads = query.ToList();

            return refLeads;
        }

        /// <summary>
        /// Gets the buyer refunded leads by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;RefundedLeads&gt;.</returns>
        public virtual IList<RefundedLeads> GetBuyerRefundedLeadsById(long Id)
        {
            string key = string.Format(CACHE_ACCOUNTING_GETALLREFUNDEDLEADSBYID, Id);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _RefundedLeadsRepository.Table
                            where x.BInvoiceId == Id && x.Approved == 1
                            orderby x.Id descending
                            select x;

                var refLeads = query.ToList();

                return refLeads;
            });
        }

        /// <summary>
        /// Gets the affiliate refunded leads by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;RefundedLeads&gt;.</returns>
        public virtual IList<RefundedLeads> GetAffiliateRefundedLeadsById(long Id)
        {
            string key = string.Format(CACHE_ACCOUNTING_GETAFFILIATEREFUNDEDLEADSBYID, Id);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _RefundedLeadsRepository.Table
                            where x.AInvoiceId == Id && x.Approved == 1
                            orderby x.Id descending
                            select x;

                var refLeads = query.ToList();

                return refLeads;
            });
        }

        /// <summary>
        /// Insert Refunded Lead
        /// </summary>
        /// <param name="refLeads">The reference leads.</param>
        /// <returns>long</returns>
        /// <exception cref="ArgumentNullException">refLeads</exception>
        public virtual long InsertRefundedLeads(RefundedLeads refLeads)
        {
            if (refLeads == null)
                throw new ArgumentNullException("refLeads");

            try
            {
                this._RefundedLeadsRepository.Insert(refLeads);
            }
            catch (Exception ex)
            {
                //Arman Handle Exception
                _logService.Error(ex.Message, ex);
                return 0;
            }

            _cacheManager.RemoveByPattern(CACHE_PROFILE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(refLeads);
            return refLeads.Id;
        }

        /// <summary>
        /// Delete Refunded Lead
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>true/false</returns>
        public virtual int DeleteRefundedLead(long Id)
        {
            RefundedLeads refLead = _RefundedLeadsRepository.GetById(Id);
            this._RefundedLeadsRepository.Delete(refLead);

            _cacheManager.RemoveByPattern(CACHE_PROFILE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(refLead);
            return 1;
        }

        /// <summary>
        /// Change Refunded Lead Status
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="Status">The status.</param>
        /// <param name="Note">The note.</param>
        /// <returns>true/false</returns>
        public virtual int ChangeRefundedStatus(long Id, byte Status, string Note)
        {
            RefundedLeads refLead = _RefundedLeadsRepository.GetById(Id);

            if (refLead != null)
            {
                refLead.Approved = Status;
                refLead.ReviewNote = Note;
                this._RefundedLeadsRepository.Update(refLead);

                if (refLead.BInvoiceId != null)
                {
                    BuyerInvoice bi = this._BuyerInvoiceRepository.GetById(refLead.BInvoiceId);
                    bi.Refunded = (double)refLead.BPrice;
                    this._BuyerInvoiceRepository.Update(bi);
                }

                if (refLead.AInvoiceId != null)
                {
                    AffiliateInvoice ai = this._AffiliateInvoiceRepository.GetById(refLead.AInvoiceId);
                    ai.Refunded = (double)refLead.APrice;
                    this._AffiliateInvoiceRepository.Update(ai);
                }

                _cacheManager.RemoveByPattern(CACHE_PROFILE_PATTERN_KEY);
                _cacheManager.ClearRemoteServersCache();
                _appEventPublisher.EntityInserted(refLead);
            }
            return 1;
        }

        /// <summary>
        /// Generates the invoices.
        /// </summary>
        /// <returns>System.Int64.</returns>
        public long GenerateInvoices()
        {
            this._historyRepository.AddHistory("AccountingSevice", HistoryAction.Invoice_Generated_Invoice, "", 0, "Starting GenerateInvoices", "", "", 0);

            int dayOfWeekNum = (int)DateTime.Now.DayOfWeek;
            int today = (int)DateTime.Now.Day;

            // For Buyers
            var buyersQuery = from c in _BuyerRepository.Table
                              where (c.BillFrequency == "w" && c.FrequencyValue == (dayOfWeekNum)) || (c.BillFrequency == "m" && c.FrequencyValue == today) ||
                              (c.BillFrequency == "bw" && c.FrequencyValue == (dayOfWeekNum))
                              orderby c.Id
                              select c;

            IList<Buyer> buyers = buyersQuery.ToList();

            foreach (Buyer b in buyers)
            {
                if (b.BillFrequency == "bw" && (!b.IsBiWeekly.HasValue || (b.IsBiWeekly.HasValue && !b.IsBiWeekly.Value)))
                {
                    continue;
                }

                DateTime dateTo;
                dateTo = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                long retId = GenerateBuyerInvoices(b.Id, null, dateTo, this._appContext.AppUser.Id);
            }

            foreach (Buyer b in buyers)
            {
                if (b.BillFrequency == "bw")
                {
                    if (b.IsBiWeekly.HasValue)
                    {
                        b.IsBiWeekly = !b.IsBiWeekly.Value;
                    }
                    else
                    {
                        b.IsBiWeekly = false;
                    }

                    _BuyerRepository.Update(b);
                }
            }

            // For Affiliates
            var affiliatesQuery = from c in _AffiliateRepository.Table
                                  where (c.BillFrequency == "w" && c.FrequencyValue == (dayOfWeekNum)) || (c.BillFrequency == "m" && c.FrequencyValue == today) ||
                                  (c.BillFrequency == "bw" && c.FrequencyValue == (dayOfWeekNum))
                                  orderby c.Id
                                  select c;

            IList<Affiliate> affiliates = affiliatesQuery.ToList();

            foreach (Affiliate a in affiliates)
            {
                if (a.BillFrequency == "bw" && (!a.IsBiWeekly.HasValue || (a.IsBiWeekly.HasValue && !a.IsBiWeekly.Value)))
                {
                    continue;
                }

                DateTime dateTo;
                dateTo = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                long retId = GenerateAffiliateInvoices(a.Id, null, dateTo, this._appContext.AppUser.Id);
            }

            foreach (Affiliate a in affiliates)
            {
                if (a.BillFrequency == "bw")
                {
                    if (a.IsBiWeekly.HasValue)
                    {
                        a.IsBiWeekly = !a.IsBiWeekly.Value;
                    }
                    else
                    {
                        a.IsBiWeekly = false;
                    }

                    _AffiliateRepository.Update(a);
                }
            }

            this._historyRepository.AddHistory("AccountingSevice", HistoryAction.Invoice_Generated_Invoice, "", 0, "Ending GenerateInvoices", "", "", 0);

            return 1;
        }

        /// <summary>
        /// Invoice Generation
        /// </summary>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="UserId">The user identifier.</param>
        /// <returns>Generated Invoice Id</returns>
        public virtual long GenerateBuyerInvoices(long BuyerId, DateTime? dateFrom, DateTime dateTo, long UserId)
        {
            var buyerIdParam = _dataProvider.GetParameter();
            buyerIdParam.ParameterName = "bID";
            buyerIdParam.Value = BuyerId;
            buyerIdParam.DbType = DbType.Int64;

            var dateFromParam = _dataProvider.GetParameter();
            dateFromParam.ParameterName = "dateFrom";
            if (dateFrom.HasValue)
                dateFromParam.Value = dateFrom.Value;
            else
                dateFromParam.Value = DBNull.Value;
            dateFromParam.DbType = DbType.DateTime;

            var dateToParam = _dataProvider.GetParameter();
            dateToParam.ParameterName = "dateTo";
            dateToParam.Value = dateTo;
            dateToParam.DbType = DbType.DateTime;

            var userIdParam = _dataProvider.GetParameter();
            userIdParam.ParameterName = "UserID";
            userIdParam.Value = UserId;
            userIdParam.DbType = DbType.Int64;

            long result = _BuyerInvoiceRepository.GetDbClientContext().SqlQuery<long>("EXECUTE [dbo].[GenerateBuyerInvoices] @bID, @dateFrom, @dateTo, @UserID", buyerIdParam, dateFromParam, dateToParam, userIdParam).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Invoice Generation for Affiliate
        /// </summary>
        /// <param name="AffiliateId">The affiliate identifier.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="UserId">The user identifier.</param>
        /// <returns>Generated Invoice Id</returns>
        public virtual long GenerateAffiliateInvoices(long AffiliateId, DateTime? dateFrom, DateTime dateTo, long UserId)
        {
            var affiliateIdParam = _dataProvider.GetParameter();
            affiliateIdParam.ParameterName = "aID";
            affiliateIdParam.Value = AffiliateId;
            affiliateIdParam.DbType = DbType.Int64;

            var dateFromParam = _dataProvider.GetParameter();
            dateFromParam.ParameterName = "dateFrom";
            if (dateFrom.HasValue)
                dateFromParam.Value = dateFrom.Value;
            else
                dateFromParam.Value = DBNull.Value;
            dateFromParam.DbType = DbType.DateTime;

            var dateToParam = _dataProvider.GetParameter();
            dateToParam.ParameterName = "dateTo";
            dateToParam.Value = dateTo;
            dateToParam.DbType = DbType.DateTime;

            var userIdParam = _dataProvider.GetParameter();
            userIdParam.ParameterName = "UserID";
            userIdParam.Value = UserId;
            userIdParam.DbType = DbType.Int64;

            long result = _AffiliateInvoiceRepository.GetDbClientContext().SqlQuery<long>("EXECUTE [dbo].[GenerateAffiliateInvoices] @aID, @dateFrom, @dateTo, @UserID", affiliateIdParam, dateFromParam, dateToParam, userIdParam).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Get Balance of Buyers
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="DateFrom">The date from.</param>
        /// <param name="DateTo">The date to.</param>
        /// <returns>BuyerBanace</returns>
        public virtual IList<BuyerBalanceView> GetBuyersBalance(long buyerId, DateTime DateFrom, DateTime DateTo)
        {            
            var buyerIdParam = _dataProvider.GetParameter();
            buyerIdParam.ParameterName = "BuyerId";
            buyerIdParam.Value = buyerId;
            buyerIdParam.DbType = DbType.Int64;

            var dateFromParam = _dataProvider.GetParameter();
            dateFromParam.ParameterName = "DateFrom";
            dateFromParam.Value = DateFrom;
            dateFromParam.DbType = DbType.DateTime;

            var dateToParam = _dataProvider.GetParameter();
            dateToParam.ParameterName = "DateTo";
            dateToParam.Value = DateTo;
            dateToParam.DbType = DbType.DateTime;

            IList<BuyerBalanceView> result = _BuyerBanaceRepository.GetDbClientContext().SqlQuery<BuyerBalanceView>("EXECUTE [dbo].[GetBuyersBalance] @BuyerId, @DateFrom, @DateTo", buyerIdParam, dateFromParam, dateToParam).ToList();
            return result;            
        }

        /// <summary>
        /// Get Balance of Buyers
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <param name="DateFrom">The date from.</param>
        /// <param name="DateTo">The date to.</param>
        /// <returns>BuyerBanace</returns>
        public virtual IList<BuyerBalanceView> GetAffiliatesBalance(long affiliateId, DateTime DateFrom, DateTime DateTo)
        {            
            var affiliateIdParam = _dataProvider.GetParameter();
            affiliateIdParam.ParameterName = "AffiliateId";
            affiliateIdParam.Value = affiliateId;
            affiliateIdParam.DbType = DbType.Int64;

            var dateFromParam = _dataProvider.GetParameter();
            dateFromParam.ParameterName = "DateFrom";
            dateFromParam.Value = DateFrom;
            dateFromParam.DbType = DbType.DateTime;

            var dateToParam = _dataProvider.GetParameter();
            dateToParam.ParameterName = "DateTo";
            dateToParam.Value = DateTo;
            dateToParam.DbType = DbType.DateTime;

            IList<BuyerBalanceView> result = _AffiliateRepository.GetDbClientContext().SqlQuery<BuyerBalanceView>("EXECUTE [dbo].[GetAffiliateBalance] @AffiliateId, @DateFrom, @DateTo", affiliateIdParam, dateFromParam, dateToParam).ToList();
            return result;            
        }

        /// <summary>
        /// Get Buyer Credit By Id
        /// </summary>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <returns>decimal (money)</returns>
        public virtual decimal GetBuyerCredit(long BuyerId)
        {            
            var query = from x in _BuyerBanaceRepository.Table
                        where x.BuyerId == BuyerId
                        select x.Credit;

            var buyerCredit = query.FirstOrDefault();

            return buyerCredit;         
        }

        /// <summary>
        /// Get Buyer Balance By Id
        /// </summary>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <returns>BuyerBalance</returns>
        public virtual BuyerBalance GetBuyerBalanceById(long BuyerId)
        {            
            var query = from x in _BuyerBanaceRepository.Table
                        where x.BuyerId == BuyerId
                        select x;

            var buyerBalance = query.FirstOrDefault();

            return buyerBalance;         
        }

        /// <summary>
        /// InsertBuyerBalance
        /// </summary>
        /// <param name="buyerBalance">The buyer balance.</param>
        /// <returns>long</returns>
        public virtual long InsertBuyerBalance(BuyerBalance buyerBalance)
        {
            _BuyerBanaceRepository.Insert(buyerBalance);
            return buyerBalance.Id;
        }

        /// <summary>
        /// UpdateBuyerBalance
        /// </summary>
        /// <param name="buyerBalance">The buyer balance.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>long</returns>
        public virtual long UpdateBuyerBalance(BuyerBalance buyerBalance, string columnName)
        {
            var query = from x in _BuyerBanaceRepository.Table
                        where x.BuyerId == buyerBalance.BuyerId
                        select x;

            BuyerBalance buyerBal = query.FirstOrDefault();

            if (buyerBal == null) return 0;

            BuyerBalance bb = _BuyerBanaceRepository.GetById(buyerBal.Id);

            if (columnName == "PaymentSum")
            {
                bb.PaymentSum += buyerBalance.PaymentSum;
                bb.Balance = bb.PaymentSum + bb.Credit - bb.SoldSum;
            }

            if (columnName == "Credit")
            {
                bb.Credit = buyerBalance.Credit;
                bb.Balance = bb.PaymentSum + bb.Credit - bb.SoldSum;
            }

            if (columnName == "DemoBalance")
            {
                if (bb.Balance <= 0)
                    bb.Balance = 50000;
            }

            _BuyerBanaceRepository.Update(bb);

            return buyerBalance.Id;
        }

        /// <summary>
        /// UpdateBuyerBalance
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="soldSum">The sold sum.</param>
        /// <returns>long</returns>
        public virtual long UpdateBuyerBalance(long buyerId, decimal soldSum)
        {
            var buyerParam = _dataProvider.GetParameter();
            buyerParam.ParameterName = "BuyerId";
            buyerParam.Value = buyerId;
            buyerParam.DbType = DbType.Int64;

            var soldSumParam = _dataProvider.GetParameter();
            soldSumParam.ParameterName = "Sum";
            soldSumParam.Value = soldSum;
            soldSumParam.DbType = DbType.Decimal;

            _BuyerBanaceRepository.GetDbClientContext().SqlQuery<BuyerBalanceView>("EXECUTE [dbo].[UpdateBuyerBalance] @BuyerId, @Sum", buyerParam, soldSumParam).FirstOrDefault();

            return 1;
        }

        /// <summary>
        /// UpdateBuyerBalance
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <returns>long</returns>
        public virtual bool CheckCredit(long buyerId)
        {
            var query = from x in _BuyerBanaceRepository.Table
                        where x.BuyerId == buyerId
                        select x.Balance;

            decimal buyerCredit = query.FirstOrDefault();

            return (buyerCredit > 0);
        }

        /// <summary>
        /// AddBuyerInvoiceAdjustment
        /// </summary>
        /// <param name="BinvoiceId">The binvoice identifier.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Price">The price.</param>
        /// <param name="Qty">The qty.</param>
        /// <returns>true/false</returns>
        public long AddBuyerInvoiceAdjustment(long BinvoiceId, string Name, double Price, int Qty)
        {
            BuyerInvoiceAdjustment bia = new BuyerInvoiceAdjustment
            {
                BuyerInvoiceId = BinvoiceId,
                Name = Name,
                Price = Price,
                Qty = Qty,
                Sum = Price * Qty
            };

            this._BuyerInvoiceAdjustmentRepository.Insert(bia);

            BuyerInvoice bi = this._BuyerInvoiceRepository.GetById(BinvoiceId);
            bi.Adjustment = Price * Qty;
            this._BuyerInvoiceRepository.Update(bi);

            return bia.Id;
        }

        /// <summary>
        /// AddBuyerInvoiceAdjustment
        /// </summary>
        /// <param name="AinvoiceId">The ainvoice identifier.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Price">The price.</param>
        /// <param name="Qty">The qty.</param>
        /// <returns>true/false</returns>
        public long AddAffiliateInvoiceAdjustment(long AinvoiceId, string Name, double Price, int Qty)
        {
            AffiliateInvoiceAdjustment aia = new AffiliateInvoiceAdjustment
            {
                AffiliateInvoiceId = AinvoiceId,
                Name = Name,
                Price = Price,
                Qty = Qty,
                Sum = Price * Qty
            };

            this._AffiliateInvoiceAdjustmentRepository.Insert(aia);

            AffiliateInvoice ai = this._AffiliateInvoiceRepository.GetById(AinvoiceId);
            ai.Adjustment = Price * Qty;
            this._AffiliateInvoiceRepository.Update(ai);

            return aia.Id;
        }

        /// <summary>
        /// DeleteBuyerInvoiceAdjustment
        /// </summary>
        /// <param name="AdjustmentId">The adjustment identifier.</param>
        /// <returns>true/false</returns>
        public long DeleteBuyerInvoiceAdjustment(long AdjustmentId)
        {
            BuyerInvoiceAdjustment biaObj = this._BuyerInvoiceAdjustmentRepository.GetById(AdjustmentId);

            if (biaObj != null)
            {
                this._BuyerInvoiceAdjustmentRepository.Delete(biaObj);

                BuyerInvoice bi = this._BuyerInvoiceRepository.GetById(biaObj.BuyerInvoiceId);
                bi.Adjustment = 0;
                this._BuyerInvoiceRepository.Update(bi);
            }

            return 1;
        }

        /// <summary>
        /// DeleteAffiliateInvoiceAdjustment
        /// </summary>
        /// <param name="AdjustmentId">The adjustment identifier.</param>
        /// <returns>true/false</returns>
        public long DeleteAffiliateInvoiceAdjustment(long AdjustmentId)
        {
            AffiliateInvoiceAdjustment biaObj = this._AffiliateInvoiceAdjustmentRepository.GetById(AdjustmentId);
            if (biaObj != null)
            {
                this._AffiliateInvoiceAdjustmentRepository.Delete(biaObj);
            }
            return 1;
        }

        /// <summary>
        /// Gets the buyer adjustments.
        /// </summary>
        /// <param name="InvoiceId">The invoice identifier.</param>
        /// <returns>IList&lt;BuyerInvoiceAdjustment&gt;.</returns>
        public IList<BuyerInvoiceAdjustment> GetBuyerAdjustments(long InvoiceId)
        {            
            var query = from x in _BuyerInvoiceAdjustmentRepository.Table
                        where x.BuyerInvoiceId == InvoiceId
                        select x;

            var buyerInvoiceAdjustment = query.ToList();

            return buyerInvoiceAdjustment;         
        }

        /// <summary>
        /// Gets the affiliate adjustments.
        /// </summary>
        /// <param name="InvoiceId">The invoice identifier.</param>
        /// <returns>IList&lt;AffiliateInvoiceAdjustment&gt;.</returns>
        public IList<AffiliateInvoiceAdjustment> GetAffiliateAdjustments(long InvoiceId)
        {            
            var query = from x in _AffiliateInvoiceAdjustmentRepository.Table
                        where x.AffiliateInvoiceId == InvoiceId
                        select x;

            var affilaiteInvoiceAdjustment = query.ToList();

            return affilaiteInvoiceAdjustment;         
        }

        /// <summary>
        /// Creates the buyer invoice.
        /// </summary>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <param name="date">The date.</param>
        /// <returns>System.Int64.</returns>
        public long CreateBuyerInvoice(long BuyerId, DateTime date)
        {
            var query = from x in _BuyerInvoiceRepository.Table
                        where x.BuyerId == BuyerId
                        select x.Number;

            BuyerInvoice bi = new BuyerInvoice
            {
                BuyerId = BuyerId,
                Number = query.DefaultIfEmpty(0).Max() + 1,
                Adjustment = 0,
                DateCreated = date,
                DateFrom = date,
                DateTo = date,
                Paid = 0,
                Refunded = 0,
                Status = 0,
                Sum = 0,
                UserID = 0
            };

            _BuyerInvoiceRepository.Insert(bi);

            return bi.Id;
        }

        /// <summary>
        /// Creates the affiliate invoice.
        /// </summary>
        /// <param name="AffiliateId">The affiliate identifier.</param>
        /// <param name="date">The date.</param>
        /// <returns>System.Int64.</returns>
        public long CreateAffiliateInvoice(long AffiliateId, DateTime date)
        {
            var query = from x in _AffiliateInvoiceRepository.Table
                        where x.AffiliateId == AffiliateId
                        select x.Number;

            AffiliateInvoice bi = new AffiliateInvoice
            {
                AffiliateId = AffiliateId,
                Number = query.DefaultIfEmpty(0).Max(),
                Adjustment = 0,
                DateCreated = date,
                DateFrom = date,
                DateTo = date,
                Paid = 0,
                Refunded = 0,
                Status = 0,
                Sum = 0,
                UserID = 0
            };

            _AffiliateInvoiceRepository.Insert(bi);

            return bi.Id;
        }


        /// <summary>
        /// Creates the custom invoice.
        /// </summary>
        /// <param name="customInvoice">The customInvoice identifier.</param>
        /// <returns>System.Int64.</returns>
        public long CreateCustomInvoice(CustomInvoice customInvoice)
        {
            _CustomInvoiceRepository.Insert(customInvoice);
            return customInvoice.Id;
        }

        /// <summary>
        /// Creates the custom invoice.
        /// </summary>
        /// <param name="customInvoiceRow">The customInvoiceRow identifier.</param>
        /// <returns>System.Int64.</returns>
        public long CreateCustomInvoiceRow(CustomInvoiceRow customInvoiceRow)
        {
            _CustomInvoiceRowRepository.Insert(customInvoiceRow);
            return customInvoiceRow.Id;
        }


        /// <summary>
        /// Get custom all invoices.
        /// </summary>
        /// <returns>List<CustomInvoice>.</returns>
        public List<CustomInvoice> GetCustomInvoices()
        {
            var query = from x in _CustomInvoiceRepository.Table
                        select x;
            
            return query.ToList();
        }

        /// <summary>
        /// Get custom all invoice rows.
        /// </summary>
        /// /// <param name="customInvoiceId">The customInvoiceRow identifier.</param>
        /// <returns>List<CustomInvoiceRows>.</returns>
        public List<CustomInvoiceRow> GetCustomInvoiceRows(long customInvoiceId)
        {
            var query = from x in _CustomInvoiceRowRepository.Table
                        where x.CustomInvoiceId == customInvoiceId
                        select x;

            return query.ToList();
        }



        #endregion Methods
    }
}