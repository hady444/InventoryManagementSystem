using DAL.Data;
using DAL.Repos.Abstraction;
using DAL.Repos.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public static class AddDAL
    {
        public static IServiceCollection AddDALPr(this IServiceCollection service)
        {
            service.AddDbContext<PrDBContext>(options=>options.UseInMemoryDatabase("InMemoryDb"));
            service.AddScoped<IWarehouseRepository, WarehouseRepository>();
            service.AddScoped<IProductRepository, ProductRepository>();
            return service;
        }
    }
}
