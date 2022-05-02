using Microsoft.EntityFrameworkCore;

namespace NutritionWebApplication.Models
{
    public class FoodContext : DbContext
    {

        public FoodContext(DbContextOptions<FoodContext> options) : base(options)
        {

        }

        public DbSet<Food> Foods { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Category> Categories  { get; set; }
        public DbSet<Nutrient> Nutrients { get; set; }

    }
}
