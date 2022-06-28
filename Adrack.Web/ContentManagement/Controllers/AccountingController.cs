// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AccountingController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Accounting;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Directory;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Web.ContentManagement.Models.Accounting;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Security;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

// using PdfSharp.Drawing;
// using PdfSharp.Pdf;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Adrack.Core.Cache;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Class AccountingController.
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public partial class AccountingController : BaseContentManagementController
    {
        #region Fields

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IAccountingService _accountingService;

        /// <summary>
        /// The affiliate service
        /// </summary>
        private readonly IAffiliateService _affiliateService;

        /// <summary>
        /// The buyer service
        /// </summary>
        private readonly IBuyerService _buyerService;

        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// The buyer repository
        /// </summary>
        private readonly IRepository<Buyer> _BuyerRepository;

        /// <summary>
        /// The affiliate repository
        /// </summary>
        private readonly IRepository<Affiliate> _AffiliateRepository;

        /// <summary>
        /// The country service
        /// </summary>
        private readonly ICountryService _countryService;

        /// <summary>
        /// The history service
        /// </summary>
        private readonly IHistoryService _historyService;

        /// <summary>
        /// The lead main response service
        /// </summary>
        private readonly ILeadMainResponseService _leadMainResponseService;

        /// <summary>
        /// The lead main service
        /// </summary>
        private readonly ILeadMainService _leadMainService;

        /// <summary>
        /// The payment methods
        /// </summary>
        public string[] PaymentMethods;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Directory Controller
        /// </summary>
        /// <param name="accountingService">The accounting service.</param>
        /// <param name="affiliateService">The affiliate service.</param>
        /// <param name="buyerService">The buyer service.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="appContext">Application Context</param>
        /// <param name="buyerRepository">The buyer repository.</param>
        /// <param name="affiliateRepository">The affiliate repository.</param>
        /// <param name="countryService">Country Service</param>
        /// <param name="historyService">The history service.</param>
        /// <param name="leadMainResponseService">The lead main response service.</param>
        /// <param name="leadMainService">The lead main service.</param>

        public AccountingController(
                                    IAccountingService accountingService,
                                    IAffiliateService affiliateService,
                                    IBuyerService buyerService,
                                    ISettingService settingService,
                                    IAppContext appContext,
                                    IRepository<Buyer> buyerRepository,
                                    IRepository<Affiliate> affiliateRepository,
                                    ICountryService countryService,
                                    IHistoryService historyService,
                                    ILeadMainResponseService leadMainResponseService,
                                    ILeadMainService leadMainService)
        {
            this._accountingService = accountingService;
            this._affiliateService = affiliateService;
            this._buyerService = buyerService;
            this._settingService = settingService;
            this._appContext = appContext;
            this._BuyerRepository = buyerRepository;
            this._AffiliateRepository = affiliateRepository;
            this._countryService = countryService;
            this._historyService = historyService;
            this._leadMainResponseService = leadMainResponseService;
            this._leadMainService = leadMainService;

            PaymentMethods = new string[] { "Cache", "Credit Card", "Wire", "Check" };
        }

        #endregion Constructor

        // GET: Accounting
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            //List<AffiliateInvoice> invoices = (List<AffiliateInvoice>)this._accountingService.GetAllAffiliateInvoices();
            return View();
        }

        /// <summary>
        /// Generates the action buttons.
        /// </summary>
        /// <param name="actionsJson">The actions json.</param>
        /// <returns>System.String.</returns>
        public string GenerateActionButtons(string actionsJson)
        {
            string actionsButtons = "";
            if (actionsJson != null)
            {
                dynamic json = JsonConvert.DeserializeObject(actionsJson);

                actionsButtons = "<ul class=\"icons-list\"><li class=\"dropdown\"><a data-toggle=\"dropdown\" class=\"dropdown-toggle\" href=\"#\" aria-expanded=\"true\"><i class=\"icon-menu9\"></i></a><ul class=\"dropdown-menu dropdown-menu-right\">";

                foreach (dynamic obj in json)
                {
                    actionsButtons += "<li><a class='" + (string)obj["Class"] + "' onclick=\"doButtonAction(1, '" + (string)obj["Url"] + "', " + obj["Confirm"] + ");\" data-toggle='modal' data-target='#" + (string)obj["Modal"] + "' href='#'><i class='" + obj["IconClass"] + "'></i> " + obj["Name"] + "</a></li>";
                }

                actionsButtons += "</ul></li></ul>";
            }
            return actionsButtons;
        }

        /// <summary>
        /// Affiliates the invoices.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Accounting / Payment Notice of Affiliate")]
        public ActionResult AffiliateInvoices()
        {
            /*
            List<AffiliateInvoice> invoices = (List<AffiliateInvoice>)this._accountingService.GetAllAffiliateInvoices(-2);

            AffiliateInvoiceModel model = new AffiliateInvoiceModel();
            model.invoices = invoices;
            */
            ViewBag.AllAffiliatesList = this._affiliateService.GetAllAffiliates();
            return View();
        }

        /// <summary>
        /// Buyers the invoices.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Accounting / Invoices of Buyers")]
        public ActionResult BuyerInvoices()
        {
            ViewBag.AllBuyersList = this._buyerService.GetAllBuyers();
            //AZ CreateReportPdf(1);
            return View(/*model*/);
        }

        /// <summary>
        /// Invoiceses this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Invoices()
        {
            if (Request["buyerid"] != null || _appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                ViewBag.EntityId = Request["buyerid"];
                ViewBag.SelectedBuyerId = Request["buyerid"];
            }
            else if (Request["affiliateid"] != null || _appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
            {
                ViewBag.SelectedAffiliateId = Request["affiliateid"];
                ViewBag.EntityId = Request["affiliateid"];
            }

            if (_appContext.AppUser.UserType != SharedData.NetowrkUserTypeId && _appContext.AppUser.UserType != SharedData.BuiltInUserTypeId)
            {
                ViewBag.EntityId = _appContext.AppUser.ParentId;
            }
            return View();
        }

        /// <summary>
        /// Buyers the invoices partial.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult BuyerInvoicesPartial(long Id)
        {
            ViewBag.EntityId = Id;
            ViewBag.TimeZoneNow = _settingService.GetTimeZoneDate(DateTime.UtcNow).ToShortDateString();

            return PartialView();
        }

        /// <summary>
        /// Affiliates the invoices partial.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult AffiliateInvoicesPartial(long Id)
        {
            ViewBag.AffiliateId = Id;
            ViewBag.TimeZoneNow = _settingService.GetTimeZoneDate(DateTime.UtcNow).ToShortDateString();

            return PartialView();
        }

        /// <summary>
        /// Buyers the invoice item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = false, Label = "Buyer Invoice")]
        public ActionResult BuyerInvoiceItem(long id)
        {
            BuyerInvoice bi = this._accountingService.GetBuyerInvoiceById(id);
            BuyerInvoiceModel biModel = new BuyerInvoiceModel();

            if (bi != null)
            {
                biModel.Status = bi.Status;
                biModel.BuyerId = bi.BuyerId;
                biModel.DateCreated = bi.DateCreated.ToShortDateString();
                biModel.DateFrom = bi.DateFrom.ToShortDateString();
                biModel.DateTo = bi.DateTo.ToShortDateString();
                biModel.Id = bi.Id;
                biModel.Number = bi.BuyerId.ToString() + "." + bi.Number.ToString();
                biModel.Sum = bi.Sum;
                biModel.UserID = bi.UserID;

                biModel.buyer = this._buyerService.GetBuyerById(bi.BuyerId);

                Country country = this._countryService.GetCountryById(biModel.buyer.CountryId);
                biModel.BuyerCountryName = country.Name;
            }

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

            return View(biModel);
        }

        /// <summary>
        /// Affiliates the invoice item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = false, Label = "Affiliate Invoice")]
        public ActionResult AffiliateInvoiceItem(long id)
        {
            AffiliateInvoice ai = this._accountingService.GetAffiliateInvoiceById(id);
            AffiliateInvoiceModel aiModel = new AffiliateInvoiceModel();

            if (ai != null)
            {
                aiModel.Adjustment = ai.Adjustment;
                aiModel.Status = ai.Status;
                aiModel.AffiliateId = ai.AffiliateId;
                aiModel.DateCreated = ai.DateCreated.ToShortDateString();
                aiModel.DateFrom = ai.DateFrom.ToShortDateString();
                aiModel.DateTo = ai.DateTo.ToShortDateString();
                aiModel.Id = ai.Id;
                aiModel.Number = ai.AffiliateId.ToString() + "." + ai.Number.ToString();
                aiModel.Refunded = ai.Refunded;
                aiModel.Sum = ai.Sum;
                aiModel.UserID = ai.UserID;

                aiModel.affiliate = this._affiliateService.GetAffiliateById(ai.AffiliateId, false);

                Country country = this._countryService.GetCountryById(aiModel.affiliate.CountryId);
                aiModel.AffiliateCountryName = country.Name;
            }

            aiModel.AffiliateInvoiceDetailsList = this._accountingService.GetAffiliateInvoiceDetails(id);

            aiModel.AffiliateRefundedLeadsList = this._accountingService.GetAffiliateRefundedLeadsById(id);

            aiModel.AffiliateInvoiceAdjustmentsList = this._accountingService.GetAffiliateAdjustments(id);

            aiModel.Total = 0;
            aiModel.RefundedTotal = 0;
            foreach (RefundedLeads rl in aiModel.AffiliateRefundedLeadsList)
            {
                aiModel.RefundedTotal += rl.APrice;
            }

            aiModel.AdjustmentTotal = 0;
            foreach (AffiliateInvoiceAdjustment bia in aiModel.AffiliateInvoiceAdjustmentsList)
            {
                aiModel.AdjustmentTotal += (decimal)bia.Sum;
            }

            aiModel.Total = (decimal)aiModel.Sum - (decimal)aiModel.RefundedTotal + (decimal)aiModel.AdjustmentTotal;

            aiModel.CompanyName = this._settingService.GetSetting("Settings.CompanyName").Value;
            aiModel.CompanyAddress = this._settingService.GetSetting("Settings.CompanyAddress").Value;
            aiModel.CompanyBank = this._settingService.GetSetting("Settings.CompanyBank").Value;
            aiModel.CompanyEmail = this._settingService.GetSetting("Settings.CompanyEmail").Value;
            aiModel.CompanyLogoPath = this._settingService.GetSetting("Settings.CompanyLogoPath").Value;

            ViewBag.UserTypeId = (this._appContext.AppUser != null ? this._appContext.AppUser.UserType : 0);

            return View(aiModel);
        }

        /// <summary>
        /// Gets the buyer invoices.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [ValidateInput(false)]
        public ActionResult GetBuyerInvoices()
        {
            if (this._appContext.AppUser == null || this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
            {
                return null;
            }

            DateTime dateFrom = new DateTime(1900, 1, 1);
            DateTime dateTo = new DateTime(2020, 1, 1);
            if (!string.IsNullOrEmpty(Request["dates"]))
            {
                var dates = Request["dates"].Split(':');
                if (dates[0] != "")
                {
                    dateFrom = Convert.ToDateTime(dates[0]);
                    dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
                }
                if (dates[1] != "")
                {
                    dateTo = Convert.ToDateTime(dates[1]);
                    dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);
                }
            }

            dateFrom = _settingService.GetUTCDate(dateFrom);
            dateTo = _settingService.GetUTCDate(dateTo);

            List<BuyerInvoice> invoices = new List<BuyerInvoice>();

            double InvoiceSum = 0;
            double ApprovedSum = 0;
            double PaidSum = 0;

            long buyerId = Request["buyerid"] != null ? long.Parse(Request["buyerid"]) : 0;

            if (Request["filterbuyerid"] != null)
            {
                buyerId = long.Parse(Request["filterbuyerid"]);
            }

            int statusFilter = Request["statfilter"] != null ? short.Parse(Request["statfilter"]) : -2; // All

            if (this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                invoices = (List<BuyerInvoice>)this._accountingService.GetBuyerInvoices(this._appContext.AppUser.ParentId, dateFrom, dateTo, -3);

                IList<BuyerBalanceView> bbvList = this._accountingService.GetBuyersBalance(buyerId, dateFrom, dateTo);
                foreach (BuyerBalanceView bbv in bbvList)
                {
                    PaidSum += (double)bbv.PaymentSum;
                }
            }
            else
            {
                invoices = (List<BuyerInvoice>)this._accountingService.GetBuyerInvoices(buyerId, dateFrom, dateTo, statusFilter);
                IList<BuyerBalanceView> bbvList = this._accountingService.GetBuyersBalance(buyerId, dateFrom, dateTo);
                foreach (BuyerBalanceView bbv in bbvList)
                {
                    PaidSum += (double)bbv.PaymentSum;
                }
            }

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = invoices.Count;
            jd.recordsFiltered = invoices.Count;
            jd.recordsSum = 0;

            string StatusStr = "";
            string DisabledClass = "";

            Setting tzSetting = _settingService.GetSetting("TimeZone");

            foreach (BuyerInvoice ai in invoices)
            {
                DisabledClass = "";
                switch (ai.Status)
                {
                    case 1:
                        {
                            StatusStr = "<span class=\"label status-label label-info\">Approved</span>";
                            break;
                        }
                    case 0:
                        {
                            StatusStr = "<span class=\"label status-label label-danger\">Not<br>Approved</span>";
                            break;
                        }

                    case 2:
                        {
                            StatusStr = "<span class=\"label status-label label-success\">Closed</span>";
                            break;
                        }
                    case -1:
                        {
                            StatusStr = "<span class=\"label status-label label-default\">Deleted</span>";
                            DisabledClass = " disabled-row ";
                            break;
                        }
                }

                double BuyerDistrib = this._accountingService.GetBuyerDistrib(ai.BuyerId);

                string BtnApprove = ai.Status == 0 ? " <button class='BtnApprove action-btn btn btn-info btn-xs' type='button' data-id='" + ai.Id.ToString() + "'>Approve</button> " : "";
                string BtnDisapprove = ai.Status == 1 ? " <button class='BtnDisapprove action-btn btn btn-warning btn-xs' type='button' data-id='" + ai.Id.ToString() + "'>Disapprove</button> " : "";
                string BtnPaid = ai.Status == 1 ? " <button class='BtnPaid action-btn btn btn-success btn-xs' type='button' data-id='" + ai.Id.ToString() + "' data-buyerid='" + ai.BuyerId.ToString() + "' data-total='" + (ai.Sum - ai.Refunded + ai.Adjustment).ToString() + "' data-distrib='" + BuyerDistrib + "'>Close</button> " : "";
                string BtnUnpaid = ai.Status == 2 ? " <button class='BtnUnpaid action-btn btn bg-pink-400 btn-xs' type='button' data-id='" + ai.Id.ToString() + "'>Unpaid</button> " : "";
                string BtnDelete = ai.Status == 0 ? " <button class='BtnDelete action-btn btn btn-danger btn-xs' type='button' data-id='" + ai.Id.ToString() + "'>Delete</button> " : "";
                string BtnDownload = ai.Status > 0 ? " <i class='BtnDownload glyphicon glyphicon-download-alt' data-id='" + ai.Id.ToString() + "'></i>" : "";

                /*
                IList<RefundedLeads> rlList = this._accountingService.GetBuyerRefundedLeadsById(ai.Id);
                ai.Refunded = 0;
                foreach( RefundedLeads rl in rlList )
                {
                    ai.Refunded += (double)rl.BPrice;
                }
                ai.Refunded = -ai.Refunded;
                */

                Buyer buyer = this._buyerService.GetBuyerById(ai.BuyerId);
                string BuyerName = "";
                if (buyer != null)
                {
                    BuyerName = buyer.Name;
                }
                /*
                IList<BuyerInvoiceAdjustment> biaList = this._accountingService.GetBuyerAdjustments(ai.Id);
                ai.Adjustment = 0;
                foreach (BuyerInvoiceAdjustment bia in biaList)
                {
                    ai.Adjustment += (double)bia.Sum;
                }
                */

                // InvoiceSum += (ai.Sum + ai.Refunded + ai.Adjustment);
                if (Request["buyerid"] != null)
                {
                    string[] names1 = {
                                      ai.Id.ToString(),
                                      "<div class='no-margin text-bold " + DisabledClass + "'><a href='/Management/Accounting/BuyerInvoiceItem/"+ai.Id+"'>"+ai.BuyerId.ToString() + "." + ai.Number.ToString() + "</a></div>",
                                      ai.DateFrom.ToShortDateString() + "<br>" + ai.DateTo.ToShortDateString(),
                                      _settingService.GetTimeZoneDate(ai.DateCreated, null, tzSetting).ToShortDateString(),
                                      "<div class=\"no-margin text-bold alignright" + DisabledClass+ "\">" + String.Format("{0:$###,###.00}", ai.Sum) + "</div>",
                                      "<div class=\"no-margin text-bold alignright"+ DisabledClass + "\">" + String.Format("{0:$###,###.00}", (ai.Sum - ai.Refunded + ai.Adjustment)) + "</div>"
                                };
                    if (ai.Status == 1 || ai.Status == 2)
                    {
                        ApprovedSum += (ai.Sum - ai.Refunded + ai.Adjustment);
                    }
                    jd.data.Add(names1);
                }
                else
                {
                    string[] names1 = {
                                      /* "<input type='checkbox' name='row_checkbox' class='row_checkbox' id='row_" + ai.Id.ToString() + "' />", */
                                      ai.Id.ToString(),
                                      "<div class='no-margin text-bold " + DisabledClass + "'><a href='/Management/Accounting/BuyerInvoiceItem/"+ai.Id+"'>"+ai.BuyerId.ToString() + "." + ai.Number.ToString() + "</a></div>",
                                      "<a href='/Management/Buyer/Item/"+ai.BuyerId.ToString()+"' title='" + ai.BuyerId.ToString() + "# " + BuyerName + "' data-buyerid='"+ai.BuyerId.ToString()+"'>" + BuyerName + "</a>",
                                      "<div style='text-align:center;'>" + ai.DateFrom.ToShortDateString() + "<br>" + ai.DateTo.ToShortDateString() + "</div>",
                                      "<div style='text-align:center;'>" + _settingService.GetTimeZoneDate(ai.DateCreated, null, tzSetting).ToShortDateString() + "</div>",
                                      /*
                                      "<div class=\"no-margin text-bold alignright" + DisabledClass+ "\">"+String.Format("{0:$###,###.00}", ai.Sum)+"</div>",
                                      ai.Refunded == 0 ? "" : "<div class=\"no-margin alignright"+ DisabledClass + "\">" + String.Format("{0:-$#.00}", ai.Refunded) + "</div>",
                                      ai.Adjustment == 0 ? "" : "<div class=\"no-margin alignright"+ DisabledClass + "\">"+String.Format("{0:$#.00}", ai.Adjustment) + "</div>",
                                      */
                                      "<div class=\"no-margin text-bold alignright"+ DisabledClass + "\">"+String.Format("{0:$###,###.00}", (ai.Sum - ai.Refunded + ai.Adjustment))+"</div>",
                                      /*
                                      ai.Paid == 0 ? "" : "<div class='alignright'>" + String.Format("{0:$###,###.00}", ai.Paid)+"</div>",
                                      "<div class='alignright'>" + Outstanding + "</div>",
                                      */

                                      "<div class=\"no-margin alignright"+ DisabledClass + "\">"+String.Format("{0:$###,###.00}", BuyerDistrib) + "</div>",
                                      "<div style='width:200px; text-align: center'>" + BtnApprove + BtnDisapprove + BtnUnpaid + BtnPaid + BtnDelete + "</div>",
                                      "<div style='text-align:center;'>" + StatusStr + "</div>",
                                      "<p style='text-align: center'>" + BtnDownload + "</p>"
                                };
                    if (ai.Status != -1)
                    {
                        jd.recordsSum += (ai.Sum - ai.Refunded + ai.Adjustment);
                    }

                    switch (ai.Status)
                    {
                        case 0:
                            {
                                InvoiceSum += (ai.Sum - ai.Refunded + ai.Adjustment);
                                break;
                            }

                        case 1:
                            {
                                InvoiceSum += (ai.Sum - ai.Refunded + ai.Adjustment);
                                ApprovedSum += (ai.Sum - ai.Refunded + ai.Adjustment);
                                break;
                            }

                        case 2:
                            {
                                InvoiceSum += (ai.Sum - ai.Refunded + ai.Adjustment);
                                ApprovedSum += (ai.Sum - ai.Refunded + ai.Adjustment);
                                break;
                            }
                    }

                    jd.data.Add(names1);
                }
            }

            jd.totalsSumStr[0] = String.Format("{0:$###,###.00}", ApprovedSum);
            jd.totalsSumStr[1] = String.Format("{0:$###,###.00}", PaidSum);
            jd.totalsSumStr[2] = String.Format("{0:$###,###.00}", (PaidSum - ApprovedSum));

            List<AffiliateInvoice> dtList = new System.Collections.Generic.List<AffiliateInvoice>();

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Disables the buyer invoice.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult DisableBuyerInvoice()
        {
            if (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            int retVal = (int)this._accountingService.DisableBuyerInvoice(long.Parse(Request["id"]));

            this._historyService.AddHistory("AccountingController", HistoryAction.Invoice_Deleted, "BuyerInvoice", long.Parse(Request["id"]), "", "", "", this._appContext.AppUser.Id);

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Disables the affiliate invoice.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult DisableAffiliateInvoice()
        {
            if (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            int retVal = (int)this._accountingService.DisableAffiliateInvoice(long.Parse(Request["id"]));

            this._historyService.AddHistory("AccountingController", HistoryAction.Invoice_Deleted, "AffiliateInvoice", long.Parse(Request["id"]), "", "", "", this._appContext.AppUser.Id);

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Approves the buyer invoice.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult ApproveBuyerInvoice()
        {
            if (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            string[] IDs = Request["id"].Split(',');

            foreach (string id in IDs)
            {
                if (id != "")
                {
                    this._accountingService.ApproveBuyerInvoice(long.Parse(id));
                    this._historyService.AddHistory("AccountingController", HistoryAction.Invoice_Status_Changed, "BuyerInvoice", long.Parse(id), "Status:Pending", "Status:Approved", "", this._appContext.AppUser.Id);
                }
            }

            //            int retVal = (int)this._accountingService.ApproveBuyerInvoice(long.Parse(Request["id"]));
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Approves the affiliate invoice.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult ApproveAffiliateInvoice()
        {
            if (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            string[] IDs = Request["id"].Split(',');

            foreach (string id in IDs)
            {
                if (id != "")
                {
                    this._accountingService.ApproveAffiliateInvoice(long.Parse(id));
                    this._historyService.AddHistory("AccountingController", HistoryAction.Invoice_Status_Changed, "AffiliateInvoice", long.Parse(id), "Status:Pending;", "Status:Approved;", "", this._appContext.AppUser.Id);
                }
            }
            //            int retVal = (int)this._accountingService.ApproveAffiliateInvoice(long.Parse(Request["id"]));
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Affiliates the invoice change status.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult AffiliateInvoiceChangeStatus()
        {
            if (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            if (Request["id"] == null || Request["id"] == null)
            {
                return null;
            }

            long ID = long.Parse(Request["id"]);

            short Status = short.Parse(Request["status"]);

            this._accountingService.AffiliateInvoiceChangeStatus(ID, Status);
            this._historyService.AddHistory("AccountingController", HistoryAction.Invoice_Status_Changed, "AffiliateInvoice", ID, "Status:Approved;", "Status:Pending;", "", this._appContext.AppUser.Id);

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Buyers the invoice change status.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult BuyerInvoiceChangeStatus()
        {
            if (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            if (Request["id"] == null || Request["id"] == null)
            {
                return null;
            }

            long ID = long.Parse(Request["id"]);

            short Status = short.Parse(Request["status"]);

            this._accountingService.BuyerInvoiceChangeStatus(ID, Status);
            this._historyService.AddHistory("AccountingController", HistoryAction.Invoice_Status_Changed, "BuyerInvoice", ID, "Status:Pending;", "Status:Approved;", "", this._appContext.AppUser.Id);

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the buyer invoice payment.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult AddBuyerInvoicePayment()
        {
            if (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            int retVal = (int)this._accountingService.AddBuyerInvoicePayment(long.Parse(Request["id"]), double.Parse(Request["amount"]));

            this._accountingService.BuyerInvoiceChangeStatus(long.Parse(Request["id"]), 2);

            this._historyService.AddHistory("AccountingController", HistoryAction.Payment_Added, "BuyerPayment", retVal, "", "Amount:" + Request["amount"] + ";", "", this._appContext.AppUser.Id);

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the affiliate invoice payment.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult AddAffiliateInvoicePayment()
        {
            if (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            int retVal = (int)this._accountingService.AddAffiliateInvoicePayment(long.Parse(Request["id"]), double.Parse(Request["amount"]));

            this._accountingService.AffiliateInvoiceChangeStatus(long.Parse(Request["id"]), 2);

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the buyer distrib.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetBuyerDistrib()
        {
            if (this._appContext.AppUser == null || this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
            {
                return null;
            }

            int retVal = (int)this._accountingService.GetBuyerDistrib(long.Parse(Request["BuyerId"]));
            return Json(retVal, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the affiliate invoices.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [ValidateInput(false)]
        public ActionResult GetAffiliateInvoices()
        {
            if (this._appContext.AppUser == null || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            DateTime dateFrom = new DateTime(1900, 1, 1);
            DateTime dateTo = new DateTime(2030, 1, 1);
            if (Request["dates"] != null && Request["dates"] != "")
            {
                var dates = Request["dates"].Split(':');
                if (dates[0] != "")
                {
                    dateFrom = Convert.ToDateTime(dates[0]);
                    dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
                }
                if (dates[1] != "")
                {
                    dateTo = Convert.ToDateTime(dates[1]);
                    dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);
                }
            }

            dateFrom = _settingService.GetUTCDate(dateFrom);
            dateTo = _settingService.GetUTCDate(dateTo);

            long affiliateid = 0;

            if (Request["filteraffiliateid"] != null)
            {
                affiliateid = long.Parse(Request["filteraffiliateid"]);
            }

            int statusFilter = Request["statfilter"] != null ? short.Parse(Request["statfilter"]) : -2; // All

            List<AffiliateInvoice> invoices = new List<AffiliateInvoice>();

            if (Request["affiliateid"] != null || this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
            {
                invoices = (List<AffiliateInvoice>)this._accountingService.GetAffiliateInvoices(this._appContext.AppUser.ParentId, dateFrom, dateTo, -3);
            }
            else
            {
                invoices = (List<AffiliateInvoice>)this._accountingService.GetAllAffiliateInvoices(affiliateid, dateFrom, dateTo, statusFilter);
            }

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = invoices.Count;
            jd.recordsFiltered = 3;
            jd.recordsSum = 0;

            string StatusStr = "";
            string DisabledClass = "";

            double InvoiceSum = 0;
            double ApprovedSum = 0;
            double PaidSum = 0;

            Setting tzSetting = _settingService.GetSetting("TimeZone");

            foreach (AffiliateInvoice ai in invoices)
            {
                InvoiceSum += (ai.Sum - ai.Refunded + ai.Adjustment);
                DisabledClass = "";
                switch (ai.Status)
                {
                    case 1:
                        {
                            StatusStr = "<span class=\"label status-label label-info\">Approved</span>";
                            break;
                        }
                    case 0:
                        {
                            StatusStr = "<span class=\"label status-label label-danger\">Not<br>Approved</span>";
                            break;
                        }

                    case 2:
                        {
                            StatusStr = "<span class=\"label status-label label-success\">Paid</span>";
                            break;
                        }
                    case -1:
                        {
                            StatusStr = "<span class=\"label status-label label-default\">Deleted</span>";
                            DisabledClass = " disabled-row ";
                            break;
                        }
                }

                /*
                IList<RefundedLeads> rlList = this._accountingService.GetAffiliateRefundedLeadsById(ai.Id);
                ai.Refunded = 0;
                foreach (RefundedLeads rl in rlList)
                {
                    ai.Refunded += (double)rl.APrice;
                }

                ai.Refunded = -ai.Refunded;

                IList<AffiliateInvoiceAdjustment> biaList = this._accountingService.GetAffiliateAdjustments(ai.Id);
                ai.Adjustment = 0;
                foreach (AffiliateInvoiceAdjustment bia in biaList)
                {
                    ai.Adjustment += (double)bia.Sum;
                }
                */
                string BillWithin = "";
                string AffiliateIdName = "";

                Affiliate aff = this._affiliateService.GetAffiliateById(ai.AffiliateId, false);
                if (aff != null)
                {
                    AffiliateIdName = "<a href='/Management/Affiliate/Item/" + ai.AffiliateId.ToString() + "'>" + aff.Name;

                    if (ai.Paid == 0)
                    {
                        if (aff.BillWithin == null)
                        {
                            aff.BillWithin = 0;
                        }
                        DateTime toDay = DateTime.Now;
                        if (aff.BillFrequency == "m")
                        {
                            DateTime billDate = new DateTime(toDay.Year, toDay.Month, (int)aff.FrequencyValue, 0, 0, 0);
                            billDate = billDate.AddDays((double)aff.BillWithin);

                            double differance = Math.Ceiling((billDate - toDay).TotalDays);
                            if (differance >= aff.BillWithin)
                            {
                                BillWithin = "<span class=\"label label-default\">Left " + differance.ToString() + " days</span>";
                            }
                            else
                            {
                                BillWithin = "<span class=\"label label-danger\">Left " + differance.ToString() + " days</span>";
                            }
                        }
                        else
                        {
                            if (aff.FrequencyValue != null && aff.BillWithin != null)
                            {
                                if ((int)toDay.DayOfWeek - (int)aff.FrequencyValue <= (int)aff.BillWithin)
                                {
                                    BillWithin = "<span class=\"label label-danger\">Left " + ((int)toDay.DayOfWeek - (int)aff.FrequencyValue).ToString() + " days</span>";
                                }
                                else
                                {
                                    BillWithin = "<span class=\"label label-default\">Left " + ((int)toDay.DayOfWeek - (int)aff.FrequencyValue).ToString() + " days</span>";
                                }
                            }
                        }
                    }
                }

                string BtnApprove = ai.Status == 0 ? " <button class='BtnApprove action-btn btn btn-info btn-xs' type='button' data-id='" + ai.Id.ToString() + "'>Approve</button> " : "";
                string BtnDisapprove = ai.Status == 1 ? " <button class='BtnDisapprove action-btn btn btn-warning btn-xs' type='button' data-id='" + ai.Id.ToString() + "'>Disapprove</button> " : "";
                string BtnPaid = ai.Status == 1 ? " <button class='BtnPaid action-btn btn btn-success btn-xs' type='button' data-id='" + ai.Id.ToString() + "' data-total='" + (ai.Sum - ai.Refunded + ai.Adjustment).ToString() + "'>Paid</button> " : "";
                string BtnUnpaid = ai.Status == 2 ? " <button class='BtnUnpaid action-btn btn bg-pink-400 btn-xs' type='button' data-id='" + ai.Id.ToString() + "'>Unpaid</button> " : "";
                string BtnDelete = ai.Status == 0 ? " <button class='BtnDelete action-btn btn btn-danger btn-xs' type='button' data-id='" + ai.Id.ToString() + "'>Delete</button> " : "";
                string BtnDownload = ai.Status > 0 ? " <i class='BtnDownload glyphicon glyphicon-download-alt' data-id='" + ai.Id.ToString() + "'></i>" : "";

                if (ai.Status != -1)
                {
                    jd.recordsSum += InvoiceSum;
                }

                if (this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
                {
                    string[] names1 = {
                                      ai.Id.ToString(),
                                      "<h6 class='no-margin text-bold alignright" + DisabledClass + "'><a href='/Management/Accounting/AffiliateInvoiceItem/"+ai.Id+"'>"+ai.AffiliateId.ToString() + "." + ai.Number.ToString() + "</a></h6> ",
                                      AffiliateIdName,
                                      "<p style='text-align: center'>" + ai.DateFrom.ToShortDateString() + "<br>" + ai.DateTo.ToShortDateString() + "</p>",
                                      "<p style='text-align: center'>" + _settingService.GetTimeZoneDate(ai.DateCreated, null, tzSetting).ToShortDateString() + "</p>",
                                      "<h6 class=\"no-margin text-bold " + DisabledClass + "\" style='text-align: right'>" + String.Format("{0:$###,###.00}", (ai.Sum - ai.Refunded + ai.Adjustment))+"</h6>",
                                      "<div style='width:200px; text-align: center'>" + BtnApprove + BtnDisapprove + BtnUnpaid + BtnPaid + BtnDelete + "</div>",
                                      "<p style='text-align: center'>" + StatusStr + "</p>",
                                      "<p style='text-align: center'>" + BtnDownload + "</p>"
                                };

                    switch (ai.Status)
                    {
                        case 1:
                            {
                                ApprovedSum += (ai.Sum - ai.Refunded + ai.Adjustment);
                                break;
                            }

                        case 2:
                            {
                                ApprovedSum += (ai.Sum - ai.Refunded + ai.Adjustment);
                                PaidSum += (ai.Paid);
                                break;
                            }
                    }

                    jd.data.Add(names1);
                }
                else
                {
                    string[] names1 = {
                                      ai.Id.ToString(),
                                      "<h6 class='no-margin text-bold alignright" + DisabledClass + "'><a href='/Management/Accounting/AffiliateInvoiceItem/"+ai.Id+"'>"+ai.AffiliateId.ToString() + "." + ai.Number.ToString() + "</a></h6>",
                                      AffiliateIdName,
                                      ai.DateFrom.ToShortDateString() + "<br>" + ai.DateTo.ToShortDateString(),
                                      _settingService.GetTimeZoneDate(ai.DateCreated, null, tzSetting).ToShortDateString(),
                                      "<div class=\"no-margin text-bold alignright" + DisabledClass + "\">" + String.Format("{0:$###,###.00}", (ai.Sum - ai.Refunded + ai.Adjustment)) + "</div>",
                                      "<div class=\"no-margin text-bold alignright" + DisabledClass + "\">" + String.Format("{0:$###,###.00}", ai.Paid) + "</div>",
                                      "<div class=\"no-margin text-bold alignright" + DisabledClass + "\">" + String.Format("{0:$###,###.00}", (ai.Sum - ai.Refunded + ai.Adjustment) - ai.Paid) + "</div>",
                                      "<div style='width:200px; text-align: center'>" + BtnApprove + BtnDisapprove + BtnUnpaid + BtnPaid + BtnDelete + "</div>",
                                      "<p style='text-align: center'>" + StatusStr + "</p>",
                                      "<p style='text-align: center'>" + BtnDownload + "</p>"
                                };
                    jd.data.Add(names1);

                    ApprovedSum += (ai.Sum - ai.Refunded + ai.Adjustment) - ai.Paid;
                    PaidSum += ai.Paid;
                }
            }

            jd.totalsSumStr[0] = String.Format("{0:$###,###.00}", InvoiceSum);
            jd.totalsSumStr[1] = String.Format("{0:$###,###.00}", PaidSum);
            jd.totalsSumStr[2] = String.Format("{0:$###,###.00}", (PaidSum - InvoiceSum));

            List<AffiliateInvoice> dtList = new System.Collections.Generic.List<AffiliateInvoice>();

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        //
        /// <summary>
        /// Buyers the payments.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Accounting / Buyer Payments")]
        public ActionResult BuyerPayments()
        {
            BuyerPaymentModel model = new BuyerPaymentModel();
            model.BuyersList = this._buyerService.GetAllBuyers();

            ViewBag.PaymentMethods = PaymentMethods;
            ViewBag.TimeZoneNow = _settingService.GetTimeZoneDate(DateTime.UtcNow).ToShortDateString();

            return View(model);
        }

        /// <summary>
        /// Buyers the payments partial.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult BuyerPaymentsPartial(long Id)
        {
            BuyerPaymentModel model = new BuyerPaymentModel();
            model.BuyersList = this._buyerService.GetAllBuyers();
            ViewBag.TimeZoneNow = _settingService.GetTimeZoneDate(DateTime.UtcNow).ToShortDateString();
            ViewBag.EntityId = Id;
            return PartialView(model);
        }

        /// <summary>
        /// Gets the buyer payments.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetBuyerPayments()
        {
            if (this._appContext.AppUser == null || this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
            {
                return null;
            }

            DateTime dateFrom = new DateTime(2000, 1, 1);
            DateTime dateTo = new DateTime(2020, 1, 1);
            double PaymentsSum = 0;

            if (Request["datefrom"] != null && Request["datefrom"] != "")
            {
                dateFrom = Convert.ToDateTime(Request["datefrom"]);
            }
            if (Request["dateto"] != null && Request["dateto"] != "")
            {
                dateTo = Convert.ToDateTime(Request["dateto"]);
            }

            dateFrom = _settingService.GetUTCDate(dateFrom);
            dateTo = _settingService.GetUTCDate(dateTo);

            List<BuyerPaymentView> BuyerPayments;

            if (this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId) // Buyer
            {
                BuyerPayments = (List<BuyerPaymentView>)this._accountingService.GetAllBuyerPaymentsByBuyer(this._appContext.AppUser.ParentId, dateFrom, dateTo);
            }
            else // Admin
            {
                if (Request["buyerid"] != null)
                {
                    BuyerPayments = (List<BuyerPaymentView>)this._accountingService.GetAllBuyerPaymentsByBuyer(long.Parse(Request["buyerid"]), dateFrom, dateTo);
                }
                else
                {
                    BuyerPayments = (List<BuyerPaymentView>)this._accountingService.GetAllBuyerPayments(0, "");
                }
            }

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = BuyerPayments.Count;
            jd.recordsFiltered = 3;
            jd.recordsSum = 0;

            foreach (BuyerPaymentView ap in BuyerPayments)
            {
                PaymentsSum += ap.Amount;
                if (this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId || Request["buyerid"] != null) // Buyer
                {
                    string[] names1 = {
                                      ap.Id.ToString(),
                                      ap.PaymentDate.ToShortDateString(),
                                      "<h6 class=\"no-margin text-bold alignright\">$" + ap.Amount.ToString() + "</h6>",
                                      ap.PaymentMethod != null ? PaymentMethods[(int)ap.PaymentMethod] : "",
                                      ap.Note,
                                      ap.Created.ToShortDateString(),
                                };
                    jd.recordsSum += ap.Amount;
                    jd.data.Add(names1);
                }
                else
                {
                    string[] names1 = {
                                      ap.Id.ToString(),
                                      ap.PaymentDate.ToShortDateString(),
                                      ap.BuyerId.ToString() + "# " + ap.BuyerName,
                                      "<h6 class=\"no-margin text-bold alignright\">$" + ap.Amount.ToString() + "</h6>",
                                      ap.PaymentMethod != null ? PaymentMethods[(int)ap.PaymentMethod] : "",
                                      ap.Note,
                                      ap.Created.ToShortDateString(),
                                      ap.UserID.ToString() + "# " + ap.UserName,
                                      GenerateActionButtons(Request["actions"])
                                };
                    jd.recordsSum += ap.Amount;
                    jd.data.Add(names1);
                }
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the affiliate payment.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult AddAffiliatePayment()
        {
            if (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            DateTime date = Convert.ToDateTime(Request["date"]);
            date = _settingService.GetUTCDate(date);

            AffiliatePayment affPay = new AffiliatePayment();
            affPay.Id = 0;
            affPay.AffiliateId = int.Parse(Request["affiliateid"]);
            affPay.Amount = double.Parse(Request["amount"]); ;
            affPay.PaymentDate = date;
            affPay.UserID = 0;
            affPay.Created = DateTime.UtcNow;
            affPay.Note = Request["note"];

            int retVal = (int)this._accountingService.InsertAffiliatePayment(affPay);

            return Json(retVal, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the buyer payment.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult AddBuyerPayment()
        {
            if (this._appContext.AppUser == null || this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            DateTime date = Convert.ToDateTime(Request["date"]);
            date = _settingService.GetUTCDate(date);

            BuyerPayment buPay = new BuyerPayment();
            buPay.Id = 0;
            buPay.BuyerId = int.Parse(Request["affiliateid"]);
            buPay.Amount = double.Parse(Request["amount"]); ;
            buPay.PaymentDate = date;
            buPay.UserID = 0;
            buPay.Created = DateTime.UtcNow;
            buPay.Note = Request["note"];
            buPay.PaymentMethod = short.Parse(Request["method"]);

            int retVal = (int)this._accountingService.InsertBuyerPayment(buPay);

            BuyerBalance bb = new BuyerBalance();

            bb.BuyerId = buPay.BuyerId;
            bb.PaymentSum = (decimal)buPay.Amount;

            this._accountingService.UpdateBuyerBalance(bb, "PaymentSum");

            this._historyService.AddHistory("AccountingController", HistoryAction.Payment_Added, "BuyerPayment", retVal, "", "Amount:" + buPay.Amount, "", this._appContext.AppUser.Id);

            return Json(retVal, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletes the affiliate payment.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult DeleteAffiliatePayment()
        {
            if (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            int retVal = (int)this._accountingService.DeleteAffiliatePayment(long.Parse(Request["id"]));
            this._historyService.AddHistory("AccountingController", HistoryAction.Payment_Deleted, "AffiliatePayment", long.Parse(Request["id"]), "", "", "", this._appContext.AppUser.Id);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletes the buyer payment.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult DeleteBuyerPayment()
        {
            if (this._appContext.AppUser == null || this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            this._accountingService.DeleteBuyerPayment(long.Parse(Request["id"]));

            this._historyService.AddHistory("AccountingController", HistoryAction.Payment_Deleted, "BuyerPayment", long.Parse(Request["id"]), "", "", "", this._appContext.AppUser.Id);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Refundeds the leads.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// Refunded Leads
        [NavigationBreadCrumb(Clear = true, Label = "Accounting / Refunded Leads")]
        public ActionResult RefundedLeads()
        {
            return View();
        }

        /// <summary>
        /// Gets the refunded leads.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetRefundedLeads()
        {
            if (this._appContext.AppUser == null || this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            List<RefundedLeads> refundedLeads = (List<RefundedLeads>)this._accountingService.GetAllRefundedLeads(0,  DateTime.UtcNow, DateTime.UtcNow, ""); //Gri Zaglushka

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = 3;
            jd.recordsFiltered = 3;

            foreach (RefundedLeads rl in refundedLeads)
            {
                string StatusStr = "";
                switch (rl.Approved)
                {
                    case 1:
                        {
                            StatusStr = "<span class=\"label label-success\">Approved</span>";
                            break;
                        }
                    case 0:
                        {
                            StatusStr = "<span class=\"label label-info\">Pending</span>";
                            break;
                        }
                    case 2:
                        {
                            StatusStr = "<span class=\"label label-danger\">Reject</span>";
                            break;
                        }
                }

                string actionBtns = "<button class='ChangeStatusModal btn btn-info btn-xs' type='button' data-target='#modal_form_change_status' data-toggle='modal'>Change Status</button>";

                string[] names1 = {
                                      rl.Id.ToString(),
                                      rl.LeadId.ToString(),
                                      rl.DateCreated.ToString(),
                                      String.Format("{0:$#.00}", rl.APrice),
                                      String.Format("{0:$#.00}", rl.BPrice),
                                      rl.AInvoiceId.ToString(),
                                      rl.BInvoiceId.ToString(),
                                      rl.Reason,
                                      rl.ReviewNote,
                                      StatusStr,
                                      actionBtns
                                };
                jd.data.Add(names1);
            }

            //            List<AffiliateInvoice> dtList = new System.Collections.Generic.List<AffiliateInvoice>();

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the refund leads.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult AddRefundLeads()
        {
            if (this._appContext.AppUser == null || this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            RefundedLeads refLeads = new RefundedLeads();
            refLeads.Id = 0;
            refLeads.Approved = 0;
            refLeads.DateCreated = DateTime.UtcNow;
            refLeads.LeadId = long.Parse(Request["leadid"]);
            refLeads.Reason = Request["note"];

            long buyerId = long.Parse(Request["buyerid"]);
            LeadMainResponse lmr = this._leadMainResponseService.GetLeadMainResponsesByLeadIdBuyerId(refLeads.LeadId, buyerId);

            int retVal = 0;

            if (lmr != null)
            {
                refLeads.BPrice = lmr.BuyerPrice;
                refLeads.APrice = lmr.AffiliatePrice;

                retVal = (int)this._accountingService.InsertRefundedLeads(refLeads);

                string NewData =
                    "DateCreated:" + refLeads.DateCreated + ";" +
                    "LeadId:" + refLeads.LeadId.ToString() + ";" +
                    "Reason:" + refLeads.Reason + ";";

                this._historyService.AddHistory("AccountingController", HistoryAction.Refund_Added, "RefundedLeads", retVal, NewData, "", "", this._appContext.AppUser.Id);
            }

            return Json(retVal, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletes the refunded leds.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult DeleteRefundedLeds()
        {
            if (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            RefundedLeads refLeads = new RefundedLeads();

            int retVal = (int)this._accountingService.DeleteRefundedLead(long.Parse(Request["id"]));

            this._historyService.AddHistory("AccountingController", HistoryAction.Refund_Deleted, "RefundedLeads", long.Parse(Request["id"]), "", "", "", this._appContext.AppUser.Id);

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Changes the refunded status.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult ChangeRefundedStatus()
        {
            if (this._appContext.AppUser == null || this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            int retVal = (int)this._accountingService.ChangeRefundedStatus(long.Parse(Request["id"]), byte.Parse(Request["status"]), Request["note"]);

            string OldData =
                "Status:" + " " + ";" +
                "Note:" + " " + ";";

            string NewData =
                "Status:" + Request["status"] + ";" +
                "Note:" + Request["note"] + ";";

            this._historyService.AddHistory("AccountingController", HistoryAction.Refund_Status_Changed, "RefundedLeads", long.Parse(Request["id"]), OldData, NewData, "", this._appContext.AppUser.Id);

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        // Custom Invoices
        //
        /// <summary>
        /// Generates the custom invoices.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Accounting / Generate Custom Invoices")]
        public ActionResult GenerateCustomInvoices()
        {
            IList<Affiliate> affiliatesList = this._affiliateService.GetAllAffiliates();

            GenerateInvoiceModel giModel = new GenerateInvoiceModel();
            giModel.buyerList = this._buyerService.GetAllBuyers();
            giModel.affiliateList = this._affiliateService.GetAllAffiliates();

            ViewBag.TimeZoneNow = this._settingService.GetTimeZoneDate(DateTime.UtcNow).ToShortDateString();

            return View(giModel);
        }

        /// <summary>
        /// Generates the invoice ajax.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GenerateInvoiceAjax()
        {
            long retId = 0;

            if (this._appContext.AppUser == null)
                return Json(retId, JsonRequestBehavior.AllowGet);

            //DateTime dt1 = Convert.ToDateTime(Request["datefrom"]);
            DateTime dt2 = Convert.ToDateTime(Request["dateto"]);

            DateTime dtTimeZone = dt2;

            //dt1 = _settingService.GetUTCDate(dt1);
            dt2 = _settingService.GetUTCDate(dt2);

            //dt2 = Convert.ToDateTime(dt2.ToShortDateString());

            //if (dt2.Day >= dtTimeZone.Day)
            //  dt2 = dt2.AddDays(-1);

            if (Request["buyerid"] != null)
            {
                retId = (long)this._accountingService.GenerateBuyerInvoices(long.Parse(Request["buyerid"]), null/*dt1*/, dt2, this._appContext.AppUser.Id);
            }
            else if (Request["affiliateid"] != null)
            {
                retId = (long)this._accountingService.GenerateAffiliateInvoices(long.Parse(Request["affiliateid"]), null/*dt1*/, dt2, this._appContext.AppUser.Id);
            }

            string OldData =
                "Status:" + " " + ";" +
                "Note:" + " " + ";";

            string NewData =
                "Date From:" + dt2 + ";" +
                "BuyerId:" + Request["buyerid"] + ";";

            this._historyService.AddHistory("AccountingController", HistoryAction.Invoice_Generated_Custom_Invoice, "BuyerInvoice", retId, OldData, NewData, "", this._appContext.AppUser.Id);

            return Json(retId, JsonRequestBehavior.AllowGet);
        }

        //        [AllowAnonymous]
        /// <summary>
        /// Generates the invoices.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAuthorize(true)]
        public ActionResult GenerateInvoices()
        {

            if (Request["password"] != "d7DVEkPMRWrnYbp227PawkH")
            {
                return null;
            }
            MemoryCacheManager.EnableRemoteCacheCleaner = false;
            try
            {
                this._historyService.AddHistory("Accounting", HistoryAction.Invoice_Generated_Invoice, "", 0,
                    "Starting Invoices Auto Generation", "", "Starting Invoices Auto Generation", 0);
                int dayOfWeekNum = (int) _settingService.GetTimeZoneDate(DateTime.UtcNow).DayOfWeek;
                int today = _settingService.GetTimeZoneDate(DateTime.UtcNow).Day;

                // For Buyers
                var buyersQuery = from c in _BuyerRepository.Table
                    where (c.BillFrequency == "w" && c.FrequencyValue == (dayOfWeekNum)) ||
                          (c.BillFrequency == "m" && c.FrequencyValue == today) ||
                          (c.BillFrequency == "bw" && c.FrequencyValue == (dayOfWeekNum))
                    orderby c.Id
                    select c;

                IList<Buyer> buyers = buyersQuery.ToList();

                GenerateInvoiceModel giModel = new GenerateInvoiceModel();
                giModel.buyerList = buyersQuery.ToList();

                foreach (Buyer b in buyers)
                {
                    if (b.BillFrequency == "bw" &&
                        (!b.IsBiWeekly.HasValue || (b.IsBiWeekly.HasValue && !b.IsBiWeekly.Value)))
                    {
                        continue;
                    }

                    DateTime dateTo;
                    dateTo = DateTime.UtcNow;

                    long retId = (long) this._accountingService.GenerateBuyerInvoices(b.Id, null, dateTo, 0);
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
                    where (c.BillFrequency == "w" && c.FrequencyValue == (dayOfWeekNum)) ||
                          (c.BillFrequency == "m" && c.FrequencyValue == today) ||
                          (c.BillFrequency == "bw" && c.FrequencyValue == (dayOfWeekNum))
                    orderby c.Id
                    select c;

                IList<Affiliate> affiliates = affiliatesQuery.ToList();

                foreach (Affiliate a in affiliates)
                {
                    if (a.BillFrequency == "bw" &&
                        (!a.IsBiWeekly.HasValue || (a.IsBiWeekly.HasValue && !a.IsBiWeekly.Value)))
                    {
                        continue;
                    }

                    DateTime dateTo;
                    dateTo = DateTime.UtcNow;

                    long retId = (long) this._accountingService.GenerateAffiliateInvoices(a.Id, null, dateTo, 0);
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

                this._historyService.AddHistory("Accounting", HistoryAction.Invoice_Generated_Invoice, "", 0,
                    "Finishing Invoices Auto Generation", "", "Finishing Invoices Auto Generation", 0);

                _leadMainService.ClearSensitiveData();
                MemoryCacheManager.EnableRemoteCacheCleaner = true;
                return PartialView(giModel);
            }
            finally
            {
                MemoryCacheManager.EnableRemoteCacheCleaner = true;

            }
            
        }

        /// <summary>
        /// Buyerses the balance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// Buyers Balance
        [NavigationBreadCrumb(Clear = true, Label = "Accounting / Buyers Balance")]
        public ActionResult BuyersBalance()
        {
            ViewBag.AllBuyersList = this._buyerService.GetAllBuyers();

            return View();
        }

        /// <summary>
        /// Gets the buyers balance ajax.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetBuyersBalanceAjax()
        {
            if (this._appContext.AppUser == null || this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
            {
                return null;
            }

            long bId = 0;
            if (Request["buyerid"] != null)
            {
                bId = long.Parse(Request["buyerid"]);
            }

            if (this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                bId = this._appContext.AppUser.ParentId;
            }

            long BuyerIdFilter = bId;
            if (bId < 0) bId = 0;

            DateTime dateFrom = new DateTime(2000, 1, 1);
            DateTime dateTo = new DateTime(2020, 1, 1);
            DateTime dateToForAfter = new DateTime(2000, 1, 1);
            if (Request["dates"] != null && Request["dates"] != "")
            {
                var dates = Request["dates"].Split(':');
                if (dates[0] != "")
                {
                    dateFrom = Convert.ToDateTime(dates[0]);
                    dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
                }
                if (dates[1] != "")
                {
                    dateTo = Convert.ToDateTime(dates[1]);
                    dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);
                    // dateToForAfter = dateFrom.AddDays(-1);
                    dateToForAfter = dateFrom;
                }
            }

            dateToForAfter = _settingService.GetUTCDate(dateToForAfter);
            dateFrom = _settingService.GetUTCDate(dateFrom);
            dateTo = _settingService.GetUTCDate(dateTo);

            IList<BuyerBalanceView> bbvAftter = this._accountingService.GetBuyersBalance(bId, new DateTime(2000, 1, 1), dateToForAfter);

            IList<BuyerBalanceView> bbv = this._accountingService.GetBuyersBalance(bId, dateFrom, dateTo);

            bId = BuyerIdFilter;

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = (int)bbv.Count;
            jd.recordsFiltered = 0;

            decimal[] totalsSum = new decimal[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

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

                totalsSum[1] += balanceAfter;
                totalsSum[2] += bb.SoldSum;
                totalsSum[3] += bb.InvoicedSum;
                totalsSum[4] += bb.PaymentSum;
                totalsSum[5] += bb.Balance;
                totalsSum[6] += balanceAfter + bb.Balance;

                string[] names1 = {
                    "<p class=''><a href='/Management/Buyer/Item/"+ bb.BuyerId +"'>"+ bb.Name + " " + "</a></p>",
                    "<p style='text-align: right;' class='"+ (balanceAfter >= 0 ? "text-success-600" : "text-danger") +"'>" + balanceAfter.ToString("$#,###,###.00") + "</p>",
                    "<p style='text-align: right;'>" + bb.SoldSum.ToString("$#,###,###.00") + "</p>",
                    "<p style='text-align: right;'>" + bb.InvoicedSum.ToString("$#,###,###.00") + "</p>",
                    "<p style='text-align: right;'>" + bb.PaymentSum.ToString("$#,###,###.00") + "</p>",
                    "<p style='text-align: right;'class='"+ (bb.Balance >= 0 ? "text-success-600" : "text-danger") +"'>" + bb.Balance.ToString("$#,###,###.00") + "</p>",
                    "<p style='text-align: right;' class='"+ (balanceAfter + bb.Balance >= 0 ? "text-success-600" : "text-danger") +"'>" +  (balanceAfter + bb.Balance).ToString("$#,###,###.00") + "</p>",
                    "<p style='text-align: right;'>" + bb.Credit.ToString("$#,###,###.00") + "</p>"
                };

                jd.data.Add(names1);
            }

            jd.totalsSumStr[0] = "<p><h5>Totals: </h5></p>";
            jd.totalsSumStr[1] = totalsSum[1].ToString("$#,###,###.00");
            jd.totalsSumStr[2] = totalsSum[2].ToString("$#,###,###.00");
            jd.totalsSumStr[3] = totalsSum[3].ToString("$#,###,###.00");
            jd.totalsSumStr[4] = totalsSum[4].ToString("$#,###,###.00");
            jd.totalsSumStr[5] = totalsSum[5].ToString("$#,###,###.00");
            jd.totalsSumStr[6] = totalsSum[6].ToString("$#,###,###.00");

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Affiliateses the balance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// Affiliates Balance
        [NavigationBreadCrumb(Clear = true, Label = "Accounting / Affiliates Balance")]
        public ActionResult AffiliatesBalance()
        {
            ViewBag.AllAffiliatesList = this._affiliateService.GetAllAffiliates();

            return View();
        }

        /// <summary>
        /// Gets the affiliates balance ajax.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetAffiliatesBalanceAjax()
        {
            if (this._appContext.AppUser == null || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }
            long bId = 0;
            if (Request["buyerid"] != null)
            {
                bId = long.Parse(Request["buyerid"]);
            }

            if (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
            {
                bId = this._appContext.AppUser.ParentId;
            }

            long BuyerIdFilter = bId;
            if (bId < 0) bId = 0;

            DateTime dateFrom = new DateTime(1900, 1, 1);
            DateTime dateTo = new DateTime(2020, 1, 1);
            DateTime dateToForAfter = new DateTime(2000, 1, 1);
            if (Request["dates"] != null && Request["dates"] != "")
            {
                var dates = Request["dates"].Split(':');
                if (dates[0] != "")
                {
                    dateFrom = Convert.ToDateTime(dates[0]);
                    dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
                }
                if (dates[1] != "")
                {
                    dateTo = Convert.ToDateTime(dates[1]);
                    dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);
                    dateToForAfter = dateFrom;
                }
            }

            dateToForAfter = _settingService.GetUTCDate(dateToForAfter);
            dateFrom = _settingService.GetUTCDate(dateFrom);
            dateTo = _settingService.GetUTCDate(dateTo);

            IList<BuyerBalanceView> bbvAftter = this._accountingService.GetAffiliatesBalance(bId, new DateTime(2000, 1, 1), dateToForAfter);

            IList<BuyerBalanceView> bbv = this._accountingService.GetAffiliatesBalance(bId, dateFrom, dateTo);

            bId = BuyerIdFilter;

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = (int)bbv.Count;
            jd.recordsFiltered = 0;

            decimal[] totalsSum = new decimal[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

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

                totalsSum[1] += balanceAfter;
                totalsSum[2] += bb.SoldSum;
                totalsSum[3] += bb.InvoicedSum;
                totalsSum[4] += bb.PaymentSum;
                totalsSum[5] += bb.Balance;
                totalsSum[6] += balanceAfter + bb.Balance;

                string[] names1 = {
                    "<p class='text-center'><a href='/Management/Affiliate/Item/"+ bb.BuyerId +"'>"+ bb.Name + " " + "</a></p>",
                    "<p class='text-center "+ (balanceAfter >= 0 ? "text-success-600" : "text-danger") +"'>" + balanceAfter.ToString("$#,###,###.00") + "</p>",
                    "<p class='text-center'>" + bb.SoldSum.ToString("$#,###,###.00") + "</p>",
                    "<p class='text-center'>" + bb.InvoicedSum.ToString("$#,###,###.00") + "</p>",
                    "<p class='text-center'>" + bb.PaymentSum.ToString("$#,###,###.00") + "</p>",
                    "<p class='text-center "+ (bb.Balance >= 0 ? "text-success-600" : "text-danger") +"'>" + bb.Balance.ToString("$#,###,###.00") + "</p>",
                    "<p class='text-center "+ (balanceAfter + bb.Balance >= 0 ? "text-success-600" : "text-danger") +"'>" +  (balanceAfter + bb.Balance).ToString("$#,###,###.00") + "</p>"
                };

                jd.data.Add(names1);
            }

            jd.totalsSumStr[0] = "<p><h5>Totals: </h5></p>";
            jd.totalsSumStr[1] = totalsSum[1].ToString("$#,###,###.00");
            jd.totalsSumStr[2] = totalsSum[2].ToString("$#,###,###.00");
            jd.totalsSumStr[3] = totalsSum[3].ToString("$#,###,###.00");
            jd.totalsSumStr[4] = totalsSum[4].ToString("$#,###,###.00");
            jd.totalsSumStr[5] = totalsSum[5].ToString("$#,###,###.00");
            jd.totalsSumStr[6] = totalsSum[6].ToString("$#,###,###.00");

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the buyer invoice adjustment.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult AddBuyerInvoiceAdjustment()
        {
            if (this._appContext.AppUser == null || (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId))
            {
                return null;
            }

            int retVal = (int)this._accountingService.AddBuyerInvoiceAdjustment(long.Parse(Request["binvoiceid"]), Request["name"], double.Parse(Request["price"]), int.Parse(Request["qty"]));

            string NewData =
                "Name:" + Request["name"] + ";" +
                "Price:" + Request["price"] + ";" +
                "Qty:" + Request["qty"] + ";";

            this._historyService.AddHistory("AccountingController", HistoryAction.Invoice_Edited_Custom_Adjustment, "BuyerInvoiceAdjustment", retVal, "", NewData, "", this._appContext.AppUser.Id);

            return Json(retVal, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the affiliate invoice adjustment.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult AddAffiliateInvoiceAdjustment()
        {
            if (this._appContext.AppUser == null || (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId))
            {
                return null;
            }

            int retVal = (int)this._accountingService.AddAffiliateInvoiceAdjustment(long.Parse(Request["ainvoiceid"]), Request["name"], double.Parse(Request["price"]), int.Parse(Request["qty"]));

            string NewData =
                "Name:" + Request["name"] + ";" +
                "Price:" + Request["price"] + ";" +
                "Qty:" + Request["qty"] + ";";

            this._historyService.AddHistory("AccountingController", HistoryAction.Payment_Notice_Custom_Adjustment, "AffiliateInvoiceAdjustment", retVal, "", NewData, "", this._appContext.AppUser.Id);

            return Json(retVal, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletes the buyer invoice adjustment.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult DeleteBuyerInvoiceAdjustment()
        {
            if (this._appContext.AppUser == null || this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            int retVal = (int)this._accountingService.DeleteBuyerInvoiceAdjustment(long.Parse(Request["adjustmentid"]));

            this._historyService.AddHistory("AccountingController", HistoryAction.Invoice_Edited_Custom_Adjustment_Deleted, "BuyerInvoiceAdjustment", long.Parse(Request["adjustmentid"]), "", "", "", this._appContext.AppUser.Id);

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletes the affiliate invoice adjustment.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult DeleteAffiliateInvoiceAdjustment()
        {
            if (this._appContext.AppUser == null || this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId || this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            int retVal = (int)this._accountingService.DeleteAffiliateInvoiceAdjustment(long.Parse(Request["adjustmentid"]));
            this._historyService.AddHistory("AccountingController", HistoryAction.Payment_Notice_Custom_Adjustment_Deleted, "AffiliateInvoiceAdjustment", long.Parse(Request["adjustmentid"]), "", "", "", this._appContext.AppUser.Id);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        // PDF * PDF * PDF * PDF * PDF * PDF * PDF * PDF * PDF * PDF * PDF * PDF * PDF * PDF * PDF * PDF * PDF * PDF
        /// <summary>
        /// PDFs the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Pdf(long id)
        {
            if (this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
            {
                return null;
            }

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

            biModel.buyer = this._buyerService.GetBuyerById(bi.BuyerId);
            Country country = this._countryService.GetCountryById(biModel.buyer.CountryId);
            biModel.BuyerCountryName = country.Name;

            ViewBag.PdfFileUrl = "/ContentManagement/Uploads/Invoice_" + biModel.Number + ".pdf";

            var doc = new iTextSharp.text.Document();
            //doc.SetMargins(0, 0, 0, 0);

            PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "/ContentManagement/Uploads/Invoice_" + biModel.Number + ".pdf", FileMode.Create));
            doc.Open();
            // iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(System.AppDomain.CurrentDomain.BaseDirectory + "/ContentManagement/Uploads/logo1.jpg");
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(System.AppDomain.CurrentDomain.BaseDirectory + biModel.CompanyLogoPath);
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

            return PartialView(biModel);
        }

        // PDF For Affiliate
        /// <summary>
        /// PDFs the affiliate.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult PdfAffiliate(long id)
        {
            if (this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                return null;
            }

            AffiliateInvoice bi = this._accountingService.GetAffiliateInvoiceById(id);
            AffiliateInvoiceModel biModel = new AffiliateInvoiceModel();

            biModel.Status = bi.Status;
            biModel.AffiliateId = bi.AffiliateId;
            biModel.DateCreated = bi.DateCreated.ToShortDateString();
            biModel.DateFrom = bi.DateFrom.ToShortDateString();
            biModel.DateTo = bi.DateTo.ToShortDateString();
            biModel.Id = bi.Id;
            biModel.Number = bi.AffiliateId.ToString() + "." + bi.Number.ToString();
            biModel.Sum = bi.Sum;
            biModel.UserID = bi.UserID;

            biModel.AffiliateInvoiceDetailsList = this._accountingService.GetAffiliateInvoiceDetails(id);

            biModel.AffiliateRefundedLeadsList = this._accountingService.GetBuyerRefundedLeadsById(id);

            biModel.AffiliateInvoiceAdjustmentsList = this._accountingService.GetAffiliateAdjustments(id);

            biModel.Total = 0;
            biModel.RefundedTotal = 0;
            foreach (RefundedLeads rl in biModel.AffiliateRefundedLeadsList)
            {
                biModel.RefundedTotal += rl.APrice;
            }

            biModel.AdjustmentTotal = 0;
            foreach (AffiliateInvoiceAdjustment bia in biModel.AffiliateInvoiceAdjustmentsList)
            {
                biModel.AdjustmentTotal += (decimal)bia.Sum;
            }

            biModel.Total = (decimal)biModel.Sum - (decimal)biModel.RefundedTotal + (decimal)biModel.AdjustmentTotal;

            biModel.CompanyName = this._settingService.GetSetting("Settings.CompanyName").Value;
            biModel.CompanyAddress = this._settingService.GetSetting("Settings.CompanyAddress").Value;
            biModel.CompanyBank = this._settingService.GetSetting("Settings.CompanyBank").Value;
            biModel.CompanyEmail = this._settingService.GetSetting("Settings.CompanyEmail").Value;
            biModel.CompanyLogoPath = this._settingService.GetSetting("Settings.CompanyLogoPath").Value;

            biModel.affiliate = this._affiliateService.GetAffiliateById(bi.AffiliateId, false);

            ViewBag.PdfFileUrl = "/ContentManagement/Uploads/Invoice_" + biModel.Number + ".pdf";

            var doc = new iTextSharp.text.Document();
            //doc.SetMargins(0, 0, 0, 0);

            PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "/ContentManagement/Uploads/Invoice_" + biModel.Number + ".pdf", FileMode.Create));
            doc.Open();
            
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(System.AppDomain.CurrentDomain.BaseDirectory + biModel.CompanyLogoPath);
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

            foreach (AffiliateInvoiceDetails bid in biModel.AffiliateInvoiceDetailsList)
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
                                    new PdfPCell( new Phrase( String.Format("{0:$#.00}", biModel.Sum), fontH2) ) { HorizontalAlignment = Element.ALIGN_RIGHT} };
            row = new PdfPRow(cells);
            table.Rows.Add(row);

            doc.Add(table);

            // Refunded/
            if (biModel.AffiliateRefundedLeadsList.Count > 0)
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

                foreach (RefundedLeads rl in biModel.AffiliateRefundedLeadsList)
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
            if (biModel.AffiliateInvoiceAdjustmentsList.Count > 0)
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

                foreach (AffiliateInvoiceAdjustment bia in biModel.AffiliateInvoiceAdjustmentsList)
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

            return PartialView(biModel);
        }

        /// <summary>
        /// Creates the bulk invoice buyer.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult CreateBulkInvoiceBuyer()
        {
            if (Request["buyerid"] == null || Request["dateto"] == null)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

            DateTime dateTo = _settingService.GetUTCDate(DateTime.Parse(Request["dateto"]));

            long retId = this._accountingService.CreateBuyerInvoice(long.Parse(Request["buyerid"]), dateTo);

            return Json(retId, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Creates the bulk invoice affiliate.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult CreateBulkInvoiceAffiliate()
        {
            if (Request["affiliateid"] == null || Request["dateto"] == null)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

            DateTime dateTo = _settingService.GetUTCDate(DateTime.Parse(Request["dateto"]));

            long retId = this._accountingService.CreateAffiliateInvoice(long.Parse(Request["affiliateid"]), dateTo);
            return Json(retId, JsonRequestBehavior.AllowGet);
        }
    }
}