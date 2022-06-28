using System;
using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Content;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Message;
using Adrack.Service.Membership;
using Adrack.WebApi.Infrastructure.Enums;
using Adrack.WebApi.Models.Membership.Register;
using Adrack.WebApi.Models.AffiliateChannel;
using Adrack.WebApi.Models.New.BuyerChannel;
using Adrack.WebApi.Models.New.Lead;
using Adrack.WebApi.Models.New.Membership;
using Adrack.WebApi.Models.New.Support;
using Adrack.Core;

namespace Adrack.WebApi.Extensions
{
    public static class ModelExtensions
    {
        #region Affiliate

        public static AffiliateViewModel GetViewModel(this Affiliate affiliate)
        {
            var model = new AffiliateViewModel
            {
                Id = affiliate.Id,
                Name = affiliate.Name,
                Status = affiliate.Status,
                Deleted = affiliate.IsDeleted,
                Email = affiliate.Email,
                City = affiliate.City,
                AddressLine1 = affiliate.AddressLine1,
                AddressLine2 = affiliate.AddressLine2,
                BillFrequency = affiliate.BillFrequency,
                BillWithin = affiliate.BillWithin,
                CountryId = affiliate.CountryId,
                CreatedOn = affiliate.CreatedOn,
                DefaultAffiliatePrice = affiliate.DefaultAffiliatePrice,
                DefaultAffiliatePriceMethod = affiliate.DefaultAffiliatePriceMethod,
                FrequencyValue = affiliate.FrequencyValue,
                IsBiWeekly = affiliate.IsBiWeekly,
                ManagerId = affiliate.ManagerId,
                Phone = affiliate.Phone,
                RegistrationIp = affiliate.RegistrationIp,
                StateProvinceId = affiliate.StateProvinceId,
                UserId = affiliate.UserId,
                Website = affiliate.Website,
                WhiteIp = affiliate.WhiteIp,
                ZipPostalCode = affiliate.ZipPostalCode
            };
            return model;
        }

        #endregion
        
        #region AffiliateChannel

        public static AffiliateChannelViewModel GetViewModel(this AffiliateChannel affiliateChannel)
        {
            var model = new AffiliateChannelViewModel
            {
                Id = affiliateChannel.Id,
                Name = affiliateChannel.Name,
                AffiliateChannelKey = affiliateChannel.ChannelKey,
                AffiliateId = affiliateChannel.AffiliateId,
                AffiliatePrice = affiliateChannel.AffiliatePrice,
                AffiliatePriceMethod = affiliateChannel.AffiliatePriceMethod,
                CampaignId = affiliateChannel.CampaignId,
                DataFormat = affiliateChannel.DataFormat,
                Deleted = affiliateChannel.IsDeleted,
                MinPriceOption = affiliateChannel.MinPriceOption,
                MinPriceOptionValue = affiliateChannel.NetworkTargetRevenue,
                MinRevenue = affiliateChannel.NetworkMinimumRevenue,
                Note = affiliateChannel.Note,
                Status = affiliateChannel.Status,
                Timeout = affiliateChannel.Timeout,
                XmlTemplate = affiliateChannel.XmlTemplate
            };

            return model;
        }

        //public static AffiliateChannel GetAffiliateChannel(this AffiliateChannelCreateModel model)
        //{
        //    var affiliateChannel = new AffiliateChannel
        //    {
        //        Name = model.Name,
        //        AffiliateChannelKey = model.AffiliateChannelKey,
        //        AffiliateId = model.AffiliateId,
        //        AffiliatePrice = model.AffiliatePrice,
        //        AffiliatePriceMethod = model.AffiliatePriceMethod,
        //        CampaignId = model.CampaignId,
        //        DataFormat = model.DataFormat,
        //        Deleted = model.Deleted,
        //        MinPriceOption = model.MinPriceOption,
        //        MinPriceOptionValue = model.MinPriceOptionValue,
        //        MinRevenue = model.MinRevenue,
        //        Note = model.Note,
        //        Status = model.Status,
        //        Timeout = model.Timeout,
        //        XmlTemplate = model.XmlTemplate
        //    };

        //    return affiliateChannel;
        //}


