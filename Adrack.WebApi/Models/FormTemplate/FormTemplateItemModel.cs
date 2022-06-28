using Adrack.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.FormTemplate
{
    public class FormTemplateItemModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public long FormTemplateId { get; set; }
        public FormTemplateElementType Type { get; set; }
        public List<FormTemplateItemStyles> StylesJson { get; set; } = new List<FormTemplateItemStyles>();
        public short Column { get; set; }
        public short Step { get; set; }
        public string ResponseFormat { get; set; }
        public long? ParentId { get; set; }
        public bool IsRequired { get; set; }
        public bool NeedsValidation { get; set; }
        public bool? InlineList { get; set; }
        public short Width { get; set; }
        public short Height { get; set; }
        public List<string> Options { get; set; } = new List<string>();
        public string HelperText { get; set; }
        public string Label { get; set; }
        public string PlaceHolderText { get; set; }
        public TextContentStyle LabelStyle { get; set; }
        public TextContentStyle HelperStyle { get; set; }
        public TextContentStyle PlaceHolderStyle { get; set; }
        public string ValidationRegex { get; set; }
        public long? ReferringFieldId { get; set; }

        public string Value { get; set; }

        public string TmpIconName { get; set; }
    }
}