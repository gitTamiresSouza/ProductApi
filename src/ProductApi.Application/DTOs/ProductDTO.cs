using System.ComponentModel.DataAnnotations;

namespace ProductApi.Application.DTOs
{
    public class ProductDTO
    {
        public ProductDTO() { }

        public int Id { get; set; }
        public string? Name { get; set; }
        [Range(0, Double.MaxValue, ErrorMessage = "The price {0} must be greater than {1}.")]
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
