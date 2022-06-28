using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Adrack.Core;
using Adrack.Core.Helpers;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Security;
using Adrack.Service.Common;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.Service.Helpers;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Infrastructure.Web.Helpers;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Employee;
using Adrack.WebApi.Models.Security;
using Adrack.WebApi.Models.Users;
using Adrack.Service.Configuration;
using Adrack.Web.Framework.Cache;
using System.Configuration;
using System.Web.Script.Serialization;
using Adrack.Core.Cache;
using Adrack.PlanManagement;
using Adrack.Core.Infrastructure;
using Adrack.Service.Content;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : BaseApiController
    {
        #region properties
        private readonly IAppContext _appContext;
        private readonly IUserService _userService;
        private readonly IProfileService _profileService;
        private readonly IEncryptionService _encryptionService;
        private readonly IAffiliateService _affiliateService;
        private readonly IBuyerService _buyerService;
        private readonly IRoleService _roleService;
        private readonly IUsersExtensionService _usersExtensionService;
        private readonly ISearchService _searchService;
        private readonly IGlobalAttributeService _globalAttributeService;
        private readonly IEmailService _emailService;
        private readonly ISettingService _settingService;
        private readonly EmailOperatorEnums _emailProvider;
        private readonly ICacheManager _cacheManager;
        private readonly IPlanService _planService;
        private readonly IStorageService _storageService;

        private string _uploadAvatarFolderUrl => $"{Request.RequestUri.GetLeftPart(UriPartial.Authority)}/Content/Uploads/Avatars/";

        private string _uploadFolderUrl => (Request != null && Request.RequestUri != null) ? $"{Request.RequestUri.GetLeftPart(UriPartial.Authority)}/Content/Uploads/" : string.Empty;

        #endregion properties

        #region constructor

        public UserController(IUserService userService,
                              IAppContext appContext,
                              IProfileService profileService,
                              IEncryptionService encryptionService,
                              IAffiliateService affiliateService,
                              IBuyerService buyerService,
                              IRoleService roleService,
                              IUsersExtensionService usersExtensionService,
                              ISearchService searchService,
                              IGlobalAttributeService globalAttributeService,
                              IEmailService emailService,
                              ISettingService settingService,
                              ICacheManager cacheManager,
                              IPlanService planService,
                              IStorageService storageService
                              )
        {
            _appContext = appContext;
            _userService = userService;
            _profileService = profileService;
            _encryptionService = encryptionService;
            _affiliateService = affiliateService;
            _buyerService = buyerService;
            _roleService = roleService;
            _usersExtensionService = usersExtensionService;
            _searchService = searchService;
            _globalAttributeService = globalAttributeService;
            _emailService = emailService;
            _settingService = settingService;
            _planService = planService;
            _cacheManager = cacheManager;
            _storageService = storageService;
        }

        #endregion constructor

        /// <summary>
        /// Get Users depends on parameters
        /// </summary>
        /// <param name="roleId">long</param>
        /// <param name="isDeleted">bool</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getUserList")]
        public IHttpActionResult GetUserList(long? roleId = null, bool isDeleted = false)
        {
            List<UserProfileModel> userModels;
            if (roleId == null || roleId == 0)
            {
                userModels = GetUsers(isDeleted);
            }
            else
            {
                userModels = GetUsersWithRolesByRoleId(roleId.Value, isDeleted);
            }

            userModels = userModels.Where(x => x.UserType != UserTypes.Super).ToList();

            return Ok(userModels);
        }


        [HttpGet]
        [Route("getAffiliateUserList")]
        public IHttpActionResult GetAffiliateUserList(long? affiliateId = null, bool isDeleted = false)
        {
            List<UserProfileModel> userModels = GetUsers(isDeleted, "network", null);

            return Ok(userModels);
        }

        [HttpGet]
        [Route("getBuyerUserList")]
        public IHttpActionResult GetBuyerUserList(long? buyerId = null, bool isDeleted = false)
        {
            try
            {
                List<UserProfileModel> userModels = GetUsers(isDeleted, "network", null);
                return Ok(userModels);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        [Route("getNetworkUserList")]
        public IHttpActionResult GetNetowrkUserList(bool isDeleted = false)
        {
            try
            {
                var userModels = GetUsers(isDeleted, "network");
                return Ok(userModels);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public void Post([FromBody] UserModel settingModel)
        {
            InsertOrUpdateUser(settingModel);
        }

        [HttpGet]
        [Route("getProfile/{userId}")]
        public IHttpActionResult GetUserProfile(int userId)
        {
            var user = _userService.GetUserById(userId);
            var userModel = new UserModel();
            if (user != null)
            {
                var userProfile = _profileService.GetProfileByUserId(user.Id);
                var profile = _profileService.GetProfileByUserId(user.Id);
                userModel = new UserModel()
                {
                    UserId = user.Id,
                    FirstName = profile?.FirstName,
                    LastName = profile?.LastName,
                    RoleName = user.Roles?.FirstOrDefault()?.Name,
                    LastLoginDate = _settingService.GetTimeZoneDate(user.LoginDate),
                    ProfilePictureURL = string.IsNullOrWhiteSpace(user.ProfilePicturePath) ? null : user.ProfilePicturePath
                };
                return Ok(userModel);
            }
            else
            {
                throw new ArgumentNullException(nameof(userModel),
                    $"Requested user can not be found");
            }
        }

        [Route("uploadProfilePicture")]
        public IHttpActionResult UploadProfilePicture()
        {
            try
            {
                var user = _userService.GetUserById(_appContext.AppUser.Id);
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files["ProfilePicture"] != null)
                {
                    var file = httpRequest.Files.Get("ProfilePicture");
                    var ext = Path.GetExtension(file.FileName);
                    var imageName = $"profile_picture_{_appContext.AppUser.Id}{Guid.NewGuid()}{ext}";
                    var relativePath = "~/Content/Uploads/Avatars/";
                    if (file != null)
                    {
                        var blobPath = "uploads";
                        var uri = _storageService.Upload(blobPath, file.InputStream, file.ContentType, imageName);
                        user.ProfilePicturePath = uri.AbsoluteUri;
                        _userService.UpdateUser(user);
                        return Ok(uri.AbsoluteUri);

                        Stream fs = file.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        byte[] bytes = br.ReadBytes((Int32)fs.Length);

                        var targetFolder = System.Web.Hosting.HostingEnvironment.MapPath(relativePath);
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

                        if (!string.IsNullOrWhiteSpace(user.ProfilePicturePath))
                        {
                            var deletePath = HttpContext.Current.Server.MapPath(user.ProfilePicturePath);

                            if (File.Exists(deletePath))
                            {
                                File.Delete(deletePath);
                            }
                        }

                        file.SaveAs(targetPath);
                    }
                    else
                        return HttpBadRequest("File is not attached");

                    user.ProfilePicturePath = imageName;
                    _userService.UpdateUser(user);
                    return Ok($"{_uploadAvatarFolderUrl}{imageName}");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("deleteProfilePicture")]
        public IHttpActionResult DeleteProfilePicture()
        {
            try
            {
                var user = _userService.GetUserById(_appContext.AppUser.Id);
                if (user != null && !string.IsNullOrWhiteSpace(user.ProfilePicturePath))
                {
                    var relativePath = "~/Content/Uploads/Avatars/";
                    var targetFolder = HttpContext.Current.Server.MapPath(relativePath);
                    var splittedFileName = user.ProfilePicturePath.Split(new[] { "profile_picture_" }, StringSplitOptions.None);
                    if (splittedFileName.Length != 0)
                    {
                        var fileName = Path.Combine(targetFolder, $"profile_picture_{splittedFileName[1]}");
                        File.Delete(fileName);
                    }
                    user.ProfilePicturePath = string.Empty;
                    _userService.UpdateUser(user);
                }
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("client/host")]
        public string GetClientHost()
        {
            var userHost = this.Request.GetClientIp();
            return userHost;
        }

        private List<UserProfileModel> GetUsersWithRolesByRoleId(long id, bool isDeleted)
        {
            var userModels = new List<UserProfileModel>();
            var users = _userService.GetUsersWithRolesByRoleId(id).Where(x => x.Deleted == isDeleted).ToList();
            foreach (var user in users)
            {
                userModels.Add(new UserProfileModel
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RoleName = user.RoleName,
                    LastLoginDate = _settingService.GetTimeZoneDate(user.LoginDate),
                    UserType = (UserTypes)user.UserType,
                    IsActive = user.Active
                });
            }
            return userModels;
        }

        private List<UserProfileModel> GetUsers(bool isDeleted, string entityName = "", long? entityId = null)
        {
            var userModels = new List<UserProfileModel>();
            List<User> users = null;
            List<AffiliateInvitation> invitations = null;

            switch (entityName)
            {
                case "":
                    users = _userService.GetAllUsers(Convert.ToInt16(isDeleted)).ToList();
                    break;
                case "affiliate":
                    users = _userService.GetAffiliateUsers(entityId, Convert.ToInt16(isDeleted)).ToList();
                    break;
                case "buyer":
                    users = _userService.GetBuyerUsers(entityId, Convert.ToInt16(isDeleted)).ToList();
                    break;
                case "network":
                    users = _userService.GetNetworkUsers(Convert.ToInt16(isDeleted)).ToList();
                    break;
            }


            foreach (var user in users)
            {
                AffiliateInvitation invitation = null;

                if (entityName == "affiliate")
                    invitation = _affiliateService.GetAffiliateInvitation(entityId.HasValue ? entityId.Value : 0, user.Email);

                var profile = _profileService.GetProfileByUserId(user.Id);

                string imageUrl = user?.ProfilePicturePath;

                Uri uri;

                if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out uri) && !string.IsNullOrEmpty(imageUrl))
                    imageUrl = $"{_uploadFolderUrl}Avatars/{user.ProfilePicturePath}"; 

                userModels.Add(new UserProfileModel()
                {
                    UserId = user.Id,
                    FirstName = profile?.FirstName,
                    LastName = profile?.LastName,
                    Email = user.Email,
                    RoleName = user.Roles?.FirstOrDefault()?.Name,
                    LastLoginDate = user != null ? _settingService.GetTimeZoneDate(user.LoginDate) : DateTime.UtcNow,
                    LogoPath = imageUrl,
                    IsActive = user.Active,
                    UserType = user.UserType,
                    ApprovalStatus = invitation != null ? invitation.Status : AffiliateInvitationStatuses.None
                });

            }

            return userModels;
        }

        [HttpPost]
        [Route("attachUser")]
        public IHttpActionResult AttachUser([FromBody] UserInviteModel userInviteModel)
        {
            try
            {
                var currentUser = _userService.GetUserById(_appContext.AppUser.Id);
                var attachUser = _userService.GetUserById(userInviteModel.UserId);
                if ((currentUser.UserType == UserTypes.Affiliate && attachUser.UserType == UserTypes.Buyer) ||
                    (currentUser.UserType == UserTypes.Buyer && attachUser.UserType == UserTypes.Affiliate))
                {
                    return HttpBadRequest($"do not have permission to attach {attachUser.UserType} type of user");
                }

                EntityOwnership entityOwnership = new EntityOwnership();
                entityOwnership.EntityId = userInviteModel.EntityId;
                entityOwnership.EntityName = userInviteModel.EntityName;
                entityOwnership.UserId = userInviteModel.UserId;
                entityOwnership.IsApproved = false;
                _userService.InsertEntityOwnership(entityOwnership);
                return Ok();
            }
            catch (Exception ex)
            {
                return HttpBadRequest(null);
            }
        }

        [HttpPost]
        [Route("detachUser/{userId}/{entityName}/{entityId}")]
        public IHttpActionResult DetachUser(long userId, string entityName, long entityId)
        {
            var user = _userService.GetUserById(userId);
            if (user == null)
            {
                return HttpBadRequest($"no user was found for given id {userId}");
            }
            var currentUser = _userService.GetUserById(_appContext.AppUser.Id);
            
            if ((currentUser.UserType == UserTypes.Affiliate && user.UserType == UserTypes.Buyer) ||
                (currentUser.UserType == UserTypes.Buyer && user.UserType == UserTypes.Affiliate))
            {
                return HttpBadRequest($"do not have permission to detach {user.UserType} type of user");
            }
            _userService.DeleteEntityOwnership(userId, entityName, Guid.Empty);

            return Ok();
        }

        [HttpPost]
        [Route("updateUser/{id}")]
        public IHttpActionResult UpdateUser(long id, [FromBody]EmployeeUpdateModel employeeModel)
        {
            try
            {
                var user = _userService.GetUserById(id);

                if (user == null)
                {
                    ModelState.AddModelError("Error", $"no user was found for given id {id}");
                    return HttpBadRequest($"no user was found for given id {id}");
                }

                if (employeeModel == null)
                {
                    ModelState.AddModelError("Error", $"user update model is null for given id {id}");
                    return HttpBadRequest($"user update model is null for given id {id}");
                }

                var role = _roleService.GetRoleById(employeeModel.RoleId);

                if (role == null || role.Deleted || !role.Active)
                {
                    ModelState.AddModelError("Error", $"no role was found for given id {employeeModel.RoleId}");
                    return HttpBadRequest($"no role was found for given id {employeeModel.RoleId}");
                }

                if (user.UserType != UserTypes.Super &&
                    user.UserType != UserTypes.Network &&
                    user.UserType != role.UserType)
                {
                    return HttpBadRequest($"can not update user with the specified role");
                }

                var profile = _profileService.GetProfileByUserId(id);
                if (profile != null)
                {
                    profile.FirstName = employeeModel.FirstName;
                    profile.LastName = employeeModel.LastName;
                    profile.MiddleName = employeeModel.MiddleName;
                    profile.JobTitle = employeeModel.JobTitle;
                    _profileService.UpdateProfile(profile);
                }

                user.Active = employeeModel.IsActive;
                user.PlanId = employeeModel.PlanId;
                _userService.UpdateUser(user);

                user.Roles.Clear();
                user.Roles.Add(role);

                _userService.ClearEntityOwnership(id);

                _userService.AddEntityOwnerships(employeeModel.Campaigns,
                                           employeeModel.Buyers,
                                           employeeModel.BuyerChannels,
                                           employeeModel.Affiliates,
                                           employeeModel.AffiliateChannels,
                                           user.Id);
                return Ok(user);
            }
            catch (Exception e)
            {
                return HttpBadRequest(null);
            }
        }

        [HttpPost]
        [Route("updateUserPlan/{userId}/{planId}")]
        public IHttpActionResult UpdateUserPlan(long userId, long planId)
        {
            try
            {
                var user = _userService.GetUserById(userId);

                if (user == null)
                {
                    ModelState.AddModelError("Error", $"no user was found for given id {userId}");
                    return HttpBadRequest($"no user was found for given id {userId}");
                }

                user.PlanId = planId;
                _userService.UpdateUser(user);

                return Ok(user);
            }
            catch (Exception e)
            {
                return HttpBadRequest(null);
            }
        }

        [HttpPost]
        [Route("addUser")]
        public IHttpActionResult AddUser([FromBody]EmployeeModel employeeModel)
        {
            try
            {
                if (employeeModel == null)
                {
                    return HttpBadRequest(null);
                }
                
                var planLimitation = _planService.CheckPlanStatusesByUserId(_appContext.AppUser.Id);
                if (planLimitation != null && planLimitation.Contains(AdrackPlanVerificationStatus.UsersLimitReached))
                {
                    return HttpBadRequest($"user limit reached.");
                }


                var role = _roleService.GetRoleById(employeeModel.RoleId);

                if (role == null || ((role.Deleted || !role.Active) && (!role.IsSystemRole.HasValue || !role.IsSystemRole.Value)))
                {
                    return HttpBadRequest($"no role was found for given id {employeeModel.RoleId}");
                }

                User employee = null;

                employee = _userService.GetUserByEmail(employeeModel.Email);

                if (employee != null)
                {
                    return HttpBadRequest($"User with '{employeeModel.Email}' email already exist");
                }

                var currentUser = _userService.GetUserById(_appContext.AppUser.Id);
                if (!_userService.ValidateCreateOppositeUser(currentUser.UserType, employeeModel.UserType))
                {
                    return HttpBadRequest($"do not have permission to add {employeeModel.UserType} type of user");
                }

                if (employeeModel.UserType != UserTypes.Super &&
                    employeeModel.UserType != UserTypes.Network &&
                    employeeModel.UserType != role.UserType)
                {
                    return HttpBadRequest($"can not add user with the specified role");
                }

                if (!_userService.ValidateUserAccessSection(
                    employeeModel.UserType, 
                    employeeModel.Buyers,
                    employeeModel.BuyerChannels,
                    employeeModel.Affiliates,
                    employeeModel.AffiliateChannels
                    )
                )
                {
                    return HttpBadRequest($"User '{employeeModel.Email}' access section is invalid");
                }
                

                string generatedPassword = GeneratePassword(8, 1); //"x0$Mm@0PgM3M";

                IEnumerable<string> headerValues;
                Request.Headers.TryGetValues("Authorization", out headerValues);
                string token = headerValues != null ? headerValues.First() : "";

                var headers = new Dictionary<string, string>();
                headers.Add("Authentication", "Bearer " + token);

                string requestKey = _cacheManager.Get($"user{_appContext.AppUser.Id}-requestkey")?.ToString();

                string masterAdminUrl = ConfigurationManager.AppSettings["MasterAdminUrl"];

                string json = new JavaScriptSerializer().Serialize(new
                {
                    firstName = employeeModel.FirstName,
                    lastName = employeeModel.LastName,
                    email = employeeModel.Email,
                    password = generatedPassword,
                    confirmPassword = generatedPassword,
                    zipCode = "33333",
                    webSite = "http://adrack.com",
                    companyName = "a",
                    address = "a",
                    phone = "8184567894",
                    city = "a",
                    countryId = 80,
                    token = requestKey
                });

                string remoteSignUpResult = "";// Helpers.Helpers.Post($"{masterAdminUrl}/api/authentication/remoteSignUp", json, "application/json", "POST", headers);
                remoteSignUpResult = "";

                if (remoteSignUpResult.ToLower() != "error")
                {
                    string saltKey = _encryptionService.CreateSaltKey(20);
                    string password = _encryptionService.CreatePasswordHash(generatedPassword, saltKey);

                    employee = new User
                    {
                        Id = 0,
                        ParentId = 0,
                        UserType = employeeModel.UserType,
                        PlanId = employeeModel.PlanId,
                        GuId = Guid.NewGuid().ToString(),
                        Username = employeeModel.Email,
                        Email = employeeModel.Email,
                        ContactEmail = employeeModel.Email,
                        Password = password,
                        SaltKey = saltKey,
                        Active = false,
                        LockedOut = false,
                        Deleted = false,
                        BuiltIn = false,
                        BuiltInName = null,
                        RegistrationDate = DateTime.UtcNow,
                        LoginDate = DateTime.UtcNow,
                        ActivityDate = DateTime.UtcNow,
                        PasswordChangedDate = null,
                        LockoutDate = null,
                        IpAddress = null,
                        FailedPasswordAttemptCount = null,
                        Comment = null,
                        DepartmentId = 1,
                        MenuType = null,
                        MaskEmail = false,
                        ValidateOnLogin = false,
                        ChangePassOnLogin = false,
                        TimeZone = null,
                        RemoteLoginGuid = null
                    };
                    employee.Roles.Add(role);
                    _userService.InsertUser(employee);

                    var profile = new Profile
                    {
                        Id = 0,
                        UserId = employee.Id,
                        FirstName = employeeModel.FirstName,
                        LastName = employeeModel.LastName,
                        MiddleName = employeeModel.MiddleName,
                        JobTitle = employeeModel.JobTitle,
                        Phone = "",
                        CellPhone = "",
                        Summary = "",
                    };
                    _profileService.InsertProfile(profile);

                    _userService.AddEntityOwnerships(employeeModel.Campaigns,
                        employeeModel.Buyers,
                        employeeModel.BuyerChannels,
                        employeeModel.Affiliates,
                        employeeModel.AffiliateChannels,
                        employee.Id);

                    var userModel = new EmployeeAdvancedModel()
                    {
                        UserId = employee.Id,
                        FirstName = profile.FirstName,
                        LastName = profile.LastName,
                        JobTitle = profile.JobTitle,
                        RoleId = employee.Roles?.FirstOrDefault()?.Id,
                        IsActive = employee.Active,
                        LogoPath = string.IsNullOrWhiteSpace(employee.ProfilePicturePath) ? null : employee.ProfilePicturePath
                    };


                    _globalAttributeService.SaveGlobalAttribute(employee, GlobalAttributeBuiltIn.FirstPasswordToken, employee.GuId);
                    ////_globalAttributeService.SaveGlobalAttribute(employee, GlobalAttributeBuiltIn.FirstPasswordTokenRequestedDate, DateTime.UtcNow);
                    _emailService.SendUserFirstPasswordMessage(employee, _appContext.AppLanguage.Id, generatedPassword);


                    //_globalAttributeService.SaveGlobalAttribute(employee, GlobalAttributeBuiltIn.MembershipActivationToken, employee.GuId);
                    //_emailService.SendUserWelcomeMessageWithUsernamePassword(employee, _appContext.AppLanguage.Id, EmailOperatorEnums.LeadNative, employee.Email, generatedPassword);


                    return Ok(userModel);
                }
                else
                {
                    return HttpBadRequest("Error");
                }
            }
            catch (Exception e)
            {
                return HttpBadRequest(e.Message);
            }
        }


        /* go to MemberShip Controller
        [HttpPost]
        [Route("addInvitatedUser")]
        public IHttpActionResult AddInvitatedUser([FromBody] InvitedEmployeeModel employeeModel)
        {
            try
            {
                if (employeeModel == null)
                {
                    return HttpBadRequest(null);
                }

                var role = _roleService.GetRoleById(employeeModel.RoleId);

                if (role == null || role.Deleted || !role.Active)
                {
                    return HttpBadRequest($"no role was found for given id {employeeModel.RoleId}");
                }

                User employee = null;

                employee = _userService.GetUserByEmail(employeeModel.Email);
                if (employee != null)
                {
                    return HttpBadRequest($"User with '{employeeModel.Email}' email already exist");
                }

                var currentUser = _userService.GetUserById(_appContext.AppUser.Id);
                if (!_userService.ValidateCreateOppositeUser(currentUser.UserType, employeeModel.UserType))
                {
                    return HttpBadRequest($"do not have permission to add {employeeModel.UserType} type of user");
                }

                if (!_userService.ValidateUserAccessSection(
                        employeeModel.UserType,
                        employeeModel.Buyers,
                        employeeModel.BuyerChannels,
                        employeeModel.Affiliates,
                        employeeModel.AffiliateChannels
                    )
                )
                {
                    return HttpBadRequest($"User '{employeeModel.Email}' access section is invalid");
                }

                var invitedUserToken = _globalAttributeService.GetGlobalAttributeByKeyAndValue(GlobalAttributeBuiltIn.InvitedUserToken, employeeModel.InvitedUserToken);
                if (invitedUserToken == null)
                {
                    return HttpBadRequest("Wrong Invited User token");
                }

                if (string.IsNullOrWhiteSpace(employeeModel.Password) || employeeModel.Password.Length <= 8)
                {
                    return HttpBadRequest($"Password must be longer than 8 characters.");
                }
                string generatedPassword = employeeModel.Password;

                IEnumerable<string> headerValues;
                Request.Headers.TryGetValues("Authorization", out headerValues);
                string token = headerValues != null ? headerValues.First() : "";

                var headers = new Dictionary<string, string>();
                headers.Add("Authentication", "Bearer " + token);

                string requestKey = _cacheManager.Get($"user{_appContext.AppUser.Id}-requestkey")?.ToString();

                string masterAdminUrl = ConfigurationManager.AppSettings["MasterAdminUrl"];

                string json = new JavaScriptSerializer().Serialize(new
                {
                    firstName = employeeModel.FirstName,
                    lastName = employeeModel.LastName,
                    email = employeeModel.Email,
                    password = generatedPassword,
                    confirmPassword = generatedPassword,
                    zipCode = "33333",
                    webSite = "http://adrack.com",
                    companyName = "a",
                    address = "a",
                    phone = "8184567894",
                    city = "a",
                    countryId = 80,
                    token = requestKey
                });

                string remoteSignUpResult = Helpers.Helpers.Post($"{masterAdminUrl}/api/authentication/remoteSignUp", json, "application/json", "POST", headers);
                remoteSignUpResult = "";

                if (remoteSignUpResult.ToLower() != "error")
                {
                    string saltKey = _encryptionService.CreateSaltKey(20);
                    string password = _encryptionService.CreatePasswordHash(generatedPassword, saltKey);

                    employee = new User
                    {
                        Id = 0,
                        ParentId = 0,
                        UserType = employeeModel.UserType,
                        GuId = Guid.NewGuid().ToString(),
                        Username = employeeModel.Email,
                        Email = employeeModel.Email,
                        ContactEmail = employeeModel.Email,
                        Password = password,
                        SaltKey = saltKey,
                        Active = true,
                        LockedOut = false,
                        Deleted = false,
                        BuiltIn = false,
                        BuiltInName = null,
                        RegistrationDate = DateTime.Now,
                        LoginDate = DateTime.Now,
                        ActivityDate = DateTime.Now,
                        PasswordChangedDate = null,
                        LockoutDate = null,
                        IpAddress = null,
                        FailedPasswordAttemptCount = null,
                        Comment = null,
                        DepartmentId = 1,
                        MenuType = null,
                        MaskEmail = false,
                        ValidateOnLogin = false,
                        ChangePassOnLogin = false,
                        TimeZone = null,
                        RemoteLoginGuid = null
                    };
                    employee.Roles.Add(role);
                    _userService.InsertUser(employee);

                    var profile = new Profile
                    {
                        Id = 0,
                        UserId = employee.Id,
                        FirstName = employeeModel.FirstName,
                        LastName = employeeModel.LastName,
                        MiddleName = employeeModel.MiddleName,
                        JobTitle = employeeModel.JobTitle,
                        Phone = "",
                        CellPhone = "",
                        Summary = "",
                    };
                    _profileService.InsertProfile(profile);

                    _userService.AddEntityOwnerships(employeeModel.Campaigns,
                        employeeModel.Buyers,
                        employeeModel.BuyerChannels,
                        employeeModel.Affiliates,
                        employeeModel.AffiliateChannels,
                        employee.Id);


                    _globalAttributeService.DeleteGlobalAttributeByKeyAndValue(GlobalAttributeBuiltIn.InvitedUserToken, invitedUserToken.Value);

                    _affiliateService.UpdateInvitationStatus(employee.Email);
                    var buyerService = AppEngineContext.Current.Resolve<IBuyerService>();
                    buyerService.UpdateInvitationStatus(employee.Email);

                    var setting = this._settingService.GetSetting("AppSetting.Url");
                    if (setting != null && !string.IsNullOrEmpty(setting.Value))
                        return Redirect(setting.Value);

                    return Ok("User successfully added");
                }
                else
                {
                    return HttpBadRequest("Error");
                }
            }
            catch (Exception e)
            {
                return HttpBadRequest(e.Message);
            }
        }
        */



        /*
                [HttpPost]
                [Route("generateUser")]
                public IHttpActionResult GenerateUser([FromBody] EmployeeModel employeeModel, string apiPassword)
                {
                    try
                    {
                        if (!_userService.CheckGlobalAttribute("FirstUserRegistration", apiPassword))
                        {
                            return HttpBadRequest("Incorrect password");
                        }

                        if (employeeModel == null)
                        {
                            return HttpBadRequest(null);
                        }


                        var role = _roleService.GetRoleById(employeeModel.RoleId);

                        if (role == null || role.Deleted || !role.Active)
                        {
                            return HttpBadRequest($"no role was found for given id {employeeModel.RoleId}");
                        }

                        User employee = null;

                        employee = _userService.GetUserByEmail(employeeModel.Email);
                        if (employee != null)
                        {
                            return HttpBadRequest($"User with '{employeeModel.Email}' email already exist");
                        }

                        var currentUser = _userService.GetUserById(_appContext.AppUser.Id);
                        if (!ValidateCreateOppositeUser(currentUser.UserType, employeeModel.UserType))
                        {
                            return HttpBadRequest($"do not have permission to add {employeeModel.UserType} type of user");
                        }

                        if(!ValidateUserAccessSection(
                                employeeModel.UserType,
                                employeeModel.Buyers,
                                employeeModel.BuyerChannels,
                                employeeModel.Affiliates,
                                employeeModel.AffiliateChannels
                            )
                        )
                        {
                            return HttpBadRequest($"User '{employeeModel.Email}' access section is invalid");
                        }

                        string generatedPassword = GeneratePassword(8, 1); //"x0$Mm@0PgM3M";

                        string saltKey = _encryptionService.CreateSaltKey(20);
                        string password = _encryptionService.CreatePasswordHash(generatedPassword, saltKey);

                        employee = new User
                        {
                            Id = 0,
                            ParentId = 0,
                            UserType = employeeModel.UserType,
                            GuId = Guid.NewGuid().ToString(),
                            Username = employeeModel.Email,
                            Email = employeeModel.Email,
                            ContactEmail = employeeModel.Email,
                            Password = password,
                            SaltKey = saltKey,
                            Active = false,
                            LockedOut = false,
                            Deleted = false,
                            BuiltIn = false,
                            BuiltInName = null,
                            RegistrationDate = DateTime.Now,
                            LoginDate = DateTime.Now,
                            ActivityDate = DateTime.Now,
                            PasswordChangedDate = null,
                            LockoutDate = null,
                            IpAddress = null,
                            FailedPasswordAttemptCount = null,
                            Comment = null,
                            DepartmentId = 1,
                            MenuType = null,
                            MaskEmail = false,
                            ValidateOnLogin = false,
                            ChangePassOnLogin = false,
                            TimeZone = null,
                            RemoteLoginGuid = null
                        };
                        employee.Roles.Add(role);
                        _userService.InsertUser(employee);

                        var profile = new Profile
                        {
                            Id = 0,
                            UserId = employee.Id,
                            FirstName = employeeModel.FirstName,
                            LastName = employeeModel.LastName,
                            MiddleName = employeeModel.MiddleName,
                            JobTitle = employeeModel.JobTitle,
                            Phone = "",
                            CellPhone = "",
                            Summary = "",
                        };
                        _profileService.InsertProfile(profile);

                        AddEntityOwnerships(employeeModel.Campaigns,
                            employeeModel.Buyers,
                            employeeModel.BuyerChannels,
                            employeeModel.Affiliates,
                            employeeModel.AffiliateChannels,
                            employee.Id);

                        var userModel = new EmployeeAdvancedModel()
                        {
                            UserId = employee.Id,
                            FirstName = profile.FirstName,
                            LastName = profile.LastName,
                            JobTitle = profile.JobTitle,
                            RoleId = employee.Roles?.FirstOrDefault()?.Id,
                            IsActive = employee.Active,
                            LogoPath = string.IsNullOrWhiteSpace(employee.ProfilePicturePath) ? null : employee.ProfilePicturePath
                        };

                        return Ok(userModel);
                    }
                    catch (Exception e)
                    {
                        return HttpBadRequest(e.Message);
                    }
                }
        */



        [HttpPost]
        [Route("restoreUserByEmail")]
        public IHttpActionResult RestoreUserByEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return HttpBadRequest($"Email is empty"); ;

                User employee = _userService.RestoreUserByEmail(email);
                if (employee == null)
                {
                    return HttpBadRequest($"User with '{email}' email has a problem");
                }

                return Ok(employee);
            }
            catch (Exception e)
            {
                return HttpBadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("deleteUser/{id}")]
        public IHttpActionResult DeleteUser(int id)
        {
            try
            {
                var employee = _userService.GetUserById(id);
                if (employee == null)
                {
                    ModelState.AddModelError("Error", "Employee does not exist");
                    return HttpBadRequest("Employee does not exist");
                }

                if (employee.UserType == UserTypes.Super)
                {
                    return HttpBadRequest("Super user can not be deleted");
                }

                employee.Deleted = true;
                employee.Active = false;
                _userService.UpdateUser(employee);

                var ainvitations = _affiliateService.GetAffiliateInvitationsByEmail(employee.Email);
                foreach(var invitation in ainvitations)
                {
                    _affiliateService.DeleteAffiliateInvitation(invitation.Id);
                }

                var binvitations =_buyerService.GetBuyerInvitationsByEmail(employee.Email);
                foreach (var invitation in binvitations)
                {
                    _buyerService.DeleteBuyerInvitation(invitation.Id);
                }

                return Ok(employee);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", e.InnerException?.Message ?? e.Message);
                return HttpBadRequest(e.InnerException?.Message ?? e.Message);
            }
        }

        private void InsertOrUpdateUser(UserModel settingModel)
        {
            var user = _userService.GetUserById(settingModel.UserId);
            if (user != null)
            {
                var userProfile = _profileService.GetProfileByUserId(user.Id);
                string saltKey = _encryptionService.CreateSaltKey(20);
                user.SaltKey = saltKey;
                var password = _encryptionService.CreatePasswordHash(settingModel.OldPassword, saltKey);

                _userService.UpdateUser(user);
                userProfile.FirstName = settingModel.FirstName;
                userProfile.LastName = settingModel.LastName;
                _profileService.UpdateProfile(userProfile);
            }
        }


        /// <summary>
        /// Get Users by email
        /// </summary>
        /// <param name="email">string</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getUsersByEmail")]
        public List<UserProfileByEmailModel> GetUsersByEmail(string email = "")
        {
            var userModels = new List<UserProfileByEmailModel>();
            var users = _userService.GetUsersByEmail(email);
            foreach (var user in users)
            {
                var profile = _profileService.GetProfileByUserId(user.Id);
                userModels.Add(new UserProfileByEmailModel()
                {
                    UserId = user.Id,
                    FirstName = profile?.FirstName,
                    LastName = profile?.LastName,
                    RoleName = user.Roles?.FirstOrDefault()?.Name,
                    LastLoginDate = _settingService.GetTimeZoneDate(user.LoginDate),
                    IsActive = user.Active,
                    Email = user.Email
                });
            }
            return userModels;
        }

        [HttpGet]
        [Route("getUserById/{id}")]
        public IHttpActionResult GetUserById(long id)
        {
            var profile = _profileService.GetProfileByUserId(id);


            var user = _userService.GetUserById(id);
            string imageUrl = user.ProfilePicturePath;

            if (user == null)
            {
                ModelState.AddModelError("Error", $"no user was found for given id {id}");
                return HttpBadRequest($"no user was found for given id {id}");
            }


            Uri uri;

            if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out uri) && !string.IsNullOrEmpty(imageUrl))
                imageUrl = $"{_uploadFolderUrl}Avatars/{user.ProfilePicturePath}";

            var userModel = new EmployeeAdvancedModel()
            {
                UserId = user.Id,
                FirstName = profile?.FirstName,
                LastName = profile?.LastName,
                JobTitle = profile?.JobTitle,
                Email = user.Email,
                RoleId = user.Roles?.FirstOrDefault()?.Id,
                IsActive = user.Active,
                LogoPath = imageUrl
            };
            userModel.Campaigns = _usersExtensionService.GetCampaignOwnerships(user.Id);
            userModel.Affiliates = _usersExtensionService.GetAffiliateOwnerships(user.Id);
            userModel.AffiliateChannels = _usersExtensionService.GetAffiliateChannelOwnerships(user.Id);
            userModel.Buyers = _usersExtensionService.GetBuyerOwnerships(user.Id);
            userModel.BuyerChannels = _usersExtensionService.GetBuyerChannelOwnerships(user.Id);

            return Ok(userModel);
        }

        [HttpGet]
        [Route("getUsersBySearchPattern")]
        public IHttpActionResult GetUsersBySearchPattern(string inputValue)
        {
            var userModels = new List<UserProfileModel>();
            var users = _userService.GetAllUsers().ToList();
            foreach (var user in users)
            {
                var profile = _profileService.GetProfileByUserId(user.Id);
                var userModel = new UserProfileModel
                {
                    UserId = user.Id,
                    FirstName = profile?.FirstName,
                    LastName = profile?.LastName,
                    Email = user?.Email,
                    RoleName = user.Roles?.FirstOrDefault()?.Name,
                    LastLoginDate = _settingService.GetTimeZoneDate(user.LoginDate),
                    IsActive = user.Active,
                    ApprovalStatus = AffiliateInvitationStatuses.None
                };

                if (_searchService.CheckPropValue(userModel, inputValue))
                {
                    userModels.Add(userModel);
                }
            }
            return Ok(userModels);
        }

        
/* go to userService
        private bool ValidateCreateOppositeUser(UserTypes currentUserType, UserTypes addedUserType)
        {
            if ((currentUserType == UserTypes.Affiliate && addedUserType == UserTypes.Buyer) ||
                (currentUserType == UserTypes.Buyer && addedUserType == UserTypes.Affiliate))
            {
                return false;
            }

            return true;
        }

        private bool ValidateUserAccessSection(
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

        private void AddEntityOwnerships(List<long> campaigns,
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
                    _userService.InsertEntityOwnership(access);
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
                    _userService.InsertEntityOwnership(access);
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
                    _userService.InsertEntityOwnership(access);
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
                    _userService.InsertEntityOwnership(access);
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
                    _userService.InsertEntityOwnership(access);
                }
            }
        }
*/
        private string GeneratePassword(int Length, int NonAlphaNumericChars)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            string allowedNonAlphaNum = "!@#$%^&*()_-+=[{]};:<>|./?";
            Random rd = new Random();

            if (NonAlphaNumericChars > Length || Length <= 0 || NonAlphaNumericChars < 0)
                throw new ArgumentOutOfRangeException();

            char[] pass = new char[Length];
            int[] pos = new int[Length];
            int i = 0, j = 0, temp = 0;
            bool flag = false;

            //Random the position values of the pos array for the string Pass
            while (i < Length - 1)
            {
                j = 0;
                flag = false;
                temp = rd.Next(0, Length);
                for (j = 0; j < Length; j++)
                    if (temp == pos[j])
                    {
                        flag = true;
                        j = Length;
                    }

                if (!flag)
                {
                    pos[i] = temp;
                    i++;
                }
            }

            //Random the AlphaNumericChars
            for (i = 0; i < Length - NonAlphaNumericChars; i++)
                pass[i] = allowedChars[rd.Next(0, allowedChars.Length)];

            //Random the NonAlphaNumericChars
            for (i = Length - NonAlphaNumericChars; i < Length; i++)
                pass[i] = allowedNonAlphaNum[rd.Next(0, allowedNonAlphaNum.Length)];

            //Set the sorted array values by the pos array for the rigth posistion
            char[] sorted = new char[Length];
            for (i = 0; i < Length; i++)
                sorted[i] = pass[pos[i]];

            string Pass = new String(sorted);

            return Pass;
        }

    }
}
