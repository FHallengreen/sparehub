-- Insert test data for address table
INSERT INTO `address` (`address`, `postal_code`, `country`)
VALUES
('123 Shipping Lane', '12345', 'Denmark'),
('456 Ocean Ave', '67890', 'Germany'),
('789 Sailor Street', '54321', 'Netherlands'),
('101 Anchor Road', '09876', 'Norway');

-- Insert test data for supplier table
INSERT INTO `supplier` (`name`, `address_id`)
VALUES
('Flexco Ltd', 1),
('Shell', 2),
('Maersk', 3),
('V.Ships USA LLC', 4);

-- Insert test data for owner table
INSERT INTO `owner` (`name`)
VALUES
('John Smith Inc.'),
('Oceanic Corp.'),
('V.Ships Holdings'),
('Maersk Group');

-- Insert test data for vessel table
-- Insert additional test data for vessel table
INSERT INTO `vessel` (`owner_id`, `name`, `imo_number`, `flag`)
VALUES
(1, 'Neptune', '5678901', 'DK'),
(2, 'Titanic II', '6789012', 'DE'),
(3, 'Poseidon', '7890123', 'NL'),
(4, 'Endeavour', '8901234', 'NO'),
(1, 'Horizon', '9012345', 'DK'),
(2, 'Mariner', '0123456', 'DE'),
(3, 'Voyager', '1123456', 'NL'),
(4, 'Discovery', '1223456', 'NO'),
(1, 'Liberty', '1323456', 'DK'),
(2, 'Enterprise', '1423456', 'DE'),
(3, 'Atlantis', '1523456', 'NL'),
(4, 'Explorer II', '1623456', 'NO'),
(1, 'Odyssey', '1723456', 'DK'),
(2, 'Catalyst', '1823456', 'DE'),
(3, 'Galileo', '1923456', 'NL'),
(4, 'Pioneer', '2023456', 'NO');



-- Insert test data for agent table
INSERT INTO `agent` (`name`)
VALUES
('Agent One'),
('Agent Two'),
('Agent Three'),
('Agent Four');

-- Insert test data for warehouse table
INSERT INTO `warehouse` (`name`, `agent_id`)
VALUES
('Amsterdam Warehouse', 1),
('Rotterdam Warehouse', 2),
('Hamburg Warehouse', 3),
('Tokyo Warehouse', 4);

-- Insert test data for order_status table
INSERT INTO `order_status` (`status`)
VALUES
('Pending'),
('Ready'),
('Inbound'),
('Stock'),
('Cancelled');

-- Insert test data for order table
-- Insert additional test data for order table
INSERT INTO `order` 
    (`order_number`, `supplier_order_number`, `supplier_id`, `vessel_id`, `warehouse_id`, 
     `expected_readiness`, `actual_readiness`, `expected_arrival`, `actual_arrival`, `order_status`)
