using bafta_api.Data;
using bafta_api.Entities;
using bafta_api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bafta_api.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext storeContext;

        public ProductRepository(StoreContext storeContext)
        {
            this.storeContext = storeContext;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await storeContext.Products.Include(p=>p.ProductType).FirstOrDefaultAsync(p=>p.Id==id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await storeContext.Products.Include(p=>p.ProductType).ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await storeContext.ProductTypes.ToListAsync();
        }
    }
}
