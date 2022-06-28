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
using Adrack.Web.Framework.Cache;
using Adrack.PlanManagement;
using Adrack.Data;
using Adrack.Service.Content;
using Adrack.WebApi.Models.Clicks;
using Adrack.Service.Click;
using Adrack.Core.Domain.Click;
using Adrack.Core.Domain.Membership;
using System.Web;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Security;
// using System.Web;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/affiliatechannel")]
    public class AffiliateChannelController : BaseApiController
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
        private readonly IPlanService _planService;
        private readonly IUserService _userService;
        private readonly IClickService _clickService;

        /// <summary>
        /// The Entity Change History service
        /// </summary>
        private readonly IEntityChangeHistoryService _entityChangeHistoryService;

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

        

        public AffiliateChannelController(
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
            IPermissionService permissionService,
            IPlanService planService,
            IUserService userService,
            IEntityChangeHistoryService entityChangeHistoryService,
            IClickService clickService)
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
            _planService = planService;
            _userService = userService;
            _entityChangeHistoryService = entityChangeHistoryService;
            _clickService = clickService;
        }

        [HttpGet]
        [Route("getPostingDetails/{id}")]
        public IHttpActionResult GetPostingDetails([FromUri] long id)
        {
            if (!_permissionService.Authorize(_viewPostingDetailsKey))
            {
                return HttpBadRequest("access-denied");
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
            response.ChannelId = affiliateChannel.ChannelKey;
            response.Password = affiliateChannel.ChannelPassword;
            response.PostSpecificationUrl = postingUrl;//Clarify
            response.TechContact = _settingService.GetSetting("AppSetting.SupportEmail")?.Value ?? "no-reply@adrack.com";

            string requestUri = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/";
            string key = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(id.ToString()));
            response.PostSpecificationUrl = $"{requestUri}public/postspecification?key=" + key;
            
            response.RequestFormat = $"HTTPS POST";
            response.ResponseFormat = "XML 1.0";
            response.ResponseEncoding = "UTF-8";
            response.RequestService = $"{Request.GetBaseUrl()}Import/";

            response.CodeSamples = GetCodeSamples("CodeSamples");
            response.TestingExamples.ResponseExamples = GetCodeSamples("ResponseExamples");

            var curAffiliateChannel = _affiliateChannelService.GetAffiliateChannelById(id);
            var xmlString = curAffiliateChannel.XmlTemplate;
            if (xmlString != null)
            {
                response.TestRequest.XML = ComposeXMLTemplate(id, true);//CorrectXML(xmlString);
                response.TestingExamples.LeadImportXMLExample = ComposeXMLTemplate(id, false);//xmlString;//ComposeXMLTemplate(id, true);

                /*var contains = response.TestRequest.XML.ToUpperInvariant().Contains("<PASSWORD>");
                if (!contains)
                {
                    var strPsd = "<REFERRAL><PASSWORD>" + curAffiliateChannel.AffiliateChannelPassword + "</PASSWORD>";
                    response.TestRequest.XML =
                        response.TestRequest.XML.Replace("<REFERRAL>", strPsd);
                }

                contains = response.TestRequest.XML.ToUpperInvariant().Contains("<CHANNELID>");
                if (!contains)
                {
                    var strChannelId = "<REFERRAL><CHANNELID>" + curAffiliateChannel.AffiliateChannelKey + "</CHANNELID>";
                    response.TestRequest.XML =
                        response.TestRequest.XML.Replace("<REFERRAL>", strChannelId);
                }*/
            }
            else
            {
                response.TestingExamples.LeadImportXMLExample = ComposeXMLTemplate(id, false);
                response.TestRequest.XML = ComposeXMLTemplate(id, true);
            }
            response.TestingExamples.RequestFields = GetRequestFields(id);

            return Ok(response);
        }

        [HttpPost]
        [Route("sendPostingDetailsEmail")]
        public IHttpActionResult SendPostingDetailsEmail([FromBody] AffiliateChannelPostingEmailModel email)
        {
            if (!_permissionService.Authorize(_viewPostingDetailsKey))
            {
                return HttpBadRequest("access-denied");
            }

            var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(email.ChannelId);

            if (affiliateChannel == null)
            {
                return HttpBadRequest("affiliate channel not found");
            }

            long id = affiliateChannel.Id;

            string requestUri = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/";

            string key = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(id.ToString()));

            string postingSpecificationUrl = HttpContext.Current.Request.Headers["Referer"];
            postingSpecificationUrl = (!postingSpecificationUrl.EndsWith("/") ? postingSpecificationUrl + "/" : postingSpecificationUrl);
            postingSpecificationUrl = $"{postingSpecificationUrl}public/postspecification?key=" + key;

            // $"{System.Convert.ToBase64String(Encoding.UTF8.GetBytes(id.ToString()))}";

            var smtpSetting = this._smtpAccountService.GetSmtpAccount();
            var user = _appContext.AppUser;
            var body = new StringBuilder();
            body.AppendLine($"Post Specification URL: {postingSpecificationUrl}<br />{email.Comment}");

            var techContact = _settingService.GetSetting("AppSetting.SupportEmail")?.Value ?? "no-reply@adrack.com";


            _emailService.SendEmail(smtpSetting, techContact, "Adrack Support", email.Email, user.BuiltInName, "Issue", body.ToString());
            return Ok();
        }

        [HttpPost]
        [Route("sendCodeSampleEmail")]
        public IHttpActionResult SendPostingCodeSampleEmail(AffiliateChannelCodeSampleEmailModel email)
        {
            var smtpSetting = this._smtpAccountService.GetSmtpAccount();
            var techContact = _settingService.GetSetting("AppSetting.SupportEmail")?.Value ?? "no-reply@adrack.com";
            var user = _appContext.AppUser;
            var body = new StringBuilder();

            body.AppendLine($"Code Sample name: {email.CodeSampleName}<br />");
            body.AppendLine($"Code Sample: {email.CodeSample}<br />");
            body.AppendLine($"Email Address: {email.Email}<br />");
            body.AppendLine($"Comment: {email.Comment}<br />");
            _emailService.SendEmail(smtpSetting, techContact, "Adrack Support", email.Email, user.BuiltInName, "Issue", body.ToString());
            return Ok();
        }

        [HttpGet]
        [Route("getAffiliateChannelNotes/{affiliateChannelId}")]
        public IHttpActionResult GetAffiliateChannelNotesList(long affiliateChannelId)
        {
            if (!_permissionService.Authorize(_viewNotesKey))
            {
                return HttpBadRequest("access-denied");
            }
            var result = new List<AffiliateChannelNoteModel>();
            var affiliateNotes = (List<AffiliateChannelNote>)this._affiliateChannelService.GetAllAffiliateChannelNotesByAffiliateChannelId(affiliateChannelId);

            foreach (var item in affiliateNotes)
            {
                Profile profile = null;

                if (item.UserId.HasValue)
                    profile = _profileService.GetProfileByUserId(item.UserId.Value);


                result.Add(new AffiliateChannelNoteModel()
                {
                    NoteId = item.Id,
                    AffiliateChannelId = item.AffiliateChannelId,
                    Created = item.Created,
                    UpdatedDate = _settingService.GetTimeZoneDate(item.Updated.HasValue ? item.Updated.Value : DateTime.UtcNow),
                    Note = item.Note,
                    Title = item.Title,
                    FirstName = profile?.FirstName,
                    LastName = profile?.LastName
                });
            }
            result = result.OrderByDescending(item => item.UpdatedDate).ToList();
            
            return Ok(result);
        }

        [HttpPost]
        [Route("updateNote")]
        public IHttpActionResult UpdateAffiliateNote(AffiliateChannelNoteModel model)
        {
            if (!_permissionService.Authorize(_editNotesKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (model == null)
            {
                return HttpBadRequest($"Given affiliate channel note argument is null");
            }
            model.UpdatedDate = DateTime.UtcNow;
            if (model.NoteId != 0)
            {
                var note = this._affiliateChannelService.GetAffiliateChannelNoteById(model.NoteId);
                if (note == null)
                {
                    return HttpBadRequest($"Given affiliate channel note does not exist");
                }
                note.Note = model.Note;
                note.Updated = model.UpdatedDate;
                note.Title = model.Title;
                this._affiliateChannelService.UpdateAffiliateChannelNote(note);
                return Ok(note);
            }
            else
            {
                var newNote = new AffiliateChannelNote()
                {
                    Created = DateTime.UtcNow,
                    AffiliateChannelId = model.AffiliateChannelId,
                    Note = model.Note,
                    Updated = null,
                    Title=model.Title,
                    UserId = _appContext.AppUser.Id
                };
                newNote.Id = this._affiliateChannelService.InsertAffiliateChannelNote(newNote);
                if(newNote.Id!=0)
                {
                    var affChannel = _affiliateChannelService.GetAffiliateChannelById(newNote.AffiliateChannelId);
                    if(affChannel!=null)
                    {
                        var affiliate = _affiliateService.GetAffiliateById(affChannel.AffiliateId, false);
                        if (affiliate != null && affiliate.ManagerId.HasValue)
                        {
                            var manager = _userService.GetUserById(affiliate.ManagerId.Value);
                            var smtpSetting = this._smtpAccountService.GetSmtpAccount();
                            var user = _appContext.AppUser;
                            var body = new StringBuilder();

                            string emailContent = model.NoteId == 0 ? " has a new note " : " note has been updated ";

                            body.AppendLine($"Affiliate channel {affChannel.Name} {emailContent}: {newNote.Note}<br />");
                            _emailService.SendEmail(smtpSetting, "no-reply@adrack.com", "Adrack", manager.Email, manager.BuiltInName, "New note", body.ToString());
                        }
                    }
                }
                return Ok(newNote);
            }
        }

        [HttpDelete]
        [Route("deleteNote/{affiliateNoteId}")]
        public IHttpActionResult DeleteAffiliateNote(long affiliateNoteId)
        {
            if (!_permissionService.Authorize(_editNotesKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (affiliateNoteId == 0)
            {
                return HttpBadRequest($"Given affiliate channel note {affiliateNoteId} Id is null");
            }
            else
            {
                var note = this._affiliateChannelService.GetAffiliateChannelNoteById(affiliateNoteId);
                if (note == null)
                {
                    return HttpBadRequest($"Given affiliate channel note does not exist");
                }
                this._affiliateChannelService.DeleteAffiliateNote(note);
            }

            return Ok();
        }


        /// <summary>
        /// Clone AffiliateChannel
        /// </summary>
        /// <param name="inpObj">CloneAffiliateChannelInpModel</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Route("cloneAffiliateChannel")]
        public IHttpActionResult CloneAffiliateChannel([FromBody] CloneAffiliateChannelInpModel inpObj)
        {
            if (!_permissionService.Authorize(_viewAffiliateChannelKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var planLimitation = _planService.CheckPlanStatusesByUserId(_appContext.AppUser.Id);
                if (planLimitation != null && planLimitation.Contains(AdrackPlanVerificationStatus.AffiliateChannelLimitReached))
                {
                    return HttpBadRequest($"affiliate channel limit reached.");
                }

                var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(inpObj.CurrentAffiliateChannelId);

                if (affiliateChannel == null)
                {
                    return HttpBadRequest($"no affiliate channel was found for given id {inpObj.CurrentAffiliateChannelId}");

                }

                AffiliateChannel cloneAffiliateChannel = new AffiliateChannel()
                {
                    Id = 0,
                    Name = inpObj.NewAffiliateChannelName,
                    ChannelKey = affiliateChannel.ChannelKey,
                    ChannelPassword = affiliateChannel.ChannelPassword,
                    AffiliateId = affiliateChannel.AffiliateId,
                    AffiliatePrice = affiliateChannel.AffiliatePrice,
                    AffiliatePriceMethod = affiliateChannel.AffiliatePriceMethod,
                    //AttachedBuyerChannels = affiliateChannel.AttachedBuyerChannels,
                    CampaignId = affiliateChannel.CampaignId,
                    DataFormat = affiliateChannel.DataFormat,
                    IsDeleted = affiliateChannel.IsDeleted,
                    MinPriceOption = affiliateChannel.MinPriceOption,
                    NetworkMinimumRevenue = affiliateChannel.NetworkMinimumRevenue,
                    NetworkTargetRevenue = affiliateChannel.NetworkTargetRevenue,
                    Note = affiliateChannel.Note,
                    Status = affiliateChannel.Status,
                    Timeout = affiliateChannel.Timeout,
                    XmlTemplate = affiliateChannel.XmlTemplate
                };

                var affiliateChannelId = _affiliateChannelService.InsertAffiliateChannel(cloneAffiliateChannel);

                var access = new EntityOwnership
                {
                    Id = 0,
                    UserId = _appContext.AppUser.Id,
                    EntityId = affiliateChannelId,
                    EntityName = EntityType.AffiliateChannel.ToString()
                };
                _userService.InsertEntityOwnership(access);


                if (affiliateChannel.AffiliateChannelFields != null)
                {
                    cloneAffiliateChannel.AffiliateChannelFields = new List<AffiliateChannelTemplate>();

                    foreach (var field in affiliateChannel.AffiliateChannelFields)
                    {
                        cloneAffiliateChannel.AffiliateChannelFields.Add(new AffiliateChannelTemplate
                        {
                            Id = 0,
                            AffiliateChannelId = affiliateChannelId,
                            CampaignTemplateId = field.CampaignTemplateId,
                            TemplateField = field.TemplateField,
                            SectionName = field.SectionName,
                            DefaultValue = field.DefaultValue,
                            DataFormat = field.DataFormat
                        });
                    }

                    _affiliateChannelService.UpdateAffiliateChannel(cloneAffiliateChannel);
                }

                return Ok(cloneAffiliateChannel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpGet]
        [Route("getAffiliateChannelById/{id}")]
        public IHttpActionResult GetAffiliateChannelById(long id)
        {
            if (!_permissionService.Authorize(_viewAffiliateChannelKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(id);

                if (affiliateChannel == null)
                {
                    return HttpBadRequest($"no affiliate channel was found for given id {id}");
                  
                }

                var returnModel = GetAffiliateChannelResult(affiliateChannel);

                return Ok(returnModel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("getAffiliateChannelFields/{id}")]
        public IHttpActionResult GetAffiliateChannelFields(long id)
        {
            if (!_permissionService.Authorize(_viewAffiliateChannelKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(id);

                if (affiliateChannel == null)
                {
                    return HttpBadRequest($"no affiliate channel was found for given id {id}");

                }

                var fields = affiliateChannel.AffiliateChannelFields;

                fields = fields.Where(x => x.SectionName != "root").OrderBy(x => x.TemplateField).ToList();

                return Ok(fields);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Insert affiliate channel
        /// </summary>
        /// <param name="affiliateChannelCreateModel">AffiliateChannelCreateModel</param>
        /// <returns></returns>
        [HttpPost]
        [Route("addAffiliateChannel")]
        public IHttpActionResult CreateAffiliateChannel(AffiliateChannelCreateModel affiliateChannelCreateModel)
        {
            if (!_permissionService.Authorize(_editAffiliateChannelKey))
            {
                return HttpBadRequest("access-denied");
            }

            string template = string.Empty;
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());
            
            try
            {
                if (affiliateChannelCreateModel.AffiliateId <= 0)
                {
                    return HttpBadRequest($"affiliate id is required");
                }

                if (!string.IsNullOrEmpty(affiliateChannelCreateModel.Note) && affiliateChannelCreateModel.Note.Length > 500)
                {
                    return HttpBadRequest($"the maximum length of note should be 500");
                }

                var planLimitation = _planService.CheckPlanStatusesByUserId(_appContext.AppUser.Id);
                if (planLimitation != null && planLimitation.Contains(AdrackPlanVerificationStatus.AffiliateChannelLimitReached))
                {
                    return HttpBadRequest($"affiliate channel limit reached.");
                }

                if (affiliateChannelCreateModel.AffiliateChannelFilters != null)
                {
                    if (!ValidationListFilters(affiliateChannelCreateModel.AffiliateChannelFilters))
                    {
                        return HttpBadRequest($"Invalid filter conditions");
                    }
                }

                Campaign campaign = null;

                if (affiliateChannelCreateModel.CampaignId.HasValue)
                {
                    campaign = _campaignService.GetCampaignById(affiliateChannelCreateModel.CampaignId.Value);

                    if (campaign == null)
                    {
                        return HttpBadRequest($"affiliate channel limit reached.");
                    }
                    else if (string.IsNullOrEmpty(affiliateChannelCreateModel.XmlTemplate))
                    {
                        affiliateChannelCreateModel.XmlTemplate = campaign.DataTemplate;
                    }
                }

                XmlDocument xmlDocument = new XmlDocument();
                if (affiliateChannelCreateModel.XmlTemplate == null)
                {
                    xmlDocument.LoadXml("<request></request>");
                }
                else
                {
                    xmlDocument.LoadXml(affiliateChannelCreateModel.XmlTemplate);
                }

                var affiliateChannelFields = new List<AffiliateChannelTemplate>();

                if (affiliateChannelCreateModel.AffiliateChannelFields != null)
                {
                    Utils.ClearXmlElements(xmlDocument, affiliateChannelCreateModel.AffiliateChannelFields.Select(x => x.TemplateField + "," + x.SectionName).ToList());

                    foreach (var field in affiliateChannelCreateModel.AffiliateChannelFields)
                    {
                        if (string.IsNullOrEmpty(field.TemplateField)) continue;

                        affiliateChannelFields.Add(new AffiliateChannelTemplate
                        {
                            Id = 0,
                            CampaignTemplateId = field.CampaignFieldId,
                            AffiliateChannelId = 0,
                            TemplateField = field.TemplateField,
                            SectionName = field.SectionName,
                            DefaultValue = field.DefaultValue,
                            DataFormat = field.GetDataFormat()
                        });

                        Utils.AddXmlElement(xmlDocument, field.SectionName, field.TemplateField);
                    }
                }

                if (affiliateChannelCreateModel.Templates.Any())
                {
                    template = string.Join(",", affiliateChannelCreateModel.Templates);
                }

                long? campaignId = affiliateChannelCreateModel.CampaignId;
                if (campaignId.HasValue && campaignId.Value == 0)
                    campaignId = null;

                var affiliateChannel = new AffiliateChannel
                {
                    Id = 0,
                    CampaignId = campaignId,
                    AffiliateId = affiliateChannelCreateModel.AffiliateId,
                    Name = affiliateChannelCreateModel.Name,
                    Status = affiliateChannelCreateModel.Status,
                    XmlTemplate = xmlDocument.OuterXml,
                    DataFormat = affiliateChannelCreateModel.DataFormat,
                    MinPriceOption = 0,
                    NetworkTargetRevenue = affiliateChannelCreateModel.NetworkTargetRevenue,
                    NetworkMinimumRevenue = affiliateChannelCreateModel.NetworkMinimumRevenue,
                    ChannelKey = !string.IsNullOrWhiteSpace(affiliateChannelCreateModel.AffiliateChannelKey) ? affiliateChannelCreateModel.AffiliateChannelKey : Helper.GetUniqueKey(7),
                    ChannelPassword = affiliateChannelCreateModel.AffiliateChannelPassword,
                    IsDeleted = false,
                    AffiliatePriceMethod = (byte)(affiliateChannelCreateModel.IsFixed ? PriceType.Fixed : (affiliateChannelCreateModel.IsRevenue ? PriceType.Revenue : PriceType.InheritFromAffiliate)),
                    AffiliatePrice = affiliateChannelCreateModel.FixedValue,
                    Timeout = affiliateChannelCreateModel.Timeout,
                    Note = affiliateChannelCreateModel.Note,
                    AffiliateChannelFields = affiliateChannelFields
                };

                var affiliateChannelId = _affiliateChannelService.InsertAffiliateChannel(affiliateChannel);

                var access = new EntityOwnership
                {
                    Id = 0,
                    UserId = _appContext.AppUser.Id,
                    EntityId = affiliateChannelId,
                    EntityName = EntityType.AffiliateChannel.ToString()
                };
                _userService.InsertEntityOwnership(access);

                if (campaign != null)
                {
                    if (affiliateChannelCreateModel.AffiliateChannelBlackLists != null)
                    {
                        foreach (var blackListModel in affiliateChannelCreateModel.AffiliateChannelBlackLists)
                        {
                            var blackList = new CustomBlackListValue
                            {
                                Id = 0,
                                ChannelId = affiliateChannelId,
                                ChannelType = (byte)ChannelType.AffiliateChannel,
                                Value = string.Join(",", blackListModel.Values),
                                TemplateFieldId = blackListModel.FieldId
                            };
                            _blackListService.InsertCustomBlackListValue(blackList);
                        }
                    }

                    if (affiliateChannelCreateModel.AffiliateChannelFilters != null)
                    {
                        UpdateListFilters(affiliateChannelId, affiliateChannelCreateModel.AffiliateChannelFilters);
                    }
                }


                if (!string.IsNullOrWhiteSpace(affiliateChannelCreateModel.Note))
                {
                    var newNote = new AffiliateChannelNote()
                    {
                        Created = DateTime.UtcNow,
                        AffiliateChannelId = affiliateChannelId,
                        Note = affiliateChannelCreateModel.Note,
                        Updated = null,
                        Title = string.Empty,
                        UserId = _appContext.AppUser.Id
                    };
                    _affiliateChannelService.InsertAffiliateChannelNote(newNote);
                }


                return Ok(affiliateChannel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
                
            }
        }

        /// <summary>
        /// Update affiliate channel all black list
        /// </summary>
        /// <param name="affiliateChannelId"></param>
        /// <param name="blackListModels"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateAffiliateChannelBlackListValues/{affiliateChannelId}")]
        public IHttpActionResult UpdateAffiliateChannelBlackListValues(long affiliateChannelId, List<AffiliateChannelBlackListModel> blackListModels)
        {
            if (!_permissionService.Authorize(_editBlackListKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(affiliateChannelId);

                if (affiliateChannel == null)
                {
                    return HttpBadRequest($"no affiliate channel was found for given id {affiliateChannelId}");
                }

                _blackListService.DeleteCustomBlackListValues(affiliateChannelId, (byte)ChannelType.AffiliateChannel);

                foreach (var blackListModel in blackListModels)
                {
                    var blackList = new CustomBlackListValue
                    {
                        Id = 0,
                        ChannelId = affiliateChannelId,
                        ChannelType = (byte)ChannelType.AffiliateChannel,
                        Value = string.Join(",", blackListModel.Values),
                        TemplateFieldId = blackListModel.FieldId
                    };
                    _blackListService.InsertCustomBlackListValue(blackList);
                }
                return Ok(blackListModels);
            }
            catch (Exception e)
            {
                return HttpBadRequest(e.Message);
            }
        }
        /// <summary>
        /// Add affiliate channel filter condition by channel id
        /// </summary>
        /// <param name="id">long</param>
        /// <param name="affiliateChannelFilterCreateModel">AffiliateChannelFilterCreateModel</param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateAffiliateChannelFilters/{affiliateChannelId}")]
        public IHttpActionResult UpdateAffiliateChannelFilters(long affiliateChannelId, List<AffiliateChannelFilterCreateModel> affiliateChannelFilterCreateModels)
        {
            if (!_permissionService.Authorize(_editFilterKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                if (affiliateChannelFilterCreateModels != null)
                {
                    if (!ValidationListFilters(affiliateChannelFilterCreateModels))
                    {
                        return HttpBadRequest($"Invalid filter conditions");
                    }
                }

                var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(affiliateChannelId);

                if (affiliateChannel == null)
                {
                    return HttpBadRequest($"no affiliate channel was found for given id {affiliateChannelId}");
                }

                UpdateListFilters(affiliateChannelId, affiliateChannelFilterCreateModels);

                return Ok(affiliateChannelFilterCreateModels);
            }
            catch (Exception e)
            {
                return HttpBadRequest(e.Message);
            }
        }

        private IHttpActionResult UpdateListFilters(long affiliateChannelId, List<AffiliateChannelFilterCreateModel> filterModels)
        {
            if (!_permissionService.Authorize(_editFilterKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(affiliateChannelId);

                if (affiliateChannel == null)
                {
                    return HttpBadRequest($"affiliate channel not found");
                }

                UpdateListFilters(affiliateChannel, filterModels);

                return Ok("");
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        protected void UpdateListFilters(AffiliateChannel affiliateChannel, List<AffiliateChannelFilterCreateModel> filterModels, bool updateInDb = true)
        {
            bool isNew = false;  

            Campaign campaign = _campaignService.GetCampaignById(affiliateChannel.CampaignId.Value);  
             
            _affiliateChannelFilterConditionService.DeleteFilterConditions(affiliateChannel.Id);
            var conditions = new List<AffiliateChannelFilterCondition>();
            Dictionary<string, List<AffiliateChannelFilterCondition>> filterConditions = new Dictionary<string, List<AffiliateChannelFilterCondition>>();

            foreach (var conditionModel in filterModels)
            {
                var filterCondition = (AffiliateChannelFilterCondition)conditionModel;

                filterCondition.AffiliateChannelId = affiliateChannel.Id;

                if (conditionModel.CampaignFieldId == 0 && campaign != null && !string.IsNullOrEmpty(conditionModel.CampaignFieldName))
                {
                    var campaignField = campaign.CampaignFields.Where(x => x.TemplateField.ToString() == conditionModel.CampaignFieldName.ToLower()).FirstOrDefault();
                    if (campaignField != null)
                    {
                        filterCondition.CampaignTemplateId = campaignField.Id;
                    }
                }

                _affiliateChannelFilterConditionService.InsertFilterCondition(filterCondition);

                conditions.Add(filterCondition);

                string filterConditionKey = $"{filterCondition.Value}{filterCondition.CampaignTemplateId}{filterCondition.Condition}{filterCondition.AffiliateChannelId}";

                var subConditions = new List<AffiliateChannelFilterCondition>();

                if (conditionModel.Children != null)
                {
                    foreach (var subConditionModel in conditionModel.Children)
                    {
                        var filterSubCondition = (AffiliateChannelFilterCondition)subConditionModel;

                        filterSubCondition.AffiliateChannelId = affiliateChannel.Id;

                        if (subConditionModel.CampaignFieldId == 0 && campaign != null && !string.IsNullOrEmpty(subConditionModel.CampaignFieldName))
                        {
                            var campaignField = campaign.CampaignFields.Where(x => x.TemplateField.ToString() == subConditionModel.CampaignFieldName.ToLower()).FirstOrDefault();
                            if (campaignField != null)
                            {
                                filterSubCondition.CampaignTemplateId = campaignField.Id;
                            }
                        }

                        subConditions.Add(filterSubCondition);
                    }
                }

                filterConditions[filterConditionKey] = subConditions;
            }

            //affiliateChannel.AffiliateChannelFilterConditions = conditions;

            foreach (var filterCondition in conditions)
            {
                string filterConditionKey = $"{filterCondition.Value}{filterCondition.CampaignTemplateId}{filterCondition.Condition}{filterCondition.AffiliateChannelId}";

                if (filterConditions.ContainsKey(filterConditionKey))
                {
                    foreach (var subFilterCondition in filterConditions[filterConditionKey])
                    {
                        subFilterCondition.ParentId = filterCondition.Id;
                        //conditions.Add(subFilterCondition);

                        _affiliateChannelFilterConditionService.InsertFilterCondition(subFilterCondition);
                    }
                }
            }

            //affiliateChannel.AffiliateChannelFilterConditions = conditions;

            if (updateInDb)
                _affiliateChannelService.UpdateAffiliateChannel(affiliateChannel);
        }

        protected object GetAffiliateChannelResult(AffiliateChannel affiliateChannel)
        {
            List<AffiliateChannelFilterCreateModel> filters = new List<AffiliateChannelFilterCreateModel>();

            if (affiliateChannel.AffiliateChannelFilterConditions != null)
            {
                foreach (var c in affiliateChannel.AffiliateChannelFilterConditions.Where(x => x.ParentId == 0))
                {
                    var ct = _campaignTemplateService.GetCampaignTemplateById(c.CampaignTemplateId);
                    AffiliateChannelFilterCreateModel filter = new AffiliateChannelFilterCreateModel();
                    filter.Id = c.Id;
                    filter.CampaignFieldId = c.CampaignTemplateId;
                    filter.CampaignFieldName = ct != null ? ct.TemplateField : "";
                    filter.Condition = c.Condition;
                    filter.ParentId = c.ParentId.HasValue ? c.ParentId.Value : 0;
                    filter.Values = new List<FilterConditionValueModel>();
                    filter.Children = new List<AffiliateChannelSubFilterModel>();

                    if (!string.IsNullOrEmpty(c.Value))
                    {
                        string[] values = c.Value.Split(new char[1] { ',' });
                        foreach (var value in values)
                        {
                            string[] ranges = value.Split(new char[1] { '-' });

                            if (ranges.Length > 0)
                            {
                                FilterConditionValueModel filterConditionValueModel = new FilterConditionValueModel();
                                filterConditionValueModel.Value1 = ranges[0];
                                if (ranges.Length > 1)
                                    filterConditionValueModel.Value2 = ranges[1];
                                filter.Values.Add(filterConditionValueModel);
                            }
                        }
                    }

                    foreach (var c2 in affiliateChannel.AffiliateChannelFilterConditions.Where(x => x.ParentId == c.Id))
                    {
                        ct = _campaignTemplateService.GetCampaignTemplateById(c2.CampaignTemplateId);
                        AffiliateChannelSubFilterModel filter2 = new AffiliateChannelSubFilterModel();
                        filter2.Id = c2.Id;
                        filter2.CampaignFieldId = c2.CampaignTemplateId;
                        filter2.CampaignFieldName = ct != null ? ct.TemplateField : "";
                        filter2.Condition = c2.Condition;
                        filter2.ParentId = c2.ParentId.HasValue ? c2.ParentId.Value : 0;
                        filter2.Values = new List<FilterConditionValueModel>();
                        if (!string.IsNullOrEmpty(c2.Value))
                        {
                            string[] values = c2.Value.Split(new char[1] { ',' });
                            foreach (var value in values)
                            {
                                string[] ranges = value.Split(new char[1] { '-' });

                                if (ranges.Length > 0)
                                {
                                    FilterConditionValueModel filterConditionValueModel = new FilterConditionValueModel();
                                    filterConditionValueModel.Value1 = ranges[0];
                                    if (ranges.Length > 1)
                                        filterConditionValueModel.Value2 = ranges[1];
                                    filter2.Values.Add(filterConditionValueModel);
                                }
                            }
                        }

                        filter.Children.Add(filter2);
                    }

                    filters.Add(filter);
                }
            }

            var returnModel = new
            {
                AffiliateChannelFields = affiliateChannel.AffiliateChannelFields,
                AffiliateChannelFilters = filters,//affiliateChannel.AffiliateChannelFilterConditions,
                AffiliateChannelKey = affiliateChannel.ChannelKey,
                AffiliateChannelPassword = affiliateChannel.ChannelPassword,
                AffiliateId = affiliateChannel.AffiliateId,
                AffiliatePrice = affiliateChannel.AffiliatePrice,
                AffiliatePriceMethod = affiliateChannel.AffiliatePriceMethod,
                AttachedBuyerChannels = _affiliateChannelService.GetAttachedBuyerChannels(affiliateChannel.Id),
                CampaignId = affiliateChannel.CampaignId,
                DataFormat = affiliateChannel.DataFormat,
                Deleted = affiliateChannel.IsDeleted,
                Id = affiliateChannel.Id,
                MinPriceOption = affiliateChannel.MinPriceOption,
                Name = affiliateChannel.Name,
                NetworkMinimumRevenue = affiliateChannel.NetworkMinimumRevenue,
                NetworkTargetRevenue = affiliateChannel.NetworkTargetRevenue,
                Note = affiliateChannel.Note,
                Status = affiliateChannel.Status,
                Timeout = affiliateChannel.Timeout,
                XmlTemplate = affiliateChannel.XmlTemplate,
                AffiliateName = _affiliateService.GetAffiliateById(affiliateChannel.AffiliateId, true)?.Name
            };

            return returnModel;
        }

        /// <summary>
        /// Delete affiliate channel filter condition by filter id
        /// </summary>
        /// <param name="id">long</param>
        /// <returns></returns>
        [HttpPost]
        [Route("deleteAffiliateChannelFilter/{id}")]
        public IHttpActionResult DeleteAffiliateChannelFilter(long id)
        {
            if (!_permissionService.Authorize(_editFilterKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var affiliateChannelFilter = _affiliateChannelFilterConditionService.GetFilterConditionById(id);

                if (affiliateChannelFilter == null)
                {
                    return HttpBadRequest($"no affiliate channel filter condition was found for given id {id}");
                }

                _affiliateChannelFilterConditionService.DeleteFilterCondition(affiliateChannelFilter);

                return Ok();
            }
            catch (Exception e)
            {
                return HttpBadRequest(e.Message);
            }
        }

        /// <summary>
        /// Update affiliate channel
        /// </summary>
        /// <param name="affiliateChannelId">long</param>
        /// <param name="affiliateChannelCreateModel">AffiliateChannelCreateModel</param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateAffiliateChannel/{affiliateChannelId}")]
        public IHttpActionResult UpdateAffiliateChannel(long affiliateChannelId, AffiliateChannelCreateModel affiliateChannelCreateModel)
        {
            if (!_permissionService.Authorize(_editAffiliateChannelKey))
            {
                return HttpBadRequest("access-denied");
            }
            string template = string.Empty;
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(affiliateChannelId);

                if (affiliateChannel == null)
                {
                    return HttpBadRequest($"no affiliate channel was found for given id {affiliateChannelId}");
                }

                if (affiliateChannelCreateModel == null)
                {
                    return HttpBadRequest($"no affiliate channel was found for given id {affiliateChannelId}");
                }

                if (!string.IsNullOrEmpty(affiliateChannelCreateModel.Note) && affiliateChannelCreateModel.Note.Length > 500)
                {
                    return HttpBadRequest($"the maximum length of note should be 500");
                }

                Campaign campaign = null;

                /*if (affiliateChannelCreateModel.CampaignId.HasValue)
                {
                    campaign = _campaignService.GetCampaignById(affiliateChannelCreateModel.CampaignId.Value);

                    if (campaign == null)
                    {
                        return HttpBadRequest($"campaign not found.");
                    }
                    else if (affiliateChannelCreateModel.XmlTemplate == "")
                    {
                        affiliateChannelCreateModel.XmlTemplate = campaign.XmlTemplate;
                    }
                }*/

                XmlDocument xmlDocument = new XmlDocument();
                if (affiliateChannelCreateModel.XmlTemplate == null)
                {
                    xmlDocument.LoadXml("<request></request>");
                }
                else
                {
                    xmlDocument.LoadXml(affiliateChannelCreateModel.XmlTemplate);
                }

                if (affiliateChannelCreateModel.Templates.Any())
                {
                    template = string.Join(",", affiliateChannelCreateModel.Templates);
                }

                long? campaignId = affiliateChannelCreateModel.CampaignId;
                if (campaignId.HasValue && campaignId.Value == 0)
                    campaignId = null;

                affiliateChannel.CampaignId = campaignId;
                affiliateChannel.AffiliateId = affiliateChannelCreateModel.AffiliateId;
                affiliateChannel.Name = affiliateChannelCreateModel.Name;
                affiliateChannel.Status = affiliateChannelCreateModel.Status;
                affiliateChannel.DataFormat = affiliateChannelCreateModel.DataFormat;
                affiliateChannel.NetworkTargetRevenue = affiliateChannelCreateModel.NetworkTargetRevenue;
                affiliateChannel.NetworkMinimumRevenue = affiliateChannelCreateModel.NetworkMinimumRevenue;
                affiliateChannel.AffiliatePriceMethod = (byte)(affiliateChannelCreateModel.IsFixed
                    ? PriceType.Fixed : (affiliateChannelCreateModel.IsRevenue ? PriceType.Revenue : PriceType.InheritFromAffiliate));
                affiliateChannel.AffiliatePrice = affiliateChannelCreateModel.FixedValue;
                affiliateChannel.Timeout = affiliateChannelCreateModel.Timeout;
                affiliateChannel.Note = affiliateChannelCreateModel.Note;
                affiliateChannel.ChannelKey = !string.IsNullOrWhiteSpace(affiliateChannelCreateModel.AffiliateChannelKey)
                    ? affiliateChannelCreateModel.AffiliateChannelKey : Helper.GetUniqueKey(7);
                affiliateChannel.ChannelPassword = affiliateChannelCreateModel.AffiliateChannelPassword;
                affiliateChannel.AffiliatePriceMethod = affiliateChannelCreateModel.AffiliatePriceMethod;
                affiliateChannel.AffiliatePrice = affiliateChannelCreateModel.AffiliatePrice;

                //if (campaign != null)
                {
                    _affiliateChannelTemplateService.DeleteAffiliateChannelTemplatesByAffiliateChannelId(affiliateChannelId);

                    Utils.ClearXmlElements(xmlDocument, affiliateChannelCreateModel.AffiliateChannelFields.Select(x => x.TemplateField + "," + x.SectionName).ToList());

                    Dictionary<string, XmlElement> sectionElements = new Dictionary<string, XmlElement>();

                    foreach (var field in affiliateChannelCreateModel.AffiliateChannelFields)
                    {
                        if (string.IsNullOrEmpty(field.TemplateField)) continue;

                        long campaignFieldId = field.CampaignTemplateId.HasValue ? field.CampaignTemplateId.Value : field.CampaignFieldId;

                        if (campaignFieldId == 0 && campaignId.HasValue)
                        {
                            var campaignFields =_campaignTemplateService.GetCampaignTemplatesByCampaignId(campaignId.Value);
                            var campaignField = campaignFields.Where(x => x.TemplateField == field.TemplateField && x.SectionName == field.SectionName).FirstOrDefault();
                            if (campaignField != null)
                            {
                                campaignFieldId = campaignField.Id;
                            }
                        }

                        affiliateChannel.AffiliateChannelFields.Add(new AffiliateChannelTemplate
                        {
                            Id = 0,
                            CampaignTemplateId = campaignFieldId,
                            AffiliateChannelId = affiliateChannel.Id,
                            TemplateField = field.TemplateField,
                            SectionName = field.SectionName,
                            DefaultValue = field.DefaultValue,
                            DataFormat = field.GetDataFormat()
                        });

                        Utils.AddXmlElement(xmlDocument, field.SectionName, field.TemplateField);
                    }

                    UpdateListFilters(affiliateChannel, affiliateChannelCreateModel.AffiliateChannelFilters, false);
                }

                affiliateChannel.XmlTemplate = xmlDocument.OuterXml;

                _affiliateChannelService.UpdateAffiliateChannel(affiliateChannel);

                affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(affiliateChannelId);

                var returnModel = GetAffiliateChannelResult(affiliateChannel);

                return Ok(affiliateChannel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Activate affiliate channel by id 
        /// </summary>
        /// <param name="id">long</param>
        /// <param name="status">short</param>
        /// <returns></returns>
        [HttpPost]
        [Route("activateAffiliateChannel/{id}/{status}")]
        public IHttpActionResult ActivateAffiliateChannel(long id, short status)
        {
            if (!_permissionService.Authorize(_editAffiliateChannelKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(id);

                if (affiliateChannel == null)
                {
                    return HttpBadRequest($"no affiliate channel was found for given id {id}");
                }

                affiliateChannel.Status = status;
                _affiliateChannelService.UpdateAffiliateChannel(affiliateChannel);

                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Insert affiliate channel template
        /// </summary>
        /// <param name="affiliateChannelIntegrationModel">AffiliateChannelIntegrationModel</param>
        /// <returns></returns>
        [HttpPost]
        [Route("addAffiliateChannelField")]
        public IHttpActionResult CreateAffiliateChannelField(AffiliateChannelIntegrationModel affiliateChannelIntegrationModel)
        {
            if (!_permissionService.Authorize(_editAffiliateChannelKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(affiliateChannelIntegrationModel.AffiliateChannelId);

                XmlDocument xmlDocument = new XmlDocument();
                try
                {
                    xmlDocument.LoadXml(affiliateChannel.XmlTemplate);
                }
                catch
                {
                    xmlDocument.LoadXml("<request></request>");
                }
                Utils.AddXmlElement(xmlDocument, affiliateChannelIntegrationModel.SectionName, affiliateChannelIntegrationModel.TemplateField);

                var affiliateChannelTemplate = new AffiliateChannelTemplate
                {
                    Id = 0,
                    CampaignTemplateId = affiliateChannelIntegrationModel.CampaignFieldId,
                    AffiliateChannelId = affiliateChannelIntegrationModel.AffiliateChannelId,
                    TemplateField = affiliateChannelIntegrationModel.TemplateField,
                    SectionName = affiliateChannelIntegrationModel.SectionName,
                    DefaultValue = affiliateChannelIntegrationModel.DefaultValue
                };

                var affiliateChannelId = _affiliateChannelTemplateService.InsertAffiliateChannelTemplate(affiliateChannelTemplate);

                affiliateChannel.XmlTemplate = xmlDocument.OuterXml;

                return Ok(affiliateChannelTemplate);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Update affiliate channel template
        /// </summary>
        /// <param name="id">long</param>
        /// <param name="affiliateChannelIntegrationModel">AffiliateChannelIntegrationModel</param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateAffiliateChannelField/{id}")]
        public IHttpActionResult UpdateAffiliateChannelField(long id, AffiliateChannelIntegrationModel affiliateChannelIntegrationModel)
        {
            if (!_permissionService.Authorize(_editAffiliateChannelKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var affiliateChannelField = _affiliateChannelTemplateService.GetAffiliateChannelTemplateById(id);

                if (affiliateChannelField == null)
                {
                    return HttpBadRequest($"no affiliate channel field was found for given id {id}");
                }

                if (affiliateChannelIntegrationModel == null)
                {
                    return HttpBadRequest($"no affiliate channel field was found for given id {id}");
                }

                affiliateChannelField.CampaignTemplateId = affiliateChannelIntegrationModel.CampaignFieldId;
                affiliateChannelField.TemplateField = affiliateChannelIntegrationModel.TemplateField;
                affiliateChannelField.SectionName = affiliateChannelIntegrationModel.SectionName;
                affiliateChannelField.DefaultValue = affiliateChannelIntegrationModel.DefaultValue;
                affiliateChannelField.DataFormat = affiliateChannelIntegrationModel.GetDataFormat();

                _affiliateChannelTemplateService.UpdateAffiliateChannelTemplate(affiliateChannelField);

                return Ok(affiliateChannelField);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPut]
        [Route("updateAffiliateChannelName")]
        public IHttpActionResult UpdateAffiliateChannelName([FromBody] AffiliateChannelNameUpdateModel affiliateChannelModel)
        {
            if (!_permissionService.Authorize(_editAffiliateChannelKey))
            {
                return HttpBadRequest("access-denied");
            }
            if (string.IsNullOrWhiteSpace(affiliateChannelModel.Name))
            {
                return HttpBadRequest("Affiliate channel name is required");
            }
            if (affiliateChannelModel.AffiliateChannelId == 0)
            {
                return HttpBadRequest("Affiliate channel id is required");
            }

            if (_affiliateChannelService.GetAffiliateChannelByName(affiliateChannelModel.Name, affiliateChannelModel.AffiliateChannelId) != null)
            {
                return HttpBadRequest("Affiliate channel with the specified name already exists");
            }

            else
            {
                var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(affiliateChannelModel.AffiliateChannelId, false);
                if (affiliateChannel == null)
                {
                    return HttpBadRequest("Affiliate channel with the specified id doesn't exist");
                }
                else
                {
                    affiliateChannel.Name = affiliateChannelModel.Name;
                    _affiliateChannelService.UpdateAffiliateChannel(affiliateChannel);
                    return Ok(affiliateChannelModel);
                }
            }
        }

        /// <summary>
        /// Get affiliate channels by campaign id
        /// </summary>
        /// <param name="id">campaign Id</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Route("getAffiliateChannelsByCampaignId/{id}")]
        public IHttpActionResult GetAffiliateChannelsByCampaignId(long id)
        {
            if (!_permissionService.Authorize(_viewAffiliateChannelKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var affiliateChannelModels = new List<AffiliateChannelViewModel>();
                var affiliateChannels = _affiliateChannelService.GetAllAffiliateChannelsByCampaignId(id);

                foreach (var channel in affiliateChannels)
                {
                    var affiliate = _affiliateService.GetAffiliateById(channel.AffiliateId, false);
                    var campaign = _campaignService.GetCampaignById(channel.CampaignId.Value, false);

                    affiliateChannelModels.Add(new AffiliateChannelViewModel
                    {
                        Id = channel.Id,
                        CampaignId = channel.CampaignId,
                        CampaignName = campaign.Name,
                        AffiliateId = channel.AffiliateId,
                        AffiliateName = affiliate.Name,
                        Name = channel.Name,
                        Status = channel.Status,
                        XmlTemplate = channel.XmlTemplate,
                        DataFormat = channel.DataFormat,
                        MinPriceOption = channel.MinPriceOption,
                        MinPriceOptionValue = channel.NetworkTargetRevenue,
                        MinRevenue = channel.NetworkMinimumRevenue,
                        AffiliateChannelKey = channel.ChannelKey,
                        Deleted = channel.IsDeleted,
                        AffiliatePriceMethod = channel.AffiliatePriceMethod,
                        AffiliatePrice = channel.AffiliatePrice,
                        Timeout = channel.Timeout,
                        Note = channel.Note
                    });
                }
                return Ok(affiliateChannelModels);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Get affiliate channels by affiliate id
        /// </summary>
        /// <param name="id">campaign Id</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Route("getAffiliateChannelsByAffiliateId/{affiliateId}")]
        public IHttpActionResult GetAffiliateChannelsByAffiliateId(long affiliateId)
        {
            if (!_permissionService.Authorize(_viewAffiliateChannelKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var affiliateChannelModels = new List<AffiliateChannelViewModel>();
                var affiliateChannels = _affiliateChannelService.GetAllAffiliateChannelsByAffiliateId(affiliateId);

                foreach (var channel in affiliateChannels)
                {
                    var affiliate = _affiliateService.GetAffiliateById(channel.AffiliateId, false);
                    Campaign campaign = null;
                    if (channel.CampaignId != null)
                    {
                        campaign = _campaignService.GetCampaignById(channel.CampaignId.Value, false);
                    }

                    affiliateChannelModels.Add(new AffiliateChannelViewModel
                    {
                        Id = channel.Id,
                        CampaignId = channel.CampaignId,
                        CampaignName = campaign?.Name,
                        AffiliateId = channel.AffiliateId,
                        AffiliateName = affiliate.Name,
                        Name = channel.Name,
                        Status = channel.Status,
                        XmlTemplate = channel.XmlTemplate,
                        DataFormat = channel.DataFormat,
                        MinPriceOption = channel.MinPriceOption,
                        MinPriceOptionValue = channel.NetworkTargetRevenue,
                        MinRevenue = channel.NetworkMinimumRevenue,
                        AffiliateChannelKey = channel.ChannelKey,
                        Deleted = channel.IsDeleted,
                        AffiliatePriceMethod = channel.AffiliatePriceMethod,
                        AffiliatePrice = channel.AffiliatePrice,
                        Timeout = channel.Timeout,
                        Note = channel.Note
                    });
                }
                return Ok(affiliateChannelModels);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get All Campaigns with fields
        /// </summary>
        /// <param name="id">campaign Id</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Route("getAffiliateChannels/{status}")]
        [ContentManagementCache("App.Cache.AffiliateChannel.")]
        public IHttpActionResult GetAffiliateChannels(EntityFilterByStatus status = EntityFilterByStatus.All, [FromUri] List<long> affiliateIds = null, [FromUri] List<long> campaignIds = null)
        {
            if (!_permissionService.Authorize(_viewAffiliateChannelKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                if (affiliateIds != null && affiliateIds.Count == 0)
                    affiliateIds = null;

                if (campaignIds != null && campaignIds.Count == 0)
                    campaignIds = null;

                var affiliateChannelModels = new List<AffiliateChannelViewModel>();
                var affiliateChannels = _affiliateChannelService.GetAllAffiliateChannels();

                if (status != EntityFilterByStatus.All)
                    affiliateChannels = affiliateChannels.Where(x => x.Status == (short)status).ToList();

                if (affiliateChannels == null)
                    return NotFound();

                var result = (from ac in affiliateChannels
                    where (affiliateIds == null || affiliateIds.Contains(ac.AffiliateId)) &&
                          (campaignIds == null || (ac.CampaignId.HasValue && campaignIds.Contains(ac.CampaignId.Value)))
                    select ac
                    ).ToList();

                foreach (var channel in result)
                {
                    affiliateChannelModels.Add(CreateAffiliateChannelModel(channel));
                }
                return Ok(affiliateChannelModels);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete affiliate channel by id
        /// </summary>
        /// <param name="id">long</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("deleteAffiliateChannelById/{id}")]
        public IHttpActionResult DeleteAffiliateChannelById(long id)
        {
            if (!_permissionService.Authorize(_editAffiliateChannelKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(id);

                if (affiliateChannel == null)
                {
                    return HttpBadRequest($"no affiliate channel was found for given id {id}");
                }

                affiliateChannel.IsDeleted = true;
                _affiliateChannelService.UpdateAffiliateChannel(affiliateChannel);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Delete affiliate channel field by id
        /// </summary>
        /// <param name="id">long</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteAffiliateChannelField/{id}")]
        public IHttpActionResult DeleteAffiliateChannelField(long id)
        {
            if (!_permissionService.Authorize(_editAffiliateChannelKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var affiliateChannelField = _affiliateChannelTemplateService.GetAffiliateChannelTemplateById(id);

                if (affiliateChannelField == null)
                {
                    return HttpBadRequest($"no affiliate channel field was found for given id {id}");
                }

                _affiliateChannelTemplateService.DeleteAffiliateChannelTemplate(affiliateChannelField);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("getAttachedBuyerChannelListById/{id}")]
        public IHttpActionResult GetAttachedBuyerChannelListById(long id)
        {
            if (!_permissionService.Authorize(_viewAndEditBuyerChannelKey))
            {
                return HttpBadRequest("access-denied");
            }
            var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(id, false);
            if (affiliateChannel == null)
            {
                return HttpBadRequest($"no affiliate channel field was found for given id {id}");
            }

            if (!affiliateChannel.CampaignId.HasValue)
            {
                return HttpBadRequest($"no campaign attached to affiliate channel");
            }

            var attachedBuyerChannels = new List<AttachedBuyerChannelModel>();
            var buyerChannels = _buyerChannelService.GetAllBuyerChannelsByCampaignId(affiliateChannel.CampaignId.Value);

            var atBuyerChannels = _affiliateChannelService.GetAttachedBuyerChannels(affiliateChannel.Id);

            foreach (var channel in buyerChannels)
            {
                var includedChannel = atBuyerChannels.FirstOrDefault(x => x.Id == channel.Id);
                attachedBuyerChannels.Add(new AttachedBuyerChannelModel
                {
                    Id = channel.Id,
                    Name = channel.Name,
                    Included = includedChannel != null ? true : false,
                    Status = channel.Status
                });
            }

            return Ok(attachedBuyerChannels);
        }

        [HttpPost]
        [Route("attachOrDetachAffiliateChannel/{id}")]
        public IHttpActionResult AttacheOrDetachAffiliateChannel(long id, [FromBody]BuyerChannelAttachOrDetachModel buyerChannelAttachOrDetachModel)
        {
            if (!_permissionService.Authorize(_editAffiliateChannelKey))
            {
                return HttpBadRequest("access-denied");
            }
            var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(id, false);
            if (affiliateChannel == null)
            {
                return HttpBadRequest($"no affiliate channel field was found for given id {id}");
            }

            if (buyerChannelAttachOrDetachModel.IsAttach)
            {
                var buyerChannel = _buyerChannelService.GetBuyerChannelById(buyerChannelAttachOrDetachModel.BuyerChannelId);
                if (buyerChannel == null)
                {
                    return HttpBadRequest($"no buyer channel field was found for given id {id}");
                }
                _affiliateChannelService.AttachBuyerChannel(affiliateChannel.Id, buyerChannel.Id);
            }
            else
            {
                var includedChannel = _affiliateChannelService.GetAttachedBuyerChannels(affiliateChannel.Id).FirstOrDefault(x => x.Id == buyerChannelAttachOrDetachModel.BuyerChannelId);
                if (includedChannel == null)
                {
                    return HttpBadRequest($"no buyer channel field was found for given id {id}");
                }

                _affiliateChannelService.DettachBuyerChannel(affiliateChannel.Id, includedChannel.Id);
            }
            _affiliateChannelService.UpdateAffiliateChannel(affiliateChannel);
            return Ok(affiliateChannel);
        }

        [HttpGet]
        [Route("getAffiliateChannelsBySearchPattern")]
        public IHttpActionResult GetAffiliateChannelsBySearchPattern(string inputValue)
        {
            if (!_permissionService.Authorize(_viewAffiliateChannelKey))
            {
                return HttpBadRequest("access-denied");
            }
            var affiliateChannelModels = new List<AffiliateChannelViewModel>();
            var affiliateChannels = _affiliateChannelService.GetAllAffiliateChannels();
            foreach (var channel in affiliateChannels)
            {
                var affiliateChannel = CreateAffiliateChannelModel(channel);
                if (_searchService.CheckPropValue(affiliateChannel, inputValue))
                {
                    affiliateChannelModels.Add(affiliateChannel);
                }
            }
            return Ok(affiliateChannelModels);
        }

        [HttpGet]
        [Route("getFormTemplate")]
        public IHttpActionResult GetFormTemplate(int affiliateChannelId, int templateId)
        {
            AffiliateChannel ach = this._affiliateChannelService.GetAffiliateChannelById(affiliateChannelId);

            if(ach == null)
            {
                return Ok(0);
            }
            /*            
                        string JsCodeString = "<script src='https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js'></script>" +
                                                "<script src='https://code.jquery.com/ui/1.12.1/jquery-ui.min.js'></script>" +
                                                "<script src='https://formz.azurewebsites.net/form/js/TimeCircles.js?v1'></script>" +
                                                "<script src='https://formz.azurewebsites.net/form/js/moment.js?v1'></script>" +
                                                "<script src='https://formz.azurewebsites.net/form/FormGeneration-us.js?'"+ ach.AffiliateChannelPassword + "#" + ach.AffiliateChannelKey + "></script>" +
                                               "<div id='FormGeneration'></div>";
            */
/*            
            string JsCodeString = "<div id='FormGeneration'></div>" +
            "<link rel='stylesheet' href='https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css\'>" +
            "<script src='https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js'></script>" +
            "<script src='https://code.jquery.com/ui/1.12.1/jquery-ui.min.js'></script>" +
            "<script src='" + Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/forms/templates/1/js/TimeCircles.js?v1' type='text/javascript'></script>" +
            "<script src='" + Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/forms/templates/1/js/moment.js?v1' type='text/javascript'></script>" +
            "<script src='" + Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/forms/templates/1/FormGeneration.js?v1' type='text/javascript'></script>";
*/
            string JsCodeString = "<div id='FormGeneration'></div>" +
            "<link rel='stylesheet' href='https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css\'>" +
            "<script src='https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js'></script>" +
            "<script src='https://code.jquery.com/ui/1.12.1/jquery-ui.min.js'></script>" +
            "<script src='https://formz.azurewebsites.net/forms/templates/"+ templateId + "/js/TimeCircles.js?v1' type='text/javascript'></script>" +
            "<script src='https://formz.azurewebsites.net/forms/templates/" + templateId + "/js/moment.js?v1' type='text/javascript'></script>" +
            "<script src='https://formz.azurewebsites.net/forms/templates/" + templateId + "/FormGeneration.js?v1' type='text/javascript'></script>";

            return Ok(JsCodeString);
        }

        [HttpPost]
        [Route("generateViewLink")]
        public IHttpActionResult GenerateViewLink(int affiliateChannelId)
        {
            AffiliateChannel affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(affiliateChannelId);
            if (affiliateChannel == null)
            {
                return HttpBadRequest("Affiliate channel not found");
            }

            ClickChannel clickChannel = _clickService.GetClickChannelByAffiliateChannelId(affiliateChannelId, false);
            if (clickChannel == null)
            {
                clickChannel = new ClickChannel()
                {
                    AccessKey = Guid.NewGuid().ToString(),
                    AffiliateChannelId = affiliateChannelId,
                    ClickPrice = 0,
                    Name = affiliateChannel.Name,
                    RedirectUrl = ""
                };
                _clickService.InsertClickChannel(clickChannel);
            }

            string url = $"{Request.GetBaseUrl()}click?key={clickChannel.AccessKey}&type=view";

            return Ok(new { url = url });
        }

        [HttpPost]
        [Route("generateClickLink")]
        public IHttpActionResult GenerateClickLink(int affiliateChannelId, [FromBody] ClickLinkModel clickLinkModel)
        {
            AffiliateChannel affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(affiliateChannelId);
            if (affiliateChannel == null)
            {
                return HttpBadRequest("Affiliate channel not found");
            }

            ClickChannel clickChannel = _clickService.GetClickChannelByAffiliateChannelId(affiliateChannelId, false);
            if (clickChannel == null)
            {
                clickChannel = new ClickChannel()
                {
                    AccessKey = Guid.NewGuid().ToString(),
                    AffiliateChannelId = affiliateChannelId,
                    ClickPrice = clickLinkModel.ClickPrice,
                    Name = affiliateChannel.Name,
                    RedirectUrl = clickLinkModel.RedirectUrl
                };
                _clickService.InsertClickChannel(clickChannel);
            }
            else
            {
                clickChannel.ClickPrice = clickLinkModel.ClickPrice;
                clickChannel.RedirectUrl = clickLinkModel.RedirectUrl;
                _clickService.UpdateClickChannel(clickChannel);
            }

            string url = $"{Request.GetBaseUrl()}click?key={clickChannel.AccessKey}&type=click";

            return Ok(new { url = url });
        }

        [HttpGet]
        [Route("getLinks")]
        public IHttpActionResult GetLinks(int affiliateChannelId)
        {
            AffiliateChannel affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(affiliateChannelId);
            if (affiliateChannel == null)
            {
                return HttpBadRequest("Affiliate channel not found");
            }

            ClickChannel clickChannel = _clickService.GetClickChannelByAffiliateChannelId(affiliateChannelId, false);

            if (clickChannel == null)
            {
                object o = null;
                return Ok(o);
            }

            string clickLink = $"{Request.GetBaseUrl()}click?key={clickChannel.AccessKey}&type=click";
            string viewLink = $"{Request.GetBaseUrl()}click?key={clickChannel.AccessKey}&type=view";

            return Ok(new { 
                viewLink = viewLink,
                clickLink = clickLink
            });
        }

        [HttpGet]
        [Route("getClicksCount")]
        public IHttpActionResult GetClicksCount(int affiliateChannelId)
        {
            var clicksCount = _clickService.GetClicksCount(affiliateChannelId);

            return Ok(clicksCount);
        }

        #region MoveToServiceLayer
        private AffiliateChannelTreeItem AffiliateChannelTreeItem(long affiliateChannelId)
        {
            var rootTreeItem = new AffiliateChannelTreeItem
            {
                Title = "root",
                Folder = true,
                Expanded = true,
                TemplateField = "",
                DefaultValue = ""
            };

            var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(affiliateChannelId);

            if (affiliateChannel == null)
                return rootTreeItem;

            var affiliateChannelTemplates =
                ((List<AffiliateChannelTemplate>)_affiliateChannelTemplateService
                    .GetAllAffiliateChannelTemplatesByAffiliateChannelId(affiliateChannelId))
                .Where(x => x.SectionName == "root").ToList();
            foreach (var affiliateChannelTemplate in affiliateChannelTemplates)
            {
                var treeItem = new AffiliateChannelTreeItem
                {
                    Title = affiliateChannelTemplate.TemplateField,
                    Folder = true,
                    Expanded = true,
                    CampaignTemplateId = affiliateChannelTemplate.CampaignTemplateId,
                    DefaultValue = string.IsNullOrEmpty(affiliateChannelTemplate.DefaultValue)
                        ? ""
                        : affiliateChannelTemplate.DefaultValue
                };

                var campaignTemplate =
                    _campaignTemplateService.GetCampaignTemplateById(affiliateChannelTemplate.CampaignTemplateId);
                if (campaignTemplate != null && campaignTemplate.Validator == 13 &&
                    string.IsNullOrEmpty(treeItem.DefaultValue))
                {
                    treeItem.DefaultValue = affiliateChannel.ChannelKey;
                }

                LoadChildren(affiliateChannel, affiliateChannelTemplate.TemplateField, treeItem.Children);

                if (treeItem.Children == null || treeItem.Children.Count == 0)
                    treeItem.Folder = false;

                rootTreeItem.Children.Add(treeItem);
            }

            return rootTreeItem;
        }

        private void LoadChildren(AffiliateChannel affiliateChannel, string parent, ICollection<AffiliateChannelTreeItem> children)
        {
            var affiliateChannelTemplates = _affiliateChannelTemplateService.GetAllAffiliateChannelTemplatesByAffiliateChannelId(affiliateChannel.Id).Where(x => x.SectionName == parent).ToList();

            foreach (var node in affiliateChannelTemplates)
            {
                var treeItem = new AffiliateChannelTreeItem
                {
                    Title = node.TemplateField
                };
                LoadChildren(affiliateChannel, node.TemplateField, treeItem.Children);
                if (treeItem.Children.Count > 0)
                {
                    treeItem.Folder = true;
                    treeItem.Expanded = true;
                    treeItem.TemplateField = "";
                    treeItem.CampaignTemplateId = 0;
                    treeItem.DefaultValue = "";
                }
                else
                {
                    treeItem.Folder = false;
                    treeItem.Expanded = false;
                    treeItem.TemplateField = node.TemplateField;
                    treeItem.CampaignTemplateId = node.CampaignTemplateId;
                    treeItem.DefaultValue = string.IsNullOrEmpty(node.DefaultValue) ? "" : node.DefaultValue;
                }

                var campaignTemplate = _campaignTemplateService.GetCampaignTemplateById(node.CampaignTemplateId);
                if (campaignTemplate != null && campaignTemplate.Validator == 13 && string.IsNullOrEmpty(treeItem.DefaultValue))
                {
                    treeItem.DefaultValue = affiliateChannel.ChannelKey;
                }

                children.Add(treeItem);
            }
        }

        private AffiliateChannelViewModel CreateAffiliateChannelModel(AffiliateChannel channel)
        {
            var affiliate = _affiliateService.GetAffiliateById(channel.AffiliateId, false);
            Campaign campaign = null;
            if (channel.CampaignId != null)
            {
                campaign = _campaignService.GetCampaignById(channel.CampaignId.Value, false);
            }

            string note = channel.Note;
            if (!string.IsNullOrEmpty(note) && note.Length > 500)
                note = note.Substring(0, 500);

            var createdBy = string.Empty;
            var createdHistoryObj = _entityChangeHistoryService.GetEntityHistory(channel.Id, "AffiliateChannel", "Added");
            if (createdHistoryObj != null)
                createdBy = _userService.GetUserById(createdHistoryObj.UserId)?.Username;

            var updatedBy = string.Empty;
            var updatedHistoryObj = _entityChangeHistoryService.GetEntityHistory(channel.Id, "AffiliateChannel", "Modified");
            if (updatedHistoryObj != null)
                updatedBy = _userService.GetUserById(updatedHistoryObj.UserId)?.Username;

            DateTime? createDate = null;
            if (createdHistoryObj != null)
                createDate = _settingService.GetTimeZoneDate(createdHistoryObj.ModifiedDate);

            DateTime? updateDate = null;
            if (updatedHistoryObj != null)
                updateDate = _settingService.GetTimeZoneDate(updatedHistoryObj.ModifiedDate);

            return new AffiliateChannelViewModel
            {
                Id = channel.Id,
                CampaignId = channel.CampaignId,
                CampaignName = campaign?.Name,
                AffiliateId = channel.AffiliateId,
                AffiliateName = affiliate.Name,
                Name = channel.Name,
                Status = channel.Status,
                XmlTemplate = "",//channel.XmlTemplate,
                DataFormat = channel.DataFormat,
                MinPriceOption = channel.MinPriceOption,
                MinPriceOptionValue = channel.NetworkTargetRevenue,
                MinRevenue = channel.NetworkMinimumRevenue,
                AffiliateChannelKey = channel.ChannelKey,
                Deleted = channel.IsDeleted,
                AffiliatePriceMethod = channel.AffiliatePriceMethod,
                AffiliatePrice = channel.AffiliatePrice,
                Timeout = channel.Timeout,
                Note = note,
                IsIntegrated = channel.CampaignId.HasValue ? true : false,
                CreatedDate = createDate,
                CreatedBy = createdBy,
                UpdatedDate = updateDate,
                UpdatedBy = updatedBy
            };
        }

        #endregion
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
                xml = BuildXMLHierarchy(affiliateChannelTemplate, generateRandomValues, affiliateChannel);
                if (xml != null)
                    result = FormatXml(xml.ToString());
            }

            return result;
        }
        private string CorrectXML(string readyXml)
        {
            for (; ;)
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
            var arr = tagName.Substring(1,tagName.Count()-1).ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
        private string GenerateRandomText(string value)
        {
            var result = string.Empty;

            return result;
        }
        private XElement BuildXMLHierarchy(List<XMLTree> tree, bool generateRandomValues, AffiliateChannel affiliateChannel)
        {
            XElement root = null;
            foreach (var item in tree)
            {
                var value = string.Empty;
                if (!string.IsNullOrEmpty(item.TemplateField) && item.TemplateField.ToLower() == "password")
                    value = !string.IsNullOrWhiteSpace(item.AffiliateChannelPassword) ? item.AffiliateChannelPassword : string.Empty;
                else if (!string.IsNullOrEmpty(item.TemplateField) && item.TemplateField.ToLower() == "channelid")
                    value = !string.IsNullOrWhiteSpace(item.AffiliateChannelKey) ? item.AffiliateChannelKey : string.Empty;
                else if (generateRandomValues)
                {
                    value = GenerateRandomValue(item.Validator.HasValue ? (Validators)item.Validator.Value : Validators.None);
                }


                if (root != null)
                {
                    var attribute = root.Descendants().FirstOrDefault(x => x.Name == item.SectionName);
                    if (attribute == null && root.Name == item.SectionName)
                        attribute = root;

                    if (attribute != null)
                    {
                        attribute.Add(new XElement(item.TemplateField, value));
                    }
                    else
                    {
                        root.Add(new XElement(item.SectionName, new XElement(item.TemplateField, value)));
                    }
                }
                else if (!string.IsNullOrEmpty(item.TemplateField))
                {
                    root = (new XElement(item.SectionName, new XElement(item.TemplateField, value)));
                }
            }

            var channelId = root.Descendants().FirstOrDefault(x => x.Name == "CHANNELID");
            var password = root.Descendants().FirstOrDefault(x => x.Name == "PASSWORD");

            if (channelId == null)
            {
                root.Add(new XElement("CHANNELID", affiliateChannel.ChannelKey));
            }

            if (password == null)
            {
                root.Add(new XElement("PASSWORD", affiliateChannel.ChannelPassword));
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
                                                Description=ct.Description,
                                                Field = act.TemplateField,
                                                Format =(ac.DataFormat),
                                                Status = ac.Status,
                                                Type = (CampaignTypes)(ct.FieldType.HasValue?ct.FieldType.Value:0)
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
                        Type = string.Join(" ",Regex.Split(Enum.GetName(typeof(CampaignTypes), (item.Type)), @"(?<!^)(?=[A-Z])"))
                    });
                }
            }
            return result.ToList();
        }


        private bool ValidationListFilters(List<AffiliateChannelFilterCreateModel> filterModels)
        {
            try
            {
                var groups = filterModels.GroupBy(item => item.CampaignFieldId);
                foreach (var group in groups)
                {
                    //Console.WriteLine("List with FieldId == {0}", group.Key);
                    var g2 = group;
                    foreach (var item in group)
                    {
                        //Console.WriteLine("    Condition: {0}", item.Condition);
                        foreach (var item2 in g2)
                        {
                            if (item.Condition == (short)Conditions.Contains && item2.Condition == (short)Conditions.NotContains)
                            {
                                if (item.GetValue() == item2.GetValue())
                                {
                                    return false;
                                }
                            }
                            else if (item.Condition == (short)Conditions.Equals && item2.Condition == (short)Conditions.NotEquals)
                            {
                                if (item.GetValue() == item2.GetValue())
                                {
                                    return false;
                                }
                            }
                            else if (item.Condition == (short)Conditions.StringByLength && item2.Condition == (short)Conditions.StringByLength)
                            {
                                if (item.GetValue() != item2.GetValue())
                                {
                                    return false;
                                }
                            }
                            else if (item.Condition == (short)Conditions.Equals && item2.Condition == (short)Conditions.Equals)
                            {
                                if (item.GetValue() != item2.GetValue())
                                {
                                    return false;
                                }
                            }
                            else if (item.Condition == (short)Conditions.Equals &&
                                     (item2.Condition == (short)Conditions.NumberGreater ||
                                      item2.Condition == (short)Conditions.NumberLess ||
                                      item2.Condition == (short)Conditions.NumberGreaterOrEqual ||
                                      item2.Condition == (short)Conditions.NumberLessOrEqual ||
                                      item2.Condition == (short)Conditions.NumberRange
                                      )
                                     )
                            {
                                return false;
                            }
                            else if (item.Condition == (short)Conditions.NumberGreater && item2.Condition == (short)Conditions.NumberLess)
                            {
                                if (Convert.ToDecimal(item.GetValue()) >= Convert.ToDecimal(item2.GetValue()))
                                {
                                    return false;
                                }
                            }
                            else if (item.Condition == (short)Conditions.NumberGreaterOrEqual && item2.Condition == (short)Conditions.NumberLessOrEqual)
                            {
                                if (Convert.ToDecimal(item.GetValue()) > Convert.ToDecimal(item2.GetValue()))
                                {
                                    return false;
                                }
                            }
                            else if (item.Condition == (short)Conditions.NumberRange && item2.Condition == (short)Conditions.NumberRange)
                            {
                                if ((Convert.ToDecimal(item.Values[0].Value1) > Convert.ToDecimal(item2.Values[0].Value2)) ||
                                    (Convert.ToDecimal(item.Values[0].Value2) < Convert.ToDecimal(item2.Values[0].Value1))
                                    )
                                {
                                    return false;
                                }
                            }

                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
    public class XMLTree
    {
        public string TemplateField { get; set; }
        public string SectionName { get; set; }
        public short? Validator { get; set; }
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string AffiliateChannelPassword { get; set; }
        public string AffiliateChannelKey { get; set; }
    }
}