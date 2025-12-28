using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using System.Threading.Tasks;

namespace CollegeGradingSys.Utilities.Binders
{
    public class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext
                .ValueProvider
                .GetValue(bindingContext.ModelName);

            var cultureInfo = new CultureInfo("en");

            decimal.TryParse(
                valueProviderResult.FirstValue,
                NumberStyles.AllowDecimalPoint,
                cultureInfo,
                out var model);

            bindingContext
                .ModelState
                .SetModelValue(bindingContext.ModelName, valueProviderResult);

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}