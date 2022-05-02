using System;
using System.Collections.Generic;
using System.Globalization;

namespace NutritionWebApplication.Models
{

    public class FoodListView
    {
        public List<FoodView> foods { get; set; }

    }

    public class FoodView
    {
        public int fdcId { get; set; }
        public string description { get; set; }
        public string lowercaseDescription { get; set; }
        public string dataType { get; set; }
        public string gtinUpc { get; set; }
        public string publishedDate { get; set; }
        public DateTime publishedDateTime => DateTime.ParseExact(publishedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        public string brandOwner { get; set; }
        public string brandName { get; set; }
        public string ingredients { get; set; }
        public string marketCountry { get; set; }
        public string foodCategory { get; set; }
        public string modifiedDate { get; set; }
        public string dataSource { get; set; }
        public string packageWeight { get; set; }
        public string servingSizeUnit { get; set; }
        public float servingSize { get; set; }
        public string allHighlightFields { get; set; }
        public float score { get; set; }
        public string householdServingFullText { get; set; }
        public string subbrandName { get; set; }
        public List<foodNutrient> foodNutrients { get; set; }


    }


    public class foodNutrient
    {
        public int nutrientId { get; set; }
        public string nutrientName { get; set; }
        public string nutrientNumber { get; set; }
        public string unitName { get; set; }
        public string derivationCode { get; set; }
        public string derivationDescription { get; set; }
        public int derivationId { get; set; }
        public float value { get; set; }
        public int foodNutrientSourceId { get; set; }
        public string foodNutrientSourceCode { get; set; }
        public string foodNutrientSourceDescription { get; set; }
        public int rank { get; set; }
        public int indentLevel { get; set; }
        public int foodNutrientId { get; set; }
    }




}



