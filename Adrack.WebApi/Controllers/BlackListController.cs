using Adrack.Core.Domain.Lead;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Lead;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Adrack.WebApi.Extensions;
using Adrack.WebApi.Models.Campaigns;
using Adrack.WebApi.Models.CustomBlackList;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/blacklist")]
    public class BlackListController : BaseApiController
    {
        #region fields

        private readonly IBlackListService _blackListService;

        private readonly ILocalizedStringService _localizedStringService;
        private readonly ICampaignTemplateService _campaignTemplateService;


        #endregion

        #region constructors

        /// <summary>
        /// Directory Controller
        /// </summary>
        /// <param name="blackListService">The black list service.</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="campaignTemplateService"></param>
        public BlackListController(IBlackListService blackListService,
                                   ILocalizedStringService localizedStringService,
                                   ICampaignTemplateService campaignTemplateService):base()
        {
            this._blackListService = blackListService;
            this._localizedStringService = localizedStringService;
            this._campaignTemplateService = campaignTemplateService;
        }

        #endregion

        #region methods

        #region route methods

        /// <summary>
        /// Shows black list item create/edit interface
        /// </summary>
        /// <param name="id">Black list type id</param>
        /// <returns>View result</returns>
        [HttpGet]
        [Route("type/{id}")]
        public BlackListModel Item([FromUri]long id = 0)
        {
            var blackListModel = new BlackListModel
            {
                BlackListTypeId = 0,
                BlackListType = 0,
                ParentId = 0
            };

            BlackListType blackListType = this._blackListService.GetBlackListTypeById(id);
            if (blackListType != null)
            {
                blackListModel.Name = blackListType.Name;
                blackListModel.BlackListTypeId = blackListType.Id;
                blackListModel.BlackListType = blackListType.BlackType;
                blackListModel.ParentId = blackListType.ParentId;
            }

            blackListModel.Conditions.Add(new SelectItem { Value = "1", Text = "EQUAL" });
            blackListModel.Conditions.Add(new SelectItem { Value = "2", Text = "STARTS WITH" });
            blackListModel.Conditions.Add(new SelectItem { Value = "3", Text = "ENDS WITH" });
            blackListModel.Conditions.Add(new SelectItem { Value = "4", Text = "CONTAINS" });

            PrepareModel(blackListModel);

            return blackListModel; //View(blackListModel);//return View(am);
        }


        /// <summary>
        /// Shows black list item create/edit interface
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="id">Black list parent id</param>
        /// <returns>View result</returns>
        [HttpGet]
        [Route("parent/{id}/type/{type}")]
        public BlackListModel Item([FromUri]string id, [FromUri]string type)
        {
            var blackListModel = new BlackListModel
            {
                BlackListTypeId = 0,
                BlackListType = 0,
                ParentId = 0
            };

            short.TryParse(type, out var blackListTypeResult);
            long.TryParse(id, out var parentIdResult);

            blackListModel.BlackListType = blackListTypeResult;
            blackListModel.ParentId = parentIdResult;

            blackListModel.Conditions.Add(new SelectItem { Value = "1", Text = "EQUAL" });
            blackListModel.Conditions.Add(new SelectItem { Value = "2", Text = "STARTS WITH" });
            blackListModel.Conditions.Add(new SelectItem { Value = "3", Text = "ENDS WITH" });
            blackListModel.Conditions.Add(new SelectItem { Value = "4", Text = "CONTAINS" });

            PrepareModel(blackListModel);

            return blackListModel; //View(blackListModel); //return View(am);
        }

        /// <summary>
        /// Handles black list item submit action
        /// </summary>
        /// <param name="model">BlackListModel reference</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [Route("item")]
        //[PublicAntiForgery]
        //[ValidateInput(false)]
        public BlackListModel Item([FromBody]BlackListModel model)
        {
            var blackListType = model.BlackListTypeId == 0 ? new BlackListType() : _blackListService.GetBlackListTypeById(model.BlackListTypeId);

            blackListType.Name = model.Name;
            blackListType.BlackType = model.BlackListType;
            blackListType.ParentId = model.ParentId;

            if (model.BlackListTypeId == 0)
                _blackListService.InsertBlackListType(blackListType);
            else
                _blackListService.UpdateBlackListType(blackListType);

            model.BlackListTypeId = blackListType.Id;

            PrepareModel(model);

            return model; //List(model); //Redirect("List");
        }

        /// <summary>
        /// Adds black list value
        /// </summary>
        /// <returns>Json result</returns>
        //[ContentManagementAntiForgery(true)]
        //[HttpGet]
        [HttpPost]
        [Route("type/{id}/value/{value}/condition/{condition}/item")]
        public void AddBlackListValue([FromUri]string id, [FromUri]string value, [FromUri]string condition)
        {
            var blackListValue = new BlackListValue
            {
                BlackListTypeId = long.Parse(id),
                Value = value,
                Condition = short.Parse(condition)
            };

            _blackListService.InsertBlackListValue(blackListValue);
            //return Json(new { id = 0 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Removes black list value
        /// </summary>
        /// <returns>Json result</returns>
        //[ContentManagementAntiForgery(true)]
        //[HttpGet]
        //[HttpPost]
        [HttpDelete]
        [Route("value/{id}")]
        public void RemoveBlackListValue([FromUri]string id)
        {
            BlackListValue blackListValue = _blackListService.GetBlackListValueById(long.Parse(id));

            if (blackListValue != null)
            {
                _blackListService.DeleteBlackListValue(blackListValue);
            }
        }

        /// <summary>
        /// Gets black list types
        /// </summary>
        /// <returns>Json result</returns>
        //[ContentManagementAntiForgery(true)]
        //[AppHttpsRequirement(SslRequirement.No)]
        [HttpGet]
        [Route("parent/{id}/type/{type}/list")]
        public List<BlackListType> GetBlackListTypes([FromUri]string type, [FromUri]string id)
        {
            short.TryParse(type, out var typeResult);
            long.TryParse(id, out var parentIdResult);

            var blackListTypes = (List<BlackListType>)this._blackListService.GetAllBlackListTypesByParentId(typeResult, parentIdResult);

            var blackListPagedTypes =
                new BasePagedItemsModel<List<BlackListType>, BlackListType>(blackListTypes);

            // TODO can returned blackListPagedTypes

            return blackListTypes;//Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get black list values
        /// </summary>
        /// <returns>Json result</returns>
        //[ContentManagementAntiForgery(true)]
        //[AppHttpsRequirement(SslRequirement.No)]
        [Route("type/{id}/list")]
        public List<BlackListValue> GetBlackListValues([FromUri]string id)
        {
            var typeId = long.Parse(id);
            var blackListValues = (List<BlackListValue>)this._blackListService.GetAllBlackListValues(typeId);

            var blackListPagedValues =
                new BasePagedItemsModel<List<BlackListValue>, BlackListValue>(blackListValues);
            // TODO can be set condition string from blackListValue condition and return paged data

            return blackListValues;//Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get custom black list values
        /// </summary>
        /// <returns>Json result</returns>
        [HttpGet]
        [Route("getCustomBlackListValues/{id}/{type}")]
        public IHttpActionResult GetCustomBlackListValues([FromUri]string id,
                                                          [FromUri]string type)
        {
            var channelId = long.Parse(id);
            var channelType = short.Parse(type);
            var customBlackListModels = new List<CustomBlackListModel>();
            var blackListValues = _blackListService.GetCustomBlackListValues(channelId, channelType);

            foreach (var cbl in blackListValues)
            {
                var campaignField = _campaignTemplateService.GetCampaignTemplateById(cbl.TemplateFieldId);

                customBlackListModels.Add(new CustomBlackListModel
                {
                    Id = cbl.Id,
                    ChannelId = cbl.ChannelId,
                    ChannelType = cbl.ChannelType,
                    Values = cbl.Value.Split(new []{','},StringSplitOptions.RemoveEmptyEntries).ToList(),
                    TemplateFieldId = cbl.TemplateFieldId,
                    TemplateField = campaignField != null? (CampaignFieldModel)campaignField : null
                });
            }
            return Ok(customBlackListModels);
        }

        /// <summary>
        /// Add custom black list value
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("addCustomBlackList")]
        public IHttpActionResult AddCustomBlackList([FromBody]CustomBlackListValueModel customBlackListValue)
        {
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var affiliateChannelBlackList = new CustomBlackListValue
                {
                    Id = 0,
                    ChannelId = customBlackListValue.ChannelId,
                    ChannelType = customBlackListValue.ChannelType,
                    Value = string.Join(",", customBlackListValue.Values),
                    TemplateFieldId = customBlackListValue.TemplateFieldId
                };
                _blackListService.InsertCustomBlackListValue(affiliateChannelBlackList);

                return Ok(affiliateChannelBlackList);
            }
            catch (Exception e)
            {
                return HttpBadRequest(e.Message);
            }
        }

        /// <summary>
        /// Update custom black list value
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("updateCustomBlackList/{id}")]
        public IHttpActionResult UpdateCustomBlackList(long id, [FromBody]CustomBlackListValueModel customBlackListValue)
        {
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var blackList = _blackListService.GetCustomBlackListValueById(id);

                if (blackList == null)
                {
                    return HttpBadRequest($"no black list was found for given id {id}");
                }

                blackList.Value = string.Join(",", customBlackListValue.Values);
                blackList.TemplateFieldId = customBlackListValue.TemplateFieldId;

                _blackListService.UpdateCustomBlackListValue(blackList);

                return Ok(blackList);
            }
            catch (Exception e)
            {
                return HttpBadRequest(e.Message);
            }
        }

        /// <summary>
        /// Delete black list by id
        /// </summary>
        /// <param name="id">long</param>
        /// <returns></returns>
        [HttpPost]
        [Route("deleteBlackList/{id}")]
        public IHttpActionResult DeleteBlackList(long id)
        {
            try
            {
                var blackList = _blackListService.GetCustomBlackListValueById(id);

                if (blackList == null)
                {
                    return HttpBadRequest($"no black list was found for given id {id}");
                }

                _blackListService.DeleteCustomBlackListValue(blackList);

                return Ok();
            }
            catch (Exception e)
            {
                return HttpBadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("loadValues/{fieldId}")]
        public IHttpActionResult LoadValues(long fieldId)
        {
            var result = new List<string>();
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files["CsvFile"] != null)
            {
                var file = httpRequest.Files.Get("CsvFile");
                if (file != null)
                {
                    using (StreamReader reader = new StreamReader(file.InputStream))
                    {
                        do
                        {
                            var textLine = reader.ReadLine()?.Replace('"', ' ');
                            var values = textLine?.Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries);
                            if (values != null && values.Any())
                                foreach (var item in values)
                                {
                                    var value = item.TrimStart().TrimEnd();
                                    result.Add(value);
                                }
                        } while (reader.Peek() != -1);
                    }
                }
            }
            else
            {
                return HttpBadRequest("The .csv file can not be empty");
            }
            return Ok(result);
        }

        #endregion

        #region private methods

        /// <summary>
        /// Prepares the model.
        /// </summary>
        /// <param name="model">The model.</param>
        protected void PrepareModel(BlackListModel model)
        {
            PropertyInfo[] properties = typeof(LeadContent).GetProperties();

            foreach (PropertyInfo pi in properties)
            {
                if (pi.Name.ToLower() == "id" ||
                    pi.Name.ToLower() == "leadid" ||
                    pi.Name.ToLower() == "affiliateid" ||
                    pi.Name.ToLower() == "campaigntype" ||
                    pi.Name.ToLower() == "minpricestr" ||
                    pi.Name.ToLower() == "created") continue;
                model.BlackListNames.Add(new SelectItem() { Text = pi.Name, Value = pi.Name });
            }
        }

        #endregion

        #endregion
    }
}