        public static AffiliateChannel GetAffiliateChannel(this AffiliateChannelUpdateModel model)
        {
            var affiliateChannel = new AffiliateChannel
            {
                Id = model.Id,
                Name = model.Name,
                ChannelKey = model.AffiliateChannelKey,
                AffiliateId = model.AffiliateId,
                AffiliatePrice = model.AffiliatePrice,
                AffiliatePriceMethod = model.AffiliatePriceMethod,
                CampaignId = model.CampaignId,
                DataFormat = model.DataFormat,
                IsDeleted = model.Deleted,
                MinPriceOption = model.MinPriceOption,
                NetworkTargetRevenue = model.MinPriceOptionValue,
                NetworkMinimumRevenue = model.MinRevenue,
                Note = model.Note,
                Status = model.Status,
                Timeout = model.Timeout,
                XmlTemplate = model.XmlTemplate
            };

            return affiliateChannel;
        }

        public static AffiliateChannelFilterCondition GetAffiliateChannelFilterCondition(this AffiliateChannelFilterCreateModel model, long affiliateChannelId)
        {
            var affiliateChannelFilterCondition = new AffiliateChannelFilterCondition
            {
                AffiliateChannelId = affiliateChannelId,
                CampaignTemplateId = model.CampaignFieldId,
                Condition = model.Condition,
                ConditionOperator = 0,
                ParentId = model.ParentId,
                Value = model.GetValue()
            };
            return affiliateChannelFilterCondition;
        }

        #endregion

        #region AffiliateResponse

        public static AffiliateResponseViewModel GetViewModel(this AffiliateResponse affiliateResponse)
        {
            var model = new AffiliateResponseViewModel
            {
                Id = affiliateResponse.Id,
                Status = affiliateResponse.Status,
                AffiliateId = affiliateResponse.AffiliateId,
                Created = affiliateResponse.Created,
                AffiliateChannelId = affiliateResponse.AffiliateChannelId,
                Message = affiliateResponse.Message,
                State = affiliateResponse.State,
                ReceivedData = affiliateResponse.ReceivedData,
                ErrorType = affiliateResponse.ErrorType,
                MinPrice = affiliateResponse.MinPrice,
                LeadId = affiliateResponse.LeadId,
                ProcessStartedAt = affiliateResponse.ProcessStartedAt,
                Response = affiliateResponse.Response,
                Validator = affiliateResponse.Validator
            };

            return model;
        }

        #endregion

        #region Buyer

        public static BuyerViewModel GetViewModel(this Buyer buyer)
        {
            var model = new BuyerViewModel
            {
                Id = buyer.Id,
                Name = buyer.Name,
                Status = buyer.Status,
                Deleted = buyer.Deleted,
                Email = buyer.Email,
                City = buyer.City,
                MaxDuplicateDays = buyer.MaxDuplicateDays,
                DailyCap = buyer.DailyCap,
                TypeId = buyer.AlwaysSoldOption,
                ManagerId = buyer.ManagerId,
                StateProvinceId = buyer.StateProvinceId,
                BillFrequency = buyer.BillFrequency,
                Phone = buyer.Phone,
                AddressLine1 = buyer.AddressLine1,
                AddressLine2 = buyer.AddressLine2,
                CreatedOn = buyer.CreatedOn,
                ZipPostalCode = buyer.ZipPostalCode,
                CountryId = buyer.CountryId,
                FrequencyValue = buyer.FrequencyValue,
                IsBiWeekly = buyer.IsBiWeekly,
                AccountId = buyer.AccountId,
                CanSendLeadId = buyer.CanSendLeadId,
                CoolOffEnabled = buyer.CoolOffEnabled,
                CoolOffEnd = buyer.CoolOffEnd,
                CoolOffStart = buyer.CoolOffStart,
                Description = buyer.Description,
                DoNotPresentPostMethod = buyer.DoNotPresentPostMethod,
                DoNotPresentRequest = buyer.DoNotPresentRequest,
                DoNotPresentResultField = buyer.DoNotPresentResultField,
                DoNotPresentResultValue = buyer.DoNotPresentResultValue,
                DoNotPresentStatus = buyer.DoNotPresentStatus,
                DoNotPresentUrl = buyer.DoNotPresentUrl,
                ExternalId = buyer.ExternalId,
                LastPosted = buyer.LastPosted,
                LastPostedSold = buyer.LastPostedSold
            };

            return model;
        }

