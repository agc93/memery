using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Memery.Infrastructure
{
    public class AngularConstraintAttribute : Attribute, IActionConstraint
    {
        private List<string> ReservedFiles => new List<string> {
            "main.js",
            "runtime.js",
            "styles.css"
        };

        private List<string> Extensions => new List<string> {
            ".js",
            ".css",
            ".html"
        };

        public int Order => 1;

        public bool Accept(ActionConstraintContext context)
        {
            return !Extensions.Any(e => context.RouteContext.HttpContext.Request.Path.Value.EndsWith(e));
            // return !context.RouteContext.HttpContext.Request.Path.Value.EndsWith(".js");
        }
    }
}