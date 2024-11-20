db = db.getSiblingDB('sparehub');

const agents = [
  { agentId: 1, agentName: "Atlantic Agents" },      // Amsterdam Warehouse
  { agentId: 2, agentName: "Pacific Brokers" },      // Houston Warehouse
  { agentId: 3, agentName: "Northern Logistics" },   // Incheon Warehouse
  { agentId: 4, agentName: "Southern Dispatch" },    // Osaka Warehouse
  { agentId: 5, agentName: "Eastern Freight" }       // Shanghai Warehouse
];

const warehouses = [
  { warehouseId: 1, warehouseName: "Amsterdam Warehouse", agentId: 1, agentName: agents[0].agentName },
  { warehouseId: 2, warehouseName: "Houston Warehouse", agentId: 2, agentName: agents[1].agentName },
  { warehouseId: 3, warehouseName: "Incheon Warehouse", agentId: 3, agentName: agents[2].agentName },
  { warehouseId: 4, warehouseName: "Osaka Warehouse", agentId: 4, agentName: agents[3].agentName },
  { warehouseId: 5, warehouseName: "Shanghai Warehouse", agentId: 5, agentName: agents[4].agentName }
];

// Function to generate a random date within a range
function randomDate(start, end) {
  return new Date(start.getTime() + Math.random() * (end.getTime() - start.getTime()));
}