VALUES
('ORD005', 'SUP005', 1, 1, 1, '2024-10-20 09:00:00', '2024-10-22 10:00:00', '2024-10-25 08:00:00', '2024-10-27 15:00:00', 'Ready'),
('ORD006', 'SUP006', 2, 6, 2, '2024-10-18 11:30:00', '2024-10-19 14:00:00', '2024-10-23 12:00:00', '2024-10-24 17:00:00', 'Pending'),
('ORD007', 'SUP007', 3, 7, 3, '2024-11-05 13:00:00', '2024-11-10 16:00:00', '2024-11-20 09:00:00', '2024-11-25 14:00:00', 'Inbound'),
('ORD008', 'SUP008', 4, 8, 4, '2024-09-01 08:00:00', '2024-09-05 10:00:00', '2024-09-10 15:00:00', '2024-09-12 18:00:00', 'Cancelled'),
('ORD009', 'SUP009', 1, 9, 1, '2024-10-25 10:00:00', '2024-10-28 12:00:00', '2024-11-01 09:00:00', '2024-11-03 16:00:00', 'Ready'),
('ORD010', 'SUP010', 2, 10, 2, '2024-10-22 07:30:00', '2024-10-24 09:00:00', '2024-10-28 14:00:00', '2024-10-30 11:00:00', 'Pending'),
('ORD011', 'SUP011', 3, 11, 3, '2024-11-10 15:00:00', '2024-11-15 18:00:00', '2024-11-25 10:00:00', '2024-11-30 13:00:00', 'Inbound'),
('ORD012', 'SUP012', 4, 12, 4, '2024-08-15 09:30:00', '2024-08-20 11:00:00', '2024-08-25 16:00:00', '2024-08-28 19:00:00', 'Stock'),
('ORD013', 'SUP013', 1, 13, 1, '2024-10-30 12:00:00', '2024-11-02 14:00:00', '2024-11-05 08:00:00', '2024-11-07 15:00:00', 'Ready'),
('ORD014', 'SUP014', 2, 14, 2, '2024-10-28 08:30:00', '2024-10-29 10:00:00', '2024-11-02 17:00:00', '2024-11-04 12:00:00', 'Pending'),
('ORD015', 'SUP015', 3, 15, 3, '2024-11-15 14:00:00', '2024-11-20 18:00:00', '2024-11-30 10:00:00', '2024-12-05 13:00:00', 'Inbound'),
('ORD016', 'SUP016', 4, 16, 4, '2024-08-20 10:00:00', '2024-08-25 12:00:00', '2024-08-30 15:00:00', '2024-09-02 18:00:00', 'Stock'),
('ORD017', 'SUP017', 1, 1, 1, '2024-11-05 09:00:00', '2024-11-07 11:00:00', '2024-11-10 08:00:00', '2024-11-12 15:00:00', 'Ready'),
('ORD018', 'SUP018', 2, 2, 2, '2024-11-03 07:30:00', '2024-11-04 09:00:00', '2024-11-08 14:00:00', '2024-11-10 11:00:00', 'Pending'),
('ORD019', 'SUP019', 3, 3, 3, '2024-12-01 15:00:00', '2024-12-06 18:00:00', '2024-12-15 10:00:00', '2024-12-20 13:00:00', 'Inbound'),
('ORD020', 'SUP020', 4, 4, 4, '2024-09-10 09:30:00', '2024-09-15 11:00:00', '2024-09-20 16:00:00', '2024-09-23 19:00:00', 'Stock'),
('ORD021', 'SUP021', 1, 5, 1, '2024-10-21 09:00:00', '2024-10-23 10:00:00', '2024-10-28 08:00:00', '2024-10-30 15:00:00', 'Ready'),
('ORD022', 'SUP022', 2, 6, 2, '2024-10-19 11:30:00', '2024-10-20 14:00:00', '2024-10-24 12:00:00', '2024-10-25 17:00:00', 'Pending'),
('ORD023', 'SUP023', 3, 7, 3, '2024-11-05 13:00:00', '2024-11-10 16:00:00', '2024-11-20 09:00:00', '2024-11-25 14:00:00', 'Inbound'),
('ORD024', 'SUP024', 4, 8, 4, '2024-09-01 08:00:00', '2024-09-05 10:00:00', '2024-09-10 15:00:00', '2024-09-12 18:00:00', 'Stock'),
('ORD025', 'SUP025', 1, 9, 1, '2024-10-25 10:00:00', '2024-10-28 12:00:00', '2024-11-01 09:00:00', '2024-11-03 16:00:00', 'Ready'),
('ORD026', 'SUP026', 2, 10, 2, '2024-10-22 07:30:00', '2024-10-24 09:00:00', '2024-10-28 14:00:00', '2024-10-30 11:00:00', 'Pending'),
('ORD027', 'SUP027', 3, 11, 3, '2024-11-10 15:00:00', '2024-11-15 18:00:00', '2024-11-25 10:00:00', '2024-11-30 13:00:00', 'Inbound'),
('ORD028', 'SUP028', 4, 12, 4, '2024-08-15 09:30:00', '2024-08-20 11:00:00', '2024-08-25 16:00:00', '2024-08-28 19:00:00', 'Stock'),
('ORD029', 'SUP029', 1, 13, 1, '2024-10-30 12:00:00', '2024-11-02 14:00:00', '2024-11-05 08:00:00', '2024-11-07 15:00:00', 'Ready'),
('ORD030', 'SUP030', 2, 14, 2, '2024-10-28 08:30:00', '2024-10-29 10:00:00', '2024-11-02 17:00:00', '2024-11-04 12:00:00', 'Pending'),
('ORD031', 'SUP031', 3, 15, 3, '2024-11-15 14:00:00', '2024-11-20 18:00:00', '2024-11-30 10:00:00', '2024-12-05 13:00:00', 'Inbound'),
('ORD032', 'SUP032', 4, 16, 4, '2024-08-20 10:00:00', '2024-08-25 12:00:00', '2024-08-30 15:00:00', '2024-09-02 18:00:00', 'Stock'),
('ORD033', 'SUP033', 1, 1, 1, '2024-11-05 09:00:00', '2024-11-07 11:00:00', '2024-11-10 08:00:00', '2024-11-12 15:00:00', 'Ready'),
('ORD034', 'SUP034', 2, 2, 2, '2024-11-03 07:30:00', '2024-11-04 09:00:00', '2024-11-08 14:00:00', '2024-11-10 11:00:00', 'Pending'),
('ORD035', 'SUP035', 3, 3, 3, '2024-12-01 15:00:00', '2024-12-06 18:00:00', '2024-12-15 10:00:00', '2024-12-20 13:00:00', 'Inbound'),
('ORD036', 'SUP036', 4, 4, 4, '2024-09-10 09:30:00', '2024-09-15 11:00:00', '2024-09-20 16:00:00', '2024-09-23 19:00:00', 'Stock'),
('ORD037', 'SUP037', 1, 1, 1, '2024-10-18 10:00:00', '2024-10-20 12:00:00', '2024-10-25 09:00:00', '2024-10-27 16:00:00', 'Ready'),
('ORD038', 'SUP038', 2, 6, 2, '2024-10-16 08:30:00', '2024-10-17 10:00:00', '2024-10-21 14:00:00', '2024-10-22 17:00:00', 'Pending'),
('ORD039', 'SUP039', 3, 7, 3, '2024-11-12 13:00:00', '2024-11-17 16:00:00', '2024-11-27 09:00:00', '2024-12-02 14:00:00', 'Inbound'),
('ORD040', 'SUP040', 4, 8, 4, '2024-09-05 08:00:00', '2024-09-09 10:00:00', '2024-09-14 15:00:00', '2024-09-16 18:00:00', 'Cancelled'),
('ORD041', 'SUP041', 1, 9, 1, '2024-10-29 10:00:00', '2024-11-01 12:00:00', '2024-11-06 09:00:00', '2024-11-08 16:00:00', 'Ready'),
('ORD042', 'SUP042', 2, 10, 2, '2024-10-26 07:30:00', '2024-10-28 09:00:00', '2024-11-01 14:00:00', '2024-11-03 11:00:00', 'Pending'),
('ORD043', 'SUP043', 3, 11, 3, '2024-11-17 15:00:00', '2024-11-22 18:00:00', '2024-12-02 10:00:00', '2024-12-07 13:00:00', 'Inbound'),
('ORD044', 'SUP044', 4, 12, 4, '2024-08-20 09:30:00', '2024-08-25 11:00:00', '2024-08-30 16:00:00', '2024-09-02 19:00:00', 'Stock'),
('ORD045', 'SUP045', 1, 13, 1, '2024-10-31 12:00:00', '2024-11-03 14:00:00', '2024-11-06 08:00:00', '2024-11-08 15:00:00', 'Ready'),
('ORD046', 'SUP046', 2, 14, 2, '2024-10-29 08:30:00', '2024-10-30 10:00:00', '2024-11-04 17:00:00', '2024-11-06 12:00:00', 'Pending'),
('ORD047', 'SUP047', 3, 15, 3, '2024-11-16 14:00:00', '2024-11-21 18:00:00', '2024-12-01 10:00:00', '2024-12-06 13:00:00', 'Inbound'),
('ORD048', 'SUP048', 4, 16, 4, '2024-08-25 10:00:00', '2024-08-30 12:00:00', '2024-09-04 15:00:00', '2024-09-07 18:00:00', 'Stock'),
('ORD049', 'SUP049', 1, 1, 1, '2024-11-06 09:00:00', '2024-11-08 11:00:00', '2024-11-11 08:00:00', '2024-11-13 15:00:00', 'Ready'),
('ORD050', 'SUP050', 2, 2, 2, '2024-11-04 07:30:00', '2024-11-05 09:00:00', '2024-11-09 14:00:00', '2024-11-11 11:00:00', 'Pending'),
('ORD051', 'SUP051', 3, 3, 3, '2024-12-02 15:00:00', '2024-12-07 18:00:00', '2024-12-16 10:00:00', '2024-12-21 13:00:00', 'Inbound'),
('ORD052', 'SUP052', 4, 4, 4, '2024-09-11 09:30:00', '2024-09-16 11:00:00', '2024-09-21 16:00:00', '2024-09-24 19:00:00', 'Cancelled'),
('ORD053', 'SUP053', 1, 5, 1, '2024-10-19 10:00:00', '2024-10-21 12:00:00', '2024-10-26 09:00:00', '2024-10-28 16:00:00', 'Ready'),
('ORD054', 'SUP054', 2, 6, 2, '2024-10-17 08:30:00', '2024-10-18 10:00:00', '2024-10-22 14:00:00', '2024-10-23 17:00:00', 'Pending'),
('ORD055', 'SUP055', 3, 7, 3, '2024-11-13 13:00:00', '2024-11-18 16:00:00', '2024-11-28 09:00:00', '2024-12-03 14:00:00', 'Inbound'),
('ORD056', 'SUP056', 4, 8, 4, '2024-09-06 08:00:00', '2024-09-10 10:00:00', '2024-09-15 15:00:00', '2024-09-17 18:00:00', 'Pending'),
('ORD057', 'SUP057', 1, 9, 1, '2024-10-30 10:00:00', '2024-11-01 12:00:00', '2024-11-06 09:00:00', '2024-11-08 16:00:00', 'Ready'),
('ORD058', 'SUP058', 2, 10, 2, '2024-10-27 07:30:00', '2024-10-29 09:00:00', '2024-11-03 14:00:00', '2024-11-05 11:00:00', 'Pending'),
('ORD059', 'SUP059', 3, 11, 3, '2024-11-18 15:00:00', '2024-11-23 18:00:00', '2024-12-03 10:00:00', '2024-12-08 13:00:00', 'Inbound'),
('ORD060', 'SUP060', 4, 12, 4, '2024-08-21 09:30:00', '2024-08-26 11:00:00', '2024-09-01 16:00:00', '2024-09-04 19:00:00', 'Cancelled'),
('ORD061', 'SUP061', 1, 13, 1, '2024-10-31 12:00:00', '2024-11-03 14:00:00', '2024-11-06 08:00:00', '2024-11-08 15:00:00', 'Ready'),
('ORD062', 'SUP062', 2, 14, 2, '2024-10-29 08:30:00', '2024-10-30 10:00:00', '2024-11-04 17:00:00', '2024-11-06 12:00:00', 'Pending'),
('ORD063', 'SUP063', 3, 15, 3, '2024-11-17 14:00:00', '2024-11-22 18:00:00', '2024-12-02 10:00:00', '2024-12-07 13:00:00', 'Inbound'),
('ORD064', 'SUP064', 4, 16, 4, '2024-08-26 10:00:00', '2024-08-31 12:00:00', '2024-09-05 15:00:00', '2024-09-08 18:00:00', 'Cancelled'),
('ORD065', 'SUP065', 1, 1, 1, '2024-11-07 09:00:00', '2024-11-09 11:00:00', '2024-11-12 08:00:00', '2024-11-14 15:00:00', 'Ready'),
('ORD066', 'SUP066', 2, 2, 2, '2024-11-05 07:30:00', '2024-11-06 09:00:00', '2024-11-10 14:00:00', '2024-11-12 11:00:00', 'Pending'),
('ORD067', 'SUP067', 3, 3, 3, '2024-12-03 15:00:00', '2024-12-08 18:00:00', '2024-12-17 10:00:00', '2024-12-22 13:00:00', 'Inbound'),
('ORD068', 'SUP068', 4, 1, 4, '2024-09-12 09:30:00', '2024-09-17 11:00:00', '2024-09-22 16:00:00', '2024-09-25 19:00:00', 'Cancelled'),
('ORD069', 'SUP069', 1, 1, 1, '2024-10-20 10:00:00', '2024-10-22 12:00:00', '2024-10-27 09:00:00', '2024-10-29 16:00:00', 'Ready'),
('ORD070', 'SUP070', 2, 1, 2, '2024-10-18 08:30:00', '2024-10-19 10:00:00', '2024-10-23 14:00:00', '2024-10-24 17:00:00', 'Pending'),
('ORD071', 'SUP071', 3, 1, 3, '2024-11-14 13:00:00', '2024-11-19 16:00:00', '2024-12-01 09:00:00', '2024-12-06 14:00:00', 'Inbound'),
('ORD072', 'SUP072', 4, 8, 4, '2024-09-07 08:00:00', '2024-09-11 10:00:00', '2024-09-16 15:00:00', '2024-09-18 18:00:00', 'Stock'),
('ORD073', 'SUP073', 1, 9, 1, '2024-10-31 10:00:00', '2024-11-02 12:00:00', '2024-11-07 09:00:00', '2024-11-09 16:00:00', 'Ready'),
('ORD074', 'SUP074', 2, 1, 2, '2024-10-28 07:30:00', '2024-10-30 09:00:00', '2024-11-04 14:00:00', '2024-11-06 11:00:00', 'Pending'),
('ORD075', 'SUP075', 3, 11, 3, '2024-11-19 15:00:00', '2024-11-24 18:00:00', '2024-12-04 10:00:00', '2024-12-09 13:00:00', 'Inbound'),
('ORD076', 'SUP076', 4, 12, 4, '2024-08-22 09:30:00', '2024-08-27 11:00:00', '2024-09-01 16:00:00', '2024-09-04 19:00:00', 'Stock'),
('ORD077', 'SUP077', 1, 13, 1, '2024-11-01 12:00:00', '2024-11-04 14:00:00', '2024-11-07 08:00:00', '2024-11-09 15:00:00', 'Ready'),
('ORD078', 'SUP078', 2, 14, 2, '2024-10-29 08:30:00', '2024-10-30 10:00:00', '2024-11-04 17:00:00', '2024-11-06 12:00:00', 'Pending'),
('ORD079', 'SUP079', 3, 15, 3, '2024-11-18 14:00:00', '2024-11-23 18:00:00', '2024-12-03 10:00:00', '2024-12-08 13:00:00', 'Inbound'),
('ORD080', 'SUP080', 4, 16, 4, '2024-08-27 10:00:00', '2024-09-01 12:00:00', '2024-09-06 15:00:00', '2024-09-09 18:00:00', 'Cancelled'),
('ORD081', 'SUP081', 1, 1, 1, '2024-11-08 09:00:00', '2024-11-10 11:00:00', '2024-11-13 08:00:00', '2024-11-15 15:00:00', 'Ready'),
('ORD082', 'SUP082', 2, 2, 2, '2024-11-06 07:30:00', '2024-11-07 09:00:00', '2024-11-11 14:00:00', '2024-11-13 11:00:00', 'Pending'),
('ORD083', 'SUP083', 3, 3, 3, '2024-12-04 15:00:00', '2024-12-09 18:00:00', '2024-12-18 10:00:00', '2024-12-23 13:00:00', 'Inbound'),
('ORD084', 'SUP084', 4, 4, 4, '2024-09-13 09:30:00', '2024-09-18 11:00:00', '2024-09-23 16:00:00', '2024-09-26 19:00:00', 'Cancelled');


