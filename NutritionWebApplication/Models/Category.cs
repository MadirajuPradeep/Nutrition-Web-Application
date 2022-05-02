using System.ComponentModel.DataAnnotations;

namespace NutritionWebApplication.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
