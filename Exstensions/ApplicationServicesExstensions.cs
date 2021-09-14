using bafta_api.Errors;
using bafta_api.Helpers;
using bafta_api.Implementations;
using bafta_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bafta_api.Exstensions
{
    public static class ApplicationServicesExstensions 
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            

            //add product service
            services.AddScoped<IProductRepository, ProductRepository>();

            //add generic service
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            //api validation error response config
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            //add photo service
            services.AddScoped<IPhotoService, PhotoService>();

            return services;
        }
    }
}
