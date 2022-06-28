using System;

namespace Adrack.Core.Domain.Lead
{
    public partial class FormTemplateItem : BaseEntity
    {
        #region Properties
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public long FormTemplateId { get; set; }
        public FormTemplateElementType Type { get; set; }
        public string StylesJson { get; set; }
        public short Column { get; set; }
        public short Step { get; set; }
        public string ResponseFormat { get; set; }
        public long? ParentId { get; set; }
        public bool IsRequired { get; set; }
        public bool NeedsValidation { get; set; }
        public bool? InlineList { get; set; }
        public short Width { get; set; }
        public short Height { get; set; }
        public string Options { get; set; }
        public string HelperText { get; set; }
        public string Label { get; set; }
        public string PlaceHolderText { get; set; }
        public string LabelStyle { get; set; }
        public string HelperStyle { get; set; }
        public string PlaceHolderStyle { get; set; }
        public string ValidationRegex { get; set; }
        public long? ReferringFieldId { get; set; }

        public string Value { get; set; }

        #endregion Properties
    }
}