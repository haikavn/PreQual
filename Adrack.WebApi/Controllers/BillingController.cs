// ***********************************************************************
// Assembly         : Adrack.Controller
// Author           : Arman Zakaryan
// Created          : 04-11-2020
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 17-11-2020
// ***********************************************************************
// <copyright file="BillingController.cs" company="Adrack.com">
//     Copyright © 2020
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using Adrack.Core;
using Adrack.Core.Domain.Billing;
using Adrack.Core.Domain.Lead.Reports;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Payment;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.WebApi.Helpers;
using Adrack.WebApi.Models.Billing;
using Adrack.WebApi.PdfBuilder.PdfReportCreators;
using Braintree;

namespace Adrack.WebApi.Controllers
{
    public class PaymentInfo
    {
        public decimal Amount;
        public string ErrorMessage = "";
        public string Id = "";
        public string PaypalSubscriptionId = "";
        public bool IsTest;
        public string SubscriptionStatus;
    }

    [RoutePrefix("api/billing")]
    public class BillingController : BaseApiController
    {
        public static readonly TransactionStatus[] TransactionSuccessStatuses =
        {
            TransactionStatus.AUTHORIZED,
            TransactionStatus.AUTHORIZING,
            TransactionStatus.SETTLED,
            TransactionStatus.SETTLING,
            TransactionStatus.SETTLEMENT_CONFIRMED,
            TransactionStatus.SETTLEMENT_PENDING,
            TransactionStatus.SUBMITTED_FOR_SETTLEMENT
        };

        #region fields

        private string Environment { get; set; }
        private string MerchantId { get; set; }
        private string PublicKey { get; set; }
        private string PrivateKey { get; set; }

        private IBraintreeGateway BraintreeGateway { get; set; }

        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly ISmtpAccountService _smtpAccountService;
        private readonly IAppContext _appContext;
        private readonly IProfileService _profileService;
        private readonly ISettingService _settingService;
        private readonly IReportService _reportService;
        private readonly IPlanService _planService;
        private readonly IAddonService _addonService;
        private readonly IPermissionService _permissionService;
        private readonly IPaymentMethodService _paymentMethodService;
        private static string _viewBillingHistorKey { get; set; } = "view-billing-history";
        private static string _modifyManageBillingKey { get; set; } = "modify-manage-billing";
        private static string _viewPingReportKey { get; set; } = "view-ping-report";
        #endregion

        #region constructor

        public BillingController( IPaymentService paymentService,
                                  IUserService userService,
                                  IAppContext appContext,
                                  IProfileService profileService,
                                  ISettingService settingService,
                                  IReportService reportService,
                                  IEmailService emailService,
                                  ISmtpAccountService smtpAccountService,
                                  IPlanService planService,
                                  IAddonService addonService,
                                  IPermissionService permissionService)
        {
            _paymentService = paymentService;
            _userService = userService;
            _planService = planService;
            _addonService = addonService;
            _appContext = appContext;
            _profileService = profileService;
            _settingService = settingService;
            _reportService = reportService;
            _permissionService = permissionService;
            _emailService = emailService;
            _smtpAccountService = smtpAccountService;
            Environment = "sandbox";
            MerchantId = "kmdyxcf8jt3y6mtb";
            PublicKey = "wgy2dkfdzhhbsrry";
            PrivateKey = "c1755b6a32b11cfaa837127f4279d605";

/*           
            Environment = "production";
            MerchantId = "th8fc3567j5zwsc6";
            PublicKey = "3cnzpb8hgszfzrxd";
            PrivateKey = "b35bed01ab75f7c83aa99e89bf9a247d";
*/
        }

        public BillingController(IAppContext appContext, IProfileService profileService, IAddonService addonService, IPaymentService paymentService)
        {
            _addonService = addonService;
            _appContext = appContext;
            _profileService = profileService;
            _paymentService = paymentService;

            Environment = "sandbox";
            MerchantId = "kmdyxcf8jt3y6mtb";
            PublicKey = "wgy2dkfdzhhbsrry";
            PrivateKey = "c1755b6a32b11cfaa837127f4279d605";

            /*           
                        Environment = "production";
                        MerchantId = "th8fc3567j5zwsc6";
                        PublicKey = "3cnzpb8hgszfzrxd";
                        PrivateKey = "b35bed01ab75f7c83aa99e89bf9a247d";
            */
        }

        #endregion

        #region methods