        #endregion

        #region BuyerChannel

        public static BuyerChannelViewModel GetViewModel(this BuyerChannel buyerChannel)
        {
            var model = new BuyerChannelViewModel
            {
                Id = buyerChannel.Id,
                Name = buyerChannel.Name,
                AffiliatePrice = buyerChannel.AffiliatePrice,
                Deleted = buyerChannel.Deleted,
                XmlTemplate = buyerChannel.XmlTemplate,
                Status = buyerChannel.Status,
                Note = buyerChannel.Note,
                DataFormat = buyerChannel.DataFormat,
                CampaignId = buyerChannel.CampaignId,
                Timeout = buyerChannel.Timeout,
                AcceptedField = buyerChannel.AcceptedField,
                AcceptedFrom = buyerChannel.AcceptedFrom,
                AcceptedValue = buyerChannel.AcceptedValue,
                AccountIdField = buyerChannel.AccountIdField,
                AffiliatePriceOption = buyerChannel.AffiliatePriceOption,
                AfterTimeout = buyerChannel.AfterTimeout,
                AllowedAffiliateChannels = buyerChannel.AllowedAffiliateChannels,
                TypeId = buyerChannel.AlwaysSoldOption,
                BuyerId = buyerChannel.BuyerId,
                BuyerPrice = buyerChannel.BuyerPrice,
                BuyerPriceOption = (short)buyerChannel.BuyerPriceOption,
                CapReachedNotification = buyerChannel.CapReachedNotification,
                ChannelMappingUniqueId = buyerChannel.ChannelMappingUniqueId,
                ChildChannels = buyerChannel.ChildChannels,
                DailyCap = buyerChannel.DailyCap,
                Delimeter = buyerChannel.Delimeter,
                DeliveryMethod = buyerChannel.DeliveryMethod,
                EnableAgeTargeting = buyerChannel.EnableAgeTargeting,
                EnableCustomPriceReject = buyerChannel.EnableCustomPriceReject,
                EnableStateTargeting = buyerChannel.EnableStateTargeting,
                EnableZipCodeTargeting = buyerChannel.EnableZipCodeTargeting,
                ErrorField = buyerChannel.ErrorField,
                ErrorFrom = buyerChannel.ErrorFrom,
                ErrorValue = buyerChannel.ErrorValue,
                FieldAppendEnabled = buyerChannel.FieldAppendEnabled,
                Holidays = buyerChannel.Holidays,
                IsFixed = buyerChannel.IsFixed,
                LeadAcceptRate = buyerChannel.LeadAcceptRate,
                LeadIdField = buyerChannel.LeadIdField,
                MaxAgeTargeting = buyerChannel.MaxAgeTargeting,
                MaxDuplicateDays = buyerChannel.MaxDuplicateDays,
                MessageField = buyerChannel.MessageField,
                MinAgeTargeting = buyerChannel.MinAgeTargeting,
                NotificationEmail = buyerChannel.NotificationEmail,
                OrderNum = buyerChannel.OrderNum,
                PostingHeaders = buyerChannel.PostingHeaders,
                PostingUrl = buyerChannel.PostingUrl,
                PriceField = buyerChannel.PriceField,
                PriceRejectField = buyerChannel.PriceRejectField,
                PriceRejectValue = buyerChannel.PriceRejectValue,
                PriceRejectWinResponse = buyerChannel.PriceRejectWinResponse,
                RedirectField = buyerChannel.RedirectField,
                RedirectUrl = buyerChannel.RedirectUrl,
                RejectedField = buyerChannel.RejectedField,
                RejectedFrom = buyerChannel.RejectedFrom,
                RejectedValue = buyerChannel.RejectedValue,
                ResponseFormat = buyerChannel.ResponseFormat,
                StateCondition = buyerChannel.StateCondition,
                StateTargeting = buyerChannel.StateTargeting,
                StatusAutoChange = buyerChannel.StatusAutoChange,
                StatusChangeMinutes = buyerChannel.StatusChangeMinutes,
                StatusExpireDate = buyerChannel.StatusExpireDate,
                StatusStr = buyerChannel.StatusStr,
                SubIdWhiteListEnabled = buyerChannel.SubIdWhiteListEnabled,
                TestField = buyerChannel.TestField,
                TestFrom = buyerChannel.TestFrom,
                TestValue = buyerChannel.TestValue,
                TimeZone = buyerChannel.TimeZone,
                TimeZoneStr = buyerChannel.TimeZoneStr,
                TimeoutNotification = buyerChannel.TimeoutNotification,
                WinResponsePostMethod = buyerChannel.WinResponsePostMethod,
                WinResponseUrl = buyerChannel.WinResponseUrl,
                ZipCodeCondition = buyerChannel.ZipCodeCondition,
                ZipCodeTargeting = buyerChannel.ZipCodeTargeting
            };

            return model;
        }

       

