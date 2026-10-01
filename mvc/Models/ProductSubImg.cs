using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    [PrimaryKey(nameof(Image), nameof(ProductId))]
    public class ProductSubImg
    {
        public string Image { get; set; }
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
