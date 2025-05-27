using System.Collections.Generic;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;
using eUseControl.BusinessLogic.Interfaces;

namespace eUseControl.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        // Constructor cu injecție de dependență corectă
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }

        public IEnumerable<Product> GetSunglasses()
        {
            return _productRepository.GetProductsByType("Sunglasses");
        }

        public IEnumerable<Product> GetOpticalFrames()
        {
            return _productRepository.GetProductsByType("OpticalFrames");
        }

        public IEnumerable<Product> GetLenses()
        {
            return _productRepository.GetProductsByType("Lenses");
        }

        public IEnumerable<Product> GetProductsByType(string type)
        {
            return _productRepository.GetProductsByType(type);
        }

        public Product GetProductById(int id)
        {
            return _productRepository.GetProductById(id);
        }

        public void AddProduct(Product product)
        {
            _productRepository.AddProduct(product);
        }

        public void UpdateProduct(Product product)
        {
            _productRepository.UpdateProduct(product);
        }

        public void DeleteProduct(int id)
        {
            _productRepository.DeleteProduct(id);
        }

        public void SeedProducts()
        {
            _productRepository.SeedProducts();
        }

        public List<Product> FilterSunglasses(decimal? minPrice, decimal? maxPrice)
        {
            return _productRepository.FilterProductsByType("Sunglasses", minPrice, maxPrice);
        }

        public IEnumerable<Product> FilterOpticalFrames(decimal? minPrice, decimal? maxPrice)
        {
            return _productRepository.FilterProductsByType("OpticalFrames", minPrice, maxPrice);
        }

        public IEnumerable<Product> FilterLenses(decimal? minPrice, decimal? maxPrice)
        {
            return _productRepository.FilterProductsByType("Lenses", minPrice, maxPrice);
        }
    }
}
