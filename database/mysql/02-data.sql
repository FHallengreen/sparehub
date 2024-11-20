USE `sparehub`;

-- -------------------------------------
-- Insert into `address`
-- -------------------------------------
INSERT INTO `address` (`address`, `postal_code`, `country`) VALUES
('123 Marine Way', '90001', 'USA'),
('456 Harbor Street', 'SW1A 1AA', 'UK'),
('789 Dockside Blvd', '2000', 'Australia'),
('321 Port Avenue', '1000', 'Canada'),
('654 Marina Drive', '3000', 'Germany');

-- -------------------------------------
-- Insert into `supplier`
-- -------------------------------------
INSERT INTO `supplier` (`name`, `address_id`) VALUES
('Ocean Supplies Ltd.', 1),
('Harbor Goods Inc.', 2),
('Dockside Distributors', 3),
('Portside Essentials', 4),
('Marina Merchants', 5);

-- -------------------------------------
-- Insert into `owner`
-- -------------------------------------
INSERT INTO `owner` (`name`) VALUES
('SeaTrans Shipping'),
('Global Vessels Co.'),
('Maritime Holdings'),
('Oceanic Enterprises'),
('Voyage Masters');

-- -------------------------------------
-- Insert into `vessel`
-- -------------------------------------
INSERT INTO `vessel` (`owner_id`, `name`, `imo_number`, `flag`) VALUES
(1, 'Guadalupe Explorer', '1234567', 'USA'),
(2, 'Ocean Voyager', '2345678', 'UK'),
(3, 'Sea King', '3456789', 'AUS'),
(4, 'Harbor Star', '4567890', 'CAN'),
(5, 'Yacht Marina Dream', '5678901', 'GER');

-- -------------------------------------
-- Insert into `agent`
-- -------------------------------------
INSERT INTO `agent` (`name`) VALUES
('Atlantic Agents'),
('Pacific Brokers'),
('Northern Logistics'),
('Southern Dispatch'),
('Eastern Freight');

-- -------------------------------------
-- Insert into `warehouse`
-- -------------------------------------
INSERT INTO `warehouse` (`name`, `agent_id`) VALUES
('Amsterdam Warehouse', 1),
('Houston Warehouse', 2),
('Incheon Warehouse', 3),
('Osaka Warehouse', 4),
('Shanghai Warehouse', 5);

-- -------------------------------------
-- Insert into `order_status`
-- -------------------------------------
INSERT INTO `order_status` (`status`) VALUES
('Pending'),
('Ready'),
('Inbound'),
('Stock'),
('Cancelled');

-- -------------------------------------
-- Insert into `role`
-- -------------------------------------
INSERT INTO `role` (`title`) VALUES
('Administrator'),
('Operator'),
('Manager'),
('Dispatcher'),
('Viewer');

-- -------------------------------------
-- Insert into `user`
-- -------------------------------------
INSERT INTO `user` (`role_id`, `name`) VALUES
(1, 'Alice Johnson'),
(2, 'Bob Smith'),
(3, 'Carol Williams'),
(4, 'David Brown'),
(5, 'Eva Davis');

-- -------------------------------------
-- Insert into `dispatch_status`
-- -------------------------------------
INSERT INTO `dispatch_status` (`status`) VALUES
('Created'),
('Sent'),
('Delivered');

-- -------------------------------------
-- Insert into `transport_mode`
-- -------------------------------------
INSERT INTO `transport_mode` (`type`) VALUES
('Air'),
('Sea'),
('Courier');

-- -------------------------------------
-- Insert into `port`
-- -------------------------------------
INSERT INTO `port` (`name`) VALUES
('Port of Los Angeles'),
('Port of London'),
('Port of Sydney'),
('Port of Toronto'),
('Port of Hamburg');

-- -------------------------------------
-- Insert into `cost_type`
-- -------------------------------------
INSERT INTO `cost_type` (`type`) VALUES
('Shipping'),
('Handling'),
('Storage'),
('Insurance'),
('Customs');

-- -------------------------------------
-- Insert into `currency`
-- -------------------------------------
INSERT INTO `currency` (`code`) VALUES
('USD'),
('GBP'),
('AUD'),
('CAD'),
('EUR');

-- -------------------------------------
-- Insert into `contact_info`
-- -------------------------------------
INSERT INTO `contact_info` (`name`, `value`, `contact_type`) VALUES
('John Doe', 'john.doe@example.com', 'email'),
('Jane Smith', '+1-202-555-0173', 'phone'),
('Emily Davis', 'emily.davis@maritime.com', 'email'),
('Michael Brown', '+44-20-7946-0958', 'mobile'),
('Sarah Wilson', 'sarah.wilson@oceanic.com', 'email');

-- -------------------------------------
-- Link `address` with `contact_info`
-- -------------------------------------
INSERT INTO `address_has_contact_info` (`address_id`, `contact_info_id`) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5);

-- -------------------------------------
-- Link `contact_info` with `user`
-- -------------------------------------
INSERT INTO `contact_info_has_user` (`contact_info_id`, `user_id`) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5);

-- -------------------------------------
-- Link `owner` with `user`
-- -------------------------------------
INSERT INTO `owner_has_user` (`owner_id`, `operator_id`) VALUES
(1, 2),
(2, 3),
(3, 4),
(4, 5),
(5, 1);

-- -------------------------------------
-- Populate `order` and `box`
-- -------------------------------------

-- Let's create 50 orders for demonstration. You can scale this up as needed.

DELIMITER $$

