// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ProfileController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Membership;
using Adrack.Service.Common;
using Adrack.Service.Configuration;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Web.ContentManagement.Models.Membership;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Security;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Represents a Profile Controller
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public partial class ProfileController : BaseContentManagementController
    {
        #region Fields

        /// <summary>
        /// Profile Service
        /// </summary>
        private readonly IProfileService _profileService;

        /// <summary>
        /// Localized String Service
        /// </summary>
        private readonly ILocalizedStringService _localizedStringService;

        /// <summary>
        /// User Registration Service
        /// </summary>
        private readonly IUserRegistrationService _userRegistrationService;

        /// <summary>
        /// Application Context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// The buyer service
        /// </summary>
        private readonly IBuyerService _buyerService;

        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// The date time helper
        /// </summary>
        private readonly IDateTimeHelper _dateTimeHelper;

        #endregion Fields



        #region Constructor

        /// <summary>
        /// Profile Controller
        /// </summary>
        /// <param name="profileService">Profile Service</param>
        /// <param name="userRegistrationService">User Registration Service</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="appContext">Application Context</param>
        /// <param name="userService">The user service.</param>
        /// <param name="buyerService">The buyer service.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="dateTimeHelper">The date time helper.</param>
        public ProfileController(IProfileService profileService, IUserRegistrationService userRegistrationService, ILocalizedStringService localizedStringService, IAppContext appContext, IUserService userService, IBuyerService buyerService, ISettingService settingService, IDateTimeHelper dateTimeHelper)
        {
            this._profileService = profileService;
            this._localizedStringService = localizedStringService;
            this._userRegistrationService = userRegistrationService;
            this._appContext = appContext;
            this._userService = userService;
            this._buyerService = buyerService;
            this._settingService = settingService;
            this._dateTimeHelper = dateTimeHelper;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Prepares the model.
        /// </summary>
        /// <param name="profileModel">The profile model.</param>
        /// <param name="user">The user.</param>
        [NonAction]
        public void PrepareModel(ProfileModel profileModel, User user)
        {
            profileModel.TimeZones = _dateTimeHelper.GetSystemTimeZones(profileModel.TimeZone);

            profileModel.MenuTypes.Add(new SelectListItem() { Value = "1", Text = "Vertical" });
            profileModel.MenuTypes.Add(new SelectListItem() { Value = "0", Text = "Horizontal" });
        }

        /// <summary>
        /// Item
        /// </summary>
        /// <returns>Action Result</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Profile")]
        public ActionResult Item()
        {
            var user = _appContext.AppUser;

            if (user == null)
            {
                return Redirect("/");
            }

            var profile = _profileService.GetProfileByUserId(user.Id);

            if (profile == null)
            {
                return Redirect("/");
            }

            ProfileModel profileModel = new ProfileModel();

            profileModel.Id = profile.Id;
            profileModel.UserId = user.Id;
            profileModel.UserType = user.UserType;
            profileModel.ParentId = user.ParentId;
            profileModel.FirstName = profile != null ? profile.FirstName : "";
            profileModel.LastName = profile != null ? profile.LastName : "";
            if (user.MenuType != null)
            {
                profileModel.MenuType = (user.MenuType.HasValue ? (short)0 : user.MenuType.Value);
            }
            else
            {
                profileModel.MenuType = 0;
            }

            Setting timeZoneSetting = this._settingService.GetSetting("TimeZone");

            profileModel.TimeZone = !string.IsNullOrEmpty(user.TimeZone) ? user.TimeZone : (timeZoneSetting != null ? timeZoneSetting.Value : "");

            //Core.Domain.Membership.User u = this._userService.GetUserById(this._appContext.AppUser.Id);
            profileModel.MenuType = user.MenuType;

            Setting profileImageSetting = this._settingService.GetSetting("Settings.ProfileImagePath-" + _appContext.AppUser.Id.ToString());

            profileModel.ProfileImagePath = profileImageSetting != null ? profileImageSetting.Value : "";

            PrepareModel(profileModel, user);

            return View(profileModel);
        }

        public Image Resize(Image image, int maxWidth = 0, int maxHeight = 0)
        {
            if (maxWidth == 0)
                maxWidth = image.Width;
            if (maxHeight == 0)
                maxHeight = image.Height;

            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

        public System.Drawing.Image FixedSize(Image image, int Width, int Height, bool needToFill)
        {
            #region calculations
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;
            int sourceX = 0;
            int sourceY = 0;
            double destX = 0;
            double destY = 0;

            double nScale = 0;
            double nScaleW = 0;
            double nScaleH = 0;

            nScaleW = ((double)Width / (double)sourceWidth);
            nScaleH = ((double)Height / (double)sourceHeight);
            if (!needToFill)
            {
                nScale = Math.Min(nScaleH, nScaleW);
            }
            else
            {
                nScale = Math.Max(nScaleH, nScaleW);
                destY = (Height - sourceHeight * nScale) / 2;
                destX = (Width - sourceWidth * nScale) / 2;
            }

            if (nScale > 1)
                nScale = 1;

            int destWidth = (int)Math.Round(sourceWidth * nScale);
            int destHeight = (int)Math.Round(sourceHeight * nScale);
            #endregion

            System.Drawing.Bitmap bmPhoto = null;
            try
            {
                bmPhoto = new System.Drawing.Bitmap(destWidth + (int)Math.Round(2 * destX), destHeight + (int)Math.Round(2 * destY));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("destWidth:{0}, destX:{1}, destHeight:{2}, desxtY:{3}, Width:{4}, Height:{5}",
                    destWidth, destX, destHeight, destY, Width, Height), ex);
            }
            using (System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto))
            {
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.CompositingQuality = CompositingQuality.HighQuality;
                grPhoto.SmoothingMode = SmoothingMode.HighQuality;

                Rectangle to = new System.Drawing.Rectangle((int)Math.Round(destX), (int)Math.Round(destY), destWidth, destHeight);
                Rectangle from = new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);
                //Console.WriteLine("From: " + from.ToString());
                //Console.WriteLine("To: " + to.ToString());
                grPhoto.DrawImage(image, to, from, System.Drawing.GraphicsUnit.Pixel);

                return bmPhoto;
            }
        }

        /// <summary>
        /// Item
        /// </summary>
        /// <param name="profileModel">Profile Model</param>
        /// <returns>Action Result</returns>
        [HttpPost]
        [ContentManagementAntiForgery]
        public ActionResult Item(ProfileModel profileModel)
        {
            var user = _appContext.AppUser;

            profileModel.UserType = user.UserType;
            profileModel.ParentId = user.ParentId;

            if (Request["MenuType"] != null)
            {
                user.MenuType = short.Parse(Request["MenuType"]);
                this._userService.UpdateUser(user);
            }
            else
            if (Request["FirstName"] != null)
            {
                var profile = _profileService.GetProfileByUserId(profileModel.UserId);
                if (profile != null)
                {
                    profile.FirstName = Request["FirstName"];
                    profile.LastName = Request["LastName"];
                    this._profileService.UpdateProfile(profile);
                }

                if (Request.Files["ProfileImage"] != null)
                {
                    string fileName = Request.Files["ProfileImage"].FileName;
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        string ext = Path.GetExtension(fileName);
                        string targetFolder = Server.MapPath("~/Uploads/");
                        string imgFileName = "user-" + _appContext.AppUser.Id.ToString() + "-" + DateTime.UtcNow.Ticks.ToString() + ext;
                        string targetPath = Path.Combine(targetFolder, imgFileName);
                        string targetPathTmp = Path.Combine(targetFolder, "user-tmp-" + _appContext.AppUser.Id.ToString() + ext);
                        //Request.Files["ProfileImage"].SaveAs(targetPathTmp);

                        System.Drawing.Bitmap bmpPostedImage = new System.Drawing.Bitmap(Request.Files["ProfileImage"].InputStream);

                        Image resizedImage = FixedSize(bmpPostedImage, 50, 50, true);
                        resizedImage.Save(targetPath);

                        //Request.Files["ProfileImage"].SaveAs(targetPath);
                        Setting set = this._settingService.GetSetting("Settings.ProfileImagePath-" + _appContext.AppUser.Id.ToString());
                        if (set == null)
                        {
                            set = new Setting();
                            set.Key = "Settings.ProfileImagePath-" + _appContext.AppUser.Id.ToString();
                            set.Value = "/Uploads/" + imgFileName;
                            this._settingService.InsertSetting(set);
                        }
                        else
                        {
                            set.Value = "/Uploads/" + imgFileName;
                            this._settingService.UpdateSetting(set);
                        }
                    }
                }
            }
            else
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(profileModel.OldPassword))
                {
                    var changePasswordRequest = new ChangePasswordRequest(true, user.Email, profileModel.NewPassword, profileModel.OldPassword);

                    var changePasswordResult = _userRegistrationService.ChangePassword(changePasswordRequest);

                    if (changePasswordResult.Success)
                    {
                        profileModel.Result = _localizedStringService.GetLocalizedString("Membership.ChangePassword.Success");
                    }
                    else
                    {
                        profileModel.Result = "Please input a valid old password";
                    }

                    foreach (var error in changePasswordResult.Errors)
                        profileModel.Result = error;
                }
            }

            user.TimeZone = profileModel.TimeZone;
            this._userService.UpdateUser(user);

            PrepareModel(profileModel, user);

            return View(profileModel);
        }

        /// <summary>
        /// Dashboards the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Dashboard(long id = 0)
        {
            return RedirectToRoute("SingleBuyerDashboard", new[] { id });
        }

        #endregion Methods
    }
}