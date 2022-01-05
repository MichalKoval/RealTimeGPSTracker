using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace RealtimeGPSTracker.Application.Helpers
{
    // Custom DateTime model binder provider based on microsoft docs:
    // https://docs.microsoft.com/en-us/aspnet/core/mvc/advanced/custom-model-binding?view=aspnetcore-3.1
    public class DateTimeModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentException(nameof(context));
            }

            // Getting custom DateTime binder
            return (
                context.Metadata.ModelType == typeof(DateTime) ||
                context.Metadata.ModelType == typeof(DateTime?)
                ) ? new DateTimeModelBinder() : null;
        }
    }
}
