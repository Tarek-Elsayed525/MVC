using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string MainImg { get; set; }
        public decimal price { get; set; }
        public int Quantity { get; set; }
        public int Rate { get; set; }
        public int Discount { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
        public int BrandId { get; set; }
        [ForeignKey(nameof(BrandId))]

        public Brand Brand { get; set; }


    }
}
