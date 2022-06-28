using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Buyers
{
    public class BuyerContactInformationModel
    {
        [RegularExpression(@"^[^@\s]+@[^@\s\.]+\.[^@\.\s]+$")]
        public string Email { get; set; }
        [RegularExpression(@"^([+]?[\s0-9]+)?(\d{3}|[(]?[0-9]+[)])?([-]?[\s]?[0-9])+$")]
        [MinLength(9)]
        [MaxLength(15)]
        public string Phone { get; set; }
        public long? ManagerId { get; set; }
        public long CountryId { get; set; }
        public long? StateProvinceId { get; set; }
        public string City { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZipPostalCode { get; set; }

    }
}