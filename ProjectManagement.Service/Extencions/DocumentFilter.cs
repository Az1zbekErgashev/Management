using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProjectManagement.Service.Extencions;
public class LowercaseDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var paths = swaggerDoc.Paths.ToDictionary(
            entry => entry.Key.ToLowerInvariant(),
            entry => entry.Value
        );

        swaggerDoc.Paths = new OpenApiPaths();
        foreach (var path in paths)
        {
            swaggerDoc.Paths.Add(path.Key, path.Value);
        }
    }
}
