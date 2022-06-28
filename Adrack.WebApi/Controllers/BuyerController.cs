using Adrack.Core;
using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Security;
using Adrack.Service.Accounting;
using Adrack.Service.Common;
using Adrack.Service.Content;
using Adrack.Service.Directory;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models.Lead;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Adrack.WebApi.Extensions;
using Adrack.WebApi.Models.Buyers;
using System.Web;
using System.IO;
using Adrack.WebApi.Models;
using Adrack.Core.Infrastructure.Data;
using System.Text;
using Adrack.Service.Configuration;
using Adrack.Web.Framework.Cache;
using Adrack.Data;
using Adrack.Core.Infrastructure;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/buyer")]
    public class BuyerController : BaseApiController
    {
        #region fields

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// The accounting service
        /// </summary>
        private readonly IAccountingService _accountingService;

        /// <summary>
        /// The search service
        /// </summary>
        private readonly ISearchService _searchService;

        /// <summary>
        /// The authentication service
        /// </summary>
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// The buyer channel filter condition service
        /// </summary>
        private readonly IBuyerChannelFilterConditionService _buyerChannelFilterConditionService;

        /// <summary>
        /// The buyer channel service
        /// </summary>
        private readonly IBuyerChannelService _buyerChannelService;

        /// <summary>
        /// The buyer channel template service
        /// </summary>
        private readonly IBuyerChannelTemplateService _buyerChannelTemplateService;

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IBuyerService _buyerService;

        /// <summary>
        /// The campaign service
        /// </summary>
        private readonly ICampaignService _campaignService;

        /// <summary>
        /// The campaign template service
        /// </summary>
        private readonly ICampaignTemplateService _campaignTemplateService;

        /// <summary>
        /// The country service
        /// </summary>
        private readonly ICountryService _countryService;

        /// <summary>
        /// The email service
        /// </summary>
        private readonly IEmailService _emailService;

        /// <summary>
        /// The global attribute service
        /// </summary>
        private readonly IGlobalAttributeService _globalAttributeService;

        /// <summary>
        /// The localized string service
        /// </summary>
        private readonly ILocalizedStringService _localizedStringService;

        /// <summary>
        /// The permission service
        /// </summary>
        private readonly IPermissionService _permissionService;

        /// <summary>
        /// The profile service
        /// </summary>
        private readonly IProfileService _profileService;

        /// <summary>
        /// The role service
        /// </summary>
        private readonly IRoleService _roleService;

        /// <summary>
        /// The state province service
        /// </summary>
        private readonly IStateProvinceService _stateProvinceService;

        /// <summary>
        /// The user registration service
        /// </summary>
        private readonly IUserRegistrationService _userRegistrationService;

        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// The Entity Change History service
        /// </summary>
        private readonly IEntityChangeHistoryService _entityChangeHistoryService;

        /// <summary>
        /// The plan service
        /// </summary>
        private readonly IPlanService _planService;

        private readonly ISmtpAccountService _smtpAccountService;
        private readonly ISettingService _settingService;

        private readonly IStorageService _storageService;

        /// <summary>
        /// The user setting
        /// </summary>
        private readonly UserSetting _userSetting; // TODO ?????

        private readonly IRepository<BuyerInvitation> _buyerInvitationRepository;

        private readonly IJWTTokenService _jWTTokenService;

        private string UploadBuyerIconFolderUrl => $"{Request.RequestUri.GetLeftPart(UriPartial.Authority)}/Content/Uploads/Icons/Buyer/";

        protected string blobPath = "uploads";

        #endregion fields

        #region constructor

        // <param name="departmentService">The department service.</param> IDepartmentService departmentService, 
        /// <summary>
        /// Directory Controller
        /// </summary>
        /// <param name="appContext">Application Context</param>
        /// <param name="accountingService">The accounting service.</param>
        /// <param name="authenticationService">The authentication service.</param>
        /// <param name="buyerChannelFilterConditionService">The buyer channel filter condition service.</param>
        /// <param name="buyerChannelService">The buyer channel service.</param>
        /// <param name="buyerChannelTemplateService">The buyer channel template service.</param>
        /// <param name="buyerService">The buyer service.</param>
        /// <param name="campaignService">The campaign service.</param>
        /// <param name="campaignTemplateService">The campaign template service.</param>
        /// <param name="countryService">Country Service</param>
        /// <param name="emailService">The email service.</param>
        /// <param name="globalAttributeService">The global attribute service.</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="permissionService">The permission service.</param>
        /// <param name="profileService">The profile service.</param>
        /// <param name="roleService">The role service.</param>
        /// <param name="stateProvinceService">State Province Service</param>
        /// <param name="userRegistrationService">The user registration service.</param>
        /// <param name="userService">The users service.</param>
        /// <param name="entityChangeHistoryService">The Entity Change History service.</param>
        /// <param name="userSetting">The user setting.</param>
        /// <param name="planService">The plan service.</param>
        public BuyerController(IAppContext appContext,
            IAccountingService accountingService, IAuthenticationService authenticationService,
            IBuyerChannelFilterConditionService buyerChannelFilterConditionService,
            IBuyerChannelService buyerChannelService, IBuyerChannelTemplateService buyerChannelTemplateService,
            IBuyerService buyerService,
            ICampaignService campaignService, ICampaignTemplateService campaignTemplateService,
            ICountryService countryService, IEmailService emailService,
            IGlobalAttributeService globalAttributeService,
            ILocalizedStringService localizedStringService, IPermissionService permissionService,
            IProfileService profileService, IRoleService roleService, IStateProvinceService stateProvinceService,
            IUserRegistrationService userRegistrationService, IUserService userService,
            IEntityChangeHistoryService entityChangeHistoryService,
            ISearchService searchService,
            IRepository<BuyerInvitation> buyerInvitationRepository,
            UserSetting userSetting,
            ISmtpAccountService smtpAccountService,
            ISettingService settingService,
            IPlanService planService,
            IJWTTokenService jWTTokenService,
            IStorageService storageService) :base()
        {
            this._appContext = appContext;
            this._accountingService = accountingService;
            this._authenticationService = authenticationService;
            this._buyerChannelFilterConditionService = buyerChannelFilterConditionService;
            this._buyerChannelService = buyerChannelService;
            this._buyerChannelTemplateService = buyerChannelTemplateService;
            this._buyerService = buyerService;
            this._campaignService = campaignService;
            this._campaignTemplateService = campaignTemplateService;
            this._countryService = countryService;
            this._emailService = emailService;
            this._globalAttributeService = globalAttributeService;
            this._localizedStringService = localizedStringService;
            this._permissionService = permissionService;
            this._profileService = profileService;
            this._roleService = roleService;
            this._stateProvinceService = stateProvinceService;
            this._userRegistrationService = userRegistrationService;
            this._userService = userService;
            this._entityChangeHistoryService = entityChangeHistoryService;
            this._userSetting = userSetting;
            this._searchService = searchService;
            this._buyerInvitationRepository = buyerInvitationRepository;
            this._smtpAccountService = smtpAccountService;
            this._settingService = settingService;
            this._planService = planService;
            this._jWTTokenService = jWTTokenService;
            this._storageService = storageService;
        }

        #endregion constructor

        #region methods

        #region route methods

        /// <summary>
        /// Get All Buyers
        /// </summary>
        /// <returns></returns>
        [Route("getBuyers")]
        [ContentManagementCache("App.Cache.Buyer.")]
        public IHttpActionResult GetBuyers(EntityFilterByStatus status = EntityFilterByStatus.All)
        {
            try
            {
                var buyerModels = new List<BuyerResponseModel>();
                var buyers = _buyerService.GetAllBuyersByStatus(status);
                foreach (var buyer in buyers)
                {
                    var createdBy = string.Empty;
                    var createdHistoryObj = _entityChangeHistoryService.GetEntityHistory(buyer.Id, "Buyer", "Added");
                    if (createdHistoryObj != null)
                        createdBy = _userService.GetUserById(createdHistoryObj.UserId)?.Username;

                    var updatedBy = string.Empty;
                    var updatedHistoryObj = _entityChangeHistoryService.GetEntityHistory(buyer.Id, "Buyer", "Modified");
                    if (updatedHistoryObj != null)
                        updatedBy = _userService.GetUserById(updatedHistoryObj.UserId)?.Username;


                    DateTime? createDate = null;
                    if (createdHistoryObj != null)
                        createDate = _settingService.GetTimeZoneDate(createdHistoryObj.ModifiedDate);

                    DateTime? updateDate = null;
                    if (updatedHistoryObj != null)
                        updateDate = _settingService.GetTimeZoneDate(updatedHistoryObj.ModifiedDate);


                    var manager = _userService.GetUserById(buyer.ManagerId ?? 0);

                    buyerModels.Add((new BuyerResponseModel
                    {
                        Id = buyer.Id,
                        Name = buyer.Name,
                        TypeId = buyer.AlwaysSoldOption,
                        Country = buyer.Country?.Name,
                        City = buyer.City,
                        State = buyer.StateProvince?.Name,
                        ZipCode = buyer.ZipPostalCode,
                        Phone = buyer.Phone,
                        Manager = manager?.Username,
                        Status = buyer.Status,
                        IconPath = buyer.IconPath,
                        CreatedDate = createDate,
                        CreatedBy = createdBy,
                        UpdatedDate = updateDate,
                        UpdatedBy = updatedBy,
                        SendStatementReport = buyer.SendStatementReport.HasValue ? buyer.SendStatementReport.Value : false
                    }).AbsolutePathBuilder(Request));
                }
                return Ok(buyerModels);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Get Buyer By Id
        /// </summary>
        /// <param name="id">long</param>
        /// <returns></returns>
        [Route("getBuyerById/{id}")]
        public IHttpActionResult GetBuyerById(long id)
        {
            if (!_permissionService.Authorize("view-information-buyer"))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var buyer = _buyerService.GetBuyerById(id);

                if (buyer == null)
                {
                    return HttpBadRequest($"no buyer was found for given id {id}");
                }

                var buyerResponseModel = ((BuyerModel)buyer).AbsolutePathBuilder(Request);

                BuyerBalance buyerBalance = _accountingService.GetBuyerBalanceById(buyer.Id);
                if (buyerBalance != null)
                {
                    buyerResponseModel.Credit = buyerBalance.Credit;
                }

                return Ok(buyerResponseModel);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Get Buyers By Input Value
        /// </summary>
        /// <param name="inputValue">string</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getBuyersBySearchPattern")]
        [ContentManagementCache("App.Cache.Buyer.")]
        public IHttpActionResult GetBuyersBySearchPattern(string inputValue)
        {
            if (!_permissionService.Authorize("view-information-buyer"))
            {
                return HttpBadRequest("access-denied");
            }
            var buyerViewModels = new List<BuyerViewModel>();
            var buyers = _buyerService.GetAllBuyers();
            foreach (var buyer in buyers)
            {
                var buyerModel = CreateBuyerViewModel(buyer);
                if (_searchService.CheckPropValue(buyerModel, inputValue))
                {
                    buyerViewModels.Add(buyerModel);
                }
            }
            return Ok(buyerViewModels);
        }

        [HttpPost]
        [Route("deleteOrRestoreBuyer/{id}")]
        public IHttpActionResult UpdateBuyerChannelIntegrationRow([FromUri]long id, BuyerDeleteModel deleteModel)
        {
            if (!_permissionService.Authorize("edit-information-buyer"))
            {
                return HttpBadRequest("access-denied");
            }
            var buyer = _buyerService.GetBuyerByIdWithDeletedStatus(id,true);
            if (buyer == null)
            {
                return HttpBadRequest($"Buyer with {id} Id  doesn't exist");
            }
            switch (deleteModel.DeleteStatus)
            {
                case DeletedStatus.Deleted:
                    buyer.Deleted = true;
                    break;
                case DeletedStatus.NotDeleted:
                    buyer.Deleted = false;
                    break;
                default:
                    return HttpBadRequest("Wrong status provided");
            }

            _buyerService.UpdateBuyer(buyer);

            return Ok();
        }

        /// <summary>
        /// Create Buyer
        /// </summary>
        /// <param name="buyerModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("createBuyer")]
        public IHttpActionResult CreateBuyer([FromBody]BuyerModel buyerModel)
        {
            if (!_permissionService.Authorize("edit-information-buyer"))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                if (buyerModel == null)
                {
                    return HttpBadRequest($"buyer channel model is null");
                }

                var existedBuyer = _buyerService.GetBuyerByName(buyerModel.Name, 0);

                if (existedBuyer != null)
                {
                    return HttpBadRequest("Buyer name already exists");
                }

                if (buyerModel.CountryId == 0)
                {
                    return HttpBadRequest($"country is a required field");
                }

                var validationMessage = ValidateDirectory(buyerModel.CountryId, buyerModel.StateProvinceId);
                if (!string.IsNullOrEmpty(validationMessage))
                {
                    return HttpBadRequest(validationMessage);
                }

                var buyer = (Buyer)buyerModel;

                var id = _buyerService.InsertBuyer(buyer);

                BuyerBalance buyerBalance = new BuyerBalance();
                buyerBalance.BuyerId = buyer.Id;
                buyerBalance.Credit = buyerModel.Credit ?? 0;
                buyerBalance.Balance = buyerModel.Credit ?? 0;
                buyerBalance.PaymentSum = 0;
                buyerBalance.SoldSum = 0;
                _accountingService.InsertBuyerBalance(buyerBalance);

                var invitations = new List<BuyerInvitationModel>();

                if (buyerModel.Invitations != null)
                {
                    var currentInvitations = _buyerService.GetBuyerInvitations(buyer.Id);
                    List<string> alreadySentEmails = new List<string>();

                    foreach (var buyerInvitationModel in buyerModel.Invitations)
                    {
                        if (alreadySentEmails.Contains(buyerInvitationModel.RecipientEmail))
                            continue;

                        var buyerInvitation = new BuyerInvitation()
                        {
                            BuyerId = buyer.Id,
                            InvitationDate = DateTime.UtcNow,
                            RecipientEmail = buyerInvitationModel.RecipientEmail,
                            Status = buyerInvitationModel.Status,
                            Role = (short)buyerInvitationModel.Role
                        };

                        var newInvitationId = _buyerService.InsertBuyerInvitation(buyerInvitation);

                        invitations.Add(new BuyerInvitationModel()
                        {
                            BuyerId = buyer.Id,
                            BuyerInvitationId = newInvitationId,
                            InvitationDate = buyerInvitation.InvitationDate,
                            RecipientEmail = buyerInvitation.RecipientEmail,
                            Role = buyerInvitation.Role,
                            Status = buyerInvitation.Status
                        });

                        if (buyerModel.CanSendEmail)
                        {
                            string invitedUserToken = Guid.NewGuid().ToString();
                            string extraData = buyerInvitation.RecipientEmail + ";" + buyerInvitation.Role + ";" + (short)UserTypes.Buyer + ";" + buyerInvitation.BuyerId;
                            _globalAttributeService.SaveGlobalAttributeOnlyKeyAndValue(GlobalAttributeBuiltIn.InvitedUserToken, invitedUserToken, extraData);
                            _emailService.SendUserInvitationMessage(buyerInvitation.RecipientEmail, invitedUserToken, _appContext.AppLanguage.Id, "buyer", buyer.Name);
                        }

                        alreadySentEmails.Add(buyerInvitationModel.RecipientEmail);
                    }
                }

                var buyerModelResult = (BuyerModel)buyer;

                buyerModelResult.Invitations = invitations;

                var access = new EntityOwnership
                {
                    Id = 0,
                    UserId = _appContext.AppUser.Id,
                    EntityId = buyer.Id,
                    EntityName = EntityType.Buyer.ToString()
                };
                _userService.InsertEntityOwnership(access);

                //User Invitation

                return Ok(buyerModelResult);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Update Buyer Contract Information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="buyerModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateBuyerContactInformation/{id}")]
        public IHttpActionResult UpdateBuyerContactInformation(long id, [FromBody]BuyerContactInformationModel buyerModel)
        {
            if (!_permissionService.Authorize("edit-information-buyer"))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                if (buyerModel == null)
                {
                    return HttpBadRequest($"buyer channel model is null");
                }

                var buyer = _buyerService.GetBuyerById(id);

                if (buyer == null)
                {
                    return HttpBadRequest($"no buyer was found for given id {id}");
                }

                if (buyerModel.CountryId == 0)
                {
                    return HttpBadRequest($"country is a required field");
                }

                var validationMessage = ValidateDirectory(buyerModel.CountryId, buyerModel.StateProvinceId);
                if (!string.IsNullOrEmpty(validationMessage))
                {
                    return HttpBadRequest(validationMessage);
                }

                buyer.Email = buyerModel.Email;
                buyer.Phone = buyerModel.Phone;
                buyer.ManagerId = buyerModel.ManagerId;
                buyer.CountryId = buyerModel.CountryId;
                buyer.StateProvinceId = buyerModel.StateProvinceId != 0 ? buyerModel.StateProvinceId : null;
                buyer.City = buyerModel.City;
                buyer.AddressLine1 = buyerModel.AddressLine1;
                buyer.AddressLine2 = buyerModel.AddressLine2;
                buyer.ZipPostalCode = buyerModel.ZipPostalCode;

                _buyerService.UpdateBuyer(buyer);

                var buyerResponseModel = ((BuyerModel) buyer).AbsolutePathBuilder(Request);
                return Ok(buyerResponseModel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        /// <summary>
        /// Update Buyer Billing Setting Data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="buyerModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateBillingSetting/{id}")]
        public IHttpActionResult UpdateBillingSetting(long id, [FromBody] BuyerBillingSettingModel buyerModel)
        {
            if (!_permissionService.Authorize("edit-information-buyer"))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                if (buyerModel == null)
                {
                    return HttpBadRequest($"buyer billing setting model is null");
                }

                var buyer = _buyerService.GetBuyerById(id);

                if (buyer == null)
                {
                    return HttpBadRequest($"no buyer was found for given id {id}");
                }


                buyer.AutosendInvoice = buyerModel.AutosendInvoice;
                buyer.BillFrequency = buyerModel.BillFrequency;
                buyer.FrequencyValue = buyerModel.FrequencyValue;
                _buyerService.UpdateBuyer(buyer);


                BuyerBalance buyerBalance = _accountingService.GetBuyerBalanceById(id);
                if (buyerBalance == null)
                {
                    buyerBalance = new BuyerBalance
                    {
                         BuyerId = id
                    };
                    _accountingService.InsertBuyerBalance(buyerBalance);
                }

                buyerBalance.Credit = buyerModel.Credit;
                _accountingService.UpdateBuyerBalance(buyerBalance, "Credit");


                return Ok(buyerBalance);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }




        /// <summary>
        /// Update Buyer Identification Information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="buyerModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateBuyerIdentification/{id}")]
        public IHttpActionResult UpdateBuyerIdentification(long id, [FromBody]BuyerIdentificationModel buyerModel)
        {
            if (!_permissionService.Authorize("edit-information-buyer"))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                if (buyerModel == null)
                {
                    return HttpBadRequest($"buyer channel model is null");
                }

                var buyer = _buyerService.GetBuyerById(id);

                if (buyer == null)
                {
                    return HttpBadRequest($"no buyer was found for given id {id}");
                }


                buyer.AlwaysSoldOption = buyerModel.TypeId;
                buyer.Status = buyerModel.Status;
                buyer.SendStatementReport = buyerModel.SendStatementReport;

                _buyerService.UpdateBuyer(buyer);

                var buyerResponseModel = ((BuyerModel)buyer).AbsolutePathBuilder(Request);
                return Ok(buyerResponseModel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("updateBuyerName/{id}")]
        public IHttpActionResult UpdateBuyerName(long id, [FromBody] BuyerNameModel buyerModel)
        {
            if (!_permissionService.Authorize("edit-information-buyer"))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                if (buyerModel == null)
                {
                    return HttpBadRequest($"buyer channel model is null");
                }

                var buyer = _buyerService.GetBuyerById(id);

                if (buyer == null)
                {
                    return HttpBadRequest($"no buyer was found for given id {id}");
                }

                var existedBuyer = _buyerService.GetBuyerByName(buyerModel.Name, 0);

                if (existedBuyer != null)
                {
                    return HttpBadRequest("Buyer name already exists");
                }

                buyer.Name = buyerModel.Name;

                _buyerService.UpdateBuyer(buyer);

                var buyerResponseModel = ((BuyerModel)buyer).AbsolutePathBuilder(Request);
                return Ok(buyerResponseModel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Update Buyer DNP Information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="buyerModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateBuyerDNP/{id}")]
        public IHttpActionResult UpdateBuyerDNP(long id, [FromBody]BuyerDNPModel buyerModel)
        {
            if (!_permissionService.Authorize("edit-information-buyer"))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                if (buyerModel == null)
                {
                    return HttpBadRequest($"buyer channel model is null");
                }

                var buyer = _buyerService.GetBuyerById(id);

                if (buyer == null)
                {
                    return HttpBadRequest($"no buyer was found for given id {id}");
                }

                buyer.DoNotPresentPostMethod = buyerModel.DoNotPresentPostMethod;
                buyer.DoNotPresentRequest = buyerModel.DoNotPresentRequest;
                buyer.DoNotPresentResultField = buyerModel.DoNotPresentResultField;
                buyer.DoNotPresentResultValue = buyerModel.DoNotPresentResultValue;
                buyer.DoNotPresentStatus = buyerModel.DoNotPresentStatus;
                buyer.DoNotPresentUrl = buyerModel.DoNotPresentUrl;

                _buyerService.UpdateBuyer(buyer);

                var buyerResponseModel = ((BuyerModel)buyer).AbsolutePathBuilder(Request);
                return Ok(buyerResponseModel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("updateBuyerPauseSettings/{id}")]
        public IHttpActionResult UpdateBuyerPauseSettings(long id, [FromBody] BuyerSettingsModel buyerModel)
        {
            if (!_permissionService.Authorize("edit-information-buyer"))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                if (buyerModel == null)
                {
                    return HttpBadRequest($"buyer channel model is null");
                }

                var buyer = _buyerService.GetBuyerById(id);

                if (buyer == null)
                {
                    return HttpBadRequest($"no buyer was found for given id {id}");
                }

                buyer.CanSendLeadId = buyerModel.KeepConsistentLeadId;
                buyer.MaxDuplicateDays = buyerModel.MaxDuplicateDays;
                buyer.DailyCap = buyerModel.DailyCap;
                buyer.CoolOffEnabled = buyerModel.CoolOffEnabled;
                if (buyerModel.CoolOffStart.HasValue)
                    buyer.CoolOffStart = buyerModel.CoolOffStart.Value;
                if (buyerModel.CoolOffEnd.HasValue)
                    buyer.CoolOffEnd = buyerModel.CoolOffEnd.Value;

                _buyerService.UpdateBuyer(buyer);

                var buyerResponseModel = ((BuyerModel)buyer).AbsolutePathBuilder(Request);
                return Ok(buyerResponseModel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        /// <summary>
        /// Update Buyer Status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatus/{id}")]
        public IHttpActionResult UpdateBuyerStatus([FromUri] long id, BuyerStatusUpdateModel model)
        {
            if (!_permissionService.Authorize("edit-information-buyer"))
            {
                return HttpBadRequest("access-denied");
            }
            var buyer = _buyerService.GetBuyerById(id);
            if (buyer == null)
            {
                return HttpBadRequest($"no buyer was found for given id {id}");
            }

            buyer.Status = (short)model.Status;
            _buyerService.UpdateBuyer(buyer);

            var buyerResponseModel = ((BuyerModel)buyer).AbsolutePathBuilder(Request);

            return Ok(buyerResponseModel);
        }

        /// <summary>
        /// Delete Buyer
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpDelete]
        [Route("deleteBuyer/{id}")]
        public IHttpActionResult DeleteBuyer([FromUri]long id)
        {
            if (!_permissionService.Authorize("edit-information-buyer"))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var validationMessage = "";
                var buyer = _buyerService.GetBuyerById(id);
                if (buyer == null)
                {
                    return HttpBadRequest($"no buyer was found for given id {id}");
                }

                validationMessage = CheckCanDelete(id);
                if (!string.IsNullOrEmpty(validationMessage))
                {
                    return HttpBadRequest(validationMessage);
                }

                buyer.Deleted = true;
                _buyerService.UpdateBuyer(buyer);

                return Ok();
            }
            catch (Exception e)
            {
                throw e;
            }
        }





        ///// <summary>
        ///// Billings the specified identifier.
        ///// </summary>
        ///// <param name="id">The identifier.</param>
        ///// <returns>ActionResult.</returns>
        ////[NavigationBreadCrumb(Clear = false, Label = "Buyer")]
        //[Route("{id}")]
        //public BuyerModel Billing([FromUri]long id = 0)
        //{
        //    if (this._appContext.AppUser != null && this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
        //    {
        //        id = this._appContext.AppUser.ParentId;
        //    }

        //    Buyer buyer = this._buyerService.GetBuyerById(id);

        //    var buyerModel = new BuyerModel { BuyerId = 0, UserType = _appContext.AppUser?.UserType ?? 0 };

        //    //ViewBag.SelectedBuyerId = id;

        //    if (buyer != null)
        //    {
        //        buyerModel.BuyerId = buyer.Id;
        //        buyerModel.CountryId = buyer.CountryId;
        //        buyerModel.StateProvinceId = buyer.StateProvinceId;
        //        buyerModel.Name = buyer.Name;
        //        buyerModel.AddressLine1 = buyer.AddressLine1;
        //        buyerModel.AddressLine2 = buyer.AddressLine2;
        //        buyerModel.City = buyer.City;
        //        buyerModel.CompanyEmail = buyer.Email;
        //        buyerModel.Phone = buyer.Phone;
        //        buyerModel.ZipPostalCode = buyer.ZipPostalCode;
        //        buyerModel.Status = buyer.Status;
        //        buyerModel.BillFrequency = buyer.BillFrequency;
        //        buyerModel.FrequencyValue = buyer.FrequencyValue ?? 0;
        //        buyerModel.Credit = this._accountingService.GetBuyerCredit(buyer.Id);
        //        buyerModel.ManagerId = buyer.ManagerId ?? 0L;
        //    }

        //    PrepareModel(buyerModel);

        //    //return View(am);
        //    return buyerModel;
        //}

        ///// <summary>
        ///// Handles buyer submit action
        ///// </summary>
        ///// <param name="buyerModel">BuyerModel reference</param>
        ///// <param name="returnUrl">Redirect url after success</param>
        ///// <returns>ActionResult.</returns>
        //[HttpPost]
        ////[PublicAntiForgery]
        ////[ValidateInput(false)]
        //[Route("item")]
        //public BuyerModel Billing([FromBody]BuyerModel buyerModel, string returnUrl)
        //{
        //    Buyer buyer = _buyerService.GetBuyerById(buyerModel.BuyerId);

        //    buyer.BillFrequency = buyerModel.BillFrequency;
        //    buyer.FrequencyValue = buyerModel.FrequencyValue;

        //    _buyerService.UpdateBuyer(buyer);

        //    BuyerBalance buyerBalance = _accountingService.GetBuyerBalanceById(buyerModel.BuyerId);

        //    if (buyerBalance == null)
        //    {
        //        buyerBalance = new BuyerBalance
        //        {
        //            BuyerId = buyer.Id
        //        };
        //        _accountingService.InsertBuyerBalance(buyerBalance);
        //    }

        //    buyerBalance.Credit = buyerModel.Credit;

        //    _accountingService.UpdateBuyerBalance(buyerBalance, "Credit");

        //    buyerModel.BuyerId = buyer.Id;

        //    PrepareModel(buyerModel);

        //    return buyerModel;
        //    //return View("List");
        //}

        ///// <summary>
        ///// Displays buyer create/edit partial interface
        ///// </summary>
        ///// <param name="id">Buyer id</param>
        ///// <returns>PartialView result</returns>
        ////[NavigationBreadCrumb(Clear = true, Label = "Buyer")]
        //[Route("{id}/item")]
        //public BuyerModel PartialItem([FromUri]long id) // PartialItem2
        //{
        //    Buyer buyer = this._buyerService.GetBuyerById(id);

        //    var buyerModel = new BuyerModel { BuyerId = 0, UserType = _appContext.AppUser?.UserType ?? 0L };


        //    if (buyer != null)
        //    {
        //        buyerModel.BuyerId = buyer.Id;
        //        buyerModel.CountryId = buyer.CountryId;
        //        buyerModel.StateProvinceId = buyer.StateProvinceId;
        //        buyerModel.Name = buyer.Name;
        //        buyerModel.AddressLine1 = buyer.AddressLine1;
        //        buyerModel.AddressLine2 = buyer.AddressLine2;
        //        buyerModel.City = buyer.City;
        //        buyerModel.CompanyEmail = buyer.Email;
        //        buyerModel.CompanyPhone = buyer.Phone;
        //        buyerModel.ZipPostalCode = buyer.ZipPostalCode;
        //        buyerModel.Status = buyer.Status;
        //        buyerModel.BillFrequency = buyer.BillFrequency;
        //        buyerModel.FrequencyValue = buyer.FrequencyValue ?? 0;
        //        buyerModel.Credit = this._accountingService.GetBuyerCredit(buyer.Id);
        //        buyerModel.ManagerId = buyer.ManagerId ?? 0L;
        //        buyerModel.AlwaysSoldOption = buyer.AlwaysSoldOption;
        //        buyerModel.Description = buyer.Description;
        //        buyerModel.CoolOffEnabled = buyer.CoolOffEnabled ?? false;
        //        buyerModel.CoolOffStart = buyer.CoolOffStart ?? DateTime.UtcNow;
        //        buyerModel.CoolOffEnd = buyer.CoolOffEnd ?? DateTime.UtcNow;
        //    }

        //    PrepareModel(buyerModel);

        //    //return View(am);
        //    return Item(buyerModel); //PartialView("Item", buyerModel);
        //}


        ///// <summary>
        ///// Dashboards the specified identifier.
        ///// </summary>
        ///// <param name="id">The identifier.</param>
        ///// <returns>ActionResult.</returns>
        //[Route("{id}/dashboard")]
        //public BuyerModel Dashboard(long id)
        //{
        //    string buyerCompanyName = null;
        //    if (id == 0 && _appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
        //    {
        //        id = _appContext.AppUser.ParentId;
        //        Buyer buyer = _buyerService.GetBuyerById(_appContext.AppUser.ParentId);
        //        buyerCompanyName = (!string.IsNullOrEmpty(buyer?.Name) ? $"Buyer > {buyer.Name} > " : string.Empty);
        //    }

        //    var buyerModel = new BuyerModel { BuyerId = id, Name = buyerCompanyName };
        //    //ViewBag.SelectedBuyerId = id;
        //    //ViewBag.BuyerCompanyName = "";

        //    return buyerModel;//View(buyerModel);
        //}

        ///// <summary>
        ///// Partials the dashboard.
        ///// </summary>
        ///// <param name="id">The identifier.</param>
        ///// <returns>ActionResult.</returns>
        //[Route("dashboard")]
        //public BuyerModel PartialDashboard([FromBody]BuyerModel buyerModel)
        //{
        //    return buyerModel;
        //}

        ///// <summary>
        ///// Partials the dashboard.
        ///// </summary>
        ///// <param name="id">The identifier.</param>
        ///// <returns>ActionResult.</returns>
        //[Route("{id}/dashboard/partial")]
        //public BuyerModel PartialDashboard(long id)
        //{
        //    if (id == 0 && _appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
        //        id = _appContext.AppUser.ParentId;

        //    var buyerModel = new BuyerModel { BuyerId = id };
        //    //ViewBag.SelectedBuyerId = id;

        //    return PartialDashboard(buyerModel); //PartialView("PartialDashboard", m);
        //}

        [HttpGet]
        [HttpPost]
        [Route("loginAs/{id}")]
        public IHttpActionResult LoginAs(long id)
        {
            if (!_permissionService.Authorize("view-information-buyer"))
            {
                return HttpBadRequest("access-denied");
            }

            Buyer buyer = _buyerService.GetBuyerById(id, false);

            if (buyer != null)
            {
                long? userId = 0;
                var entities = _userService.GetEntityOwnership("buyer", buyer.Id);
                if (entities.Count > 0)
                    userId = entities.Select(x => x.UserId).FirstOrDefault();

                if (userId.HasValue)
                {
                    var user = _userService.GetUserById(userId.Value);

                    if (user != null || !user.Active || user.Deleted)
                    {
                        _authenticationService.SignIn(user, false);

                        var refreshTokens = _jWTTokenService.GetAllRefreshTokens() ?? new Dictionary<long, string>();
                        var accessToken = _jWTTokenService.GenerateAccessToken(user.Id, user.Username);
                        var newRefreshToken = _jWTTokenService.GenerateRefreshToken(user.Id, user.Username);
                        refreshTokens[user.Id] = newRefreshToken;
                        _jWTTokenService.InsertRefreshToken(refreshTokens);

                        _appContext.AppUser = user;

                        return Ok(new
                        {
                            accessToken = accessToken,
                            refreshToken = newRefreshToken
                        });
                    }
                }
            }

            return HttpBadRequest(""); // Redirect(Helper.GetBaseUrl(Request) + "/Management/Home/Dashboard");
        }

        ///// <summary>
        ///// Bulks the filter changes.
        ///// </summary>
        ///// <returns>ActionResult.</returns>
        //[Route("filter")]
        //public BuyerReportModel BulkFilterChanges()
        //{
        //    var model = new BuyerReportModel { BaseUrl = Request.GetBaseUrl() };

        //    var campaigns = (List<Campaign>)_campaignService.GetAllCampaigns(0);

        //    foreach (var c in campaigns)
        //    {
        //        model.ListCampaigns.Add(new SelectItem { Text = c.Name, Value = c.Id.ToString() });
        //    }

        //    return model;// View(model);
        //}

        ///// <summary>
        ///// Applies the bulk filters.
        ///// </summary>
        ///// <returns>ActionResult.</returns>
        //[HttpPost]
        ////[ContentManagementAntiForgery(true)]
        //[Route("filter/bulk")]
        //public List<string> ApplyBulkFilters([FromBody]string buyerChannelIds, [FromBody]string filter)
        //{
        //    var messages = new List<string>();

        //    dynamic filters = JsonConvert.DeserializeObject(filter);

        //    string[] buyerChannelIdStrings = buyerChannelIds.Split(new char[1] { ',' });

        //    for (int j = 0; j < buyerChannelIdStrings.Length; j++)
        //    {
        //        BuyerChannel buyerChannel = _buyerChannelService.GetBuyerChannelById(long.Parse(buyerChannelIdStrings[j]));
        //        if (buyerChannel == null) continue;

        //        var zipFound = false;
        //        var ageFound = false;
        //        var stateFound = false;

        //        for (int i = 0; i < filters.Count; i++)
        //        {
        //            string field = filters[i]["field"].ToString();
        //            string condition = filters[i]["condition"].ToString();
        //            string value = filters[i]["value"].ToString();

        //            long campaignTemplateId = long.Parse(field);

        //            CampaignTemplate campaignTemplate = _campaignTemplateService.GetCampaignTemplateById(campaignTemplateId);

        //            if (campaignTemplate == null) continue;

        //            BuyerChannelTemplate buyerChannelTemplate = _buyerChannelTemplateService.GetBuyerChannelTemplate(campaignTemplateId);

        //            if (buyerChannelTemplate == null)
        //            {
        //                messages.Add($"Campaign field '{campaignTemplate.TemplateField}' is not integrated with it's corresponding buyer channel's field");
        //                continue;
        //            }

        //            var items = _buyerChannelFilterConditionService.GetFilterConditionsByBuyerChannelIdAndCampaignTemplateId(buyerChannel.Id, campaignTemplateId);
        //            BuyerChannelFilterCondition filterCondition = null;

        //            if (items.Count > 0)
        //                filterCondition = items[0];

        //            var isNew = false;
        //            if (filterCondition == null)
        //            {
        //                isNew = true;
        //                filterCondition = new BuyerChannelFilterCondition();
        //            }

        //            if (campaignTemplate.Validator == 7)
        //            {
        //                zipFound = true;
        //                buyerChannel.EnableZipCodeTargeting = true;
        //                buyerChannel.ZipCodeTargeting = value;
        //                buyerChannel.ZipCodeCondition = short.Parse(condition);
        //            }
        //            else if (campaignTemplate.Validator == 11)
        //            {
        //                stateFound = true;
        //                buyerChannel.EnableStateTargeting = true;
        //                buyerChannel.StateTargeting = value;
        //                buyerChannel.StateCondition = short.Parse(condition);
        //            }
        //            else if (campaignTemplate.Validator == 14)
        //            {
        //                ageFound = true;
        //                buyerChannel.EnableAgeTargeting = true;

        //                string[] values = value.Split(new char[1] { '-' });

        //                short v = 0;
        //                if (values.Length > 0)
        //                    short.TryParse(values[0], out v);

        //                short v2 = 0;
        //                if (values.Length > 1)
        //                    short.TryParse(values[1], out v2);

        //                buyerChannel.MinAgeTargeting = v;
        //                buyerChannel.MaxAgeTargeting = v2;
        //            }

        //            filterCondition.BuyerChannelId = buyerChannel.Id;
        //            filterCondition.Condition = short.Parse(condition);
        //            filterCondition.Value = value.Trim();
        //            filterCondition.ConditionOperator = short.Parse(condition);
        //            filterCondition.CampaignTemplateId = campaignTemplateId;
        //            if (isNew)
        //                _buyerChannelFilterConditionService.InsertFilterCondition(filterCondition);
        //            else
        //                _buyerChannelFilterConditionService.UpdateFilterCondition(filterCondition);
        //        }

        //        if (!zipFound)
        //        {
        //            buyerChannel.EnableZipCodeTargeting = false;
        //            buyerChannel.ZipCodeTargeting = string.Empty;
        //        }

        //        if (!stateFound)
        //        {
        //            buyerChannel.EnableStateTargeting = false;
        //            buyerChannel.StateTargeting = string.Empty;
        //        }

        //        if (!ageFound)
        //        {
        //            buyerChannel.EnableAgeTargeting = false;
        //            buyerChannel.MaxAgeTargeting = 0;
        //            buyerChannel.MinAgeTargeting = 0;
        //        }

        //        messages.Add($"Filters for '{buyerChannel.Name}' are updated.");
        //    }

        //    return messages; //Json(results, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        [Route("inviteUser")]
        public IHttpActionResult AddBuyerInvitation([FromBody] List<BuyerInvitationModel> buyerInvitationModel)
        {
            if (!_permissionService.Authorize("edit-invitation-buyer"))
            {
                //return HttpBadRequest("access-denied");
            }
            {
                buyerInvitationModel.ForEach(buyerInvitation =>
                {
                    if (buyerInvitation.BuyerInvitationId == 0)
                    {
                        var invitationList = _buyerService.GetBuyerInvitations(buyerInvitation.BuyerId);
                        if (invitationList.All(x => x.RecipientEmail.ToLower().Trim() != buyerInvitation.RecipientEmail.ToLower().Trim()))
                        {
                            if (_userService.GetUserByEmail(buyerInvitation.RecipientEmail.ToLower().Trim()) == null)
                            {
                                buyerInvitation.InvitationDate = DateTime.UtcNow;
                                buyerInvitation.Status = BuyerInvitationStatuses.Pending;

                                var newInvitationId = _buyerService.InsertBuyerInvitation(new BuyerInvitation()
                                {
                                    BuyerId = buyerInvitation.BuyerId,
                                    InvitationDate = DateTime.UtcNow,
                                    RecipientEmail = buyerInvitation.RecipientEmail,
                                    Status = buyerInvitation.Status,
                                    Role = buyerInvitation.Role
                                });
                                buyerInvitation.BuyerInvitationId = newInvitationId;

                                var buyer = _buyerService.GetBuyerById(buyerInvitation.BuyerId, true);

                                if (buyerInvitation.CanSendEmail)
                                {
                                    string invitedUserToken = Guid.NewGuid().ToString();
                                    string extraData = buyerInvitation.RecipientEmail + ";" + buyerInvitation.Role + ";" +
                                                       (short)UserTypes.Buyer + ";" + buyerInvitation.BuyerId;
                                    _globalAttributeService.SaveGlobalAttributeOnlyKeyAndValue(
                                        GlobalAttributeBuiltIn.InvitedUserToken, invitedUserToken, extraData);
                                    _emailService.SendUserInvitationMessage(buyerInvitation.RecipientEmail,
                                        invitedUserToken, _appContext.AppLanguage.Id, "buyer", buyer.Name);
                                }
                            }
                        }
                    }
                });
                return Ok(buyerInvitationModel);
            }
        }


        [HttpPost]
        [Route("updateInvitationRole")]
        public IHttpActionResult UpdateInvitationRole([FromBody]BuyerInvitationRoleUpdateModel buyerInvitationModel)
        {
            if (!_permissionService.Authorize("edit-invitation-buyer"))
            {
                return HttpBadRequest("access-denied");
            }
            {
                if (buyerInvitationModel.BuyerInvitationId != 0)
                {
                    var invitations = _buyerService.GetBuyerInvitations(buyerInvitationModel.BuyerId);
                    if (invitations != null && invitations.Any())
                    {
                        var invitation = invitations.Where(inv => inv.Id == buyerInvitationModel.BuyerInvitationId).FirstOrDefault();
                        if (invitation != null && invitation.Status == BuyerInvitationStatuses.Pending)
                        {
                            invitation.Role = (short)buyerInvitationModel.Role;
                            _buyerService.UpdateBuyerInvitation(invitation);
                        }
                        else
                        {
                            return HttpBadRequest("Invitation is already accepted");
                        }
                    }
                    else
                    {
                        return HttpBadRequest("Invitation can not be found for the selected buyer");
                    }

                }
                return Ok(buyerInvitationModel);
            }

        }

        [HttpPost]
        [Route("inviteUsers")]
        public IHttpActionResult AddBuyerInvitations([FromBody] List<BuyerInvitationModel> buyerInvitationModel)
        {
            if (!_permissionService.Authorize("edit-invitation-buyer"))
            {
                return HttpBadRequest("access-denied");
            }

            try
            {
                var invitationBuyerList = _buyerService.GetAllBuyerInvitations();

                var affiliateService = AppEngineContext.Current.Resolve<IAffiliateService>();
                var invitationAffiliateList = affiliateService.GetAllAffiliateInvitations();

                buyerInvitationModel.ForEach(buyerInvitation =>
                {
                    buyerInvitation.IsSendInvitation = false;

                    if (buyerInvitation.BuyerInvitationId == 0)
                    {
                        //if (!invitationList.Any(x => x.RecipientEmail.ToLower().Trim() == buyerInvitation.RecipientEmail.ToLower().Trim()))
                        if (invitationBuyerList.All(x => x.RecipientEmail.ToLower().Trim() != buyerInvitation.RecipientEmail.ToLower().Trim()) &&
                            invitationAffiliateList.All(x => x.RecipientEmail.ToLower().Trim() != buyerInvitation.RecipientEmail.ToLower().Trim()))
                        {
                            if (_userService.GetUserByEmail(buyerInvitation.RecipientEmail.ToLower().Trim()) == null)
                            {
                                buyerInvitation.InvitationDate = DateTime.UtcNow;
                                buyerInvitation.Status = BuyerInvitationStatuses.Pending;

                                var newInvitationId = _buyerService.InsertBuyerInvitation(new BuyerInvitation()
                                {
                                    BuyerId = buyerInvitation.BuyerId,
                                    InvitationDate = buyerInvitation.InvitationDate,
                                    RecipientEmail = buyerInvitation.RecipientEmail,
                                    Status = buyerInvitation.Status,
                                    Role = (short) buyerInvitation.Role
                                });
                                buyerInvitation.BuyerInvitationId = newInvitationId;

                                var buyer = _buyerService.GetBuyerById(buyerInvitation.BuyerId, true);

                                if (buyerInvitation.CanSendEmail)
                                {
                                    string invitedUserToken = Guid.NewGuid().ToString();
                                    string extraData = buyerInvitation.RecipientEmail + ";" + buyerInvitation.Role + ";" +
                                                       (short)UserTypes.Buyer + ";" + buyerInvitation.BuyerId;
                                    _globalAttributeService.SaveGlobalAttributeOnlyKeyAndValue(
                                        GlobalAttributeBuiltIn.InvitedUserToken, invitedUserToken, extraData);
                                    _emailService.SendUserInvitationMessage(buyerInvitation.RecipientEmail,
                                        invitedUserToken, _appContext.AppLanguage.Id, "buyer", buyer.Name);
                                }

                                buyerInvitation.IsSendInvitation = true;
                            }
                        }
                    }
                });
                return Ok(buyerInvitationModel);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete]
        [Route("deleteBuyerInvitation/{invitationId}")]
        public IHttpActionResult DeleteBuyerInvitation(long invitationId)
        {
            if (!_permissionService.Authorize("edit-invitation-buyer"))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                if (invitationId == 0)
                {
                    return HttpBadRequest("Invitation Id can not be 0");
                }
                else
                {
                    var invitation = _buyerInvitationRepository.GetById(invitationId);
                    if (invitation == null)
                    {
                        return HttpBadRequest($"Invitation with {invitationId} id doesn't exist");
                    }
                    _buyerService.DeleteBuyerInvitation(invitationId);
                }
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }

            return Ok();
        }



        [HttpGet]
        [Route("getBuyerInvitations/{buyerId}")]
        public IHttpActionResult GetBuyerInvitationList(long buyerId)
        {
            if (!_permissionService.Authorize("view-invitation-buyer"))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var result = new List<BuyerInvitationModel>();
                var buyerUsersInvitations = _buyerService.GetBuyerInvitations(buyerId);
                buyerUsersInvitations.ForEach(x =>
                {
                    result.Add(new BuyerInvitationModel()
                    {
                        BuyerId = x.BuyerId,
                        BuyerInvitationId = x.Id,
                        InvitationDate = x.InvitationDate,
                        RecipientEmail = x.RecipientEmail.Trim(),
                        Status = x.Status,
                        Role = x.Role
                    });
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("uploadBuyerIcon/{id}")]
        public IHttpActionResult UploadBuyerIcon(long id)
        {
            if (!_permissionService.Authorize("edit-information-buyer"))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var buyer = _buyerService.GetBuyerById(id, false);
                if (buyer == null)
                {
                    return HttpBadRequest($"no buyer was found for given id {id}");
                }

                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files["Icon"] != null)
                {
                    var file = httpRequest.Files.Get("Icon");
                    var ext = Path.GetExtension(file.FileName);
                    var imageName = $"buyer_{_appContext.AppUser.Id}{Guid.NewGuid()}{ext}";
                    var relativePath = "~/Content/Uploads/Icons/Buyer/";
                    if (file != null)
                    {
                        var uri = _storageService.Upload(blobPath, file.InputStream, file.ContentType, imageName);
                        buyer.IconPath = Path.Combine(relativePath, imageName);
                        _buyerService.UpdateBuyer(buyer);

                        return Ok(uri.AbsoluteUri);

                        Stream fs = file.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        byte[] bytes = br.ReadBytes((Int32)fs.Length);

                        var targetFolder = HttpContext.Current.Server.MapPath(relativePath);
                        var exists = Directory.Exists(targetFolder);
                        if (!exists)
                            Directory.CreateDirectory(targetFolder);

                        var targetPath = Path.Combine(targetFolder, imageName);


                        var validationResult = ValidationHelper.ValidateImage(bytes, file.FileName.Split('.')[file.FileName.Split('.').Length - 1]
                            , new List<string> { "png", "jpg", "jpeg", "gif" }, 1024, 768, 1048576);

                        if (!validationResult.Item1)
                        {
                            return HttpBadRequest(validationResult.Item2);
                        }

                        if (!string.IsNullOrWhiteSpace(buyer.IconPath))
                        {
                            var deletePath = System.Web.Hosting.HostingEnvironment.MapPath(buyer.IconPath);
                            if (!string.IsNullOrEmpty(deletePath) && File.Exists(deletePath))
                            {
                                File.Delete(deletePath);
                            }
                        }

                        file.SaveAs(targetPath);
                    }
                    else
                        return HttpBadRequest("No file attached");

                    buyer.IconPath = Path.Combine(relativePath, imageName);
                    _buyerService.UpdateBuyer(buyer);
                    return Ok($"{UploadBuyerIconFolderUrl}{imageName}");
                }
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        [Route("deleteBuyerIcon/{id}")]
        public IHttpActionResult DeleteBuyerIcon(long id)
        {
            if (!_permissionService.Authorize("edit-information-buyer"))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var buyer = _buyerService.GetBuyerById(id, false);
                if (buyer != null && !string.IsNullOrWhiteSpace(buyer.IconPath))
                {
                    var relativePath = "~/Content/Uploads/Icons/Buyer/";
                    var targetFolder = HttpContext.Current.Server.MapPath(relativePath);
                    var splittedFileName = buyer.IconPath.Split(new[] { "buyer_" }, StringSplitOptions.None);
                    if (splittedFileName.Length != 0)
                    {
                        var fileName = Path.Combine(targetFolder, $"buyer_{splittedFileName[1]}");
                        File.Delete(fileName);
                    }
                    buyer.IconPath = null;
                    _buyerService.UpdateBuyer(buyer);
                }
                else if (buyer == null)
                {
                    return HttpBadRequest($"no buyer was found for given id {id}");
                }
                else if (string.IsNullOrWhiteSpace(buyer.IconPath))
                {
                    return HttpBadRequest($"the buyer with given {id} id doesn't have icon");
                }
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region private methods

        private BuyerViewModel CreateBuyerViewModel(Buyer buyer)
        {

            string managerEmail = string.Empty;
            if (buyer.ManagerId.HasValue)
            {
                var manager = _userService.GetUserById(buyer.ManagerId.Value) ?? null;
                managerEmail = manager?.ContactEmail;
            }

            return new BuyerViewModel
            {
                BuyerId = buyer.Id,
                ManagerId = buyer.ManagerId,
                Manager = managerEmail,
                Phone = buyer.Phone,
                CountryName = buyer.Country?.Name,
                CountryId = buyer.Country?.Id ?? 0,
                StateProvinceId = buyer.StateProvince?.Id,
                StateProvinceName = buyer.StateProvince?.Name,
                ZipCode = buyer.ZipPostalCode,
                Name = buyer.Name,
                Status = buyer.Status,
                BuyerType = (BuyerType)buyer.AlwaysSoldOption
            };
        }

        private string CheckCanDelete(long id)
        {
            var users = _userService.GetBuyerUsers(id);

            if (users.Any())
                return "can not delete buyer because the are active users";

            var buyerChannels = _buyerChannelService.GetAllBuyerChannels(id);

            if (buyerChannels.Any())
                return "can not delete buyer because the are active buyer channels";

            return "";
        }

        private string ValidateDirectory(long countryId, long? stateProvinceId)
        {
            var validationMessage = "";

            var countries = _countryService.GetAllCountries();

            if (!countries.Select(x=>x.Id).Contains(countryId))
            {
                validationMessage = "no country was found for given id";
            }

            var stateProvinces = _stateProvinceService.GetStateProvinceByCountryId(countryId);

            if (stateProvinceId != null && !stateProvinces.Select(x => x.Id).Contains(stateProvinceId.Value))
            {
                validationMessage = "no state province was found for given id";
            }

            return validationMessage;
        }

        ///// <summary>
        ///// Prepares buyer model
        ///// </summary>
        ///// <param name="model">BuyerModel reference</param>
        //protected void PrepareModel(BuyerModel model)
        //{
        //    model.ListStatus.Add(new SelectItem { Text = "Inactive", Value = "0" });
        //    model.ListStatus.Add(new SelectItem { Text = "Active", Value = "1" });

        //    model.ListDoNotPresentPostMethod.Add(new SelectItem { Text = "POST", Value = "POST" });
        //    model.ListDoNotPresentPostMethod.Add(new SelectItem { Text = "GET", Value = "GET" });

        //    model.ListDoNotPresentStatus.Add(new SelectItem { Text = "Off", Value = "0" });
        //    model.ListDoNotPresentStatus.Add(new SelectItem { Text = "Local", Value = "1" });
        //    model.ListDoNotPresentStatus.Add(new SelectItem { Text = "Url", Value = "2" });

        //    model.ListAlwaysSoldOption.Add(new SelectItem { Text = "Online", Value = "0" });

        //    model.ListUser.Add(new SelectItem { Text = _localizedStringService.GetLocalizedString("Affiliate.SelectAccountManager"), Value = "" });

        //    foreach (var value in _userService.GetUsersByType(UserTypes.Network))
        //    {
        //        if (value.Deleted) continue;

        //        model.ListUser.Add(new SelectItem
        //        {
        //            Text = value.GetFullName(),
        //            Value = value.Id.ToString(),
        //            IsSelected = value.Id == model.ManagerId
        //        });
        //    }

        //    model.ListCountry.Add(new SelectItem { Text = _localizedStringService.GetLocalizedString("Address.SelectCountry"), Value = "" });

        //    foreach (var value in _countryService.GetAllCountries())
        //    {
        //        model.ListCountry.Add(new SelectItem
        //        {
        //            Text = value.GetLocalized(x => x.Name),
        //            Value = value.Id.ToString(),
        //            IsSelected = value.Id == model.CountryId
        //        });
        //    }

        //    var stateProvince = _stateProvinceService.GetStateProvinceByCountryId(model.CountryId).ToList();

        //    if (stateProvince.Any())
        //    {
        //        model.ListStateProvince.Add(new SelectItem { Text = _localizedStringService.GetLocalizedString("Address.SelectStateProvince"), Value = "" });

        //        foreach (var value in stateProvince)
        //        {
        //            model.ListStateProvince.Add(new SelectItem
        //            {
        //                Text = value.GetLocalized(x => x.Name),
        //                Value = value.Id.ToString(),
        //                IsSelected = value.Id == model.StateProvinceId
        //            });
        //        }
        //    }
        //    else
        //    {
        //        bool anyCountrySelected = model.ListCountry.Any(x => x.IsSelected);

        //        model.ListStateProvince.Add(new SelectItem
        //        {
        //            Text = _localizedStringService.GetLocalizedString(anyCountrySelected ? "Address.OtherNonUS" : "Address.SelectStateProvince"),
        //            Value = "0"
        //        });
        //    }

        //    model.ListUserRole.Add(new SelectItem { Text = "Select role", Value = string.Empty });

        //    var roles = _roleService.GetAllRoles();

        //    foreach (var value in _roleService.GetAllRoles())
        //    {
        //        if (value.UserType != model.UserType) continue;

        //        model.ListUserRole.Add(new SelectItem
        //        {
        //            Text = value.Name,
        //            Value = value.Id.ToString(),
        //            IsSelected = value.Id == (long)model.UserType
        //        });
        //    }
        //}

        #endregion

        #endregion
    }

    static class BuyerExtension
    {
        internal static BuyerModel AbsolutePathBuilder(this BuyerModel buyerModel, HttpRequestMessage request)
        {
            var buyerIcon = buyerModel.IconPath;
            if (!string.IsNullOrWhiteSpace(buyerIcon) && buyerIcon.Contains("~"))
            {
                buyerIcon = buyerIcon.Replace("~", string.Empty);
                buyerIcon = $"{request.RequestUri.GetLeftPart(UriPartial.Authority)}{buyerIcon}";
                buyerModel.IconPath = buyerIcon;
            }

            return buyerModel;
        }
        internal static BuyerResponseModel AbsolutePathBuilder(this BuyerResponseModel buyerModel, HttpRequestMessage request)
        {
            var buyerIcon = buyerModel.IconPath;
            if (!string.IsNullOrWhiteSpace(buyerIcon) && buyerIcon.Contains("~"))
            {
                buyerIcon = buyerIcon.Replace("~", string.Empty);
                buyerIcon = $"{request.RequestUri.GetLeftPart(UriPartial.Authority)}{buyerIcon}";
                buyerModel.IconPath = buyerIcon;
            }

            return buyerModel;
        }
    }
}
