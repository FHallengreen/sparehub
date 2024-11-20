using AutoMapper;
using Domain.Models;
using Persistence.MongoDb;

namespace Service.Mapping;

public class MappingMongoDbProfile : Profile
{
    public MappingMongoDbProfile()
    {
        // Map OrderCollection to Order
        CreateMap<OrderCollection, Order>()
            .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => new Domain.Models.Supplier
            {
                Id = src.SupplierId.ToString(),
                Name = src.SupplierName
            }))
            .ForMember(dest => dest.Vessel, opt => opt.MapFrom(src => new Vessel
            {
                Id = src.VesselId.ToString(),
                Name = src.VesselName,
                Owner = new Owner
                {
                    Id = src.VesselOwnerId.ToString(),
                    Name = src.VesselOwnerName
                }
            }))


            .ForMember(dest => dest.Warehouse, opt => opt.MapFrom(src => new Domain.Models.Warehouse
            {
                Id = src.WarehouseId.ToString(),
                Name = src.WarehouseName,
                Agent = new Domain.Models.Agent
                {
                    Id = src.AgentId.ToString(),
                    Name = src.AgentName
                }
            }))

            .ForMember(dest => dest.Boxes, opt => opt.MapFrom(src => src.Boxes))
            .ReverseMap()
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name))
            .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => int.Parse(src.Supplier.Id)))
            .ForMember(dest => dest.VesselName, opt => opt.MapFrom(src => src.Vessel.Name))
            .ForMember(dest => dest.VesselId, opt => opt.MapFrom(src => int.Parse(src.Vessel.Id)))
            .ForMember(dest => dest.VesselOwnerName, opt => opt.MapFrom(src => src.Vessel.Owner.Name))
            .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse.Name))
            .ForMember(dest => dest.WarehouseId, opt => opt.MapFrom(src => int.Parse(src.Warehouse.Id)))
            .ForMember(dest => dest.Boxes, opt => opt.MapFrom(src => src.Boxes));

        // Add Mapping for BoxCollection to Box
        CreateMap<BoxCollection, Box>()
            .ReverseMap();
    }
}