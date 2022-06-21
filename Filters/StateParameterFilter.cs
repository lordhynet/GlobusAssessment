using GlobusAssessment.Domain.Models;
using GlobusAssessment.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GLobusAssessment.Api.Filters
{
    public class StateParameterFilter : IParameterFilter
    {
        readonly IServiceScopeFactory _serviceScopeFactory;

        public StateParameterFilter(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            if (parameter.Name.Equals("state", StringComparison.InvariantCultureIgnoreCase))
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    IEnumerable<State> states = _context.States.ToArray();

                    parameter.Schema.Enum = states.Select(p => new OpenApiString(p.Name)).ToList<IOpenApiAny>();
                }
            }
        }
    }
}
