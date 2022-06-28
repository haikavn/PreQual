using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.WebApi.Models.Interfaces
{
    public interface IPagedData
    {
        int RecordsStart { get; set; }
        int RecordsTotal { get; set; }
        int RecordsFiltered { get; set; }
    }
}
