-- Disable foreign key checks to allow data insertion in any order
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;

USE sparehub;

-- -------------------------------
-- Insert Data into address
-- -------------------------------
INSERT INTO address (address, postal_code, country) VALUES
('123 Main Street', '10001', 'USA'),            -- id = 1
('456 Elm Street', '77001', 'USA'),             -- id = 2 (Houston)
('789 Oak Avenue', '10115', 'Germany'),         -- id = 3 (Berlin)
('321 Pine Road', 'SW1A 1AA', 'UK'),            -- id = 4 (London)
('654 Maple Drive', '1000', 'Belgium');         -- id = 5 (Brussels)

-- -------------------------------
-- Insert Data into supplier
-- -------------------------------
INSERT INTO supplier (name, address_id) VALUES
('Global Supplies Inc.', 1),                      -- id = 1
('Oceanic Parts Ltd.', 2),                        -- id = 2
('Berlin Spare Parts', 3),                        -- id = 3
('London Components', 4),                         -- id = 4
('Brussels Tools', 5);                            -- id = 5

-- -------------------------------
-- Insert Data into owner
-- -------------------------------
INSERT INTO owner (name) VALUES
('Maersk'),                                      -- id = 1
('Hellas Marine'),                                    -- id = 2
('Acme Shipping Co.'),                             -- id = 3
('Maritime Ventures'),                             -- id = 4
('SeaCorp International');                        -- id = 5

-- -------------------------------
-- Insert Data into vessel
-- -------------------------------
INSERT INTO vessel (owner_id, name, imo_number, flag) VALUES
(1, 'Voyager', '1234567', 'USA'),                 -- id = 1
(2, 'Explorer', '2345678', 'GBR'),                -- id = 2
(3, 'Navigator', '3456789', 'DEU'),               -- id = 3
(4, 'Mariner', '4567890', 'GBR'),                 -- id = 4
(5, 'Seafarer', '5678901', 'CAN');                -- id = 5

-- -------------------------------
-- Insert Data into agent
-- -------------------------------
INSERT INTO agent (name) VALUES
('Fast Logistics'),                                -- id = 1 (Amsterdam)
('Reliable Agents'),                               -- id = 2 (Houston)
('Swift Transport'),                               -- id = 3 (Berlin)
('Global Agents'),                                 -- id = 4 (London)
('Premier Shipping');                              -- id = 5 (Brussels)

-- -------------------------------
-- Insert Data into warehouse
-- -------------------------------
INSERT INTO warehouse (name, agent_id, address_id) VALUES
('Amsterdam Warehouse', 1, 1),   -- id = 1, address_id = 1
('Houston Warehouse', 2, 2),     -- id = 2, address_id = 2
('Berlin Warehouse', 3, 3),      -- id = 3, address_id = 3
('London Warehouse', 4, 4),      -- id = 4, address_id = 4
('Brussels Warehouse', 5, 5);    -- id = 5, address_id = 5


-- -------------------------------
-- Insert Data into role
-- -------------------------------
INSERT INTO role (title) VALUES
('Administrator'),                                  -- id = 1
('Operator'),                                       -- id = 2
('Viewer'),                                         -- id = 3
('Manager'),                                        -- id = 4,
('Owner');                                        -- id = 5

-- -------------------------------
-- Insert Data into user
-- -------------------------------
INSERT INTO user (role_id, name, email, password) VALUES
(1, 'Admin User', 'admin@sparehub.com', '$2a$10$4QxhJZO7Ml0AVNk5Gbf7MeMRoYx7HkNm7YgxB22aVITEcmXWR1ka2'),           -- id = 1
(2, 'Operator One', 'operator1@sparehub.com', '$2a$10$UzHeXh5gF7nH9wgG7BCpFe6fxW6a20bR0501Sp9oQ5MpQ7OY3ufX2'),     -- id = 2
(3, 'Viewer User', 'viewer@sparehub.com', '$2a$10$hIVOxIdJDM8z3DJOtUq.t.6Twmtd2yFRSoJ.qriHKYglV1M7R8rRC'),         -- id = 3
(4, 'Manager User', 'manager@sparehub.com', '$2a$10$3T8DXAbXWAoOS8x4NtHi7.p0ABw23rN2oyP480bcMaQBTV8Ap8Ulq'),       -- id = 4
(5, 'Owner User', 'owner@sparehub.com', '$2a$10$fbzkAgH3jaJWyKHwLNnnKeOAYYTOgy4OvKIn25ziW6cub75k3aZOm');       -- id = 5

-- -------------------------------
-- Insert Data into contact_info
-- -------------------------------
INSERT INTO contact_info (name, value, contact_type) VALUES
('John Doe', 'john.doe@sparehub.com', 'email'),                      -- id = 1
('Jane Smith', '+1234567890', 'phone'),                              -- id = 2
('Global Supplies Inc.', 'info@globalsupplies.com', 'email'),         -- id = 3
('Oceanic Parts Ltd.', '+0987654321', 'phone'),                       -- id = 4
('support@sparehub.com', 'support@sparehub.com', 'email');             -- id = 5

