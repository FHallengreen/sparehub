using AutoMapper;
using Domain.Models;
using Domain.MySql;
using Shared.Order;

namespace Service.Mapping;

public class MappingMySqlProfile : Profile
{
    public MappingMySqlProfile()
    {
        // Box mappings
        CreateMap<Box, BoxEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => int.Parse(src.OrderId)))
            .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.Length))
            .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight));

        CreateMap<BoxEntity, Box>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId.ToString()))
            .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.Length))
            .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight));

        // Order mappings
        CreateMap<Order, OrderEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => int.Parse(src.Id)))
            .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
            .ForMember(dest => dest.SupplierOrderNumber, opt => opt.MapFrom(src => src.SupplierOrderNumber))
            .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => int.Parse(src.SupplierId)))
            .ForMember(dest => dest.VesselId, opt => opt.MapFrom(src => int.Parse(src.VesselId)))
            .ForMember(dest => dest.WarehouseId, opt => opt.MapFrom(src => int.Parse(src.WarehouseId)))
            .ForMember(dest => dest.ExpectedReadiness, opt => opt.MapFrom(src => src.ExpectedReadiness))
            .ForMember(dest => dest.ActualReadiness, opt => opt.MapFrom(src => src.ActualReadiness))
            .ForMember(dest => dest.ExpectedArrival, opt => opt.MapFrom(src => src.ExpectedArrival))
            .ForMember(dest => dest.ActualArrival, opt => opt.MapFrom(src => src.ActualArrival))
            .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus))
            .ForMember(dest => dest.Boxes, opt => opt.Ignore())
            .ForMember(dest => dest.Supplier, opt => opt.Ignore())
            .ForMember(dest => dest.Vessel, opt => opt.Ignore())
            .ForMember(dest => dest.Warehouse, opt => opt.Ignore());

        CreateMap<OrderEntity, Order>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
            .ForMember(dest => dest.SupplierOrderNumber, opt => opt.MapFrom(src => src.SupplierOrderNumber))
            .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId.ToString()))
            .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.Supplier))
            .ForMember(dest => dest.VesselId, opt => opt.MapFrom(src => src.VesselId.ToString()))
            .ForMember(dest => dest.Vessel, opt => opt.MapFrom(src => src.Vessel))
            .ForMember(dest => dest.WarehouseId, opt => opt.MapFrom(src => src.WarehouseId.ToString()))
            .ForMember(dest => dest.Warehouse, opt => opt.MapFrom(src => src.Warehouse))
            .ForMember(dest => dest.ExpectedReadiness, opt => opt.MapFrom(src => src.ExpectedReadiness))
            .ForMember(dest => dest.ActualReadiness, opt => opt.MapFrom(src => src.ActualReadiness))
            .ForMember(dest => dest.ExpectedArrival, opt => opt.MapFrom(src => src.ExpectedArrival))
            .ForMember(dest => dest.ActualArrival, opt => opt.MapFrom(src => src.ActualArrival))
            .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus))
            .ForMember(dest => dest.Boxes, opt => opt.MapFrom(src => src.Boxes));

        // Supplier mappings
        CreateMap<SupplierEntity, Domain.Models.Supplier>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<BoxRequest, Box>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid().ToString()));
        
        // Vessel mappings
        CreateMap<VesselEntity, Vessel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ImoNumber, opt => opt.MapFrom(src => src.ImoNumber))
            .ForMember(dest => dest.Flag, opt => opt.MapFrom(src => src.Flag))
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner));

        // Warehouse mappings
        CreateMap<WarehouseEntity, Domain.Models.Warehouse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Agent, opt => opt.MapFrom(src => src.Agent));

        // Owner mappings
        CreateMap<OwnerEntity, Owner>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        // Agent mappings
        CreateMap<AgentEntity, Domain.Models.Agent>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        
        // Dispatch mappings
        CreateMap<DispatchEntity, Dispatch>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.OriginId, opt => opt.MapFrom(src => src.OriginId.ToString()))
            .ForMember(dest => dest.DestinationType, opt => opt.MapFrom(src => src.DestinationType))
            .ForMember(dest => dest.DestinationId, opt => opt.MapFrom(src => src.DestinationId.ToString()))
            .ForMember(dest => dest.DispatchStatus, opt => opt.MapFrom(src => src.DispatchStatus))
            .ForMember(dest => dest.TransportModeType, opt => opt.MapFrom(src => src.TransportModeType))
            .ForMember(dest => dest.TrackingNumber, opt => opt.MapFrom(src => src.TrackingNumber))
            .ForMember(dest => dest.DispatchDate, opt => opt.MapFrom(src => src.DispatchDate))
            .ForMember(dest => dest.DeliveryDate, opt => opt.MapFrom(src => src.DeliveryDate))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.userEntity));
    }
}