-- Insert test data for role table
INSERT INTO `role` (`title`)
VALUES
('Operator'),
('Manager'),
('Supervisor');

-- Insert test data for user table
INSERT INTO `user` (`role_id`, `name`)
VALUES
(1, 'Operator One'),
(2, 'Manager Two'),
(3, 'Supervisor Three');

-- Insert test data for dispatch_status table
INSERT INTO `dispatch_status` (`status`)
VALUES
('Created'),
('Sent'),
('Delivered');

-- Insert test data for transport_mode table
INSERT INTO `transport_mode` (`type`)
VALUES
('Air'),
('Sea'),
('Courier');

-- Insert test data for dispatch table
-- Insert additional test data for dispatch table
INSERT INTO `dispatch` 
    (`origin_type`, `origin_id`, `destination_type`, `destination_id`, 
     `dispatch_status`, `transport_mode_type`, `tracking_number`, 
     `dispatch_date`, `delivery_date`, `user_id`)
VALUES
('Warehouse', 2, 'Warehouse', 3, 'Created', 'Sea', 'TRK456789', '2024-10-05 13:00:00', '2024-10-10 15:00:00', 1),
('Warehouse', 4, 'Warehouse', 1, 'Sent', 'Air', 'TRK567890', '2024-09-20 10:00:00', '2024-09-25 17:00:00', 2),
('Supplier', 2, 'Address', 4, 'Delivered', 'Courier', 'TRK678901', '2024-08-15 11:00:00', '2024-08-20 19:00:00', 3),
('Warehouse', 1, 'Address', 2, 'Created', 'Sea', 'TRK789012', '2024-10-10 14:00:00', '2024-10-15 16:00:00', 1),
('Supplier', 3, 'Warehouse', 4, 'Sent', 'Air', 'TRK890123', '2024-09-25 09:00:00', '2024-09-30 18:00:00', 2),
('Warehouse', 3, 'Supplier', 1, 'Delivered', 'Courier', 'TRK901234', '2024-08-20 12:00:00', '2024-08-25 20:00:00', 3),
('Address', 1, 'Warehouse', 2, 'Created', 'Sea', 'TRK012345', '2024-10-15 15:00:00', '2024-10-20 17:00:00', 1),
('Warehouse', 2, 'Address', 3, 'Sent', 'Air', 'TRK123457', '2024-09-30 08:00:00', '2024-10-05 19:00:00', 2),
('Supplier', 4, 'Warehouse', 1, 'Delivered', 'Courier', 'TRK234568', '2024-08-25 13:00:00', '2024-08-30 21:00:00', 3),
('Warehouse', 4, 'Warehouse', 3, 'Created', 'Sea', 'TRK345679', '2024-10-20 16:00:00', '2024-10-25 18:00:00', 1),
('Supplier', 1, 'Address', 2, 'Sent', 'Air', 'TRK456780', '2024-09-05 07:00:00', '2024-09-10 20:00:00', 2),
('Warehouse', 3, 'Warehouse', 4, 'Delivered', 'Courier', 'TRK567891', '2024-08-30 14:00:00', '2024-09-04 22:00:00', 3),
('Address', 4, 'Warehouse', 1, 'Created', 'Sea', 'TRK678902', '2024-10-25 17:00:00', '2024-10-30 19:00:00', 1),
('Warehouse', 1, 'Address', 3, 'Sent', 'Air', 'TRK789013', '2024-09-10 06:00:00', '2024-09-15 21:00:00', 2),
('Supplier', 2, 'Warehouse', 2, 'Delivered', 'Courier', 'TRK890124', '2024-08-10 15:00:00', '2024-08-15 23:00:00', 3),
('Warehouse', 4, 'Warehouse', 2, 'Created', 'Sea', 'TRK901235', '2024-10-30 18:00:00', '2024-11-04 20:00:00', 1),
('Supplier', 3, 'Address', 1, 'Sent', 'Air', 'TRK012346', '2024-09-15 05:00:00', '2024-09-20 22:00:00', 2),
('Warehouse', 2, 'Warehouse', 4, 'Delivered', 'Courier', 'TRK123458', '2024-08-20 16:00:00', '2024-08-25 23:00:00', 3),
('Address', 3, 'Warehouse', 1, 'Created', 'Sea', 'TRK234569', '2024-10-05 19:00:00', '2024-10-10 21:00:00', 1),
('Warehouse', 1, 'Address', 4, 'Sent', 'Air', 'TRK345680', '2024-09-25 04:00:00', '2024-09-30 23:00:00', 2),
('Supplier', 4, 'Warehouse', 3, 'Delivered', 'Courier', 'TRK456781', '2024-08-05 17:00:00', '2024-08-10 23:00:00', 3);

