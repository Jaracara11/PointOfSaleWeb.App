﻿using System.ComponentModel.DataAnnotations;

namespace PointOfSaleWeb.Models
{
    [Dapper.Contrib.Extensions.Table("Categories")]
    public class Category
    {
        [Dapper.Contrib.Extensions.Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Category name must have between 4 and 50 characters.")]
        public string CategoryName { get; set; } = string.Empty;
    }
}
