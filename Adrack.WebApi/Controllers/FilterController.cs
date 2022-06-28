// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="FilterController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Data;
using Adrack.Service.Content;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Web.Framework.Security;
using Adrack.WebApi.Controllers;
using Adrack.WebApi.Models.Campaigns;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;


namespace Adrack.Web.ContentManagement.Controllers
{
    [RoutePrefix("api/filter")]
    public partial class FilterController : BaseApiController
    {
        #region Fields

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IFilterService _filterService;

        /// <summary>
        /// The campaign service
        /// </summary>
        private readonly ICampaignService _campaignService;

        /// <summary>
        /// The vertical service
        /// </summary>
        private readonly IVerticalService _verticalService;

        /// <summary>
        /// The campaign template service
        /// </summary>
        private readonly ICampaignTemplateService _campaignTemplateService;

        /// <summary>
        /// The localized string service
        /// </summary>
        private readonly ILocalizedStringService _localizedStringService;

        /// <summary>
        /// The history service
        /// </summary>
        private readonly IHistoryService _historyService;

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Directory Controller
        /// </summary>
        /// <param name="filterService">The filter service.</param>
        /// <param name="campaignService">The campaign service.</param>
        /// <param name="campaignTemplateService">The campaign template service.</param>
        /// <param name="verticalService">The vertical service.</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="historyService">The history service.</param>
        /// <param name="appContext">Application Context</param>
        public FilterController(IFilterService filterService, ICampaignService campaignService, ICampaignTemplateService campaignTemplateService, IVerticalService verticalService, ILocalizedStringService localizedStringService, IHistoryService historyService, IAppContext appContext)
        {
            this._filterService = filterService;
            this._localizedStringService = localizedStringService;
            this._campaignService = campaignService;
            this._campaignTemplateService = campaignTemplateService;
            this._verticalService = verticalService;
            this._historyService = historyService;
            this._appContext = appContext;
        }

        #endregion Constructor


        [HttpGet]
        [Route("getFilterSetById/{id}")]

        public IHttpActionResult GetFilterSetById(long id)
        {
            var filter = _filterService.GetFilterById(id);

            if (filter == null)
            {
                return HttpBadRequest($"filter set not found");
            }

            var filterModel = new FilterModel();

            filterModel.Id = filter.Id;
            filterModel.Name = filter.Name;
            filterModel.CampaignId = filter.CampaignId;
            filterModel.Conditions = new List<FilterConditionModel>();

            var filterConditions = _filterService.GetFilterConditionsByFilterId(filter.Id);

            foreach (var condition in filterConditions)
            {
                List<FilterConditionValueModel> valueModels = SetFilterConditionValueModel(condition);

                var conditionModel = new FilterConditionModel()
                {
                    CampaignFieldId = condition.CampaignTemplateId,
                    Condition = condition.Condition,
                    FilterId = filter.Id,
                    ParentId = condition.ParentId.HasValue ? condition.ParentId.Value : 0,
                    Values = valueModels,
                    Children = new List<FilterSubConditionModel>(),
                    Id = condition.Id
                };

                List<FilterCondition> subConditions = (List<FilterCondition>)_filterService.GetFilterConditionsByFilterId(filter.Id, condition.Id);

                foreach (var subCondition in subConditions)
                {
                    var subValueModels = SetFilterConditionValueModel(subCondition);

                    var subConditionModel = new FilterSubConditionModel()
                    {
                        CampaignFieldId = subCondition.CampaignTemplateId,
                        Condition = subCondition.Condition,
                        FilterId = filter.Id,
                        ParentId = subCondition.ParentId.HasValue ? subCondition.ParentId.Value : 0,
                        Values = subValueModels,
                        Id = subCondition.Id
                    };

                    conditionModel.Children.Add(subConditionModel);
                }

                filterModel.Conditions.Add(conditionModel);
            }

            return Ok(filterModel);
        }

        [HttpGet]
        [Route("getFilterSetsByCampaignId/{campaignId}")]

        public IHttpActionResult GetFilterSetsByCampaignId(long campaignId)
        {
            var filterModels = GetFilterSetsListByCampaignId(campaignId);

            return Ok(filterModels);
        }


