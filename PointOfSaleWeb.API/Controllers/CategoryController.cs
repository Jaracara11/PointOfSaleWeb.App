﻿using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Repository.Interfaces;

namespace PointOfSaleWeb.API.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _catRepo;
        public CategoryController(ICategoryRepository catRepo)
        {
            _catRepo = catRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var categories = await _catRepo.GetAllCategories();
            if (categories == null || !categories.Any())
            {
                return NotFound();
            }
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryByID(int id)
        {
            var category = await _catRepo.GetCategoryByID(id);

            if (category == null)
            {
                return NotFound();
            }

            if (category != null)
            {
                return Ok(category);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
