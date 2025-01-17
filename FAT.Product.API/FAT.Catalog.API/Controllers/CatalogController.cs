﻿using FAT.Catalog.API.Models;
using FAT.Catalog.API.Models.Interfaces;
using FAT.Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FAT.Catalog.API.Controllers
{
    [ApiController]
    [Authorize]
    public class CatalogController
    {
        private readonly IProductRepository _productRepository;

        public CatalogController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [ClaimsAuthorize("Catalog", "Read")]
        [HttpGet("catalog/products")]
        public async Task<IEnumerable<Product>> Index()
        {
            return await _productRepository.GetAll();
        }

        [ClaimsAuthorize("Catalog", "Read")]
        [HttpGet("catalog/products/{id}")]
        public async Task<Product> ProductDetail(Guid id)
        {
            return await _productRepository.GetById(id);
        }
    }
}
