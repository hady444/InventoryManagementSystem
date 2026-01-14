using BLL.Mapper;
using BLL.Services.Abstraction;
using BLL.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public static class AddBLL
    {
        public static IServiceCollection AddBLLPr(this IServiceCollection service)
        {
            service.AddScoped<IWarehouseService, WarehouseService>();
            service.AddScoped<IProductService, ProductService>();
            service.AddScoped<IStockTransactionSerivce, StockTransactionSerivce>();
            service.AddAutoMapper(a => a.AddProfile(new DomainProfile()));
            return service;
        }
    }
}
