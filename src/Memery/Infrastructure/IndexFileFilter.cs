using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Memery.Infrastructure
{
    public class IndexFileFilter : IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (context.HttpContext.Request.Path.Value.Contains("index.yml")) {
                context.Result = new Microsoft.AspNetCore.Mvc.NotFoundResult();
            }
        }
    }
}