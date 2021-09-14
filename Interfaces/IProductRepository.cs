using bafta_api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bafta_api.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IReadOnlyList<Product>> GetProductsAsync();
        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();


        //add entity
        void Add<T>(T entity) where T : class;

        //delete entity
        void Delete<T>(T entity) where T : class;
        //see if changes saved
        Task<bool> SaveAll();

        //get single product
        Task<Product> GetProduct(int id);

        //get single photo
        Task<Photo> GetPhoto(int id);


    }
}
