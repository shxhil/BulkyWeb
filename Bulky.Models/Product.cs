using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Title")]
        [MaxLength(90)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string ISBN { get; set; }

        [Required]
        [MaxLength(50)]
        public string Author { get; set; }

        [Required]
        [Range(1, 1000)]
        [DisplayName("List Price")]
        public double ListPrice { get; set; }

        [Required]
        [Range(1, 1000)]
        [DisplayName("Price for 1-50")]
        public double Price { get; set; }

        [Required]
        [Range(1, 1000)]
        [DisplayName("Price for 50+")]
        public double Price50 { get; set; }

        [Required]
        [Range(1, 1000)]
        [DisplayName("Price for 100+")]
        public double Price100 { get; set; }

        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }
    }
}
