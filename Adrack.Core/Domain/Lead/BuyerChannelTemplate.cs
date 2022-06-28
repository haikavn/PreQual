// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerChannelTemplate.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class BuyerChannelTemplate.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class BuyerChannelTemplate : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or Sets the State province Identifier
        /// </summary>
        /// <value>The campaign template identifier.</value>
        public long CampaignTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the buyer channel identifier.
        /// </summary>
        /// <value>The buyer channel identifier.</value>
        public long BuyerChannelId { get; set; }

        /// <summary>
        /// Gets or Sets the First name
        /// </summary>
        /// <value>The template field.</value>
        public string TemplateField { get; set; }

        /// <summary>
        /// Gets or sets the name of the section.
        /// </summary>
        /// <value>The name of the section.</value>
        public string SectionName { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>The default value.</value>
        public string DefaultValue { get; set; }


        /// <summary>
        /// Gets or sets the data format.
        /// </summary>
        /// <value>The data format.</value>
        public string DataFormat { get; set; }

        [NotMapped]
        public List<string> DataFormatValues
        {
            get
            {
                List<string> list = new List<string>();
                if (!string.IsNullOrEmpty(DataFormat))
                {
                    list.AddRange(DataFormat.Split(new char[1] { ',' }));
                }
                return list;
            }
        }

        #endregion Properties

        #region Navigation properties
        private BuyerChannel _buyerChannel;
        public virtual BuyerChannel BuyerChannel
        {
            get
            {
                return _buyerChannel;
            }
            set
            {
                _buyerChannel = value;
            }
        }

        #endregion
    }
}