// Switch to the 'sparehub' database
db = db.getSiblingDB('sparehub');

// Optional: Drop existing collections to start fresh
db.agents.drop();
db.warehouses.drop();
db.users.drop();
db.suppliers.drop();
db.vessels.drop();
db.orders.drop();
db.dispatches.drop();
db.boxes.drop();

// 1. Define and Insert Agents
const agents = [
  { _id: ObjectId(), agentId: 1, agentName: "Atlantic Agents" },      // Amsterdam Warehouse
  { _id: ObjectId(), agentId: 2, agentName: "Pacific Brokers" },      // Houston Warehouse
  { _id: ObjectId(), agentId: 3, agentName: "Northern Logistics" },   // Incheon Warehouse
  { _id: ObjectId(), agentId: 4, agentName: "Southern Dispatch" },    // Osaka Warehouse
  { _id: ObjectId(), agentId: 5, agentName: "Eastern Freight" }       // Shanghai Warehouse
];

// Insert Agents
db.agents.insertMany(agents);

// 2. Define and Insert Warehouses with References to Agents
const warehouses = [
  { _id: ObjectId(), warehouseId: 1, warehouseName: "Amsterdam Warehouse", agentId: 1 },
  { _id: ObjectId(), warehouseId: 2, warehouseName: "Houston Warehouse", agentId: 2 },
  { _id: ObjectId(), warehouseId: 3, warehouseName: "Incheon Warehouse", agentId: 3 },
  { _id: ObjectId(), warehouseId: 4, warehouseName: "Osaka Warehouse", agentId: 4 },
  { _id: ObjectId(), warehouseId: 5, warehouseName: "Shanghai Warehouse", agentId: 5 }
];

// Insert Warehouses
db.warehouses.insertMany(warehouses);

// 3. Define and Insert Users
const users = [
  { _id: ObjectId(), userId: 1, name: "Alice Johnson" },
  { _id: ObjectId(), userId: 2, name: "Bob Smith" },
  { _id: ObjectId(), userId: 3, name: "Charlie Davis" },
  { _id: ObjectId(), userId: 4, name: "Diana Prince" },
  { _id: ObjectId(), userId: 5, name: "Ethan Hunt" }
];

// Insert Users
db.users.insertMany(users);

// 4. Define and Insert Suppliers
const suppliers = [
  { _id: ObjectId(), supplierId: 1, supplierName: "Ocean Supplies Ltd." },
  { _id: ObjectId(), supplierId: 2, supplierName: "Harbor Goods Inc." },
  { _id: ObjectId(), supplierId: 3, supplierName: "Dockside Distributors" },
  { _id: ObjectId(), supplierId: 4, supplierName: "Portside Essentials" },
  { _id: ObjectId(), supplierId: 5, supplierName: "Marina Merchants" }
];

// Insert Suppliers
db.suppliers.insertMany(suppliers);

// 5. Define and Insert Vessels
const vessels = [
  { _id: ObjectId(), vesselId: 1, vesselName: "SS Marine Explorer", ownerId: 1, ownerName: "SeaTrans Shipping" },
  { _id: ObjectId(), vesselId: 2, vesselName: "MV Ocean Voyager", ownerId: 2, ownerName: "Global Vessels Co." },
  { _id: ObjectId(), vesselId: 3, vesselName: "HMS Sea King", ownerId: 3, ownerName: "Maritime Holdings" },
  { _id: ObjectId(), vesselId: 4, vesselName: "FV Harbor Star", ownerId: 4, ownerName: "Oceanic Enterprises" },
  { _id: ObjectId(), vesselId: 5, vesselName: "Yacht Marina Dream", ownerId: 5, ownerName: "Voyage Masters" }
];

// Insert Vessels
db.vessels.insertMany(vessels);

// 6. Define Enums and Utility Functions
const orderStatuses = ["Pending", "Ready", "Inbound", "Stock", "Cancelled"];
const transportModes = ["Air", "Sea", "Land"];
const dispatchStatuses = ["Dispatched", "In Transit", "Delivered", "Failed"];

// Function to generate a random date between start and end
function randomDate(start, end) {
  return new Date(start.getTime() + Math.random() * (end.getTime() - start.getTime()));
}

// Function to generate a random integer between min and max (inclusive)
function randomInt(min, max) {
  return Math.floor(Math.random() * (max - min + 1)) + min;
}

