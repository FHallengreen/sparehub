// Switch to the 'sparehub' database
db = db.getSiblingDB('sparehub');

// Optional: Drop existing collections to start fresh
db.orders.drop();
db.boxes.drop();

// 1. Define fixed ObjectIds for orders
const order1Id = ObjectId("67769d0a4f4929bddffe6911");
const order2Id = ObjectId("67769d0a4f4929bddffe6912");
const order3Id = ObjectId("67769d0a4f4929bddffe6913");
const order4Id = ObjectId("67769d0a4f4929bddffe6914");

// 2. Define and Insert Test Data for Orders without Boxes
const orders = [
  {
    _id: order1Id,
    OrderNumber: "ORD-2024-0001",
    SupplierOrderNumber: "SUPORD-0001",
    Supplier: {
      Id: "supplier-1",
      Name: "Ocean Supplies Ltd."
    },
    Vessel: {
      Id: "vessel-1",
      Name: "SS Marine Explorer",
      ImoNumber: "IMO1234567",
      Flag: "Denmark",
      Owner: {
        Id: "owner-1",
        Name: "SeaTrans Shipping"
      }
    },
    Warehouse: {
      Id: "warehouse-1",
      Name: "Amsterdam Warehouse",
      Agent: {
        Id: "agent-1",
        Name: "Atlantic Agents"
      },
      Address: {
        Id: "address-1",
        AddressLine: "123 Amsterdam St",
        PostalCode: "1011 AA",
        Country: "Netherlands"
      }
    },
    Agent: {
      Id: "agent-1",
      Name: "Atlantic Agents"
    },
    ExpectedReadiness: ISODate("2024-04-15T10:00:00Z"),
    ActualReadiness: ISODate("2024-04-16T12:30:00Z"),
    ExpectedArrival: ISODate("2024-04-20T08:00:00Z"),
    ActualArrival: ISODate("2024-04-20T09:15:00Z"),
    OrderStatus: "Ready"
    // Removed Boxes array
  },
  {
    _id: order2Id,
    OrderNumber: "ORD-2024-0002",
    SupplierOrderNumber: null,
    Supplier: {
      Id: "supplier-2",
      Name: "Harbor Goods Inc."
    },
    Vessel: {
      Id: "vessel-2",
      Name: "MV Ocean Voyager",
      ImoNumber: "IMO2345678",
      Flag: "Norway",
      Owner: {
        Id: "owner-2",
        Name: "Global Vessels Co."
      }
    },
    Warehouse: {
      Id: "warehouse-2",
      Name: "Houston Warehouse",
      Agent: {
        Id: "agent-2",
        Name: "Pacific Brokers"
      },
      Address: {
        Id: "address-2",
        AddressLine: "456 Houston Rd",
        PostalCode: "77001",
        Country: "USA"
      }
    },
    Agent: {
      Id: "agent-2",
      Name: "Pacific Brokers"
    },
    ExpectedReadiness: ISODate("2024-05-10T09:00:00Z"),
    ActualReadiness: null,
    ExpectedArrival: ISODate("2024-05-15T07:00:00Z"),
    ActualArrival: null,
    OrderStatus: "Pending"
    // Removed Boxes array
  },
  {
    _id: order3Id,
    OrderNumber: "ORD-2024-0003",
    SupplierOrderNumber: "SUPORD-0003",
    Supplier: {
      Id: "supplier-3",
      Name: "Dockside Distributors"
    },
    Vessel: {
      Id: "vessel-3",
      Name: "HMS Sea King",
      ImoNumber: "IMO3456789",
      Flag: "Panama",
      Owner: {
        Id: "owner-3",
        Name: "Maritime Holdings"
      }
    },
    Warehouse: {
      Id: "warehouse-3",
      Name: "Incheon Warehouse",
      Agent: {
        Id: "agent-3",
        Name: "Northern Logistics"
      },
      Address: {
        Id: "address-3",
        AddressLine: "789 Incheon Ave",
        PostalCode: "403-720",
        Country: "South Korea"
      }
    },
    Agent: {
      Id: "agent-3",
      Name: "Northern Logistics"
    },
    ExpectedReadiness: ISODate("2024-06-01T14:00:00Z"),
    ActualReadiness: ISODate("2024-06-02T16:45:00Z"),
    ExpectedArrival: ISODate("2024-06-07T11:00:00Z"),
    ActualArrival: ISODate("2024-06-07T10:50:00Z"),
    OrderStatus: "Inbound"
    // Removed Boxes array
  },
  {
    _id: order4Id,
    OrderNumber: "ORD-2024-0004",
    SupplierOrderNumber: "SUPORD-0004",
    Supplier: {
      Id: "supplier-4",
      Name: "Portside Essentials"
    },
    Vessel: {
      Id: "vessel-4",
      Name: "FV Harbor Star",
      ImoNumber: "IMO4567890",
      Flag: "Liberia",
      Owner: {
        Id: "owner-4",
        Name: "Oceanic Enterprises"
      }
    },
    Warehouse: {
      Id: "warehouse-4",
      Name: "Osaka Warehouse",
      Agent: {
        Id: "agent-4",
        Name: "Southern Dispatch"
      },
      Address: {
        Id: "address-4",
        AddressLine: "101 Osaka Blvd",
        PostalCode: "530-0001",
        Country: "Japan"
      }
    },
    Agent: {
      Id: "agent-4",
      Name: "Southern Dispatch"
    },
    ExpectedReadiness: ISODate("2024-07-20T13:00:00Z"),
    ActualReadiness: ISODate("2024-07-21T15:20:00Z"),
    ExpectedArrival: ISODate("2024-07-25T09:30:00Z"),
    ActualArrival: ISODate("2024-07-25T10:00:00Z"),
    OrderStatus: "Stock"
    // Removed Boxes array
  }
];

// Insert Orders without Boxes
db.orders.insertMany(orders);

// 3. Define and Insert Test Data for Boxes (Separate Collection)
const boxes = [
  // Boxes for Order ORD-2024-0001
  {
    _id: ObjectId(),
    Length: 50,
    Width: 40,
    Height: 30,
    Weight: 25.5,
    OrderId: order1Id // Correctly reference the order's _id as string
  },
  {
    _id: ObjectId(),
    Length: 60,
    Width: 45,
    Height: 35,
    Weight: 30.2,
    OrderId: order1Id
  },
  // Boxes for Order ORD-2024-0002
  {
    _id: ObjectId(),
    Length: 55,
    Width: 42,
    Height: 32,
    Weight: 28.0,
    OrderId: order2Id
  },
  // Boxes for Order ORD-2024-0003
  {
    _id: ObjectId(),
    Length: 65,
    Width: 50,
    Height: 40,
    Weight: 35.7,
    OrderId: order3Id
  },
  {
    _id: ObjectId(),
    Length: 70,
    Width: 55,
    Height: 45,
    Weight: 40.3,
    OrderId: order3Id
  },
  {
    _id: ObjectId(),
    Length: 60,
    Width: 48,
    Height: 38,
    Weight: 32.8,
    OrderId: order3Id
  },
  // Boxes for Order ORD-2024-0004
  {
    _id: ObjectId(),
    Length: 58,
    Width: 44,
    Height: 34,
    Weight: 29.5,
    OrderId: order4Id
  }
];

// Insert Boxes into separate 'boxes' collection
db.boxes.insertMany(boxes);
