// Clear existing data (use with caution in a test database)
MATCH (n) DETACH DELETE n;

// Create Suppliers
CREATE (:Supplier {id: 'supplier-1', name: 'Global Supplies Inc.'});
CREATE (:Supplier {id: 'supplier-2', name: 'Maritime Logistics Ltd.'});
CREATE (:Supplier {id: 'supplier-3', name: 'Pacific Traders'});
CREATE (:Supplier {id: 'supplier-4', name: 'EuroParts Co.'});
CREATE (:Supplier {id: 'supplier-5', name: 'Atlantic Cargo'});

// Create Owners
CREATE (:Owner {id: 'owner-1', name: 'Oceanic Holdings'});
CREATE (:Owner {id: 'owner-2', name: 'Maritime Ventures'});
CREATE (:Owner {id: 'owner-3', name: 'SeaLink Enterprises'});
CREATE (:Owner {id: 'owner-4', name: 'BlueWave Shipping'});

// Link Vessels to existing Owners
MATCH (own:Owner {id: 'owner-1'})
CREATE (:Vessel {id: 'vessel-1', name: 'Neptune Voyager', imoNumber: 'IMO1234567', flag: 'Denmark'})-[:OWNED_BY]->(own);

MATCH (own:Owner {id: 'owner-2'})
CREATE (:Vessel {id: 'vessel-2', name: 'Ocean Explorer', imoNumber: 'IMO2345678', flag: 'Norway'})-[:OWNED_BY]->(own);

MATCH (own:Owner {id: 'owner-3'})
CREATE (:Vessel {id: 'vessel-3', name: 'SeaLink Pioneer', imoNumber: 'IMO3456789', flag: 'Panama'})-[:OWNED_BY]->(own);

MATCH (own:Owner {id: 'owner-4'})
CREATE (:Vessel {id: 'vessel-4', name: 'BlueWave Spirit', imoNumber: 'IMO4567890', flag: 'Liberia'})-[:OWNED_BY]->(own);

// Create Warehouses
CREATE (:Warehouse {id: 'warehouse-1', name: 'Amsterdam Warehouse'});
CREATE (:Warehouse {id: 'warehouse-2', name: 'Osaka Warehouse'});
CREATE (:Warehouse {id: 'warehouse-3', name: 'Houston Warehouse'});
CREATE (:Warehouse {id: 'warehouse-4', name: 'Singapore Warehouse'});

// Create Statuses
CREATE (:Status {name: 'Pending'});
CREATE (:Status {name: 'Ready'});
CREATE (:Status {name: 'Inbound'});
CREATE (:Status {name: 'Stock'});
CREATE (:Status {name: 'Cancelled'});
CREATE (:Status {name: 'Delivered'});

// Create Orders
CREATE (:Order {
    id: 'order-1',
    orderNumber: 'ORD-1001',
    supplierOrderNumber: 'SUP-2001',
    expectedReadiness: date('2024-01-15'),
    actualReadiness: date('2024-01-14'),
    expectedArrival: date('2024-01-25'),
    actualArrival: date('2024-01-24'),
    trackingNumber: 'TRK-7891011',
    transporter: 'TransCo Logistics'
});

CREATE (:Order {
    id: 'order-2',
    orderNumber: 'ORD-1002',
    supplierOrderNumber: 'SUP-2002',
    expectedReadiness: date('2024-02-10'),
    actualReadiness: null,
    expectedArrival: date('2024-02-20'),
    actualArrival: null,
    trackingNumber: 'TRK-11121314',
    transporter: 'Speedy Transport'
});

CREATE (:Order {
    id: 'order-3',
    orderNumber: 'ORD-1003',
    supplierOrderNumber: 'SUP-2003',
    expectedReadiness: date('2024-03-01'),
    actualReadiness: date('2024-03-02'),
    expectedArrival: date('2024-03-10'),
    actualArrival: date('2024-03-09'),
    trackingNumber: 'TRK-987654',
    transporter: 'Global Freight'
});

CREATE (:Order {
    id: 'order-4',
    orderNumber: 'ORD-1004',
    supplierOrderNumber: 'SUP-2004',
    expectedReadiness: date('2024-04-05'),
    actualReadiness: date('2024-04-04'),
    expectedArrival: date('2024-04-15'),
    actualArrival: date('2024-04-14'),
    trackingNumber: 'TRK-12131415',
    transporter: 'Express Movers'
});

CREATE (:Order {
    id: 'order-5',
    orderNumber: 'ORD-1005',
    supplierOrderNumber: 'SUP-2005',
    expectedReadiness: date('2024-05-20'),
    actualReadiness: null,
    expectedArrival: date('2024-05-30'),
    actualArrival: null,
    trackingNumber: 'TRK-16171819',
    transporter: 'Oceanic Transit'
});

// Link Orders to Suppliers
MATCH (o:Order {id: 'order-1'}), (s:Supplier {id: 'supplier-1'})
CREATE (o)-[:SUPPLIED_BY]->(s);

MATCH (o:Order {id: 'order-2'}), (s:Supplier {id: 'supplier-2'})
CREATE (o)-[:SUPPLIED_BY]->(s);

