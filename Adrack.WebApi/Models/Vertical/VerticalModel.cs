using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Adrack.Core.Domain.Lead;

namespace Adrack.WebApi.Models.Vertical
{
    public class VerticalModel : BaseModel
    {
        #region Constructor

        public VerticalModel()
        {
        }

        #endregion

        #region Properties
        public  long Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Invalid name")]
        [MinLength(3, ErrorMessage = "Invalid name")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Icon is required")]
        public string IconName { get; set; }
        /*public string Group { get; set; }
        public VerticalType Type { get; set; }
        public bool IsDeleted { get; set; }
        public List<VerticalFieldModel> Fields { get; set; }*/

        #endregion

        public static explicit operator Core.Domain.Lead.Vertical(VerticalModel verticalModel)
        {
            var fields = new List<VerticalField>();
            /*foreach (var field in verticalModel.Fields)
            {
                fields.Add((VerticalField)field);
            }*/
            return new Core.Domain.Lead.Vertical
            {
                Name = verticalModel.Name,
                VerticalFields = fields,
                Id = verticalModel.Id
            };
        }
    }
}