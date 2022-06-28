using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;
using Adrack.Core;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Configuration;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Common;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Directory;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.Web.Framework.Cache;
using Adrack.WebApi.Helpers;
using Adrack.WebApi.Infrastructure.Constants;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Infrastructure.Enums;
using Adrack.WebApi.Infrastructure.Web.Helpers;
using Adrack.WebApi.Models;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Lead;
using Newtonsoft.Json;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/affiliate")]
    public class AffiliateController : BaseApiController
    {
        private const int StateCodeUnitedStates = 80;

        #region fields

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// The affiliate channel filter condition service
        /// </summary>
        private readonly IAffiliateChannelFilterConditionService _affiliateChannelFilterConditionService;

        /// <summary>
        /// The affiliate channel service
        /// </summary>
        private readonly IAffiliateChannelService _affiliateChannelService;

        /// <summary>
        /// The affiliate note service
        /// </summary>
        private readonly IAffiliateNoteService _affiliateNoteService;

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IAffiliateService _affiliateService;

        /// <summary>
        /// The authentication service
        /// </summary>
        private readonly IAuthenticationService _authenticationService;

        ///// <summary>
        ///// The buyer channel service
        ///// </summary>
        //private readonly IBuyerChannelService _buyerChannelService;

        ///// <summary>
        ///// The campaign service
        ///// </summary>
        //private readonly ICampaignService _campaignService;

        /// <summary>
        /// The country service
        /// </summary>
        private readonly ICountryService _countryService;

        /// <summary>
        /// The department service
        /// </summary>
        private readonly IDepartmentService _departmentService;

        /// <summary>
        /// The email service
        /// </summary>
        private readonly IEmailService _emailService;

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

        ///// <summary>
        ///// The setting service
        ///// </summary>
        //private readonly ISettingService _settingService;

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

        private readonly ISmtpAccountService _smtpAccountService;

        private readonly IJWTTokenService _jWTTokenService;

        private readonly IStorageService _storageService;
        
        private readonly IEntityChangeHistoryService _entityChangeHistoryService;



        private string _uploadAffiliateIconFolderUrl =>
            $"{Request.RequestUri.GetLeftPart(UriPartial.Authority)}/Content/Uploads/Icons/Affiliate/";

        protected string blobPath = "uploads";

        /// <summary>
        /// The user setting
        /// </summary>
        private readonly UserSetting _userSetting; // TODO ?????????

        private readonly IRepository<EntityOwnership> _entityOwnerRepository;

        private readonly IRepository<User> _userRepository;

        private readonly ISearchService _searchService;

        private readonly IPlanService _planService;

        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;
        private static string _viewAffiliateInformationKey { get; set; } = "view-information-affiliate";
        private static string _editAffiliateInformationKey { get; set; } = "edit-information-affiliate";
        private static string _viewAffiliateInvitationKey { get; set; } = "view-invitation-affiliate";
        private static string _editAffiliateInvitationKey { get; set; } = "edit-invitation-affiliate";
        #endregion

        #region constructors

        /// <summary>
        /// AffiliateController constructor
        /// </summary>
        /// <param name="appContext">AppContext reference</param>
        /// <param name="affiliateChannelFilterConditionService">AffiliateChannelFilterConditionService reference</param>
        /// <param name="affiliateChannelService">AffiliateChannelService reference</param>
        /// <param name="affiliateService">AffiliateService reference</param>
        /// <param name="affiliateNoteService">AffiliateNoteService reference</param>
        /// <param name="authenticationService">The authentication service.</param>
        /// <param name="buyerChannelService">The buyer channel service.</param>
        /// <param name="departmentService">DepartmentService reference</param>
        /// <param name="campaignService">The campaign service.</param>
        /// <param name="countryService">CountryService reference</param>
        /// <param name="emailService">The email service.</param>
        /// <param name="localizedStringService">LocalizedStringService reference</param>
        /// <param name="globalAttributeService"></param>
        /// <param name="permissionService">PermissionService reference</param>
        /// <param name="profileService">ProfileService reference</param>
        /// <param name="roleService">RoleService reference</param>
        /// <param name="settingService">SettingService reference</param>
        /// <param name="stateProvinceService">StateProvinceService reference</param>
        /// <param name="entityChangeHistoryService">EntityChangeHistoryService reference</param>
        /// <param name="userRegistrationService">UserRegistrationService reference</param>
        /// <param name="usersService">UsersService reference</param>
        /// <param name="userSetting">UserSetting reference</param>
        /// <param name="searchService"></param>
        /// <param name="planService"></param>
        public AffiliateController(IAppContext appContext, IAffiliateService affiliateService,
            IAffiliateChannelFilterConditionService affiliateChannelFilterConditionService,
            IAffiliateChannelService affiliateChannelService, IAffiliateNoteService affiliateNoteService,
            IAuthenticationService authenticationService, ICountryService countryService,
            //IBuyerChannelService buyerChannelService, ICampaignService campaignService, 
            IDepartmentService departmentService, IEmailService emailService,
            IGlobalAttributeService globalAttributeService,
            ILocalizedStringService localizedStringService,
            IPermissionService permissionService, IProfileService profileService, IRoleService roleService,
            //ISettingService settingService, 
            IRepository<EntityOwnership> entityOwnerRepository,
            IRepository<User> userRepository,
            IStateProvinceService stateProvinceService,
            IEntityChangeHistoryService entityChangeHistoryService,
            IUserRegistrationService userRegistrationService, IUserService usersService,
            UserSetting userSetting,
            ISearchService searchService,
            ISettingService settingService,
            ISmtpAccountService smtpAccountService,
            IPlanService planService,
            IJWTTokenService jWTTokenService,
            IStorageService storageService)
        {
            this._appContext = appContext;
            this._affiliateChannelFilterConditionService = affiliateChannelFilterConditionService;
            this._affiliateChannelService = affiliateChannelService;
            this._affiliateNoteService = affiliateNoteService;
            this._affiliateService = affiliateService;
            this._authenticationService = authenticationService;
            //this._buyerChannelService = buyerChannelService;
            //this._campaignService = campaignService;
            this._countryService = countryService;
            this._departmentService = departmentService;
            this._emailService = emailService;
            this._globalAttributeService = globalAttributeService;
            this._localizedStringService = localizedStringService;
            this._permissionService = permissionService;
            this._profileService = profileService;
            this._roleService = roleService;
            //this._settingService = settingService;
            this._stateProvinceService = stateProvinceService;
            this._entityChangeHistoryService = entityChangeHistoryService;
            this._userRegistrationService = userRegistrationService;
            this._userService = usersService;
            this._userSetting = userSetting;
            this._entityOwnerRepository = entityOwnerRepository;
            this._userRepository = userRepository;
            this._searchService = searchService;
            this._settingService = settingService;
            this._smtpAccountService = smtpAccountService;
            this._planService = planService;
            this._jWTTokenService = jWTTokenService;
            this._storageService = storageService;
        }

        #endregion

        #region methods

        #region route methods

        [HttpPost]
        [Route("uploadAffiliateIcon/{id}")]
        public IHttpActionResult UploadAffiliateIcon(long id)
        {
            if (!_permissionService.Authorize(_editAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var affiliate = _affiliateService.GetAffiliateById(id, false);
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files["Icon"] != null)
                {
                    var file = httpRequest.Files.Get("Icon");
                    var ext = Path.GetExtension(file.FileName);
                    var imageName = $"affiliate_{_appContext.AppUser.Id}{Guid.NewGuid()}{ext}";
                    var relativePath = "~/Content/Uploads/Icons/Affiliate/";
                    if (file != null)
                    {
                        var uri = _storageService.Upload(blobPath, file.InputStream, file.ContentType, imageName);
                        affiliate.IconPath = uri.AbsoluteUri;
                        _affiliateService.UpdateAffiliate(affiliate);

                        return Ok(uri.AbsoluteUri);

                        Stream fs = file.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        byte[] bytes = br.ReadBytes((Int32)fs.Length);

                        var targetFolder = HttpContext.Current.Server.MapPath(relativePath);
                        var targetPath = Path.Combine(targetFolder, imageName);

                        var validationResult = ValidationHelper.ValidateImage(bytes,
                            file.FileName.Split('.')[file.FileName.Split('.').Length - 1]
                            , new List<string> { "png", "jpg", "jpeg", "gif" }, 1024, 768, 1048576);

                        if (!validationResult.Item1)
                        {
                            return HttpBadRequest(validationResult.Item2);
                        }

                        if (!string.IsNullOrWhiteSpace(affiliate.IconPath))
                        {
                            var deletePath = System.Web.Hosting.HostingEnvironment.MapPath(affiliate.IconPath);
                            if (!string.IsNullOrEmpty(deletePath) && File.Exists(deletePath))
                            {
                                File.Delete(deletePath);
                            }
                        }

                        file.SaveAs(targetPath);
                        file.InputStream.Position = 0;
                        //ss.DeleteFile(blobPath, "avatars/" + file.FileName);
                    }
                    else
                        return HttpBadRequest("No file attached");

                    affiliate.IconPath = Path.Combine(relativePath, imageName);
                    _affiliateService.UpdateAffiliate(affiliate);
                    return Ok($"{_uploadAffiliateIconFolderUrl}{imageName}");
                }

                return Ok();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpDelete]
        [Route("deleteAffiliateIcon/{id}")]
        public IHttpActionResult DeleteAffiliateIcon(long id)
        {
            if (!_permissionService.Authorize(_editAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var affiliate = _affiliateService.GetAffiliateById(id, false);
                if (affiliate != null && !string.IsNullOrWhiteSpace(affiliate.IconPath))
                {
                    var relativePath = "~/Content/Uploads/Icons/Affiliate/";
                    var targetFolder = HttpContext.Current.Server.MapPath(relativePath);
                    var splittedFileName = affiliate.IconPath.Split(new[] {"affiliate_"}, StringSplitOptions.None);
                    if (splittedFileName.Length != 0)
                    {
                        var fileName = Path.Combine(targetFolder, $"affiliate_{splittedFileName[1]}");
                        File.Delete(fileName);
                    }

                    affiliate.IconPath = null;
                    _affiliateService.UpdateAffiliate(affiliate);
                }

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the affiliate notes.
        /// </summary>
        /// <returns>ActionResult.</returns>
        //[ContentManagementAntiForgery(true)]
        //[AppHttpsRequirement(SslRequirement.No)]


        ///// <summary>
        ///// Indexes this instance.
        ///// </summary>
        ///// <returns>ActionResult.</returns>
        //// GET: Affiliate
        //[HttpGet]
        //[Route("")]
        //public string Index()
        //{
        //    this.Request.GetClientIp();
        //    return "";
        //}

        /// <summary>
        /// Shows affiliate list interface
        /// </summary>
        /// <returns>View result</returns>
        //[NavigationBreadCrumb(Clear = true, Label = "Affiliates")]
        [HttpGet]
        [Route("getAffiliateStatus")]
        public IHttpActionResult GetStatus()
        {
            if (!_permissionService.Authorize(_viewAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            IList<StatusCountsClass> affiliatesStatusCounts = this._affiliateService.GetAffiliatesStatusCounts();

            string[] statusArrayStrings = {"0", "0", "0", "0", "0", "0", "0"};
            int totalStatuses = 0;
            foreach (StatusCountsClass statusCount in affiliatesStatusCounts)
            {
                if (statusCount.Status <= statusArrayStrings.Length)
                {
                    totalStatuses += statusCount.Counts;
                    statusArrayStrings[statusCount.Status] = statusCount.Counts.ToString();
                }
            }

            var affiliateStatus = new AffiliateStatusInfo
            {
                StatusArray = statusArrayStrings,
                TotalStatuses = totalStatuses
            };

            return Ok(affiliateStatus);
        }

        /// <summary>
        /// Displays affiliate item create/edit interface
        /// </summary>
        /// <param name="id">Affiliate id</param>
        /// <returns>View result</returns>
        //[NavigationBreadCrumb(Clear = false, Label = "Affiliate")]
        [HttpGet]
        [Route("getAffiliateById/{id}")]
        public IHttpActionResult Item(long id)
        {
            if (!_permissionService.Authorize(_viewAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (this._appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                throw new ArgumentException("User type is Buyer not allowed for current action");
            }

            if (this._appContext.AppUser != null &&
                this._appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
            {
                id = this._appContext.AppUser.ParentId;
            }

            AffiliateModel affiliateModel;
            if (id == 0L)
            {
                affiliateModel = new AffiliateModel
                {
                    AffiliateId = 0L,
                    ParentId = 0L,
                    UserRoleId = GetUserRoleIdByKey(UserRoleKeys.AffiliateUserKey),
                };
                return Ok(NeedToRegister(affiliateModel));
            }

            Affiliate affiliate = this._affiliateService.GetAffiliateById(id, false);

            if (affiliate != null)
            {
                affiliateModel = (new AffiliateModel
                {
                    Icon = affiliate.IconPath,
                    AffiliateId = affiliate.Id,
                    CountryId = affiliate.CountryId,
                    StateProvinceId = affiliate.StateProvinceId ?? 0L,
                    Name = affiliate.Name,
                    AddressLine1 = affiliate.AddressLine1,
                    AddressLine2 = affiliate.AddressLine2,
                    City = affiliate.City,
                    CompanyEmail = affiliate.Email,
                    CompanyPhone = affiliate.Phone,
                    ZipPostalCode = affiliate.ZipPostalCode,
                    Status = affiliate.Status,
                    BillFrequency = affiliate.BillFrequency,
                    FrequencyValue = affiliate.FrequencyValue ?? 0,
                    BillWithin = affiliate.BillWithin ?? 0,
                    Website = affiliate.Website,
                    WhiteIp = affiliate.WhiteIp,
                    ManagerId = affiliate.ManagerId ?? 0L,
                    DefaultAffiliatePrice = Math.Round(affiliate.DefaultAffiliatePrice ?? 0, 2),
                    DefaultAffiliatePriceMethod = affiliate.DefaultAffiliatePriceMethod ?? 0,
                    Notes =
                        (List<AffiliateNote>) _affiliateNoteService.GetAllAffiliateNotesByAffiliateId(affiliate.Id)
                }).AbsolutePathBuilder(Request);

            }
            else
            {
                affiliateModel = new AffiliateModel
                {
                    AffiliateId = 0L
                };
            }

            return Ok(affiliateModel);
        }

        /// <summary>
        /// Shows affiliate item create/edit partial item
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>PartialView result</returns>
        //[NavigationBreadCrumb(Clear = true, Label = "Affiliate")]
        [HttpGet]
        [Route("{id}/partial")]
        public IHttpActionResult PartialItem(long id)
        {
            if (!_permissionService.Authorize(_viewAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            AffiliateModel affiliateModel;

            if (id == 0L)
            {
                affiliateModel = new AffiliateModel
                {
                    AffiliateId = 0L,
                    ParentId = 0L,
                    UserRoleId = GetUserRoleIdByKey(UserRoleKeys.AccountManagerKey)
                };
                return Ok(NeedToRegister(affiliateModel));
            }

            Affiliate affiliate = this._affiliateService.GetAffiliateById(id, false);

            if (affiliate != null)
            {
                affiliateModel = (new AffiliateModel
                {
                    AffiliateId = affiliate.Id,
                    CountryId = affiliate.CountryId,
                    StateProvinceId = affiliate.StateProvinceId ?? 0L,
                    Name = affiliate.Name,
                    AddressLine1 = affiliate.AddressLine1,
                    AddressLine2 = affiliate.AddressLine2,
                    City = affiliate.City,
                    CompanyEmail = affiliate.Email,
                    CompanyPhone = affiliate.Phone,
                    ZipPostalCode = affiliate.ZipPostalCode,
                    Status = affiliate.Status,
                    BillFrequency = affiliate.BillFrequency,
                    FrequencyValue = affiliate.FrequencyValue ?? 0,
                    BillWithin = affiliate.BillWithin ?? 0,
                    Website = affiliate.Website,
                    WhiteIp = affiliate.WhiteIp
                }).AbsolutePathBuilder(Request);

                if (affiliate.Status == (short)AffiliateActivityStatuses.None)
                {
                    affiliate.Status = (short)AffiliateActivityStatuses.Applied;
                    _affiliateService.UpdateAffiliate(affiliate);
                }

                affiliateModel.Notes =
                    (List<AffiliateNote>) _affiliateNoteService.GetAllAffiliateNotesByAffiliateId(affiliate.Id);
            }
            else
            {
                affiliateModel = new AffiliateModel
                {
                    AffiliateId = 0
                };
            }

            return Ok(affiliateModel); //Item(new AffiliateExtendedModel {Affiliate = affiliateModel});
        }

        /// <summary>
        /// Handles affiliate item submit action
        /// </summary>
        /// <returns>RedirectAction result</returns>
        [HttpPost]
        //[PublicAntiForgery]
        //[ValidateInput(false)]
        [Route("createOrUpdate")]
        public IHttpActionResult CreateOrUpdate([FromBody] AffiliateCreateUpdateModel affiliateModel)
        {
            if (!_permissionService.Authorize(_editAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                Affiliate affiliate;
                {
                    if (_countryService.GetCountryById(affiliateModel.CountryId) == null)
                    {
                        return HttpBadRequest($"no country was found for given id {affiliateModel.CountryId}");
                    }

                    if (affiliateModel.StateProvinceId != 0 && _stateProvinceService.GetStateProvinceById(affiliateModel.StateProvinceId) == null)
                    {
                        return HttpBadRequest(
                            $"no state province was found for given id {affiliateModel.StateProvinceId}");
                    }

                    if (_userService.GetUserById(affiliateModel.ManagerId) == null)
                    {
                        return HttpBadRequest($"no manager found for given id {affiliateModel.ManagerId}");
                    }

                    if (affiliateModel.AffiliateId == 0)
                    {
                        if (_affiliateService.CheckAffiliateName(affiliateModel.Name.Trim()) != null)
                            return HttpBadRequest("Affiliate with the specified name already exists");
                    }
                    else
                    {
                        if (_affiliateService.GetAffiliateByName(affiliateModel.Name, affiliateModel.AffiliateId) != null)
                            return HttpBadRequest("Affiliate with the specified name already exists");
                    }

                    affiliate = _affiliateService.GetAffiliateById(affiliateModel.AffiliateId, false);

                    short status = affiliateModel.Status;
                    if (affiliate == null)
                    {
                        affiliate = new Affiliate();
                        status = affiliateModel.Status;
                    }

                    affiliate.Name = affiliateModel.Name.Trim();
                    affiliate.ManagerId = affiliateModel.ManagerId;

                    long? stateProvinceId = null;
                    if (affiliateModel.StateProvinceId != 0)
                        stateProvinceId = affiliateModel.StateProvinceId;

                    affiliate.CountryId = affiliateModel.CountryId;
                    affiliate.StateProvinceId = stateProvinceId;
                    affiliate.City = affiliateModel.City;
                    affiliate.AddressLine1 = affiliateModel.AddressLine1;
                    affiliate.AddressLine2 = affiliateModel.AddressLine2;
                    affiliate.ZipPostalCode = affiliateModel.ZipPostalCode;
                    affiliate.Phone = affiliateModel.CompanyPhone;
                    affiliate.Email = affiliateModel.CompanyEmail;

                    affiliate.DefaultAffiliatePrice = affiliateModel.DefaultAffiliatePrice;
                    affiliate.DefaultAffiliatePriceMethod = (short) affiliateModel.DefaultAffiliatePriceMethod;

                    affiliate.Status = status;
                    affiliate.CreatedOn = DateTime.UtcNow;

                    affiliate.UserId = _appContext.AppUser.Id;

                    affiliate.BillFrequency = affiliateModel.BillFrequency;
                    affiliate.FrequencyValue = affiliateModel.FrequencyValue;
                    affiliate.BillWithin = affiliateModel.BillWithin;

                    affiliate.Website = affiliateModel.Website;
                    affiliate.WhiteIp = affiliateModel.WhiteIp;

                    if (affiliateModel.AffiliateId == 0)
                    {
                        long newId = _affiliateService.InsertAffiliate(affiliate);
                        affiliate.Id = newId;

                        var access = new EntityOwnership
                        {
                            Id = 0,
                            UserId = _appContext.AppUser.Id,
                            EntityId = affiliate.Id,
                            EntityName = EntityType.Affiliate.ToString()
                        };
                        _userService.InsertEntityOwnership(access);
                    }
                    else
                    {
                        _affiliateService.UpdateAffiliate(affiliate);
                    }

                    affiliateModel.AffiliateId = affiliate.Id;
                }

                if (affiliateModel.Invitations != null)
                {
                    var invitations = _affiliateService.GetAffiliateInvitations(affiliate.Id);
                    List<string> alreadySentEmails = new List<string>();

                    foreach (var affiliateInvitationModel in affiliateModel.Invitations)
                    {
                        if (alreadySentEmails.Contains(affiliateInvitationModel.RecipientEmail))
                            continue;

                        var newInvitationId = _affiliateService.InsertAffiliateInvitation(new AffiliateInvitation()
                        {
                            AffiliateId = affiliate.Id,
                            InvitationDate = DateTime.UtcNow,
                            RecipientEmail = affiliateInvitationModel.RecipientEmail,
                            Status = affiliateInvitationModel.Status,
                            Role = affiliateInvitationModel.Role
                        });

                        if (affiliateModel.CanSendEmail)
                        {
                            string invitedUserToken = Guid.NewGuid().ToString();
                            //$"{Guid.NewGuid().ToString()};{buyerInvitation.RecipientEmail};{buyerInvitation.Role.ToString()};buyer;{buyerInvitation.BuyerId.ToString()}";
                            string extraData = affiliateInvitationModel.RecipientEmail + ";" + affiliateInvitationModel.Role + ";" + (short)UserTypes.Affiliate + ";" + affiliateInvitationModel.AffiliateId;
                            _globalAttributeService.SaveGlobalAttributeOnlyKeyAndValue(GlobalAttributeBuiltIn.InvitedUserToken, invitedUserToken, extraData);
                            _emailService.SendUserInvitationMessage(affiliateInvitationModel.RecipientEmail, invitedUserToken, _appContext.AppLanguage.Id, "affiliate", affiliate.Name);
                        }

                        alreadySentEmails.Add(affiliateInvitationModel.RecipientEmail);
                    }
                }

                return Ok(affiliateModel); //List();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Handles affiliate item submit action
        /// </summary>
        /// <returns>RedirectAction result</returns>
        [HttpPut]
        //[PublicAntiForgery]
        //[ValidateInput(false)]
        [Route("updateAffiliateName")]
        public IHttpActionResult UpdateName([FromBody] AffiliateNameUpdateModel affiliateModel)
        {
            if (!_permissionService.Authorize(_editAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (string.IsNullOrWhiteSpace(affiliateModel.Name))
            {
                return HttpBadRequest("Affiliate name is required");
            }

            if (affiliateModel.AffiliateId == 0)
            {
                return HttpBadRequest("Affiliate id is required");
            }

            if (_affiliateService.GetAffiliateByName(affiliateModel.Name, affiliateModel.AffiliateId) != null)
            {
                return HttpBadRequest("Affiliate with the specified name already exists");
            }

            else
            {
                var affiliate = _affiliateService.GetAffiliateById(affiliateModel.AffiliateId, false);
                if (affiliate == null)
                {
                    return HttpBadRequest("Affiliate with the specified id doesn't exist");
                }
                else
                {
                    affiliate.Name = affiliateModel.Name;
                    _affiliateService.UpdateAffiliate(affiliate);
                    return Ok(affiliateModel);
                }
            }
        }


        [HttpPost]
        [Route("checkAffiliateName")]
        public IHttpActionResult CheckAffiliateName(string name)
        {
            if (_affiliateService.CheckAffiliateName(name.Trim()) != null)
                return HttpBadRequest("Affiliate with the specified name already exists");

            return Ok();
        }

        [HttpPut]
        //[PublicAntiForgery]
        //[ValidateInput(false)]
        [Route("updateAffiliateStatus")]
        public IHttpActionResult UpdateStatus([FromBody] AffiliateStatusUpdateModel affiliateModel)
        {
            if (!_permissionService.Authorize(_editAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (affiliateModel.AffiliateId == 0)
            {
                return HttpBadRequest("Affiliate id is required");
            }
            else
            {
                var affiliate = _affiliateService.GetAffiliateById(affiliateModel.AffiliateId, false);
                if (affiliate == null)
                {
                    return HttpBadRequest("Affiliate with the specified id doesn't exist");
                }
                else
                {
                    affiliate.Status = (short) affiliateModel.Status;
                    _affiliateService.UpdateAffiliate(affiliate);
                    return Ok(affiliateModel);
                }

            }
        }

        [HttpGet]
        [Route("getAffiliatesList")]
        [ContentManagementCache("App.Cache.Affiliate.")]
        public IHttpActionResult GetAffiliateList(string name = null, EntityFilterByStatus status = EntityFilterByStatus.All)
        {
            if (!_permissionService.Authorize(_viewAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            var result = new List<AffiliateListItemModel>();
            //var affiliateList = GetAffiliateAdvancedList(name, status);
            var affiliateList = _affiliateService.GetAllAffiliates(name, status);
            //var affiliateList = _affiliateService.GetAffiliatesByUser(name, status);
            foreach (var affiliate in affiliateList)
            {
                result.Add(CreateAffiliateListItem(affiliate));
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("updateInvitation")]
        public IHttpActionResult AddAffiliateInvitation([FromBody] AffiliateInvitationModel affiliateInvitationModel)
        {
            if (!_permissionService.Authorize(_editAffiliateInvitationKey))
            {
                return HttpBadRequest("access-denied");
            }
            {
                if (affiliateInvitationModel.AffiliateInvitationId == 0)
                {
                    var invitationList =
                        _affiliateService.GetAffiliateInvitations(affiliateInvitationModel.AffiliateId);
                    if (invitationList.Where(x =>
                            x.RecipientEmail.ToLower().Trim() ==
                            affiliateInvitationModel.RecipientEmail.ToLower().Trim())
                        .Count() == 0)
                    {
                        var newInvitationId = _affiliateService.InsertAffiliateInvitation(new AffiliateInvitation()
                        {
                            AffiliateId = affiliateInvitationModel.AffiliateId,
                            InvitationDate = DateTime.UtcNow,
                            RecipientEmail = affiliateInvitationModel.RecipientEmail,
                            Status = affiliateInvitationModel.Status,
                            Role = (short) affiliateInvitationModel.Role
                        });
                        affiliateInvitationModel.AffiliateInvitationId = newInvitationId;

                        var affiliate = _affiliateService.GetAffiliateById(affiliateInvitationModel.AffiliateId, false);

                        if (affiliate != null)
                        {
                            string invitedUserToken = Guid.NewGuid().ToString();//$"{Guid.NewGuid().ToString()};{buyerInvitation.RecipientEmail};{buyerInvitation.Role.ToString()};buyer;{buyerInvitation.BuyerId.ToString()}";
                            string extraData = affiliateInvitationModel.RecipientEmail + ";" + affiliateInvitationModel.Role + ";" + (short)UserTypes.Affiliate + ";" + affiliateInvitationModel.AffiliateId;
                            _globalAttributeService.SaveGlobalAttributeOnlyKeyAndValue(GlobalAttributeBuiltIn.InvitedUserToken, invitedUserToken, extraData);
                            _emailService.SendUserInvitationMessage(affiliateInvitationModel.RecipientEmail, invitedUserToken, _appContext.AppLanguage.Id, "affiliate", affiliate.Name);
                        }

                    }
                }
                else
                {
                    _affiliateService.UpdateAffiliateInvitation(new AffiliateInvitation()
                    {
                        AffiliateId = affiliateInvitationModel.AffiliateId,
                        Id = affiliateInvitationModel.AffiliateInvitationId,
                        InvitationDate = affiliateInvitationModel.InvitationDate,
                        RecipientEmail = affiliateInvitationModel.RecipientEmail,
                        Status = affiliateInvitationModel.Status,
                        Role = (short) affiliateInvitationModel.Role
                    });
                }

                return Ok(affiliateInvitationModel);
            }

        }

        [HttpPost]
        [Route("updateInvitationRole")]
        public IHttpActionResult UpdateInvitationRole(
            [FromBody] AffiliateInvitationRoleUpdateModel affiliateInvitationModel)
        {
            if (!_permissionService.Authorize(_editAffiliateInvitationKey))
            {
                return HttpBadRequest("access-denied");
            }
            {
                if (affiliateInvitationModel == null)
                {
                    return HttpBadRequest("Invitation can not be null");
                }

                if (affiliateInvitationModel.AffiliateInvitationId != 0)
                {
                    var invitations = _affiliateService.GetAffiliateInvitations(affiliateInvitationModel.AffiliateId);
                    if (invitations != null && invitations.Any())
                    {
                        var invitation = invitations.FirstOrDefault(inv => inv.Id == affiliateInvitationModel.AffiliateInvitationId);
                        if (invitation != null && invitation.Status == AffiliateInvitationStatuses.Pending)
                        {
                            invitation.Role = (short) affiliateInvitationModel.Role;
                            _affiliateService.UpdateAffiliateInvitation(invitation);
                        }
                        else
                        {
                            return HttpBadRequest("Invitation is already accepted");

                        }
                    }
                    else
                    {
                        return HttpBadRequest("Invitation can not be found for the selected affiliate");

                    }

                }

                return Ok(affiliateInvitationModel);
            }

        }

        [HttpPost]
        [Route("inviteUsers")]
        public IHttpActionResult AddAffiliateInvitations([FromBody] List<AffiliateInvitationModel> affiliateInvitationModel)
        {
            if (!_permissionService.Authorize(_editAffiliateInvitationKey))
            {
                return HttpBadRequest("access-denied");
            }

            try
            {
                var invitationAffiliateList = _affiliateService.GetAllAffiliateInvitations();
                
                var buyerService = AppEngineContext.Current.Resolve<IBuyerService>();
                var invitationBuyerList = buyerService.GetAllBuyerInvitations();
                
                affiliateInvitationModel.ForEach(affiliateInvitation =>
                {
                    affiliateInvitation.IsSendInvitation = false;

                    if (affiliateInvitation.AffiliateInvitationId == 0)
                    {
                        //var invitationList = _affiliateService.GetAffiliateInvitations(affiliateInvitation.AffiliateId);

                        if (invitationAffiliateList.All(x => x.RecipientEmail.ToLower().Trim() != affiliateInvitation.RecipientEmail.ToLower().Trim()) &&
                            invitationBuyerList.All(x => x.RecipientEmail.ToLower().Trim() != affiliateInvitation.RecipientEmail.ToLower().Trim())
                        )
                        {
                            if (_userService.GetUserByEmail(affiliateInvitation.RecipientEmail.ToLower().Trim()) == null)
                            {
                                affiliateInvitation.InvitationDate = DateTime.UtcNow;
                                affiliateInvitation.Status = AffiliateInvitationStatuses.Pending;

                                var newInvitationId = _affiliateService.InsertAffiliateInvitation(
                                    new AffiliateInvitation()
                                    {
                                        AffiliateId = affiliateInvitation.AffiliateId,
                                        InvitationDate = DateTime.UtcNow,
                                        RecipientEmail = affiliateInvitation.RecipientEmail,
                                        Status = AffiliateInvitationStatuses.Pending,
                                        Role = (short) affiliateInvitation.Role
                                    });
                                affiliateInvitation.AffiliateInvitationId = newInvitationId;

                                var affiliate =
                                    _affiliateService.GetAffiliateById(affiliateInvitation.AffiliateId, true);

                                if (affiliateInvitation.CanSendEmail)
                                {
                                    string invitedUserToken = Guid.NewGuid().ToString();
                                    string extraData = affiliateInvitation.RecipientEmail + ";" + affiliateInvitation.Role +
                                                       ";" + (short)UserTypes.Affiliate + ";" +
                                                       affiliateInvitation.AffiliateId;
                                    _globalAttributeService.SaveGlobalAttributeOnlyKeyAndValue(
                                        GlobalAttributeBuiltIn.InvitedUserToken, invitedUserToken, extraData);
                                    _emailService.SendUserInvitationMessage(affiliateInvitation.RecipientEmail,
                                        invitedUserToken, _appContext.AppLanguage.Id, "affiliate", affiliate.Name);
                                }

                                affiliateInvitation.IsSendInvitation = true;
                            }
                        }
                    }
                });
                return Ok(affiliateInvitationModel);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete]
        [Route("deleteAffiliateInvitation/{invitationId}")]
        public IHttpActionResult DeleteAffiliateInvitation(long invitationId)
        {
            if (!_permissionService.Authorize(_editAffiliateInvitationKey))
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
                    _affiliateService.DeleteAffiliateInvitation(invitationId);
                }
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpGet]
        [Route("getAffiliateInvitations/{affiliateId}")]
        public IHttpActionResult AffiliateInvitationList(long affiliateId)
        {
            if (!_permissionService.Authorize(_viewAffiliateInvitationKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var result = new List<AffiliateInvitationModel>();
                var affiliateUsersInvitations = _affiliateService.GetAffiliateInvitations(affiliateId);
                affiliateUsersInvitations.ForEach(x =>
                {
                    result.Add(new AffiliateInvitationModel()
                    {
                        AffiliateId = x.AffiliateId,
                        AffiliateInvitationId = x.Id,
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

        /// <summary>
        /// Changes the affiliate status
        /// </summary>
        /// <returns>Json result</returns>
        //[HttpGet]
        [HttpPost]
        //[ContentManagementAntiForgery(true)]
        [Route("{id}/status/{status}")]
        public IHttpActionResult SetAffiliateStatus(string id, string status)
        {
            if (!_permissionService.Authorize(_editAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (this._appContext.AppUser == null) return Ok(0); //new { res = 0 };
            if (this._appContext.AppUser.UserType != SharedData.BuiltInUserTypeId) return Ok(0); //new { res = 0 };

            long.TryParse(id, out var idResult);

            short.TryParse(status, out var statusResult);

            Affiliate affiliate = _affiliateService.GetAffiliateById(idResult, false);

            if (affiliate != null)
            {
                affiliate.Status = statusResult;
                _affiliateService.UpdateAffiliate(affiliate);
            }

            return Ok(0); //new { res = 0 };
        }

        /// <summary>
        /// Dashboards the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("{id}/dashboard")]
        public IHttpActionResult Dashboard(long id = 0)
        {
            if (!_permissionService.Authorize(_viewAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (id == 0 && _appContext.AppUser != null &&
                _appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
            {
                id = _appContext.AppUser.ParentId;
            }

            Affiliate affiliate = _affiliateService.GetAffiliateById(id, false);
            //ViewBag.BuyerCompanyName = !string.IsNullOrEmpty(affiliate?.Name) ? $"Affiliate > {affiliate.Name} > " : string.Empty;
            //ViewBag.SelectedAffiliateId = id;

            AffiliateModel affiliateModel = new AffiliateModel {AffiliateId = id, Name = affiliate?.Name};

            return Ok(affiliateModel);
        }

        /// <summary>
        /// Partials the dashboard.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("{id}/dashboard/partial")]
        public IHttpActionResult PartialDashboard(long id = 0)
        {
            if (!_permissionService.Authorize(_viewAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (id == 0 && _appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                id = _appContext.AppUser.ParentId;

            AffiliateModel affiliateModel = new AffiliateModel {AffiliateId = id};
            //ViewBag.SelectedAffiliateId = id;

            return Ok(affiliateModel); //PartialDashboard(affiliateModel);
        }

        /// <summary>
        /// Channels the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("{id}/channels")]
        public IHttpActionResult Channels(long id = 0)
        {
            if (!_permissionService.Authorize(_viewAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (id == 0 && _appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                id = _appContext.AppUser.ParentId;

            Affiliate affiliate = _affiliateService.GetAffiliateById(id, false);
            //ViewBag.BuyerCompanyName = !string.IsNullOrEmpty(affiliate?.Name) ? $"Affiliate > {affiliate.Name} > " : string.Empty;
            //ViewBag.SelectedAffiliateId = id;

            var affiliateModel = new AffiliateModel {AffiliateId = id, Name = affiliate?.Name};
            return Ok(affiliateModel);
        }

        /// <summary>
        /// Campaigns this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("campaigns")]
        public List<Campaign> Campaigns() // TODO remove ?????????????
        {
            // Name/Description "Create Affiliate Chanel"
            return new List<Campaign>();
        }

        /// <summary>
        /// Payments the options.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("{id}/payment/options")]
        public IHttpActionResult PaymentOptions(long id = 0)
        {
            if (!_permissionService.Authorize(_viewAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (id == 0 && _appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                id = _appContext.AppUser.ParentId;

            var affiliateModel = new AffiliateModel {AffiliateId = id};

            //ViewBag.SelectedAffiliateId = id;

            return Ok(affiliateModel);
        }

        /// <summary>
        /// Histories the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("{id}/history")]
        public IHttpActionResult History(long id)
        {
            if (!_permissionService.Authorize(_viewAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (id == 0 && _appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
            {
                id = _appContext.AppUser.ParentId;
            }

            var affiliateModel = new AffiliateModel {AffiliateId = id};
            return Ok(affiliateModel); // View() History
        }

        /// <summary>
        /// Users the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("{id}/users")]
        public IHttpActionResult Users(long id = 0)
        {
            if (!_permissionService.Authorize(_viewAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (id == 0 && _appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                id = _appContext.AppUser.ParentId;

            Affiliate affiliate = _affiliateService.GetAffiliateById(id, false);
            //ViewBag.BuyerCompanyName = !string.IsNullOrEmpty(affiliate?.Name) ? $"Affiliate > {affiliate.Name} > " : string.Empty;
            //ViewBag.SelectedAffiliateId = id;

            var affiliateModel = new AffiliateModel {AffiliateId = id, Name = affiliate?.Name};

            return Ok(affiliateModel);
        }

        /// <summary>
        /// Reports the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("{id}/reports")]
        public IHttpActionResult Reports(long id = 0)
        {
            if (!_permissionService.Authorize(_viewAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (_appContext.AppUser != null)
            {
                if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                    id = _appContext.AppUser.ParentId;
                else if (_appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
                {
                    throw new ArgumentException("User type is Buyer not allowed for current action");
                }
            }

            Affiliate affiliate = _affiliateService.GetAffiliateById(id, false);
            //ViewBag.BuyerCompanyName = !string.IsNullOrEmpty(affiliate?.Name) ? $"Affiliate > {affiliate.Name} > " : string.Empty;
            //ViewBag.SelectedAffiliateId = id;

            var affiliateModel = new AffiliateModel {AffiliateId = id, Name = affiliate?.Name};

            return Ok(affiliateModel);
        }

        /// <summary>
        /// Get affiliate list
        /// </summary>
        /// <returns>Json result</returns>
        //[ContentManagementAntiForgery(true)]
        [HttpGet]
        [Route("list/deleted/{deleted}/status/{status}")]
        public IHttpActionResult GetAffiliates(string deleted, string status = null)
        {
            if (!_permissionService.Authorize(_viewAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            short.TryParse(deleted, out var deletedResult);

            //var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
            short statusResult = -1;
            if (status != null)
            {
                statusResult = short.Parse(status);
            }

            var affiliates = (List<Affiliate>) this._affiliateService.GetAllAffiliates(_permissionService.Authorize(PermissionProvider.AffiliatesShowAll) ? null : _appContext.AppUser, deletedResult);

            var jd = new JsonData {draw = 1, recordsTotal = affiliates.Count, recordsFiltered = affiliates.Count};
            foreach (Affiliate ai in affiliates)
            {
                if (statusResult != -1 && ai.Status != statusResult) continue;

                Country country = _countryService.GetCountryById(ai.CountryId);
                User user = _userService.GetUserById(ai.ManagerId ?? 0);
                var channels = (List<AffiliateChannel>) _affiliateChannelService.GetAllAffiliateChannelsByAffiliateId(ai.Id);

                var activeChannels = 0;
                var inactiveChannels = 0;

                foreach (AffiliateChannel channel in channels)
                {
                    if (channel.Status == 1) activeChannels++;
                    else inactiveChannels++;
                }

                string appliedStatus = "Applied";
                string color = "yellow";

                switch (ai.Status)
                {
                    case 1:
                        appliedStatus = "Active";
                        color = "green";
                        break;
                    case 0:
                        appliedStatus = "Blocked";
                        color = "red";
                        break;
                    case 2:
                        appliedStatus = "Applied";
                        color = "blue";
                        break;
                    case 3:
                        appliedStatus = "Pending";
                        color = "orange";
                        break;
                    case 4:
                        appliedStatus = "Rejected";
                        color = "red";
                        break;
                }

                string[] names1 =
                {
                    ai.Id.ToString(),
                    _permissionService.Authorize(PermissionProvider.AffiliatesModify)
                        ? "<a href=\"/Management/Affiliate/Item/" + ai.Id.ToString() + "\">" + ai.Name + "</a>"
                        : "<b>" + ai.Name + "</b>",
                    ai.Email,
                    activeChannels.ToString() + "/" + inactiveChannels.ToString(),
                    user == null ? "" : user.Username,
                    ai.CreatedOn.ToShortDateString(),
                    ai.RegistrationIp,
                    "<span style='color: " + color + "'>" + appliedStatus + "</span>",
                    "<a href='#' onclick='deleteAffiliate(" + ai.Id + ")'>" +
                    (ai.IsDeleted.HasValue ? (ai.IsDeleted.Value ? "Restore" : "Delete") : "Delete") + "</a>"
                };
                jd.data.Add(names1);
            }

            return Ok(jd);
        }

        /// <summary>
        /// Checks the can delete.
        /// </summary>
        /// <param name="affiliate">The affiliate.</param>
        /// <returns>System.String.</returns>
        [HttpGet]
        [NonAction]
        [Route("checkCanDelete")]
        public string CheckCanDelete([FromBody] Affiliate affiliate)
        {
            var users = (List<User>) _userService.GetUsersByParentId(affiliate.Id, SharedData.AffiliateUserTypeId, 0);

            if (users.Count > 0)
            {
                return "Can not delete affiliate because there are active users.";
            }

            var affiliateChannels =
                (List<AffiliateChannel>) _affiliateChannelService.GetAllAffiliateChannelsByAffiliateId(affiliate.Id, 0);

            if (affiliateChannels.Count > 0)
            {
                return "Can not delete because there are active affiliate channels.";
            }

            var notes = (List<AffiliateNote>) _affiliateNoteService.GetAllAffiliateNotesByAffiliateId(affiliate.Id);

            if (notes.Count > 0)
            {
                return "Can not delete because there are active affiliate notes.";
            }

            return string.Empty;
        }

        /// <summary>
        /// Deletes the affiliate.
        /// </summary>
        /// <returns>ActionResult.</returns>
        //[HttpGet]
        //[HttpPost]
        [HttpDelete]
        [Route("deleteAffiliate/{id}")]
        //[ContentManagementAntiForgery(true)]
        public IHttpActionResult DeleteAffiliate([FromUri] long id)
        {
            string message = string.Empty;
            if (!_permissionService.Authorize(_editAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }

            Affiliate affiliate = _affiliateService.GetAffiliateById(id, false);
            if (affiliate != null)
            {
                if (affiliate.IsDeleted.HasValue)
                {
                    affiliate.IsDeleted = !affiliate.IsDeleted.Value;
                }
                else
                    affiliate.IsDeleted = true;

                if (affiliate.IsDeleted.Value)
                    message = CheckCanDelete(affiliate);

                if (message.Length > 0)
                {
                    affiliate.IsDeleted = !affiliate.IsDeleted.Value;
                    return HttpBadRequest(message);
                    //Json(new { result = false, message = message }, JsonRequestBehavior.AllowGet);
                }

                _affiliateService.UpdateAffiliate(affiliate);
            }
            else
                return HttpBadRequest("Affiliate does not exist");

            return Ok(new {id = affiliate.Id});
        }

        /// <summary>
        /// Logins as.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [HttpPost]
        [Route("loginAs/{id}")]
        public IHttpActionResult LoginAs(long id)
        {
            if (!_permissionService.Authorize(_viewAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }

            Affiliate affiliate = _affiliateService.GetAffiliateById(id, false);

            if (affiliate != null)
            {
                long? userId = 0;
                var entities = _userService.GetEntityOwnership("affiliate", affiliate.Id);
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

            return HttpBadRequest("No user found"); // Redirect(Helper.GetBaseUrl(Request) + "/Management/Home/Dashboard");
        }


        [HttpGet]
        [Route("getAffiliatesBySearchPattern")]
        public IHttpActionResult GetAffiliatesBySearchPattern(string inputValue)
        {
            if (!_permissionService.Authorize(_viewAffiliateInformationKey))
            {
                return HttpBadRequest("access-denied");
            }
            var result = new List<AffiliateListItemModel>();
            //var affiliates = GetAffiliateAdvancedList("");
            var affiliates = _affiliateService.GetAffiliatesByUser("");
            foreach (var affiliate in affiliates)
            {
                var affiliateModel = CreateAffiliateListItem(affiliate);
                if (_searchService.CheckPropValue(affiliateModel, inputValue))
                {
                    result.Add(affiliateModel);
                }
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("getFormTemplateById/{id}")]
        public IHttpActionResult GetFormTemplateById(int id)
        {
            try
            {
                var key = $"FormTemplate_{id}";
                var setting = _settingService.GetSetting(key);
                return Ok(setting.Value);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region private methods

        private long GetUserRoleIdByKey(string key)
        {
            return this._roleService.GetRoleByKey(key).Id;
        }

        private AffiliateModel NeedToRegister(AffiliateModel affiliateModel)
        {
            affiliateModel.IsNotRegistered = true;
            return affiliateModel;
        }


        private AffiliateListItemModel CreateAffiliateListItem(Affiliate affiliate)
        {
            var createdBy = string.Empty;
            var createdHistoryObj = _entityChangeHistoryService.GetEntityHistory(affiliate.Id, "Affiliate", "Added");
            if(createdHistoryObj != null)
                createdBy = _userRepository.Table.FirstOrDefault(x => x.Id == createdHistoryObj.UserId)?.Username;

            var updatedBy = string.Empty;
            var updatedHistoryObj = _entityChangeHistoryService.GetEntityHistory(affiliate.Id, "Affiliate", "Modified");
            if (updatedHistoryObj != null)
                updatedBy = _userRepository.Table.FirstOrDefault(x => x.Id == updatedHistoryObj.UserId)?.Username;

            DateTime? createDate = null;
            if (createdHistoryObj != null)
                createDate = _settingService.GetTimeZoneDate(createdHistoryObj.ModifiedDate);

            DateTime? updateDate = null;
            if (updatedHistoryObj != null)
                updateDate = _settingService.GetTimeZoneDate(updatedHistoryObj.ModifiedDate);


            var manager = _userRepository.Table.FirstOrDefault(x => x.Id == affiliate.ManagerId);
            var allChannels = _affiliateChannelService.GetAllAffiliateChannelsByAffiliateId(affiliate.Id);
            var activeChannels = allChannels.Where(x => x.Status == (short)EntityStatus.Active).ToList();

            var affiliateAllChannelsCount = allChannels.Count(x => x.AffiliateId == affiliate.Id);
            var affiliateActiveChannelsCount = activeChannels.Count(x => x.AffiliateId == affiliate.Id);
            return new AffiliateListItemModel()
            {
                ActiveChannels = affiliateActiveChannelsCount,
                AllChannels = affiliateAllChannelsCount,
                AffiliateId = affiliate.Id,
                AffiliateName = affiliate.Name,
                IsActive = (EntityStatus)affiliate.Status,
                Manager = manager?.Email,
                RegistrationDate = affiliate.CreatedOn,
                RegistrationIpAddress = affiliate.RegistrationIp,
                Icon = affiliate.IconPath,
                CreatedDate = createDate,
                CreatedBy = createdBy,
                UpdatedDate = updateDate,
                UpdatedBy = updatedBy
            };
        }

        /*
        private AffiliateListItemModel CreateAffiliateListItem(AffiliateAdvancedModel affiliateAdvancedModel)
        {
            var allChannels = _affiliateChannelService.GetAllAffiliateChannelsByAffiliateId(affiliateAdvancedModel.Affiliate.Id);
            var activeChannels = allChannels.Where(x => x.Status == (short) EntityStatus.Active).ToList();

            var affiliateAllChannelsCount = allChannels.Count(x => x.AffiliateId == affiliateAdvancedModel.Affiliate.Id);
            var affiliateActiveChannelsCount = activeChannels.Count(x => x.AffiliateId == affiliateAdvancedModel.Affiliate.Id);
            return new AffiliateListItemModel()
            {
                ActiveChannels = affiliateActiveChannelsCount,
                AllChannels = affiliateAllChannelsCount,
                AffiliateId = affiliateAdvancedModel.Affiliate.Id,
                AffiliateName = affiliateAdvancedModel.Affiliate.Name,
                IsActive = (EntityStatus) affiliateAdvancedModel.Affiliate.Status,
                Manager = affiliateAdvancedModel.Manager,
                RegistrationDate = affiliateAdvancedModel.Affiliate.CreatedOn,
                RegistrationIpAddress = affiliateAdvancedModel.Affiliate.RegistrationIp,
                Icon = affiliateAdvancedModel.Affiliate.IconPath
            };
        }
        */

        /*
        private List<AffiliateAdvancedModel> GetAffiliateAdvancedList(string name, EntityFilterByStatus status = EntityFilterByStatus.Active)
        {
            var affiliateAndManager = (from a in _affiliateService.GetAllAffiliates(name, status)
                join u in _userRepository.Table on a.ManagerId equals u.Id
                where a.UserId == _appContext.AppUser.Id
                select new AffiliateAdvancedModel
                {
                    Affiliate = a,
                    Manager = u?.Email
                }).ToList();
            return affiliateAndManager;
        }
        */

        #endregion

        #endregion
    }

    static class AffiliateExtension
    {
        internal static AffiliateModel AbsolutePathBuilder(this AffiliateModel affiliateModel,
            System.Net.Http.HttpRequestMessage request)
        {
            var affiliateIcon = affiliateModel.Icon;
            if (!string.IsNullOrWhiteSpace(affiliateIcon) && affiliateIcon.Contains("~"))
            {
                affiliateIcon = affiliateIcon.Replace("~", string.Empty);
                affiliateIcon = $"{request.RequestUri.GetLeftPart(UriPartial.Authority)}{affiliateIcon}";
                affiliateModel.Icon = affiliateIcon;
            }

            return affiliateModel;
        }
    }
}
