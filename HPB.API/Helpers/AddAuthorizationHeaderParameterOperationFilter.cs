using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace HPB.API.Helpers
{
    public class AddAuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var descriptor = context.ApiDescription.ActionDescriptor;

            var isAuthorized = descriptor.FilterDescriptors
                 .Any(i => i.Filter is AuthorizeFilter);


            var allowAnonymous = descriptor.FilterDescriptors
                .Any(i => i.Filter is AllowAnonymousFilter);

            if (isAuthorized && !allowAnonymous)
            {
                if (operation.Parameters != null)
                {
                    operation.Parameters.Add(new NonBodyParameter
                    {
                        Name = "Authorization",
                        In = "header",
                        Description = "access token",
                        Required = false,
                        Type = "string",
                        Default = "Bearer "
                    });
                }
            }
        }
    }
}
