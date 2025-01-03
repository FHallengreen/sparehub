using AutoMapper;
using Domain.Models;
using Persistence.MySql;
using Persistence.MySql.SparehubDbContext;
using Shared.DTOs.Order;
using Shared.DTOs.User;
using Shared.DTOs.Warehouse;

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

        CreateMap<OrderRequest, Order>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
            .ForMember(dest => dest.SupplierOrderNumber, opt => opt.MapFrom(src => src.SupplierOrderNumber))
            .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId))
            .ForMember(dest => dest.VesselId, opt => opt.MapFrom(src => src.VesselId))
            .ForMember(dest => dest.WarehouseId, opt => opt.MapFrom(src => src.WarehouseId))
            .ForMember(dest => dest.ExpectedReadiness, opt => opt.MapFrom(src => src.ExpectedReadiness))
            .ForMember(dest => dest.ActualReadiness, opt => opt.MapFrom(src => src.ActualReadiness))
            .ForMember(dest => dest.ExpectedArrival, opt => opt.MapFrom(src => src.ExpectedArrival))
            .ForMember(dest => dest.ActualArrival, opt => opt.MapFrom(src => src.ActualArrival))
            .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus))
            .ForMember(dest => dest.TrackingNumber, opt => opt.MapFrom(src => src.TrackingNumber))
            .ForMember(dest => dest.Transporter, opt => opt.MapFrom(src =>
                string.IsNullOrWhiteSpace(src.Transporter) ? null : src.Transporter))
            .ForMember(dest => dest.Boxes, opt => opt.MapFrom(src => src.Boxes));

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
            .ForMember(dest => dest.TrackingNumber, opt => opt.MapFrom(src => src.TrackingNumber))
            .ForMember(dest => dest.Transporter, opt => opt.MapFrom(src => src.Transporter))
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
            .ForMember(dest => dest.TrackingNumber, opt => opt.MapFrom(src => src.TrackingNumber))
            .ForMember(dest => dest.Transporter, opt => opt.MapFrom(src => src.Transporter))
            .ForMember(dest => dest.Boxes, opt => opt.MapFrom(src => src.Boxes));

        CreateMap<Supplier, SupplierEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => int.Parse(src.Id))) // Assuming Supplier.Id is a string
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<SupplierEntity, Supplier>()
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

        CreateMap<Vessel, VesselEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => int.Parse(src.Id)))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ImoNumber, opt => opt.MapFrom(src => src.ImoNumber))
            .ForMember(dest => dest.Flag, opt => opt.MapFrom(src => src.Flag))
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => int.Parse(src.Owner.Id)))
            .ForMember(dest => dest.Owner, opt => opt.Ignore());

        //Owner mappings
        CreateMap<OwnerEntity, Owner>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<Owner, OwnerEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => int.Parse(src.Id)))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        // Port mappings
        CreateMap<PortEntity, Port>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<Port, PortEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => int.Parse(src.Id)))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        // VesselAtPort mappings
        CreateMap<VesselAtPortEntity, VesselAtPort>()
            .ForMember(dest => dest.VesselId, opt => opt.MapFrom(src => src.VesselId.ToString()))
            .ForMember(dest => dest.PortId, opt => opt.MapFrom(src => src.PortId.ToString()))
            .ForMember(dest => dest.ArrivalDate, opt => opt.MapFrom(src => src.ArrivalDate))
            .ForMember(dest => dest.DepartureDate, opt => opt.MapFrom(src => src.DepartureDate));

        CreateMap<VesselAtPort, VesselAtPortEntity>()
            .ForMember(dest => dest.VesselId, opt => opt.MapFrom(src => int.Parse(src.VesselId)))
            .ForMember(dest => dest.PortId, opt => opt.MapFrom(src => int.Parse(src.PortId)))
            .ForMember(dest => dest.VesselEntity, opt => opt.Ignore())
            .ForMember(dest => dest.PortEntity, opt => opt.Ignore());

        // Warehouse mappings
        CreateMap<WarehouseEntity, Warehouse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Agent, opt => opt.MapFrom(src => src.Agent));

        // Agent mappings
        CreateMap<Agent, AgentEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Id) ? 0 : int.Parse(src.Id)))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<AgentEntity, Agent>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        // Dispatch mappings
        CreateMap<Dispatch, DispatchEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DestinationType, opt => opt.MapFrom(src => src.DestinationType))
            .ForMember(dest => dest.DestinationId, opt => opt.MapFrom(src => src.DestinationId))
            .ForMember(dest => dest.DispatchStatus, opt => opt.MapFrom(src => src.DispatchStatus))
            .ForMember(dest => dest.TransportModeType, opt => opt.MapFrom(src => src.TransportModeType))
            .ForMember(dest => dest.TrackingNumber, opt => opt.MapFrom(src => src.TrackingNumber))
            .ForMember(dest => dest.DispatchDate, opt => opt.MapFrom(src => src.DispatchDate))
            .ForMember(dest => dest.DeliveryDate, opt => opt.MapFrom(src => src.DeliveryDate))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.Orders))
            .ForMember(dest => dest.Invoices, opt => opt.Ignore()); // Removed OriginType and OriginId

        CreateMap<DispatchEntity, Dispatch>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.DestinationType, opt => opt.MapFrom(src => src.DestinationType))
            .ForMember(dest => dest.DestinationId, opt => opt.MapFrom(src => src.DestinationId))
            .ForMember(dest => dest.DispatchStatus, opt => opt.MapFrom(src => src.DispatchStatus))
            .ForMember(dest => dest.TransportModeType, opt => opt.MapFrom(src => src.TransportModeType))
            .ForMember(dest => dest.TrackingNumber, opt => opt.MapFrom(src => src.TrackingNumber))
            .ForMember(dest => dest.DispatchDate, opt => opt.MapFrom(src => src.DispatchDate))
            .ForMember(dest => dest.DeliveryDate, opt => opt.MapFrom(src => src.DeliveryDate))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.Orders)); // Removed OriginType and OriginId
        
        // User mappings
        CreateMap<UserEntity, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Title))
            .ForMember(dest => dest.Operator, opt => opt.MapFrom(src => src.Operator))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));


        CreateMap<OperatorEntity, Operator>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));

        CreateMap<UserEntity, UserResponse>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Title));

        // Address mappings
        CreateMap<AddressEntity, Address>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.AddressLine, opt => opt.MapFrom(src => src.AddressLine))
            .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country));

        CreateMap<Address, AddressEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => int.Parse(src.Id)))
            .ForMember(dest => dest.AddressLine, opt => opt.MapFrom(src => src.AddressLine))
            .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country));

        // Warehouse mappings
        CreateMap<Warehouse, WarehouseEntity>()
            .ForMember(dest => dest.Agent,
                opt => opt.MapFrom(src => src.Agent)) // Leverages Agent -> AgentEntity mapping
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // Ignore DB-generated Id

        CreateMap<WarehouseRequest, Warehouse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.AgentId, opt => opt.MapFrom(src => src.AgentId))
            .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.AddressId));
        CreateMap<WarehouseEntity, Warehouse>()
            .ForMember(dest => dest.Agent,
                opt => opt.MapFrom(src => src.Agent)) // Leverages AgentEntity -> Agent mapping
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
    }
}