        private IBraintreeGateway GetGateway()
        {
            if (BraintreeGateway == null)
            {
                BraintreeGateway = new BraintreeGateway(Environment, MerchantId, PublicKey, PrivateKey);
            }

            return BraintreeGateway;
        }
        private string GetPlanId(int TariffId)
        {
            PlanModel plan = _planService.GetPlanByID(TariffId);
            return "Lead_" + plan.Object;
            /*
            var tariffStr = "";

            switch (TariffId)
            {
                case 1:
                    {
                        tariffStr = "Starter";
                        break;
                    }
                case 2:
                    {
                        tariffStr = "Professional";
                        break;
                    }
                case 3:
                    {
                        tariffStr = "Enterprise";
                        break;
                    }
                default:
                    return "";
            }
            */
            /*
            Lead_Professional_150000
            Lead_Professional_250000
            Lead_Professional_50000
            */

            // return "Lead_" + tariffStr + "_" + pingLimit.ToString();
        }

        private string GetAddonPlanId(string addonKey)
        {
            return "Lead_Addon_" + addonKey;
        }

        private double GetAddonPlanPrice(long AddonId)
        {
            Addon addon = _addonService.GetAddonById(AddonId);
            return (double)addon.Amount;
        }

        private double GetPlanPrice(long PlanId)
        {
            PlanModel plan = _planService.GetPlanByID(PlanId);
            return plan.Amount;
            /*AZ
            var tariffStr = "";

            switch (TariffId)
            {
                case 1:
                    {
                        tariffStr = "Starter";
                        break;
                    }
                case 2:
                    {
                        tariffStr = "Professional";
                        break;
                    }
                case 3:
                    {
                        tariffStr = "Enterprise";
                        break;
                    }
                default:
                    {
                        tariffStr = "Starter";
                        break;
                    }
            }

            var setting = this._settingService.GetSetting($"Plan.Lead_{tariffStr}_{pingLimit}");
            return float.Parse(setting?.Value ?? string.Empty);
            */
        }

        [HttpGet]
        [Route("isLimitReached")]
        public IHttpActionResult IsLimitReached(long userId)
        {
            if (!_permissionService.Authorize(_viewBillingHistorKey))
            {
                return HttpBadRequest("access-denied");
            }
            var payments = _paymentService.GetPaymentsByUser(userId);
            
            if(payments.Count == 0 )
            {
                return Ok(true);
            }
            else
            {
                if (payments[0].ExpireDate < DateTime.UtcNow)
                {
                    return Ok(true);
                }
            }

            DateTime startDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            DateTime endDate = DateTime.UtcNow;

            List<ReportTotalsByDate> totalsByDate = (List<ReportTotalsByDate>)_reportService.ReportTotalsByDate(
                startDate,
                endDate,
                0, 0);

            if(totalsByDate.Count > 0)
            {
                if( totalsByDate[0].received >= payments[0].LeadsLimit ||
                    totalsByDate[0].posted >= payments[0].PingsLimit )
                {
                    return Ok(true);
                }
            }
            return Ok(false);
        }



