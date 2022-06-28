// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Helper.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Message;
using Adrack.Core.Infrastructure;
using Adrack.Service.Accounting;
using Adrack.Service.Configuration;
using Adrack.Service.Directory;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.Web.ContentManagement.Controllers;
using Adrack.Web.Framework.ViewEngines.Razor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Adrack.Web.ContentManagement
{
    /// <summary>
    /// Class Helper.
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// The tz setting
        /// </summary>
        protected static Setting tzSetting = null;

        /// <summary>
        /// Gets the time zone string.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetTimeZoneStr()
        {
            ISettingService settingService = AppEngineContext.Current.Resolve<ISettingService>();
            if (tzSetting == null)
                tzSetting = settingService.GetSetting("TimeZone");
            return settingService.GetTimeZoneDate(DateTime.UtcNow, null, tzSetting).ToShortDateString();
        }

        /// <summary>
        /// Gets the base URL.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>System.String.</returns>
        public static string GetBaseUrl(HttpRequestBase request)
        {
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/") appUrl += "/";

            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            return baseUrl;
        }

        /// <summary>
        /// Gets the unique key.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="ArgumentException">Length must be between 1 and " + guidResult.Length</exception>
        public static string GetUniqueKey(int length)
        {
            string guidResult = string.Empty;

            while (guidResult.Length < length)
            {
                // Get the GUID.
                guidResult += Guid.NewGuid().ToString().GetHashCode().ToString("x");
            }

            // Make sure length is valid.
            if (length <= 0 || length > guidResult.Length)
                throw new ArgumentException("Length must be between 1 and " + guidResult.Length);

            // Return the first length bytes.
            return guidResult.Substring(0, length);
        }

        /// <summary>
        /// Renders the view to string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewData">The view data.</param>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewPath">The view path.</param>
        /// <param name="model">The model.</param>
        /// <returns>System.String.</returns>
        public static string RenderViewToString<T>(ViewDataDictionary viewData, ControllerContext controllerContext, string viewPath, T model)
        {
            viewData.Model = model;
            using (var writer = new StringWriter())
            {
                var view = new WebFormView(controllerContext, viewPath);
                var vdd = new ViewDataDictionary<T>(model);
                var viewCxt = new ViewContext(controllerContext, view, vdd,
                                            new TempDataDictionary(), writer);
                viewCxt.View.Render(viewCxt, writer);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Creates the controller.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routeData">The route data.</param>
        /// <returns>T.</returns>
        public static T CreateController<T>(RouteData routeData = null)
                    where T : Controller, new()
        {
            T controller = new T();

            // Create an MVC Controller Context
            var wrapper = new HttpContextWrapper(System.Web.HttpContext.Current);

            if (routeData == null)
                routeData = new RouteData();

            if (!routeData.Values.ContainsKey("controller") && !routeData.Values.ContainsKey("Controller"))
                routeData.Values.Add("controller", controller.GetType().Name
                                                            .ToLower()
                                                            .Replace("controller", ""));

            controller.ControllerContext = new ControllerContext(wrapper, routeData, controller);
            return controller;
        }

        /// <summary>
        /// Posts the XML.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="xml">The XML.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="postingHeaders">The posting headers.</param>
        /// <returns>System.String.</returns>
        public static string PostXml(string url, string xml, int timeout, string contentType, Dictionary<string, string> postingHeaders)
        {
            try
            {
                var data = System.Text.Encoding.UTF8.GetBytes(xml);

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol =
                //SecurityProtocolType.Tls
                //| SecurityProtocolType.Tls11
                SecurityProtocolType.Tls12;
                //| SecurityProtocolType.Ssl3;

                //ServicePointManager.DefaultConnectionLimit = 500;

                var request = (HttpWebRequest)WebRequest.Create(url);

                request.Timeout = timeout == 0 ? 30000 : timeout;
                request.Method = "POST";
                request.ContentType = contentType;
                request.ContentLength = data.Length;

                foreach (string key in postingHeaders.Keys)
                {
                    request.Headers.Add(key, postingHeaders[key]);
                }

                //string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("Opploans:LeadsHello"));
                //request.Headers.Add("Authorization", "Basic " + svcCredentials);

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return responseString;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Gets the active email template.
        /// </summary>
        /// <param name="emailTemplateName">Name of the email template.</param>
        /// <returns>EmailTemplate.</returns>
        protected static EmailTemplate GetActiveEmailTemplate(string emailTemplateName)
        {
            IEmailTemplateService _emailTemplateService = AppEngineContext.Current.Resolve<IEmailTemplateService>();

            var emailTemplateService = _emailTemplateService.GetEmailTemplateByName(emailTemplateName);

            if (emailTemplateService == null)
                return null;

            var active = emailTemplateService.Active;

            if (!active)
                return null;

            return emailTemplateService;
        }

        /// <summary>
        /// Gets the email template SMTP account.
        /// </summary>
        /// <param name="emailTemplate">The email template.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <returns>SmtpAccount.</returns>
        protected static SmtpAccount GetEmailTemplateSmtpAccount(EmailTemplate emailTemplate, long languageId)
        {
            ISmtpAccountService _smtpAccountService = AppEngineContext.Current.Resolve<ISmtpAccountService>();

            var smtpAccountId = emailTemplate.GetLocalized(x => x.SmtpAccountId, languageId);

            var smtpAccount = _smtpAccountService.GetSmtpAccountById(smtpAccountId);

            if (smtpAccount == null)
                smtpAccount = _smtpAccountService.GetAllSmtpAccounts().FirstOrDefault();

            return smtpAccount;
        }

        /// <summary>
        /// Builds the email queue.
        /// </summary>
        /// <param name="emailTemplate">The email template.</param>
        /// <param name="smtpAccount">The SMTP account.</param>
        /// <param name="emailToken">The email token.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="replyToName">Name of the reply to.</param>
        /// <param name="attachmentName">Name of the attachment.</param>
        /// <param name="attachmentPath">The attachment path.</param>
        /// <returns>System.Int64.</returns>
        protected static long BuildEmailQueue(EmailTemplate emailTemplate, SmtpAccount smtpAccount, IEnumerable<EmailToken> emailToken, long languageId, string recipient, string recipientName, string replyTo = null, string replyToName = null, string attachmentName = null, string attachmentPath = null)
        {
            IEmailTokenizer _emailTokenizer = AppEngineContext.Current.Resolve<IEmailTokenizer>();
            IEmailQueueService _emailQueueService = AppEngineContext.Current.Resolve<IEmailQueueService>();

            // Get Localized Template
            var bcc = emailTemplate.GetLocalized(x => x.Bcc, languageId);
            var subject = emailTemplate.GetLocalized(x => x.Subject, languageId);
            var body = emailTemplate.GetLocalized(x => x.Body, languageId);

            var subjectReplaced = _emailTokenizer.Replace(subject, emailToken, false);
            var bodyReplaced = _emailTokenizer.Replace(body, emailToken, true);

            var emailQueue = new EmailQueue
            {
                SmtpAccountId = smtpAccount.Id,
                AttachmentId = emailTemplate.AttachmentId,
                Sender = smtpAccount.Email,
                SenderName = smtpAccount.DisplayName,
                Recipient = recipient,
                RecipientName = recipientName,
                ReplyTo = replyTo,
                ReplyToName = replyToName,
                Cc = string.Empty,
                Bcc = bcc,
                Subject = subjectReplaced,
                Body = bodyReplaced,
                AttachmentName = attachmentName,
                AttachmentPath = attachmentPath,
                Priority = 5,
                DeliveryAttempts = 0,
                SentOn = null,
                CreatedOn = DateTime.UtcNow
            };

            _emailQueueService.InsertEmailQueue(emailQueue);

            return emailQueue.Id;
        }

        /// <summary>
        /// Sends the lead information.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="leadEmailFields">The lead email fields.</param>
        /// <param name="lead">The lead.</param>
        /// <returns>System.Int64.</returns>
        public static long SendLeadInfo(string email, string leadEmailFields, LeadMain lead)
        {
            long languageId = 1;

            MvcHandler.DisableMvcResponseHeader = true;
            AppEngineContext.Initialize(false);
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new WebAppRazorViewEngine());
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            var wrapper = new HttpContextWrapper(System.Web.HttpContext.Current);

            LeadController lController = new LeadController(
                AppEngineContext.Current.Resolve<ILeadMainService>(),
                AppEngineContext.Current.Resolve<IAccountingService>(),
                AppEngineContext.Current.Resolve<ILeadMainResponseService>(),
                AppEngineContext.Current.Resolve<IAffiliateResponseService>(),
                AppEngineContext.Current.Resolve<ISettingService>(),
                AppEngineContext.Current.Resolve<ICampaignService>(),
                AppEngineContext.Current.Resolve<IAffiliateService>(),
                AppEngineContext.Current.Resolve<IAffiliateChannelService>(),
                AppEngineContext.Current.Resolve<IBuyerService>(),
                AppEngineContext.Current.Resolve<IBuyerChannelService>(),
                AppEngineContext.Current.Resolve<ILeadContentDublicateService>(),
                AppEngineContext.Current.Resolve<IAppContext>(),
                AppEngineContext.Current.Resolve<IRedirectUrlService>(),
                AppEngineContext.Current.Resolve<INoteTitleService>(),
                AppEngineContext.Current.Resolve<IUserService>(),
                AppEngineContext.Current.Resolve<IEncryptionService>(),
                AppEngineContext.Current.Resolve<ICampaignTemplateService>(),
                AppEngineContext.Current.Resolve<ILeadSensitiveDataService>(),
                AppEngineContext.Current.Resolve<IBlackListService>(),
                AppEngineContext.Current.Resolve<IStateProvinceService>(),
                AppEngineContext.Current.Resolve<HttpContextBase>()
                );

            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Lead");

            lController.ControllerContext = new ControllerContext(wrapper, routeData, lController);

            IEmailService emailService = AppEngineContext.Current.Resolve<IEmailService>();

            EmailTemplate emailTemplate = new EmailTemplate();
            emailTemplate.Active = true;
            emailTemplate.Body = lController.GetEmailItem(lead, leadEmailFields);
            emailTemplate.SmtpAccountId = 1;
            emailTemplate.Subject = "Lead info #" + lead.Id.ToString();

            var smtpAccount = GetEmailTemplateSmtpAccount(emailTemplate, languageId);

            var emailToken = new List<EmailToken>();

            IEmailTokenDictionary _emailTokenDictionary = AppEngineContext.Current.Resolve<IEmailTokenDictionary>();

            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);

            return BuildEmailQueue(emailTemplate, smtpAccount, emailToken, languageId, email, "");
        }

        /// <summary>
        /// Encrypts the specified clear text.
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <returns>System.String.</returns>
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "abc123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>System.String.</returns>
        public static string Decrypt(string cipherText)
        {
            try
            {
                string EncryptionKey = "abc123";
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch { }
            return cipherText;
        }

        /// <summary>
        /// Gets the sub ids.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static List<string> GetSubIds(string filename)
        {
            List<string> list = new List<string>();
            using (var reader = new StreamReader(filename))
            {
                var line = reader.ReadLine();
                string[] values = line.Split(';');

                if (values.Length == 0) return list;
                if (values[0].ToLower() != "subid") return list;

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    values = line.Split(';');
                    if (values.Length == 0) continue;
                    list.Add(values[0]);
                }
            }
            return list;
        }
    }
}