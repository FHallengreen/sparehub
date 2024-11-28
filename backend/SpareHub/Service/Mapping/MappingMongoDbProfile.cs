using AutoMapper;
using Domain.Models;
using Persistence.MongoDb;
using Shared.DTOs.Order;
using Shared.Order;

namespace Service.Mapping;

public class MappingMongoDbProfile : Profile
{
   public MappingMongoDbProfile()
    {
        // Map OrderCollection to Order
        CreateMap<OrderCollection, Order>()
            .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId.ToString()))
            .ForMember(dest => dest.VesselId, opt => opt.MapFrom(src => src.VesselId.ToString()))
            .ForMember(dest => dest.WarehouseId, opt => opt.MapFrom(src => src.WarehouseId.ToString()))
            .ReverseMap();

        // Map BoxCollection to Box
        CreateMap<BoxRequest, BoxCollection>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ReverseMap();

        CreateMap<BoxCollection, BoxResponse>()
            .ReverseMap();

        CreateMap<BoxCollection, Box>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.Length))
            .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            .ReverseMap();

        CreateMap<OrderCollection, Order>().ReverseMap();
    }
}