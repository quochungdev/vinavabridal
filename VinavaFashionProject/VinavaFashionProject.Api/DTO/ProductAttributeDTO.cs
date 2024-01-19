using Microsoft.CodeAnalysis;

namespace VinavaFashionProject.Api.DTO
{
    public class ProductAttributeDTO
    {
        public int Id { get; set; }

        public int? ProductId { get; set; }

        public int? AttributeId { get; set; }

        public virtual AttributeDTO? Attribute { get; set; }

    }
}
