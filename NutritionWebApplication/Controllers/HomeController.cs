using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NutritionWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nutrition.Models;
using Nutrition.Utilities;

namespace NutritionWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly FoodContext _context;

        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, FoodContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {

            return View();
        }


        public IActionResult Contact()
        {

            return View();
        }


        public IActionResult Trackers()
        {
            loadCombos();
            return View( new List<FoodView>());
        }


        void loadCombos()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.GetAsync($"{BASE_URL}foods/search?api_key={API_KEY}" +
                                                               $"&query=" +
                                                               $"&dataType=Branded"
                //$"&brandOwner={owner}" +
                //$"&pageSize=10"
            ).GetAwaiter().GetResult();
            var data = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var xl = Newtonsoft.Json.JsonConvert.DeserializeObject<FoodListView>(data);

            var blist = xl.foods.Select(c => c.brandName).Distinct();
            var clist = xl.foods.Select(c => c.foodCategory).Distinct();


            ViewData["CategoryId"] = new SelectList(clist);
            ViewData["BrandId"] = new SelectList(blist);

        }


        HttpClient httpClient = new HttpClient();
        static string BASE_URL = "https://api.nal.usda.gov/fdc/v1/";
        static string API_KEY = "Iyzx7vXF5bJDGL1D2WAV9TGGPy3UqHML3l1LqePy";
        [HttpPost]
        public IActionResult Trackers(string q, string owner)
        {
            loadCombos();

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.GetAsync($"{BASE_URL}foods/search?api_key={API_KEY}" +
                                                               $"&query={q}" +
                                                               $"&dataType=Branded" +
                                                               $"&brandOwner={owner}" +
                                                               $"&pageSize=10").GetAwaiter().GetResult();
            var data = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            var xl = Newtonsoft.Json.JsonConvert.DeserializeObject<FoodListView>(data);


            ViewData["q"] = q;
            ViewData["owner"] = owner;

            ViewBag.doit = "do";

            // TO DB 

            if (xl.foods.Count > 0)
            {
                // ADD Category 

                var categories = xl.foods.Select(c => c.foodCategory);
                var dbcategories = _context.Categories.Select(c => c.Name);
                var newcategories = categories.Except(dbcategories).ToList();

                if (newcategories.Any())
                {
                    _context.Categories.AddRange(newcategories.Select(c=> new Category(){Name = c}));
                    _context.SaveChanges();
                }

                var owners = xl.foods.Select(c => c.brandOwner);
                var dbowners = _context.Owners.Select(c => c.Name);
                var newowners = owners.Except(dbowners).ToList();

                if (newcategories.Any())
                {
                    _context.Owners.AddRange(newowners.Select(c => new Owner() { Name = c }));
                    _context.SaveChanges();
                }
                

                foreach (var food in xl.foods)
                {
                    var catId = _context.Categories.FirstOrDefault(c => c.Name == food.foodCategory)?.Id??0;
                    var ownId = _context.Owners.FirstOrDefault(c => c.Name == food.brandOwner)?.Id??0;
                    if(catId ==0 || ownId ==0 )
                        continue;

                    var newfood = new Food()
                    {
                        Name = food.description,
                        Fat = food.foodNutrients.FirstOrDefault(c => c.nutrientName == "Total lipid (fat)")?.value+"",
                        Protein = food.foodNutrients.FirstOrDefault(c => c.nutrientName == "Protein")?.value+"",
                        Carbohydrate = food.foodNutrients.FirstOrDefault(c => c.nutrientName == "Carbohydrate, by difference")?.value+"",
                        CategoryId = catId,
                        OwnerId = ownId,
                    };

                    _context.Foods.Add(newfood);
                    _context.SaveChanges();


                    // Add Nut

                    foreach (var nutrient in food.foodNutrients)
                    {
                        var newnutrient = new Nutrient()
                        {
                            FoodId = newfood.Id,
                            Name = nutrient.nutrientName,
                            Code = nutrient.derivationCode,
                            Desc = nutrient.derivationDescription,
                            Number = nutrient.nutrientNumber,
                            Unit = nutrient.unitName,
                            Value = nutrient.value+""
                        };
                        _context.Nutrients.Add(newnutrient);
                        _context.SaveChanges();
                    }

                }




            }



            return View(xl.foods);
        }

        public List<FoodData> foodItems = new();
        public async Task<IActionResult> Tracker(TrackerViewModel model)
        {
            string selectedBrand = model.SelectedBrandValue;
            string selectedCategory = model.SelectedCategoryValue;

            if (foodItems.Count == 0)
            {
                await GetFoodList();
            }

            List<SelectListItem> categories = new();
            List<SelectListItem> brands = new();

            categories.Add(new SelectListItem
            {
                Text = "Choose the food category",
                Value = ""
            });

            brands.Add(new SelectListItem
            {
                Text = "Choose the brand",
                Value = ""
            });

            List<FoodData> distinctFoodCategories = foodItems.GroupBy(elem => elem.description).Select(group => group.First()).ToList();
            List<FoodData> distinctFoodBrands = foodItems.GroupBy(elem => elem.brandOwner).Select(group => group.First()).ToList();
            List<NutrientsData> availableNutrients = new();

            foreach (FoodData item in distinctFoodCategories)
            {
                categories.Add(new SelectListItem
                {
                    Text = item.description,
                    Value = item.description
                });
            }

            foreach (FoodData item in distinctFoodBrands)
            {
                brands.Add(new SelectListItem
                {
                    Text = item.brandOwner,
                    Value = item.brandOwner
                });
            }

            if (string.IsNullOrEmpty(selectedBrand) == false && string.IsNullOrEmpty(selectedCategory) == false)
            {
                FoodData item = foodItems.Where(item => item.brandOwner == selectedBrand && item.description == selectedCategory).FirstOrDefault();
                if (item is not null)
                {
                    availableNutrients = item.foodNutrients;
                }
            }

            TrackerViewModel category = new()
            {
                CategoryValues = categories,
                BrandValues = brands,
                Nutrients = availableNutrients,
            };

            return View(category);
        }
        private async Task GetFoodList()
        {
            using HttpClientManager client = new HttpClientManager();
            HttpResponseMessage response = await client.Get("https://api.nal.usda.gov/fdc/v1/foods/list?api_key=GqXhMmXQTFPISxcBi2yQpRkUxZsrfulRM4eMbAfj&dataType=Branded&pageSize=200&pageNumber=1");
            if (response.IsSuccessStatusCode == true && response.Content is not null)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(jsonString) == false)
                {
                    foodItems = JsonSerializer.Deserialize<List<FoodData>>(jsonString);
                }
            }
        }

        public IActionResult ReadChartTrackers(string q, string owner)
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.GetAsync($"{BASE_URL}foods/search?api_key={API_KEY}"+
                                                               $"&query={q}" +
                                                               $"&dataType=Branded" +
                                                               $"&brandOwner={owner}" 
                                                               ).GetAwaiter().GetResult();
            var data = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var xl = Newtonsoft.Json.JsonConvert.DeserializeObject<FoodListView> (data);
            var grou= xl.foods.GroupBy(c => c.publishedDate,
                (key, g) => new { year = key, count = g.Count() });
                //.OrderByDescending(c=>c.count);


            return Json(
                new
                {
                    labels = grou.Select(c => c.year).Take(10),
                    data = grou.Select(c => c.count).Take(10)
                });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
