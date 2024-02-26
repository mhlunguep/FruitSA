using System.ComponentModel.DataAnnotations;

namespace FruitSA.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        public string CategoryCode { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Username { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
    }
}
