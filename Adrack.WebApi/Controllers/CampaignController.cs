using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Security;
using Adrack.Web.Framework.Security;
using Adrack.WebApi.Helpers;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Campaigns;
using Adrack.WebApi.Models.Common;
using Adrack.WebApi.Models.Lead;
using Adrack.WebApi.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Adrack.Web.Framework.Cache;
using Adrack.PlanManagement;
using Adrack.Service.Membership;
using Adrack.Core.Helpers;
using Adrack.Core.Domain.Lead.Reports;
using Adrack.Data;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Adrack.Service.Content;
using Adrack.Service.Configuration;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Security;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/campaign")]
    public class CampaignController : BaseApiController
    {
        #region fields

        private readonly ICampaignService _campaignService;
        private readonly ICampaignTemplateService _campaignTemplateService;
        private readonly IAffiliateChannelService _affiliateChannelService;
        private readonly IVerticalService _verticalService;
        private readonly IBuyerChannelService _buyerChannelService;
        private readonly IFilterService _filterService;
        private readonly ISearchService _searchService;
        private readonly IPermissionService _permissionService;
        private readonly IAppContext _appContext;
        private readonly IPlanService _planService;
        private readonly IReportService _reportService;
        private readonly ISettingService _settingService;


        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// The Entity Change History service
        /// </summary>
        private readonly IEntityChangeHistoryService _entityChangeHistoryService;

        protected PermoformanceCountersManager permoformanceCountersManager = new PermoformanceCountersManager();



        private static string _viewCampaignGeneralInfoKey { get; set; } = "view-general-info-campaign";
        private static string _editCampaignGeneralInfoKey { get; set; } = "edit-general-info-campaign";
        private static string _viewCampaignFiltersetsKey { get; set; } = "view-filtersets-campaign";
        private static string _viewCampaignIntegrationKey { get; set; } = "view-integration-campaign";
        private static string _editCampaignIntegrationKey { get; set; } = "edit-integration-campaign";
        #endregion

        #region constructors

        public CampaignController(ICampaignService campaignService,
                                  IVerticalService verticalService,
                                  ICampaignTemplateService campaignTemplateService,
                                  IAffiliateChannelService affiliateChannelService,
                                  IBuyerChannelService buyerChannelService,
                                  IFilterService filterService,
                                  ISearchService searchService,
                                  IPermissionService permissionService,
                                  IAppContext appContext,
                                  IPlanService planService,
                                  IReportService reportService,
                                  IUserRegistrationService userRegistrationService, IUserService userService,
                                  IEntityChangeHistoryService entityChangeHistoryService,
                                  ISettingService settingService)
        {
            _campaignService = campaignService;
            _campaignTemplateService = campaignTemplateService;
            _verticalService = verticalService;
            _affiliateChannelService = affiliateChannelService;
            _buyerChannelService = buyerChannelService;
            _filterService = filterService;
            _searchService = searchService;
            _permissionService = permissionService;
            _appContext = appContext;
            _planService = planService;
            _reportService = reportService;
            _userService = userService;
            _entityChangeHistoryService = entityChangeHistoryService;
            _settingService = settingService;
            permoformanceCountersManager.SetUpPerformanceCounters();
        }

        #endregion

        #region route methods

        /// <summary>
        /// Get All Campaigns with fields
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Route("getCampaignList/{status?}")]
        public IHttpActionResult GetCampaignList(ActivityStatuses status = ActivityStatuses.Active)
        {
            if (!_permissionService.Authorize(_viewCampaignGeneralInfoKey))
            {
                return HttpBadRequest("access-denied");
            }
            //var campaigns = _campaignService.GetAllCampaignsByStatus(status:(short)status);
            //var verticals = _verticalService.GetAllVerticals();
            var result = _campaignService.GetCampaignListWithVerticals(status).OrderByDescending(x => x.CampaignId).ToList();

            foreach(var res in result)
            {
                /*
                List<ReportByDays> Report = (List<ReportByDays>)_reportService.ReportByDays(new DateTime(2016, 1, 1), DateTime.Now, res.Id, 0);
                foreach (ReportByDays r in Report)
                {
                    res.Profit += r.Profit;
                    res.Cost += r.BuyerPrice;
                    res.Revenue += r.BuyerPrice - r.AffiliatePrice;
                }
                */

                
                var createdBy = string.Empty;
                var createdHistoryObj = _entityChangeHistoryService.GetEntityHistory(res.CampaignId, "Campaign", "Added");
                if (createdHistoryObj != null)
                    createdBy = _userService.GetUserById(createdHistoryObj.UserId)?.Username;

                var updatedBy = string.Empty;
                var updatedHistoryObj = _entityChangeHistoryService.GetEntityHistory(res.CampaignId, "Campaign", "Modified");
                //if (updatedHistoryObj != null)
                //    updatedBy = _userService.GetUserById(updatedHistoryObj.UserId)?.Username;

                DateTime? createDate = null;
                if (createdHistoryObj != null)
                    createDate = _settingService.GetTimeZoneDate(createdHistoryObj.ModifiedDate);

                DateTime? updateDate = null;
                if (updatedHistoryObj != null)
                    updateDate = _settingService.GetTimeZoneDate(updatedHistoryObj.ModifiedDate);

                res.CreatedDate = createDate;
                res.CreatedBy = createdBy;
                res.UpdatedDate = updateDate;
                //res.UpdatedBy = updatedBy;
                
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("getConnectionString")]
        public IHttpActionResult GetConnectionString()
        {
            /*string s = Arack.Encryption.ADLEncryptionManager.Encrypt("1122");

            s = Arack.Encryption.ADLEncryptionManager.Decrypt(s);

            s = Arack.Encryption.ADLEncryptionManager.Encrypt("11223");
            s = Arack.Encryption.ADLEncryptionManager.Encrypt("1122");*/

            return Ok(_campaignService.GetConnectionString());
        }

        [HttpGet]
        [Route("getAllKeys")]
        public IHttpActionResult GetAllKeys()
        {
            var keys = _campaignService.GetAllKeys();
            keys.Insert(0, WebHelper.GetSubdomain());
            return Ok(keys);
        }

        /// <summary>
        /// Clone Campaign
        /// </summary>
        /// <param name="inpObj">CloneCampaignInpModel</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Route("cloneCampaign")]
        public IHttpActionResult CloneCampaign([FromBody] CloneCampaignInpModel inpObj)
        {
            if (!_permissionService.Authorize(_viewCampaignGeneralInfoKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var campaign = _campaignService.GetCampaignById(inpObj.CurrentCampaignId);

                if (campaign == null)
                {
                    return HttpBadRequest($"no campaign was found for given id {inpObj.CurrentCampaignId}");
                }

                var existedCampaign = _campaignService.GetCampaignByName(inpObj.NewCampaignName, 0);
                if (existedCampaign != null)
                {
                    return HttpBadRequest($"campaign already exist for given name {inpObj.NewCampaignName}");
                }

                Campaign cloneCampaign = new Campaign()
                {
                    Id = 0,
                    Name = inpObj.NewCampaignName,
                    Status = campaign.Status,
                    DataTemplate = campaign.DataTemplate,
                    CampaignKey = campaign.CampaignKey,
                    CampaignType = campaign.CampaignType,
                    CreatedOn = campaign.CreatedOn,
                    IsDeleted = campaign.IsDeleted,
                    Description = campaign.Description,
                    Finish = campaign.Finish,
                    HtmlFormId = campaign.HtmlFormId,
                    IsTemplate = campaign.IsTemplate,
                    NetworkMinimumRevenue = campaign.NetworkMinimumRevenue,
                    NetworkTargetRevenue = campaign.NetworkTargetRevenue,
                    PingTreeCycle = campaign.PingTreeCycle,
                    PriceFormat = campaign.PriceFormat,
                    PrioritizedEnabled = campaign.PrioritizedEnabled,
                    Start = campaign.Start,
                    VerticalId = campaign.VerticalId,
                    Visibility = campaign.Visibility
                };

                var cloneCampaignId = _campaignService.InsertCampaign(cloneCampaign);

                if (campaign.CampaignFields != null && campaign.CampaignFields.Any())
                {
                    cloneCampaign.CampaignFields = new List<CampaignField>();

                    foreach (var field in campaign.CampaignFields)
                    {
                        var cloneFields = new CampaignField()
                        {
                            Id = 0,
                            CampaignId = cloneCampaignId,
                            Description = field.Description,
                            BlackListTypeId = field.BlackListTypeId,
                            ColumnNumber = field.ColumnNumber,
                            DatabaseField = field.DatabaseField,
                            FieldFilterSettings = field.FieldFilterSettings,
                            FieldType = field.FieldType,
                            IsFilterable = field.IsFilterable,
                            IsFormField = field.IsFormField,
                            IsHash = field.IsHash,
                            IsHidden = field.IsHidden,
                            Label = field.Label,
                            MaxLength = field.MaxLength,
                            MinLength = field.MinLength,
                            OptionValues = field.OptionValues,
                            PageNumber = field.PageNumber,
                            PossibleValue = field.PossibleValue,
                            Required = field.Required,
                            SectionName = field.SectionName,
                            TemplateField = field.TemplateField,
                            Validator = field.Validator,
                            ValidatorSettings = field.ValidatorValue

                        };

                        cloneCampaign.CampaignFields.Add((CampaignField) cloneFields);
                    }

                    _campaignService.UpdateCampaign(cloneCampaign);
                }

                return Ok(cloneCampaign);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Campaign By Id
        /// </summary>
        /// <param name="id">long</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Route("getCampaignById/{id}")]
        public IHttpActionResult GetCampaignById(long id)
        {
            if (!_permissionService.Authorize(_viewCampaignGeneralInfoKey))
            {
                return HttpBadRequest("access-denied");
            }
            var campaign = _campaignService.GetCampaignById(id);

            if (campaign == null)
            {
                return HttpBadRequest($"no campaign was found for given id {id}");
            }

            return Ok(campaign);
        }

        /// <summary>
        /// Insert Campaign With Fields
        /// </summary>
        /// <param name="campaignModel">CampaignModel</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Route("addCampaign")]
        public IHttpActionResult InsertCampaign([FromBody]AddCampaignModel campaignModel)
        {
            if (!_permissionService.Authorize(_editCampaignGeneralInfoKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                if (campaignModel == null)
                {
                    return HttpBadRequest($"campaign model is null");
                }

                if (!ModelState.IsValid)
                {
                    return HttpBadRequest(ModelState.GetErrorMessage());
                }

                var  planLimitation = _planService.CheckPlanStatusesByUserId(_appContext.AppUser.Id);
                if(planLimitation != null && planLimitation.Contains(AdrackPlanVerificationStatus.CampaignLimitReached)) {
                    return HttpBadRequest($"campaign limit reached.");
                }

                var existedCampaign = _campaignService.GetCampaignByName(campaignModel.Name, 0);
                if (existedCampaign != null)
                {
                    return HttpBadRequest($"campaign already exist for given name {campaignModel.Name}");
                }

                var vertical = _verticalService.GetVerticalById(campaignModel.VerticalId);
                if (vertical == null)
                {
                    return HttpBadRequest($"vertical does not exist");
                }

                var campaign = (Campaign)campaignModel;
                _campaignService.InsertCampaign(campaign);

                var access = new EntityOwnership
                {
                    Id = 0,
                    UserId = _appContext.AppUser.Id,
                    EntityId = campaign.Id,
                    EntityName = EntityType.Campaign.ToString()
                };
                _userService.InsertEntityOwnership(access);

                var pingTrees = _campaignService.GetPingTrees(campaign.Id);
                if (pingTrees.Count == 0)
                {
                    var pingTree = new PingTree()
                    {
                        CampaignId = campaign.Id,
                        Name = "Main",
                        Quantity = int.MaxValue
                    };

                    _campaignService.InsertPingTree(pingTree);
                }

                if (campaignModel.FilterSets != null)
                {
                    foreach (var filterSetModel in campaignModel.FilterSets)
                    {
                        Filter filterSet = (Filter)filterSetModel;
                        //filterSet.FilterConditions.Clear();

                        filterSet.CampaignId = campaign.Id;

                        _filterService.InsertFilter(filterSet);
                        _filterService.DeleteFilterConditions(filterSet.Id);

                        foreach (var conditionModel in filterSetModel.Conditions)
                        {
                            FilterCondition filterCondition = (FilterCondition)conditionModel;
                            filterCondition.FilterId = filterSet.Id;

                            if (!string.IsNullOrEmpty(conditionModel.CampaignFieldName))
                            {
                                var campaignTemplate = campaign.CampaignFields.Where(x => x.TemplateField == conditionModel.CampaignFieldName).FirstOrDefault();
                                if (campaignTemplate != null)
                                {
                                    filterCondition.CampaignTemplateId = campaignTemplate.Id;
                                }
                            }

                            _filterService.InsertFilterCondition(filterCondition);

                            foreach (var subConditionModel in conditionModel.Children)
                            {
                                var filterSetSubCondition = (FilterCondition)subConditionModel;
                                filterSetSubCondition.ParentId = filterCondition.Id;
                                filterSetSubCondition.FilterId = filterSet.Id;

                                if (!string.IsNullOrEmpty(subConditionModel.CampaignFieldName))
                                {
                                    var campaignTemplate = campaign.CampaignFields.Where(x => x.TemplateField == subConditionModel.CampaignFieldName).FirstOrDefault();
                                    if (campaignTemplate != null)
                                    {
                                        filterSetSubCondition.CampaignTemplateId = campaignTemplate.Id;
                                    }
                                }

                                _filterService.InsertFilterCondition(filterSetSubCondition);
                            }
                        }
                    }
                }

                if (campaignModel.CampaignFields != null)
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    if (string.IsNullOrEmpty(campaignModel.XmlTemplate))
                    {
                        xmlDocument.LoadXml("<request></request>");
                    }
                    else
                    {
                        xmlDocument.LoadXml(campaignModel.XmlTemplate);
                    }

                    Utils.ClearXmlElements(xmlDocument, campaignModel.CampaignFields.Select(x => x.TemplateField + "," + x.SectionName).ToList());

                    foreach (var field in campaignModel.CampaignFields)
                    {
                        Utils.AddXmlElement(xmlDocument, field.SectionName, field.TemplateField);
                    }
                    campaign.DataTemplate = xmlDocument.OuterXml;
                }

                //_campaignService.UpdateCampaign(campaign);

                return Ok(campaignModel);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Insert Campaign With Fields
        /// </summary>
        /// <param name="campaignModel">CampaignModel</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Route("validateNewCampaign")]
        public IHttpActionResult ValidateNewCampaign([FromBody]AddCampaignModel campaignModel)
        {
            if (!_permissionService.Authorize(_editCampaignGeneralInfoKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                if (campaignModel == null)
                {
                    return HttpBadRequest($"campaign model is null");
                }

                if (!ModelState.IsValid)
                {
                    return HttpBadRequest(ModelState.GetErrorMessage());
                }

                var planLimitation = _planService.CheckPlanStatusesByUserId(_appContext.AppUser.Id);
                if (planLimitation != null && planLimitation.Contains(AdrackPlanVerificationStatus.CampaignLimitReached))
                {
                    return HttpBadRequest($"campaign limit reached.");
                }

                var existedCampaign = _campaignService.GetCampaignByName(campaignModel.Name, 0);
                if (existedCampaign != null)
                {
                    return HttpBadRequest($"campaign already exist for given name {campaignModel.Name}");
                }

                var vertical = _verticalService.GetVerticalById(campaignModel.VerticalId);
                if (vertical == null)
                {
                    return HttpBadRequest($"vertical does not exist");
                }
                return Ok();
            }
            
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Update Campaign With Fields
        /// </summary>
        /// <param name="id">long</param>
        /// <param name="campaignModel">CampaignModel</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Route("updateCampaign/{id}")]
        public IHttpActionResult UpdateCampaign(long id, [FromBody]CampaignModel campaignModel)
        {
            if (!_permissionService.Authorize(_editCampaignGeneralInfoKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var campaign = _campaignService.GetCampaignById(id);

                if (campaign == null)
                {
                    return HttpBadRequest($"no campaign was found for given id {id}");
                }

                if (campaignModel == null)
                {
                    return HttpBadRequest($"campaign model is null for given id {id}");
                }

                if (campaign.Name != campaignModel.Name)
                {
                    var existedCampaign = _campaignService.GetCampaignByName(campaignModel.Name, campaign.Id);
                    if (existedCampaign != null)
                    {
                        return HttpBadRequest($"campaign already exist for given name {campaignModel.Name}");
                    }
                }

                campaign.CampaignKey = campaignModel.CampaignKey;
                campaign.CampaignType = campaignModel.CampaignType;
                campaign.Description = campaignModel.Description;
                campaign.Finish = DateTime.UtcNow;
                campaign.HtmlFormId = null;
                campaign.Name = campaignModel.Name;
                campaign.NetworkMinimumRevenue = campaignModel.NetworkMinimumRevenue;
                campaign.NetworkTargetRevenue = campaignModel.NetworkTargetRevenue;
                campaign.PingTreeCycle = campaignModel.PingTreeCycle;
                campaign.PriceFormat = 0;
                campaign.PrioritizedEnabled = campaignModel.PrioritizedEnabled;
                campaign.Start = DateTime.UtcNow;
                campaign.VerticalId = campaignModel.VerticalId;
                campaign.Visibility = 0;
                campaign.DataTemplate = campaignModel.XmlTemplate;
                campaign.Status = campaignModel.Status;
                //campaign.CampaignFields = new List<CampaignTemplate>();

                //_campaignService.DeleteCampaignTemplates(campaign.Id);

                _campaignService.UpdateCampaign(campaign);

                List<long> editedIds = new List<long>();

                if (campaignModel.CampaignFields != null)
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    try
                    {
                        xmlDocument.LoadXml(campaign.DataTemplate);
                    }
                    catch
                    {
                        xmlDocument.LoadXml("<request></request>");
                    }

                    Utils.ClearXmlElements(xmlDocument, campaignModel.CampaignFields.Select(x => x.TemplateField + "," + x.SectionName).ToList());

                    var currentCampaignFields = _campaignTemplateService.GetCampaignTemplatesByCampaignId(campaign.Id);

                    foreach (var campaignFieldModel in campaignModel.CampaignFields)
                    {
                        var existedField = _campaignTemplateService.GetCampaignTemplateBySectionAndName(campaignFieldModel.TemplateField, campaignFieldModel.SectionName,0);
                        if (existedField != null)
                        {
                            return HttpBadRequest($"campaign field already exist for given field name {campaignFieldModel.TemplateField} and section name {campaignFieldModel.SectionName}");
                        }

                        var campaignField = (CampaignField)campaignFieldModel;
                        var campaignFieldToEdit = _campaignTemplateService.GetCampaignTemplateById(campaignField.Id);

                        if (campaignFieldToEdit == null)
                        {
                            //campaign.CampaignFields.Add(campaignField);
                            _campaignTemplateService.InsertCampaignTemplate(campaignField);
                        }
                        else
                        {
                            editedIds.Add(campaignFieldToEdit.Id);
                            campaignFieldToEdit.BlackListTypeId = campaignField.BlackListTypeId;
                            campaignFieldToEdit.DatabaseField = campaignField.DatabaseField;
                            campaignFieldToEdit.Description = campaignField.Description;
                            campaignFieldToEdit.FieldFilterSettings = campaignField.FieldFilterSettings;
                            campaignFieldToEdit.FieldType = campaignField.FieldType;
                            campaignFieldToEdit.IsFilterable = campaignField.IsFilterable;
                            campaignFieldToEdit.IsHash = campaignField.IsHash;
                            campaignFieldToEdit.IsHidden = campaignField.IsHidden;
                            campaignFieldToEdit.MaxLength = campaignField.MaxLength;
                            campaignFieldToEdit.MinLength = campaignField.MinLength;
                            campaignFieldToEdit.OptionValues = campaignField.OptionValues;
                            campaignFieldToEdit.PossibleValue = campaignField.PossibleValue;
                            campaignFieldToEdit.Required = campaignField.Required;
                            campaignFieldToEdit.SectionName = campaignField.SectionName;
                            campaignFieldToEdit.TemplateField = campaignField.TemplateField;
                            campaignFieldToEdit.Validator = campaignField.Validator;
                            campaignFieldToEdit.ValidatorSettings = campaignField.ValidatorSettings;
                            _campaignTemplateService.UpdateCampaignTemplate(campaignFieldToEdit);
                        }

                        Utils.AddXmlElement(xmlDocument, campaignField.SectionName, campaignField.TemplateField);
                    }

                    campaign.DataTemplate = xmlDocument.OuterXml;

                    foreach (var campaignField in currentCampaignFields.Where(x => !editedIds.Contains(x.Id)).ToList())
                    {
                        var filterSets = _filterService.GetFiltersByCampaignId(campaign.Id);

                        foreach (var filterSet in filterSets)
                        {
                            var filterConditions = _filterService.GetFilterConditionsByCampaignFieldId(filterSet.Id, campaignField.Id);

                            if (filterConditions.Count > 0)
                            {
                                return HttpBadRequest($"Campaign field \"{campaignField.TemplateField}\" can not be deleted. There is a filter set condition attached.");
                            }
                        }

                        _campaignTemplateService.DeleteCampaignTemplate(_campaignTemplateService.GetCampaignTemplateById(campaignField.Id));

                        //campaign.CampaignFields.Remove(campaignField);
                    }
                }

                return Ok(campaign);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("activateCampaign/{id}/{status}")]
        public IHttpActionResult ActivateCampaign(long id, ActivityStatuses status)
        {
            if (!_permissionService.Authorize(_editCampaignGeneralInfoKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var campaign = _campaignService.GetCampaignById(id);

                if (campaign == null)
                {
                    return HttpBadRequest($"no affiliate channel was found for given id {id}");
                }

                campaign.Status = status;
                _campaignService.UpdateCampaign(campaign);

                return Ok(campaign);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Delete Campaign If Not Contains Any Field
        /// </summary>
        /// <param name="id">long</param>
        /// <returns>ActionResult</returns>
        [HttpDelete]
        [Route("deleteCampaign/{id}")]
        public IHttpActionResult DeleteCampaign(long id)
        {
            if (!_permissionService.Authorize(_editCampaignGeneralInfoKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var campaign = _campaignService.GetCampaignById(id);

                if (campaign == null)
                {
                    return HttpBadRequest($"no campaign was found for given id {id}");
                }

                campaign.IsDeleted = true;
                _campaignService.UpdateCampaign(campaign);
                return Ok(campaign);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }

            /*
            if (!_permissionService.Authorize(_editCampaignGeneralInfoKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var campaign = _campaignService.GetCampaignById(id);

                if (campaign == null)
                {
                    return HttpBadRequest($"no campaign was found for given id {id}");
                }

                var validationMessage = CheckCanDelete(campaign);
                if (!string.IsNullOrEmpty(validationMessage))
                {
                    return HttpBadRequest(validationMessage);
                }

                _campaignService.DeleteCampaignTemplates(campaign.Id);

                _campaignService.DeleteCampaign(campaign);
                return Ok(campaign);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
            */
        }

        /// <summary>
        /// Soft Delete Campaign
        /// </summary>
        /// <param name="id">long</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Route("softDeleteCampaign/{id}")]
        public IHttpActionResult SoftDeleteCampaign(long id)
        {
            if (!_permissionService.Authorize(_editCampaignGeneralInfoKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var campaign = _campaignService.GetCampaignById(id);

                if (campaign == null)
                {
                    return HttpBadRequest($"no campaign was found for given id {id}");
                }

                campaign.IsDeleted = true;
                _campaignService.UpdateCampaign(campaign);
                return Ok(campaign);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Campaings By Vertical Id
        /// </summary>
        /// <param name="id">long</param>
        /// <returns>IHttpActionResult</returns>
        [HttpGet]
        [Route("getCampaignsByVerticalId/{id}")]
        [ContentManagementCache("App.Cache.Campaign.")]
        public IHttpActionResult GetCampaignsByVerticalId(long id)
        {
            if (!_permissionService.Authorize(_viewCampaignGeneralInfoKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var campaigns = _campaignService.GetCampaignsByVerticalId(id);
                return Ok(campaigns);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Campaigns By Vertical Which Have Fields
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getCampaignFieldsByCampaignId/{campaignId}")]
        public IHttpActionResult GetCampaignTemplatesByCampaignId(long campaignId)
        {
            if (!_permissionService.Authorize(_viewCampaignGeneralInfoKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var campaignFields = GetCampaignFields(campaignId);
                var campaignFieldModels = new List<CampaignFieldModel>();

                foreach(var campaignField in campaignFields)
                {
                    CampaignFieldModel campaignFieldModel = new CampaignFieldModel();

                    campaignFieldModel.CampaignId = campaignId;
                    campaignFieldModel.DatabaseField = campaignField.DatabaseField;
                    campaignFieldModel.Description = campaignField.Description;
                    campaignFieldModel.IsFilterable = campaignField.IsFilterable;
                    campaignFieldModel.IsHash = campaignField.IsHash;
                    campaignFieldModel.MaxLength = campaignField.MaxLength;
                    campaignFieldModel.MinLength = campaignField.MinLength;
                    campaignFieldModel.PossibleValue = campaignField.PossibleValue;
                    campaignFieldModel.Required = campaignField.Required;
                    campaignFieldModel.SectionName = campaignField.SectionName;
                    campaignFieldModel.TemplateField = campaignField.TemplateField;
                    campaignFieldModel.Validator = campaignField.Validator;
                    campaignFieldModel.ValidatorValue = campaignField.ValidatorSettings;

                    campaignFieldModels.Add(campaignFieldModel);
                }

                return Ok(campaignFieldModels);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Campaigns By Vertical Which Have Fields
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getFilterableCampaignTemplatesByCampaignId/{campaignId}")]
        public IHttpActionResult GetFilterableCampaignTemplatesByCampaignId(long campaignId)
        {
            if (!_permissionService.Authorize(_viewCampaignFiltersetsKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var campaignFields = GetCampaignFields(campaignId, true);
                var campaignFieldModels = new List<CampaignFieldModel>();

                foreach (var campaignField in campaignFields)
                {
                    CampaignFieldModel campaignFieldModel = new CampaignFieldModel();

                    campaignFieldModel.CampaignId = campaignId;
                    campaignFieldModel.DatabaseField = campaignField.DatabaseField;
                    campaignFieldModel.Description = campaignField.Description;
                    campaignFieldModel.IsFilterable = campaignField.IsFilterable;
                    campaignFieldModel.IsHash = campaignField.IsHash;
                    campaignFieldModel.MaxLength = campaignField.MaxLength;
                    campaignFieldModel.MinLength = campaignField.MinLength;
                    campaignFieldModel.PossibleValue = campaignField.PossibleValue;
                    campaignFieldModel.Required = campaignField.Required;
                    campaignFieldModel.SectionName = campaignField.SectionName;
                    campaignFieldModel.TemplateField = campaignField.TemplateField;
                    campaignFieldModel.Validator = campaignField.Validator;
                    campaignFieldModel.ValidatorValue = campaignField.ValidatorSettings;

                    campaignFieldModels.Add(campaignFieldModel);
                }

                return Ok(campaignFieldModels);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Current Campaign Fields
        /// </summary>
        /// <param name="id">long</param>
        /// <returns>IHttpActionResult</returns>
        [HttpGet]
        [Route("getCampaignFieldListByCampaignId/{id}")]
        public IHttpActionResult GetCampaignFieldListByCampaignId(long id)
        {
            if (!_permissionService.Authorize(_viewCampaignFiltersetsKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var campaignFields = _campaignTemplateService.GetCampaignTemplatesByCampaignId(id);
                campaignFields = campaignFields.OrderBy(x => x.TemplateField).ToList();
                campaignFields.Insert(0, new CampaignField() { 
                    CampaignId = id,
                    DatabaseField = "NONE",
                    SectionName = "",
                    TemplateField = "[NONE]",
                    Id = 0,
                    Validator = 0                    
                });
                return Ok(campaignFields);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Campaign Field By Id
        /// </summary>
        /// <param name="id">long</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Route("getCampaignFieldById/{id}")]
        public IHttpActionResult GetCampaignFieldById(long id)
        {
            if (!_permissionService.Authorize(_viewCampaignIntegrationKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var campaign = _campaignTemplateService.GetCampaignTemplateById(id);

                if (campaign == null)
                {
                    return HttpBadRequest($"no campaign field was found for given id {id}");
                }

                return Ok(campaign);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Insert Campaign Field
        /// </summary>
        /// <param name="campaignFieldModel">CampaignFieldModel</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Route("insertCampaignField")]
        public IHttpActionResult InsertCampaignField([FromBody]CampaignFieldModel campaignFieldModel)
        {
            if (!_permissionService.Authorize(_editCampaignIntegrationKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                if (campaignFieldModel == null)
                {
                    return HttpBadRequest($"campaign field model is null");
                }

                var existedField = _campaignTemplateService.GetCampaignTemplateBySectionAndName(campaignFieldModel.TemplateField, campaignFieldModel.SectionName, 0);
                if (existedField != null)
                {
                    return HttpBadRequest($"campaign field already exist for given field name {campaignFieldModel.TemplateField} and section name {campaignFieldModel.SectionName}");
                }

                var campaign = _campaignService.GetCampaignById(campaignFieldModel.CampaignId);

                var campaignField = (CampaignField)campaignFieldModel;
                _campaignTemplateService.InsertCampaignTemplate(campaignField);

                XmlDocument xmlDocument = new XmlDocument();
                try
                {
                    xmlDocument.LoadXml(campaign.DataTemplate);
                }
                catch
                {
                    xmlDocument.LoadXml("<request></request>");
                }
                Utils.AddXmlElement(xmlDocument, campaignField.SectionName, campaignField.TemplateField);
                campaign.DataTemplate = xmlDocument.OuterXml;
                _campaignService.UpdateCampaign(campaign);

                return Ok(campaignField);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Campaign Field
        /// </summary>
        /// <param name="id">long</param>
        /// <param name="campaignIntegrationModel">CampaignIntegrationModel</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Route("updateCampaignFieldById/{id}")]
        public IHttpActionResult UpdateCampaignFieldById(long id, [FromBody]CampaignIntegrationModel campaignIntegrationModel)
        {
            if (!_permissionService.Authorize(_editCampaignIntegrationKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var field = _campaignTemplateService.GetCampaignTemplateById(id);

                if (field == null)
                {
                    return HttpBadRequest($"no campaign field was found for given id {id}");
                }

                if (campaignIntegrationModel == null)
                {
                    return HttpBadRequest($"campaign field model is null for given id {id}");
                }

                var existedField = _campaignTemplateService.GetCampaignTemplateBySectionAndName(campaignIntegrationModel.CampaignField, campaignIntegrationModel.SectionName, 0);
                if (existedField != null)
                {
                    return HttpBadRequest($"campaign field already exist for given field name {campaignIntegrationModel.CampaignField} and section name {campaignIntegrationModel.SectionName}");
                }

                field.TemplateField = campaignIntegrationModel.CampaignField;
                field.DatabaseField = campaignIntegrationModel.SystemField;
                field.Validator = campaignIntegrationModel.DataType;
                field.Description = campaignIntegrationModel.Description;
                field.PossibleValue = campaignIntegrationModel.PossibleValue;
                field.Required = campaignIntegrationModel.IsRequired;
                field.IsHash = campaignIntegrationModel.IsHash;
                field.IsFilterable = campaignIntegrationModel.IsFilterable;

                _campaignTemplateService.UpdateCampaignTemplate(field);

                var campaign = _campaignService.GetCampaignById(field.CampaignId);
                XmlDocument xmlDocument = new XmlDocument();
                try
                {
                    xmlDocument.LoadXml(campaign.DataTemplate);
                }
                catch
                {
                    xmlDocument.LoadXml("<request></request>");
                }
                Utils.AddXmlElement(xmlDocument, field.SectionName, field.TemplateField);
                campaign.DataTemplate = xmlDocument.OuterXml;

                return Ok(field);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Campaign Field
        /// </summary>
        /// <param name="id">long</param>
        /// <returns>ActionResult</returns>
        [HttpDelete]
        [Route("deleteCampaignFieldById/{id}")]
        public IHttpActionResult DeleteCampaignFieldById(long id)
        {
            if (!_permissionService.Authorize(_editCampaignIntegrationKey))
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var field = _campaignTemplateService.GetCampaignTemplateById(id);

                if (field == null)
                {
                    return HttpBadRequest($"no campaign field was found for given id {id}");
                }

                _campaignTemplateService.DeleteCampaignTemplate(field);
                return Ok(field);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getCampaignBySearchPattern")]
        public IHttpActionResult GetCampaignBySearchPattern(string inputValue)
        {
            if (!_permissionService.Authorize(_viewCampaignGeneralInfoKey))
            {
                return HttpBadRequest("access-denied");
            }
            var campaignViewModels = new List<CampaignListModel>();
            var campaigns = _campaignService.GetAllCampaigns();
            var verticals = _verticalService.GetAllVerticals();
            foreach (var campaign in campaigns)
            {
                var campaignModel = new CampaignListModel()
                {
                    CampaignName = campaign.Name,
                    CampaignId = campaign.Id,
                    Vertical = verticals.FirstOrDefault(x => x.Id == campaign.VerticalId)?.Name,
                    VerticalId = campaign.VerticalId,
                    Revenue = 0,
                    Cost = 0,
                    Profit = 0,
                    Status = campaign.Status,
                };
                if (_searchService.CheckPropValue(campaignModel, inputValue))
                {
                    campaignViewModels.Add(campaignModel);
                }
            }
            return Ok(campaignViewModels);
        }

        #endregion route methods

        #region private methods

        private string CheckCanDelete(Campaign campaign)
        {
            var buyerChannels = _buyerChannelService.GetAllBuyerChannelsByCampaignId(campaign.Id, -1);

            if (buyerChannels != null && buyerChannels.Any())
            {
                return "can not delete campaign because there are active buyer channels";
            }

            var affiliateChannels = _affiliateChannelService.GetAllAffiliateChannelsByCampaignId(campaign.Id, -1);

            if (affiliateChannels != null && affiliateChannels.Any())
            {
                return "can not delete campaign because there are active affiliate channels";
            }

            return "";
        }

        #endregion

        #region campaignField methods
        /// <summary>
        /// Get CampaignField List by XML, JSON, CSV
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Route("getCampaignFieldList")]
        public IHttpActionResult GetCampaignFieldList([FromBody] InpCampaignFieldModel inpModel)
        {
            List<CampaignFieldModel> campaignField = new List<CampaignFieldModel>();

            try
            {
                XmlDocument xmlDoc;
                string inpStr = inpModel.InpValue.Trim();

                if (inpStr.StartsWith("<") && inpStr.EndsWith(">"))
                {
                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(inpStr);
                }
                else if (inpStr.StartsWith("{") && inpStr.EndsWith("}") || inpStr.StartsWith("[") && inpStr.EndsWith("]"))
                {
                    inpStr = "{'jsonObj':[" + inpStr + "]}";
                    xmlDoc = JsonConvert.DeserializeXmlNode(inpStr);
                }
                else if (inpStr.Split(';').Length > 1)
                {
                    string strCsv = string.Empty;
                    var headers = inpStr.Split(';');
                    foreach (var header in headers)
                    {
                        if (!string.IsNullOrEmpty(header))
                        {
                            strCsv += "<" + header + ">1</" + header + ">";
                        }
                    }

                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml("<csvObj>" + strCsv + "</csvObj>");
                }
                else
                {
                    return HttpBadRequest("The source file must be in XML or JSON or CSV or QUERYSTRING format");
                }


                var nodes = xmlDoc.SelectNodes("//*");
                if (nodes != null)
                {
                    var rnd = new Random();

                    foreach (XmlNode node in nodes)
                    {
                        if (!string.IsNullOrEmpty(node.FirstChild.Value))
                        {
                            var field = new CampaignFieldModel()
                            {
                                TemplateField = node.Name,
                                SectionName = (node.ParentNode != null) ? node.ParentNode.Name : string.Empty
                            };

                            if (campaignField.Exists(x => x.TemplateField == field.TemplateField && x.SectionName == field.SectionName))
                            {
                                field.SectionName += rnd.Next(1000, 9999).ToString();
                            }

                            campaignField.Add(field);
                        }
                    }
                }

                return Ok(campaignField);
            }
            catch (Exception e)
            {
                return HttpBadRequest(e.InnerException?.Message ?? e.Message);
            }
        }

        #endregion

        #region Private methods

        private List<CampaignField> GetCampaignFields(long campaignId, bool? isFilterable = null)
        {
            var campaign = _campaignService.GetCampaignById(campaignId);
            if (isFilterable.HasValue)
            {
                return campaign.CampaignFields.Where(x => x.IsFilterable == isFilterable.Value).ToList();
            }

            return campaign.CampaignFields.ToList();
        }

        #endregion

    }
}