        public static BuyerChannel GetBuyerChannel(this BuyerChannelUpdateModel model)
        {
            var buyerChannel = new BuyerChannel
            {
                Id = model.Id,
                Name = model.Name,
                AcceptedField = model.AcceptedField,
                AcceptedFrom = model.AcceptedFrom,
                AcceptedValue = model.AcceptedValue,
                AccountIdField = model.AccountIdField,
                AffiliatePrice = model.AffiliatePrice,
                AffiliatePriceOption = model.AffiliatePriceOption,
                AfterTimeout = model.AfterTimeout,
                AllowedAffiliateChannels = model.AllowedAffiliateChannels,
                AlwaysSoldOption = model.TypeId,
                BuyerId = model.BuyerId,
                BuyerPrice = model.BuyerPrice,
                BuyerPriceOption = model.BuyerPriceOption,
                CampaignId = model.CampaignId,
                CapReachedNotification = model.CapReachedNotification,
                ChannelMappingUniqueId = model.ChannelMappingUniqueId,
                ChildChannels = model.ChildChannels,
                DailyCap = model.DailyCap,
                DataFormat = model.DataFormat,
                Deleted = model.Deleted,
                Delimeter = model.Delimeter,
                DeliveryMethod = model.DeliveryMethod,
                EnableAgeTargeting = model.EnableAgeTargeting,
                EnableCustomPriceReject = model.EnableCustomPriceReject,
                EnableStateTargeting = model.EnableStateTargeting,
                EnableZipCodeTargeting = model.EnableZipCodeTargeting,
                ErrorField = model.ErrorField,
                ErrorFrom = model.ErrorFrom,
                ErrorValue = model.ErrorValue,
                FieldAppendEnabled = model.FieldAppendEnabled,
                Holidays = model.Holidays,
                IsFixed = model.IsFixed,
                LeadAcceptRate = model.LeadAcceptRate,
                LeadIdField = model.LeadIdField,
                MaxAgeTargeting = model.MaxAgeTargeting,
                MaxDuplicateDays = model.MaxDuplicateDays,
                MessageField = model.MessageField,
                MinAgeTargeting = model.MinAgeTargeting,
                Note = model.Note,
                NotificationEmail = model.NotificationEmail,
                OrderNum = model.OrderNum,
                PostingHeaders = model.PostingHeaders,
                PostingUrl = model.PostingUrl,
                PriceField = model.PriceField,
                PriceRejectField = model.PriceRejectField,
                PriceRejectValue = model.PriceRejectValue,
                PriceRejectWinResponse = model.PriceRejectWinResponse,
                RedirectField = model.RedirectField,
                RedirectUrl = model.RedirectUrl,
                RejectedField = model.RejectedField,
                RejectedFrom = model.RejectedFrom,
                RejectedValue = model.RejectedValue,
                ResponseFormat = model.ResponseFormat,
                StateCondition = model.StateCondition,
                StateTargeting = model.StateTargeting,
                Status = model.Status,
                StatusAutoChange = model.StatusAutoChange,
                StatusChangeMinutes = model.StatusChangeMinutes,
                StatusExpireDate = model.StatusExpireDate,
                StatusStr = model.StatusStr,
                SubIdWhiteListEnabled = model.SubIdWhiteListEnabled,
                TestField = model.TestField,
                TestFrom = model.TestFrom,
                TestValue = model.TestValue,
                TimeZone = model.TimeZone,
                TimeZoneStr = model.TimeZoneStr,
                Timeout = model.AfterTimeout,
                TimeoutNotification = model.TimeoutNotification,
                WinResponsePostMethod = model.WinResponsePostMethod,
                WinResponseUrl = model.WinResponseUrl,
                XmlTemplate = model.XmlTemplate,
                ZipCodeCondition = model.ZipCodeCondition,
                ZipCodeTargeting = model.ZipCodeTargeting
            };

            return buyerChannel;
        }

        
        #endregion

