using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DAL
{
    public static class AddDAL
    {
        public static IServiceCollection AddDALPr(this IServiceCollection service)
        {
            service.AddDbContext<PrDBContext>(options=>options.UseInMemoryDatabase("InMemoryDb"));
            return service;
        }
    }
}
