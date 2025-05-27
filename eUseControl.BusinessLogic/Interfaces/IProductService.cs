using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.BusinessLogic.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);

        IEnumerable<Product> GetSunglasses();
        IEnumerable<Product> GetOpticalFrames();
        IEnumerable<Product> GetLenses();

        List<Product> FilterSunglasses(decimal? minPrice, decimal? maxPrice);
        IEnumerable<Product> FilterOpticalFrames(decimal? minPrice, decimal? maxPrice);
        IEnumerable<Product> FilterLenses(decimal? minPrice, decimal? maxPrice);

        IEnumerable<Product> GetProductsByType(string type);

        void SeedProducts();
    }
}