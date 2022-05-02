using System.ComponentModel.DataAnnotations;

namespace NutritionWebApplication.Models
{
    public class Owner
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
