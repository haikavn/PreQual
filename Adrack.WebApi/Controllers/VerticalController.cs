using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Adrack.Core.Domain.Lead;
using Adrack.Service.Lead;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Campaigns;
using Adrack.WebApi.Models.Vertical;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/vertical")]
    public class VerticalController : BaseApiController
    {
        #region fields

        private readonly IVerticalService _verticalService;
        private readonly ICampaignService _campaignService;


        #endregion fields

        #region constructors

        public VerticalController(IVerticalService verticalService, ICampaignService campaignService)
        {
            _verticalService = verticalService;
            _campaignService = campaignService;
        }

        #endregion constructors

        #region methods

        /// <summary>
        /// Get All Verticals
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Route("getVerticalList")]
        public IHttpActionResult GetVerticalList()
        {
            var result = new List<VerticalListModel>();
            var verticals = _verticalService.GetAllVerticals();
            foreach (var item in verticals)
            {
                result.Add(new VerticalListModel()
                {
                    Group = item.Group,
                    Name = item.Name,
                    Id = item.Id,
                    IconName = item.IconName,
                    Campaigns = new List<IdNameModel>()
                });
            }
            return Ok(result);
        }

        /// <summary>
        /// Get All Verticals with campaigns
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Route("getVerticalListWithCampaigns")]
        public IHttpActionResult GetVerticalListWithCampaigns()
        {
            var result = new List<VerticalListModel>();
            var verticals = _verticalService.GetAllVerticals();
            foreach (var item in verticals)
            {
                var campaignModels = new List<IdNameModel>();
                var campaigns = _campaignService.GetCampaignsByVerticalId(item.Id);

                foreach (var campaign in campaigns)
                {
                    if (!campaign.IsTemplate)
                        continue;
                    campaignModels.Add(new IdNameModel()
                    {

                        Id = campaign.Id,
                        Name = campaign.Name
                    });
                }                

                result.Add(new VerticalListModel()
                {
                    Group = item.Group,
                    Name = item.Name,
                    Id = item.Id,
                    IconName = item.IconName,
                    Campaigns = campaignModels
                });
            }
            return Ok(result);
        }

        /// <summary>
        /// Get Vertical By Id
        /// </summary>
        /// <param name="id">long</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Route("getVerticalById/{Id}")]
        public IHttpActionResult GetVerticalById(long id) 
        {
            var vertical = _verticalService.GetVerticalById(id);

            if (vertical == null)
                return HttpBadRequest($"no vertical was found for given id {id}");

            return Ok(vertical);
        }

        /// <summary>
        /// Insert Vertical With Fields
        /// </summary>
        /// <param name="verticalModel">VerticalModel</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Route("addVertical")]
        public IHttpActionResult AddVertical([FromBody]VerticalModel verticalModel)
        {
            try
            {
                if (verticalModel == null)
                    return HttpBadRequest($"vertical model is null");

                var existedVertical = _verticalService.GetVerticalByName(verticalModel.Name);
                if (existedVertical != null)
                {
                    return HttpBadRequest("Category name is already in use.");
                }

                var vertical = (Vertical) verticalModel;
                vertical.IconName = verticalModel.IconName;

                var id = _verticalService.InsertVertical(vertical);

                var addedVertical = _verticalService.GetVerticalById(id);

                return Ok(addedVertical);
            }
            catch (Exception ex)
            {
               return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Vertical With Fields
        /// </summary>
        /// <param name="id">long</param>
        /// <param name="verticalModel">VerticalModel</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Route("updateVertical")]
        public IHttpActionResult UpdateVertical([FromUri]long id, [FromBody]VerticalModel verticalModel)
        {
            try
            {
                var vertical = _verticalService.GetVerticalById(id);

                if (vertical == null)
                   return HttpBadRequest($"no vertical was found for given id {id}");

                if (verticalModel == null)
                   return HttpBadRequest($"vertical model is null for given id {id}");

                var existedVertical = _verticalService.GetVerticalByName(verticalModel.Name);
                if (existedVertical != null)
                {
                    return HttpBadRequest("Category name is already in use.");
                }

                vertical.Name = verticalModel.Name;
                vertical.IconName = verticalModel.IconName;
                /*vertical.Group = verticalModel.Group;
                foreach (var field in verticalModel.Fields)
                {
                    if (field.Id == 0)
                    {
                       vertical.VerticalFields.Add((VerticalField)field); 
                    }
                    else
                    {
                        var item = vertical.VerticalFields.FirstOrDefault(x => x.Id == field.Id);
                        item.Name = field.Name;
                        item.DataType = field.DataType;
                        item.IsRequired = field.IsRequired;
                        item.Description = field.Description;
                        
                    }
                }*/

                _verticalService.UpdateVertical(vertical);
                return Ok(vertical);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Vertical If Not Contains Any Field
        /// </summary>
        /// <param name="id">long</param>
        /// <returns>ActionResult</returns>
        [HttpDelete]
        [Route("deleteVertical")]
        public IHttpActionResult DeleteVertical(int id)
        {
            try
            {
                var vertical = _verticalService.GetVerticalById(id);

                if (vertical == null)
                    return HttpBadRequest($"no vertical was found for given id {id}");

                //if (vertical.VerticalFields != null && vertical.VerticalFields.Any())
                //   return HttpBadRequest("can't remove this vertical");

                _verticalService.DeleteVertical(vertical);
                return Ok();
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Soft Delete Vertical
        /// </summary>
        /// <param name="id">long</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Route("softDeleteVertical")]
        public IHttpActionResult SoftDeleteVertical(int id)
        {
            try
            {
                var vertical = _verticalService.GetVerticalById(id);

                if (vertical == null)
                   return HttpBadRequest($"no vertical was found for given id {id}");

                _verticalService.UpdateVertical(vertical);
                return Ok(vertical);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Vertical Fields
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Route("getVerticalFieldList")]
        public IHttpActionResult GetVerticalFieldList()
        {
            var verticalFields = _verticalService.GetAllVerticalFields();
            return Ok(verticalFields);
        }

        /// <summary>
        /// Get Vertical Field By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Route("getVerticalFieldById")]
        public IHttpActionResult GetVerticalFieldById([FromUri]long id)
        {
            var verticalField = _verticalService.GetVerticalFieldById(id);

            if (verticalField == null)
                return HttpBadRequest($"no vertical field was found for given id {id}");
            
            return Ok(verticalField);
        }

        /// <summary>
        /// Insert Vertical Field
        /// </summary>
        /// <param name="verticalFieldModel">VerticalFieldModel</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Route("insertVerticalField")]
        public IHttpActionResult InsertVerticalField([FromBody]VerticalFieldModel verticalFieldModel)
        {
            try
            {
                if (verticalFieldModel == null)
                   return HttpBadRequest($"vertical field model is null");
                var verticalField = (VerticalField)verticalFieldModel;
                _verticalService.InsertVerticalField(verticalField);
                return Ok(verticalField);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Vertical Field
        /// </summary>
        /// <param name="id">long</param>
        /// <param name="verticalFieldModel">VerticalFieldModel</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Route("updateVerticalField")]
        public IHttpActionResult UpdateVerticalField([FromUri]long id, [FromBody]VerticalFieldModel verticalFieldModel)
        {
            try
            {
                var verticalField = _verticalService.GetVerticalFieldById(id);

                if (verticalField == null)
                    return HttpBadRequest($"no vertical field was found for given id {id}");

                if (verticalFieldModel == null)
                   return HttpBadRequest($"vertical field model is null for given id {id}");

                verticalField.Name = verticalFieldModel.Name;
                verticalField.DataType = verticalFieldModel.DataType;
                verticalField.IsRequired = verticalFieldModel.IsRequired;
                verticalField.Description = verticalFieldModel.Description;

                _verticalService.UpdateVerticalField(verticalField);
                return Ok(verticalField);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Vertical Field
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("deleteVerticalField")]
        public IHttpActionResult DeleteVerticalField(int id)
        {
            try
            {
                var verticalField = _verticalService.GetVerticalFieldById(id);

                if (verticalField == null)
                    return HttpBadRequest($"no vertical field was found for given id {id}");

                _verticalService.DeleteVerticalField(verticalField);
                return Ok();
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Load Field Properties From Xml
        /// </summary>
        /// <param name="xml">string</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Route("loadVerticalFieldsFromXml")]
        public IHttpActionResult LoadVerticalFieldsFromXml(string xml)
        {
            try
            {
                if (string.IsNullOrEmpty(xml))
                {
                   return HttpBadRequest("Xml is empty");
                }
                var fieldNames = _verticalService.LoadFieldNamesFromXml(xml);
                return Ok(fieldNames);
            }
            catch (Exception ex)
            {
               return HttpBadRequest(ex.Message);
            }
        }

        #endregion methods
    }
}