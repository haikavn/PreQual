using Adrack.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.FormTemplate
{
    public partial class FormTemplateUIModel
    {
        #region Properties

        public long Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        public string Properties { get; set; }
        public DateTime Created { get; set; }
        public long AffiliateChannelId { get; set; }
        public long CampaignId { get; set; }
        public FormTemplateType Type { get; set; }
        public IntegrationType IntegrationType { get; set; }

        public int Submissions { get; set; }
        public DateTime LastModified { get; set; }

        #endregion Properties
    }
}