        #region Lead

        public static LeadMainContentItemViewModel GetItemViewModel(this LeadMainContent leadMainContent)
        {
            var model = new LeadMainContentItemViewModel
            {
                Id = leadMainContent.Id,
                AffiliatePrice = leadMainContent.AffiliatePrice,
                Status = leadMainContent.Status,
                CampaignId = leadMainContent.CampaignId,
                AffiliateId = leadMainContent.AffiliateId,
                BuyerPrice = leadMainContent.BuyerPrice,
                BuyerId = leadMainContent.BuyerId,
                BuyerChannelId = leadMainContent.BuyerChannelId,
                Created = leadMainContent.Created,
                AffiliateChannelId = leadMainContent.AffiliateChannelId,
                AccountType = leadMainContent.AccountType,
                Address = leadMainContent.Address,
                AddressMonth = leadMainContent.AddressMonth,
                AffiliateSubId = leadMainContent.AffiliateSubId,
                Age = leadMainContent.Age,
                BankPhone = leadMainContent.BankPhone,
                CampaignType = leadMainContent.CampaignType,
                CellPhone = leadMainContent.CellPhone,
                City = leadMainContent.City,
                ClickIp = leadMainContent.ClickIp,
                Clicked = leadMainContent.Clicked,
                DirectDeposit = leadMainContent.Directdeposit,
                Dob = leadMainContent.Dob,
                DuplicateLeadId = leadMainContent.DublicateLeadId,
                Email = leadMainContent.Email,
                EmpTime = leadMainContent.Emptime,
                ErrorType = leadMainContent.ErrorType,
                Firstname = leadMainContent.Firstname,
                HomePhone = leadMainContent.HomePhone,
                IncomeType = leadMainContent.IncomeType,
                Ip = leadMainContent.Ip,
                Lastname = leadMainContent.Lastname,
                LeadId = leadMainContent.LeadId,
                LeadNumber = leadMainContent.LeadNumber,
                MinPrice = leadMainContent.Minprice,
                MinPriceStr = leadMainContent.MinpriceStr,
                NetMonthlyIncome = leadMainContent.NetMonthlyIncome,
                PayFrequency = leadMainContent.PayFrequency,
                ProcessingTime = leadMainContent.ProcessingTime,
                ReceivedData = leadMainContent.ReceivedData,
                RequestedAmount = leadMainContent.RequestedAmount,
                RiskScore = leadMainContent.RiskScore,
                Ssn = leadMainContent.Ssn,
                State = leadMainContent.State,
                String1 = leadMainContent.String1,
                String2 = leadMainContent.String2,
                String3 = leadMainContent.String3,
                String4 = leadMainContent.String4,
                String5 = leadMainContent.String5,
                UpdateDate = leadMainContent.UpdateDate,
                Warning = leadMainContent.Warning,
                Zip = leadMainContent.Zip
            };

            return model;
        }

