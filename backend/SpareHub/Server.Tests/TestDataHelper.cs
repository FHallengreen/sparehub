using System;
using System.Collections.Generic;
using Domain.Models;
using Persistence.MySql;

namespace Server.Tests;

public static class TestDataHelper
{
    public static OrderEntity CreateOrderEntity(
        int id = 1,
        string orderNumber = "ORD001",
        string supplierOrderNumber = "SUP001",
        int supplierId = 1,
        int vesselId = 1,
        int warehouseId = 1,
        DateTime? expectedReadiness = null,
        DateTime? actualReadiness = null,
        DateTime? expectedArrival = null,
        DateTime? actualArrival = null,
        OrderStatus orderStatus = OrderStatus.Pending,
        string trackingNumber = "TRACK001",
        string transporter = "DHL",
        SupplierEntity supplier = null,
        VesselEntity vessel = null,
        WarehouseEntity warehouse = null,
        List<BoxEntity> boxes = null
    )
    {
        return new OrderEntity
        {
            Id = id,
            OrderNumber = orderNumber,
            SupplierOrderNumber = supplierOrderNumber,
            SupplierId = supplierId,
            VesselId = vesselId,
            WarehouseId = warehouseId,
            ExpectedReadiness = expectedReadiness ?? DateTime.UtcNow.AddDays(5),
            ActualReadiness = actualReadiness,
            ExpectedArrival = expectedArrival ?? DateTime.UtcNow.AddDays(10),
            ActualArrival = actualArrival,
            OrderStatus = orderStatus.ToString(),
            TrackingNumber = trackingNumber,
            Transporter = transporter,
            Supplier = supplier ?? new SupplierEntity
            {
                Id = supplierId,
                Name = "Global Supplies Inc.",
                AddressEntity = new AddressEntity
                {
                    Id = 1,
                    AddressLine = "123 Main Street",
                    PostalCode = "10001",
                    Country = "USA"
                }
            },
            Vessel = vessel ?? new VesselEntity
            {
                Id = vesselId,
                Name = "Voyager",
                ImoNumber = "1234567",
                Flag = "USA",
                Owner = new OwnerEntity
                {
                    Id = 1,
                    Name = "Maersk"
                }
            },
            Warehouse = new WarehouseEntity
            {
                Id = warehouse?.Id ?? warehouseId,
                Name = warehouse?.Name ?? "Amsterdam Warehouse",
                Agent = warehouse?.Agent ?? new AgentEntity
                {
                    Id = 1,
                    Name = "Fast Logistics"
                },
                Address = warehouse?.Address ?? new AddressEntity
                {
                    Id = 1,
                    AddressLine = "123 Main Street",
                    PostalCode = "10001",
                    Country = "USA"
                }
            },
            Boxes = boxes ??
            [
                new BoxEntity
                {
                    Id = Guid.NewGuid(),
                    Length = 30,
                    Width = 20,
                    Height = 15,
                    Weight = 5.5
                }
            ]
        };
    }
}