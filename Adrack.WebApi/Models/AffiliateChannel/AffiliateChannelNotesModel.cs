using System;
using System.Collections.Generic;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.AffiliateChannel
{
    public class AffiliateChannelNotesModel
    {
        #region constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public AffiliateChannelNotesModel()
        {
            
        }

        #endregion constructor

        #region properties
        public string Notes { get; set; }
        #endregion Properties
    }
}