// 7. Define and Insert Orders
const orders = [
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
    WarehouseId: 1,
    WarehouseName: "Amsterdam Warehouse",
    AgentId: 1,
    AgentName: "Atlantic Agents",
    ExpectedReadiness: ISODate("2024-04-15T10:00:00Z"),
    ActualReadiness: ISODate("2024-04-16T12:30:00Z"),
    ExpectedArrival: ISODate("2024-04-20T08:00:00Z"),
    ActualArrival: ISODate("2024-04-20T09:15:00Z"),
    OrderStatus: "Ready"
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
    WarehouseId: 2,
    WarehouseName: "Houston Warehouse",
    AgentId: 2,
    AgentName: "Pacific Brokers",
    ExpectedReadiness: ISODate("2024-05-10T09:00:00Z"),
    ActualReadiness: null,
    ExpectedArrival: ISODate("2024-05-15T07:00:00Z"),
    ActualArrival: null,
    OrderStatus: "Pending"
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
    WarehouseId: 3,
    WarehouseName: "Incheon Warehouse",
    AgentId: 3,
    AgentName: "Northern Logistics",
    ExpectedReadiness: ISODate("2024-06-01T14:00:00Z"),
    ActualReadiness: ISODate("2024-06-02T16:45:00Z"),
    ExpectedArrival: ISODate("2024-06-07T11:00:00Z"),
    ActualArrival: ISODate("2024-06-07T10:50:00Z"),
    OrderStatus: "Inbound"
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
    WarehouseId: 4,
    WarehouseName: "Osaka Warehouse",
    AgentId: 4,
    AgentName: "Southern Dispatch",
    ExpectedReadiness: ISODate("2024-07-20T13:00:00Z"),
    ActualReadiness: ISODate("2024-07-21T15:20:00Z"),
    ExpectedArrival: ISODate("2024-07-25T09:30:00Z"),
    ActualArrival: ISODate("2024-07-25T10:00:00Z"),
    OrderStatus: "Stock"
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
    WarehouseId: 5,
    WarehouseName: "Shanghai Warehouse",
    AgentId: 5,
    AgentName: "Eastern Freight",
    ExpectedReadiness: ISODate("2024-08-05T11:00:00Z"),
    ActualReadiness: null,
    ExpectedArrival: ISODate("2024-08-10T08:45:00Z"),
    ActualArrival: null,
    OrderStatus: "Cancelled"
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
    WarehouseId: 1,
    WarehouseName: "Amsterdam Warehouse",
    AgentId: 1,
    AgentName: "Atlantic Agents",
    ExpectedReadiness: ISODate("2024-09-12T10:30:00Z"),
    ActualReadiness: ISODate("2024-09-13T12:00:00Z"),
    ExpectedArrival: ISODate("2024-09-18T07:30:00Z"),
    ActualArrival: ISODate("2024-09-18T08:15:00Z"),
    OrderStatus: "Ready"
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
    WarehouseId: 2,
    WarehouseName: "Houston Warehouse",
    AgentId: 2,
    AgentName: "Pacific Brokers",
    ExpectedReadiness: ISODate("2024-10-01T09:45:00Z"),
    ActualReadiness: ISODate("2024-10-02T11:30:00Z"),
    ExpectedArrival: ISODate("2024-10-07T06:50:00Z"),
    ActualArrival: ISODate("2024-10-07T07:10:00Z"),
    OrderStatus: "Inbound"
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
    WarehouseId: 3,
    WarehouseName: "Incheon Warehouse",
    AgentId: 3,
    AgentName: "Northern Logistics",
    ExpectedReadiness: ISODate("2024-11-15T14:20:00Z"),
    ActualReadiness: null,
    ExpectedArrival: ISODate("2024-11-20T10:10:00Z"),
    ActualArrival: null,
    OrderStatus: "Pending"
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
    WarehouseId: 4,
    WarehouseName: "Osaka Warehouse",
    AgentId: 4,
    AgentName: "Southern Dispatch",
    ExpectedReadiness: ISODate("2024-12-05T08:15:00Z"),
    ActualReadiness: ISODate("2024-12-06T09:45:00Z"),
    ExpectedArrival: ISODate("2024-12-11T05:30:00Z"),
    ActualArrival: ISODate("2024-12-11T06:00:00Z"),
    OrderStatus: "Stock"
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
    WarehouseId: 5,
    WarehouseName: "Shanghai Warehouse",
    AgentId: 5,
    AgentName: "Eastern Freight",
    ExpectedReadiness: ISODate("2025-01-10T12:00:00Z"),
    ActualReadiness: ISODate("2025-01-11T13:30:00Z"),
    ExpectedArrival: ISODate("2025-01-16T09:00:00Z"),
    ActualArrival: ISODate("2025-01-16T09:45:00Z"),
    OrderStatus: "Ready"
  }
];

