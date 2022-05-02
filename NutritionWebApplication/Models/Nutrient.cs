using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutritionWebApplication.Models
{
    public class Nutrient
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Unit { get; set; }
        public string Desc { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }


        [ForeignKey("Food")]
        public int FoodId { get; set; }
        public Food Food { get; set; }
        
    }
}
