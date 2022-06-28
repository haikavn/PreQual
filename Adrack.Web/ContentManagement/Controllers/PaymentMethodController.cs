// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="PaymentMethodController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using Adrack.Data;
using Adrack.Service.Directory;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Web.ContentManagement.Models.Lead;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Security;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Class PaymentMethodController.
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public partial class PaymentMethodController : BaseContentManagementController
    {
        #region Fields

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IPaymentMethodService _paymentMethodService;

        /// <summary>
        /// The affiliate service
        /// </summary>
        private readonly IAffiliateService _affiliateService;

        /// <summary>
        /// The localized string service
        /// </summary>
        private readonly ILocalizedStringService _localizedStringService;

        /// <summary>
        /// The country service
        /// </summary>
        private readonly ICountryService _countryService;

        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Directory Controller
        /// </summary>
        /// <param name="paymentMethodService">The payment method service.</param>
        /// <param name="affiliateService">The affiliate service.</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="countryService">Country Service</param>
        /// <param name="stateProvinceService">State Province Service</param>
        /// <param name="usersService">The users service.</param>
        public PaymentMethodController(IPaymentMethodService paymentMethodService, IAffiliateService affiliateService, ILocalizedStringService localizedStringService, ICountryService countryService, IStateProvinceService stateProvinceService, IUserService usersService)
        {
            this._paymentMethodService = paymentMethodService;
            this._countryService = countryService;
            this._localizedStringService = localizedStringService;
            this._affiliateService = affiliateService;
            this._userService = usersService;
        }

        #endregion Constructor

        // GET: PaymentMethod
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            List<PaymentMethod> paymentMethods = (List<PaymentMethod>)this._paymentMethodService.GetAllPaymentMethods();

            return View();
        }

        /// <summary>
        /// Lists the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult List(long Id)
        {
            List<PaymentMethod> paymentMethods = (List<PaymentMethod>)this._paymentMethodService.GetAllPaymentMethodsByAffiliateId(Id);

            ViewBag.PaymentMethodList = paymentMethods;

            return PartialView();
        }

        /// <summary>
        /// Prepares the model.
        /// </summary>
        /// <param name="model">The model.</param>
        protected void PrepareModel(PaymentMethodModel model)
        {
            model.ListPaymentMethod.Add(new SelectListItem { Text = "Wire", Value = "0" });
            model.ListPaymentMethod.Add(new SelectListItem { Text = "ACH", Value = "1" });
            model.ListPaymentMethod.Add(new SelectListItem { Text = "WebMoney", Value = "2" });
            model.ListPaymentMethod.Add(new SelectListItem { Text = "Check", Value = "3" });

            model.ListCountry.Add(new SelectListItem { Text = _localizedStringService.GetLocalizedString("Address.SelectCountry"), Value = "" });

            foreach (var value in _countryService.GetAllCountries())
            {
                model.ListCountry.Add(new SelectListItem
                {
                    Text = value.GetLocalized(x => x.Name),
                    Value = value.Id.ToString(),
                    Selected = false
                });
            }

            foreach (var value in _affiliateService.GetAllAffiliates())
            {
                model.ListAffiliates.Add(new SelectListItem
                {
                    Text = value.Name,
                    Value = value.Id.ToString(),
                    Selected = false
                });
            }
        }

        /// <summary>
        /// Items the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [NavigationBreadCrumb(Clear = false, Label = "PaymentMethod")]
        [ContentManagementAntiForgery(true)]
        public ActionResult Item(long id = 0)
        {
            PaymentMethod paymentMethod = this._paymentMethodService.GetPaymentMethodById(id);

            PaymentMethodModel am = new PaymentMethodModel();

            am.PaymentMethodId = 0;

            if (paymentMethod != null)
            {
                am.PaymentMethodId = paymentMethod.Id;
                am.AffiliateId = paymentMethod.AffiliateId;
                am.BankAddress = paymentMethod.BankAddress;
                am.BankName = paymentMethod.BankName;
                am.BankPhone = paymentMethod.BankPhone;
                am.IsPrimary = paymentMethod.IsPrimary;
                am.NameOnAccount = paymentMethod.NameOnAccount;
                am.PaymentMethod = paymentMethod.PaymentType;
                am.SpecialInstructions = paymentMethod.SpecialInstructions;
                am.SwiftRoutingNumber = paymentMethod.SwiftRoutingNumber;
                am.AccountNumber = paymentMethod.AccountNumber;
                am.AccountOwnerAddress = paymentMethod.AccountOwnerAddress;
                am.AccountOwnerPhone = paymentMethod.AccountOwnerPhone;
            }

            PrepareModel(am);

            return PartialView(am);
        }

        /// <summary>
        /// Items the specified payment method model.
        /// </summary>
        /// <param name="paymentMethodModel">The payment method model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [ValidateInput(false)]
        [ContentManagementAntiForgery(true)]
        public ActionResult Item(PaymentMethodModel paymentMethodModel, string returnUrl)
        {
            PaymentMethod paymentMethod = null;

            //if (ModelState.IsValid)
            {
                if (paymentMethodModel.PaymentMethodId == 0)
                {
                    paymentMethod = new PaymentMethod();
                }
                else
                {
                    paymentMethod = _paymentMethodService.GetPaymentMethodById(paymentMethodModel.PaymentMethodId);
                }

                paymentMethod.AffiliateId = paymentMethodModel.AffiliateId;
                paymentMethod.BankAddress = paymentMethodModel.BankAddress;
                paymentMethod.BankName = paymentMethodModel.BankName;
                paymentMethod.BankPhone = paymentMethodModel.BankPhone;
                paymentMethod.IsPrimary = paymentMethodModel.IsPrimary;
                paymentMethod.NameOnAccount = paymentMethodModel.NameOnAccount;
                paymentMethod.PaymentType = paymentMethodModel.PaymentMethod;
                paymentMethod.SpecialInstructions = paymentMethodModel.SpecialInstructions;
                paymentMethod.SwiftRoutingNumber = paymentMethodModel.SwiftRoutingNumber;
                paymentMethod.AccountNumber = paymentMethodModel.AccountNumber;
                paymentMethod.AccountOwnerAddress = paymentMethodModel.AccountOwnerAddress;
                paymentMethod.AccountOwnerPhone = paymentMethodModel.AccountOwnerPhone;

                if (paymentMethodModel.PaymentMethodId == 0)
                    _paymentMethodService.InsertPaymentMethod(paymentMethod);
                else
                    _paymentMethodService.UpdatePaymentMethod(paymentMethod);

                paymentMethodModel.PaymentMethodId = paymentMethod.Id;

                PrepareModel(paymentMethodModel);

                return PartialView(paymentMethodModel);

                //return RedirectToAction("List");
            }

            // return RedirectToAction("List");
        }

        /// <summary>
        /// Gets the payment methods.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetPaymentMethods(long Id = 0)
        {
            List<PaymentMethod> paymentMethods = new List<PaymentMethod>();

            if (Id > 0)
                paymentMethods = (List<PaymentMethod>)this._paymentMethodService.GetAllPaymentMethodsByAffiliateId(Id);
            else
                paymentMethods = (List<PaymentMethod>)this._paymentMethodService.GetAllPaymentMethods();

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = paymentMethods.Count;
            jd.recordsFiltered = paymentMethods.Count;
            foreach (PaymentMethod ai in paymentMethods)
            {
                Affiliate affiliate = _affiliateService.GetAffiliateById(ai.AffiliateId, true);

                string AccountTypeStr = "";
                switch (ai.PaymentType)
                {
                    case 0: { AccountTypeStr = "Wire"; break; }
                    case 1: { AccountTypeStr = "ACH"; break; }
                    case 2: { AccountTypeStr = "WebMoney"; break; }
                    case 3: { AccountTypeStr = "Check"; break; }
                }

                string[] names1 = {
                                      ai.Id.ToString(),
                                      "<b>" + ai.NameOnAccount + "</b>",
                                      AccountTypeStr,
                                      ai.AccountNumber,
                                      ai.BankName,
                                      ai.IsPrimary.ToString(),
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Sets the default.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult SetDefault()
        {
            if (Request["Id"] != null && Request["AffiliateId"] != null)
            {
                _paymentMethodService.SetDefaultPaymentMethod(long.Parse(Request["Id"]), long.Parse(Request["AffiliateId"]));
            }

            return Json("1", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the edit.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult AddEdit()
        {
            PaymentMethod pmm = new PaymentMethod();

            if (Request["PaymentType"] != null)
            {
                pmm.PaymentType = short.Parse(Request["PaymentType"]);
            }

            if (Request["NameOnAccount"] != null)
            {
                pmm.NameOnAccount = Request["NameOnAccount"];
            }

            if (Request["AffiliateId"] != null)
            {
                pmm.AffiliateId = long.Parse(Request["AffiliateId"]);
            }

            if (Request["BankName"] != null)
            {
                pmm.BankName = Request["BankName"];
            }

            if (Request["AccountNumber"] != null)
            {
                pmm.AccountNumber = Request["AccountNumber"];
            }

            if (Request["BankAddress"] != null)
            {
                pmm.BankAddress = Request["BankAddress"];
            }

            if (Request["BankPhone"] != null)
            {
                pmm.BankPhone = Request["BankPhone"];
            }

            if (Request["AccountOwnerAddress"] != null)
            {
                pmm.AccountOwnerAddress = Request["AccountOwnerAddress"];
            }

            if (Request["AccountOwnerPhone"] != null)
            {
                pmm.AccountOwnerPhone = Request["AccountOwnerPhone"];
            }

            if (Request["SwiftRoutingNumber"] != null)
            {
                pmm.SwiftRoutingNumber = Request["SwiftRoutingNumber"];
            }

            if (Request["IsPrimary"] != null)
            {
                pmm.IsPrimary = Request["IsPrimary"] == "true" ? true : false;
            }

            long retId = 0;
            if (Request["PaymentMethodId"] != null && long.Parse(Request["PaymentMethodId"]) > 0)
            {
                pmm.Id = long.Parse(Request["PaymentMethodId"]);

                _paymentMethodService.UpdatePaymentMethod(pmm);
                retId = pmm.Id;
            }
            else
            {
                retId = _paymentMethodService.InsertPaymentMethod(pmm);
            }

            return Json(retId, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult Delete()
        {
            if (Request["Id"] != null)
            {
                _paymentMethodService.DeletePaymentMethod(long.Parse(Request["Id"]));
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}