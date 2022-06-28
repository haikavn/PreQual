using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.BaseModels
{
    public class UserAddonsModel : BaseIdentifiedItem
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public string AddData { get; set; }

        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public short Status { get; set; }
    }
}