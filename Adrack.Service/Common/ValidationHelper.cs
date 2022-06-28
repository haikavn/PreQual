using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Service.Common
{
    public class ValidationHelper
    {
        public static Tuple<bool,string> ValidateImage(byte[] byteArray, string extension, List<string> allowedExtensions, double allowedMaxWeight, double allowedMaxHeight, double allowedLengthInBytes)
        {
            var validated = true;
            var validationMessage = string.Empty;
            var result = new Tuple<bool, string>(validated, validationMessage);

            try
            {
                using (var ms = new MemoryStream(byteArray))
                {
                    Image image = Image.FromStream(ms);
                    if (image.Height > allowedMaxHeight || image.Width > allowedMaxWeight)
                    {
                        validated = false;
                        validationMessage = $"Allowed image size is {allowedMaxHeight}px height and {allowedMaxWeight}px weight";
                        return result = new Tuple<bool, string>(validated, validationMessage);
                    }
                    if (!allowedExtensions.Contains(extension.ToLower()))
                    {
                        validated = false;
                        validationMessage = $".{extension} is not a valid image format";
                        return result = new Tuple<bool, string>(validated, validationMessage);
                    }
                    if (byteArray.Length > allowedLengthInBytes)
                    {
                        validated = false;
                        validationMessage = $"Invalid image size. Allowed image size should be maximum {allowedLengthInBytes}bytes";
                        return result = new Tuple<bool, string>(validated, validationMessage);
                    }
                }
            }
            catch (Exception exception)
            {
                return result = new Tuple<bool, string>(false, exception.Message);
            }

            return result;
        }
    }
}
