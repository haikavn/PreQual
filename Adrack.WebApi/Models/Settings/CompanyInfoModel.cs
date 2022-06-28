using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Settings
{
    public class CompanyInfoModel
    {
        public string Name { get; set; }
        [RegularExpression(@"^[^@\s]+@[^@\s\.]+\.[^@\.\s]+$")]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [RegularExpression(@"^[^@\s]+@[^@\s\.]+\.[^@\.\s]+$")]
        public string AccountManager { get; set; }
        [MaxLengthAttribute(100)]
        public string Address { get; set; }
        [MaxLengthAttribute(100)]
        public string SecondaryAddress { get; set; }
        public string LogoPath { get; set; }
    }
}