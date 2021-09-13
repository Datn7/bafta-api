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
using AutoMapper;
using bafta_api.Dtos;

namespace bafta_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> productsRepo;
        private readonly IGenericRepository<ProductType> prouctTypeRepo;
        private readonly IMapper mapper;

        public ProductsController(IGenericRepository<Product> productsRepo, IGenericRepository<ProductType> prouctTypeRepo, IMapper mapper)
        {
            this.productsRepo = productsRepo;
            this.prouctTypeRepo = prouctTypeRepo;
            this.mapper = mapper;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesSpecification();
            var products = await productsRepo.ListAsync(spec);

            return Ok(mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }

        // GET: api/Products/1

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesSpecification(id);

            var product = await productsRepo.GetEntityWithSpec(spec);

            return mapper.Map<Product, ProductToReturnDto>(product);
        }


        // GET: api/types
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await prouctTypeRepo.ListAllAsync());
        }

    }
}