// ***********************************************************************
// Assembly         : Adrack.Controller
// Author           : Arman Zakaryan
// Created          : 18-11-2020
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 04-02-2021
// ***********************************************************************
// <copyright file="AccountingController.cs" company="Adrack.com">
//     Copyright © 2020
// </copyright>
// <summary></summary>
// ***********************************************************************


using System;
using System.Collections.Generic;
using System.Web.Http;
using Adrack.Core;
using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure.Data;
using Adrack.Service.Accounting;
using Adrack.Service.Configuration;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.WebApi.Models.Accounting;
using System.Linq;
using Adrack.WebApi.Models.Lead;
using Adrack.Core.Domain.Directory;
using System.IO;
using iTextSharp.text.pdf;
using Adrack.Service.Directory;
using iTextSharp.text;
using Adrack.Service.Message;
using System.Text;
using System.Web;
using Adrack.Data;
using Adrack.Web.Framework.Security;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/accounting")]
    public class AccountingController : BaseApiPublicController //BaseApiController
    {
        #region fields

        private readonly IAccountingService _accountingService;
        private readonly IUserService _userService;
        private readonly IAppContext _appContext;
        private readonly IProfileService _profileService;
        private readonly ISettingService _settingService;
        private readonly IPermissionService _permissionService;
        private readonly IRepository<Buyer> _BuyerRepository;
        private readonly IRepository<Affiliate> _AffiliateRepository;
        private readonly ILeadMainService _leadMainService;
        private readonly ILeadMainResponseService _leadMainResponseService;
        private readonly IBuyerService _buyerService;
        private readonly IAffiliateService _affiliateService;
        private readonly ICountryService _countryService;
        private readonly IEmailService _emailService;
        private readonly ISmtpAccountService _smtpAccountService;

        #endregion

        #region constructor

        public AccountingController(    IAccountingService accountingService,
                                        IUserService userService,
                                        IAppContext appContext,
                                        IProfileService profileService,
                                        ISettingService settingService,
                                        IPermissionService permissionService,
                                        IRepository<Buyer> buyerRepository,
                                        IRepository<Affiliate> affiliateRepository,
                                        ILeadMainService leadMainService,
                                        ILeadMainResponseService leadMainResponseService,
                                        IBuyerService buyerService,
                                        IAffiliateService affiliateService,
                                        ICountryService countryService,
                                        ISmtpAccountService smtpAccountService,
                                        IEmailService emailService)
        {
            _accountingService = accountingService;
            _userService = userService;
            _appContext = appContext;
            _profileService = profileService;
            _settingService = settingService;
            _permissionService = permissionService;
            _BuyerRepository = buyerRepository;
            _AffiliateRepository = affiliateRepository;
            _leadMainService = leadMainService;
            _leadMainResponseService = leadMainResponseService;
            _buyerService = buyerService;
            _countryService = countryService;
            _affiliateService = affiliateService;
            _emailService = emailService;
            _smtpAccountService = smtpAccountService;
        }

        #endregion

        #region methods

        #region Buyer Invoices
        [HttpGet]
        [Route("getBuyerInvoices")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetBuyerInvoices(  [FromUri] long buyerId, // 0 = All Buyers
                                                    [FromUri] DateTime dateFrom,
                                                    [FromUri] DateTime dateTo,
                                                    [FromUri] int status) // -2 = All Statuses
        {
            /*
            if (! _permissionService.Authorize("accounting-buyer-invoice"))
            {
                return HttpBadRequest("No Access");
            }
            */

            if (dateFrom == null)
            {
                dateFrom = DateTime.MinValue;
            }

            if (dateTo == null)
            {
                dateTo = DateTime.UtcNow;
            }

            BuyerInvoicesModel buyerInvoicesModel = new BuyerInvoicesModel();

            IList<BuyerInvoice> biList = _accountingService.GetBuyerInvoices(buyerId, dateFrom, dateTo, status);

            buyerInvoicesModel.approvedInvoices = 0;
            buyerInvoicesModel.payments = 0;
            buyerInvoicesModel.outstanding = 0;

            foreach (BuyerInvoice bi in biList)
            {
                Buyer buyer = this._buyerService.GetBuyerById(bi.BuyerId);

                if (buyer != null)
                {

                    buyerInvoicesModel.buyerInvoices.Add(new BuyerInvoiceModel()
                    {
                        Adjustment = bi.Adjustment,
                        BuyerCountryName = buyer.Country?.Name,
                        BuyerId = bi.BuyerId,
                        CompanyAddress = buyer.AddressLine1,
                        CompanyName = buyer.Name,
                        CompanyLogoPath = buyer.IconPath,
                        DateFrom = bi.DateFrom.ToString(),
                        DateTo = bi.DateTo.ToString(),
                        DateCreated = bi.DateCreated.ToString(),
                        Id = bi.Id,
                        Number = bi.BuyerId + "." + bi.Number,
                        Refunded = bi.Refunded,
                        Status = bi.Status,
                        Sum = bi.Sum,
                        Total = (decimal)bi.Sum,
                        Distribute = bi.Sum - bi.Paid,
                        RefundedTotal = (decimal)bi.Refunded,
                        UserID = bi.UserID,
                        BuyerInvoiceAdjustmentsList = this._accountingService.GetBuyerAdjustments(bi.Id)

                    });

                    if (bi.Status == 1 || bi.Status == 2)
                    {
                        buyerInvoicesModel.approvedInvoices += (bi.Sum - bi.Refunded + bi.Adjustment);
                    }
                }
            }

            IList<BuyerBalanceView> bbvList = this._accountingService.GetBuyersBalance(buyerId, dateFrom, dateTo);
            foreach (BuyerBalanceView bbv in bbvList)
            {
                buyerInvoicesModel.payments += (double)bbv.PaymentSum;
            }

            buyerInvoicesModel.outstanding = buyerInvoicesModel.payments - buyerInvoicesModel.approvedInvoices;


            return Ok(buyerInvoicesModel);
        }

        [HttpGet]
        [Route("getBuyerInvoice/{id}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetBuyerInvoice(long id)
        {
            BuyerInvoice bi = this._accountingService.GetBuyerInvoiceById(id);
            BuyerInvoiceModel biModel = new BuyerInvoiceModel();

            biModel.Status = bi.Status;
            biModel.BuyerId = bi.BuyerId;
            biModel.DateCreated = bi.DateCreated.ToShortDateString();
            biModel.DateFrom = bi.DateFrom.ToShortDateString();
            biModel.DateTo = bi.DateTo.ToShortDateString();
            biModel.Id = bi.Id;
            biModel.Number = bi.BuyerId.ToString() + "." + bi.Number.ToString();
            biModel.Sum = bi.Sum;
            biModel.UserID = bi.UserID;

            biModel.BuyerInvoiceDetailsList = this._accountingService.GetBuyerInvoiceDetails(id);
            biModel.BuyerRefundedLeadsList = this._accountingService.GetBuyerRefundedLeadsById(id);
            biModel.BuyerInvoiceAdjustmentsList = this._accountingService.GetBuyerAdjustments(id);

            biModel.Total = 0;
            biModel.RefundedTotal = 0;
            foreach (RefundedLeads rl in biModel.BuyerRefundedLeadsList)
            {
                biModel.RefundedTotal += rl.BPrice;
            }

            biModel.AdjustmentTotal = 0;
            foreach (BuyerInvoiceAdjustment bia in biModel.BuyerInvoiceAdjustmentsList)
            {
                biModel.AdjustmentTotal += (decimal)bia.Sum;
            }

            biModel.Total = (decimal)biModel.Sum - (decimal)biModel.RefundedTotal + (decimal)biModel.AdjustmentTotal;

            biModel.CompanyName = this._settingService.GetSetting("Settings.CompanyName").Value;
            biModel.CompanyAddress = this._settingService.GetSetting("Settings.CompanyAddress").Value;
            biModel.CompanyBank = this._settingService.GetSetting("Settings.CompanyBank").Value;
            biModel.CompanyEmail = this._settingService.GetSetting("Settings.CompanyEmail").Value;
            biModel.CompanyLogoPath = this._settingService.GetSetting("Settings.CompanyLogoPath").Value;

            Buyer buyer = this._buyerService.GetBuyerById(bi.BuyerId);
            Country country = this._countryService.GetCountryById(buyer.CountryId);
            biModel.BuyerCountryName = country.Name;

            return Ok(biModel);
        }

        [HttpPost]
        [Route("approveBuyerInvoice/{id}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult ApproveBuyerInvoice(long id)
        {
            if (_appContext.AppUser.UserTypeId == (long)SharedData.AffiliateUserTypeId)
            {
                return null;
            }

            _accountingService.ApproveBuyerInvoice(id);
            return Ok(1);
        }

        [HttpPost]
        [Route("disapproveBuyerInvoice/{id}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult DisapproveBuyerInvoice(long id)
        {
            if (_appContext.AppUser.UserTypeId == (long)SharedData.AffiliateUserTypeId)
            {
                return null;
            }

            _accountingService.BuyerInvoiceChangeStatus(id, 0);
            return Ok(1);
        }

        [HttpPost]
        [Route("closeBuyerInvoice/{id}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult CloseBuyerInvoice(long id)
        {
            if (_appContext.AppUser.UserTypeId == (long)SharedData.AffiliateUserTypeId)
            {
                return null;
            }

            BuyerInvoice bInvoice = _accountingService.GetBuyerInvoiceById(id);
            if(bInvoice == null)
            {
                return HttpBadRequest("Not found");
            }
            _accountingService.AddBuyerInvoicePayment(id, bInvoice.Sum);
            _accountingService.BuyerInvoiceChangeStatus(id, 2);

            return Ok(1);
        }


        [HttpPost]
        [Route("disableBuyerInvoice/{id}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult DisableBuyerInvoice(long id)
        {
            if (_appContext.AppUser.UserTypeId == (long)SharedData.AffiliateUserTypeId)
            {
                return null;
            }

            _accountingService.DisableBuyerInvoice(id);
            return Ok(1);
        }

        [HttpPost]
        [Route("reopenBuyerInvoice/{id}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult ReopenBuyerInvoice(long id)
        {
            if (_appContext.AppUser.UserTypeId == (long)SharedData.AffiliateUserTypeId)
            {
                return null;
            }

            _accountingService.BuyerInvoiceChangeStatus(id, 1);
            return Ok(1);
        }

        [HttpPost]
        [Route("addBuyerInvoiceAdjustment")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult AddBuyerInvoiceAdjustment(
                                                    [FromUri] long buyerInvoiceId,
                                                    [FromUri] string name,
                                                    [FromUri] double price,
                                                    [FromUri] int quantity )
        {
            if (_appContext.AppUser.UserTypeId == (long)SharedData.AffiliateUserTypeId)
            {
                return null;
            }

            _accountingService.AddBuyerInvoiceAdjustment(buyerInvoiceId, name, price, quantity);
            return Ok(1);
        }

        [HttpPost]
        [Route("deleteBuyerInvoiceAdjustment/{adjustmentId}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult DeleteBuyerInvoiceAdjustment(long adjustmentId)
        {
            if (_appContext.AppUser.UserTypeId == (long)SharedData.AffiliateUserTypeId)
            {
                return null;
            }

            _accountingService.DeleteBuyerInvoiceAdjustment(adjustmentId);
            return Ok(1);
        }

        [HttpPost]
        [Route("sendEmailInvoiceToBuyer/{invoiceId}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult SendEmailInvoiceToBuyer(long invoiceId)
        {
            var smtpSetting = this._smtpAccountService.GetSmtpAccount();
            var techContact = _settingService.GetSetting("AppSetting.SupportEmail")?.Value ?? "no-reply@adrack.com";
            var user = _appContext.AppUser;
            var body = new StringBuilder();

            BuyerInvoice bi = _accountingService.GetBuyerInvoiceById(invoiceId);

            Buyer buyer = _BuyerRepository.GetById(bi.BuyerId);

            string PdfFileUrl = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/Uploads/Invoice_" + bi.Number + ".pdf";

            body.AppendLine($"Invoice: <a href='{PdfFileUrl}'>Download</a> ");
            
            _emailService.SendEmail(smtpSetting, techContact, "Adrack Support", buyer.Email, buyer.Name, "Invoice #" + bi.BuyerId.ToString() + "." + bi.Number.ToString(), body.ToString());

            return Ok(1);
        }

        [HttpPost]
        [Route("createBlankInvoiceBuyer")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult CreateBlankInvoiceBuyer(   [FromUri] long buyerId,
                                                            [FromUri] DateTime dateTo)
        {
            DateTime dateToUtc = _settingService.GetUTCDate(dateTo);

            long retId = this._accountingService.CreateBuyerInvoice(buyerId, dateToUtc);

            return Ok(retId);
        }


        #endregion


        #region Affiliate Invoices
        [HttpGet]
        [Route("getAffiliateInvoices")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAffiliateInvoices(  [FromUri] long affiliateId, // 0 = All Affiliates
                                                        [FromUri] DateTime dateFrom,
                                                        [FromUri] DateTime dateTo,
                                                        [FromUri] int status) // -2 = All Statuses
        {
            /*
            if (_appContext.AppUser.UserTypeId == (long)SharedData.AffiliateUserTypeId)
            {
                return null;
            }
            */

            if (dateFrom == null)
            {
                dateFrom = DateTime.MinValue;
            }

            if (dateTo == null)
            {
                dateTo = DateTime.UtcNow;
            }

            AffiliateInvoicesModel affiliateInvoicesModel = new AffiliateInvoicesModel();

            affiliateInvoicesModel.affiliateInvoices = _accountingService.GetAllAffiliateInvoices(affiliateId, dateFrom, dateTo, status);
            
            affiliateInvoicesModel.approvedInvoices = 0;
            affiliateInvoicesModel.payments = 0;
            affiliateInvoicesModel.outstanding = 0;

            foreach (AffiliateInvoice ai in affiliateInvoicesModel.affiliateInvoices)
            {
                if (ai.Status == 1 || ai.Status == 2)
                {
                    affiliateInvoicesModel.approvedInvoices += (ai.Sum - ai.Refunded + ai.Adjustment);
                }
            }

            IList<BuyerBalanceView> bbvList = this._accountingService.GetAffiliatesBalance(affiliateId, dateFrom, dateTo);
            foreach (BuyerBalanceView bbv in bbvList)
            {
                affiliateInvoicesModel.payments += (double)bbv.PaymentSum;
            }

            affiliateInvoicesModel.outstanding = affiliateInvoicesModel.payments - affiliateInvoicesModel.approvedInvoices;

            return Ok(affiliateInvoicesModel);
        }

        [HttpGet]
        [Route("getAffiliateInvoice/{id}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAffiliateInvoice(long id)
        {
            AffiliateInvoice bi = this._accountingService.GetAffiliateInvoiceById(id);
            AffiliateInvoiceModel affModel = new AffiliateInvoiceModel();


            affModel.Status = bi.Status;
            affModel.AffiliateId = bi.AffiliateId;
            affModel.DateCreated = bi.DateCreated.ToShortDateString();
            affModel.DateFrom = bi.DateFrom.ToShortDateString();
            affModel.DateTo = bi.DateTo.ToShortDateString();
            affModel.Id = bi.Id;
            affModel.Number = bi.AffiliateId.ToString() + "." + bi.Number.ToString();
            affModel.Sum = bi.Sum;
            affModel.UserID = bi.UserID;


            affModel.AffiliateInvoiceDetailsList = this._accountingService.GetAffiliateInvoiceDetails(id);

            affModel.AffiliateRefundedLeadsList = this._accountingService.GetBuyerRefundedLeadsById(id);

            affModel.AffiliateInvoiceAdjustmentsList = this._accountingService.GetAffiliateAdjustments(id);

            affModel.Total = 0;
            affModel.RefundedTotal = 0;
            foreach (RefundedLeads rl in affModel.AffiliateRefundedLeadsList)
            {
                affModel.RefundedTotal += rl.APrice;
            }

            affModel.AdjustmentTotal = 0;
            foreach (AffiliateInvoiceAdjustment bia in affModel.AffiliateInvoiceAdjustmentsList)
            {
                affModel.AdjustmentTotal += (decimal)bia.Sum;
            }

            affModel.Total = (decimal)affModel.Sum - (decimal)affModel.RefundedTotal + (decimal)affModel.AdjustmentTotal;

            affModel.CompanyName = this._settingService.GetSetting("Settings.CompanyName").Value;
            affModel.CompanyAddress = this._settingService.GetSetting("Settings.CompanyAddress").Value;
            affModel.CompanyBank = this._settingService.GetSetting("Settings.CompanyBank").Value;
            affModel.CompanyEmail = this._settingService.GetSetting("Settings.CompanyEmail").Value;
            affModel.CompanyLogoPath = this._settingService.GetSetting("Settings.CompanyLogoPath").Value;

            affModel.affiliate = this._affiliateService.GetAffiliateById(bi.AffiliateId, false);

            return Ok(affModel);
        }

        [HttpPost]
        [Route("approveAffiliateInvoice/{id}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult ApproveAffiliateInvoice(long id)
        {
            _accountingService.ApproveAffiliateInvoice(id);
            return Ok(1);
        }

        [HttpPost]
        [Route("disapproveAffiliateInvoice/{id}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult DisapproveAffiliateInvoice(long id)
        {
            if (_appContext.AppUser.UserTypeId == (long)SharedData.AffiliateUserTypeId)
            {
                return null;
            }

            _accountingService.AffiliateInvoiceChangeStatus(id, 0);
            return Ok(1);
        }

        [HttpPost]
        [Route("closeAffiliateInvoice/{id}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult CloseAffiliateInvoice(long id)
        {
            if (_appContext.AppUser.UserTypeId == (long)SharedData.AffiliateUserTypeId)
            {
                return null;
            }

            AffiliateInvoice aInvoice = _accountingService.GetAffiliateInvoiceById(id);
            if (aInvoice == null)
            {
                return HttpBadRequest("Not found");
            }
            _accountingService.AddAffiliateInvoicePayment(id, aInvoice.Sum);
            _accountingService.AffiliateInvoiceChangeStatus(id, 2);

            return Ok(1);
        }


        [HttpPost]
        [Route("disableAffiliateInvoice/{id}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult DisableAffiliateInvoice(long id)
        {
            _accountingService.DisableAffiliateInvoice(id);
            return Ok(1);
        }

        [HttpPost]
        [Route("reopenAffiliateInvoice/{id}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult ReopenAffiliateInvoice(long id)
        {
            if (_appContext.AppUser.UserTypeId == (long)SharedData.AffiliateUserTypeId)
            {
                return null;
            }

            _accountingService.AffiliateInvoiceChangeStatus(id, 1);
            return Ok(1);
        }

        [HttpPost]
        [Route("addAffiliateInvoiceAdjustment")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult AddAffiliateInvoiceAdjustment(
                                                    [FromUri] long affiliateInvoiceId,
                                                    [FromUri] string name,
                                                    [FromUri] double price,
                                                    [FromUri] int quantity)
        {
            _accountingService.AddAffiliateInvoiceAdjustment(affiliateInvoiceId, name, price, quantity);
            return Ok(1);
        }

        [HttpPost]
        [Route("deleteAffiliateInvoiceAdjustment/{adjustmentId}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult DeleteAffiliateInvoiceAdjustment(long adjustmentId)
        {
            _accountingService.DeleteAffiliateInvoiceAdjustment(adjustmentId);
            return Ok(1);
        }

        [HttpGet]
        [Route("getAffiliatesBalance")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAffiliatesBalance([FromUri] long affiliateId, // 0=All Affiliates, -1=Only Affiliate with Activity, -2=Only affiliates with not Activity
                                                    [FromUri] DateTime dateFrom,
                                                    [FromUri] DateTime dateTo)
        {
            AffiliateBalanceOutModel affiliateBalanceOut = new AffiliateBalanceOutModel();

            long BuyerIdFilter = affiliateId;
            if (affiliateId < 0) affiliateId = 0;

            if (dateFrom == null)
            {
                dateFrom = new DateTime(2000, 1, 1);
            }

            if (dateTo == null)
            {
                dateTo = DateTime.UtcNow;
            }
            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);

            DateTime dateToForAfter = _settingService.GetUTCDate(dateFrom);
            dateFrom = _settingService.GetUTCDate(dateFrom);
            dateTo = _settingService.GetUTCDate(dateTo);

            IList<BuyerBalanceView> bbvAftter = this._accountingService.GetAffiliatesBalance(affiliateId, new DateTime(2000, 1, 1), dateToForAfter);
            IList<BuyerBalanceView> bbv = this._accountingService.GetAffiliatesBalance(affiliateId, dateFrom, dateTo);

            decimal balanceAfter = 0;
            foreach (BuyerBalanceView bb in bbv)
            {
                foreach (BuyerBalanceView bAfter in bbvAftter)
                {
                    if (bb.BuyerId == bAfter.BuyerId)
                    {
                        balanceAfter = bAfter.Balance;
                        break;
                    }
                }

                long isAllNull = Convert.ToInt64(balanceAfter + bb.SoldSum + bb.InvoicedSum + bb.PaymentSum);

                if (BuyerIdFilter == -1 && isAllNull == 0)
                {
                    continue;
                }

                if (BuyerIdFilter == -2 && isAllNull != 0)
                {
                    continue;
                }


                affiliateBalanceOut.AffiliateBalances.Add(new AffiliateBalanceRow
                {
                    Balance = bb.Balance,
                    AffiliateId = bb.BuyerId,
                    AffiliateName = bb.Name,
                    Credit = bb.Credit,
                    FinalBalance = balanceAfter + bb.Balance,
                    InitialBalance = balanceAfter,
                    InvoiceSum = bb.InvoicedSum,
                    PaymentSum = bb.PaymentSum,
                    SoldSum = bb.SoldSum
                });

                affiliateBalanceOut.TotalInitialBalance += balanceAfter;
                affiliateBalanceOut.TotalSoldSum += bb.SoldSum;
                affiliateBalanceOut.TotalInvoiceSum += bb.InvoicedSum;
                affiliateBalanceOut.TotalPaymentSum += bb.PaymentSum;
                affiliateBalanceOut.TotalBalance += bb.Balance;
                affiliateBalanceOut.TotalFinalBalance += balanceAfter + bb.Balance;

            }

            return Ok(affiliateBalanceOut);
        }

        [HttpPost]
        [Route("sendEmailInvoiceToAffiliate/{invoiceId}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult SendEmailInvoiceToAffiliate(long invoiceId)
        {
            var smtpSetting = this._smtpAccountService.GetSmtpAccount();
            var techContact = _settingService.GetSetting("AppSetting.SupportEmail")?.Value ?? "no-reply@adrack.com";
            var user = _appContext.AppUser;
            var body = new StringBuilder();

            AffiliateInvoice ai = _accountingService.GetAffiliateInvoiceById(invoiceId);

            Affiliate affiliate = _AffiliateRepository.GetById(ai.AffiliateId);

            string PdfFileUrl = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/Uploads/Pdf/Invoice_" + ai.Number + ".pdf";

            body.AppendLine($"Invoice: <a href='{PdfFileUrl}'>Download</a> ");

            _emailService.SendEmail(smtpSetting, techContact, "Adrack Support", affiliate.Email, affiliate.Name, "Invoice #" + ai.AffiliateId.ToString() + "." + ai.Number.ToString(), body.ToString());

            return Ok(1);
        }

        [HttpPost]
        [Route("createBlankInvoiceAffiliate")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult CreateBlankInvoiceAffiliate([FromUri] long affiliateId,
                                                             [FromUri] DateTime dateTo)
        {
            DateTime dateToUtc = _settingService.GetUTCDate(dateTo);

            long retId = this._accountingService.CreateAffiliateInvoice(affiliateId, dateToUtc);

            return Ok(retId);
        }

        #endregion


        #region Payments

        [HttpGet]
        [Route("getBuyerPayments")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetBuyerPayments(  [FromUri] DateTime dateFrom,
                                                    [FromUri] DateTime dateTo,
                                                    [FromUri] short paymentMethod = 0,
                                                    [FromUri] string keyword = "")
        {
            if (dateFrom == null)
            {
                dateFrom = DateTime.MinValue;
            }

            if (dateTo == null)
            {
                dateTo = DateTime.UtcNow;
            }

            dateFrom = _settingService.GetUTCDate(dateFrom);
            dateTo = _settingService.GetUTCDate(dateTo);

            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);

            List<BuyerPaymentView> buyerPayments = new List<BuyerPaymentView>();

            var allBuyerPayments = (List<BuyerPaymentView>)this._accountingService.GetAllBuyerPayments(paymentMethod, keyword);

            foreach (var item in allBuyerPayments)
            {
                if (dateFrom <= item.Created && item.Created <= dateTo)
                    buyerPayments.Add(item);
            }

            return Ok(buyerPayments);
        }

        [HttpPost]
        [Route("addBuyerPayment")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult AddBuyerPayment([FromBody] BuyerPaymentInModel buyerPaymentModel)
        {
            BuyerPayment buyerPayment = new BuyerPayment();
            buyerPayment.Id = 0;
            buyerPayment.BuyerId = buyerPaymentModel.BuyerId;
            buyerPayment.Amount = buyerPaymentModel.Amount;
            buyerPayment.PaymentDate = _settingService.GetUTCDate(buyerPaymentModel.PaymentDate);
            buyerPayment.UserID = 0;
            buyerPayment.Created = DateTime.UtcNow;
            buyerPayment.Note = buyerPaymentModel.Note;
            buyerPayment.PaymentMethod = buyerPaymentModel.PaymentMethod;

            int retVal = (int)_accountingService.InsertBuyerPayment(buyerPayment);


            BuyerBalance bb = new BuyerBalance();

            bb.BuyerId = buyerPayment.BuyerId;
            bb.PaymentSum = (decimal)buyerPayment.Amount;

            this._accountingService.UpdateBuyerBalance(bb, "PaymentSum");

            return Ok(retVal);
        }

        [HttpPost]
        [Route("deleteBuyerPayment/{id}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult DeleteBuyerPayment(long id)
        {
            this._accountingService.DeleteBuyerPayment(id);

            return Ok(1);
        }

        [HttpPost]
        [Route("editBuyerPayment")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult EditBuyerPayment([FromBody] BuyerPaymentInModel buyerPaymentModel)
        {
            if (buyerPaymentModel == null)
                return HttpBadRequest(null);

            if (buyerPaymentModel.Id <= 0)
                return HttpBadRequest($"Please enter a valid ID");
            
            BuyerPayment buyerPayment = new BuyerPayment();
            buyerPayment.Id = buyerPaymentModel.Id;
            buyerPayment.BuyerId = buyerPaymentModel.BuyerId;
            buyerPayment.Amount = buyerPaymentModel.Amount;
            buyerPayment.PaymentDate = _settingService.GetUTCDate(buyerPaymentModel.PaymentDate);
            buyerPayment.UserID = 0;
            buyerPayment.Created = DateTime.UtcNow;
            buyerPayment.Note = buyerPaymentModel.Note;
            buyerPayment.PaymentMethod = buyerPaymentModel.PaymentMethod;

            this._accountingService.EditBuyerPayment(buyerPayment);

            return Ok(1);
        }

        [HttpGet]
        [Route("getBuyersBalance")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetBuyersBalance(  [FromUri] long buyerId, // 0=All Buyers, -1=Only Buyers with Activity, -2=Only Buyers with not Activity, 
                                                    [FromUri] DateTime dateFrom,
                                                    [FromUri] DateTime dateTo)
        {

            BuyerBalanceOutModel buyerBalanceOut = new BuyerBalanceOutModel();
            

            long BuyerIdFilter = buyerId;
            if (buyerId < 0) buyerId = 0;

            if (dateFrom == null)
            {
                dateFrom = new DateTime(2000, 1, 1);
            }

            if (dateTo == null)
            {
                dateTo = DateTime.UtcNow;
            }
            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);

            DateTime dateToForAfter = _settingService.GetUTCDate(dateFrom);
            dateFrom = _settingService.GetUTCDate(dateFrom);
            dateTo = _settingService.GetUTCDate(dateTo);

            IList<BuyerBalanceView> bbvAftter = this._accountingService.GetBuyersBalance(buyerId, new DateTime(2000, 1, 1), dateToForAfter);
            IList<BuyerBalanceView> bbv = this._accountingService.GetBuyersBalance(buyerId, dateFrom, dateTo);

            decimal balanceAfter = 0;
            foreach (BuyerBalanceView bb in bbv)
            {
                foreach (BuyerBalanceView bAfter in bbvAftter)
                {
                    if (bb.BuyerId == bAfter.BuyerId)
                    {
                        balanceAfter = bAfter.Balance;
                        break;
                    }
                }

                long isAllNull = Convert.ToInt64(balanceAfter + bb.SoldSum + bb.InvoicedSum + bb.PaymentSum);

                if (BuyerIdFilter == -1 && isAllNull == 0)
                {
                    continue;
                }

                if (BuyerIdFilter == -2 && isAllNull != 0)
                {
                    continue;
                }


                buyerBalanceOut.BuyerBalances.Add( new BuyerBalanceRow
                                        {
                                            Balance = bb.Balance,
                                            BuyerId = bb.BuyerId,
                                            BuyerName = bb.Name,
                                            Credit = bb.Credit,
                                            FinalBalance = balanceAfter + bb.Balance,
                                            InitialBalance = balanceAfter,
                                            InvoiceSum = bb.InvoicedSum,
                                            PaymentSum = bb.PaymentSum,
                                            SoldSum = bb.SoldSum
                                        } );

                buyerBalanceOut.TotalInitialBalance += balanceAfter;
                buyerBalanceOut.TotalSoldSum += bb.SoldSum;
                buyerBalanceOut.TotalInvoiceSum += bb.InvoicedSum;
                buyerBalanceOut.TotalPaymentSum += bb.PaymentSum;
                buyerBalanceOut.TotalBalance += bb.Balance;
                buyerBalanceOut.TotalFinalBalance += balanceAfter + bb.Balance;

            }

            return Ok(buyerBalanceOut);
        }

        #endregion

        [HttpGet]
        [Route("getRefundedLeads")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetRefundedLeads(  [FromUri] int status, // Statuses: -1 All, 0 Pending, 1 Approve, 2 Reject 
                                                    [FromUri] DateTime fromDate,
                                                    [FromUri] DateTime toDate,
                                                    [FromUri] string keyword = "")
        {
            List<RefundedLeads> refundedLeads = (List<RefundedLeads>)this._accountingService.GetAllRefundedLeads(status, fromDate, toDate, keyword);

            return Ok(refundedLeads);
        }

        [HttpGet]
        [Route("changeRefundedStatus")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult ChangeRefundedStatus(  [FromUri] long id,
                                                        [FromUri] byte status,
                                                        [FromUri] string note)
        {

            int retVal = _accountingService.ChangeRefundedStatus(id, status, note);

            return Ok(retVal);
        }

        [HttpGet]
        [Route("generateBuyerCustomInvoice")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GenerateBuyerCustomInvoice( [FromUri] long buyerId,
                                                             [FromUri] DateTime toDate)
        {
            long retId = 0;

            toDate = _settingService.GetUTCDate(toDate);

            retId = _accountingService.GenerateBuyerInvoices(buyerId, null, toDate, _appContext.AppUser.Id);

            return Ok(retId);
        }

        [HttpGet]
        [Route("generateAffiliateCustomInvoice")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GenerateAffiliateCustomInvoice( [FromUri] long affiliateId,
                                                                 [FromUri] DateTime toDate)
        {
            long retId = 0;

            toDate = _settingService.GetUTCDate(toDate);

            retId = _accountingService.GenerateAffiliateInvoices(affiliateId, null, toDate, _appContext.AppUser.Id);

            return Ok(retId);
        }

        [HttpGet]
        [Route("autoGenerateInvoices")]
        public IHttpActionResult AutoGenerateInvoices([FromUri] string pass)
        {
            if (pass == null || (pass != null && pass != "passw0rd"))
            {
                return null;
            }

            //m=0, w=1, bw=2  
            int dayOfWeekNum = (int)_settingService.GetTimeZoneDate(DateTime.UtcNow).DayOfWeek;
            int today = _settingService.GetTimeZoneDate(DateTime.UtcNow).Day;

            // For Buyers
            var buyersQuery = from c in _BuyerRepository.Table
                              where (c.BillFrequency == "1" && c.FrequencyValue == (dayOfWeekNum)) || (c.BillFrequency == "0" && c.FrequencyValue == today) ||
                              (c.BillFrequency == "2" && c.FrequencyValue == (dayOfWeekNum))
                              orderby c.Id
                              select c;

            IList<Buyer> buyers = buyersQuery.ToList();

            foreach (Buyer b in buyers)
            {
                if (b.BillFrequency == "2" && (!b.IsBiWeekly.HasValue || (b.IsBiWeekly.HasValue && !b.IsBiWeekly.Value)))
                {
                    continue;
                }

                DateTime dateTo;
                dateTo = DateTime.UtcNow;

                long retId = (long)this._accountingService.GenerateBuyerInvoices(b.Id, null, dateTo, 0);
                
                SendEmailInvoiceToBuyer(retId);
            }

            foreach (Buyer b in buyers)
            {
                if (b.BillFrequency == "2")
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
                                  where (c.BillFrequency == "1" && c.FrequencyValue == (dayOfWeekNum)) || (c.BillFrequency == "0" && c.FrequencyValue == today) ||
                                  (c.BillFrequency == "2" && c.FrequencyValue == (dayOfWeekNum))
                                  orderby c.Id
                                  select c;

            IList<Affiliate> affiliates = affiliatesQuery.ToList();

            foreach (Affiliate a in affiliates)
            {
                if (a.BillFrequency == "2" && (!a.IsBiWeekly.HasValue || (a.IsBiWeekly.HasValue && !a.IsBiWeekly.Value)))
                {
                    continue;
                }

                DateTime dateTo;
                dateTo = DateTime.UtcNow;

                long retId = (long)this._accountingService.GenerateAffiliateInvoices(a.Id, null, dateTo, 0);
                SendEmailInvoiceToAffiliate(retId);
            }

            foreach (Affiliate a in affiliates)
            {
                if (a.BillFrequency == "2")
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

            _leadMainService.ClearSensitiveData();

            return Ok(1);
        }


        [HttpPost]
        [Route("saveCusomInvoice")]
        public IHttpActionResult SaveCusomInvoice([FromBody] CustomInvoiceInModel customInvoice)
        {
            CustomInvoice cInvoice = new CustomInvoice();
            cInvoice.address = customInvoice.address;
            cInvoice.billingAddress = customInvoice.billingAddress;
            cInvoice.billingContactInformation = customInvoice.billingContactInformation;
            cInvoice.billingFullName = customInvoice.billingFullName;
            cInvoice.billingPeriod = customInvoice.billingPeriod;
            cInvoice.contactInformation = customInvoice.contactInformation;
            cInvoice.dateOfDue = customInvoice.dateOfDue;
            cInvoice.dateOfIssue = customInvoice.dateOfIssue;
            cInvoice.total  = customInvoice.total;
            cInvoice.website = customInvoice.website;

            long retVal = _accountingService.CreateCustomInvoice(cInvoice);

            foreach (row r in customInvoice.rows)
            {
                _accountingService.CreateCustomInvoiceRow(
                    new CustomInvoiceRow
                    {
                        CustomInvoiceId = retVal,
                        amount = r.amount,
                        description = r.description,
                        qty = r.qty,
                        unitPrice = r.unitPrice
                    }
                );
            }

            return Ok(retVal);
        }


        [HttpGet]
        [Route("getCusomInvoices")]
        public IHttpActionResult getCusomInvoices()
        {
            List<CustomInvoice> customInvoices = _accountingService.GetCustomInvoices();
            List<row> customInvoiceRow = new List<row>();

            List<CustomInvoiceInModel> customInvoiceList = new List<CustomInvoiceInModel>();

            foreach (CustomInvoice ci in customInvoices)
            {
                List<CustomInvoiceRow> customInvoiceRows = _accountingService.GetCustomInvoiceRows(ci.Id);
                
                foreach (CustomInvoiceRow cir in customInvoiceRows)
                {
                    customInvoiceRow.Add(new row()
                        {
                            amount = cir.amount,
                            customInvoiceId = cir.CustomInvoiceId,
                            description = cir.description,
                            qty = cir.qty,
                            unitPrice = cir.unitPrice
                        }
                    );
                }

                customInvoiceList.Add(new CustomInvoiceInModel()
                    {
                        address = ci.address,
                        billingAddress = ci.billingAddress,
                        billingContactInformation = ci.billingContactInformation,
                        billingFullName = ci.billingFullName,
                        billingPeriod = ci.billingPeriod,
                        contactInformation = ci.contactInformation,
                        dateOfDue = ci.dateOfDue,
                        dateOfIssue = ci.dateOfIssue,
                        total = ci.total,
                        website = ci.website,
                        Id = ci.Id,
                        rows = customInvoiceRow
                }
                );
            }
            
            return Ok(customInvoiceList);
        }


        [HttpPost]
        [Route("addRefundLeads")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult AddRefundLeads([FromBody] AddRefundLeadsModel addRefundLeadsModel)
        {
            RefundedLeads refLeads = new RefundedLeads();
            refLeads.Id = 0;
            refLeads.Approved = 0;
            refLeads.DateCreated = DateTime.UtcNow;
            refLeads.LeadId = addRefundLeadsModel.LeadId;
            refLeads.Reason = addRefundLeadsModel.Note;

            long buyerId = addRefundLeadsModel.BuyerId;
            LeadMainResponse lmr = this._leadMainResponseService.GetLeadMainResponsesByLeadIdBuyerId(refLeads.LeadId, buyerId);

            int retVal = 0;

            if (lmr != null)
            {
                refLeads.BPrice = lmr.BuyerPrice;
                refLeads.APrice = lmr.AffiliatePrice;

                retVal = (int)this._accountingService.InsertRefundedLeads(refLeads);
            }

            return Ok(new { refundedLeadId = retVal });
        }


        #region Pdf Generation

        [HttpGet]
        [Route("generateBuyerInvoicePdf")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GenerateBuyerInvoicePdf(long id)
        {
            BuyerInvoice bi = this._accountingService.GetBuyerInvoiceById(id);
            BuyerInvoiceModel biModel = new BuyerInvoiceModel();

            biModel.Status = bi.Status;
            biModel.BuyerId = bi.BuyerId;
            biModel.DateCreated = bi.DateCreated.ToShortDateString();
            biModel.DateFrom = bi.DateFrom.ToShortDateString();
            biModel.DateTo = bi.DateTo.ToShortDateString();
            biModel.Id = bi.Id;
            biModel.Number = bi.BuyerId.ToString() + "." + bi.Number.ToString();
            biModel.Sum = bi.Sum;
            biModel.UserID = bi.UserID;

            biModel.BuyerInvoiceDetailsList = this._accountingService.GetBuyerInvoiceDetails(id);
            biModel.BuyerRefundedLeadsList = this._accountingService.GetBuyerRefundedLeadsById(id);
            biModel.BuyerInvoiceAdjustmentsList = this._accountingService.GetBuyerAdjustments(id);

            biModel.Total = 0;
            biModel.RefundedTotal = 0;
            foreach (RefundedLeads rl in biModel.BuyerRefundedLeadsList)
            {
                biModel.RefundedTotal += rl.BPrice;
            }

            biModel.AdjustmentTotal = 0;
            foreach (BuyerInvoiceAdjustment bia in biModel.BuyerInvoiceAdjustmentsList)
            {
                biModel.AdjustmentTotal += (decimal)bia.Sum;
            }

            biModel.Total = (decimal)biModel.Sum - (decimal)biModel.RefundedTotal + (decimal)biModel.AdjustmentTotal;

            biModel.CompanyName = this._settingService.GetSetting("Settings.CompanyName").Value;
            biModel.CompanyAddress = this._settingService.GetSetting("Settings.CompanyAddress").Value;
            biModel.CompanyBank = this._settingService.GetSetting("Settings.CompanyBank").Value;
            biModel.CompanyEmail = this._settingService.GetSetting("Settings.CompanyEmail").Value;
            biModel.CompanyLogoPath = this._settingService.GetSetting("Settings.CompanyLogoPath").Value;

            Buyer buyer = this._buyerService.GetBuyerById(bi.BuyerId);
            Country country = this._countryService.GetCountryById(buyer.CountryId);
            biModel.BuyerCountryName = country.Name;

            string PdfFileUrl = "/Uploads/Invoice_" + biModel.Number + ".pdf";

            var doc = new iTextSharp.text.Document();
            //doc.SetMargins(0, 0, 0, 0);

            PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "/Uploads/Invoice_" + biModel.Number + ".pdf", FileMode.Create));
            doc.Open();
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(System.AppDomain.CurrentDomain.BaseDirectory + "/Uploads/adrack-logo.png");
            jpg.ScalePercent(20f);

            iTextSharp.text.Font fontH1 = FontFactory.GetFont("arial", 16); fontH1.SetStyle("Bold");
            iTextSharp.text.Font fontH2 = FontFactory.GetFont("arial", 14); fontH2.SetStyle("Bold");
            iTextSharp.text.Font fontText = FontFactory.GetFont("arial", 10);

            iTextSharp.text.BaseColor color1 = new iTextSharp.text.BaseColor(220, 220, 250);



            PdfPTable tableHead = new PdfPTable(2) { TotalWidth = 100f };
            float[] widths = new float[] { 70f, 30f };
            tableHead.SetTotalWidth(widths);


            PdfPCell[] cellsHead = new PdfPCell[] {
                                        new PdfPCell( jpg ) {  Border = 0, VerticalAlignment = Element.ALIGN_LEFT },
                                        new PdfPCell(
                                                new Paragraph("INVOICE #" + biModel.Number, fontH1) ) { Border = 0, VerticalAlignment = Element.ALIGN_RIGHT }
                                };

            PdfPRow rowHead = new PdfPRow(cellsHead);
            tableHead.Rows.Add(rowHead);

            cellsHead = new PdfPCell[] {
                                       new PdfPCell( new Paragraph( biModel.CompanyName + "\n" +
                                                                    biModel.CompanyAddress + "\n" +
                                                                    biModel.CompanyBank + "\n" +
                                                                    biModel.CompanyEmail, fontText) ) { Border = 0, VerticalAlignment = Element.ALIGN_LEFT},
                                    new PdfPCell(
                                                new Paragraph(  "Date: " + biModel.DateCreated + " \n" +
                                                                "Date From: " + biModel.DateFrom + "\n" +
                                                                "Date To: " + biModel.DateTo + "\n" +
                                                                "Total Due: " + String.Format("{0:$#.00}", biModel.Total)
                                                                , fontText )
                                        ) { Border = 0, VerticalAlignment = Element.ALIGN_RIGHT }
                                };

            rowHead = new PdfPRow(cellsHead);
            tableHead.Rows.Add(rowHead);
            doc.Add(tableHead);


            PdfPTable table = new PdfPTable(4);
            widths = new float[] { 68f, 20f, 12f, 20f };
            table.SetTotalWidth(widths);

            doc.Add(new Paragraph("\n", fontH2));

            PdfPCell[] cells = new PdfPCell[] {
                                    new PdfPCell( new Phrase("Leads / Campaing\n\n", fontH2) ) { VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = color1 },
                                    new PdfPCell( new Phrase("Price", fontH2) ) {VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = color1 },
                                    new PdfPCell( new Phrase("Count", fontH2) ) {VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = color1 },
                                    new PdfPCell( new Phrase("Total", fontH2) ) {VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = color1 } };
            PdfPRow row = new PdfPRow(cells);
            table.Rows.Add(row);

            foreach (BuyerInvoiceDetails bid in biModel.BuyerInvoiceDetailsList)
            {
                cells = new PdfPCell[] {
                                    new PdfPCell( new Phrase(bid.CampaignId.ToString() + ". " + bid.CampaignName.ToString()) ),
                                    new PdfPCell( new Phrase(String.Format("{0:$#.00}", bid.BuyerPrice)) ) { HorizontalAlignment = Element.ALIGN_RIGHT },
                                    new PdfPCell( new Phrase(bid.BuyerLeadsCount.ToString()) ) { HorizontalAlignment = Element.ALIGN_CENTER} ,
                                    new PdfPCell( new Phrase(String.Format("{0:$#.00}", bid.BuyerSum)) ) { HorizontalAlignment = Element.ALIGN_RIGHT} };
                row = new PdfPRow(cells);
                table.Rows.Add(row);
            }

            cells = new PdfPCell[] {
                                    new PdfPCell( new Phrase( "Leads Subtotal ", fontH2) ),
                                    new PdfPCell( new Phrase( "" ) ) { HorizontalAlignment = Element.ALIGN_RIGHT },
                                    new PdfPCell( new Phrase( "" ) ) { HorizontalAlignment = Element.ALIGN_CENTER} ,
                                    new PdfPCell( new Phrase( String.Format("{0:$#.00}", biModel.Sum), fontH2) ) { HorizontalAlignment = Element.ALIGN_RIGHT} };
            row = new PdfPRow(cells);
            table.Rows.Add(row);

            doc.Add(table);


            // Refunded/
            if (biModel.BuyerRefundedLeadsList.Count > 0)
            {
                doc.Add(new Paragraph("\n", fontH2));

                table = new PdfPTable(4);
                widths = new float[] { 68f, 20f, 12f, 20f };
                table.SetTotalWidth(widths);


                cells = new PdfPCell[] {
                                    new PdfPCell( new Phrase("Refunded Leads\n\n", fontH2) ) { BackgroundColor = color1 },
                                    new PdfPCell( new Phrase("Price") ) { BackgroundColor = color1 },
                                    new PdfPCell( new Phrase("Count") ) {VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = color1 },
                                    new PdfPCell( new Phrase("Total") ) { BackgroundColor = color1 } };
                row = new PdfPRow(cells);
                table.Rows.Add(row);

                foreach (RefundedLeads rl in biModel.BuyerRefundedLeadsList)
                {
                    cells = new PdfPCell[] {
                                    new PdfPCell( new Phrase("Lead ID: #" + rl.LeadId.ToString() +" ("+ rl.Reason + ")") ),
                                    new PdfPCell( new Phrase( "-" + String.Format("{0:$#.00}", rl.BPrice) ) ) { HorizontalAlignment = Element.ALIGN_RIGHT },
                                    new PdfPCell( new Phrase("1") ) { HorizontalAlignment = Element.ALIGN_CENTER} ,
                                    new PdfPCell( new Phrase( "-" + String.Format("{0:$#.00}", rl.BPrice) ) ) { HorizontalAlignment = Element.ALIGN_RIGHT} };
                    row = new PdfPRow(cells);
                    table.Rows.Add(row);
                }
                cells = new PdfPCell[] {
                                    new PdfPCell( new Phrase( "Refunded Subtotal ", fontH2) ),
                                    new PdfPCell( new Phrase( "" ) ),
                                    new PdfPCell( new Phrase( "" ) ),
                                    new PdfPCell( new Phrase( String.Format("{0:$#.00}", biModel.RefundedTotal), fontH2) ) { HorizontalAlignment = Element.ALIGN_RIGHT} };
                row = new PdfPRow(cells);
                table.Rows.Add(row);

                doc.Add(table);
            }

            // Custom Adjustments //
            if (biModel.BuyerInvoiceAdjustmentsList.Count > 0)
            {
                doc.Add(new Paragraph("\n", fontH2));

                table = new PdfPTable(4);
                widths = new float[] { 68f, 20f, 12f, 20f };
                table.SetTotalWidth(widths);


                cells = new PdfPCell[] {
                                    new PdfPCell( new Phrase("Custom Adjustments\n\n", fontH2) ) { BackgroundColor = color1 },
                                    new PdfPCell( new Phrase("Price") ) { BackgroundColor = color1 },
                                    new PdfPCell( new Phrase("Count") ) {VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = color1 },
                                    new PdfPCell( new Phrase("Total") ) { BackgroundColor = color1 } };
                row = new PdfPRow(cells);
                table.Rows.Add(row);

                foreach (BuyerInvoiceAdjustment bia in biModel.BuyerInvoiceAdjustmentsList)
                {
                    cells = new PdfPCell[] {
                                    new PdfPCell( new Phrase( bia.Name ) ),
                                    new PdfPCell( new Phrase( String.Format("{0:$#.00}", bia.Price) ) ) { HorizontalAlignment = Element.ALIGN_RIGHT },
                                    new PdfPCell( new Phrase( bia.Qty.ToString() ) ) { HorizontalAlignment = Element.ALIGN_CENTER} ,
                                    new PdfPCell( new Phrase( String.Format("{0:$#.00}", @bia.Sum) ) ) { HorizontalAlignment = Element.ALIGN_RIGHT} };
                    row = new PdfPRow(cells);
                    table.Rows.Add(row);
                }

                cells = new PdfPCell[] {
                                    new PdfPCell( new Phrase( "Adjustments Subtotal ", fontH2) ),
                                    new PdfPCell( new Phrase( "" ) ),
                                    new PdfPCell( new Phrase( "" ) ),
                                    new PdfPCell( new Phrase( String.Format("{0:$#.00}", biModel.AdjustmentTotal), fontH2) ) { HorizontalAlignment = Element.ALIGN_RIGHT} };
                row = new PdfPRow(cells);
                table.Rows.Add(row);

                doc.Add(table);
            }

            doc.Add(new Paragraph("\n                 Total Due: " + String.Format("{0:$#.00}", biModel.Total) + " \n", fontH1));

            //doc.Add(new Paragraph("\nThank you for using Leadz. This invoice can be paid via PayPal, Bank transfer, Skrill or Payoneer. Payment is due within 30 days from the date of delivery. Late payment is possible, but with with a fee of 10% per month. Company registered in England \n", fontText));


            doc.Close();

            return Ok( Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/" + PdfFileUrl);
        }

        [HttpGet]
        [Route("generateAffiliateInvoicePdf")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GenerateAffiliateInvoicePdf(long id)
        {
            AffiliateInvoice bi = this._accountingService.GetAffiliateInvoiceById(id);
            AffiliateInvoiceModel affModel = new AffiliateInvoiceModel();


            affModel.Status = bi.Status;
            affModel.AffiliateId = bi.AffiliateId;
            affModel.DateCreated = bi.DateCreated.ToShortDateString();
            affModel.DateFrom = bi.DateFrom.ToShortDateString();
            affModel.DateTo = bi.DateTo.ToShortDateString();
            affModel.Id = bi.Id;
            affModel.Number = bi.AffiliateId.ToString() + "." + bi.Number.ToString();
            affModel.Sum = bi.Sum;
            affModel.UserID = bi.UserID;


            affModel.AffiliateInvoiceDetailsList = this._accountingService.GetAffiliateInvoiceDetails(id);

            affModel.AffiliateRefundedLeadsList = this._accountingService.GetBuyerRefundedLeadsById(id);

            affModel.AffiliateInvoiceAdjustmentsList = this._accountingService.GetAffiliateAdjustments(id);

            affModel.Total = 0;
            affModel.RefundedTotal = 0;
            foreach (RefundedLeads rl in affModel.AffiliateRefundedLeadsList)
            {
                affModel.RefundedTotal += rl.APrice;
            }

            affModel.AdjustmentTotal = 0;
            foreach (AffiliateInvoiceAdjustment bia in affModel.AffiliateInvoiceAdjustmentsList)
            {
                affModel.AdjustmentTotal += (decimal)bia.Sum;
            }

            affModel.Total = (decimal)affModel.Sum - (decimal)affModel.RefundedTotal + (decimal)affModel.AdjustmentTotal;

            affModel.CompanyName = this._settingService.GetSetting("Settings.CompanyName").Value;
            affModel.CompanyAddress = this._settingService.GetSetting("Settings.CompanyAddress").Value;
            affModel.CompanyBank = this._settingService.GetSetting("Settings.CompanyBank").Value;
            affModel.CompanyEmail = this._settingService.GetSetting("Settings.CompanyEmail").Value;
            affModel.CompanyLogoPath = this._settingService.GetSetting("Settings.CompanyLogoPath").Value;

            affModel.affiliate = this._affiliateService.GetAffiliateById(bi.AffiliateId, false);


            string PdfFileUrl = "/Uploads/A_Invoice_" + affModel.Number + ".pdf";

            var doc = new iTextSharp.text.Document();
            //doc.SetMargins(0, 0, 0, 0);

            PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "/Uploads/A_Invoice_" + affModel.Number + ".pdf", FileMode.Create));
            doc.Open();
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(System.AppDomain.CurrentDomain.BaseDirectory + "/Uploads/adrack-logo.png");
            jpg.ScalePercent(20f);

            iTextSharp.text.Font fontH1 = FontFactory.GetFont("arial", 16); fontH1.SetStyle("Bold");
            iTextSharp.text.Font fontH2 = FontFactory.GetFont("arial", 14); fontH2.SetStyle("Bold");
            iTextSharp.text.Font fontText = FontFactory.GetFont("arial", 10);

            iTextSharp.text.BaseColor color1 = new iTextSharp.text.BaseColor(220, 220, 250);



            PdfPTable tableHead = new PdfPTable(2) { TotalWidth = 100f };
            float[] widths = new float[] { 70f, 30f };
            tableHead.SetTotalWidth(widths);


            PdfPCell[] cellsHead = new PdfPCell[] {
                                        new PdfPCell( jpg ) {  Border = 0, VerticalAlignment = Element.ALIGN_LEFT },
                                        new PdfPCell(
                                                new Paragraph("INVOICE #" + affModel.Number, fontH1) ) { Border = 0, VerticalAlignment = Element.ALIGN_RIGHT }
                                };

            PdfPRow rowHead = new PdfPRow(cellsHead);
            tableHead.Rows.Add(rowHead);

            cellsHead = new PdfPCell[] {
                                       new PdfPCell( new Paragraph( affModel.CompanyName + "\n" +
                                                                    affModel.CompanyAddress + "\n" +
                                                                    affModel.CompanyBank + "\n" +
                                                                    affModel.CompanyEmail, fontText) ) { Border = 0, VerticalAlignment = Element.ALIGN_LEFT},
                                    new PdfPCell(
                                                new Paragraph(  "Date: " + affModel.DateCreated + " \n" +
                                                                "Date From: " + affModel.DateFrom + "\n" +
                                                                "Date To: " + affModel.DateTo + "\n" +
                                                                "Total Due: " + String.Format("{0:$#.00}", affModel.Total)
                                                                , fontText )
                                        ) { Border = 0, VerticalAlignment = Element.ALIGN_RIGHT }
                                };

            rowHead = new PdfPRow(cellsHead);
            tableHead.Rows.Add(rowHead);
            doc.Add(tableHead);


            PdfPTable table = new PdfPTable(4);
            widths = new float[] { 68f, 20f, 12f, 20f };
            table.SetTotalWidth(widths);

            doc.Add(new Paragraph("\n", fontH2));

            PdfPCell[] cells = new PdfPCell[] {
                                    new PdfPCell( new Phrase("Leads / Campaing\n\n", fontH2) ) { VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = color1 },
                                    new PdfPCell( new Phrase("Price", fontH2) ) {VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = color1 },
                                    new PdfPCell( new Phrase("Count", fontH2) ) {VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = color1 },
                                    new PdfPCell( new Phrase("Total", fontH2) ) {VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = color1 } };
            PdfPRow row = new PdfPRow(cells);
            table.Rows.Add(row);

            foreach (AffiliateInvoiceDetails bid in affModel.AffiliateInvoiceDetailsList)
            {
                cells = new PdfPCell[] {
                                    new PdfPCell( new Phrase(bid.CampaignId.ToString() + ". " + bid.CampaignName.ToString()) ),
                                    new PdfPCell( new Phrase(String.Format("{0:$#.00}", bid.AffiliatePrice)) ) { HorizontalAlignment = Element.ALIGN_RIGHT },
                                    new PdfPCell( new Phrase(bid.AffiliateLeadsCount.ToString()) ) { HorizontalAlignment = Element.ALIGN_CENTER} ,
                                    new PdfPCell( new Phrase(String.Format("{0:$#.00}", bid.AffiliateSum)) ) { HorizontalAlignment = Element.ALIGN_RIGHT} };
                row = new PdfPRow(cells);
                table.Rows.Add(row);
            }

            cells = new PdfPCell[] {
                                    new PdfPCell( new Phrase( "Leads Subtotal ", fontH2) ),
                                    new PdfPCell( new Phrase( "" ) ) { HorizontalAlignment = Element.ALIGN_RIGHT },
                                    new PdfPCell( new Phrase( "" ) ) { HorizontalAlignment = Element.ALIGN_CENTER} ,
                                    new PdfPCell( new Phrase( String.Format("{0:$#.00}", affModel.Sum), fontH2) ) { HorizontalAlignment = Element.ALIGN_RIGHT} };
            row = new PdfPRow(cells);
            table.Rows.Add(row);

            doc.Add(table);


            // Refunded/
            if (affModel.AffiliateRefundedLeadsList.Count > 0)
            {
                doc.Add(new Paragraph("\n", fontH2));

                table = new PdfPTable(4);
                widths = new float[] { 68f, 20f, 12f, 20f };
                table.SetTotalWidth(widths);


                cells = new PdfPCell[] {
                                    new PdfPCell( new Phrase("Refunded Leads\n\n", fontH2) ) { BackgroundColor = color1 },
                                    new PdfPCell( new Phrase("Price") ) { BackgroundColor = color1 },
                                    new PdfPCell( new Phrase("Count") ) {VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = color1 },
                                    new PdfPCell( new Phrase("Total") ) { BackgroundColor = color1 } };
                row = new PdfPRow(cells);
                table.Rows.Add(row);

                foreach (RefundedLeads rl in affModel.AffiliateRefundedLeadsList)
                {
                    cells = new PdfPCell[] {
                                    new PdfPCell( new Phrase("Lead ID: #" + rl.LeadId.ToString() +" ("+ rl.Reason + ")") ),
                                    new PdfPCell( new Phrase( "-" + String.Format("{0:$#.00}", rl.BPrice) ) ) { HorizontalAlignment = Element.ALIGN_RIGHT },
                                    new PdfPCell( new Phrase("1") ) { HorizontalAlignment = Element.ALIGN_CENTER} ,
                                    new PdfPCell( new Phrase( "-" + String.Format("{0:$#.00}", rl.BPrice) ) ) { HorizontalAlignment = Element.ALIGN_RIGHT} };
                    row = new PdfPRow(cells);
                    table.Rows.Add(row);
                }
                cells = new PdfPCell[] {
                                    new PdfPCell( new Phrase( "Refunded Subtotal ", fontH2) ),
                                    new PdfPCell( new Phrase( "" ) ),
                                    new PdfPCell( new Phrase( "" ) ),
                                    new PdfPCell( new Phrase( String.Format("{0:$#.00}", affModel.RefundedTotal), fontH2) ) { HorizontalAlignment = Element.ALIGN_RIGHT} };
                row = new PdfPRow(cells);
                table.Rows.Add(row);

                doc.Add(table);
            }

            // Custom Adjustments //
            if (affModel.AffiliateInvoiceAdjustmentsList.Count > 0)
            {
                doc.Add(new Paragraph("\n", fontH2));

                table = new PdfPTable(4);
                widths = new float[] { 68f, 20f, 12f, 20f };
                table.SetTotalWidth(widths);


                cells = new PdfPCell[] {
                                    new PdfPCell( new Phrase("Custom Adjustments\n\n", fontH2) ) { BackgroundColor = color1 },
                                    new PdfPCell( new Phrase("Price") ) { BackgroundColor = color1 },
                                    new PdfPCell( new Phrase("Count") ) {VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = color1 },
                                    new PdfPCell( new Phrase("Total") ) { BackgroundColor = color1 } };
                row = new PdfPRow(cells);
                table.Rows.Add(row);

                foreach (AffiliateInvoiceAdjustment bia in affModel.AffiliateInvoiceAdjustmentsList)
                {
                    cells = new PdfPCell[] {
                                    new PdfPCell( new Phrase( bia.Name ) ),
                                    new PdfPCell( new Phrase( String.Format("{0:$#.00}", bia.Price) ) ) { HorizontalAlignment = Element.ALIGN_RIGHT },
                                    new PdfPCell( new Phrase( bia.Qty.ToString() ) ) { HorizontalAlignment = Element.ALIGN_CENTER} ,
                                    new PdfPCell( new Phrase( String.Format("{0:$#.00}", @bia.Sum) ) ) { HorizontalAlignment = Element.ALIGN_RIGHT} };
                    row = new PdfPRow(cells);
                    table.Rows.Add(row);
                }

                cells = new PdfPCell[] {
                                    new PdfPCell( new Phrase( "Adjustments Subtotal ", fontH2) ),
                                    new PdfPCell( new Phrase( "" ) ),
                                    new PdfPCell( new Phrase( "" ) ),
                                    new PdfPCell( new Phrase( String.Format("{0:$#.00}", affModel.AdjustmentTotal), fontH2) ) { HorizontalAlignment = Element.ALIGN_RIGHT} };
                row = new PdfPRow(cells);
                table.Rows.Add(row);

                doc.Add(table);
            }

            doc.Add(new Paragraph("\n                 Total Due: " + String.Format("{0:$#.00}", affModel.Total) + " \n", fontH1));

            //doc.Add(new Paragraph("\nThank you for using Leadz. This invoice can be paid via PayPal, Bank transfer, Skrill or Payoneer. Payment is due within 30 days from the date of delivery. Late payment is possible, but with with a fee of 10% per month. Company registered in England \n", fontText));


            doc.Close();

            return Ok(Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/" + PdfFileUrl);
        }
        #endregion


        #endregion methods
    }
}