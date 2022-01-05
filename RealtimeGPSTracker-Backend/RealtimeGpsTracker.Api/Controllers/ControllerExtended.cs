using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RealtimeGpsTracker.Api.Controllers
{
    public class ControllerExtended : ControllerBase
    {
        public string GetOwnerId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public static List<string> GetErrors(ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        }
    }
}