// Insert Orders
db.orders.insertMany(orders);

// 8. Insert Dispatches and Link to Orders
const dispatches = [];

// Define some sample dispatches
const sampleDispatches = [
  {
    OriginType: "Warehouse",
    OriginId: 1,
    DestinationType: "Vessel",
    DestinationId: 1,
    DispatchStatus: "Dispatched",
    TransportModeType: "Sea",
    TrackingNumber: "TRK-ORD-2024-0001-1",
    DispatchDate: ISODate("2024-04-16T13:00:00Z"),
    DeliveryDate: ISODate("2024-04-20T10:00:00Z"),
    UserId: 1, // Alice Johnson
    User: users.find(u => u.userId === 1)._id.toString(),
    Invoices: [
      {
        _id: ObjectId(),
        InvoiceNumber: "INV-ORD-2024-0001-1-1",
        Amount: 1500.00,
        Currency: "USD"
      }
    ],
    Orders: [orders[0]._id.toString()]
  },
  {
    OriginType: "Warehouse",
    OriginId: 2,
    DestinationType: "Vessel",
    DestinationId: 2,
    DispatchStatus: "In Transit",
    TransportModeType: "Air",
    TrackingNumber: "TRK-ORD-2024-0002-1",
    DispatchDate: ISODate("2024-05-11T10:00:00Z"),
    DeliveryDate: ISODate("2024-05-15T09:00:00Z"),
    UserId: 2, // Bob Smith
    User: users.find(u => u.userId === 2)._id.toString(),
    Invoices: [
      {
        _id: ObjectId(),
        InvoiceNumber: "INV-ORD-2024-0002-1-1",
        Amount: 2000.00,
        Currency: "EUR"
      }
    ],
    Orders: [orders[1]._id.toString()]
  },
  {
    OriginType: "Warehouse",
    OriginId: 3,
    DestinationType: "Vessel",
    DestinationId: 3,
    DispatchStatus: "Delivered",
    TransportModeType: "Land",
    TrackingNumber: "TRK-ORD-2024-0003-1",
    DispatchDate: ISODate("2024-06-03T15:00:00Z"),
    DeliveryDate: ISODate("2024-06-07T12:00:00Z"),
    UserId: 3, // Charlie Davis
    User: users.find(u => u.userId === 3)._id.toString(),
    Invoices: [
      {
        _id: ObjectId(),
        InvoiceNumber: "INV-ORD-2024-0003-1-1",
        Amount: 2500.00,
        Currency: "USD"
      },
      {
        _id: ObjectId(),
        InvoiceNumber: "INV-ORD-2024-0003-1-2",
        Amount: 1800.00,
        Currency: "EUR"
      }
    ],
    Orders: [orders[2]._id.toString()]
  },
  {
    OriginType: "Warehouse",
    OriginId: 4,
    DestinationType: "Vessel",
    DestinationId: 4,
    DispatchStatus: "Failed",
    TransportModeType: "Sea",
    TrackingNumber: "TRK-ORD-2024-0004-1",
    DispatchDate: ISODate("2024-07-21T16:00:00Z"),
    DeliveryDate: null, // Delivery failed
    UserId: 4, // Diana Prince
    User: users.find(u => u.userId === 4)._id.toString(),
    Invoices: [
      {
        _id: ObjectId(),
        InvoiceNumber: "INV-ORD-2024-0004-1-1",
        Amount: 1200.00,
        Currency: "USD"
      }
    ],
    Orders: [orders[3]._id.toString()]
  },
  {
    OriginType: "Warehouse",
    OriginId: 5,
    DestinationType: "Vessel",
    DestinationId: 5,
    DispatchStatus: "Dispatched",
    TransportModeType: "Air",
    TrackingNumber: "TRK-ORD-2024-0006-1",
    DispatchDate: ISODate("2024-09-14T14:00:00Z"),
    DeliveryDate: ISODate("2024-09-18T11:00:00Z"),
    UserId: 5, // Ethan Hunt
    User: users.find(u => u.userId === 5)._id.toString(),
    Invoices: [
      {
        _id: ObjectId(),
        InvoiceNumber: "INV-ORD-2024-0006-1-1",
        Amount: 2200.00,
        Currency: "EUR"
      }
    ],
    Orders: [orders[5]._id.toString()]
  },
  // Add more dispatches as needed
];