        public static LeadMainContentDetailsViewModel GetDetailsViewModel(this LeadMainContent leadMainContent)
        {
            var model = new LeadMainContentDetailsViewModel
            {
                Id = leadMainContent.Id,
                AffiliatePrice = leadMainContent.AffiliatePrice,
                Status = leadMainContent.Status,
                CampaignId = leadMainContent.CampaignId,
                BuyerPrice = leadMainContent.BuyerPrice,
                Created = leadMainContent.Created,
                AccountType = leadMainContent.AccountType,
                Address = leadMainContent.Address,
                AddressMonth = leadMainContent.AddressMonth,
                AffiliateSubId = leadMainContent.AffiliateSubId,
                Age = leadMainContent.Age,
                BankPhone = leadMainContent.BankPhone,
                CellPhone = leadMainContent.CellPhone,
                City = leadMainContent.City,
                ClickIp = leadMainContent.ClickIp,
                Clicked = leadMainContent.Clicked,
                DirectDeposit = leadMainContent.Directdeposit,
                Dob = leadMainContent.Dob,
                DuplicateLeadId = leadMainContent.DublicateLeadId,
                Email = leadMainContent.Email,
                EmpTime = leadMainContent.Emptime,
                ErrorType = leadMainContent.ErrorType,
                Firstname = leadMainContent.Firstname,
                HomePhone = leadMainContent.HomePhone,
                IncomeType = leadMainContent.IncomeType,
                Ip = leadMainContent.Ip,
                Lastname = leadMainContent.Lastname,
                LeadId = leadMainContent.LeadId,
                LeadNumber = leadMainContent.LeadNumber,
                MinPrice = leadMainContent.Minprice,
                MinPriceStr = leadMainContent.MinpriceStr,
                NetMonthlyIncome = leadMainContent.NetMonthlyIncome,
                PayFrequency = leadMainContent.PayFrequency,
                ProcessingTime = leadMainContent.ProcessingTime,
                ReceivedData = leadMainContent.ReceivedData,
                RequestedAmount = leadMainContent.RequestedAmount,
                RiskScore = leadMainContent.RiskScore,
                Ssn = leadMainContent.Ssn,
                State = leadMainContent.State,
                String1 = leadMainContent.String1,
                String2 = leadMainContent.String2,
                String3 = leadMainContent.String3,
                String4 = leadMainContent.String4,
                String5 = leadMainContent.String5,
                UpdateDate = leadMainContent.UpdateDate,
                Warning = leadMainContent.Warning,
                Zip = leadMainContent.Zip,
                
            };
            return model;
        }

        #endregion

        #region LeadResponse

        public static LeadResponseViewModel GetViewModel(this LeadResponse leadResponse)
        {
            var model = new LeadResponseViewModel
            {
                Id = leadResponse.Id,
                Status = leadResponse.Status,
                AffiliateId = leadResponse.AffiliateId,
                Created = leadResponse.Created,
                BuyerId = leadResponse.BuyerId,
                AffiliateChannelId = leadResponse.AffiliateChannelId,
                BuyerChannelId = leadResponse.BuyerChannelId,
                BuyerName = leadResponse.BuyerName,
                MinPrice = leadResponse.MinPrice,
                Response = leadResponse.Response,
                BuyerChanelName = leadResponse.BuyerChanelName,
                LeadId = leadResponse.LeadId,
                Posted = leadResponse.Posted,
                ResponseCreated = leadResponse.ResponseCreated,
                ResponseTime = leadResponse.ResponseTime
            };

            return model;
        }


        #endregion

        #region LeadContentDuplicate

        public static LeadContentDuplicateViewModel GetViewModel(this LeadContentDublicate leadContentDuplicate)
        {
            var model = new LeadContentDuplicateViewModel
            {
                Id = leadContentDuplicate.Id,
                AffiliateId = leadContentDuplicate.AffiliateId,
                Created = leadContentDuplicate.Created,
                LeadId = leadContentDuplicate.LeadId,
                City = leadContentDuplicate.City,
                Email = leadContentDuplicate.Email,
                MinPrice = leadContentDuplicate.Minprice,
                AffiliateName = leadContentDuplicate.AffiliateName,
                AccountType = leadContentDuplicate.AccountType,
                Firstname = leadContentDuplicate.Firstname,
                State = leadContentDuplicate.State,
                HomePhone = leadContentDuplicate.HomePhone,
                BankPhone = leadContentDuplicate.BankPhone,
                CampaignType = leadContentDuplicate.CampaignType,
                Address = leadContentDuplicate.Address,
                CellPhone = leadContentDuplicate.CellPhone,
                AddressMonth = leadContentDuplicate.AddressMonth,
                Age = leadContentDuplicate.Age,
                DirectDeposit = leadContentDuplicate.Directdeposit,
                Dob = leadContentDuplicate.Dob,
                EmpTime = leadContentDuplicate.Emptime,
                IncomeType = leadContentDuplicate.IncomeType,
                Ip = leadContentDuplicate.Ip,
                Lastname = leadContentDuplicate.Lastname,
                NetMonthlyIncome = leadContentDuplicate.NetMonthlyIncome,
                OriginalLeadId = leadContentDuplicate.OriginalLeadId,
                PayFrequency = leadContentDuplicate.PayFrequency,
                RequestedAmount = leadContentDuplicate.RequestedAmount,
                Ssn = leadContentDuplicate.Ssn,
                Zip = leadContentDuplicate.Zip,
                String1 = leadContentDuplicate.String1,
                String2 = leadContentDuplicate.String2,
                String3 = leadContentDuplicate.String3,
                String4 = leadContentDuplicate.String4,
                String5 = leadContentDuplicate.String5
            };

            return model;
        }

