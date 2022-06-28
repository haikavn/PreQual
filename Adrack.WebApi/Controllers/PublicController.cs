using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using Adrack.Core;
using Adrack.Core.Helpers;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Message;
using Adrack.Core.Infrastructure.Data;
using Adrack.Service.Configuration;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.Service.Helpers;
using Adrack.WebApi.Extensions;
using Adrack.WebApi.Helpers;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Infrastructure.Web.Helpers;
using Adrack.WebApi.Models;
using Adrack.WebApi.Models.AffiliateChannel;
using Adrack.WebApi.Models.BuyerChannels;
using Adrack.WebApi.Models.Campaigns;
using Adrack.WebApi.Models.Lead;
using Adrack.WebApi.Models.Settings;
using Adrack.Web.Framework.Security;
using Adrack.Data;
// using System.Web;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/public")]
    public class PublicController : BaseApiPublicController
    {
        private readonly IAffiliateChannelService _affiliateChannelService;
        private readonly IAffiliateChannelTemplateService _affiliateChannelTemplateService;
        private readonly ICampaignService _campaignService;

        private readonly ICampaignTemplateService _campaignTemplateService;
        private readonly IAffiliateChannelFilterConditionService _affiliateChannelFilterConditionService;
        private readonly IAffiliateService _affiliateService;
        private readonly IBlackListService _blackListService;
        private readonly IBuyerChannelService _buyerChannelService;
        private readonly ISettingService _settingService;
        private readonly IEmailService _emailService;
        private readonly ISmtpAccountService _smtpAccountService;
        private readonly IAppContext _appContext;
        private readonly ISearchService _searchService;
        private readonly IProfileService _profileService;
        private readonly IRepository<AffiliateChannelTemplate> _affiliateChannelTemplateRepo;
        private readonly IRepository<AffiliateChannel> _affiliateChannelRepo;
        private readonly IRepository<CampaignField> _campaignTemplateRepo;
        private readonly IRepository<Campaign> _campaignRepo;
        private readonly IPermissionService _permissionService;

        private static string _viewPostingDetailsKey { get; set; } = "view-posting-details-affiliatechannel";
        private static string _viewNotesKey { get; set; } = "view-notes-affiliatechannel";
        private static string _editNotesKey { get; set; } = "edit-notes-affiliatechannel";
        private static string _viewAffiliateChannelKey { get; set; } = "view-general-information-affiliatechannel";
        private static string _editAffiliateChannelKey { get; set; } = "edit-general-information-affiliatechannel";
        private static string _viewBlackListKey { get; set; } = "view-black-list-affiliatechannel";
        private static string _editBlackListKey { get; set; } = "edit-black-list-affiliatechannel";
        private static string _viewFilterKey { get; set; } = "view-filter-affiliatechannel";
        private static string _editFilterKey { get; set; } = "edit-filter-affiliatechannel";
        private static string _viewAndEditBuyerChannelKey { get; set; } = "view-edit-buyer-channels-affiliatechannel";

        

        public PublicController(
            IAppContext appContext,
            IAffiliateChannelService affiliateChannelService,
            IAffiliateChannelTemplateService affiliateChannelTemplateService,
            ICampaignTemplateService campaignTemplateService,
            IAffiliateChannelFilterConditionService affiliateChannelFilterConditionService,
            IAffiliateService affiliateService,
            IBlackListService blackListService,
            ICampaignService campaignService,
            ISettingService settingService,
            IBuyerChannelService buyerChannelService,
            IEmailService emailService,
            ISmtpAccountService smtpAccountService,
            ISearchService searchService,
            IProfileService profileService,
            IRepository<AffiliateChannelTemplate> affiliateChannelTemplateRepo,
            IRepository<AffiliateChannel> affiliateChannelRepo,
            IRepository<CampaignField> campaignTemplateRepo,
            IRepository<Campaign> campaignRepo,
            IPermissionService permissionService)
        {
            _appContext = appContext;
            _affiliateChannelService = affiliateChannelService;
            _affiliateChannelTemplateService = affiliateChannelTemplateService;
            _campaignTemplateService = campaignTemplateService;
            _affiliateChannelFilterConditionService = affiliateChannelFilterConditionService;
            _affiliateService = affiliateService;
            _campaignService = campaignService;
            _blackListService = blackListService;
            _campaignService = campaignService;
            _settingService = settingService;
            _buyerChannelService = buyerChannelService;
            _emailService = emailService;
            _smtpAccountService = smtpAccountService;
            _searchService = searchService;
            _profileService = profileService;
            _affiliateChannelTemplateRepo = affiliateChannelTemplateRepo;
            _affiliateChannelRepo = affiliateChannelRepo;
            _campaignTemplateRepo = campaignTemplateRepo;
            _campaignRepo = campaignRepo;
            _permissionService = permissionService;
        }

        #region PrivateMethods
        private string GetPostingURL()
        {
            try
            {
                var postingUrl = string.Empty;
                postingUrl = _settingService.GetSetting("System.PostingUrl")?.Value;
                return postingUrl;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<CodeSamples> GetCodeSamples(string subFolder)
        {
            var result = new List<CodeSamples>();
            try
            {
                string[] files = Directory.GetFiles(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/StaticContent/PostingDetails/" + subFolder + "/"));
                if (files != null && files.Any())
                {
                    foreach (var file in files)
                    {
                        var content = File.ReadAllText(file);
                        var fileName = Path.GetFileNameWithoutExtension(file);
                        result.Add(new CodeSamples()
                        {
                            CodeSample = content,
                            TabName = fileName
                        });
                    }
                }
            }
            catch
            {
                result.Add(new CodeSamples() { CodeSample = "", TabName = "" });
                result.Add(new CodeSamples() { CodeSample = "", TabName = "" });
                result.Add(new CodeSamples() { CodeSample = "", TabName = "" });
                result.Add(new CodeSamples() { CodeSample = "", TabName = "" });
            }
            return result;
        }

        private string ComposeXMLTemplate(long affiliateChannelId, bool generateRandomValues)
        {
            var result = string.Empty;
            var affChannelTemplateLIst = _affiliateChannelTemplateRepo.Table.Where(item => item.AffiliateChannelId == affiliateChannelId).OrderBy(x => x.Id);

            var affiliateChannelTemplate = (from act in affChannelTemplateLIst
                                            join ct in _campaignTemplateRepo.Table on act.CampaignTemplateId equals ct.Id
                                            into join1

                                            join ac in _affiliateChannelRepo.Table on act.AffiliateChannelId equals ac.Id
                                            from j in join1.DefaultIfEmpty()
                                            select new XMLTree
                                            {
                                                OrderId = act.Id,
                                                TemplateField = act.TemplateField,
                                                SectionName = act.SectionName,
                                                Validator = j == null ? (short?)null : j.Validator,
                                                Id = act.CampaignTemplateId,
                                                AffiliateChannelKey = ac.ChannelKey,
                                                AffiliateChannelPassword = ac.ChannelPassword
                                            }
                                            ).OrderBy(res => res.OrderId).ToList();
            var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(affiliateChannelId);
            XElement xml = null;
            if (affiliateChannelTemplate != null && affiliateChannelTemplate.Any())
            {
                xml = BuildXMLHierarchy(affiliateChannelTemplate, generateRandomValues);
                if (xml != null)
                    result = FormatXml(xml.ToString());
            }

            return result;
        }
        private string CorrectXML(string readyXml)
        {
            for (; ; )
            {
                var index = readyXml.IndexOf("></");
                if (index != -1)
                {
                    readyXml = readyXml.Insert(index + 1, (Guid.NewGuid()).ToString().Substring(0, 6));
                    //var tagName = TakeTagName(readyXml, index);
                }
                else break;
            }
            return readyXml;
        }
        private string TakeTagName(string xmlText, int index)
        {
            var tagName = string.Empty;
            for (; ; )
            {
                if (xmlText[index] != '<')
                {
                    tagName += xmlText[index];
                }
                else if (xmlText[index] == '<')
                    break;
                index--;
            }
            var arr = tagName.Substring(1, tagName.Count() - 1).ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
        private string GenerateRandomText(string value)
        {
            var result = string.Empty;

            return result;
        }
        private XElement BuildXMLHierarchy(List<XMLTree> tree, bool generateRandomValues)
        {
            XElement root = null;
            foreach (var item in tree)
            {
                var value = string.Empty;
                if (!string.IsNullOrEmpty(item.TemplateField) && item.TemplateField.ToLower() == "password")
                    value = !string.IsNullOrWhiteSpace(item.AffiliateChannelPassword) ? item.AffiliateChannelPassword : string.Empty;
                else if (!string.IsNullOrEmpty(item.TemplateField) && item.TemplateField.ToLower() == "channelid")
                    value = !string.IsNullOrWhiteSpace(item.AffiliateChannelKey) ? item.AffiliateChannelKey : string.Empty;
                else if (item.Validator.HasValue && generateRandomValues)
                {
                    value = GenerateRandomValue((Validators)item.Validator.Value);
                }


                if (root != null)
                {
                    var attribute = root.Descendants().FirstOrDefault(x => x.Name == item.SectionName);

                    if (attribute != null)
                    {
                        attribute.Add(new XElement(item.TemplateField, item.Id > 0 ? value : " "));
                    }
                    else
                    {
                        root.Add(new XElement(item.SectionName, new XElement(item.TemplateField, item.Id > 0 ? value : " ")));
                    }
                }
                else if (!string.IsNullOrEmpty(item.TemplateField))
                {
                    root = (new XElement(item.SectionName, new XElement(item.TemplateField, item.Id > 0 ? "78" : " ")));
                }
            }

            return root;
        }
        private string GenerateRandomValue(Validators validator)
        {
            var result = string.Empty;
            var states = new string[] { "California", "Alabama", "Colorado", "Arizona", "Nevada", "Texas", "Georgia" };
            switch (validator)
            {
                case Validators.None:
                    result = (Guid.NewGuid()).ToString().Substring(0, 6);
                    break;
                case Validators.String:
                    result = (Guid.NewGuid()).ToString().Substring(0, 6);
                    break;
                case Validators.Number:
                    result = (new Random()).Next(5000, 9999).ToString();
                    break;
                case Validators.Email:
                    result = $"{(Guid.NewGuid()).ToString().Substring(0, 6)}@adrack.com";
                    break;
                case Validators.AccountNumber:
                    result = $"{(new Random()).Next(1000, 9999).ToString()} {(new Random()).Next(1000, 9999).ToString()} {(new Random()).Next(1000, 9999).ToString()} {(new Random()).Next(1000, 9999).ToString()}";
                    break;
                case Validators.Ssn:
                    result = $"{(new Random()).Next(1000, 9999).ToString()} {(new Random()).Next(1000, 9999).ToString()} {(new Random()).Next(1000, 9999).ToString()} {(new Random()).Next(1000, 9999).ToString()}";
                    break;
                case Validators.Zip:
                    result = (new Random()).Next(5000, 9999).ToString();
                    break;
                case Validators.Phone:
                    result = $"+1 626 {(new Random()).Next(100, 999).ToString()} {(new Random()).Next(10, 99).ToString()} {(new Random()).Next(10, 99).ToString()}";
                    break;
                case Validators.DateTime:
                    result = $"{(new Random()).Next(1, 31).ToString()}/09/2020 09:{(new Random()).Next(0, 59).ToString()}:{(new Random()).Next(0, 59).ToString()}";
                    break;
                case Validators.State:
                    result = states[(new Random()).Next(0, 6)];
                    break;
                case Validators.RoutingNumber:
                    result = (new Random()).Next(5000, 9999).ToString();
                    break;
                case Validators.DateOfBirth:
                    result = $"{(new Random()).Next(1, 31).ToString()}/09/{(new Random()).Next(1900, 1990).ToString()} 09:{(new Random()).Next(0, 59).ToString()}:{(new Random()).Next(0, 59).ToString()}";
                    break;
                case Validators.Decimal:
                    result = ((new Random()).Next(10, 31) * 1.6).ToString();
                    break;
                case Validators.SubId:
                    result = ((new Random()).Next(10, 999)).ToString();
                    break;
                default:
                    result = (Guid.NewGuid()).ToString().Substring(0, 6);
                    break;
            }
            return result;
        }
        private string FormatXml(string xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(xml);
                return doc.ToString();
            }
            catch (Exception)
            {
                // Handle and throw if fatal exception here; don't just ignore them
                return xml;
            }
        }
        private List<RequestFields> GetRequestFields(long affiliateChannelId)
        {
            var result = new List<RequestFields>();
            var affChannelTemplateLIst = _affiliateChannelTemplateRepo.Table.Where(item => item.AffiliateChannelId == affiliateChannelId).OrderBy(x => x.Id);

            var affiliateChannelTemplate = (from act in affChannelTemplateLIst
                                            join ct in _campaignTemplateRepo.Table on act.CampaignTemplateId equals ct.Id
                                            join ac in _affiliateChannelRepo.Table on act.AffiliateChannelId equals ac.Id
                                            select new
                                            {
                                                Description = ct.Description,
                                                Field = act.TemplateField,
                                                Format = (ac.DataFormat),
                                                Status = ac.Status,
                                                Type = (CampaignTypes)(ct.FieldType.HasValue ? ct.FieldType.Value : 0)
                                            });

            if (affiliateChannelTemplate != null && affiliateChannelTemplate.Any())
            {
                foreach (var item in affiliateChannelTemplate)
                {
                    result.Add(new RequestFields()
                    {
                        Description = item.Description,
                        Field = item.Field,
                        Format = Enum.GetName(typeof(DataFormat), (item.Format)),
                        Status = Enum.GetName(typeof(ActivityStatuses), item.Status),
                        Type = string.Join(" ", Regex.Split(Enum.GetName(typeof(CampaignTypes), (item.Type)), @"(?<!^)(?=[A-Z])"))
                    });
                }
            }
            return result.ToList();
        }

        #endregion


        [HttpGet]
        [Route("postSpecification")]
        public IHttpActionResult PostSpecification([FromUri] string key)
        {
            long id = 0;

            string IdStr = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(key));
            try
            {
                id = Int32.Parse(IdStr);
            }
            catch (Exception e)
            {
                return Ok(0);
            }

            var response = new AffiliateChannelPostingDetailsReturnModel();
            var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(id);
            if (affiliateChannel == null)
            {
                return HttpBadRequest($"Affiliate channel with {id} Id doesn't exist");
            }
            var postingUrl = GetPostingURL();
            response.Id = affiliateChannel.Id;
            response.Name = affiliateChannel.Name;
            response.Password = affiliateChannel.ChannelPassword;
            response.PostSpecificationUrl = postingUrl;//Clarify
            response.TechContact = _settingService.GetSetting("AppSetting.SupportEmail")?.Value ?? "no-reply@adrack.com";
            response.PostSpecificationUrl = $"{Request.GetBaseUrl()}Management/AffiliateChannel/PostSpecification?p=" +
                $"{Convert.ToBase64String(BitConverter.GetBytes(affiliateChannel.Id))}";
            response.RequestFormat = $"HTTPS POST";
            response.ResponseFormat = "XML 1.0";
            response.ResponseEncoding = "UTF-8";
            response.RequestService = $"{Request.GetBaseUrl()}Import/";

            response.CodeSamples = GetCodeSamples("CodeSamples");
            response.TestingExamples.ResponseExamples = GetCodeSamples("ResponseExamples");

            var xmlString = _affiliateChannelService.GetAffiliateChannelById(id).XmlTemplate;
            if (xmlString != null)
            {
                response.TestRequest.XML = CorrectXML(xmlString);
                response.TestingExamples.LeadImportXMLExample = xmlString;//ComposeXMLTemplate(id, true);
            }
            else
            {
                response.TestingExamples.LeadImportXMLExample = ComposeXMLTemplate(id, false);
                response.TestRequest.XML = ComposeXMLTemplate(id, true);
            }
            response.TestingExamples.RequestFields = GetRequestFields(id);

            return Ok(response);
        }

    }

}