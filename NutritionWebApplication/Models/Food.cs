using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutritionWebApplication.Models
{
    public class Food
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Owner")]
        public int OwnerId { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Owner Owner { get; set; }
        public Category Category { get; set; }

        public string Protein { get; set; }
        public string Fat { get; set; }
        public string Carbohydrate { get; set; }
        

    }
}
