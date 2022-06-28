// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="DocumentController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Content;
using Adrack.Data;
using Adrack.Service.Content;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Web.Framework.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Class DocumentController.
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public class DocumentController : BaseContentManagementController
    {
        #region Fields

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IDocumentService _documentService;

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// The affiliate service
        /// </summary>
        private readonly IAffiliateService _affiliateService;

        /// <summary>
        /// The history service
        /// </summary>
        private readonly IHistoryService _historyService;

        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentController"/> class.
        /// </summary>
        /// <param name="appContext">The application context.</param>
        /// <param name="affiliateService">The affiliate service.</param>
        /// <param name="documentService">The document service.</param>
        /// <param name="historyService">The history service.</param>
        /// <param name="userService">The user service.</param>
        public DocumentController(
                                    IAppContext appContext,
                                    IAffiliateService affiliateService,
                                    IDocumentService documentService,
                                    IHistoryService historyService,
                                    IUserService userService)
        {
            this._affiliateService = affiliateService;
            this._appContext = appContext;
            this._historyService = historyService;
            this._documentService = documentService;
            this._userService = userService;
        }

        #endregion Constructor

        // GET: Document
        /// <summary>
        /// Indexes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Index(long id)
        {
            ViewBag.AffId = id;
            return PartialView();
        }

        /// <summary>
        /// Adds the document.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult AddDocument()
        {
            int retVal = 0;

            if (Request["affiliateid"] == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            long affId = long.Parse(Request["affiliateid"]);

            if (Request.Files.Count > 0)
            {
                Random rand = new Random();

                HttpPostedFileBase file = Request.Files[0];

                var fileName = affId + "-" + rand.Next(0, 1000).ToString() + "-" + Path.GetFileName(file.FileName);

                var path = Path.Combine(Server.MapPath("~/Documents"), fileName);
                file.SaveAs(path);

                Document doc = new Document();
                doc.Name = fileName;
                doc.Type = Path.GetExtension(file.FileName);
                doc.AffiliateId = affId;
                doc.Created = DateTime.Now;
                doc.Path = "\\Documents\\" + fileName;
                doc.UserId = _appContext.AppUser.Id;

                _documentService.AddDocument(doc);
            }

            return Json(retVal, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult DeleteDocument()
        {
            if (Request["id"] == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            _documentService.DeleteDocument(long.Parse(Request["id"]));
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the documents.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetDocuments()
        {
            if (Request["affiliateid"] == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = 3;
            jd.recordsFiltered = 3;

            IList<Document> docList = _documentService.GetDocumentsByAffiliateId(long.Parse(Request["affiliateid"]));

            string iconka = "";
            foreach (Document doc in docList)
            {
                switch (doc.Type)
                {
                    case ".jpg": { iconka = "<img src='" + doc.Path + "' width='50px' />"; break; }
                    case ".jpeg": { iconka = "<img src='" + doc.Path + "' width='50px' />"; break; }
                    case ".png": { iconka = "<img src='" + doc.Path + "' width='50px' />"; break; }
                    case ".pdf": { iconka = "<i style='font-size:30px' class='icon-file-pdf'></i>"; break; }
                    case ".doc": { iconka = "<i style='font-size:30px' class='icon-file-word'></i>"; break; }
                    case ".docx": { iconka = "<i style='font-size:30px' class='icon-file-word'></i>"; break; }
                    case ".xls": { iconka = "<i style='font-size:30px' class='icon-file-excel'></i>"; break; }
                    case ".xlsx": { iconka = "<i style='font-size:30px' class='icon-file-excel'></i>"; break; }
                    default: { iconka = "<i style='font-size:30px' class='icon-libreoffice'></i>"; break; }
                }

                string[] names1 = {
                                      doc.Id.ToString(),
                                      "<a download href='" + doc.Path + "'>" + iconka + "</a>" ,
                                      "<h6 class=\"no-margin text-bold\"><a download href='" + doc.Path + "'>" + doc.Name + "</a></h6>",
                                      doc.UserId.ToString() + " / " + this._userService.GetUserById(doc.UserId).GetFullName(),
                                      doc.Created.ToString(),
                                      "<span class='delete_doc' id='del" + doc.Id.ToString() + "'><i class='icon-cross2 text-danger'></i></span>"
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }
    }
}