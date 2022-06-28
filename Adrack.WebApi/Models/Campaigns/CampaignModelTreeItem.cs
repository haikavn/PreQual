using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Campaigns
{
    public class CampaignModelTreeItem : BaseIdentifiedItem
    {

        #region Fields
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CampaignModelTreeItem"/> is folder.
        /// </summary>
        /// <value><c>true</c> if folder; otherwise, <c>false</c>.</value>
        public bool Folder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CampaignModelTreeItem"/> is expanded.
        /// </summary>
        /// <value><c>true</c> if expanded; otherwise, <c>false</c>.</value>
        public bool Expanded { get; set; }

        /// <summary>
        /// Gets or sets the database field.
        /// </summary>
        /// <value>The database field.</value>
        public string DatabaseField { get; set; }

        /// <summary>
        /// Gets or sets the validator.
        /// </summary>
        /// <value>The validator.</value>
        public short Validator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CampaignModelTreeItem"/> is required.
        /// </summary>
        /// <value><c>true</c> if required; otherwise, <c>false</c>.</value>
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is hash.
        /// </summary>
        /// <value><c>true</c> if this instance is hash; otherwise, <c>false</c>.</value>
        public bool IsHash { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is hidden.
        /// </summary>
        /// <value><c>true</c> if this instance is hidden; otherwise, <c>false</c>.</value>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the possible value.
        /// </summary>
        /// <value>The possible value.</value>
        public string PossibleValue { get; set; }

        /// <summary>
        /// Gets or sets the black list type identifier.
        /// </summary>
        /// <value>The black list type identifier.</value>
        public long BlackListTypeId { get; set; }

        /// <summary>
        /// Gets or sets the minimum length.
        /// </summary>
        /// <value>The minimum length.</value>
        public int MinLength { get; set; }

        /// <summary>
        /// Gets or sets the maximum length.
        /// </summary>
        /// <value>The maximum length.</value>
        public int MaxLength { get; set; }

        // <summary>
        // The data format HTML
        // </summary>
        //public string DataFormatHtml = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is filterable.
        /// </summary>
        /// <value><c>true</c> if this instance is filterable; otherwise, <c>false</c>.</value>
        public bool IsFilterable { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the column number.
        /// </summary>
        /// <value>The column number.</value>
        public int ColumnNumber { get; set; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        /// <value>The page number.</value>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is form field.
        /// </summary>
        /// <value><c>true</c> if this instance is form field; otherwise, <c>false</c>.</value>
        public bool IsFormField { get; set; }

        /// <summary>
        /// Gets or sets the option values.
        /// </summary>
        /// <value>The option values.</value>
        public string OptionValues { get; set; }

        /// <summary>
        /// Gets or sets the type of the field.
        /// </summary>
        /// <value>The type of the field.</value>
        public short FieldType { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public List<CampaignModelTreeItem> Children { get; set; }

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignModelTreeItem"/> class.
        /// </summary>
        public CampaignModelTreeItem()
        {
            Children = new List<CampaignModelTreeItem>();
            Folder = false;
            Expanded = false;
        }

        #endregion
    }

}