using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FruitSA.Models
{
    public class Product
    {
        [Required]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Please provide a Product Code")]
        public string ProductCode { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please provide a Product Name")]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        [Required(ErrorMessage = "Please provide a Price")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public double Price { get; set; }

        [ValidateNever]
        [Display(Name = "Product Image")]
        public string? ImageUrl { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