-- -------------------------------
-- Insert Data into address_has_contact_info
-- -------------------------------
INSERT INTO address_has_contact_info (address_id, contact_info_id) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5);

-- -------------------------------
-- Insert Data into contact_info_has_user
-- -------------------------------
INSERT INTO contact_info_has_user (contact_info_id, user_id) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5);

-- -------------------------------
-- Insert Data into currency
-- -------------------------------
INSERT INTO currency (code) VALUES
('USD'),                                           -- id = 1
('EUR'),                                           -- id = 2
('GBP'),                                           -- id = 3
('AUD'),                                           -- id = 4
('CAD');                                           -- id = 5

-- -------------------------------
-- Insert Data into cost_type
-- -------------------------------
INSERT INTO cost_type (type) VALUES
('Shipping'),                                      -- id = 1
('Handling'),                                      -- id = 2
('Storage'),                                       -- id = 3
('Customs'),                                       -- id = 4
('Insurance');                                     -- id = 5

-- -------------------------------
-- Insert Data into order_status
-- -------------------------------
INSERT INTO order_status (status) VALUES
('Pending'),
('Ready'),
('Inbound'),
('Stock'),
('Cancelled'),
('Delivered');

-- -------------------------------
-- Insert Data into `order`
-- -------------------------------
INSERT INTO `order` 
(order_number, supplier_order_number, supplier_id, vessel_id, warehouse_id, expected_readiness, actual_readiness, expected_arrival, actual_arrival, order_status) 
VALUES
('ORD001', 'SUP001', 1, 1, 1, '2024-12-01 10:00:00', '2024-12-02 12:00:00', '2024-12-05 08:00:00', '2024-12-05 09:00:00', 'Delivered'),   -- id = 1
('ORD002', 'SUP002', 2, 2, 2, '2024-12-03 11:00:00', NULL, '2024-12-06 09:00:00', NULL, 'Pending'),                     -- id = 2
('ORD003', 'SUP003', 3, 3, 3, '2024-12-04 12:00:00', NULL, '2024-12-07 10:00:00', NULL, 'Ready'),                       -- id = 3
('ORD004', 'SUP004', 4, 4, 4, '2024-12-05 13:00:00', NULL, '2024-12-08 11:00:00', NULL, 'Inbound'),                     -- id = 4
('ORD005', 'SUP005', 5, 5, 5, '2024-12-06 14:00:00', NULL, '2024-12-09 12:00:00', NULL, 'Cancelled');                 -- id = 5

-- -------------------------------
-- Insert Data into dispatch
-- -------------------------------
INSERT INTO dispatch 
(origin_type, origin_id, destination_type, destination_id, dispatch_status, transport_mode_type, tracking_number, dispatch_date, delivery_date, user_id) 
VALUES
('Warehouse', 1, 'Supplier', 1, 'Created', 'Sea', 'TRK123456', '2024-12-01 08:00:00', NULL, 1),      -- id = 1
('Supplier', 2, 'Warehouse', 2, 'Sent', 'Air', 'TRK234567', '2024-12-02 09:00:00', '2024-12-03 10:00:00', 2), -- id = 2
('Address', 3, 'Warehouse', 3, 'Delivered', 'Courier', 'TRK345678', '2024-12-03 10:00:00', '2024-12-04 12:00:00', 3), -- id = 3
('Warehouse', 4, 'Address', 4, 'Created', 'Sea', 'TRK456789', '2024-12-04 11:00:00', NULL, 4),      -- id = 4
('Supplier', 5, 'Warehouse', 5, 'Sent', 'Air', 'TRK567890', '2024-12-05 12:00:00', '2024-12-06 14:00:00', 5); -- id = 5

-- -------------------------------
-- Insert Data into dispatch_has_order
-- -------------------------------
INSERT INTO dispatch_has_order (dispatch_id, order_id) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5);

-- -------------------------------
-- Insert Data into invoice
-- -------------------------------
INSERT INTO invoice (dispatch_id) VALUES
(1),                                               -- id = 1
(2),                                               -- id = 2
(3),                                               -- id = 3
(4),                                               -- id = 4
(5);                                               -- id = 5

-- -------------------------------
-- Insert Data into financial
-- -------------------------------
INSERT INTO financial (invoice_id, cost_type_id, currency_id) VALUES
(1, 1, 1),                                         -- id = 1
(2, 2, 2),                                         -- id = 2
(3, 3, 3),                                         -- id = 3
(4, 4, 4),                                         -- id = 4
(5, 5, 5);                                         -- id = 5

-- -------------------------------
-- Insert Data into box
-- -------------------------------
INSERT INTO box (id, length, width, height, weight, order_id) VALUES
(UUID(), 30, 20, 15, 5.5, 1),
(UUID(), 25, 15, 10, 3.2, 2),
(UUID(), 40, 30, 20, 10.0, 3),
(UUID(), 50, 40, 25, 15.0, 4),
(UUID(), 35, 25, 20, 7.8, 5);

