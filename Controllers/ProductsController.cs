using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bafta_api.Data;
using bafta_api.Entities;
using bafta_api.Interfaces;
using bafta_api.Specifications;

namespace bafta_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> productsRepo;
        private readonly IGenericRepository<ProductType> prouctTypeRepo;

        public ProductsController(IGenericRepository<Product> productsRepo, IGenericRepository<ProductType> prouctTypeRepo)
        {
            this.productsRepo = productsRepo;
            this.prouctTypeRepo = prouctTypeRepo;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var spec = new ProductsWithTypesSpecification();
            var products = await productsRepo.ListAsync(spec);

            return Ok(products);
        }

        // GET: api/Products/1

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesSpecification(id);

            return await productsRepo.GetEntityWithSpec(spec);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await prouctTypeRepo.ListAllAsync());
        }

    }
}