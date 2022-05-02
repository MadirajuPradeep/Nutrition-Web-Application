using System.Collections.Generic;

namespace Nutrition.Models
{
    public class FoodData
    {
        public int fdcId { get; set; }
        public string description { get; set; }
        public string dataType { get; set; }
        public string publicationDate { get; set; }
        public string brandOwner { get; set; }
        public string gtinUpc { get; set; }
        public List<NutrientsData> foodNutrients { get; set; }
    }

    public class NutrientsData
    {
        public string number { get; set; }
        public string name { get; set; }
        public double amount { get; set; }
        public string unitName { get; set; }
        public string derivationCode { get; set; }
        public string derivationDescription { get; set; }
    }
}