CREATE PROCEDURE PopulateOrdersAndBoxes()
BEGIN
    DECLARE i INT DEFAULT 1;
    DECLARE total_orders INT DEFAULT 50;
    DECLARE supplier_count INT;
    DECLARE vessel_count INT;
    DECLARE warehouse_count INT;
    DECLARE status_count INT;

    SELECT COUNT(*) INTO supplier_count FROM supplier;
    SELECT COUNT(*) INTO vessel_count FROM vessel;
    SELECT COUNT(*) INTO warehouse_count FROM warehouse;
    SELECT COUNT(*) INTO status_count FROM order_status;

    WHILE i <= total_orders DO
        INSERT INTO `order` (
            `order_number`,
            `supplier_order_number`,
            `supplier_id`,
            `vessel_id`,
            `warehouse_id`,
            `expected_readiness`,
            `actual_readiness`,
            `expected_arrival`,
            `actual_arrival`,
            `order_status`
        ) VALUES (
            CONCAT('ORD-', LPAD(i, 5, '0')),
            CONCAT('SUPORD-', LPAD(i, 5, '0')),
            FLOOR(1 + RAND() * supplier_count),
            FLOOR(1 + RAND() * vessel_count),
            FLOOR(1 + RAND() * warehouse_count),
            DATE_ADD(NOW(), INTERVAL FLOOR(RAND() * 30) DAY),
            NULL,
            DATE_ADD(NOW(), INTERVAL FLOOR(RAND() * 60) DAY),
            NULL,
            (SELECT `status` FROM `order_status` ORDER BY RAND() LIMIT 1)
        );

        -- Get the last inserted order ID
        SET @last_order_id = LAST_INSERT_ID();

        -- Insert between 1 to 5 boxes per order
        SET @box_count = FLOOR(1 + RAND() * 5);
        SET @j = 1;

        WHILE @j <= @box_count DO
            INSERT INTO `box` (
                `id`,
                `length`,
                `width`,
                `height`,
                `weight`,
                `order_id`
            ) VALUES (
                UUID(),
                FLOOR(10 + RAND() * 100),  -- length in cm
                FLOOR(10 + RAND() * 100),  -- width in cm
                FLOOR(10 + RAND() * 100),  -- height in cm
                ROUND(1 + RAND() * 50, 2), -- weight in kg
                @last_order_id
            );
            SET @j = @j + 1;
        END WHILE;

        SET i = i + 1;
    END WHILE;
END$$

DELIMITER ;

-- Call the procedure to populate orders and boxes
CALL PopulateOrdersAndBoxes();

-- Drop the procedure as it's no longer needed
DROP PROCEDURE PopulateOrdersAndBoxes;

-- -------------------------------------
-- Populate `dispatch`, `invoice`, and related tables
-- -------------------------------------

-- Example: Create dispatches for some orders
INSERT INTO `dispatch` (
    `origin_type`,
    `origin_id`,
    `destination_type`,
    `destination_id`,
    `dispatch_status`,
    `transport_mode_type`,
    `tracking_number`,
    `dispatch_date`,
    `delivery_date`,
    `user_id`
) VALUES
('Warehouse', 1, 'Address', 1, 'Created', 'Sea', 'TRACK123456', NOW(), NULL, 2),
('Supplier', 2, 'Warehouse', 3, 'Sent', 'Air', 'TRACK234567', DATE_ADD(NOW(), INTERVAL -5 DAY), NULL, 3),
('Address', 3, 'Supplier', 4, 'Delivered', 'Courier', 'TRACK345678', DATE_ADD(NOW(), INTERVAL -10 DAY), DATE_ADD(NOW(), INTERVAL -2 DAY), 4);

-- Link dispatches with orders
INSERT INTO `dispatch_has_order` (`dispatch_id`, `order_id`) VALUES
(1, 1),
(1, 2),
(2, 3),
(3, 4);

-- Create invoices for dispatches
INSERT INTO `invoice` (`dispatch_id`) VALUES
(1),
(2),
(3);

-- Populate `financial` data
INSERT INTO `financial` (`invoice_id`, `cost_type_id`, `currency_id`) VALUES
(1, 1, 1),
(1, 2, 1),
(2, 3, 2),
(3, 4, 3);

-- -------------------------------------
-- Populate `vessel_at_port`
-- -------------------------------------

-- Example entries
INSERT INTO `vessel_at_port` (`vessel_id`, `port_id`, `arrival_date`, `departure_date`) VALUES
(1, 1, DATE_ADD(NOW(), INTERVAL -3 DAY), DATE_ADD(NOW(), INTERVAL -1 DAY)),
(2, 2, DATE_ADD(NOW(), INTERVAL -10 DAY), DATE_ADD(NOW(), INTERVAL -5 DAY)),
(3, 3, DATE_ADD(NOW(), INTERVAL -7 DAY), DATE_ADD(NOW(), INTERVAL -2 DAY)),
(4, 4, DATE_ADD(NOW(), INTERVAL -4 DAY), DATE_ADD(NOW(), INTERVAL -1 DAY)),
(5, 5, DATE_ADD(NOW(), INTERVAL -6 DAY), DATE_ADD(NOW(), INTERVAL -3 DAY));

-- -------------------------------------
-- Final Notes
-- -------------------------------------

-- You can extend the `PopulateOrdersAndBoxes` procedure to create more orders and boxes as needed.
-- Additionally, consider using tools or scripts (e.g., Python with Faker library) for generating larger datasets.

-- Ensure that all foreign key constraints are satisfied when inserting data.
