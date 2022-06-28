// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="PaymentSourceCardModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Web.Framework;
using Adrack.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Models.Lead
{
    /// <summary>
    /// Class PaymentSourceCardModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class PaymentSourceCardModel : BaseAppModel
    {
        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public PaymentSourceCardModel()
        {
            Months = new List<SelectListItem>();
            Years = new List<SelectListItem>();

            for (int i = 1; i <= 12; i++)
            {
                Months.Add(new System.Web.Mvc.SelectListItem
                {
                    Text = i.ToString() + " - " + DateTimeFormatInfo.CurrentInfo.GetMonthName(i),
                    Value = i.ToString()
                });
            }

            for (int i = DateTime.Now.Year; i <= DateTime.Now.Year + 12; i++)
            {
                Years.Add(new System.Web.Mvc.SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or sets the card number.
        /// </summary>
        /// <value>The card number.</value>
        [AppLocalizedStringDisplayName("PaymentSource.Card.CardNumber")]
        public string CardNumber { get; set; }

        /// <summary>
        /// Gets or sets the valid month.
        /// </summary>
        /// <value>The valid month.</value>
        [AppLocalizedStringDisplayName("PaymentSource.Card.ValidMonth")]
        public short ValidMonth { get; set; }

        /// <summary>
        /// Gets or sets the valid year.
        /// </summary>
        /// <value>The valid year.</value>
        [AppLocalizedStringDisplayName("PaymentSource.Card.ValidYear")]
        public short ValidYear { get; set; }

        /// <summary>
        /// Gets or sets the CSV.
        /// </summary>
        /// <value>The CSV.</value>
        [AppLocalizedStringDisplayName("PaymentSource.Card.CSV")]
        public string CSV { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        [AppLocalizedStringDisplayName("PaymentSource.Card.FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        [AppLocalizedStringDisplayName("PaymentSource.Card.LastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [AppLocalizedStringDisplayName("PaymentSource.Card.EMail")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the confirm email.
        /// </summary>
        /// <value>The confirm email.</value>
        [AppLocalizedStringDisplayName("PaymentSource.Card.ConfirmEMail")]
        public string ConfirmEmail { get; set; }

        /// <summary>
        /// Gets or sets the state region.
        /// </summary>
        /// <value>The state region.</value>
        [AppLocalizedStringDisplayName("PaymentSource.Card.StateRegion")]
        public string StateRegion { get; set; }

        /// <summary>
        /// Gets or sets the billing address1.
        /// </summary>
        /// <value>The billing address1.</value>
        [AppLocalizedStringDisplayName("PaymentSource.Card.BillingAddress1")]
        public string BillingAddress1 { get; set; }

        /// <summary>
        /// Gets or sets the billing address2.
        /// </summary>
        /// <value>The billing address2.</value>
        [AppLocalizedStringDisplayName("PaymentSource.Card.BillingAddress2")]
        public string BillingAddress2 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary.
        /// </summary>
        /// <value><c>true</c> if this instance is primary; otherwise, <c>false</c>.</value>
        [AppLocalizedStringDisplayName("PaymentSource.Card.IsPrimary")]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Gets or sets the months.
        /// </summary>
        /// <value>The months.</value>
        public IList<SelectListItem> Months { get; set; }

        /// <summary>
        /// Gets or sets the years.
        /// </summary>
        /// <value>The years.</value>
        public IList<SelectListItem> Years { get; set; }

        #endregion Properties
    }
}