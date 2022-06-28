using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core.Domain.Lead;

namespace Adrack.WebApi.Models.Vertical
{
    public class VerticalFieldModel : BaseModel
    {
        #region Constructor
        public VerticalFieldModel()
        {
        }

        #endregion

        #region Properties

        public long VerticalId { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public long Id { get; set; }

        #endregion

        public static explicit operator VerticalField(VerticalFieldModel verticalFieldModel)
        {
            return new VerticalField
            {
                VerticalId = verticalFieldModel.VerticalId,
                Name = verticalFieldModel.Name,
                DataType = verticalFieldModel.DataType,
                Description = verticalFieldModel.Description,
                IsRequired = verticalFieldModel.IsRequired,
                Id =  verticalFieldModel.Id
            };
        }
    }
}
