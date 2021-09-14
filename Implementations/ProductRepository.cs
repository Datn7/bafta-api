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

        public void Add<T>(T entity) where T : class
        {
            storeContext.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            storeContext.Remove(entity);    
        }

        public async Task<Photo> GetPhoto(int id)
        {
            //get photo from db with id
            var photo = await storeContext.Photos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }

        public async Task<Product> GetProduct(int id)
        {

            //get user and include photos also which is seperate class but included property
            var product = await storeContext.Products.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);

            //return user
            return product;
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

        public async Task<bool> SaveAll()
        {
            return await storeContext.SaveChangesAsync() > 0;
        }
    }
}