// Insert 10 Order documents with Agent and Warehouse information
db.Order.insertMany([
  {
    _id: ObjectId(),
    OrderNumber: "ORD-2024-0001",
    SupplierOrderNumber: "SUPORD-0001",
    SupplierId: 1,
    SupplierName: "Ocean Supplies Ltd.",
    VesselId: 1,
    VesselName: "SS Marine Explorer",
    VesselOwnerId: 1,
    VesselOwnerName: "SeaTrans Shipping",
    WarehouseId: warehouses[0].warehouseId,
    WarehouseName: warehouses[0].warehouseName,
    AgentId: warehouses[0].agentId,
    AgentName: warehouses[0].agentName,
    ExpectedReadiness: ISODate("2024-04-15T10:00:00Z"),
    ActualReadiness: ISODate("2024-04-16T12:30:00Z"),
    ExpectedArrival: ISODate("2024-04-20T08:00:00Z"),
    ActualArrival: ISODate("2024-04-20T09:15:00Z"),
    OrderStatus: "Ready",
    Boxes: [
      {
        _id: ObjectId(),
        Length: 50,
        Width: 40,
        Height: 30,
        Weight: 25.5
      },
      {
        _id: ObjectId(),
        Length: 60,
        Width: 45,
        Height: 35,
        Weight: 30.2
      }
    ],
    Dispatches: []
  },
  {
    _id: ObjectId(),
    OrderNumber: "ORD-2024-0002",
    SupplierOrderNumber: null,
    SupplierId: 2,
    SupplierName: "Harbor Goods Inc.",
    VesselId: 2,
    VesselName: "MV Ocean Voyager",
    VesselOwnerId: 2,
    VesselOwnerName: "Global Vessels Co.",
    WarehouseId: warehouses[1].warehouseId,
    WarehouseName: warehouses[1].warehouseName,
    AgentId: warehouses[1].agentId,
    AgentName: warehouses[1].agentName,
    ExpectedReadiness: ISODate("2024-05-10T09:00:00Z"),
    ActualReadiness: null,
    ExpectedArrival: ISODate("2024-05-15T07:00:00Z"),
    ActualArrival: null,
    OrderStatus: "Pending",
    Boxes: [
      {
        _id: ObjectId(),
        Length: 55,
        Width: 42,
        Height: 32,
        Weight: 28.0
      }
    ],
    Dispatches: []
  },
  {
    _id: ObjectId(),
    OrderNumber: "ORD-2024-0003",
    SupplierOrderNumber: "SUPORD-0003",
    SupplierId: 3,
    SupplierName: "Dockside Distributors",
    VesselId: 3,
    VesselName: "HMS Sea King",
    VesselOwnerId: 3,
    VesselOwnerName: "Maritime Holdings",
    WarehouseId: warehouses[2].warehouseId,
    WarehouseName: warehouses[2].warehouseName,
    AgentId: warehouses[2].agentId,
    AgentName: warehouses[2].agentName,
    ExpectedReadiness: ISODate("2024-06-01T14:00:00Z"),
    ActualReadiness: ISODate("2024-06-02T16:45:00Z"),
    ExpectedArrival: ISODate("2024-06-07T11:00:00Z"),
    ActualArrival: ISODate("2024-06-07T10:50:00Z"),
    OrderStatus: "Inbound",
    Boxes: [
      {
        _id: ObjectId(),
        Length: 65,
        Width: 50,
        Height: 40,
        Weight: 35.7
      },
      {
        _id: ObjectId(),
        Length: 70,
        Width: 55,
        Height: 45,
        Weight: 40.3
      },
      {
        _id: ObjectId(),
        Length: 60,
        Width: 48,
        Height: 38,
        Weight: 32.8
      }
    ],
    Dispatches: []
  },
  {
    _id: ObjectId(),
    OrderNumber: "ORD-2024-0004",
    SupplierOrderNumber: "SUPORD-0004",
    SupplierId: 4,
    SupplierName: "Portside Essentials",
    VesselId: 4,
    VesselName: "FV Harbor Star",
    VesselOwnerId: 4,
    VesselOwnerName: "Oceanic Enterprises",
    WarehouseId: warehouses[3].warehouseId,
    WarehouseName: warehouses[3].warehouseName,
    AgentId: warehouses[3].agentId,
    AgentName: warehouses[3].agentName,
    ExpectedReadiness: ISODate("2024-07-20T13:00:00Z"),
    ActualReadiness: ISODate("2024-07-21T15:20:00Z"),
    ExpectedArrival: ISODate("2024-07-25T09:30:00Z"),
    ActualArrival: ISODate("2024-07-25T10:00:00Z"),
    OrderStatus: "Stock",
    Boxes: [
      {
        _id: ObjectId(),
        Length: 58,
        Width: 44,
        Height: 34,
        Weight: 29.5
      }
    ],
    Dispatches: []
  },
  {
    _id: ObjectId(),
    OrderNumber: "ORD-2024-0005",
    SupplierOrderNumber: null,
    SupplierId: 5,
    SupplierName: "Marina Merchants",
    VesselId: 5,
    VesselName: "Yacht Marina Dream",
    VesselOwnerId: 5,
    VesselOwnerName: "Voyage Masters",
    WarehouseId: warehouses[4].warehouseId,
    WarehouseName: warehouses[4].warehouseName,
    AgentId: warehouses[4].agentId,
    AgentName: warehouses[4].agentName,
    ExpectedReadiness: ISODate("2024-08-05T11:00:00Z"),
    ActualReadiness: null,
    ExpectedArrival: ISODate("2024-08-10T08:45:00Z"),
    ActualArrival: null,
    OrderStatus: "Cancelled",
    Boxes: [],
    Dispatches: []
  },
  {
    _id: ObjectId(),
    OrderNumber: "ORD-2024-0006",
    SupplierOrderNumber: "SUPORD-0006",
    SupplierId: 1,
    SupplierName: "Ocean Supplies Ltd.",
    VesselId: 2,
    VesselName: "MV Ocean Voyager",
    VesselOwnerId: 2,
    VesselOwnerName: "Global Vessels Co.",
    WarehouseId: warehouses[0].warehouseId,
    WarehouseName: warehouses[0].warehouseName,
    AgentId: warehouses[0].agentId,
    AgentName: warehouses[0].agentName,
    ExpectedReadiness: ISODate("2024-09-12T10:30:00Z"),
    ActualReadiness: ISODate("2024-09-13T12:00:00Z"),
    ExpectedArrival: ISODate("2024-09-18T07:30:00Z"),
    ActualArrival: ISODate("2024-09-18T08:15:00Z"),
    OrderStatus: "Ready",
    Boxes: [
      {
        _id: ObjectId(),
        Length: 62,
        Width: 46,
        Height: 36,
        Weight: 31.2
      },
      {
        _id: ObjectId(),
        Length: 68,
        Width: 52,
        Height: 42,
        Weight: 37.5
      }
    ],
    Dispatches: []
  },
  {
    _id: ObjectId(),
    OrderNumber: "ORD-2024-0007",
    SupplierOrderNumber: "SUPORD-0007",
    SupplierId: 2,
    SupplierName: "Harbor Goods Inc.",
    VesselId: 3,
    VesselName: "HMS Sea King",
    VesselOwnerId: 3,
    VesselOwnerName: "Maritime Holdings",
    WarehouseId: warehouses[1].warehouseId,
    WarehouseName: warehouses[1].warehouseName,
    AgentId: warehouses[1].agentId,
    AgentName: warehouses[1].agentName,
    ExpectedReadiness: ISODate("2024-10-01T09:45:00Z"),
    ActualReadiness: ISODate("2024-10-02T11:30:00Z"),
    ExpectedArrival: ISODate("2024-10-07T06:50:00Z"),
    ActualArrival: ISODate("2024-10-07T07:10:00Z"),
    OrderStatus: "Inbound",
    Boxes: [
      {
        _id: ObjectId(),
        Length: 54,
        Width: 40,
        Height: 30,
        Weight: 26.4
      },
      {
        _id: ObjectId(),
        Length: 59,
        Width: 43,
        Height: 33,
        Weight: 28.7
      }
    ],
    Dispatches: []
  },
  {
    _id: ObjectId(),
    OrderNumber: "ORD-2024-0008",
    SupplierOrderNumber: null,
    SupplierId: 3,
    SupplierName: "Dockside Distributors",
    VesselId: 4,
    VesselName: "FV Harbor Star",
    VesselOwnerId: 4,
    VesselOwnerName: "Oceanic Enterprises",
    WarehouseId: warehouses[2].warehouseId,
    WarehouseName: warehouses[2].warehouseName,
    AgentId: warehouses[2].agentId,
    AgentName: warehouses[2].agentName,
    ExpectedReadiness: ISODate("2024-11-15T14:20:00Z"),
    ActualReadiness: null,
    ExpectedArrival: ISODate("2024-11-20T10:10:00Z"),
    ActualArrival: null,
    OrderStatus: "Pending",
    Boxes: [
      {
        _id: ObjectId(),
        Length: 61,
        Width: 47,
        Height: 37,
        Weight: 33.0
      }
    ],
    Dispatches: []
  },
  {
    _id: ObjectId(),
    OrderNumber: "ORD-2024-0009",
    SupplierOrderNumber: "SUPORD-0009",
    SupplierId: 4,
    SupplierName: "Portside Essentials",
    VesselId: 5,
    VesselName: "Yacht Marina Dream",
    VesselOwnerId: 5,
    VesselOwnerName: "Voyage Masters",
    WarehouseId: warehouses[3].warehouseId,
    WarehouseName: warehouses[3].warehouseName,
    AgentId: warehouses[3].agentId,
    AgentName: warehouses[3].agentName,
    ExpectedReadiness: ISODate("2024-12-05T08:15:00Z"),
    ActualReadiness: ISODate("2024-12-06T09:45:00Z"),
    ExpectedArrival: ISODate("2024-12-11T05:30:00Z"),
    ActualArrival: ISODate("2024-12-11T06:00:00Z"),
    OrderStatus: "Stock",
    Boxes: [
      {
        _id: ObjectId(),
        Length: 57,
        Width: 41,
        Height: 31,
        Weight: 27.5
      },
      {
        _id: ObjectId(),
        Length: 63,
        Width: 49,
        Height: 39,
        Weight: 34.1
      }
    ],
    Dispatches: []
  },
  {
    _id: ObjectId(),
    OrderNumber: "ORD-2024-0010",
    SupplierOrderNumber: "SUPORD-0010",
    SupplierId: 5,
    SupplierName: "Marina Merchants",
    VesselId: 1,
    VesselName: "SS Marine Explorer",
    VesselOwnerId: 1,
    VesselOwnerName: "SeaTrans Shipping",
    WarehouseId: warehouses[4].warehouseId,
    WarehouseName: warehouses[4].warehouseName,
    AgentId: warehouses[4].agentId,
    AgentName: warehouses[4].agentName,
    ExpectedReadiness: ISODate("2025-01-10T12:00:00Z"),
    ActualReadiness: ISODate("2025-01-11T13:30:00Z"),
    ExpectedArrival: ISODate("2025-01-16T09:00:00Z"),
    ActualArrival: ISODate("2025-01-16T09:45:00Z"),
    OrderStatus: "Ready",
    Boxes: [
      {
        _id: ObjectId(),
        Length: 64,
        Width: 50,
        Height: 40,
        Weight: 36.5
      }
    ],
    Dispatches: []
  }
]);

// Verify the inserted Orders
print("Inserted 10 Order documents:");
db.Order.find().pretty();