        #endregion

        #region SupportTickets

        public static SupportTickets GetSupportTickets(this SupportTicketsCreateModel model)
        {
            var supportTickets = new SupportTickets
            {
                Id = (long)model.Id,
                DateTime = model.DateTime,
                ManagerID = model.ManagerId,
                Message = model.Message,
                Priority = model.Priority,
                Status = model.Status,
                Subject = model.Subject,
                UserID = model.UserId,
                DueDate = model.DueDate,
                TicketType = model.TicketType
            };

            return supportTickets;
        }

        public static SupportTicketsViewModel GetViewModel(this SupportTickets supportTickets)
        {
            var model = new SupportTicketsViewModel
            {
                Id = supportTickets.Id,
                Status = supportTickets.Status,
                Message = supportTickets.Message,
                DateTime = supportTickets.DateTime,
                Priority = supportTickets.Priority,
                Subject = supportTickets.Subject,
                UserId = supportTickets.UserID,
                ManagerId = supportTickets.ManagerID
            };

            return model;
        }

        #endregion

        #region SupportTicketMessage

        public static SupportTicketsMessages GetSupportTicketsMessage(this SupportTicketMessageCreateModel model)
        {
            var supportTicketMessage = new SupportTicketsMessages
            {
                AuthorID = model.AuthorId,
                DateTime = model.DateTime,
                FilePath = model.FilePath,
                IsNew = model.IsNew,
                Message = model.Message,
                TicketID = model.TicketId
            };
            return supportTicketMessage;
        }

        public static SupportTicketMessageViewModel GetViewModel(this SupportTicketsMessages supportTicketMessage)
        {
            var model = new SupportTicketMessageViewModel
            {
                Id = supportTicketMessage.Id,
                Message = supportTicketMessage.Message,
                DateTime = supportTicketMessage.DateTime,
                IsNew = supportTicketMessage.IsNew,
                TicketId = supportTicketMessage.TicketID,
                FilePath = supportTicketMessage.FilePath,
                AuthorId = supportTicketMessage.AuthorID
            };

            return model;
        }

        public static SupportTicketsMessagesViewViewModel GetViewModel(this SupportTicketsMessagesView supportTicketMessageVew)
        {
            var model = new SupportTicketsMessagesViewViewModel
            {
                Id = supportTicketMessageVew.Id,
                Message = supportTicketMessageVew.Message,
                DateTime = supportTicketMessageVew.DateTime,
                IsNew = supportTicketMessageVew.IsNew,
                TicketId = supportTicketMessageVew.TicketID,
                FilePath = supportTicketMessageVew.FilePath,
                AuthorId = supportTicketMessageVew.AuthorID,
                FirstName = supportTicketMessageVew.FirstName,
                LastName = supportTicketMessageVew.LastName,
                UserName = supportTicketMessageVew.UserName,
                Avatar = supportTicketMessageVew.Avatar
            };

            return model;
        }

        public static SupportTicketsViewViewModel GetViewModel(this SupportTicketsView supportTicketsView)
        {
            var supportTicketsViewViewModel = new SupportTicketsViewViewModel
            {
                Id = supportTicketsView.Id,
                Status = supportTicketsView.Status,
                Message = supportTicketsView.Message,
                DateTime = supportTicketsView.DateTime,
                Subject = supportTicketsView.Subject,
                Priority = supportTicketsView.Priority,
                UserId = supportTicketsView.UserID,
                ManagerId = supportTicketsView.ManagerID,
                ManagerName = supportTicketsView.Managername,
                NewCount = supportTicketsView.NewCount,
                Username = supportTicketsView.Username
            };

            return supportTicketsViewViewModel;
        }

