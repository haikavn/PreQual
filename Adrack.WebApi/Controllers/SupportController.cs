using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using Adrack.Core;
using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.WebApi.Extensions;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models.New.Support;
using Adrack.WebApi.Models.Support;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/support/tickets")]
    public class SupportController : BaseApiController
    {
        private readonly ISupportTicketsService _supportTicketsService;
        private readonly ISupportTicketsMessagesService _supportTicketsMessagesService;
        private readonly IAppContext _appContext;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IUsersExtensionService _usersExtensionService;
        private readonly IBuyerService _buyerService;
        private readonly IAffiliateService _affiliateService;
        private readonly ISettingService _settingService;
        private readonly EmailOperatorEnums _emailProvider;
        private readonly IStorageService _storageService;

        public SupportController(
            ISupportTicketsService supportTicketsService, 
            ISupportTicketsMessagesService supportTicketsMessagesService,
            IEmailService emailService,
            IUserService userService,
            IUsersExtensionService usersExtensionService,
            IBuyerService buyerService,
            IAffiliateService affiliateService,
            IAppContext appContext,
            ISettingService settingService,
            IStorageService storageService)
        {
            _supportTicketsService = supportTicketsService;
            _supportTicketsMessagesService = supportTicketsMessagesService;
            _appContext = appContext;
            _emailService = emailService;
            _userService = userService;
            _usersExtensionService = usersExtensionService;
            _buyerService = buyerService;
            _affiliateService = affiliateService;
            _settingService = settingService;
            var emailProviderSetting = _settingService.GetSetting("System.EmailProvider");
            _emailProvider = emailProviderSetting != null ? (EmailOperatorEnums)Convert.ToInt16(emailProviderSetting.Value) : EmailOperatorEnums.LeadNative;
            _storageService = storageService;
        }

        [HttpGet]
        [Route("getAllSupportTickets")]
        public IHttpActionResult GetAllSupportTickets()
        {
            try
            {
                var supportTickets = _supportTicketsService.GetAllSupportTickets();

                foreach(var ticket in supportTickets)
                {
                    ticket.DateTime = _settingService.GetTimeZoneDate(ticket.DateTime);

                    if (ticket.DueDate.HasValue)
                        ticket.DueDate = _settingService.GetTimeZoneDate(ticket.DueDate.Value);
                }

                if (supportTickets == null)
                {
                    return NotFound();
                }

                return Ok(supportTickets);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("getSupportTicketsByKeyword")]
        public IHttpActionResult GetSupportTicketsByKeyword(string searchKey = "")
        {
            AllTicketsModel allTicketsModel = new AllTicketsModel();

            if (searchKey == null || string.IsNullOrEmpty(searchKey) || searchKey.Length < 3)
            {
                return Ok(allTicketsModel);
            }

            try
            {
                TicketsModel ticketsModel = new TicketsModel();

                var supportTickets = _supportTicketsService.GetSupportTicketsByKeyword(searchKey);

                foreach (SupportTicketsView stv in supportTickets)
                {
                    if (stv.UserID != 0)
                    {
                        User u = _userService.GetUserById(stv.UserID);
                        if (u != null)
                        {
                            switch (u.UserType)
                            {
                                case UserTypes.Buyer:
                                    {
                                        var buyerIds = _userService.GetUserEntityIds(stv.UserID, "Buyer", Guid.Empty);
                                        if (buyerIds != null)
                                        {
                                            Buyer b = _buyerService.GetBuyerById(buyerIds.FirstOrDefault());
                                            stv.CompanyName = b.Name;
                                        }
                                        break;
                                    }

                                case UserTypes.Affiliate:
                                    {
                                        var affiliateIds = _userService.GetUserEntityIds(stv.UserID, "Affiliate", Guid.Empty);
                                        if (affiliateIds != null)
                                        {
                                            Affiliate a = _affiliateService.GetAffiliateById(affiliateIds.FirstOrDefault(), true);
                                            stv.CompanyName = a.Name;
                                        }

                                        break;
                                    }
                                case UserTypes.Network:
                                    {
                                        stv.CompanyName = "Network User";
                                        break;
                                    }
                                case UserTypes.Super:
                                    {
                                        stv.CompanyName = "Super Admin";
                                        break;
                                    }
                                default:
                                    {
                                        stv.CompanyName = "Other";
                                        break;
                                    }
                            }
                        }

                        stv.DateTime = _settingService.GetTimeZoneDate(stv.DateTime);
                        if (stv.DueDate.HasValue)
                            stv.DueDate = _settingService.GetTimeZoneDate(stv.DueDate.Value);

                        allTicketsModel.supportTicketsViewItem.Add(stv);
                    }
                }

                if (allTicketsModel == null)
                {
                    return NotFound();
                }

                allTicketsModel.TotalCount = supportTickets.Count;

                return Ok(allTicketsModel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("getSupportTicketsByFilters")]
        public IHttpActionResult GetSupportTicketsByFilters(int status, DateTime date, DateTime? duedate = null, string userIds = "", string managerIds = "")
        {
            if (managerIds == null || string.IsNullOrEmpty(managerIds) || string.IsNullOrWhiteSpace(managerIds) || managerIds == "\"\"")
                managerIds = "";

            if (userIds == null || string.IsNullOrEmpty(userIds) || string.IsNullOrWhiteSpace(userIds) || userIds == "\"\"")
                userIds = "";

            try
            {
                TicketsModel ticketsModel = new TicketsModel();

                if (duedate == null)
                    duedate = DateTime.UtcNow.AddYears(1);

                duedate = _settingService.GetUTCDate(duedate.Value);
                date = _settingService.GetUTCDate(date);

                var supportTickets =  _supportTicketsService.GetSupportTicketsByFilters(userIds, managerIds, status, date, duedate);
                
                AllTicketsModel allTicketsModel = new AllTicketsModel();

                foreach (SupportTicketsView stv in supportTickets)
                {
                    if (stv.UserID != 0)
                    {
                        User u = _userService.GetUserById(stv.UserID);
                        if (u != null)
                        {
                            switch (u.UserType)
                            {
                                case UserTypes.Buyer:
                                    {
                                        var buyerIds = _userService.GetUserEntityIds(stv.UserID, "Buyer", Guid.Empty);
                                        if (buyerIds != null)
                                        {
                                            Buyer b = _buyerService.GetBuyerById(buyerIds.FirstOrDefault());
                                            if (b == null) continue;
                                            stv.CompanyName = b.Name;
                                        }
                                        break;
                                    }

                                case UserTypes.Affiliate:
                                    {
                                        var affiliateIds = _userService.GetUserEntityIds(stv.UserID, "Affiliate", Guid.Empty);
                                        if (affiliateIds != null)
                                        {
                                            Affiliate a = _affiliateService.GetAffiliateById(affiliateIds.FirstOrDefault(), true);
                                            if (a == null) continue;
                                            stv.CompanyName = a.Name;
                                        }

                                        break;
                                    }
                                case UserTypes.Network:
                                    {
                                        stv.CompanyName = "Network User";
                                        break;
                                    }
                                case UserTypes.Super:
                                    {
                                        stv.CompanyName = "Super Admin";
                                        break;
                                    }
                                default:
                                    {
                                        stv.CompanyName = "Other";
                                        break;
                                    }
                            }
                        }

                        stv.DateTime = _settingService.GetTimeZoneDate(stv.DateTime);
                        if (stv.DueDate.HasValue)
                            stv.DueDate = _settingService.GetTimeZoneDate(stv.DueDate.Value);

                        allTicketsModel.supportTicketsViewItem.Add(stv);
                    }
                }

                if (allTicketsModel == null)
                {
                    return NotFound();
                }

                allTicketsModel.TotalCount = supportTickets.Count;

                return Ok(allTicketsModel);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("getSupportTicketsbyUsers")]
        public IHttpActionResult GetSupportTicketsbyUsers(long userid, int type)
        {
            try
            {
                var supportTickets = _supportTicketsService.GetSupportTicketsbyUsers(userid, type);

                foreach (var ticket in supportTickets)
                {
                    ticket.DateTime = _settingService.GetTimeZoneDate(ticket.DateTime);

                    if (ticket.DueDate.HasValue)
                        ticket.DueDate = _settingService.GetTimeZoneDate(ticket.DueDate.Value);
                }

                if (supportTickets == null)
                {
                    return NotFound();
                }

                return Ok(supportTickets);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("getSupportTicketById/{id}")]
        public IHttpActionResult GetSupportTicketById(long id)
        {
            try
            {
                var supportTicket = _supportTicketsService.GetSupportTicketById(id);
                supportTicket.DateTime = _settingService.GetTimeZoneDate(supportTicket.DateTime);

                if (supportTicket.DueDate.HasValue)
                    supportTicket.DueDate = _settingService.GetTimeZoneDate(supportTicket.DueDate.Value);

                if (supportTicket == null)
                {
                    return NotFound();
                }

                return Ok(supportTicket);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


       

        [HttpPost]
        [Route("addSupportTicket")]
        public IHttpActionResult AddSupportTicket()
        {
            /*
            //public IHttpActionResult AddSupportTicket(SupportTicketsCreateModel model)

            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            model.UserId = _appContext.AppUser?.Id ?? 0;
            model.DateTime = DateTime.UtcNow;
            model.TicketType = model.TicketType == null ? (byte)0 : (byte)1;
            model.DueDate = model.DueDate == null ? DateTime.UtcNow.AddDays(1) : model.DueDate;

            var supportTicket = model.GetSupportTickets();
            */

            var httpRequest = HttpContext.Current.Request;
            var subject = httpRequest["Subject"];
            var message = httpRequest["Message"];


            if (string.IsNullOrEmpty(subject))
                return HttpBadRequest("Subject is required");

            if (subject.Length > 255 )
                return HttpBadRequest("Subject's MaxLength = 255");

            if (string.IsNullOrEmpty(message))
                return HttpBadRequest("Message is required");

            if (message.Length > 1024)
                return HttpBadRequest("Message's MaxLength = 1024");

            try
            {
                /*
                var managerId = string.IsNullOrEmpty(httpRequest["ManagerId"]) ? 0 : Convert.ToInt64(httpRequest["ManagerId"]);
                var status = string.IsNullOrEmpty(httpRequest["Status"]) ? 0 : Convert.ToUInt16(httpRequest["Status"]);
                var priority = string.IsNullOrEmpty(httpRequest["Priority"]) ? 0 : Convert.ToUInt16(httpRequest["Priority"]);
                var ticketType = string.IsNullOrEmpty(httpRequest["TicketType"]) ? (byte)0 : Convert.ToByte(httpRequest["Priority"]);
                var dueDate = string.IsNullOrEmpty(httpRequest["DueDate"]) ? DateTime.UtcNow.AddDays(1) : Convert.ToDateTime(httpRequest["DueDate"]);
                */

                var supportTicket = new SupportTickets
                {
                    Id = 0,
                    Subject = subject,
                    Message = message,
                    UserID = _appContext.AppUser?.Id ?? 0,
                    DateTime = DateTime.UtcNow,
                    ManagerID = string.IsNullOrEmpty(httpRequest["ManagerId"]) ? 0 : Convert.ToInt64(httpRequest["ManagerId"]),
                    Priority = string.IsNullOrEmpty(httpRequest["Priority"]) ? 0 : Convert.ToUInt16(httpRequest["Priority"]),
                    Status = string.IsNullOrEmpty(httpRequest["Status"]) ? 0 : Convert.ToUInt16(httpRequest["Status"]),
                    DueDate = string.IsNullOrEmpty(httpRequest["DueDate"]) ? DateTime.UtcNow.AddDays(1) : Convert.ToDateTime(httpRequest["DueDate"]),
                    TicketType = string.IsNullOrEmpty(httpRequest["TicketType"]) ? (byte)0 : Convert.ToByte(httpRequest["Priority"])
                };


                if (httpRequest.Files["TicketFilePath"] != null)
                {
                    var file = httpRequest.Files.Get("TicketFilePath");
                    if (file != null)
                    {
                        var ext = Path.GetExtension(file.FileName);
                        var fileName = $"ticket_file_{Guid.NewGuid()}{ext}";

                        var blobPath = "uploads";
                        var uri = _storageService.Upload(blobPath, file.InputStream, file.ContentType, fileName);
                        supportTicket.TicketFilePath = uri.AbsoluteUri;

                    }
                }

                supportTicket.Id = _supportTicketsService.InsertSupportTicket(supportTicket);

                User user = _userService.GetUserById(supportTicket.ManagerID);
                if (user != null)
                {
                    _emailService.SendNewTicket(user.Email, _usersExtensionService.GetFullName(user), _emailProvider);
                }

                return Ok(supportTicket);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpPost]
        [Route("editSupportTicket")]
        public IHttpActionResult EditSupportTicket(SupportTicketsCreateModel model)
        {
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());
            
            try
            {
                model.UserId = _appContext.AppUser?.Id ?? 0;
                model.DateTime = DateTime.UtcNow;
                model.TicketType = model.TicketType == null ? (byte)0 : (byte)1;
                model.DueDate = model.DueDate == null ? DateTime.UtcNow.AddDays(1) : model.DueDate;

                var supportTicket = model.GetSupportTickets();

                var supportTicketId = _supportTicketsService.UpdateSupportTicket(supportTicket);

                return Ok(supportTicket);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("addSupportTicketMessage")]
        public IHttpActionResult AddSupportTicketMessage()
        {
            //public IHttpActionResult AddSupportTicketMessage(SupportTicketMessageCreateModel model)
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState.GetErrorMessage());

            try
            {
                var httpRequest = HttpContext.Current.Request;

                var ticketId = string.IsNullOrEmpty(httpRequest["TicketId"]) ? 0 : Convert.ToInt64(httpRequest["TicketId"]);
                if (ticketId  <= 0)
                    return HttpBadRequest("TicketID is required");

                var authorId = string.IsNullOrEmpty(httpRequest["AuthorId"])? 0 : Convert.ToInt64(httpRequest["AuthorId"]);
                if (authorId <= 0)
                    return HttpBadRequest("AuthorID is required");

                var supportTicketMessage = new SupportTicketsMessages
                {
                    TicketID = ticketId,
                    AuthorID = authorId,
                    DateTime = DateTime.UtcNow,
                    IsNew = !string.IsNullOrEmpty(httpRequest["IsNew"]) && Convert.ToBoolean(httpRequest["IsNew"]),
                    Message = httpRequest["Message"],
                    FilePath = ""
                };
                

                if (httpRequest.Files["FilePath"] != null)
                {
                    var file = httpRequest.Files.Get("FilePath");
                    if (file != null)
                    {
                        var ext = Path.GetExtension(file.FileName);
                        var fileName = $"ticket_message_file_{Guid.NewGuid()}{ext}";

                        var blobPath = "uploads";
                        var uri = _storageService.Upload(blobPath, file.InputStream, file.ContentType, fileName);
                        supportTicketMessage.FilePath = uri.AbsoluteUri;

                    }
                }

                //var supportTicketMessage = model.GetSupportTicketsMessage();
                var supportTicketMessageId = _supportTicketsMessagesService.InsertSupportTicketsMessage(supportTicketMessage);
                supportTicketMessage.Id = supportTicketMessageId;

                var supportTicket = _supportTicketsService.GetSupportTicketById(supportTicketMessage.TicketID);

                User user = _userService.GetUserById(supportTicket.ManagerID);
                if (user != null)
                {
                    _emailService.SendNewTicketMessage(user.Email, _usersExtensionService.GetFullName(user), _emailProvider);
                }

                user = _userService.GetUserById(supportTicket.UserID);
                if (user != null)
                {
                    _emailService.SendNewTicketMessage(user.Email, _usersExtensionService.GetFullName(user), _emailProvider);
                }

                return Ok(supportTicketMessage);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("getSupportTicketFilesByTicketId")]
        public IHttpActionResult GetSupportTicketFilesByTicketId(long ticketId)
        {
            List<string> ticketFiles = new List<string>();
            try
            {
                var supportTicketMessages = _supportTicketsMessagesService.GetSupportTicketsMessages(ticketId);
                foreach(SupportTicketsMessagesView supportTicketMessage in supportTicketMessages){
                    ticketFiles.Add(supportTicketMessage.FilePath);
                }
          
                return Ok(ticketFiles);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("getSupportTicketMessagesByTicketId")]
        public IHttpActionResult GetSupportTicketMessagesByTicketId(long ticketId)
        {
            try
            {
                var supportTicketMessages = _supportTicketsMessagesService.GetSupportTicketsMessages(ticketId);
                var supportTicketsMessagesViewViewModels = supportTicketMessages.Select(supportTicketsMessagesView => supportTicketsMessagesView.GetViewModel()).ToList();

                foreach (var message in supportTicketsMessagesViewViewModels)
                {
                    message.DateTime = _settingService.GetTimeZoneDate(message.DateTime);
                }

                return Ok(supportTicketsMessagesViewViewModels);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }


        [HttpPost]
        [Route("addTicketsMessage")]
        public IHttpActionResult AddTicketsMessage([FromUri]long id, [FromBody]TicketMessagesModel supportTicketMessagesModel)
        {
            try
            {
                var supportTicketsMessages = new SupportTicketsMessages
                {
                    AuthorID = _appContext.AppUser?.Id ?? 0,
                    DateTime = DateTime.Now,
                    IsNew = true,
                    Message = supportTicketMessagesModel.Message,
                    TicketID = id,
                    FilePath = supportTicketMessagesModel.FilePath ?? string.Empty,
                };

                supportTicketsMessages.Id = _supportTicketsMessagesService.InsertSupportTicketsMessage(supportTicketsMessages);
                 
                return Ok(supportTicketsMessages);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("changeTicketStatus")]
        public IHttpActionResult ChangeTicketStatus([FromUri]long id, int status)
        {
            int retVal = _supportTicketsService.ChangeTicketsStatus(id, status);

            var supportTicket = _supportTicketsService.GetSupportTicketById(id);

            User user = _userService.GetUserById(supportTicket.UserID);

            if (user != null)
            {
                _emailService.SendTicketStatusChange(user.Email, _usersExtensionService.GetFullName(user));
            }

            return Ok(retVal);

        }
    }
}