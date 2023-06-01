using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Companies.API.DataTransferObjects;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Companies.API.Filters
{
    public class MatchingKeysFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var id = context.RouteData.Values["id"];

            var dto = context.ActionArguments.FirstOrDefault(p => p.Value.ToString().Contains("Dto")).Value;

            if (dto is IKey hasKey && id is not null && dto is not null)
            {
                if (id.ToString() == hasKey.Id.ToString())
                    return;
            }

            context.Result = new BadRequestObjectResult("Guid don't match");
        }
    }
}
