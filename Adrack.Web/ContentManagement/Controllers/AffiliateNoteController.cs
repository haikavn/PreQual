// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AffiliateNoteController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using Adrack.Data;
using Adrack.Service.Configuration;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Web.Framework.Security;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Class AffiliateNoteController.
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public partial class AffiliateNoteController : BaseContentManagementController
    {
        #region Fields

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IAffiliateNoteService _affiliateNoteService;

        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// The localized string service
        /// </summary>
        private readonly ILocalizedStringService _localizedStringService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Directory Controller
        /// </summary>
        /// <param name="affiliateNoteService">The affiliate note service.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="localizedStringService">Localized String Service</param>
        public AffiliateNoteController(IAffiliateNoteService affiliateNoteService, ISettingService settingService, ILocalizedStringService localizedStringService)
        {
            this._affiliateNoteService = affiliateNoteService;
            this._localizedStringService = localizedStringService;
            this._settingService = settingService;
        }

        #endregion Constructor

        // GET: Affiliate
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// Gets the affiliate notes.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetAffiliateNotes()
        {
            List<AffiliateNote> affiliateNotes = (List<AffiliateNote>)this._affiliateNoteService.GetAllAffiliateNotesByAffiliateId(0);

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = affiliateNotes.Count;
            jd.recordsFiltered = affiliateNotes.Count;
            foreach (AffiliateNote ai in affiliateNotes)
            {
                string[] names1 = {
                                      ai.Id.ToString(),
                                      ai.Created.ToShortDateString(),
                                      ai.Note
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }
    }
}