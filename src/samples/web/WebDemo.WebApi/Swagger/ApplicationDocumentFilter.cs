using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace WebDemo.WebApi.Swagger
{
    public class ApplicationDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            //添加Tag
            swaggerDoc.tags = new List<Tag>();
            var controllers = apiExplorer.ApiDescriptions.Select(p => p.ActionDescriptor.ControllerDescriptor).Distinct();
            foreach (var item in controllers)
            {
                var desc = item.GetCustomAttributes<DescriptionAttribute>();
                if (desc != null && desc.Count > 0)
                {
                    swaggerDoc.tags.Add(new Tag() { name = item.ControllerName, description = desc[0].Description });
                }
            }
        }
    }
}
