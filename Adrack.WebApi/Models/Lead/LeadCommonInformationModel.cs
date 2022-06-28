using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadCommonInformationModel
	{
        //------REFERRAL------------------------
        public string ChannelId { get; set; }
        public string Password { get; set; }
        public string AffiliateSubId { get; set; }
        public string AffiliateSubId2 { get; set; }
        public string ReferringUrl { get; set; }
        public string MinPrice { get; set; }
        //------REFERRAL------------------------

        //------PERSONAL------------------------
        public string IpAddress { get; set; }
        public string RequestedAmount { get; set; }
        public string Ssn { get; set; }
        public string Dob { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string HomePhone { get; set; }
        public string CellPhone { get; set; }
        public string DlState { get; set; }
        public string DlNumber { get; set; }
        public string ArmedForces { get; set; }
        public string ContactTime { get; set; }
        public string RentOrOwn { get; set; }
        public string Email { get; set; }
        public string AddressMonth { get; set; }
        public string Citizenship { get; set; }
        //------PERSONAL------------------------

        //------EMPLOYMENT----------------------
        public string IncomeType { get; set; }
        public short EmpTime { get; set; }
        public string EmpName { get; set; }
        public string EmpPhone { get; set; }
        public string JobTitle { get; set; }
        public string PayFrequency { get; set; }
        public string NextPayDate { get; set; }
        public string SecondPayDate { get; set; }
        //------EMPLOYMENT-------------------------

        //------BANK-------------------------------
        public string BankName { get; set; }
        public string BankPhone { get; set; }
        public string AccountType { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
        public string BankMonths { get; set; }
        public string NetMonthlyIncome { get; set; }
        public string DirectDeposit { get; set; }
        //------BANK-------------------------------

    }
}