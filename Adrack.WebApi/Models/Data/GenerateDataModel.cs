using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core;
using Adrack.Core.Domain.Lead;

namespace Adrack.WebApi.Models.Employee
{
    public class GenerateDataModel
    {
        public DataGenerationTypes DataTypes { get; set; }
        public Object Data { get; set; }
    }
}