using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Adrack.Core.Domain.Membership;
using Adrack.Web.Framework;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Membership.Register
{
    public class RegistrationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleNameName { get; set; }
        [RegularExpression(@"^[^@\s]+@[^@\s\.]+\.[^@\.\s]+$", ErrorMessage = "invalid email address")]
        public string Email { get; set; }
        public string JobTitle { get; set; }
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "invalid password")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "password and confirmation password do not match")]
        public string RepeatPassword { get; set; }
        public string CompanyName { get; set; }
        public string CompanyWebSite { get; set; }
        public long VerticalId { get; set; }
        public long CountryId { get; set; }
        public long? StateProvinceId { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string SecondaryAddress { get; set; }
        public string ZipCode { get; set; }
    }
}