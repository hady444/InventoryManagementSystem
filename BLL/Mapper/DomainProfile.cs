using AutoMapper;
using BLL.ViewModel;
using DAL.Models;

namespace BLL.Mapper
{
    public class DomainProfile: Profile
    {
        public DomainProfile()
        {
            CreateMap<Warehouse, CreateWarehouseVM>().ReverseMap();
            CreateMap<Product, CreateProductVM>().ReverseMap();
            CreateMap<StockTransaction, CreateTransactionVM>().ReverseMap();
        }
    }
}
