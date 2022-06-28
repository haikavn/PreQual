using AutoMapper.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.FormBuilder
{
    public enum EnumDataFormat
    {
        Text = 0,
        Time = 1,
        File = 2,
        Month = 3,
        Week = 4,
        Date = 5,
        DatetimeLocal = 6,
        Email = 7,
        Url = 8,
        Search = 9,
        Tel = 10,
        Color = 11,
        Number = 12,
        Password = 13,
        Checkbox = 14,
        Radio = 15,
        Image = 16,
        Select = 17,
        Button=18
    }


    [Serializable]
    public class FormBuilderFieldGenerationOptions
    {
        public long Step=0;
        public string Name = "Default";
        public enum EnumColumnPostion
        {
            Left=0,
            MiddleTop=1,
            Right=2,
            MiddleBottom=3
        }

        
        public enum EnumDataType
        {
            Text,
            CountryCode,
            Checkbox,
            Number,
            NumberFloat
        }
        public class FormBuilderFieldLayout
        {
            public object ColumnGrid;
            public object ColumnGap;
            public object FieldsGap;
            public object ColumnLabelAlign;
            public object LabelAndInputSpacing;
        }

        public class FormBuilderFieldProperties
        {            
            public string DefaultValue;
            public object DataFormat;
            public bool Required;
            public string InlineList="";
            public string ValidationRule;
            public string Label;
            public string HelperText;
            public string PlaceHolderText;
            public string ImageBase64;
            public int Width;
            public int Height;
        }

        public class FormBuilderFieldStyle
        {
            public object Italic;
            public object Bold;
            public object TextSize;
            public object TextInputColor;
            public object TextLabelColor;
            public object BorderColor;            
            public object BorderWidth;
            public object ID;            
            public object BackgroundColor;
            public object ButtonAction;
            public object ButtonText;
            public object AdditionalClass;
            public object ImageURL;            
        }

        public FormBuilderFieldLayout Layout=new FormBuilderFieldLayout();
        public FormBuilderFieldStyle Style=new FormBuilderFieldStyle();
        public FormBuilderFieldProperties Properties=new FormBuilderFieldProperties();

        public void TestFill(int gridPos,string prefix, EnumColumnPostion columnPosition, EnumDataFormat format=EnumDataFormat.Text)
        {
            Layout.ColumnGap = 1;
            Layout.ColumnGrid = gridPos;
            Layout.ColumnLabelAlign = columnPosition;
            Layout.FieldsGap = 10;
            Layout.LabelAndInputSpacing = 10;

            Properties.DataFormat = format;
            
            Properties.DefaultValue = "Default Value";
            Properties.Height = 50;
            Properties.Width = 100;
            Properties.ValidationRule = "";
            Properties.Required = true;
            Properties.Label = prefix + "Label";
            Properties.HelperText = prefix + "Helper";
            Properties.PlaceHolderText = prefix + "PlaceHolder";

            Style.BorderColor = "black";
            Style.BorderWidth = 1;
            Style.TextInputColor = "black";
            Style.TextLabelColor = "black";
            Style.TextSize = 11;
        }
    }
}