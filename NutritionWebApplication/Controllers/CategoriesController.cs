using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NutritionWebApplication.Models;

namespace NutritionWebApplication.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly FoodContext _context;

        public CategoriesController(FoodContext context)
        {
            _context = context;
        }

   
        public IActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),"Foods");
            }
            return View(category);
        }

    }
}
