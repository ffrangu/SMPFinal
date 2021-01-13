using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace SMP.Utils.Binders
{
    public class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == null)
            {
                return Task.CompletedTask;
            }

            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            value = value.Replace(",", string.Empty).Trim();

            decimal myValue = 0;
            if (!decimal.TryParse(value, NumberStyles.Any, new CultureInfo("en-US"), out myValue))
            {
                bindingContext.ModelState.TryAddModelError(
                                        bindingContext.ModelName,
                                        "Could not parse MyValue.");
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(myValue);
            return Task.CompletedTask;
        }
    }
}