        private List<FilterModel> GetFilterSetsListByCampaignId(long campaignId)
        {
            List<FilterModel> filterModels = new List<FilterModel>();

            var filters = _filterService.GetFiltersByCampaignId(campaignId);

            foreach (var filter in filters)
            {
                var filterModel = new FilterModel();

                filterModel.Id = filter.Id;
                filterModel.Name = filter.Name;
                filterModel.CampaignId = filter.CampaignId;
                filterModel.Conditions = new List<FilterConditionModel>();

                var filterConditions = _filterService.GetFilterConditionsByFilterId(filter.Id);

                foreach (var condition in filterConditions)
                {
                    var valueModels = SetFilterConditionValueModel(condition);

                    var conditionModel = new FilterConditionModel()
                    {
                        CampaignFieldId = condition.CampaignTemplateId,
                        Condition = condition.Condition,
                        FilterId = filter.Id,
                        ParentId = condition.ParentId.HasValue ? condition.ParentId.Value : 0,
                        Values = valueModels,
                        Children = new List<FilterSubConditionModel>(),
                        Id = condition.Id
                    };

                    List<FilterCondition> subConditions = (List<FilterCondition>)_filterService.GetFilterConditionsByFilterId(filter.Id, condition.Id);

                    foreach (var subCondition in subConditions)
                    {
                        var subValueModels = SetFilterConditionValueModel(subCondition);

                        var subConditionModel = new FilterSubConditionModel()
                        {
                            CampaignFieldId = subCondition.CampaignTemplateId,
                            Condition = subCondition.Condition,
                            FilterId = filter.Id,
                            ParentId = subCondition.ParentId.HasValue ? subCondition.ParentId.Value : 0,
                            Values = subValueModels,
                            Id = subCondition.Id
                        };

                        conditionModel.Children.Add(subConditionModel);
                    }

                    filterModel.Conditions.Add(conditionModel);
                }

                filterModels.Add(filterModel);
            }

            return filterModels;
        }

        private List<FilterConditionValueModel> SetFilterConditionValueModel(FilterCondition condition)
        {
            List<FilterConditionValueModel> valueModels = new List<FilterConditionValueModel>();

            if (!string.IsNullOrEmpty(condition.Value))
            {
                string[] values = condition.Value.Split(new char[1] { ',' });
                foreach (string val in values)
                {
                    string[] rangeValues = val.Split(new char[1] { '-' });
                    if (rangeValues.Length > 0)
                    {
                        FilterConditionValueModel filterConditionValueModel = new FilterConditionValueModel();
                        filterConditionValueModel.Value1 = rangeValues[0];
                        if (rangeValues.Length > 1)
                            filterConditionValueModel.Value2 = rangeValues[1];
                        valueModels.Add(filterConditionValueModel);
                    }
                }
            }

            return valueModels;
        }

        [HttpPost]
        [Route("updateFilter")]

        public IHttpActionResult UpdateFilter([FromBody] FilterModel filterModel)
        {
            var result = UpdateSingleFilter(filterModel);
            if (!string.IsNullOrEmpty(result.error))
                return HttpBadRequest(result.error);

            return Ok(result);
        }

        [HttpPost]
        [Route("updateFilters/{campaignId}")]

        public IHttpActionResult UpdateFilters(long campaignId, [FromBody] List<FilterModel> filterModels)
        {
            List<long> ids = filterModels.Select(x => x.Id).ToList();
            Dictionary<long, bool> canDelete = new Dictionary<long, bool>();

            foreach (var filterModel in filterModels)
            {
                if (string.IsNullOrEmpty(filterModel.Name))
                    continue;
                var result = UpdateSingleFilter(filterModel);
                if (!string.IsNullOrEmpty(result.error))
                    return HttpBadRequest(result.error);
                if (filterModel.Id == 0)
                    canDelete[result.id] = false;
                else
                    canDelete[result.id] = true;
            }

            var filterSetsModels = GetFilterSetsListByCampaignId(campaignId);

            var filterSetsModelsCopy = filterSetsModels.ToList();

            foreach (var filterSetId in filterSetsModels.Where(x => !ids.Contains(x.Id)).Select(x => x.Id))
            {
                if (canDelete.ContainsKey(filterSetId) && !canDelete[filterSetId]) continue;
                var filterSet = _filterService.GetFilterById(filterSetId);
                if (filterSet != null)
                {
                    _filterService.DeleteFilter(filterSet);
                    filterSetsModelsCopy.Remove(filterSetsModelsCopy.Where(x => x.Id == filterSet.Id).FirstOrDefault());
                }
            }

            return Ok(filterSetsModelsCopy);
        }

