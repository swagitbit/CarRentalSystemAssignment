using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystemAssignment.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Where(i=>i.Value.Errors.Any()).SelectMany(i=>i.Value.Errors).Select(i=>i.ErrorMessage).ToList();
                context.Result = new BadRequestObjectResult(new
                {
                    Message = "Validation Failed",
                    Errors = errors
                });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
