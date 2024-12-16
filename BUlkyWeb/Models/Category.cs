using System.ComponentModel.DataAnnotations;

namespace BUlkyWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required] //not null
        public string Name { get; set; }

        public int DisplayOrder { get; set; }
    }
}
