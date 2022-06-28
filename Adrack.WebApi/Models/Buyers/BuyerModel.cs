using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.Lead;

namespace Adrack.WebApi.Models.Buyers
{
    public class BuyerModel
    {
        public long Id { get; set; }
        
        [MaxLength(50)]
        public string Name { get; set; }
        [RegularExpression(@"^[^@\s]+@[^@\s\.]+\.[^@\.\s]+$")]
        
        [MaxLength(50)]
        public string Email { get; set; }
        [RegularExpression(@"^([+]?[\s0-9]+)?(\d{3}|[(]?[0-9]+[)])?([-]?[\s]?[0-9])+$")]
        [MinLength(9)]
        [MaxLength(15)]
        public string Phone { get; set; }
        public long? ManagerId { get; set; }
        public long CountryId { get; set; }
        public long? StateProvinceId { get; set; }
        public string City { get; set; }
        
        [MaxLength(150)]
        public string AddressLine1 { get; set; }
        
        [MaxLength(150)]
        public string AddressLine2 { get; set; }

        [MaxLength(20)]
        public string ZipPostalCode { get; set; }
        public short Status { get; set; }
        public DateTime CreatedOn { get; set; }
        [MaxLength(10)]
        public string BillFrequency { get; set; }
        public int? FrequencyValue { get; set; }
        public decimal? Credit { get; set; }
        public DateTime? LastPostedSold { get; set; }
        public DateTime? LastPosted { get; set; }
        public short TypeId { get; set; }
        public short MaxDuplicateDays { get; set; }
        public int DailyCap { get; set; }
        [MaxLength(2000)]
        public string Description { get; set; }
        public bool? CoolOffEnabled { get; set; }
        public DateTime? CoolOffStart { get; set; }
        public DateTime? CoolOffEnd { get; set; }
        public short? DoNotPresentStatus { get; set; }

        [MaxLength(300)]
        public string DoNotPresentUrl { get; set; }
        [MaxLength(50)]
        public string DoNotPresentResultField { get; set; }
        [MaxLength(50)]
        public string DoNotPresentResultValue { get; set; }
        [MaxLength(2000)]
        public string DoNotPresentRequest { get; set; }
        [MaxLength(50)]
        public string DoNotPresentPostMethod { get; set; }
        public bool? CanSendLeadId { get; set; }
        public int AccountId { get; set; }
        [MaxLength(200)]
        public string IconPath { get; set; }

        public bool SendStatementReport { get; set; }

        public List<BuyerInvitationModel> Invitations { get; set; }

        public bool CanSendEmail { get; set; } = true;

        public bool? AutosendInvoice { get; set; }


        public static explicit operator BuyerModel(Buyer buyer)
        {
            return new BuyerModel
            {
                Id = buyer.Id,
                Name = buyer.Name,
                Email = buyer.Email,
                Phone = buyer.Phone,
                ManagerId = buyer.ManagerId,
                CountryId = buyer.CountryId,
                StateProvinceId = buyer.StateProvinceId,
                City = buyer.City,
                AddressLine1 = buyer.AddressLine1,
                AddressLine2 = buyer.AddressLine2,
                ZipPostalCode = buyer.ZipPostalCode,
                Status = buyer.Status,
                CreatedOn = buyer.CreatedOn,
                BillFrequency = buyer.BillFrequency,
                FrequencyValue = buyer.FrequencyValue,
                LastPostedSold = buyer.LastPostedSold,
                LastPosted = buyer.LastPosted,
                TypeId = buyer.AlwaysSoldOption,
                MaxDuplicateDays = buyer.MaxDuplicateDays,
                DailyCap = buyer.DailyCap,
                Description = buyer.Description,
                CoolOffEnabled = buyer.CoolOffEnabled,
                CoolOffStart = buyer.CoolOffStart,
                CoolOffEnd = buyer.CoolOffEnd,
                DoNotPresentStatus = buyer.DoNotPresentStatus,
                DoNotPresentUrl = buyer.DoNotPresentUrl,
                DoNotPresentResultField = buyer.DoNotPresentResultField,
                DoNotPresentResultValue = buyer.DoNotPresentResultValue,
                DoNotPresentRequest = buyer.DoNotPresentRequest,
                DoNotPresentPostMethod = buyer.DoNotPresentPostMethod,
                CanSendLeadId = buyer.CanSendLeadId,
                AccountId = buyer.AccountId??0,
                IconPath = buyer.IconPath,
                AutosendInvoice = buyer.AutosendInvoice,
                SendStatementReport = buyer.SendStatementReport.HasValue ? buyer.SendStatementReport.Value : false
            };
        }

        public static explicit operator Buyer(BuyerModel buyer)
        {
            return new Buyer
            {
                Id = buyer.Id,
                Name = buyer.Name,
                Email = buyer.Email,
                Phone = buyer.Phone,
                ManagerId = buyer.ManagerId,
                CountryId = buyer.CountryId,
                StateProvinceId = buyer.StateProvinceId,
                City = buyer.City,
                AddressLine1 = buyer.AddressLine1,
                AddressLine2 = buyer.AddressLine2,
                ZipPostalCode = buyer.ZipPostalCode,
                Status = buyer.Status,
                CreatedOn = buyer.CreatedOn,
                BillFrequency = buyer.BillFrequency,
                FrequencyValue = buyer.FrequencyValue,
                LastPostedSold = buyer.LastPostedSold,
                LastPosted = buyer.LastPosted,
                AlwaysSoldOption = buyer.TypeId,
                MaxDuplicateDays = buyer.MaxDuplicateDays,
                DailyCap = buyer.DailyCap,
                Description = buyer.Description,
                CoolOffEnabled = buyer.CoolOffEnabled,
                CoolOffStart = buyer.CoolOffStart,
                CoolOffEnd = buyer.CoolOffEnd,
                DoNotPresentStatus = buyer.DoNotPresentStatus,
                DoNotPresentUrl = buyer.DoNotPresentUrl,
                DoNotPresentResultField = buyer.DoNotPresentResultField,
                DoNotPresentResultValue = buyer.DoNotPresentResultValue,
                DoNotPresentRequest = buyer.DoNotPresentRequest,
                DoNotPresentPostMethod = buyer.DoNotPresentPostMethod,
                CanSendLeadId = buyer.CanSendLeadId,
                AccountId = buyer.AccountId,
                IconPath = buyer.IconPath,
                SendStatementReport = buyer.SendStatementReport
            };
        }
    }
}