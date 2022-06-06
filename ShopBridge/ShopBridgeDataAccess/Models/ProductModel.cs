using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ShopBridgeDataAccess.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage ="Product Name Required")]
        [RegularExpression("@[a-zA-Z0-9]+$", ErrorMessage = "Special Character and space not allowed.")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Product Code Required")]
        [RegularExpression("@^[a-zA-Z ]+$",ErrorMessage ="Allow only character with space.")]
        public string ProductCode { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Price Required")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "File Required")]
        public IFormFile Image { get; set; }
        public string ImagePath { get; set; }

        public int CreatedBy { get; set; }
    }
    public class Pagination
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}
