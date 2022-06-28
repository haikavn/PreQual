using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.WebApi.Infrastructure.Core.Interfaces
{
    public interface ISearchService
    {
        bool CheckPropValue(object src, string inputValue);
    }
}
