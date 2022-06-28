using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using Adrack.Core.Domain.Content;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.WebApi.Extensions;
using Adrack.WebApi.Infrastructure.Core.DTOs;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models.History;
using Microsoft.Ajax.Utilities;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/history")]
    public class HistoryController : BaseApiController
    {
        #region fields
        private readonly IEntityChangeHistoryService _entityChangeHistoryService;
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        #endregion

        #region constructor

        public HistoryController(IEntityChangeHistoryService entityChangeHistoryService,
                                 IUserService userService, IPermissionService permissionService,
                                 ISettingService settingService)
        {
            _entityChangeHistoryService = entityChangeHistoryService;
            _userService = userService;
            _permissionService = permissionService;
            _settingService = settingService;
        }

        #endregion

        #region methods

        [HttpPost]
        [Route("getHistory")]
        public IHttpActionResult GetHistory([FromBody]HistoryFilterModel historyFilterModel)
        {
            try
            {
                long count = 0;
                var historyModels = new List<HistoryViewModel>();
               
                var history = _entityChangeHistoryService.GetHistory(historyFilterModel.EntityId,
                                                                                        historyFilterModel.EntityName,
                                                                                        historyFilterModel.StartDate,
                                                                                        historyFilterModel.EndDate
                                                                                        );

                foreach (var entityRow in history)
                {
                    var historyModel = new HistoryViewModel
                    {
                        Id = entityRow.Id,
                        EntityId = entityRow.EntityId,
                        EntityName = entityRow.Entity,
                        ModifiedDate = _settingService.GetTimeZoneDate(entityRow.ModifiedDate),
                        Action = $"{entityRow.Entity} {entityRow.State}",
                        Note = entityRow.Note,
                        User = entityRow.FullName,
                        Role = entityRow.RoleName,
                        Changes = new List<PropertyChangeHistory>()
                    };
                    if (historyFilterModel.WithChanges)
                    {
                        var propertyChangeHistory = new JavaScriptSerializer().Deserialize<List<PropertyChangeHistory>>(entityRow.ChangedData);

                        foreach (var item in propertyChangeHistory)
                        {
                            historyModel.Changes.Add(item);
                        }
                    }

                    historyModels.Add(historyModel);
                }

                return Ok(historyModels);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("getHistoryPaging")]
        public IHttpActionResult GetHistoryPaging([FromBody]HistoryFilterModelWithPaging historyFilterModel)
        {
            try
            {
                long count = 0;
                var historyModels = new List<HistoryViewModel>();

                var history = _entityChangeHistoryService.GetHistory(historyFilterModel.EntityId,
                                                                                        historyFilterModel.EntityName,
                                                                                        historyFilterModel.StartDate,
                                                                                        historyFilterModel.EndDate,
                                                                                        historyFilterModel.Page - 1,
                                                                                        historyFilterModel.Take,
                                                                                        out count);

                foreach (var entityRow in history)
                {
                    var historyModel = new HistoryViewModel
                    {
                        Id = entityRow.Id,
                        EntityId = entityRow.EntityId,
                        EntityName = entityRow.Entity,
                        ModifiedDate = _settingService.GetTimeZoneDate(entityRow.ModifiedDate),
                        Action = $"{entityRow.Entity} {entityRow.State}",
                        Note = entityRow.Note,
                        User = entityRow.FullName,
                        Role = entityRow.RoleName,
                        Changes = new List<PropertyChangeHistory>()
                    };
                    if (historyFilterModel.WithChanges)
                    {
                        var propertyChangeHistory = new JavaScriptSerializer().Deserialize<List<PropertyChangeHistory>>(entityRow.ChangedData);

                        foreach (var item in propertyChangeHistory)
                        {
                            historyModel.Changes.Add(item);
                        }
                    }

                    historyModels.Add(historyModel);
                }

                return Ok(new HistoryFilterOutModel()
                {
                    Count = count,
                    HistoryFilterModel = historyFilterModel,
                    HistoryViewModel = historyModels
                });
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("getHistoryById")]
        public IHttpActionResult GetHistoryById(long id)
        {
            try
            {
                var entityRow = _entityChangeHistoryService.GetHistoryById(id);

                if (entityRow == null)
                {
                    return HttpBadRequest($"no history record was found for given id {id}");
                }

                var user = _userService.GetUserById(entityRow.UserId);
                var historyModel = new HistoryViewModel
                {
                    Id = entityRow.Id,
                    EntityId = entityRow.EntityId,
                    EntityName = entityRow.Entity,
                    ModifiedDate = _settingService.GetTimeZoneDate(entityRow.ModifiedDate),
                    Action = $"{entityRow.Entity} {entityRow.State}",
                    Note = entityRow.Note,
                    User = user?.GetFullName(),
                    Role = user?.Roles.FirstOrDefault()?.Name,
                    Changes = new List<PropertyChangeHistory>(),
                };

                var propertyChangeHistory = new JavaScriptSerializer().Deserialize<List<PropertyChangeHistory>>(entityRow.ChangedData);

                foreach (var item in propertyChangeHistory)
                {
                    historyModel.Changes.Add(item);
                }

                return Ok(historyModel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("updateHistoryNote/{id}")]
        public IHttpActionResult UpdateHistoryNote(long id, [FromBody]HistoryModel historyModel)
        {
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var history = _entityChangeHistoryService.GetHistoryById(id);

                if (historyModel == null)
                {
                    return HttpBadRequest($"no history row was found for given id {id}");
                }

                history.Note = historyModel.Note;
                _entityChangeHistoryService.UpdateHistory(history);

                return Ok(history);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        #endregion methods
    }
}