-- Insert test data for invoice table
INSERT INTO `invoice` (`dispatch_id`)
VALUES
(1),
(2),
(3);

-- Insert test data for dispatch_has_order table
INSERT INTO `dispatch_has_order` (`dispatch_id`, `order_id`)
VALUES
(1, 1),
(2, 2),
(3, 3);

-- Insert test data for owner_has_user table
INSERT INTO `owner_has_user` (`owner_id`, `operator_id`)
VALUES
(1, 1),
(2, 2),
(3, 3);

-- Insert test data for contact_info table
INSERT INTO `contact_info` (`name`, `value`, `contact_type`)
VALUES
('Phone', '1234567890', 'phone'),
('Email', 'example@domain.com', 'email'),
('Mobile', '0987654321', 'mobile');

-- Insert test data for address_has_contact_info table
INSERT INTO `address_has_contact_info` (`address_id`, `contact_info_id`)
VALUES
(1, 1),
(2, 2),
(3, 3);

-- Insert test data for contact_info_has_user table
INSERT INTO `contact_info_has_user` (`contact_info_id`, `user_id`)
VALUES
(1, 1),
(2, 2),
(3, 3);

-- Insert test data for cost_type table
INSERT INTO `cost_type` (`type`)
VALUES
('Airfreight'),
('Export'),
('Handling');

-- Insert test data for currency table
INSERT INTO `currency` (`code`)
VALUES
('EUR'),
('USD'),
('DKK');

-- Insert test data for financial table
INSERT INTO `financial` (`invoice_id`, `cost_type_id`, `currency_id`)
VALUES
(1, 1, 1),
(2, 2, 2),
(3, 3, 3);