        private (string error, long id) UpdateSingleFilter(FilterModel filterModel)
        {
            try
            {
                if (filterModel == null)
                {
                    return ($"filter model is null", 0);
                }

                Filter filter = null;
                bool isNew = false;

                Campaign campaign = _campaignService.GetCampaignById(filterModel.CampaignId);

                if (filterModel.Id == 0)
                {
                    filter = (Filter)filterModel;
                    _filterService.InsertFilter(filter);
                    isNew = true;
                }
                else
                {
                    filter = _filterService.GetFilterById(filterModel.Id);
                    if (filter == null)
                    {
                        return ($"the filter set does not exist", 0);
                    }
                }

                filter.Name = filterModel.Name;
                _filterService.DeleteFilterConditions(filter.Id);
                var conditions = new List<FilterCondition>();
                Dictionary<string, List<FilterCondition>> filterConditions = new Dictionary<string, List<FilterCondition>>();

                if (filterModel.Conditions != null && filterModel.Conditions.Any())
                {
                    foreach (var conditionModel in filterModel.Conditions)
                    {
                        var filterCondition = (FilterCondition)conditionModel;

                        //if (isNew)
                            filterCondition.FilterId = filter.Id;

                        if (conditionModel.CampaignFieldId == 0 && campaign != null && !string.IsNullOrEmpty(conditionModel.CampaignFieldName))
                        {
                            var campaignField = campaign.CampaignFields.Where(x => x.TemplateField.ToString() == conditionModel.CampaignFieldName.ToLower()).FirstOrDefault();
                            if (campaignField != null)
                            {
                                filterCondition.CampaignTemplateId = campaignField.Id;
                            }
                        }

                        _filterService.InsertFilterCondition(filterCondition);

                        //conditions.Add(filterCondition);

                        string filterConditionKey = $"{filterCondition.Value}{filterCondition.Value2}{filterCondition.CampaignTemplateId}{filterCondition.Condition}{filterCondition.FilterId}";

                        var subConditions = new List<FilterCondition>();

                        if (conditionModel.Children != null)
                        {
                            foreach (var subConditionModel in conditionModel.Children)
                            {
                                var filterSubCondition = (FilterCondition)subConditionModel;
                                filterSubCondition.ParentId = filterCondition.Id;

                                //if (isNew)
                                    filterSubCondition.FilterId = filter.Id;

                                if (subConditionModel.CampaignFieldId == 0 && campaign != null && !string.IsNullOrEmpty(subConditionModel.CampaignFieldName))
                                {
                                    var campaignField = campaign.CampaignFields.Where(x => x.TemplateField.ToString() == subConditionModel.CampaignFieldName.ToLower()).FirstOrDefault();
                                    if (campaignField != null)
                                    {
                                        filterSubCondition.CampaignTemplateId = campaignField.Id;
                                    }
                                }

                                //subConditions.Add(filterSubCondition);
                                _filterService.InsertFilterCondition(filterSubCondition);

                            }
                        }

                        filterConditions[filterConditionKey] = subConditions;
                    }
                }

                /*foreach (var filterCondition in filter.FilterConditions.ToList())
                {
                    string filterConditionKey = $"{filterCondition.Value}{filterCondition.Value2}{filterCondition.CampaignTemplateId}{filterCondition.Condition}{filterCondition.FilterId}";

                    if (filterConditions.ContainsKey(filterConditionKey))
                    {
                        foreach (var subFilterCondition in filterConditions[filterConditionKey])
                        {
                            subFilterCondition.ParentId = filterCondition.Id;
                            conditions.Add(subFilterCondition);
                        }
                    }
                }

                _filterService.UpdateFilter(filter);*/

                return ("", filter.Id);
            }
            catch (Exception ex)
            {
                return (ex.Message, 0);
            }
        }

        [HttpDelete]
        [Route("deleteFilterById/{id}")]

