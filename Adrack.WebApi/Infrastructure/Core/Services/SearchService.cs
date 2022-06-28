using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Infrastructure.Core.Interfaces;

namespace Adrack.WebApi.Infrastructure.Core.Services
{
    public class SearchService: ISearchService
    {

        public bool CheckPropValue(object src, string inputValue)
        {
            foreach (var prop in src.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                {
                    var value = src.GetType().GetProperty(prop?.Name)?.GetValue(src, null)?.ToString();
                    if (!string.IsNullOrEmpty(value) &&
                        value.Replace(" ", string.Empty).ToLower().
                            Contains(inputValue.Replace(" ", string.Empty).ToLower()))
                    {
                        return true;
                    }
                }
                else if (prop.PropertyType == typeof(Affiliate))
                {
                    var propertyInfo = src.GetType().GetProperty(prop?.Name);
                    var subSrc = propertyInfo?.GetValue(src, null);
                    while (CheckPropValue(subSrc, inputValue))
                    {
                        return true;
                    }
                }
                else
                {
                    continue;
                }
            }
            return false;
        }
    }
}