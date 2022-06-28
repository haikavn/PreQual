using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core.Domain.Content;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Support
{
    public class AllTicketsModel : BasePagedModel, IBaseOutModel
    {
        #region Constructor

        public AllTicketsModel()
        {
            supportTicketsViewItem = new List<SupportTicketsView>();
        }

        #endregion

        #region Properties

        public List<SupportTicketsView> supportTicketsViewItem { get; internal set; }

        public int TotalCount;

        #endregion
    }
}