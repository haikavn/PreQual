// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="SettingModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Web.Framework.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Models.Content
{
    /// <summary>
    /// Represents a Activation Model
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public partial class SettingModel : BaseAppModel
    {
        public SettingModel()
        {
            this.ListCountry = new List<SelectListItem>();
        }

        #region Properties

        /// <summary>
        /// Company Settings
        /// </summary>
        /// <value>The page title.</value>

        public string PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>The name of the company.</value>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the company address.
        /// </summary>
        /// <value>The company address.</value>
        public string CompanyAddress { get; set; }

        /// <summary>
        /// Gets or sets the company address.
        /// </summary>
        /// <value>The company address 2.</value>
        public string CompanyAddress2 { get; set; }

        /// <summary>
        /// Gets or sets the company address.
        /// </summary>
        /// <value>City.</value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the company address.
        /// </summary>
        /// <value>State.</value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the company address.
        /// </summary>
        /// <value>Zip Code.</value>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the company address.
        /// </summary>
        /// <value>Country.</value>
        public string Country { get; set; }

        public long CountryId { get; set; }


        /// <summary>
        /// Gets or sets the company bank.
        /// </summary>
        /// <value>The company bank.</value>
        public string CompanyBank { get; set; }

        /// <summary>
        /// Gets or sets the company bank.
        /// </summary>
        /// <value>Account Type. (Checking|Saving)</value>
        public string AccountType { get; set; }
        /// <summary>
        /// Gets or sets the company bank.
        /// </summary>
        /// <value>Routing Number.</value>
        public string RoutingNumber { get; set; }
        /// <summary>
        /// Gets or sets the company bank.
        /// </summary>
        /// <value>Account Number.</value>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets the company email.
        /// </summary>
        /// <value>The company email.</value>
        public string CompanyEmail { get; set; }

        /// <summary>
        /// Gets or sets the company logo path.
        /// </summary>
        /// <value>The company logo path.</value>
        public string CompanyLogoPath { get; set; }

        /// <summary>
        /// Gets or sets the type of the user menu.
        /// </summary>
        /// <value>The type of the user menu.</value>
        public short UserMenuType { get; set; }

        /// <summary>
        /// SMTP (Mail Server Settings)
        /// </summary>
        /// <value>The SMTP email.</value>

        public string SmtpEmail { get; set; }

        /// <summary>
        /// Gets or sets the display name of the SMTP.
        /// </summary>
        /// <value>The display name of the SMTP.</value>
        public string SmtpDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the SMTP host.
        /// </summary>
        /// <value>The SMTP host.</value>
        public string SmtpHost { get; set; }

        /// <summary>
        /// Gets or sets the SMTP port.
        /// </summary>
        /// <value>The SMTP port.</value>
        public int SmtpPort { get; set; }

        /// <summary>
        /// Gets or sets the SMTP username.
        /// </summary>
        /// <value>The SMTP username.</value>
        public string SmtpUsername { get; set; }

        /// <summary>
        /// Gets or sets the SMTP password.
        /// </summary>
        /// <value>The SMTP password.</value>
        public string SmtpPassword { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [SMTP enable SSL].
        /// </summary>
        /// <value><c>true</c> if [SMTP enable SSL]; otherwise, <c>false</c>.</value>
        public bool SmtpEnableSsl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [SMTP use default credentials].
        /// </summary>
        /// <value><c>true</c> if [SMTP use default credentials]; otherwise, <c>false</c>.</value>
        public bool SmtpUseDefaultCredentials { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is saved.
        /// </summary>
        /// <value><c>true</c> if this instance is saved; otherwise, <c>false</c>.</value>
        public bool IsSaved { get; set; }

        /// <summary>
        /// Gets or sets the time zones.
        /// </summary>
        /// <value>The time zones.</value>
        public List<SelectListItem> TimeZones { get; set; }

        /// <summary>
        /// Gets or sets the selected time zone.
        /// </summary>
        /// <value>The selected time zone.</value>
        public string SelectedTimeZone { get; set; }

        /// <summary>
        /// Gets or sets the affiliate XML field.
        /// </summary>
        /// <value>The affiliate XML field.</value>
        public string AffiliateXmlField { get; set; }

        /// <summary>
        /// Gets or sets the posting URL.
        /// </summary>
        /// <value>The posting URL.</value>
        public string PostingUrl { get; set; }

        /// <summary>
        /// Gets or sets the login expire.
        /// </summary>
        /// <value>The login expire.</value>
        public int LoginExpire { get; set; }

        /// <summary>
        /// Gets or sets the maximum processing leads.
        /// </summary>
        /// <value>The maximum processing leads.</value>
        public int MaxProcessingLeads { get; set; }

        /// <summary>
        /// Gets or sets the processing delay.
        /// </summary>
        /// <value>The processing delay.</value>
        public int ProcessingDelay { get; set; }

        

        /// <summary>
        /// Gets or sets the call client.
        /// </summary>
        /// <value>The call client.</value>
        public string CallClient { get; set; }

        /// <summary>
        /// Gets or sets the name of the call first name to client.
        /// </summary>
        /// <value>The name of the call first name to client.</value>
        public short CallFirstNameToClientName { get; set; }

        /// <summary>
        /// Gets or sets the dublicate monitor.
        /// </summary>
        /// <value>The dublicate monitor.</value>
        public short DublicateMonitor { get; set; }

        /// <summary>
        /// Gets or sets the allow affiliate redirect.
        /// </summary>
        /// <value>The allow affiliate redirect.</value>
        public short AllowAffiliateRedirect { get; set; }

        /// <summary>
        /// Gets or sets the affiliate redirect URL.
        /// </summary>
        /// <value>The affiliate redirect URL.</value>
        public string AffiliateRedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the white ip.
        /// </summary>
        /// <value>The white ip.</value>
        public string WhiteIp { get; set; }

        /// <summary>
        /// Gets or sets the lead email.
        /// </summary>
        /// <value>The lead email.</value>
        public short LeadEmail { get; set; }

        /// <summary>
        /// Gets or sets the lead email to.
        /// </summary>
        /// <value>The lead email to.</value>
        public string LeadEmailTo { get; set; }

        /// <summary>
        /// Gets or sets the lead email fields.
        /// </summary>
        /// <value>The lead email fields.</value>
        public string LeadEmailFields { get; set; }

        /// <summary>
        /// Gets or sets the minimum processing mode.
        /// </summary>
        /// <value>The minimum processing mode.</value>
        public short MinProcessingMode { get; set; }

        /// <summary>
        /// Gets or sets the system on hold.
        /// </summary>
        /// <value>The system on hold.</value>
        public short SystemOnHold { get; set; }

        /// <summary>
        /// Gets or sets the auto cache mode.
        /// </summary>
        /// <value>The auto cache mode.</value>
        public short AutoCacheMode { get; set; }

        /// <summary>
        /// Gets or sets the auto cache urls.
        /// </summary>
        /// <value>The auto cache urls.</value>
        public string AutoCacheUrls { get; set; }


        /// <summary>
        /// Gets or sets the debug mode.
        /// </summary>
        /// <value>The debug mode.</value>
        public short DebugMode { get; set; }

        public string AppUrl { get; set; }

        public IList<SelectListItem> ListCountry { get; set; }

        #endregion Properties
    }
}