// Insert Dispatches
db.dispatches.insertMany(sampleDispatches);

// 9. Insert Boxes Linked to Orders
const boxes = [];

// Iterate through each order to create boxes based on your initial script
orders.forEach(order => {
  switch (order.OrderNumber) {
    case "ORD-2024-0001":
      boxes.push(
        {
          _id: ObjectId(),
          Length: 50,
          Width: 40,
          Height: 30,
          Weight: 25.5,
          OrderId: order._id.toString()
        },
        {
          _id: ObjectId(),
          Length: 60,
          Width: 45,
          Height: 35,
          Weight: 30.2,
          OrderId: order._id.toString()
        }
      );
      break;
    case "ORD-2024-0002":
      boxes.push(
        {
          _id: ObjectId(),
          Length: 55,
          Width: 42,
          Height: 32,
          Weight: 28.0,
          OrderId: order._id.toString()
        }
      );
      break;
    case "ORD-2024-0003":
      boxes.push(
        {
          _id: ObjectId(),
          Length: 65,
          Width: 50,
          Height: 40,
          Weight: 35.7,
          OrderId: order._id.toString()
        },
        {
          _id: ObjectId(),
          Length: 70,
          Width: 55,
          Height: 45,
          Weight: 40.3,
          OrderId: order._id.toString()
        },
        {
          _id: ObjectId(),
          Length: 60,
          Width: 48,
          Height: 38,
          Weight: 32.8,
          OrderId: order._id.toString()
        }
      );
      break;
    case "ORD-2024-0004":
      boxes.push(
        {
          _id: ObjectId(),
          Length: 58,
          Width: 44,
          Height: 34,
          Weight: 29.5,
          OrderId: order._id.toString()
        }
      );
      break;
    case "ORD-2024-0005":
      // No boxes for this order
      break;
    case "ORD-2024-0006":
      boxes.push(
        {
          _id: ObjectId(),
          Length: 62,
          Width: 46,
          Height: 36,
          Weight: 31.2,
          OrderId: order._id.toString()
        },
        {
          _id: ObjectId(),
          Length: 68,
          Width: 52,
          Height: 42,
          Weight: 37.5,
          OrderId: order._id.toString()
        }
      );
      break;
    case "ORD-2024-0007":
      boxes.push(
        {
          _id: ObjectId(),
          Length: 54,
          Width: 40,
          Height: 30,
          Weight: 26.4,
          OrderId: order._id.toString()
        },
        {
          _id: ObjectId(),
          Length: 59,
          Width: 43,
          Height: 33,
          Weight: 28.7,
          OrderId: order._id.toString()
        }
      );
      break;
    case "ORD-2024-0008":
      boxes.push(
        {
          _id: ObjectId(),
          Length: 61,
          Width: 47,
          Height: 37,
          Weight: 33.0,
          OrderId: order._id.toString()
        }
      );
      break;
    case "ORD-2024-0009":
      boxes.push(
        {
          _id: ObjectId(),
          Length: 57,
          Width: 41,
          Height: 31,
          Weight: 27.5,
          OrderId: order._id.toString()
        },
        {
          _id: ObjectId(),
          Length: 63,
          Width: 49,
          Height: 39,
          Weight: 34.1,
          OrderId: order._id.toString()
        }
      );
      break;
    case "ORD-2024-0010":
      boxes.push(
        {
          _id: ObjectId(),
          Length: 64,
          Width: 50,
          Height: 40,
          Weight: 36.5,
          OrderId: order._id.toString()
        }
      );
      break;
    default:
      // No boxes defined
      break;
  }
});

// Insert Boxes
if (boxes.length > 0) {
  db.boxes.insertMany(boxes);
}

// 10. Verification: Print Counts of Inserted Documents
print("Inserted Documents:");
print(`- Agents: ${db.agents.countDocuments()}`);
print(`- Warehouses: ${db.warehouses.countDocuments()}`);
print(`- Users: ${db.users.countDocuments()}`);
print(`- Suppliers: ${db.suppliers.countDocuments()}`);
print(`- Vessels: ${db.vessels.countDocuments()}`);
print(`- Orders: ${db.orders.countDocuments()}`);
print(`- Dispatches: ${db.dispatches.countDocuments()}`);
print(`- Boxes: ${db.boxes.countDocuments()}`);

// Optional: Display a Sample Order Document
print("Sample Order Document:");
printjson(db.orders.findOne());
