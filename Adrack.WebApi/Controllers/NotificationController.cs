using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Directory;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Localization;
using Adrack.Core.Domain.Security;
using Adrack.Service.Configuration;
using Adrack.Service.Directory;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.Web.Framework.Security;
using Adrack.WebApi.Infrastructure.Enums;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Common;
using Adrack.WebApi.Models.Localization;
using Adrack.Service.Notification;
using Braintree;
using Newtonsoft.Json;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Notification;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.WebApi.Models.Notification;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/notification")]
    public class NotificationController : BaseApiPublicController
    {
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IDepartmentService _departmentService;
        private readonly ILocalizedStringService _localizedStringService;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;
        private readonly IAppContext _appContext;
        private readonly ICacheManager _cacheManager;
        private readonly IVerticalService _verticalService;
        private readonly ICampaignTemplateService _campaignTemplateService;
        private readonly INotificationService _notificationService;
        private readonly IRepository<Core.Domain.Notification.UserNotification> _userNotificationRepository;

        public NotificationController(ICountryService countryService,
                              IStateProvinceService stateProvinceService,
                              IDepartmentService departmentService,
                              ILocalizedStringService localizedStringService,
                              IRoleService roleService,
                              IUserService userService,
                              ISettingService settingService,
                              IAppContext appContext,
                              ICacheManager cacheManager,
                              IVerticalService verticalService,
                              ICampaignTemplateService campaignTemplateService,
                              INotificationService notificationService,
                              IRepository<Core.Domain.Notification.UserNotification> userNotificationRepository)
        {
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
            _departmentService = departmentService;
            _localizedStringService = localizedStringService;
            _roleService = roleService;
            _userService = userService;
            _settingService = settingService;
            _appContext = appContext;
            _cacheManager = cacheManager;
            _verticalService = verticalService;
            _campaignTemplateService = campaignTemplateService;
            _notificationService = notificationService;
            _userNotificationRepository = userNotificationRepository;
        }

        #region HTTP methods

        [HttpPost]
        [Route("send")]
        public IHttpActionResult Send([FromBody] Adrack.Core.Domain.Notification.Notification notification)
        {
            try
            {
                var user = _userService.GetUserById(notification.UserId);
                if (user != null)
                {
                    var result = _notificationService.Send(notification);
                    return Ok(result);
                }
                return HttpBadRequest("The provided user doesn't exist");
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("bulksend")]
        public IHttpActionResult BulkSend([FromBody] Models.Notification.SendBulkNotificationModel notification)
        {
            try
            {
                var users = new List<User>();
                if (notification.UserIds.Any())
                {
                    notification.UserIds.ForEach(u =>
                    {
                        notification.Notification.UserId = u;
                        users.Add(new Core.Domain.Membership.User()
                        {
                            Id = u
                        });
                    });
                }
                _notificationService.BulkSendNotification(users, notification.Notification);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("getnotifications/{userId}/{isRead?}")]
        public IHttpActionResult GetNotifications(long userId, bool? isRead = null)
        {
            try
            {
                var user = _userService.GetUserById(userId);
                if (user == null)
                {
                    return HttpBadRequest("User doesn't exist");
                }

                var notifications = _notificationService.GetNotifications(user.Id);
                if (notifications == null)
                {
                    return HttpBadRequest("User's notification doesn't exist");
                }

                if (isRead.HasValue)
                {
                    notifications = notifications.Where(x => x.IsRead == isRead.Value).ToList();
                }

                return Ok(notifications);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPut]
        [Route("readnotification/{notificationId}")]
        public IHttpActionResult ReadNotification([FromUri] long notificationId)
        {
            try
            {
                var notification = _notificationService.GetNotification(notificationId, _appContext.AppUser.Id);
                //var notification = _userNotificationRepository.Table.Where(un => un.NotificationId == notificationId && un.UserId == _appContext.AppUser.Id).FirstOrDefault();
                if (notification != null)
                {
                    var result = _notificationService.Read(notificationId);

                    if (result == null)
                        return HttpBadRequest("No user notification found");

                    return Ok(new { 
                        Title = notification.Title,
                        Message = notification.Message,
                        NotificationType = notification.NotificationType,
                        UserId = _appContext.AppUser.Id,
                        DateTimeUtc = result.DateTimeUtc,
                        IsRead = result.IsRead,
                        Id = notification.Id
                    });
                }
                else
                {
                    return HttpBadRequest("Provided notification doesn't exist");
                }
                
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("countries")]
        public IHttpActionResult GetAllCountries()
        {
            try
            {
                var countries = GetCountries();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("stateProvinces")]
        public IHttpActionResult GetAllStateProvinces(int countryId)
        {
            try
            {
                var stateProvinces = GetStateProvinces(countryId);
                return Ok(stateProvinces);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getZipCodeLength/{countryId}")]
        public IHttpActionResult GetZipCodeLength(int countryId)
        {
            try
            {

                var country = GetCountries().FirstOrDefault(x => x.Id == countryId);
                if (country == null)
                {
                    return HttpBadRequest("Wrong country Id is provided");
                }

                return Ok(country.ZipCodeLength);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("departments")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAllDepartments()
        {
            try
            {
                var departments = GetDepartments();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("localizedStrings")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAllLocalizedStrings()
        {
            try
            {
                var localizedStrings = _localizedStringService.GetAllLocalizedStrings().ToList();
                var localizedStringList = new List<LocalizedStringModel>();
                foreach (var item in localizedStrings)
                {
                    localizedStringList.Add((LocalizedStringModel)item);
                }
                return Ok(localizedStringList);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("roles")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAllRoles()
        {
            try
            {
                var roles = GetRoles();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("userTypes")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAllUserTypes()
        {
            try
            {
                var userTypes = GetUserTypes();
                return Ok(userTypes);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("dataFormats")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAllDataFormats()
        {
            try
            {
                var dataFormats = GetDataFormats();
                return Ok(dataFormats);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getValidators")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAllValidators()
        {
            try
            {
                var validators = GetValidators();
                return Ok(validators);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getConditions")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAllConditions([FromUri]byte? validator = null)
        {
            try
            {
                var conditions = GetConditions(validator);
                return Ok(conditions);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getAffiliatePriceMethods")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAffiliatePriceMethods()
        {
            try
            {
                var validators = GetAffiliatePriceMethodList();
                return Ok(validators);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getAffiliateChannelPriceMethods")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAffiliateChannelPriceMethods()
        {
            try
            {
                var priceMethods = GetAffiliateChannelPriceMethodList();
                return Ok(priceMethods);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("listsByKeys")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetListsByKeys([FromUri]DataKey[] keys, [FromUri] object param = null)
        {
            try
            {
                var list = new List<KeyValuePair<string, object>>();
                foreach (var key in keys)
                {
                    switch (key)
                    {
                        case DataKey.Country:
                            var countries = GetCountries();
                            list.Add(new KeyValuePair<string, object>(key.ToString(), countries));
                            break;
                        case DataKey.StateProvince:
                            int countryId = 0;
                            if (param != null)
                            {
                                int.TryParse(param.ToString(), out countryId);
                            }
                            var stateProvinces = GetStateProvinces(countryId);
                            list.Add(new KeyValuePair<string, object>(key.ToString(), stateProvinces));
                            break;
                        case DataKey.Role:
                            var roles = GetRoles();
                            list.Add(new KeyValuePair<string, object>(key.ToString(), roles));
                            break;
                        case DataKey.UserType:
                            var userTypes = GetUserTypes();
                            list.Add(new KeyValuePair<string, object>(key.ToString(), userTypes));
                            break;
                        case DataKey.Department:
                            var departments = GetDepartments();
                            list.Add(new KeyValuePair<string, object>(key.ToString(), departments));
                            break;
                    }
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getSystemFieldsByVerticalId/{verticalId}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetSystemFieldsByVerticalId(long verticalId)
        {
            try
            {
                var list = new List<string>();

                list.Add("Firstname");
                list.Add("Lastname");
                list.Add("Email");
                list.Add("SSN");
                list.Add("Address");
                list.Add("State");
                list.Add("Zip");

                return Ok(list);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getColumnsVisibility/{page}")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetColumnsVisibility(string page)
        {
            var setting = _settingService.GetSetting($"ColumnsVisibility.{page}.{_appContext.AppUser.Id}");
            if (setting == null)
            {
                setting = _settingService.GetSetting($"ColumnsVisibility.{page}");
            }

            if (setting == null)
            {
                //return HttpBadRequest($"no setting was found for given page {page}");
                return Ok(new List<ColumnsVisibilityModel>());
            }

            var columns = JsonConvert.DeserializeObject<List<ColumnsVisibilityModel>>(setting.Value);
            columns = columns.GroupBy(x => x.Name).Select(x => x.First()).ToList();

            return Ok(columns);
        }


        [HttpPut]
        [Route("setColumnsVisibility")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult SetColumnsVisibility([FromBody]ColumnVisibilityUpdateModel columnsVisibilityModel)
        {
            var key = $"ColumnsVisibility.{columnsVisibilityModel.Page}.{_appContext.AppUser.Id}";
            var setting = _settingService.GetSetting(key);
            var columns = JsonConvert.SerializeObject(columnsVisibilityModel.ColumnsVisibilities);
            if (setting == null)
            {
                setting = new Setting
                {
                    Id = 0,
                    Key = key,
                    Value = columns,
                    Description = null
                };
                _settingService.InsertSetting(setting);
            }
            else
            {
                setting.Value = columns;
                _settingService.UpdateSetting(setting);
            }

            return Ok();
        }

        [HttpGet]
        [Route("getEntityTypes")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAllEntityTypeResults()
        {
            try
            {
                var entityTypes = GetEntityTypes();
                return Ok(entityTypes);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getLeadStatusList")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetLeadStatusResults()
        {
            try
            {
                var status = GetLeadStatusList();
                return Ok(status);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getBillFrequencyList")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult BillFrequencyResults()
        {
            try
            {
                var billFrList = GetBillFrequencyList();
                return Ok(billFrList);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getDateTimeFormats")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetDateTimeFormats()
        {
            string[] dateFormats = DateTimeFormatInfo.CurrentInfo.GetAllDateTimePatterns('d');

            return Ok(dateFormats);
        }


        [HttpPost]
        [Route("clearCache")]
        public IHttpActionResult ClearCache()
        {
            _cacheManager.Clear();
            return Ok();
        }

        [HttpGet]
        [Route("getBuyerTypes")]
        [ContentManagementApiAuthorize(false)]
        public IHttpActionResult GetAllBuyerTypeResults()
        {
            try
            {
                var buyerTypes = GetBuyerTypes();
                return Ok(buyerTypes);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getCompanyIndustries")]
        public IHttpActionResult GetAllCompanyIndustry()
        {
            try
            {
                var industries = GetCompanyIndustries();
                return Ok(industries);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("getDataFormat")]
        public IHttpActionResult GetDataFormat(Validators validator)
        {
            try
            {
                List<DataFormatTypeModel> dataFormatTypeModels = new List<DataFormatTypeModel>();
                var dataFormatTypeModel = new DataFormatTypeModel();

                switch (validator)
                {
                    case Validators.String:
                        dataFormatTypeModel = new DataFormatTypeModel();
                        dataFormatTypeModel.DataFormatType = DataFormatTypes.EditBox;
                        dataFormatTypeModel.Name = "Min length";
                        dataFormatTypeModel.Values.Add("1");
                        dataFormatTypeModels.Add(dataFormatTypeModel);

                        dataFormatTypeModel = new DataFormatTypeModel();
                        dataFormatTypeModel.DataFormatType = DataFormatTypes.EditBox;
                        dataFormatTypeModel.Name = "Max length";
                        dataFormatTypeModel.Values.Add("150");
                        dataFormatTypeModels.Add(dataFormatTypeModel);
                        break;
                    case Validators.DateOfBirth:
                    case Validators.DateTime:
                        string[] formats = DateTimeFormatInfo.CurrentInfo.GetAllDateTimePatterns('d');

                        dataFormatTypeModel = new DataFormatTypeModel();
                        dataFormatTypeModel.DataFormatType = DataFormatTypes.DropDown;
                        dataFormatTypeModel.Name = "Date formats";
                        dataFormatTypeModel.Values.AddRange(formats);
                        dataFormatTypeModels.Add(dataFormatTypeModel);
                        break;
                }

                return Ok(dataFormatTypeModels);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getTimeZones")]
        public IHttpActionResult GetTimeZones()
        {
            List<object> timeZones = new List<object>();

            try
            {
                foreach (TimeZoneInfo z in TimeZoneInfo.GetSystemTimeZones())
                {
                    timeZones.Add(new { id = z.Id, name = z.DisplayName });
                }

                return Ok(timeZones);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        #endregion


        #region Private methods
        private List<IdNameModel> GetStateProvinces(int countryId)
        {
            var stateProvinces = _stateProvinceService.GetStateProvinceByCountryId(countryId);

            return stateProvinces.Select(x => new IdNameModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        private List<IdNameModel> GetRoles()
        {
            var roles = _roleService.GetAllRoles();
            return roles.Select(item => new IdNameModel()
            {
                Id = item.Id,
                Name = item.Name
            }).ToList();
        }
        private List<IdNameModel> GetDepartments()
        {
            var departments = _departmentService.GetAllDepartments();
            return departments.Select(item => new IdNameModel()
            {
                Id = item.Id,
                Name = item.Name

            }).ToList();
        }

        private List<CountryViewModel> GetCountries()
        {
            var countries = _countryService.GetAllCountries();

            return countries.Select(x => new CountryViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.TwoLetteroCode,
                ZipCodeLength = x.ZipLength
            }).ToList();
        }

        private List<ValidatorModel> GetValidators()
        {
            var validatorModels = new List<ValidatorModel>();

            var validators = Enum.GetValues(typeof(Validators)).Cast<Validators>().ToList();
            foreach (var validator in validators)
            {
                validatorModels.Add(new ValidatorModel
                {
                    Id = (byte)validator,
                    Name = validator.ToString()
                });
            }

            return validatorModels;
        }

        private List<ConditionModel> GetConditions(byte? validator)
        {
            var conditionModels = new List<ConditionModel>();

            var conditions = Enum.GetValues(typeof(Conditions)).Cast<Conditions>().ToList();
            if (validator.HasValue)
            {
                switch (validator)
                {
                    case (byte)Validators.Email:
                    case (byte)Validators.String:
                    case (byte)Validators.Zip:
                    case (byte)Validators.Phone:
                    case (byte)Validators.Ssn:
                    case (byte)Validators.AccountNumber:
                    case (byte)Validators.State:
                        conditions = conditions.Where(x => x == Conditions.Contains ||
                                                           x == Conditions.NotContains ||
                                                           x == Conditions.StartsWith ||
                                                           x == Conditions.EndsWith ||
                                                           x == Conditions.NotEquals ||
                                                           x == Conditions.StringByLength).ToList();
                        break;
                    case (byte)Validators.Number:
                    case (byte)Validators.RoutingNumber:
                    case (byte)Validators.Decimal:
                    case (byte)Validators.SubId:
                        conditions = conditions.Where(x => x == Conditions.Equals ||
                                                           x == Conditions.NotEquals ||
                                                           x == Conditions.NumberGreater ||
                                                           x == Conditions.NumberGreaterOrEqual ||
                                                           x == Conditions.NumberLess ||
                                                           x == Conditions.NumberLessOrEqual ||
                                                           x == Conditions.NumberRange ||
                                                           x == Conditions.StringByLength).ToList();
                        break;
                    case (byte)Validators.DateTime:
                    case (byte)Validators.DateOfBirth:
                        conditions = conditions.Where(x => x == Conditions.Equals ||
                                                           x == Conditions.NotEquals ||
                                                           x == Conditions.NumberGreater ||
                                                           x == Conditions.NumberGreaterOrEqual ||
                                                           x == Conditions.NumberLess ||
                                                           x == Conditions.NumberLessOrEqual ||
                                                           x == Conditions.StringByLength).ToList();
                        break;

                }
            }

            foreach (var condition in conditions)
            {
                conditionModels.Add(new ConditionModel
                {
                    Id = (byte)condition,
                    Name = condition.ToString()
                });
            }

            return conditionModels;
        }

        [HttpGet]
        [Route("setUserTypes")]
        private List<IdNameModel> GetUserTypes()
        {
            var userTypesModel = new List<IdNameModel>();

            var userTypes = Enum.GetValues(typeof(UserTypes)).Cast<UserTypes>().ToList();
            foreach (var userType in userTypes)
            {
                userTypesModel.Add(new IdNameModel
                {
                    Id = (int)userType,
                    Name = userType.ToString()
                });
            }

            return userTypesModel;
        }

        private List<IdNameModel> GetAffiliatePriceMethodList()
        {
            var affiliatePriceMethodList = new List<IdNameModel>();

            var affiliatePriceMethods = Enum.GetValues(typeof(AffilatePriceMethods)).Cast<AffilatePriceMethods>().ToList();
            foreach (var affiliatePriceMethod in affiliatePriceMethods)
            {
                affiliatePriceMethodList.Add(new IdNameModel
                {
                    Id = (int)affiliatePriceMethod,
                    Name = affiliatePriceMethod.ToString()
                });
            }

            return affiliatePriceMethodList;
        }

        private List<IdNameModel> GetAffiliateChannelPriceMethodList()
        {
            var affiliatePriceMethodList = new List<IdNameModel>();

            var affiliatePriceMethods = Enum.GetValues(typeof(AffiliateChannelPriceMethods)).Cast<AffiliateChannelPriceMethods>().ToList();
            foreach (var affiliatePriceMethod in affiliatePriceMethods)
            {
                affiliatePriceMethodList.Add(new IdNameModel
                {
                    Id = (int)affiliatePriceMethod,
                    Name = affiliatePriceMethod.ToString()
                });
            }

            return affiliatePriceMethodList;
        }

        private List<IdNameModel> GetDataFormats()
        {
            var formatModels = new List<IdNameModel>();

            var formats = Enum.GetValues(typeof(DataFormat)).Cast<DataFormat>().ToList();
            foreach (var format in formats)
            {
                formatModels.Add(new IdNameModel()
                {
                    Id = Convert.ToInt64(format),
                    Name = format.ToString()
                });
            }

            return formatModels;
        }

        private List<IdNameModel> GetBuyerTypes()
        {
            var buyerTypesModel = new List<IdNameModel>();

            var buyerTypes = Enum.GetValues(typeof(BuyerType)).Cast<BuyerType>().ToList();
            foreach (var type in buyerTypes)
            {
                buyerTypesModel.Add(new IdNameModel
                {
                    Id = (int)type,
                    Name = type.ToString()
                });
            }

            return buyerTypesModel;
        }

        private List<IdNameModel> GetCompanyIndustries()
        {
            var industryModels = new List<IdNameModel>();

            var industries = _verticalService.GetAllVerticals();
            foreach (var industry in industries)
            {
                industryModels.Add(new IdNameModel
                {
                    Id = industry.Id,
                    Name = industry.Name
                });
            }

            return industryModels;
        }

        private List<IdNameModel> GetEntityTypes()
        {
            var entityTypesModel = new List<IdNameModel>();

            var entityTypes = Enum.GetValues(typeof(EntityType)).Cast<EntityType>().ToList();
            foreach (var type in entityTypes)
            {
                entityTypesModel.Add(new IdNameModel
                {
                    Id = (int)type,
                    Name = type.ToString()
                });
            }

            return entityTypesModel;
        }

        private List<IdNameModel> GetLeadStatusList()
        {
            var StatusModelList = new List<IdNameModel>();

            var statusList = Enum.GetValues(typeof(LeadResponseStatus)).Cast<LeadResponseStatus>().ToList();
            foreach (var type in statusList)
            {
                StatusModelList.Add(new IdNameModel
                {
                    Id = (int)type,
                    Name = type.ToString()
                });
            }

            return StatusModelList;
        }

        private List<IdNameModel> GetBillFrequencyList()
        {
            var BillFrequencyList = new List<IdNameModel>();

            var billFrequencyList = Enum.GetValues(typeof(BillFrequency)).Cast<BillFrequency>().ToList();
            foreach (var type in billFrequencyList)
            {
                BillFrequencyList.Add(new IdNameModel
                {
                    Id = (int)type,
                    Name = type.GetDescription()
                });
            }

            return BillFrequencyList;
        }

        #endregion
    }
}