using System.Linq;
using System.Web.Http.ModelBinding;

namespace Adrack.WebApi.Extensions
{
    public static class ValidationExtensions
    {
        public static string GetErrorMessage(this ModelStateDictionary modelState)
        {
            var allErrors = modelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
            return string.Join(", ", allErrors);
        }
    }
}