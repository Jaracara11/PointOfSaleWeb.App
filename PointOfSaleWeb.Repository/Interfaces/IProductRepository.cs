﻿using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;

namespace PointOfSaleWeb.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<IEnumerable<BestSellerProductDTO>> GetBestSellerProducts();
        Task<IEnumerable<Product>> GetProductsByCategoryID(int id);
        Task<Product> GetProductByID(string id);
        Task<DbResponse<Product>> AddNewProduct(Product product);
        Task<DbResponse<Product>> UpdateProduct(Product product);
        Task<DbResponse<Product>> DeleteProduct(string id);
    }
}