MATCH (o:Order {id: 'order-3'}), (s:Supplier {id: 'supplier-1'})
CREATE (o)-[:SUPPLIED_BY]->(s);

MATCH (o:Order {id: 'order-4'}), (s:Supplier {id: 'supplier-3'})
CREATE (o)-[:SUPPLIED_BY]->(s);

MATCH (o:Order {id: 'order-5'}), (s:Supplier {id: 'supplier-4'})
CREATE (o)-[:SUPPLIED_BY]->(s);

// Link Orders to Vessels
MATCH (o:Order {id: 'order-1'}), (v:Vessel {id: 'vessel-1'})
CREATE (o)-[:FOR_VESSEL]->(v);

MATCH (o:Order {id: 'order-2'}), (v:Vessel {id: 'vessel-2'})
CREATE (o)-[:FOR_VESSEL]->(v);

MATCH (o:Order {id: 'order-3'}), (v:Vessel {id: 'vessel-3'})
CREATE (o)-[:FOR_VESSEL]->(v);

MATCH (o:Order {id: 'order-4'}), (v:Vessel {id: 'vessel-4'})
CREATE (o)-[:FOR_VESSEL]->(v);

MATCH (o:Order {id: 'order-5'}), (v:Vessel {id: 'vessel-2'})
CREATE (o)-[:FOR_VESSEL]->(v);

// Link Orders to Warehouses
MATCH (o:Order {id: 'order-1'}), (w:Warehouse {id: 'warehouse-1'})
CREATE (o)-[:STORED_AT]->(w);

MATCH (o:Order {id: 'order-2'}), (w:Warehouse {id: 'warehouse-2'})
CREATE (o)-[:STORED_AT]->(w);

MATCH (o:Order {id: 'order-3'}), (w:Warehouse {id: 'warehouse-1'})
CREATE (o)-[:STORED_AT]->(w);

MATCH (o:Order {id: 'order-4'}), (w:Warehouse {id: 'warehouse-3'})
CREATE (o)-[:STORED_AT]->(w);

MATCH (o:Order {id: 'order-5'}), (w:Warehouse {id: 'warehouse-4'})
CREATE (o)-[:STORED_AT]->(w);

// Link Orders to Boxes
// Order 1
MATCH (o:Order {id: 'order-1'})
CREATE (b:Box {id: 'box-1', length: 120, width: 80, height: 50, weight: 105})
CREATE (b)-[:BELONGS_TO]->(o);

MATCH (o:Order {id: 'order-1'})
CREATE (b:Box {id: 'box-2', length: 100, width: 70, height: 40, weight: 83})
CREATE (b)-[:BELONGS_TO]->(o);

// Order 2
MATCH (o:Order {id: 'order-2'})
CREATE (b:Box {id: 'box-3', length: 130, width: 85, height: 55, weight: 95})
CREATE (b)-[:BELONGS_TO]->(o);

MATCH (o:Order {id: 'order-2'})
CREATE (b:Box {id: 'box-4', length: 110, width: 75, height: 45, weight: 90})
CREATE (b)-[:BELONGS_TO]->(o);

// Order 3
MATCH (o:Order {id: 'order-3'})
CREATE (b:Box {id: 'box-5', length: 180, width: 90, height: 60, weight: 150})
CREATE (b)-[:BELONGS_TO]->(o);

MATCH (o:Order {id: 'order-3'})
CREATE (b:Box {id: 'box-6', length: 200, width: 100, height: 80, weight: 200})
CREATE (b)-[:BELONGS_TO]->(o);

// Order 4
MATCH (o:Order {id: 'order-4'})
CREATE (b:Box {id: 'box-7', length: 140, width: 80, height: 50, weight: 100})
CREATE (b)-[:BELONGS_TO]->(o);

MATCH (o:Order {id: 'order-4'})
CREATE (b:Box {id: 'box-8', length: 160, width: 90, height: 60, weight: 120})
CREATE (b)-[:BELONGS_TO]->(o);

// Order 5
MATCH (o:Order {id: 'order-5'})
CREATE (b:Box {id: 'box-9', length: 150, width: 85, height: 55, weight: 110})
CREATE (b)-[:BELONGS_TO]->(o);

MATCH (o:Order {id: 'order-5'})
CREATE (b:Box {id: 'box-10', length: 170, width: 95, height: 65, weight: 130})
CREATE (b)-[:BELONGS_TO]->(o);

// Link Orders to Statuses
MATCH (o:Order {id: 'order-1'}), (s:Status {name: 'Delivered'})
CREATE (o)-[:HAS_STATUS]->(s);

MATCH (o:Order {id: 'order-2'}), (s:Status {name: 'Pending'})
CREATE (o)-[:HAS_STATUS]->(s);

MATCH (o:Order {id: 'order-3'}), (s:Status {name: 'Inbound'})
CREATE (o)-[:HAS_STATUS]->(s);

MATCH (o:Order {id: 'order-4'}), (s:Status {name: 'Ready'})
CREATE (o)-[:HAS_STATUS]->(s);

MATCH (o:Order {id: 'order-5'}), (s:Status {name: 'Pending'})
CREATE (o)-[:HAS_STATUS]->(s);
