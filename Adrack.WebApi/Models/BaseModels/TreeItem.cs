using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.BaseModels
{
    /// <summary>
    /// Class TreeItem.
    /// </summary>
    public class TreeItem
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TreeItem"/> is folder.
        /// </summary>
        /// <value><c>true</c> if folder; otherwise, <c>false</c>.</value>
        public bool IsFolder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TreeItem"/> is expanded.
        /// </summary>
        /// <value><c>true</c> if expanded; otherwise, <c>false</c>.</value>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Gets or sets the template field.
        /// </summary>
        /// <value>The template field.</value>
        public string TemplateField { get; set; }

        /// <summary>
        /// Gets or sets the item template identifier.
        /// </summary>
        /// <value>The item template identifier.</value>
        public long CampaignTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>The default value.</value>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the minimum price option.
        /// </summary>
        /// <value>The minimum price option.</value>
        public short MinPriceOption { get; set; }

        /// <summary>
        /// Gets or sets the minimum price option value.
        /// </summary>
        /// <value>The minimum price option value.</value>
        public int MinPriceOptionValue { get; set; }

        /// <summary>
        /// Gets or sets the minimum revenue.
        /// </summary>
        /// <value>The minimum revenue.</value>
        public decimal MinRevenue { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public List<TreeItem> Children { get; set; }
        public string Matchings { get; internal set; }
        public long BuyerChannelTemplateId { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeItem"/> class.
        /// </summary>
        public TreeItem()
        {
            Children = new List<TreeItem>();
            IsFolder = false;
            IsExpanded = false;
        }
    }
}