        #endregion

        #region RegistrationRequest

        public static RegistrationRequest GetRegistrationRequest(this RegistrationRequestCreateModel model)
        {
            var registrationRequest = new RegistrationRequest
            {
                Code = Guid.NewGuid().ToString(),
                Email = model.Email,
                Created = DateTime.UtcNow,
                Name = model.Name
            };

            return registrationRequest;
        }

        public static RegistrationRequest GetRegistrationRequest(this RegisterValidationModel model)
        {
            var registrationRequest = new RegistrationRequest
            {
                Code = Guid.NewGuid().ToString(),
                Email = model.Email,
                Created = DateTime.UtcNow,
                Name = model.Name
            };

            return registrationRequest;
        }

        #endregion


        #region Register

        public static Buyer GetBuyer(this RegisterModel model)
        {
            var buyer = new Buyer()
            {
                Name = model.Name,
                CountryId = model.CountryId,
                StateProvinceId = model.StateProvinceId,
                Email = model.CompanyEmail,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                City = model.City,
                ZipPostalCode = model.ZipPostalCode,
                Phone = model.CompanyPhone,
                CreatedOn = DateTime.UtcNow,
                Status = 1,
                ManagerId = model.ManagerId,
                BillFrequency = "m",
                FrequencyValue = 1,
                AlwaysSoldOption = model.AlwaysSoldOption,
                MaxDuplicateDays = model.MaxDuplicateDays,
                DailyCap = model.DailyCap,
                Description = model.Description,
                DoNotPresentResultField = model.DoNotPresentResultField,
                DoNotPresentResultValue = model.DoNotPresentResultValue,
                DoNotPresentStatus = model.DoNotPresentStatus,
                DoNotPresentPostMethod = model.DoNotPresentPostMethod,
                DoNotPresentRequest = model.DoNotPresentRequest,
                DoNotPresentUrl = model.DoNotPresentUrl,
                CanSendLeadId = model.CanSendLeadId,
                AccountId = model.AccountId
            };

            return buyer;
        }

        public static BuyerBalance GetBuyerBalance(this RegisterModel model, long buyerId)
        {
            var buyerBalance = new BuyerBalance
            {
                Credit = model.Credit,
                Balance = model.Credit,
                BuyerId = buyerId,
                PaymentSum = 0M,
                SoldSum = 0M
            };

            return buyerBalance;
        }

        public static Affiliate GetAffiliate(this RegisterModel registerModel, long userId, string registrationIpAddress)
        {
            var affiliate = new Affiliate
            {
                Name = registerModel.Name,
                CountryId = registerModel.CountryId,
                StateProvinceId = registerModel.StateProvinceId,
                Email = registerModel.CompanyEmail,
                AddressLine1 = registerModel.AddressLine1,
                AddressLine2 = registerModel.AddressLine2,
                City = registerModel.City,
                ZipPostalCode = registerModel.ZipPostalCode,
                Phone = registerModel.CompanyPhone,
                UserId = userId,
                CreatedOn = DateTime.UtcNow,
                Status = (short)AffiliateActivityStatuses.Applied,
                RegistrationIp = registrationIpAddress,
                ManagerId = registerModel.ManagerId,
                WhiteIp = registerModel.WhiteIp
            };

            return affiliate;
        }

        public static Profile GetProfile(this RegisterModel registerModel, long userId)
        {
            var profile = new Profile
            {
                UserId = userId,
                FirstName = registerModel.FirstName,
                MiddleName = registerModel.MiddleName,
                LastName = registerModel.LastName,
                Summary = string.Empty,
                Phone = registerModel.Phone,
                CellPhone = registerModel.CellPhone
            };

            return profile;
        }


        public static EmailSubscription GetEmailSubscription(this string email)
        {
            var emailSubscription = new EmailSubscription
            {
                GuId = Guid.NewGuid().ToString().ToUpper(),
                Email = email,
                Active = true,
                CreatedOn = DateTime.UtcNow
            };
            return emailSubscription;
        }

        #endregion

    }
}