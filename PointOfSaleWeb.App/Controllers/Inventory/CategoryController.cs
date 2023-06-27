﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Repository.Interfaces;

namespace PointOfSaleWeb.App.Controllers.Inventory
{
    [Route("api/category")]
    [Authorize]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _catRepo;
        public CategoryController(ICategoryRepository catRepo)
        {
            _catRepo = catRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories() => Ok(await _catRepo.GetAllCategories());

        [HttpGet("{id}")]
        [ResponseCache(Duration = 5)]
        public async Task<ActionResult<Category>> GetCategoryByID(int id)
        {
            var category = await _catRepo.GetCategoryByID(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<Category>> AddNewCategory(Category category)
        {
            var response = await _catRepo.AddNewCategory(category.CategoryName);

            if (!response.Success)
            {
                ModelState.AddModelError("CategoryError", response.Message);
                return BadRequest(ModelState);
            }

            return Created("Category", response.Data);
        }

        [HttpPut("edit")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> UpdateCategory(Category category)
        {
            var response = await _catRepo.UpdateCategory(category);

            if (!response.Success)
            {
                ModelState.AddModelError("CategoryError", response.Message);
                return BadRequest(ModelState);
            }

            return Ok(response);
        }

        [HttpDelete("{id}/delete")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var response = await _catRepo.DeleteCategory(id);

            if (!response.Success)
            {
                ModelState.AddModelError("CategoryError", response.Message);
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