        [HttpGet]
        [Route("getAllPayments")]
        public IHttpActionResult GetAllPayments()
        {
            try
            {
                if (!_permissionService.Authorize(_viewBillingHistorKey))
                {
                    return HttpBadRequest("access-denied");
                }
                //ALL var payments = _paymentService.GetAllPayments(null, null);
                var payments = _paymentService.GetPaymentsByUser(_appContext.AppUser.Id);


                GetAllPaymentsModel getAllPaymentsModel = new GetAllPaymentsModel();
                List<PaymentModel> paymentModelList = new List<PaymentModel>();
                int k = 0;
                foreach (Payment p in payments)
                {
                    PaymentsModel paymentsModel = new PaymentsModel()
                    {
                        Id = p.Id,
                        Plan = p.Plan,
                        Annually = p.Annually,
                        CreditCardType = p.CreditCardType,
                        ExpireDate = p.ExpireDate,
                        PaymentDate = p.PaymentDate,
                        PaymentMethod = p.PaymentMethod,
                        Price = p.Price,
                        Status = p.Status,
                        InvoiceNumber = p.InvoiceNumber,
                        LeadsLimit = p.LeadsLimit,
                        PingsLimit = p.PingsLimit,
                        InvoiceUrl = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/Content/Uploads/Pdf/Invoice_" + p.UserId + "." + p.InvoiceNumber + ".pdf"
                    };
                    /*
                    switch (p.Plan)
                    {
                        case 1: { paymentsModel.PlanName = "Starter"; break; } // MUST BE FROM DB
                        case 2: { paymentsModel.PlanName = "Professional"; break; }
                        case 3: { paymentsModel.PlanName = "Enterprise"; break; }
                    }
                    */

                    PlanModel planModel = _planService.GetPlanByID((long)p.Plan);
                    paymentsModel.PlanName = planModel.Name;


                    switch (p.PaymentMethod)
                    {
                        case 1: { paymentsModel.PaymentMethodName = "PayPal"; break; } // MUST BE FROM DB
                        case 2: { paymentsModel.PaymentMethodName = "Credit Card"; break; }
                        case 3: { paymentsModel.PaymentMethodName = "Visa"; break; }
                        case 4: { paymentsModel.PaymentMethodName = "Master"; break; }
                    }
                    

                    getAllPaymentsModel.paymentsModelList.Add(paymentsModel);

                    if(k++ == 0) //first
                    {
                        getAllPaymentsModel.CurrentLeadsLimit = p.PingsLimit;
                        getAllPaymentsModel.CurrentPingsLimit = p.LeadsLimit;
                        getAllPaymentsModel.ExpireDate = p.ExpireDate;


                        List<ReportTotalsByDate> totalsByDate = (List<ReportTotalsByDate>)_reportService.ReportTotalsByDate(
                            p.PaymentDate,
                            DateTime.UtcNow,
                            0,
                            0);

                        if (totalsByDate.Count > 0)
                        {
                            getAllPaymentsModel.RemainingLeadsLimit = getAllPaymentsModel.CurrentLeadsLimit - totalsByDate[0].received;
                            getAllPaymentsModel.RemainingPingsLimit = getAllPaymentsModel.CurrentPingsLimit - totalsByDate[0].posted;
                        }

                        getAllPaymentsModel.CurrentPlanName = paymentsModel.PlanName;
                    }

                }

                IList<UserAddons> userAddonPayments = _addonService.GetAddonsByUserId(_appContext.AppUser.Id);

                List<GetAddonPaymentsModel> AddonPaymentsList = new List<GetAddonPaymentsModel>();
                foreach (UserAddons ua in userAddonPayments)
                {
                    string AddonInvoiceUrl = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + $"/Content/Uploads/Pdf/InvoiceAddon_{ua.UserId}.{ua.Id}.pdf";
                    getAllPaymentsModel.addonPaymentsModelList.Add(new GetAddonPaymentsModel
                    {
                        AddonName = _addonService.GetAddonById(ua.AddonId).Name,
                        ExpireDate = ua.Date.AddMonths(1),
                        PaymentDate = ua.Date,
                        Price = (double)ua.Amount,
                        Status = (short)ua.IsTrial,
                        Id = ua.Id,
                        Annually = false,
                        InvoiceNumber = (int)ua.Id,
                        InvoiceUrl = AddonInvoiceUrl
                    });
                }

                
                return Ok(getAllPaymentsModel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("getPaymentsByUser/{id}")]
        public IHttpActionResult getPaymentsByUser(long id)
        {
            try
            {
                if (!_permissionService.Authorize(_viewBillingHistorKey))
                {
                    return HttpBadRequest("access-denied");
                }
                var payments = _paymentService.GetPaymentsByUser(id);
                return Ok(payments);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("getPaymentById/{id}")]
        public IHttpActionResult GetPaymentById(long id)
        {
            try
            {
                if (!_permissionService.Authorize(_viewBillingHistorKey))
                {
                    return HttpBadRequest("access-denied");
                }
                var payment = _paymentService.GetPaymentById(id);
                return Ok(payment);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("initPayment")]
        public IHttpActionResult InitPayment()
        {
            try
            {
                if (!_permissionService.Authorize(_modifyManageBillingKey))
                {
                    return HttpBadRequest("access-denied");
                }
                BraintreeGateway = GetGateway();
                string client_token = BraintreeGateway.ClientToken.Generate();
                
                return Ok(client_token);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpPost]
        [Route("doPaymentSale")]
        public IHttpActionResult DoPaymentSale([FromBody] PaymentModel paymentModel)
        {
            string paymentMethodNonce = paymentModel.PaymentMethodNonce;
            decimal amount = 1;

            var gateway = GetGateway();

            Profile userProfile = _profileService.GetProfileByUserId(_appContext.AppUser.Id);
            try
            {
                var requestCustomer = new CustomerRequest
                {
                    FirstName = userProfile?.FirstName,
                    LastName = userProfile?.LastName,
                    Company = userProfile?.CompanyName,
                    Email = _appContext.AppUser?.Email,
                    Phone = userProfile?.CellPhone,
                    PaymentMethodNonce = paymentMethodNonce
                };

                var resultCustomer = gateway.Customer.Create(requestCustomer); // Create customer in PayPal

                if (resultCustomer.IsSuccess())
                {
                    var transactionRequest = new TransactionRequest
                    {
                        Amount = amount,
                        PaymentMethodNonce = paymentMethodNonce,
                        Options = new TransactionOptionsRequest
                        {
                            SubmitForSettlement = true
                        }
                    };

                    /*
                    requestPlan = new SubscriptionRequest
                    {
                        PaymentMethodToken = cardToken,
                        PlanId = GetPlanId(paymentModel.Plan, paymentModel.LeadsLimit, paymentModel.PingsLimit, paymentModel.Annually)
                    };

                    var resultPlan = gateway.Subscription.Create(requestPlan); // Create new Subscription
                    */

                    Result<Transaction> result = gateway.Transaction.Sale(transactionRequest);
                    if (result.IsSuccess())
                    {
                        return Ok(result.Message);
                    }
                    else
                    {
                        return Ok(result.Errors);
                    }
                }
                else
                {
                    return Ok(resultCustomer.Message);
                }
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpPost]
        [Route("doPayment")]
        public IHttpActionResult DoPayment([FromBody] PaymentModel paymentModel)
        {
            if (paymentModel.Plan == 0) paymentModel.Plan = 2; //AZ Patch

            int invoiceNumber = 1;

            string paymentMethodNonce = paymentModel.PaymentMethodNonce;
            decimal amount = (decimal)GetPlanPrice((long)paymentModel.Plan);

            var gateway = GetGateway();
            var request = new TransactionRequest
            {
                Amount = amount,
                PaymentMethodNonce = paymentMethodNonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Profile userProfile = _profileService.GetProfileByUserId(_appContext.AppUser.Id);
            Customer BtCustomer = null;

            try
            {
                IList<Payment> payments = _paymentService.GetPaymentsByUser(_appContext.AppUser.Id);

                if (payments != null && payments.Count > 0)
                {
                    invoiceNumber = payments.OrderByDescending(x => x.PaymentDate).FirstOrDefault().InvoiceNumber + 1;
                    BtCustomer = gateway.Customer.Find(payments.OrderByDescending(x => x.PaymentDate).FirstOrDefault().PayPalCustomerId); // 630218192
                    //BtCustomer = gateway.Customer.Find("6302181920"); // 630218192
                }

            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
            
            try
            {
                var requestCustomer = new CustomerRequest
                {
                    FirstName = userProfile?.FirstName,
                    LastName = userProfile?.LastName,
                    Company = userProfile?.CompanyName,
                    Email = _appContext.AppUser?.Email,
                    Phone = userProfile?.CellPhone,
                    PaymentMethodNonce = paymentMethodNonce
                };

                Result<Customer> resultCustomer = null;
                Customer paypalCustomer = null;
                if (BtCustomer == null)
                {
                    resultCustomer = gateway.Customer.Create(requestCustomer); // Create customer in PayPal
                    if (resultCustomer.IsSuccess())
                    {
                        paypalCustomer = resultCustomer.Target;
                    }
                }
                else
                {
                    paypalCustomer = BtCustomer;
                }
                


                if (paypalCustomer != null)
                {
                    // Customer paypalCustomer = null;

                    var cardToken = paypalCustomer.PaymentMethods[0].Token;

                    SubscriptionRequest requestPlan;

                    requestPlan = new SubscriptionRequest
                    {
                        PaymentMethodToken = cardToken,
                        PlanId = GetPlanId(paymentModel.Plan)
                    };
                    
                    var resultPlan = gateway.Subscription.Create(requestPlan); // Create new Subscription

                    if(! resultPlan.IsSuccess())
                    {
                        return Ok("Create new Subscription Error");
                    }


                    Payment payment = new Payment();

                    payment.Annually = paymentModel.Annually;
                    payment.PaymentDate = DateTime.UtcNow;
                    payment.ExpireDate = DateTime.UtcNow.AddMonths(1);
                    payment.InvoiceNumber = invoiceNumber;
                    payment.PaymentMethod = 1;
                    if (BtCustomer != null)
                    {
                        payment.PayPalSubscriptionId = BtCustomer.Id;
                    }
                    else
                    {
                        payment.PayPalSubscriptionId = resultCustomer.Target.Id;
                    }
                    
                    payment.PingsLimit = paymentModel.PingsLimit;
                    payment.LeadsLimit = paymentModel.LeadsLimit;
                    payment.Plan = paymentModel.Plan;
                    payment.Price = (double)amount; //AZ  _paymentService.CalculatePayment(_appContext.AppUser.Id, paymentModel.CouponCode); //AZ  GetPlanPrice(paymentModel.Plan, paymentModel.LeadsLimit, paymentModel.PingsLimit);
                    payment.Status = 1; // 
                    payment.TransactionId = cardToken;
                    payment.UserId = _appContext.AppUser.Id;

                    payment.PayPalCustomerId = paypalCustomer.Id;
                    if (resultPlan.Target.Transactions != null && resultPlan.Target.Transactions.Count > 0)
                    {
                        payment.CreditCardLastFour = resultPlan.Target.Transactions[0].CreditCard.LastFour;
                        payment.CreditCardMaskedNumber = resultPlan.Target.Transactions[0].CreditCard.MaskedNumber;
                        payment.CreditCardType = (int)resultPlan.Target.Transactions[0].CreditCard.CardType;
                        payment.CreditCardCardholderName = resultPlan.Target.Transactions[0].CreditCard.CardholderName??"";
                        payment.CreditCardExpirationDate = resultPlan.Target.Transactions[0].CreditCard.ExpirationDate;
                    }

                    _paymentService.DoPayment(payment);

                    PdfReportCreator creator = new PdfReportInvoice(_planService,_reportService, null);
                    //AZ Babken _paymentService.DisposeCoupon(paymentModel.CouponCode);

                    //string fileName = "/Uploads/Invoice_" + payment.UserId + "." + payment.InvoiceNumber + ".pdf";
                    //string fileName = $"{System.AppDomain.CurrentDomain.BaseDirectory}Content\\Uploads\\Pdf\\Invoice_{payment.UserId}.{ payment.InvoiceNumber}.pdf";
                    // creator.GenerateReport(fileName);

                    string fileName = $"{System.AppDomain.CurrentDomain.BaseDirectory}Content\\Uploads\\Pdf\\Invoice_{payment.UserId}.{ payment.InvoiceNumber}.pdf";
                    PdfReportInvoiceContact invoiceData = new PdfReportInvoiceContact();
                    invoiceData.BuyerName = userProfile.FirstName + " " + userProfile.LastName;
                    invoiceData.InvoiceNumber = payment.InvoiceNumber;
                    invoiceData.CurrentPlanType = payment.Plan.ToString();
                    invoiceData.PlanRate = payment.Price;
                    invoiceData.AmountPaid = payment.Price;
                    invoiceData.CustomerID = _appContext.AppUser.Id;
                    invoiceData.Last4Digits = payment.CreditCardLastFour;
                    invoiceData.PingLimitation = payment.PingsLimit;
                    invoiceData.InvoiceDate = payment.PaymentDate;
                    invoiceData.InvoicePeriodStart = payment.PaymentDate;
                    invoiceData.InvoicePeriodEnd = payment.ExpireDate;
                    invoiceData.Country = "US";
                    invoiceData.SubscriptionFees = payment.Price;
                    invoiceData.PingsIncluded = payment.PingsLimit;
                    invoiceData.Subtotal = payment.Price;
                    invoiceData.TotalDue = payment.Price;



                    creator.GenerateInvoice(fileName, invoiceData);


                    return Ok(payment);
                }
                else
                {
                    return Ok(resultCustomer.Message);
                }
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("doUpgrade")]
        public IHttpActionResult DoUpgrade([FromBody] PaymentModel paymentModel)
        {
            PdfReportCreator creator = new PdfReportInvoice(_planService, _reportService, null);
            int invoiceNumber = 1;

            if (paymentModel.Plan == 0) paymentModel.Plan = 2; //AZ Patch

            decimal amount = (decimal)GetPlanPrice((long)paymentModel.Plan);

            var gateway = GetGateway();

            Profile userProfile = _profileService.GetProfileByUserId(_appContext.AppUser.Id);
            Customer paypalCustomer = null;

            try
            {
                IList<Payment> payments = _paymentService.GetPaymentsByUser(_appContext.AppUser.Id);

                if (payments != null && payments.Count > 0)
                {
                    invoiceNumber = payments.OrderByDescending(x => x.PaymentDate).FirstOrDefault().InvoiceNumber + 1;
                    paypalCustomer = gateway.Customer.Find(payments.OrderByDescending(x => x.PaymentDate).FirstOrDefault().PayPalCustomerId); // 630218192
                    //BtCustomer = gateway.Customer.Find("6302181920"); // 630218192
                }

            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }

            try
            {
                if (paypalCustomer != null)
                {
                    var cardToken = paypalCustomer.PaymentMethods[0].Token;

                    SubscriptionRequest requestPlan;

                    requestPlan = new SubscriptionRequest
                    {
                        PaymentMethodToken = cardToken,
                        PlanId = GetPlanId(paymentModel.Plan)
                    };

                    var resultPlan = gateway.Subscription.Create(requestPlan); // Create new Subscription

                    if (!resultPlan.IsSuccess())
                    {
                        return Ok("Create new Subscription Error");
                    }

                    Payment payment = new Payment();


                    payment.Annually = paymentModel.Annually;
                    payment.PaymentDate = DateTime.UtcNow;
                    payment.ExpireDate = DateTime.UtcNow.AddMonths(1);
                    payment.InvoiceNumber = invoiceNumber;
                    payment.PaymentMethod = 1;
                    
                    payment.PayPalSubscriptionId = paypalCustomer.Id;
                    
                    payment.PingsLimit = paymentModel.PingsLimit;
                    payment.LeadsLimit = paymentModel.LeadsLimit;
                    payment.Plan = paymentModel.Plan;
                    payment.Price = (double)amount; //AZ  _paymentService.CalculatePayment(_appContext.AppUser.Id, paymentModel.CouponCode); //AZ  GetPlanPrice(paymentModel.Plan, paymentModel.LeadsLimit, paymentModel.PingsLimit);
                    payment.Status = 1; // 
                    payment.TransactionId = cardToken;
                    payment.UserId = _appContext.AppUser.Id;

                    payment.PayPalCustomerId = paypalCustomer.Id;
                    if (resultPlan.Target.Transactions != null && resultPlan.Target.Transactions.Count > 0)
                    {
                        payment.CreditCardLastFour = resultPlan.Target.Transactions[0].CreditCard.LastFour;
                        payment.CreditCardMaskedNumber = resultPlan.Target.Transactions[0].CreditCard.MaskedNumber;
                        payment.CreditCardType = (int)resultPlan.Target.Transactions[0].CreditCard.CardType;
                        payment.CreditCardCardholderName = resultPlan.Target.Transactions[0].CreditCard.CardholderName ?? "";
                        payment.CreditCardExpirationDate = resultPlan.Target.Transactions[0].CreditCard.ExpirationDate;
                    }

                    _paymentService.DoPayment(payment);

                    // PdfReportCreator creator = new PdfReportInvoice(_planService, _reportService, null);
                    //AZ Babken _paymentService.DisposeCoupon(paymentModel.CouponCode);

                    //string fileName = "/Uploads/Invoice_" + payment.UserId + "." + payment.InvoiceNumber + ".pdf";
                    //string fileName = $"{System.AppDomain.CurrentDomain.BaseDirectory}Content\\Uploads\\Pdf\\Invoice_{payment.UserId}.{ payment.InvoiceNumber}.pdf";
                    //creator.GenerateReport(fileName);
                    //string fileName = "/Uploads/Invoice_" + 8 + "." + 888 + ".pdf";
                    //string fileName = $"{System.AppDomain.CurrentDomain.BaseDirectory}Content\\Uploads\\Pdf\\Invoice_8.8.pdf";

                    string fileName = $"{System.AppDomain.CurrentDomain.BaseDirectory}Content\\Uploads\\Pdf\\Invoice_{payment.UserId}.{ payment.InvoiceNumber}.pdf";
                    PdfReportInvoiceContact invoiceData = new PdfReportInvoiceContact();
                    invoiceData.BuyerName = userProfile.FirstName + " " + userProfile.LastName;
                    invoiceData.InvoiceNumber = payment.InvoiceNumber;
                    invoiceData.CurrentPlanType = payment.Plan.ToString();
                    invoiceData.PlanRate = payment.Price;
                    invoiceData.AmountPaid = payment.Price;
                    invoiceData.CustomerID = _appContext.AppUser.Id;
                    invoiceData.Last4Digits = payment.CreditCardLastFour;
                    invoiceData.PingLimitation = payment.PingsLimit;
                    invoiceData.InvoiceDate = payment.PaymentDate;
                    invoiceData.InvoicePeriodStart = payment.PaymentDate;
                    invoiceData.InvoicePeriodEnd = payment.ExpireDate;
                    invoiceData.Country = "US";
                    invoiceData.SubscriptionFees = payment.Price;
                    invoiceData.PingsIncluded = payment.PingsLimit;
                    invoiceData.Subtotal = payment.Price;
                    invoiceData.TotalDue = payment.Price;

                    creator.GenerateInvoice(fileName, invoiceData);


                    return Ok(payment);
                }
                else
                {
                    return Ok("Error: Customer not found");
                }
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("doPaymentAddon")]
        public IHttpActionResult DoPaymentAddon(long addonId)
        {
            decimal amount = (decimal)GetAddonPlanPrice(addonId);
            string AddonKey = _addonService.GetAddonById(addonId).Key;

            var gateway = GetGateway();

            Profile userProfile = _profileService.GetProfileByUserId(_appContext.AppUser.Id);
            Customer paypalCustomer = null;

            try
            {
                IList<Payment> payments = _paymentService.GetPaymentsByUser(_appContext.AppUser.Id);

                if (payments != null && payments.Count > 0)
                {
                    paypalCustomer = gateway.Customer.Find(payments.OrderByDescending(x => x.PaymentDate).FirstOrDefault().PayPalCustomerId); // 630218192
                    //BtCustomer = gateway.Customer.Find("6302181920"); // 630218192
                }

            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }

            try
            {
                if (paypalCustomer != null)
                {
                    var cardToken = paypalCustomer.PaymentMethods[0].Token;

                    SubscriptionRequest requestPlan;

                    requestPlan = new SubscriptionRequest
                    {
                        PaymentMethodToken = cardToken,
                        PlanId = GetAddonPlanId(AddonKey)
                    };

                    var resultPlan = gateway.Subscription.Create(requestPlan); // Create new Subscription

                    if (!resultPlan.IsSuccess())
                    {
                        return Ok("Create new Subscription Error");
                    }

                    UserAddons ua = new UserAddons()
                    {
                        Amount = (double)amount,
                        Date = DateTime.UtcNow,
                        IsTrial = 1,
                        Status = 1,
                        AddonId = addonId,
                        UserId = _appContext.AppUser.Id
                    };

                    long userAddonId = _addonService.InsertUserAddon(ua);


                    string fileName = $"{System.AppDomain.CurrentDomain.BaseDirectory}Content\\Uploads\\Pdf\\InvoiceAddon_{_appContext.AppUser.Id}.{userAddonId}.pdf";
                    PdfReportInvoiceContact invoiceData = new PdfReportInvoiceContact();
                    invoiceData.BuyerName = userProfile.FirstName + " " + userProfile.LastName;
                    invoiceData.InvoiceNumber = userAddonId;
                    invoiceData.CurrentPlanType = AddonKey;
                    invoiceData.PlanRate = (double)ua.Amount;
                    invoiceData.AmountPaid = (double)ua.Amount;
                    invoiceData.CustomerID = _appContext.AppUser.Id;
                    invoiceData.Last4Digits = "";
                    invoiceData.InvoiceDate = ua.Date;
                    invoiceData.InvoicePeriodStart = ua.Date;
                    invoiceData.InvoicePeriodEnd = ua.Date.AddMonths(1);
                    invoiceData.Country = "US";
                    invoiceData.SubscriptionFees = (double)ua.Amount;
                    invoiceData.Subtotal = (double)ua.Amount;
                    invoiceData.TotalDue = (double)ua.Amount;


                    PdfReportCreator creator = new PdfReportInvoice(_planService, _reportService, null);
                    creator.GenerateAddonInvoice(fileName, invoiceData);

                    //PdfReportCreator creator = new PdfReportInvoice(_planService, _reportService, null);
                    //AZ Babken _paymentService.DisposeCoupon(paymentModel.CouponCode);

                    string InvoiceUrl = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + $"/Content/Uploads/Pdf/InvoiceAddon_{_appContext.AppUser.Id}.{userAddonId}.pdf";
                    return Ok(new {
                        Amount = (double)amount,
                        Date = DateTime.UtcNow,
                        IsTrial = 1,
                        Status = 1,
                        AddonId = addonId,
                        UserId = _appContext.AppUser.Id,
                        InvoiceUrl = InvoiceUrl
                    });
                }
                else
                {
                    return Ok("Error");
                }
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }

        }

        [HttpPost]
        [Route("cancelSubscription")]
        public IHttpActionResult CancelSubscription()
        {
            if (!_permissionService.Authorize(_modifyManageBillingKey))
            {
                return HttpBadRequest("access-denied");
            }
            Profile userProfile = _profileService.GetProfileByUserId(_appContext.AppUser.Id);

            var payPalSubscribtionId = _paymentService.GetPayPalSubscribtionId(_appContext.AppUser.Id);
            if (string.IsNullOrEmpty(payPalSubscribtionId))
                return Ok("Error");

            var gateway = GetGateway();
            var resSub = gateway.Subscription.Cancel(payPalSubscribtionId);

            if (resSub.IsSuccess() != true)
            {
                return Ok("Error");
            }
            _paymentService.CancelSubscription(payPalSubscribtionId);

            return Ok("OK");
        }

        [HttpPost]
        [Route("getCardInfo")]
        public IHttpActionResult GetCardInfo()
        {
            if (!_permissionService.Authorize(_viewBillingHistorKey))
            {
                return HttpBadRequest("access-denied");
            }
            CreditCard creditCard;
            try
            {
                var payPalSubscribtionId = _paymentService.GetPayPalSubscribtionId(_appContext.AppUser.Id);

                var gateway = GetGateway();

                var paypalCustomer = gateway.Customer.Find(payPalSubscribtionId);

                if (paypalCustomer == null)
                {
                    return Ok("Error");
                }
                var cardToken = paypalCustomer.PaymentMethods[0].Token;
                creditCard = paypalCustomer.CreditCards[0];
            }
            catch (Exception e)
            {
                return HttpBadRequest("PayPal User not found");
            }

            return Ok(creditCard);
        }

        [HttpPost]
        [Route("updateCard")]
        public IHttpActionResult UpdateCard(string cardholderName, string cardNumber, string expirationDate, string cvv)
        {
            if (!_permissionService.Authorize(_viewBillingHistorKey))
            {
                return HttpBadRequest("access-denied");
            }
            string payPalCustomerId = _paymentService.GetPayPalCustomerId(_appContext.AppUser.Id);

            var gateway = GetGateway();

            var paypalCustomer = gateway.Customer.Find(payPalCustomerId);
            var cardToken = paypalCustomer.PaymentMethods[0].Token;

            var creditCardUpdateRequest = new CreditCardRequest
            {
                CardholderName = cardholderName,
                Number = cardNumber,
                ExpirationDate = expirationDate,
                CVV = cvv
                /* ,
                BillingAddress = new CreditCardAddressRequest
                {
                    Region = "IL",
                },
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = false
                }
                */
            };
            var creditCard = gateway.CreditCard.Update(cardToken, creditCardUpdateRequest);
            if (creditCard == null )
            {
                return Ok("Invalid Card");
            }

            return Ok(creditCard.Target);
        }

        [HttpPost]
        [Route("getPingReport")]
        public IHttpActionResult GetPingReport([FromBody] PingReportModel pingReportModel)
        {
            if (!_permissionService.Authorize(_viewPingReportKey))
            {
                return HttpBadRequest("access-denied");
            }
            long parentid = 0;
            if( pingReportModel.buyerId != 0)
            {
                parentid = pingReportModel.buyerId;
            }
            else
            {
                parentid = pingReportModel.affiliateId;
            }

            List<ReportTotalsByDate> totalsByDate = (List<ReportTotalsByDate>)_reportService.ReportTotalsByDate(
                    pingReportModel.startDate,
                    pingReportModel.endDate,
                    pingReportModel.campaignId,
                    parentid);

            return Ok(totalsByDate);
        }

        [HttpGet]
        [Route("notifyPastDueInvoices")]
        public IHttpActionResult NotifyPastDueInvoices()
        {
            if (!_permissionService.Authorize(_viewPingReportKey))
            {
                return HttpBadRequest("access-denied");
            }

            var smtpSetting = this._smtpAccountService.GetSmtpAccount();
            var techContact = _settingService.GetSetting("AppSetting.SupportEmail")?.Value ?? "no-reply@adrack.com";
            List<PaymentPastDays> paymentPastDays = (List<PaymentPastDays>)_paymentService.GetPastDuePayments();
            foreach (var item in paymentPastDays)
            {
                var body = new StringBuilder();

                //body.AppendLine($"Plan: {GetPlanId(item.Payment.Plan, item.Payment.PingsLimit, item.Payment.LeadsLimit.Value, true)}<br />");
                body.AppendLine($"Plan:{33} <br/>");
                body.AppendLine($"Ping Limit: {item.Payment.PingsLimit}<br />");
                body.AppendLine($"Price: {item.Payment.Price}<br />");
                body.AppendLine($"Comment: {item.Payment.LeadsLimit}<br />");

                _emailService.SendEmail(smtpSetting, techContact, "Adrack", item.User.Email, item.User.BuiltInName, "Payment Past due invoice"
                   , body.ToString());
            }
            return Ok();
        }

        #endregion methods
    }
}