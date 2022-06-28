using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class AffiliateCampaignTemplateModel
    {
        /// <summary>
        /// The template field
        /// </summary>
        public string TemplateField = string.Empty;

        /// <summary>
        /// The description
        /// </summary>
        public string Description = string.Empty;

        /// <summary>
        /// The format
        /// </summary>
        public string Format = string.Empty;

        /// <summary>
        /// Is required
        /// </summary>
        public bool IsRequired = false;
    }
}