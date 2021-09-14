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
using bafta_api.Errors;
using bafta_api.Helpers;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using CloudinaryDotNet.Actions;

namespace bafta_api.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> productsRepo;
        private readonly IGenericRepository<ProductType> prouctTypeRepo;
        private readonly IMapper mapper;
        private readonly IOptions<CloudinarySettings> cloudinaryConfig;
        private readonly IPhotoService photoService;
        private readonly StoreContext storeContext;
        private Cloudinary cloudinary;

        public ProductsController(
            IGenericRepository<Product> productsRepo, 
            IGenericRepository<ProductType> prouctTypeRepo,
            IMapper mapper,
            IOptions<CloudinarySettings> cloudinaryConfig,
            IPhotoService photoService,
            StoreContext storeContext

            )
        {
            this.productsRepo = productsRepo;
            this.prouctTypeRepo = prouctTypeRepo;
            this.mapper = mapper;
            this.cloudinaryConfig = cloudinaryConfig;
            this.photoService = photoService;
            this.storeContext = storeContext;

            //configure cloudinary config
            Account acc = new Account(
                cloudinaryConfig.Value.CloudName,
                cloudinaryConfig.Value.ApiKey,
                cloudinaryConfig.Value.ApiSecret
            );

            //pass account configuration to a field type of Cloudinary
            cloudinary = new Cloudinary(acc);
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesSpecification(productParams);
            var countSpec = new ProductWithFiltersForCountSpecification(productParams);
            var totalItems = await productsRepo.CountAsync(countSpec);
            var products = await productsRepo.ListAsync(spec);

            var data = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,productParams.PageSize,totalItems,data));
        }

        // GET: api/Products/1

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesSpecification(id);

            var product = await productsRepo.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            return mapper.Map<Product, ProductToReturnDto>(product);
        }


        // GET: api/types
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await prouctTypeRepo.ListAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {


            storeContext.Products.Add(product);
            await storeContext.SaveChangesAsync();

            return Ok("Created");
        }
        
        }
    }
