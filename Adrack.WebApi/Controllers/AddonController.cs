using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Data;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.Web.Framework.Security;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models.Addon;
using Adrack.WebApi.Models.AffiliateChannel;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Campaigns;
using Adrack.WebApi.Models.Common;
using Adrack.WebApi.Models.Lead;
using Adrack.WebApi.Models.Membership;
using Adrack.WebApi.Models.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Adrack.Service.Message;
using Adrack.Service.Content;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/addon")]
    public class AddonController : BaseApiController
    {
        private readonly IAddonService _addonService;
        private readonly IUserService _userService;
        private readonly ICacheManager _cacheManager;

        private readonly IPermissionService _permissionService;
        private readonly IProfileService _profileService;
        private readonly IAppContext _appContext;
        private readonly IPaymentService _paymentService;
        //private static string _viewAddonGeneralInfoKey { get; set; } = "view-general-info-addon";
        //private static string _viewUserAddonsInfoKey { get; set; } = "view-info-user-addons";
        private readonly IEmailService _emailService;
        

        public AddonController(IAddonService addonService,
            IUserService userService,
            ICacheManager cacheManager,
            IPermissionService permissionService,
            IAppContext appContext,
            IProfileService profileService,
            IPaymentService paymentService,
            IEmailService emailService)
        {
            _addonService = addonService;
            _userService = userService;
            _paymentService = paymentService;
            _profileService = profileService;
            _cacheManager = cacheManager;
            _permissionService = permissionService;
            _appContext = appContext;
            _emailService = emailService;
        }


        #region Methods

        /// <summary>
        /// Get Addon List.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("getAddonList/{status}")]
        public IHttpActionResult GetAddonList(short status = -1)
        {
            //if (!_permissionService.Authorize(_viewAddonGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            var addons = _addonService.GetAllAddons();
            var userAddons = new List<Models.BaseModels.UserAddonsModel>();

            foreach (var addon in addons)
            {
                var userAddon = _addonService.GetAddonsByUserId(_appContext.AppUser.Id).Where(x => x.AddonId == addon.Id).FirstOrDefault();

                if (userAddon != null && userAddon.Status.HasValue && userAddon.Status.Value != status && status != -1)
                {
                    continue;
                }

                userAddons.Add(new Models.BaseModels.UserAddonsModel()
                {
                    AddData = addon.AddData,
                    Date = userAddon != null ? userAddon.Date : DateTime.UtcNow,
                    Key = addon.Key,
                    Name = addon.Name,
                    Id = addon.Id,
                    //Amount = userAddon != null && userAddon.Amount.HasValue ? userAddon.Amount.Value : (addon.Amount.HasValue ? addon.Amount.Value : 0),
                    Amount = addon.Amount.HasValue ? addon.Amount.Value : 0,
                    Status = userAddon != null && userAddon.Status.HasValue ? userAddon.Status.Value : (short)0
                });
            }

            return Ok(userAddons);
        }


        [HttpGet]
        [Route("getAddonsByUserId")]
        public IHttpActionResult GetAddonsByUserId(long userId)
        {

            var addons = _addonService.GetActivatedAddonsByUserId(userId);
            return Ok(addons);
        }

        [HttpPost]
        [Route("addAddon")]
        public IHttpActionResult AddAddon([FromBody] Addon addon)
        {
            //if (!_permissionService.Authorize(_viewAddonGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            if (addon == null)
            {
                return HttpBadRequest("Addon model is empty");
            }

            try
            {
                var addonId = _addonService.InsertAddon(addon);
                return Ok(addonId);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpPost]
        [Route("updateAddon")]
        public IHttpActionResult UpdateAddon([FromBody] Addon addon)
        {
            //if (!_permissionService.Authorize(_viewAddonGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            if (addon == null)
                return HttpBadRequest("Addon model is empty");

            var currentAddon = _addonService.GetAddonById(addon.Id);
            if (currentAddon == null)
                return HttpBadRequest("Addon not found");

            try
            {
                currentAddon.Name = addon.Name ?? currentAddon.Name;
                currentAddon.Key = addon.Key ?? currentAddon.Key; 
                currentAddon.AddData = addon.AddData ?? currentAddon.AddData; 
                currentAddon.Amount = addon.Amount; 
                currentAddon.Description = addon.Description; 

                _addonService.UpdateAddon(currentAddon);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("deleteAddon")]
        public IHttpActionResult DeleteAddon([FromBody] Addon addon)
        {
            //if (!_permissionService.Authorize(_viewAddonGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            if (addon == null)
                return HttpBadRequest("Addon model is empty");

            var currentAddon = _addonService.GetAddonById(addon.Id);
            if (currentAddon == null)
                return HttpBadRequest("Addon not found");

            try
            {
                _addonService.DeleteAddon(currentAddon);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpPost]
        [Route("deactivateAddon")]
        public IHttpActionResult DeactivateAddon([FromBody] AddonActivationModel addon)
        {
            //if (!_permissionService.Authorize(_viewAddonGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            if (addon == null)
                return HttpBadRequest("Addon model is empty");

            var currentAddon = _addonService.GetAddonById(addon.AddonId);
            if (currentAddon == null)
                return HttpBadRequest("Addon not found");

            try
            {
                currentAddon.Deactivate = 1;

                _addonService.UpdateAddon(currentAddon);

                var permissionIds =_addonService.GetPermissionIdsByAddonId(currentAddon.Id);

                foreach(var permissionId in permissionIds)
                {
                    var rolePermissions = _permissionService.GetRolePermissionsByPermissionId(permissionId);
                    foreach(var rolePermission in rolePermissions)
                    {
                        _permissionService.UpdateRolePermission(rolePermission.RoleId, rolePermission.PermissionId, 0);
                    }
                }

                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpPost]
        [Route("activateAddon")]
        public IHttpActionResult ActivateAddon([FromBody] AddonActivationModel addon)
        {
            //if (!_permissionService.Authorize(_viewAddonGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            if (addon == null)
                return HttpBadRequest("Addon model is empty");

            var currentAddon = _addonService.GetAddonById(addon.AddonId);
            if (currentAddon == null)
                return HttpBadRequest("Addon not found");

            try
            {
                currentAddon.Deactivate = 0;
                /*
                var bb = new BillingController(_appContext, _profileService, _addonService, _paymentService);
                bb.DoPaymentAddon(currentAddon.Key, addon.AddonId, Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority);
                */

                _addonService.UpdateAddon(currentAddon);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }
        #endregion

        #region UserAddons Methods
        /// <summary>
        /// Get User Addons List.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("getAllUserAddons")]
        public IHttpActionResult GetAllUserAddons()
        {
            //if (!_permissionService.Authorize(_viewUserAddonsInfoKey))
            //    return HttpBadRequest("access-denied");

            List<UserAddonsResultModel> result = new List<UserAddonsResultModel>();  
            
            var userAddons = _addonService.GetAllUserAddons();
            if (userAddons != null)
            {
                foreach (var userAddon in userAddons)
                {
                    var item = new UserAddonsResultModel
                    {
                        AddonId = userAddon.AddonId,
                        UserId = userAddon.UserId,
                        Date = userAddon.Date,
                        Status = userAddon.Status,
                        UserAddonAmount = userAddon.Amount,
                        Description = _addonService.GetAddonById(userAddon.AddonId)?.Description
                    };

                    result.Add(item);
                }
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("addUserAddon")]
        public IHttpActionResult AddUserAddon([FromBody] UserAddons userAddon)
        {
            //if (!_permissionService.Authorize(_viewAddonGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            if (userAddon == null)
            {
                return HttpBadRequest("UserAddons model is empty");
            }
            var user = _userService.GetUserById(userAddon.UserId);
            if (user == null)
            {
                return HttpBadRequest("This User not found.");
            }
            var addon = _addonService.GetAddonById(userAddon.AddonId);
            if (addon == null)
            {
                return HttpBadRequest("This Addon not found.");
            }

            var currentUserAddon = _addonService.GetUserAddon(userAddon.UserId, userAddon.AddonId);
            if (currentUserAddon != null)
            {
                return HttpBadRequest("This User's Addon already exists");
            }

            try
            {
                var id = _addonService.InsertUserAddon(userAddon);
                return Ok(id);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpPost]
        [Route("updateUserAddon")]
        public IHttpActionResult UpdateUserAddon([FromBody] UserAddons userAddon)
        {
            if (userAddon == null)
                return HttpBadRequest("UserAddons model is empty");

            var currentUserAddon = _addonService.GetUserAddon(userAddon.UserId, userAddon.AddonId);
            if (currentUserAddon == null)
                return HttpBadRequest("User's addon not found");

            try
            {
                currentUserAddon.Amount = userAddon.Amount;
                currentUserAddon.Status = userAddon.Status;

                _addonService.UpdateUserAddon(currentUserAddon);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("activateUserAddon")]
        public IHttpActionResult ActivateUserAddon([FromBody] UserAddonActivationModel userAddonActivationModel)
        {
            var user = _userService.GetUserById(userAddonActivationModel.UserId);
            if (user == null)
                return HttpBadRequest("This User not found.");

            var addon = _addonService.GetAddonById(userAddonActivationModel.AddonId);
            if (addon == null)
                return HttpBadRequest("Addon not found");

            var currentUserAddon = _addonService.GetUserAddon(userAddonActivationModel.UserId, userAddonActivationModel.AddonId);
            /*
            if (currentUserAddon == null)
            {
                if (userAddonActivationModel.Status == 0)
                {
                    
                    UserAddons userAddon = new UserAddons()
                    {
                        AddonId = userAddonActivationModel.AddonId,
                        UserId = userAddonActivationModel.UserId,
                        IsTrial = userAddonActivationModel.IsTrial,
                        Amount = addon.Amount.HasValue ? addon.Amount.Value : 0,
                        Date = DateTime.UtcNow,
                        Status = userAddonActivationModel.Status
                    };
                    
                    if (userAddon != null)
                    {
                        _addonService.InsertUserAddon(userAddon);
                        _emailService.SendAddonChangeStatusMessage(user.Email, _appContext.AppLanguage.Id, "Deactivate");

                        return Ok();
                    }
                    else
                    {
                        return HttpBadRequest("Addon payment Error");
                    }
                    
                }
                return HttpBadRequest("User's addon not found");
            }
            */

            try
            {
                // currentUserAddon.Status = userAddonActivationModel.Status;

                if (currentUserAddon != null)
                {
                    _addonService.DeleteUserAddon(currentUserAddon);
                    _emailService.SendAddonChangeStatusMessage(user.Email, _appContext.AppLanguage.Id, (currentUserAddon.Status == 1) ? "Activate" : "Deactivate ");
                    return Ok();
                }
                else
                {
                    return HttpBadRequest("User Addon not found");
                }
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        private bool PayAddon(UserAddons userAddon)
        {
            bool retVal = true;


            return retVal;
        }

        [HttpPost]
        [Route("deleteUserAddon")]
        public IHttpActionResult DeleteUserAddon([FromBody] UserAddons userAddon)
        {
            //if (!_permissionService.Authorize(_viewAddonGeneralInfoKey))
             //   return HttpBadRequest("access-denied");

            if (userAddon == null)
            {
                return HttpBadRequest("UserAddons model is empty");
            }

            var currentUserAddon = _addonService.GetUserAddon(userAddon.UserId, userAddon.AddonId);
            if (currentUserAddon == null)
            {
                return HttpBadRequest("This User's Addon not found.");
            }

            try
            {
                _addonService.DeleteUserAddon(currentUserAddon);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        #endregion


        #region PermissionAddons Methods
        /// <summary>
        /// Get Permission Addons List.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("getAllPermissionAddons")]
        public IHttpActionResult GetAllPermissionAddons()
        {
            var permissionAddons = _addonService.GetAllPermissionAddons();

            return Ok(permissionAddons);
        }

        [HttpPost]
        [Route("addPermissionAddon")]
        public IHttpActionResult AddPermissionAddon([FromBody] PermissionAddon permissionAddon)
        {
            //if (!_permissionService.Authorize(_viewAddonGeneralInfoKey))
            //    return HttpBadRequest("access-denied");

            if (permissionAddon == null)
            {
                return HttpBadRequest("UserAddon model is empty");
            }

            var permission = _permissionService.GetPermissionById(permissionAddon.PermissionId);
            if (permission == null)
            {
                return HttpBadRequest("This Permission not found.");
            }

            var addon = _addonService.GetAddonById(permissionAddon.AddonId);
            if (addon == null)
            {
                return HttpBadRequest("This Addon not found.");
            }

            var currentPermissionAddon = _addonService.GetPermissionAddon(permissionAddon.PermissionId, permissionAddon.AddonId);
            if (currentPermissionAddon != null)
            {
                return HttpBadRequest("This Permission's Addon already exists");
            }

            try
            {
                var id = _addonService.InsertPermissionAddon(permissionAddon);
                return Ok(id);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("deletePermissionAddon")]
        public IHttpActionResult DeletePermissionAddon([FromBody] PermissionAddon permissionAddon)
        {
            if (permissionAddon == null)
            {
                return HttpBadRequest("PermissionAddon model is empty");
            }

            var currentPermissionAddon = _addonService.GetPermissionAddon(permissionAddon.PermissionId, permissionAddon.AddonId);
            if (currentPermissionAddon == null)
            {
                return HttpBadRequest("This Permission's Addon not found.");
            }

            try
            {
                _addonService.DeletePermissionAddon(currentPermissionAddon);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }
        #endregion
    }
}