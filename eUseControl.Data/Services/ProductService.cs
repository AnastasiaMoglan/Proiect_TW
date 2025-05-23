using System;
using System.Collections.Generic;
using eUseControl.Data.Repository;
using eUseControl.Domain.Entities;

namespace eUseControl.Data.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService()
        {
            _productRepository = new ProductRepository();
        }

        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }

        public List<Product> GetSunglasses()
        {
            return _productRepository.GetProductsByType("Sunglasses");
        }

        public List<Product> GetOpticalFrames()
        {
            return _productRepository.GetProductsByType("OpticalFrames");
        }

        public List<Product> GetLenses()
        {
            return _productRepository.GetProductsByType("Lenses");
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
            var sunglasses = _productRepository.FilterProductsByType("Sunglasses", minPrice, maxPrice);

            return sunglasses;
        }

        public List<Product> FilterOpticalFrames(decimal? minPrice, decimal? maxPrice)
        {
            var opticalFrames = _productRepository.FilterProductsByType("OpticalFrames", minPrice, maxPrice);

            return opticalFrames;
        }

        public List<Product> FilterLenses(decimal? minPrice, decimal? maxPrice)
        {
            var lenses = _productRepository.FilterProductsByType("Lenses", minPrice, maxPrice);

            return lenses;
        }
    }
}