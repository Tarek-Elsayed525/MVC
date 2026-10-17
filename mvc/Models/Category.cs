using System.ComponentModel.DataAnnotations;

namespace mvc.Models
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(30)]
        [MinLength(2)]
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}
