// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ValidatorTypeController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Class ValidatorTypeController.
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public partial class ValidatorTypeController : BaseContentManagementController
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorTypeController"/> class.
        /// </summary>
        public ValidatorTypeController()
        {
        }

        #endregion Constructor

        // GET: Affiliate
        /// <summary>
        /// Gets the type of the validator.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult GetValidatorType(string type)
        {
            short t = 0;

            short.TryParse(type, out t);

            switch (t)
            {
                case 1: return PartialView("MinMax", "1;150");
                case 10: return PartialView("DateTimeFormats", "");
                case 14: return PartialView("DateTimeFormats", "");
                case 15: return PartialView("Regexp", "");
                case 16: return PartialView("Decimal", "10;2");
                case 7: return PartialView("ZipCode", "");
            }

            string format = "";
            string validators = "";

            switch (type.ToLower())
            {
                case "none": break;
                case "ip": format = "Ip address"; validators = "1"; break;
                case "minprice": format = "Min price"; validators = "2,16"; break;
                case "firstname": format = "First name"; validators = "1"; break;
                case "lastname": format = "Last name"; validators = "1"; break;
                case "address": format = "Address"; validators = "1"; break;
                case "city": format = "City"; validators = "1"; break;
                case "state": format = "State"; validators = "1,11"; break;
                case "zip": format = "Zip"; validators = "7,1"; break;
                case "dob": format = "Date of Birth"; validators = "10,14"; break;
                case "age": format = "Age"; validators = "2"; break;
                case "requestedamount": format = "Requested amount"; validators = "2,16"; break;
                case "ssn": format = "SSN Number"; validators = "1,6"; break;
                case "homephone": format = "US Phone Number"; validators = "1,8"; break;
                case "cellphone": format = "US Phone Number"; validators = "1,8"; break;
                case "email": format = "E-mail address"; validators = "3,1"; break;
                case "payfrequency": format = "Pay frequency"; validators = "2"; break;
                case "directdeposit": format = "Direct deposit"; validators = "2"; break;
                case "accounttype": format = "Account type"; validators = "1"; break;
                case "incometype": format = "Income type"; validators = "1"; break;
                case "netmonthlyincome": format = "Net monthly income"; validators = "2,16"; break;
                case "emptime": format = "Emp. time"; validators = "2"; break;
                case "addressmonth": format = "Address month"; validators = "2"; break;
                case "affiliatesubid": format = "Affiliate sub id"; validators = "1,17"; break;
                case "affiliatesubid2": format = "Affiliate sub id2"; validators = "1"; break;
                default: return Content("");
            }

            return Content("{\"field\": \"" + type + "\", \"format\":\"" + format + "\", \"validators\":\"" + validators + "\"}");
        }

        /// <summary>
        /// Gets the validator type HTML.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="model">The model.</param>
        /// <returns>System.String.</returns>
        public string GetValidatorTypeHtml(short type, object model)
        {
            string content = "";

            switch (type)
            {
                case 1: content = this.RenderPartialViewToString("MinMax", model); break;
                case 10: content = this.RenderPartialViewToString("DateTimeFormats", model); break;
                case 14: content = this.RenderPartialViewToString("DateTimeFormats", model); break;
                case 15: content = this.RenderPartialViewToString("Regexp", model); break;
                case 16: content = this.RenderPartialViewToString("Decimal", model); break;
                case 7: content = this.RenderPartialViewToString("ZipCode", model); break;
            }

            return content;
        }
    }
}