-- -------------------------------
-- Insert Data into port
-- -------------------------------
INSERT INTO port (name) VALUES
('Port of Amsterdam'),                              -- id = 1
('Port of Houston'),                                -- id = 2
('Port of Berlin'),                                 -- id = 3
('Port of London'),                                 -- id = 4
('Port of Brussels');                               -- id = 5

-- -------------------------------
-- Insert Data into vessel_at_port
-- -------------------------------
INSERT INTO vessel_at_port (vessel_id, port_id, arrival_date, departure_date) VALUES
(1, 1, '2024-11-01 08:00:00', '2024-11-02 18:00:00'), -- vessel_id = 1, port_id = 1
(2, 2, '2024-11-03 09:00:00', '2024-11-04 19:00:00'), -- vessel_id = 2, port_id = 2
(3, 3, '2024-11-05 10:00:00', '2024-11-06 20:00:00'), -- vessel_id = 3, port_id = 3
(4, 4, '2024-11-07 11:00:00', '2024-11-08 21:00:00'), -- vessel_id = 4, port_id = 4
(5, 5, '2024-11-09 12:00:00', '2024-11-10 22:00:00'); -- vessel_id = 5, port_id = 5

-- -------------------------------
-- Insert Data into owner_has_user
-- -------------------------------
INSERT INTO owner_has_user (owner_id, user_id) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5);

-- -------------------------------
-- Insert Data into operator
-- -------------------------------
INSERT INTO operator (name, title, user_id) VALUES
('Operator One', 'Senior Operator', 2),            -- id = 1
('Operator Two', 'Junior Operator', 3),            -- id = 2
('Operator Three', 'Lead Operator', 4),            -- id = 3
('Operator Four', 'Operator', 5),                  -- id = 4
('Operator Five', 'Assistant Operator', 1);        -- id = 5

-- -------------------------------
-- Insert 100 Auto-Generated Orders
-- -------------------------------
-- -------------------------------
-- Insert 100 Auto-Generated Orders with Boxes
-- -------------------------------
DELIMITER $$

CREATE PROCEDURE GenerateOrders()
BEGIN
    DECLARE i INT DEFAULT 6;
    DECLARE max_orders INT DEFAULT 105;
    DECLARE random_supplier INT;
    DECLARE random_vessel INT;
    DECLARE random_warehouse INT;
    DECLARE random_status VARCHAR(20); -- Changed from ENUM to VARCHAR for flexibility
    DECLARE base_date DATETIME DEFAULT '2024-12-07 10:00:00';
    
    -- Variables for Box Attributes
    DECLARE box_length INT;
    DECLARE box_width INT;
    DECLARE box_height INT;
    DECLARE box_weight DOUBLE;
    
    WHILE i <= max_orders DO
        -- Randomly select supplier_id, vessel_id, and warehouse_id between 1 and 5
        SET random_supplier = FLOOR(1 + (RAND() * 5));
        SET random_vessel = FLOOR(1 + (RAND() * 5));
        SET random_warehouse = FLOOR(1 + (RAND() * 5));
        
        -- Randomly select order status
        SET random_status = ELT(FLOOR(1 + RAND() * 6), 'Pending', 'Ready', 'Inbound', 'Stock', 'Cancelled', 'Delivered');
        
        -- Insert the new order
        INSERT INTO `order` 
        (
            order_number, 
            supplier_order_number, 
            supplier_id, 
            vessel_id, 
            warehouse_id, 
            expected_readiness, 
            actual_readiness, 
            expected_arrival, 
            actual_arrival, 
            order_status
        ) 
        VALUES
        (
            CONCAT('ORD', LPAD(i, 3, '0')),
            CONCAT('SUP', LPAD(i, 3, '0')),
            random_supplier,
            random_vessel,
            random_warehouse,
            DATE_ADD(base_date, INTERVAL i DAY),
            IF(random_status IN ('Ready', 'Inbound', 'Delivered'), DATE_ADD(base_date, INTERVAL i + 1 DAY), NULL),
            DATE_ADD(base_date, INTERVAL i + 4 DAY),
            IF(random_status = 'Delivered', DATE_ADD(base_date, INTERVAL i + 5 DAY), NULL),
            random_status
        );
        
        -- Check if the order status is not 'Pending' to generate a box
        IF random_status != 'Pending' THEN
            -- Generate random dimensions and weight for the box
            SET box_length = FLOOR(20 + (RAND() * 30));  -- Length between 20 and 50
            SET box_width = FLOOR(10 + (RAND() * 20));   -- Width between 10 and 30
            SET box_height = FLOOR(5 + (RAND() * 20));   -- Height between 5 and 25
            SET box_weight = ROUND(1 + (RAND() * 1099), 2); -- Weight between 1.00 and 20.00
            
            -- Insert the box associated with the current order
            INSERT INTO box 
            (
                id, 
                length, 
                width, 
                height, 
                weight, 
                order_id
            ) 
            VALUES
            (
                UUID(), 
                box_length, 
                box_width, 
                box_height, 
                box_weight, 
                i
            );
        END IF;
        
        SET i = i + 1;
    END WHILE;
END$$

DELIMITER ;

CALL GenerateOrders();

