using Microsoft.EntityFrameworkCore;

namespace mvc.Data
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Models.Brand> Brands { get; set; }
        public DbSet<Models.ProductColor> ProductColors { get; set; }
        public DbSet<Models.ProductSubImg> ProductSubImgs { get; set; }


        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-8F377B2;initial catalog=MvcTest;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True");
        }
    }
}
