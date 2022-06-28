using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.Web.Framework.Security;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models.AffiliateChannel;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Campaigns;
using Adrack.WebApi.Models.Common;
using Adrack.WebApi.Models.Lead;
using Adrack.WebApi.Models.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/general")]
    public class GeneralController : BaseApiController
    {
        private readonly IAffiliateService _affiliateService;
        private readonly IAffiliateChannelService _affiliateChannelService;
        private readonly IReportService _reportService;

        private readonly IBuyerService _buyerService;
        private readonly IBuyerChannelService _buyerChannelService;
        private readonly IRepository<BuyerInvoice> _buyerInvoiceRepository;
        private readonly IRepository<AffiliateInvoice> _affiliateInvoiceRepository;

        private readonly ICampaignService _campaignService;

        private readonly ISearchService _searchService;

        private readonly IUserService _userService;

        private readonly IVerticalService _verticalService;

        private readonly ICacheManager _cacheManager;

        public GeneralController(IAffiliateService affiliateService,
            IAffiliateChannelService affiliateChannelService,
            IBuyerService buyerService,
            IBuyerChannelService buyerChannelService,
            ICampaignService campaignService,
            ISearchService searchService,
            IUserService userService,
            IVerticalService verticalService,
            IReportService reportService,
            IRepository<BuyerInvoice> buyerInvoiceRepository,
            IRepository<AffiliateInvoice> affiliateInvoiceRepository,
        ICacheManager cacheManager)
        {
            _affiliateService = affiliateService;
            _affiliateChannelService = affiliateChannelService;
            _buyerService = buyerService;
            _buyerChannelService = buyerChannelService;
            _campaignService = campaignService;
            _searchService = searchService;
            _userService = userService;
            _verticalService = verticalService;
            _reportService = reportService;
            _buyerInvoiceRepository = buyerInvoiceRepository;
            _affiliateInvoiceRepository = affiliateInvoiceRepository;
            _cacheManager = cacheManager;
        }


        #region model field list methods
        /// <summary>
        /// Get Model Field List by XML, JSON, CSV
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Route("GetFieldListByReturnModelType")]
        public IHttpActionResult GetFieldListByReturnModelType([FromBody] InpStructureModel inpModel)
        {
            try
            {
                XmlDocument xmlDoc;
                string inpStr = inpModel.Value.Trim();

                if (inpStr.StartsWith("<") && inpStr.EndsWith(">"))
                {
                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(inpStr);
                }
                else if (inpStr.StartsWith("{") && inpStr.EndsWith("}") || inpStr.StartsWith("[") && inpStr.EndsWith("]"))
                {
                    //inpStr = "{'request':[" + inpStr + "]}";
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
                    xmlDoc.LoadXml("<request>" + strCsv + "</request>");
                }
                else
                {
                    return HttpBadRequest("The source file must be in XML or JSON or CSV or QUERYSTRING format");
                }

                if (inpModel.ReturnModelType.ToLower() == "campaignfield")
                {
                    return Ok(new { xml = xmlDoc.OuterXml, fields = getCampaignFieldList(xmlDoc) });
                }
                else if (inpModel.ReturnModelType.ToLower() == "affiliatechannelfield")
                {
                    return Ok(new { xml = xmlDoc.OuterXml, fields = getAffiliateChannelFieldList(xmlDoc) });
                }
                else if (inpModel.ReturnModelType.ToLower() == "buyerchannelfield")
                {
                    return Ok(new { xml = xmlDoc.OuterXml, fields = getBuyerChannelFieldList(xmlDoc) });
                }
                else
                {
                    return HttpBadRequest("The ReturnModelType must be campaignField or affiliateChannelField or buyerChannelField");
                }

            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", e.InnerException?.Message ?? e.Message);
                return HttpBadRequest(e.InnerException?.Message ?? e.Message);
            }
        }


        private List<CampaignFieldModel> getCampaignFieldList(XmlDocument xmlDoc)
        {
            List<CampaignFieldModel> modelField = new List<CampaignFieldModel>();

            var nodes = xmlDoc.SelectNodes("//*");
            if (nodes != null)
            {
                var rnd = new Random();

                foreach (XmlNode node in nodes)
                {
                    if ((node.FirstChild == null || (node.FirstChild != null && node.FirstChild.NodeType != XmlNodeType.Element)) && node.NodeType == XmlNodeType.Element)
                    {
                        var field = new CampaignFieldModel()
                        {
                            TemplateField = node.Name,
                            SectionName = (node.ParentNode != null) ? node.ParentNode.Name : string.Empty,
                            DatabaseField = "",
                            Description = "",
                            IsFilterable = false,
                            IsHash = false,
                            Required = false,
                            Validator = 0
                        };

                        if (modelField.Exists(x => x.TemplateField == field.TemplateField && x.SectionName == field.SectionName))
                        {
                            field.SectionName += rnd.Next(1000, 9999).ToString();
                        }

                        modelField.Add(field);
                    }
                }
            }

            return modelField;

        }


        private List<AffiliateChannelFieldModel> getAffiliateChannelFieldList(XmlDocument xmlDoc)
        {
            List<AffiliateChannelFieldModel> modelField = new List<AffiliateChannelFieldModel>();

            var nodes = xmlDoc.SelectNodes("//*");
            if (nodes != null)
            {
                var rnd = new Random();

                foreach (XmlNode node in nodes)
                {
                    if (node.FirstChild == null || (node.FirstChild != null && node.FirstChild.NodeType != XmlNodeType.Element))
                    {
                        var field = new AffiliateChannelFieldModel()
                        {
                            TemplateField = node.Name,
                            SectionName = (node.ParentNode != null) ? node.ParentNode.Name : string.Empty
                        };

                        if (modelField.Exists(x => x.TemplateField == field.TemplateField && x.SectionName == field.SectionName))
                        {
                            field.SectionName += rnd.Next(1000, 9999).ToString();
                        }

                        modelField.Add(field);
                    }
                }
            }

            return modelField;

        }


        private List<AffiliateChannelFieldModel> getBuyerChannelFieldList(XmlDocument xmlDoc)
        {
            List<AffiliateChannelFieldModel> modelField = new List<AffiliateChannelFieldModel>();

            var nodes = xmlDoc.SelectNodes("//*");
            if (nodes != null)
            {
                var rnd = new Random();

                foreach (XmlNode node in nodes)
                {
                    if (node.FirstChild == null || (node.FirstChild != null && node.FirstChild.NodeType != XmlNodeType.Element))
                    {
                        var field = new AffiliateChannelFieldModel()
                        {
                            TemplateField = node.Name,
                            SectionName = (node.ParentNode != null) ? node.ParentNode.Name : string.Empty
                        };

                        if (modelField.Exists(x => x.TemplateField == field.TemplateField && x.SectionName == field.SectionName))
                        {
                            field.SectionName += rnd.Next(1000, 9999).ToString();
                        }

                        modelField.Add(field);
                    }
                }
            }

            return modelField;
        }

        [HttpGet]
        [Route("search")]
        public IHttpActionResult Search(string inputValue)
        {
            List<SearchResultModel> searchResultModels = new List<SearchResultModel>();

            string affiliateInvocieKey = string.Format("App.Cache.AffiliateInvoice.Search.Result-{0}", inputValue);
            var affiliateInvoiceResults = _cacheManager.Get(affiliateInvocieKey, () =>
            {
                List<SearchResultModel> list = new List<SearchResultModel>();
                var affiliateInvoices = _affiliateInvoiceRepository.Table.Select(x => x);
                foreach (var affiliateInvoice in affiliateInvoices)
                {
                    if (_searchService.CheckPropValue(affiliateInvoice, inputValue))
                    {
                        list.Add(new SearchResultModel()
                        {
                            Entity = "affiliateInvoice",
                            Id = affiliateInvoice.Id,
                            Name = affiliateInvoice.Number.ToString()
                        });
                    }
                }
                return list;
            });
            searchResultModels.AddRange(affiliateInvoiceResults);

            string buyerInvocieKey = string.Format("App.Cache.BuyerInvoice.Search.Result-{0}", inputValue);
            var buyerInvoiceResults = _cacheManager.Get(buyerInvocieKey, () =>
            {
                List<SearchResultModel> list = new List<SearchResultModel>();
                var buyerInvoices = _buyerInvoiceRepository.Table.Select(x => x);
                foreach (var buyerInvoice in buyerInvoices)
                {
                    if (_searchService.CheckPropValue(buyerInvoice, inputValue))
                    {
                        list.Add(new SearchResultModel()
                        {
                            Entity = "buyerReport",
                            Id = buyerInvoice.Id,
                            Name = buyerInvoice.Number.ToString()
                        });
                    }
                }
                return list;
            });
            searchResultModels.AddRange(buyerInvoiceResults);

            string customReportKey = string.Format("App.Cache.CustomReport.Search.Result-{0}", inputValue);
            var customReportResults = _cacheManager.Get(customReportKey, () =>
            {
                List<SearchResultModel> list = new List<SearchResultModel>();
                var customReports = _reportService.GetAllCustomReports();
                foreach (var report in customReports)
                {
                    if (_searchService.CheckPropValue(report, inputValue))
                    {
                        list.Add(new SearchResultModel()
                        {
                            Entity = "customreport",
                            Id = report.Id,
                            Name = report.Name
                        });
                    }
                }
                return list;
            });
            searchResultModels.AddRange(customReportResults);
            string affilliateChannelKey = string.Format("App.Cache.AffiliateChannel.Search.Result-{0}", inputValue);
            var affiliateChannelResults = _cacheManager.Get(affilliateChannelKey, () =>
            {
                List<SearchResultModel> list = new List<SearchResultModel>();
                var affiliateChannels = _affiliateChannelService.GetAllAffiliateChannels();
                foreach (var channel in affiliateChannels)
                {
                    var affiliateChannel = CreateAffiliateChannelModel(channel);
                    if (_searchService.CheckPropValue(affiliateChannel, inputValue))
                    {
                        list.Add(new SearchResultModel()
                        {
                            Entity = "affiliatechannel",
                            Id = affiliateChannel.Id,
                            Name = affiliateChannel.Name
                        });
                    }
                }
                return list;
            });
            searchResultModels.AddRange(affiliateChannelResults);

            string buyerChannelKey = string.Format("App.Cache.BC.BuyerChannel.Search.Result-{0}", inputValue);
            var buyerChannelResults = _cacheManager.Get(buyerChannelKey, () =>
            {
                List<SearchResultModel> list = new List<SearchResultModel>();
                var buyersChannels = _buyerChannelService.GetAllBuyerChannels();
                foreach (var buyersChannel in buyersChannels)
                {
                    var buyerChannelModel = CreateBuyerChannelViewModel(buyersChannel);
                    if (_searchService.CheckPropValue(buyerChannelModel, inputValue))
                    {
                        list.Add(new SearchResultModel()
                        {
                            Entity = "buyerchannel",
                            Id = buyersChannel.Id,
                            Name = buyersChannel.Name
                        });
                    }
                }
                return list;
            });
            searchResultModels.AddRange(buyerChannelResults);

            string buyerKey = string.Format("App.Cache.Buyer.Search.Result-{0}", inputValue);
            var buyerResults = _cacheManager.Get(buyerKey, () =>
            {
                List<SearchResultModel> list = new List<SearchResultModel>();
                var buyers = _buyerService.GetAllBuyers();
                foreach (var buyer in buyers)
                {
                    var buyerModel = CreateBuyerViewModel(buyer);
                    if (_searchService.CheckPropValue(buyerModel, inputValue))
                    {
                        list.Add(new SearchResultModel()
                        {
                            Entity = "buyer",
                            Id = buyer.Id,
                            Name = buyer.Name
                        });
                    }
                }
                return list;
            });
            searchResultModels.AddRange(buyerResults);

            string campaignKey = string.Format("App.Cache.Campaign.Search.Result-{0}", inputValue);
            var campaignResults = _cacheManager.Get(campaignKey, () =>
            {
                List<SearchResultModel> list = new List<SearchResultModel>();
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
                        list.Add(new SearchResultModel()
                        {
                            Entity = "campaign",
                            Id = campaign.Id,
                            Name = campaign.Name
                        });
                    }
                }
                return list;
            });
            searchResultModels.AddRange(campaignResults);


            string userKey = "App.Cache.User.Search.Result";
            var userResults = _cacheManager.Get(userKey, () =>
            {
                List<SearchResultModel> list = new List<SearchResultModel>();
                var users = _userService.GetUsers();
                foreach (var user in users)
                {
                    var userModel = new UserInfoViewModel()
                    {
                        Id = user.Id,
                        UserName = user.Username,
                        Email = user.Email,
                        BuiltInName = user.BuiltInName
                    };

                    if (_searchService.CheckPropValue(userModel, inputValue))
                    {
                        list.Add(new SearchResultModel()
                        {
                            Entity = "user",
                            Id = userModel.Id,
                            Name = userModel.UserName
                        });
                    }
                }
                return list;
            });
            searchResultModels.AddRange(userResults);

            return Ok(searchResultModels.OrderBy(x => x.Name).ToList());
        }

        #endregion

        #region Private methods

        private AffiliateChannelViewModel CreateAffiliateChannelModel(AffiliateChannel channel)
        {
            var affiliate = _affiliateService.GetAffiliateById(channel.AffiliateId, false);
            Campaign campaign = null;
            if (channel.CampaignId != null)
            {
                campaign = _campaignService.GetCampaignById(channel.CampaignId.Value, false);
            }

            return new AffiliateChannelViewModel
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
            };
        }

        private BuyersChannelViewModel CreateBuyerChannelViewModel(BuyerChannel buyerChannel)
        {
            var buyerName = string.Empty;
            var campaignName = string.Empty;
            var buyer = _buyerService.GetBuyerById(buyerChannel.BuyerId);
            var campaign = _campaignService.GetCampaignById(buyerChannel.CampaignId);
            if (buyer != null)
            {
                buyerName = buyer.Name;
            }
            if (campaign != null)
            {
                campaignName = campaign.Name;
            }

            return new BuyersChannelViewModel
            {
                BuyerId = buyerChannel.BuyerId,
                BuyerName = buyerName,
                BuyersChannelId = buyerChannel.Id,
                CampaignId = buyerChannel.CampaignId,
                Campaign = campaignName,
                BuyersChannelName = buyerChannel.Name,
                Status = (ActivityStatuses)buyerChannel.Status,
            };
        }

        private BuyerViewModel CreateBuyerViewModel(Buyer buyer)
        {

            string managerEmail = string.Empty;
            if (buyer.ManagerId.HasValue)
            {
                var manager = _userService.GetUserById(buyer.ManagerId.Value) ?? null;
                managerEmail = manager?.ContactEmail;
            }

            return new BuyerViewModel
            {
                BuyerId = buyer.Id,
                ManagerId = buyer.ManagerId,
                Manager = managerEmail,
                Phone = buyer.Phone,
                CountryName = buyer.Country.Name,
                CountryId = buyer.Country.Id,
                StateProvinceId = buyer.StateProvince != null ? buyer.StateProvince.Id : 0,
                StateProvinceName = buyer.StateProvince != null ? buyer.StateProvince.Name : "",
                ZipCode = buyer.ZipPostalCode,
                Name = buyer.Name,
                Status = buyer.Status,
                BuyerType = (BuyerType)buyer.AlwaysSoldOption
            };
        }

        #endregion
    }
}