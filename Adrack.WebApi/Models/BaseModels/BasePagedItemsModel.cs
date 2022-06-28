using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Infrastructure.Web.Helpers;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.BaseModels
{
    public class BasePagedItemsModel<T, TItem> : BasePagedModel where T : IList<TItem>
    {
        public BasePagedItemsModel(T tItems)
        {
             Items = tItems;
             if (Items.Count > 3)
             {
                 this.SetInstanceValues(1, Items.Count, Items.Count);
             }
             else
             {
                 this.SetInstanceValues();
             }
        }
        public T Items { get; set; }
    }
}