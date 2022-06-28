using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Support
{
    public class TicketsModel : BasePagedModel, IBaseOutModel
    {
        #region Constructor

        public TicketsModel()
        {
            TicketMessageViewItems = new List<TicketMessageViewItem>();
        }

        #endregion

        #region Properties

        public List<TicketMessageViewItem> TicketMessageViewItems { get; internal set; }

        #endregion
    }
}