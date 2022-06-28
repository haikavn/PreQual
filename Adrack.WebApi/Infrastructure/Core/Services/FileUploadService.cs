using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Adrack.Core;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.WebApi.Infrastructure.Core.DTOs;
using Adrack.WebApi.Infrastructure.Core.Interfaces;

namespace Adrack.WebApi.Infrastructure.Core.Services
{
    public class FileUploadService : IFileUploadService
    {
        #region fields

        private const int RecordsFilteredCount = 5;

        private readonly IAppContext _appContext;

        #endregion

        #region constructor

        public FileUploadService(IAppContext appContext)
        {
            _appContext = appContext;
        }

        #endregion

        #region methods

        #endregion methods

        #region IFileUploadService interface implementation

        string IFileUploadService.UploadFile(long id)
        {
            return string.Empty;
        }

        //string IFileUploadService.UploadFile(long id)
        //{
        //    string filePath = string.Empty;
        //    if (Request.Files.Count > 0)
        //    {
        //        Random rand = new Random();

        //        System.Web.HttpPostedFileBase file = Request.Files[0];
        //        var fileName = id + "-" + rand.Next(0, 1000).ToString() + "-" +
        //                       System.IO.Path.GetFileName(file.FileName);

        //        var path = System.IO.Path.Combine(HostingEnvironment.MapPath("~/Uploads"), fileName);
        //        file.SaveAs(path);

        //        filePath = fileName;
        //    }

        //    return filePath;
        //}

        bool IFileUploadService.UploadLogoFile(string fileName, long id, out string errMessage)
        {
            errMessage = string.Empty;
            return true;
        }
        //string IFileUploadService.UploadLogoFile()
        //{
        //    if (Request.Files["CompanyLogo"] != null)
        //    {
        //        string fileName = Request.Files["CompanyLogo"].FileName;
        //        if (!string.IsNullOrEmpty(fileName))
        //        {
        //            string ext = System.IO.Path.GetExtension(fileName);
        //            string targetFolder = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/");
        //            string targetFileName = $"logo-{_appContext.AppUser.Id.ToString()}{ext}";
        //            string targetTempFileName = $"logo-tmp-{_appContext.AppUser.Id.ToString()}{ext}";

        //            string targetPath = System.IO.Path.Combine(targetFolder, targetFileName);
        //            string targetPathTmp = System.IO.Path.Combine(targetFolder, targetTempFileName);
        //            Request.Files["CompanyLogo"].SaveAs(targetPathTmp);
        //            /*******************/
        //            if (!Request.Content.IsMimeMultipartContent())
        //                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

        //            string root = System.Web.HttpContext.Current.Server.MapPath("~/Uploads");
        //            var provider = new MultipartFormDataStreamProvider(root);

        //            fileName = provider.Contents[0].Headers.ContentDisposition.FileName.Trim('"');
        //            var fname = Request.Content.Headers.ContentDisposition.FileName;
        //            var ff = Request.Content.ReadAsStreamAsync().Result;
        //            var xx = WebRequestMethods.File.UploadFile(root);
        //            var k = Request.Content.ReadAsMultipartAsync(provider).Result;
        //            provider.FileData[0].LocalFileName

        //            /******************/
        //            System.Drawing.Image img = System.Drawing.Image.FromFile(targetPathTmp);
        //            if (img.Width > 200)
        //            {
        //                settingModel.ErrorMessage = "The maximum width is 200px";
        //            }
        //            else if (img.Height > 200)
        //            {
        //                settingModel.ErrorMessage = "The maximum height is 200px";
        //            }
        //            else
        //            {
        //                Request.Files["CompanyLogo"].SaveAs(targetPath);
        //                set = this._settingService.GetSetting("Settings.CompanyLogoPath");
        //                set.Value = $"/Uploads/{targetFileName}";
        //                this._settingService.UpdateSetting(set);
        //                settingModel.IsSaved = true;
        //            }
        //        }
        //    }

        //}

        void IFileUploadService.DeleteFile(BuyerChannel buyerChannel, HttpPostedFileBase file)
        {
            //HttpPostedFileBase file = (Request.Files.Count > 0 ? Request.Files[0] : null);

            //if (file != null && file.ContentLength > 0)
            //{
            //    _subIdWhiteListService.DeleteAllSubIdWhiteList(buyerChannel.Id);

            //    // extract only the filename
            //    var fileName = Path.GetFileName(file.FileName);
            //    string dir = Server.MapPath("~/App_Data/Uploads");
            //    if (!Directory.Exists(dir))
            //        Directory.CreateDirectory(dir);

            //    // store the file inside ~/App_Data/uploads folder
            //    var path = Path.Combine(dir, fileName);
            //    file.SaveAs(path);

            //    StreamReader sr = new StreamReader(path);
            //    string line = "";
            //    int lineNumber = 0;
            //    int index = -1;

            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        string[] row = line.Split(',');

            //        if (lineNumber == 0)
            //        {
            //            index = Array.IndexOf(row, "MAX_SUBID");
            //        }
            //        else if (index >= 0 && index < row.Length)
            //        {
            //            _subIdWhiteListService.InsertSubIdWhiteList(new SubIdWhiteList() { SubId = row[index], BuyerChannelId = buyerChannel.Id });
            //        }

            //        lineNumber++;
            //    }
            //    sr.Close();

            //    System.IO.File.Delete(path);
            //}
        }

        #endregion IFileUploadService interface implementation
    }
}