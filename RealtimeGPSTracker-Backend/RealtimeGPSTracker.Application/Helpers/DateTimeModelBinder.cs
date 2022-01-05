using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace RealtimeGPSTracker.Application.Helpers
{
    // Custom DateTime model binder based on microsoft docs:
    // https://docs.microsoft.com/en-us/aspnet/core/mvc/advanced/custom-model-binding?view=aspnetcore-3.1
    public class DateTimeModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Throwing exception when binding context is null
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            // Getting value provider for binding context argument by it's model name
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            // Setting model value to binding context
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            // Checking if string value which represents DateTime value exists
            string dateTimeStr = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(dateTimeStr))
            {
                return Task.CompletedTask;
            }

            // Parsing DateTime value to match specific DateTime format: yyyyMMddTHHmmss
            DateTime parserDateTime;
            if (!DateTime.TryParseExact(dateTimeStr, "yyyyMMddTHHmmss", null, System.Globalization.DateTimeStyles.None, out parserDateTime))
            {
                // Setting binding error that string does not match specific DateTime format
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "DateTime must be in format 'yyyyMMddTHHmmss'");
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(parserDateTime);
            return Task.CompletedTask;            
        }
    }
}
