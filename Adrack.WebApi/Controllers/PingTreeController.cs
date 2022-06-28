using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Campaigns;
using Adrack.WebApi.Models.Vertical;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/pingtree")]
    public class PingTreeController : BaseApiController 
    {
        #region fields

        private readonly IBuyerChannelService _buyerChannelService;
        private readonly ICampaignService _campaignService;

        private readonly ISettingService _settingService;


        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// The Entity Change History service
        /// </summary>
        private readonly IEntityChangeHistoryService _entityChangeHistoryService;
        #endregion fields

        #region constructors

        public PingTreeController(ICampaignService campaignService,
            IBuyerChannelService buyerChannelService,
            IUserRegistrationService userRegistrationService, IUserService userService,
            IEntityChangeHistoryService entityChangeHistoryService,
            ISettingService settingService)
        {
            _campaignService = campaignService;
            _buyerChannelService = buyerChannelService;
            _userService = userService;
            _entityChangeHistoryService = entityChangeHistoryService;
            _settingService = settingService;
        }

        #endregion constructors

        #region methods

        /// <summary>
        /// Get All Verticals
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Route("getPingTrees/{campaignId}")]
        public IHttpActionResult GetPingTrees(long campaignId)
        {
            var pingTrees = _campaignService.GetPingTrees(campaignId).OrderBy(x => x.Id).ToList();

            List<object> list = new List<object>();
            var buyerChannels = _buyerChannelService.GetAllBuyerChannelsByCampaignId(campaignId);

            foreach (var pingTree in pingTrees)
            {
                var items = _campaignService.GetPingTreeItems(pingTree.Id).OrderBy(x => x.OrderNum).ToList();

                List<object> channels = new List<object>();

                foreach (var item in items)
                {
                    var buyerChannel = buyerChannels.Where(x => x.Id == item.BuyerChannelId).FirstOrDefault();
                    if (buyerChannel != null)
                    {
                        channels.Add(new
                        {
                            Id = item.Id,
                            BuyerChannelId = buyerChannel.Id,
                            BuyerChannelName = buyerChannel.Name,
                            OrderNum = item.OrderNum,
                            GroupNum = item.GroupNum,
                            Percent = item.Percent,
                            IsLocked = item.IsLocked,
                            BuyerPrice = buyerChannel.BuyerPrice,
                            BuyerPriceOption = buyerChannel.BuyerPriceOption.ToString(),
                            Status = item.Status.ToString()
                        });
                    }
                }

                var createdBy = string.Empty;
                DateTime? createdAt = null;
                var createdHistoryObj = _entityChangeHistoryService.GetEntityHistory(pingTree.Id, "PingTree", "Added");
                if (createdHistoryObj != null)
                {
                    createdBy = _userService.GetUserById(createdHistoryObj.UserId)?.Username;
                    createdAt = _settingService.GetTimeZoneDate(createdHistoryObj.ModifiedDate);
                }

                var updatedBy = string.Empty;
                DateTime? updatedAt = null;
                var updatedHistoryObj = _entityChangeHistoryService.GetEntityHistory(pingTree.Id, "PingTree", "Modified");
                if (updatedHistoryObj != null)
                {
                    updatedBy = _userService.GetUserById(updatedHistoryObj.UserId)?.Username;
                    updatedAt = _settingService.GetTimeZoneDate(updatedHistoryObj.ModifiedDate);
                }

                list.Add(new
                {
                    Id = pingTree.Id,
                    Name = pingTree.Name,
                    Quantity = pingTree.Quantity,
                    CampaingId = pingTree.CampaignId,
                    Channels = channels,
                    CreatedDate = createdAt,
                    CreatedBy = createdBy,
                    UpdatedDate = updatedAt,
                    UpdatedBy = updatedBy
                });
            }

            return Ok(list);
        }

        /// <summary>
        /// Get Vertical By Id
        /// </summary>
        /// <param name="id">long</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [Route("getPingTreeById/{Id}")]
        public IHttpActionResult GetPingTreeById(long id) 
        {
            var pingTree = _campaignService.GetPingTreeById(id);

            if (pingTree == null)
                return HttpBadRequest($"no ping tree was found for given id {id}");

            var items = _campaignService.GetPingTreeItems(id).OrderBy(x => x.OrderNum).ToList();

            var buyerChannels = _buyerChannelService.GetAllBuyerChannelsByCampaignId(pingTree.CampaignId);

            List<object> channels = new List<object>();

            foreach(var item in items)
            {
                var buyerChannel = buyerChannels.Where(x => x.Id == item.BuyerChannelId).FirstOrDefault();
                if (buyerChannel != null)
                {
                    channels.Add(new
                    {
                        Id = item.Id,
                        BuyerChannelId = buyerChannel.Id,
                        BuyerChannelName = buyerChannel.Name,
                        OrderNum = item.OrderNum,
                        GroupNum = item.GroupNum,
                        Percent = item.Percent,
                        IsLocked = item.IsLocked,
                        BuyerPrice = buyerChannel.BuyerPrice,
                        BuyerPriceOption = buyerChannel.BuyerPriceOption.ToString(),
                        Status = item.Status.ToString()
                    });
                }
            }

            return Ok(new { 
                Id = pingTree.Id,
                Name = pingTree.Name,
                Quantity = pingTree.Quantity,
                CampaingId = pingTree.CampaignId,
                Channels = channels
            });
        }

        /// <summary>
        /// Insert Vertical With Fields
        /// </summary>
        /// <param name="verticalModel">VerticalModel</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Route("addPingTree")]
        public IHttpActionResult AddPingTree([FromBody]PingTreeModel pingTreeModel)
        {
            try
            {
                if (pingTreeModel == null)
                    return HttpBadRequest($"pingTreeModel model is null");

                var pingTree = (PingTree)pingTreeModel;
                _campaignService.InsertPingTree(pingTree);
                pingTreeModel.Id = pingTree.Id; 

                foreach(var item in pingTreeModel.Items)
                {
                    var pingTreeItem = (PingTreeItem)item;
                    var buyerChannel = _buyerChannelService.GetBuyerChannelById(item.BuyerChannelId);
                    if (buyerChannel == null || buyerChannel.CampaignId != pingTreeModel.CampaignId)
                        continue;
                    pingTreeItem.PingTreeId = pingTree.Id;
                    _campaignService.InsertPingTreeItem(pingTreeItem);
                    item.Id = pingTreeItem.Id;
                }

                if (pingTreeModel.Items == null || pingTreeModel.Items.Count == 0)
                {
                    pingTreeModel.Items = new List<PingTreeItemModel>();

                    var buyerChannels = _buyerChannelService.GetAllBuyerChannelsByCampaignId(pingTreeModel.CampaignId).OrderByDescending(x => x.BuyerPrice).ToList();
                    int orderNum = 1;
                    foreach(var buyerChannel in buyerChannels)
                    {
                        if (buyerChannel == null || buyerChannel.CampaignId != pingTreeModel.CampaignId)
                            continue;

                        var pingTreeItem = new PingTreeItem()
                        {
                            BuyerChannelId = buyerChannel.Id,
                            GroupNum = 0,
                            OrderNum = orderNum,
                            IsLocked = false,
                            Percent = 100,
                            PingTreeId = pingTree.Id,
                            Status = EntityStatus.Active
                        };
                        pingTreeItem.PingTreeId = pingTree.Id;
                        _campaignService.InsertPingTreeItem(pingTreeItem);

                        pingTreeModel.Items.Add(new PingTreeItemModel()
                        {
                            BuyerChannelId = buyerChannel.Id,
                            GroupNum = 0,
                            IsLocked = false,
                            OrderNum = orderNum,
                            Percent = 100,
                            Status = EntityStatus.Active,
                            Id = pingTreeItem.Id
                        });
                        
                        orderNum++;
                    }

                }
  
                return Ok(pingTreeModel);
            }
            catch (Exception ex)
            {
               return HttpBadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("updatePingTree")]
        public IHttpActionResult UpdatePingTree([FromBody] PingTreeModel pingTreeModel)
        {
            try
            {
                if (pingTreeModel == null)
                    return HttpBadRequest($"pingTreeModel model is null");

                var pingTree = _campaignService.GetPingTreeById(pingTreeModel.Id);
                if (pingTree == null)
                {
                    return HttpBadRequest("ping tree does not exist");
                }

                pingTree.Name = pingTreeModel.Name;
                pingTree.Quantity = pingTreeModel.Quantity;

                _campaignService.UpdatePingTree(pingTree);
                var items = _campaignService.GetPingTreeItems(pingTree.Id);

                foreach (var item in pingTreeModel.Items)
                {
                    var pingTreeItem = items.Where(x => x.Id == item.Id).FirstOrDefault();
                    if (pingTreeItem == null) continue;
                    var buyerChannel = _buyerChannelService.GetBuyerChannelById(item.BuyerChannelId);
                    if (buyerChannel == null || buyerChannel.CampaignId != pingTreeModel.CampaignId)
                        continue;
                    pingTreeItem.OrderNum = item.OrderNum;
                    pingTreeItem.IsLocked = item.IsLocked;
                    pingTreeItem.GroupNum = item.GroupNum;
                    pingTreeItem.Percent = item.Percent;
                    pingTreeItem.Status = item.Status;
                    _campaignService.UpdatePingTreeItem(pingTreeItem);
                    items.Remove(pingTreeItem);
                }

                foreach (var item in items)
                {
                    _campaignService.DeletePingTreeItem(item);
                }
                _campaignService.UpdatePingTree(pingTree);

                return Ok(pingTreeModel);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Insert Vertical With Fields
        /// </summary>
        /// <param name="verticalModel">VerticalModel</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Route("addPingTreeItem/{pingTreeId}")]
        public IHttpActionResult AddPingTreeItem(int pingTreeId, [FromBody] PingTreeItemModel pingTreeItemModel)
        {
            try
            {
                if (pingTreeItemModel == null)
                    return HttpBadRequest($"pingTreeItemModel model is null");

                var pingTree = _campaignService.GetPingTreeById(pingTreeId);
                if (pingTree == null)
                {
                    return HttpBadRequest("ping tree not found");
                }

                var pingTreeItem = (PingTreeItem)pingTreeItemModel;
                var buyerChannel = _buyerChannelService.GetBuyerChannelById(pingTreeItem.BuyerChannelId);
                if (buyerChannel == null || buyerChannel.CampaignId != pingTree.CampaignId)
                {
                    return HttpBadRequest("buyer channel does not belong to the campaign");
                }
                pingTreeItem.PingTreeId = pingTreeId;
                _campaignService.InsertPingTreeItem(pingTreeItem);
 
                return Ok(pingTreeItemModel);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("updatePingTreeItem")]
        public IHttpActionResult UpdatePingTreeItem([FromBody] PingTreeItemUpdateModel pingTreeItemModel)
        {
            try
            {
                if (pingTreeItemModel == null)
                    return HttpBadRequest($"pingTreeItemModel model is null");

                var pingTreeItem = _campaignService.GetPingTreeItemById(pingTreeItemModel.PingTreeItemId);
                if (pingTreeItem == null)
                {
                    return HttpBadRequest($"pingTreeItem not found");
                }

                var pingTree = _campaignService.GetPingTreeById(pingTreeItem.PingTreeId);

                var buyerChannel = _buyerChannelService.GetBuyerChannelById(pingTreeItem.BuyerChannelId);
                if (buyerChannel == null || buyerChannel.CampaignId != pingTree.CampaignId)
                {
                    return HttpBadRequest("buyer channel does not belong to the campaign");
                }

                pingTreeItem.OrderNum = pingTreeItemModel.OrderNum;
                pingTreeItem.IsLocked = pingTreeItemModel.IsLocked;
                pingTreeItem.GroupNum = pingTreeItemModel.GroupNum;
                pingTreeItem.Percent = pingTreeItemModel.Percent;

                if (pingTreeItemModel.Status.HasValue)
                {
                    pingTreeItem.Status = pingTreeItemModel.Status.Value;
                }

                _campaignService.UpdatePingTreeItem(pingTreeItem);

                return Ok(pingTreeItemModel);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("deletePingTree/{pingTreeId}")]
        public IHttpActionResult DeletePingTree(int pingTreeId)
        {
            try
            {
                var pingTree = _campaignService.GetPingTreeById(pingTreeId);
                if (pingTree == null)
                {
                    return HttpBadRequest("ping tree not found");
                }

                _campaignService.DeletePingTreeItems(pingTree.Id);

                _campaignService.DeletePingTree(pingTree);

                return Ok(pingTree);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("deletePingTreeItem/{pingTreeItemId}")]
        public IHttpActionResult DeletePingTreeItem(int pingTreeItemId)
        {
            try
            {

                var pingTreeItem = _campaignService.GetPingTreeItemById(pingTreeItemId);
                if (pingTreeItem == null)
                {
                    return HttpBadRequest("ping tree item not found");
                }

                _campaignService.DeletePingTreeItem(pingTreeItem);

                return Ok(pingTreeItem);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        /*[HttpPut]
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


                _verticalService.UpdateVertical(vertical);
                return Ok(vertical);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

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
        }*/

        #endregion methods
    }
}