        public IHttpActionResult DeleteFilterById(long Id)
        {
            try
            {
                var filter = _filterService.GetFilterById(Id);

                if (filter == null)
                {
                    return HttpBadRequest($"filter does not exist");
                }

                _filterService.DeleteFilterConditions(Id);
                _filterService.DeleteFilter(filter);


                return Ok();
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("deleteFiltersByIds")]

        public IHttpActionResult DeleteFiltersByIds([FromBody] List<long> Ids)
        {
            foreach (long Id in Ids)
            {
                try
                {
                    var filter = _filterService.GetFilterById(Id);

                    if (filter != null)
                    {
                        _filterService.DeleteFilterConditions(Id);
                        _filterService.DeleteFilter(filter);
                    }
                }
                catch (Exception ex)
                {
                    return HttpBadRequest(ex.Message);
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("updateFilterCondition")]

        public IHttpActionResult UpdateFilterCondition([FromBody] FilterConditionModel filterConditionModel)
        {
            try
            {
                if (filterConditionModel == null)
                {
                    return HttpBadRequest($"filter condition model is null");
                }

                FilterCondition filterCondition = null;

                if (filterConditionModel.Id == 0)
                {
                    filterCondition = (FilterCondition)filterConditionModel;
                    _filterService.InsertFilterCondition(filterCondition);
                }
                else
                {
                    filterCondition = _filterService.GetFilterConditionById(filterConditionModel.Id);
                    filterCondition.ParentId = filterConditionModel.ParentId;
                    filterCondition.Value = filterConditionModel.GetValue();
                    filterCondition.Value2 = filterConditionModel.GetValue();
                    filterCondition.CampaignTemplateId = filterConditionModel.CampaignFieldId;
                    filterCondition.Condition = filterConditionModel.Condition;
                    _filterService.UpdateFilterCondition(filterCondition);
                }

                return Ok(filterCondition);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("deleteFilterConditionById")]

        public IHttpActionResult DeleteFilterConditionById(long Id)
        {
            try
            {
                var filterCondition = _filterService.GetFilterConditionById(Id);

                if (filterCondition == null)
                {
                    return HttpBadRequest($"filter condition does not exist");
                }

                _filterService.DeleteFilterCondition(filterCondition);


                return Ok();
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        #region PrivateMethods

        [HttpPost]
        [Route("updateFilters/test")]
        public IHttpActionResult Validate()
        {
            ValidateFilter(new FilterModel());
            return Ok();
        }

        private FilterModel ValidateFilter(FilterModel filterModel)
        {
            filterModel = new FilterModel();
            filterModel.Conditions.Add(new FilterConditionModel()
            {
                Condition = (short)Conditions.Contains,
                Values = new List<FilterConditionValueModel>() {
                    new FilterConditionValueModel(){
                        Value1="ll",Value2="78"
                    }
               }
            });
            filterModel.Conditions.Add(new FilterConditionModel()
            {
                Condition = (short)Conditions.NotContains,
                Values = new List<FilterConditionValueModel>() {
                    new FilterConditionValueModel(){
                        Value1="ll",Value2="78"
                    }
               }
            });
            filterModel.Conditions.Add(new FilterConditionModel()
            {
                Condition = (short)Conditions.Contains,
                Values = new List<FilterConditionValueModel>() {
                    new FilterConditionValueModel(){
                        Value1="lus",Value2="78"
                    }
               }
            });
            filterModel.Conditions.Add(new FilterConditionModel()
            {
                Condition = (short)Conditions.StartsWith,
                Values = new List<FilterConditionValueModel>() {
                    new FilterConditionValueModel(){
                        Value1="aaa",Value2="78"
                    }
               }
            });
            filterModel.Conditions.Add(new FilterConditionModel()
            {
                Condition = (short)Conditions.StartsWith,
                Values = new List<FilterConditionValueModel>() {
                    new FilterConditionValueModel(){
                        Value1="bbb",Value2="78"
                    }
               }
            });
            filterModel.Conditions.Add(new FilterConditionModel()
            {
                Condition = (short)Conditions.EndsWith,
                Values = new List<FilterConditionValueModel>() {
                    new FilterConditionValueModel(){
                        Value1="bab",Value2="78"
                    }
               }
            });
            filterModel.Conditions.Add(new FilterConditionModel()
            {
                Condition = (short)Conditions.EndsWith,
                Values = new List<FilterConditionValueModel>() {
                    new FilterConditionValueModel(){
                        Value1="bab",Value2="78"
                    }
               }
            });
            filterModel.Conditions.Add(new FilterConditionModel()
            {
                Condition = (short)Conditions.Equals,
                Values = new List<FilterConditionValueModel>() {
                    new FilterConditionValueModel(){
                        Value1="bab",Value2="78"
                    }
               }
            });
            filterModel.Conditions.Add(new FilterConditionModel()
            {
                Condition = (short)Conditions.NotEquals,
                Values = new List<FilterConditionValueModel>() {
                    new FilterConditionValueModel(){
                        Value1="lll",Value2="78"
                    }
               }
            });
            filterModel.Conditions.Add(new FilterConditionModel()
            {
                Condition = (short)Conditions.Equals,
                Values = new List<FilterConditionValueModel>() {
                    new FilterConditionValueModel(){
                        Value1="bab",Value2="78"
                    }
               }
            });
            filterModel.Conditions.Add(new FilterConditionModel()
            {
                Condition = (short)Conditions.NumberGreater,
                Values = new List<FilterConditionValueModel>() {
                    new FilterConditionValueModel(){
                        Value1="45",Value2="78"
                    }
               }
            });
            filterModel.Conditions.Add(new FilterConditionModel()
            {
                Condition = (short)Conditions.NumberGreater,
                Values = new List<FilterConditionValueModel>() {
                    new FilterConditionValueModel(){
                        Value1="10",Value2="78"
                    }
               }
            });
            filterModel.Conditions.Add(new FilterConditionModel()
            {
                Condition = (short)Conditions.NumberLess,
                Values = new List<FilterConditionValueModel>() {
                    new FilterConditionValueModel(){
                        Value1="10",Value2="78"
                    }
               }
            });
            var result = new FilterModel();
            if (filterModel.Conditions != null && filterModel.Conditions.Any())
            {
                string filterTemplate = "шш";
                string equalString = string.Empty;
                double value1 = 0, value2 = 0;
                bool alreadyCheckedEqual = false;
                bool alreadyCheckedGreater = false;
                bool alreadyCheckedLess = false;
                bool alreadyCheckedRange = false;
                foreach (var filter in filterModel.Conditions)
                {
                    ChangeFilterTemplateByFilter(ref filterTemplate, ref value1, ref value2, filter, "", 0, ref alreadyCheckedEqual, ref equalString, ref alreadyCheckedGreater, ref alreadyCheckedLess, ref alreadyCheckedRange);
                }
            }
            return result;
        }

        private bool ChangeFilterTemplateByFilter(ref string template, ref double value1, ref double value2, FilterConditionModel filter, string value, int number, ref bool alreadyCheckedEqual, ref string equalString, ref bool alreadyCheckedGreater, ref bool alreadyCheckedLess, ref bool alreadyCheckedRange)
        {
            string pure = template.Replace("ш", ""); ;
            switch ((Conditions)filter.Condition)
            {
                case Conditions.Contains:
                    {
                        if (!pure.Contains(filter.Values[0].Value1))
                        {
                            var templates = template.Split('ш');
                            template = templates[0] + "ш" + templates[1] + filter.Values[0].Value1 + "ш" + templates[2];
                            return true;
                        }
                        else return false;
                    }
                case Conditions.NotContains:
                    {
                        if (pure.Contains(filter.Values[0].Value1))
                        {
                            return false;
                        }
                        else return true;
                    }
                case Conditions.StartsWith:
                    {
                        var templates = template.Split('ш');
                        if (string.IsNullOrWhiteSpace(templates.First()))
                        {
                            template = filter.Values[0].Value1 + "ш" + templates[1] + "ш" + templates[2];
                            return true;
                        }
                        else if (!string.IsNullOrWhiteSpace(templates.First()) && templates.First().Contains(filter.Values[0].Value1))
                        { return true; }
                        else return false;
                    }
                case Conditions.EndsWith:
                    {
                        var templates = template.Split('ш');
                        if (string.IsNullOrWhiteSpace(templates.Last()))
                        {
                            template = templates[0] + "ш" + templates[1] + "ш" + filter.Values[0].Value1;
                            return true;
                        }
                        else if (!string.IsNullOrWhiteSpace(templates.Last()) && templates.Last().Contains(filter.Values[0].Value1))
                        { return true; }
                        else return false;
                    }
                case Conditions.Equals:
                    if (!alreadyCheckedEqual)
                    {
                        alreadyCheckedEqual = true;
                        equalString = filter.Values[0].Value1;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case Conditions.NotEquals:
                    if (filter.Values[0].Value1 == equalString)
                    {
                        return false;
                    }
                    else return true;
                case Conditions.NumberGreater:
                    {
                        double passedValue = 0;
                        if (double.TryParse(filter.Values[0].Value1, out passedValue))
                        {
                            if (alreadyCheckedGreater)
                            {
                                return false;
                            }
                            else if (!alreadyCheckedGreater && alreadyCheckedLess && passedValue <= value1)
                            {
                                value1 = passedValue;
                                alreadyCheckedGreater = true;
                                return true;
                            }
                            else if (!alreadyCheckedLess && !alreadyCheckedGreater)
                            {
                                value1 = passedValue;
                                alreadyCheckedGreater = true;
                                return true;
                            }
                            else return false;
                        }
                        else return false;
                    }
                case Conditions.NumberGreaterOrEqual:
                    {
                        double passedValue = 0;
                        if (double.TryParse(filter.Values[0].Value1, out passedValue))
                        {
                            if (alreadyCheckedGreater || alreadyCheckedRange)
                            {
                                return false;
                            }
                            else if (!alreadyCheckedGreater && alreadyCheckedLess && passedValue <= value1)
                            {
                                value1 = passedValue;
                                alreadyCheckedGreater = true;
                                return true;
                            }
                            else if (!alreadyCheckedLess && !alreadyCheckedGreater)
                            {
                                value1 = passedValue;
                                alreadyCheckedGreater = true;
                                return true;
                            }
                            else return false;
                        }
                        else return false;
                    }
                case Conditions.NumberLess:
                    {
                        double passedValue = 0;
                        if (double.TryParse(filter.Values[0].Value1, out passedValue))
                        {
                            if (alreadyCheckedLess || alreadyCheckedRange)
                            {
                                return false;
                            }
                            else if (!alreadyCheckedLess && alreadyCheckedGreater && passedValue >= value1)
                            {
                                value2 = passedValue;
                                alreadyCheckedLess = true;
                                return true;
                            }
                            else if (!alreadyCheckedGreater && !alreadyCheckedLess)
                            {
                                value2 = passedValue;
                                alreadyCheckedLess = true;
                                return true;
                            }
                            else return false;
                        }
                        else return false;
                    }
                case Conditions.NumberLessOrEqual:
                    {
                        double passedValue = 0;
                        if (double.TryParse(filter.Values[0].Value1, out passedValue))
                        {
                            if (alreadyCheckedLess)
                            {
                                return false;
                            }
                            else if (!alreadyCheckedLess && alreadyCheckedGreater && passedValue >= value1)
                            {
                                value2 = passedValue;
                                alreadyCheckedLess = true;
                                return true;
                            }
                            else if (!alreadyCheckedGreater && !alreadyCheckedLess)
                            {
                                value2 = passedValue;
                                alreadyCheckedLess = true;
                                return true;
                            }
                            else return false;
                        }
                        else return false;
                    }
                case Conditions.NumberRange:
                    {
                        double passedValue1 = 0, passedValue2 = 0;

                        if (double.TryParse(filter.Values[0].Value1, out passedValue1) && value1 != 0
                            && double.TryParse(filter.Values[0].Value2, out passedValue1) && value2 != 0)
                        {
                            if (passedValue2 > passedValue1)
                            {
                                if (alreadyCheckedGreater)
                                {
                                    return false;
                                }
                                else if (!alreadyCheckedGreater)
                                {
                                    alreadyCheckedRange = true;
                                    value1 = passedValue1;
                                }
                                if (alreadyCheckedLess)
                                {
                                    return false;
                                }
                                else if (!alreadyCheckedLess)
                                {
                                    alreadyCheckedRange = true;
                                    value2 = passedValue2;
                                }
                            }
                            else return false;
                        }
                        break;
                    }
                case Conditions.StringByLength:
                    break;

            }
            return true;
        }
        #endregion
    }
}