using AutoMapper;
using Domain.Models;
using Persistence.MongoDb;
using Persistence.MongoDb.Models;
using Shared.DTOs.Order;

namespace Service.Mapping
{
    public class MappingMongoDbProfile : Profile
    {
        public MappingMongoDbProfile()
        {
            // Map OrderCollection to Order
            CreateMap<OrderCollection, Order>()
                .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.Supplier))
                .ForMember(dest => dest.Vessel, opt => opt.MapFrom(src => src.Vessel))
                .ForPath(dest => dest.Vessel.Owner, opt => opt.MapFrom(src => src.Vessel.Owner)) // Safe mapping
                .ForMember(dest => dest.Warehouse, opt => opt.MapFrom(src => src.Warehouse))
                .ForPath(dest => dest.Warehouse.Agent, opt => opt.MapFrom(src => src.Agent))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus))
                .ForMember(dest => dest.Boxes, opt => opt.Ignore()) // Boxes handled separately
                .ReverseMap();

            // Map MongoSupplier to Supplier
            CreateMap<MongoSupplier, Supplier>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SupplierId))
                .ReverseMap();

            // Map MongoVessel to Vessel
            CreateMap<MongoVessel, Vessel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.VesselId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ImoNumber, opt => opt.MapFrom(src => src.ImoNumber))
                .ForMember(dest => dest.Flag, opt => opt.MapFrom(src => src.Flag))
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner))
                .ReverseMap();

            // Map MongoOwner to Owner
            CreateMap<MongoOwner, Owner>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OwnerId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();

            // Map MongoWarehouse to Warehouse
            CreateMap<MongoWarehouse, Warehouse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.WarehouseId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Agent, opt => opt.MapFrom(src => src.Agent))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ReverseMap();

            // Map MongoAgent to Agent
            CreateMap<MongoAgent, Agent>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AgentId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();

            // Map MongoAddress to Address
            CreateMap<MongoAddress, Address>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AddressId))
                .ForMember(dest => dest.AddressLine, opt => opt.MapFrom(src => src.AddressLine))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ReverseMap();

            // Map BoxRequest to BoxCollection
            CreateMap<BoxRequest, BoxCollection>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            // Map BoxCollection to BoxResponse
            CreateMap<BoxCollection, BoxResponse>()
                .ReverseMap();

            // Map BoxCollection to Box (the model used in domain layer)
            CreateMap<BoxCollection, Box>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.Length))
                .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId)) // Link the Box to an Order
                .ReverseMap();

            // Additional mapping for BoxCollection -> Box if needed in other contexts
            CreateMap<BoxCollection, Box>().ReverseMap();
        }
    }
}
