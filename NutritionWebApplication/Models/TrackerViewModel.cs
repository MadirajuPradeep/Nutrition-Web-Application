using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nutrition.Models
{
    public class TrackerViewModel
    {
        public string SelectedCategoryValue { get; set; }
        public string SelectedBrandValue { get; set; }
        public IEnumerable<SelectListItem> CategoryValues { get; set; }
        public IEnumerable<SelectListItem> BrandValues { get; set; }
        public IEnumerable<NutrientsData> Nutrients { get; set; }
    }
}
