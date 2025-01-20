using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BUlkyWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required] //not null
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [MaxLength(1,100)]
        public int DisplayOrder { get; set; }
    }
}
