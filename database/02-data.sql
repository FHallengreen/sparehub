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
INSERT INTO `vessel` (`owner_id`, `name`, `imo_number`, `flag`)
VALUES
(1, 'Aurora', '1234567', 'DK'),
(2, 'Oceanic', '2345678', 'DE'),
(3, 'CSL Metis', '3456789', 'NL'),
(4, 'Explorer', '4567890', 'NO');

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
INSERT INTO `order` (`order_number`, `supplier_order_number`, `supplier_id`, `vessel_id`, `warehouse_id`, `expected_readiness`, `actual_readiness`, `expected_arrival`, `actual_arrival`, `order_status`)
VALUES
('ORD001', 'SUP001', 1, 1, 1, '2024-10-01 10:00:00', '2024-10-05 14:00:00', '2024-10-10 09:00:00', '2024-10-12 16:00:00', 'Pending'),
('ORD002', 'SUP002', 2, 2, 2, '2024-09-15 08:30:00', '2024-09-20 12:00:00', '2024-09-25 17:00:00', '2024-09-28 11:00:00', 'Stock'),
('ORD003', 'SUP003', 3, 3, 3, '2024-11-01 14:00:00', '2024-11-07 18:00:00', '2024-11-15 10:00:00', '2024-11-20 13:00:00', 'Inbound'),
('ORD004', 'SUP004', 4, 4, 4, '2024-08-10 09:00:00', '2024-08-15 11:00:00', '2024-08-20 15:00:00', '2024-08-23 18:00:00', 'Cancelled');

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
INSERT INTO `dispatch` (`origin_type`, `origin_id`, `destination_type`, `destination_id`, `dispatch_status`, `transport_mode_type`, `tracking_number`, `dispatch_date`, `delivery_date`, `user_id`)
VALUES
('Warehouse', 1, 'Warehouse', 2, 'Created', 'Air', 'TRK123456', '2024-10-01 12:00:00', '2024-10-05 14:00:00', 1),
('Warehouse', 3, 'Warehouse', 4, 'Sent', 'Sea', 'TRK234567', '2024-09-15 09:00:00', '2024-09-18 16:00:00', 2),
('Supplier', 1, 'Address', 3, 'Delivered', 'Courier', 'TRK345678', '2024-08-10 10:00:00', '2024-08-15 18:00:00', 3);

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
