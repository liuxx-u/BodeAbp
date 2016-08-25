using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace Abp.EntityFramework.Extensions
{
    public static class CreateIndexExtensions
    {
        public static PrimitivePropertyConfiguration CreateIndex(this PrimitivePropertyConfiguration propertyConfiguration)
        {
            return propertyConfiguration.HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(
                    new IndexAttribute()
                    )
                );
        }

        public static PrimitivePropertyConfiguration CreateIndex(this PrimitivePropertyConfiguration propertyConfiguration, string name)
        {
            return propertyConfiguration.HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(
                    new IndexAttribute(name)
                    )
                );
        }

        public static PrimitivePropertyConfiguration CreateIndex(this PrimitivePropertyConfiguration propertyConfiguration, string name, int order)
        {
            return propertyConfiguration.HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(
                    new IndexAttribute(name, order)
                    )
                );
        }
    }
}