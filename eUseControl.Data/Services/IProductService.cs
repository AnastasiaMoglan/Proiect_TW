using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Data.Services
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        List<Product> GetSunglasses();
        List<Product> GetOpticalFrames();
        List<Product> GetLenses();
        Product GetProductById(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
        void SeedProducts();
        List<Product> FilterSunglasses(decimal? minPrice, decimal? maxPrice);
        List<Product> FilterOpticalFrames(decimal? minPrice, decimal? maxPrice);
        List<Product> FilterLenses(decimal? minPrice, decimal? maxPrice);

    }
}