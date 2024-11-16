-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema sparehub
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema sparehub
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `sparehub` DEFAULT CHARACTER SET utf8 ;
USE `sparehub` ;

-- -----------------------------------------------------
-- Table `sparehub`.`address`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`address` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `address` VARCHAR(100) NOT NULL,
  `postal_code` VARCHAR(10) NOT NULL,
  `country` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`supplier`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`supplier` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `address_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Supplier_Address1_idx` (`address_id` ASC) VISIBLE,
  CONSTRAINT `fk_Supplier_Address1`
    FOREIGN KEY (`address_id`)
    REFERENCES `sparehub`.`address` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`owner`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`owner` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`vessel`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`vessel` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `owner_id` INT NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  `imo_number` VARCHAR(7) NULL,
  `flag` VARCHAR(3) NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Vessel_Owner1_idx` (`owner_id` ASC) VISIBLE,
  CONSTRAINT `fk_Vessel_Owner1`
    FOREIGN KEY (`owner_id`)
    REFERENCES `sparehub`.`owner` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`agent`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`agent` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`warehouse`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`warehouse` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `agent_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Warehouse_Agent1_idx` (`agent_id` ASC) VISIBLE,
  CONSTRAINT `fk_Warehouse_Agent1`
    FOREIGN KEY (`agent_id`)
    REFERENCES `sparehub`.`agent` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`order_status`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`order_status` (
  `status` ENUM('Pending', 'Ready', 'Inbound', 'Stock', 'Cancelled') NOT NULL,
  PRIMARY KEY (`status`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`order`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`order` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `order_number` VARCHAR(45) NOT NULL,
  `supplier_order_number` VARCHAR(45) NULL,
  `supplier_id` INT NOT NULL,
  `vessel_id` INT NOT NULL,
  `warehouse_id` INT NOT NULL,
  `expected_readiness` DATETIME NOT NULL,
  `actual_readiness` DATETIME NULL,
  `expected_arrival` DATETIME NULL,
  `actual_arrival` DATETIME NULL,
  `order_status` ENUM('Pending', 'Ready', 'Inbound', 'Stock', 'Cancelled') NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Order_Supplier_idx` (`supplier_id` ASC) VISIBLE,
  INDEX `fk_Order_Vessel1_idx` (`vessel_id` ASC) VISIBLE,
  INDEX `fk_Order_Warehouse1_idx` (`warehouse_id` ASC) VISIBLE,
  INDEX `fk_order_order_status1_idx` (`order_status` ASC) VISIBLE,
  CONSTRAINT `fk_Order_Supplier`
    FOREIGN KEY (`supplier_id`)
    REFERENCES `sparehub`.`supplier` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Order_Vessel1`
    FOREIGN KEY (`vessel_id`)
    REFERENCES `sparehub`.`vessel` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Order_Warehouse1`
    FOREIGN KEY (`warehouse_id`)
    REFERENCES `sparehub`.`warehouse` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_order_order_status1`
    FOREIGN KEY (`order_status`)
    REFERENCES `sparehub`.`order_status` (`status`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`role`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`role` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`user`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`user` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `role_id` INT NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Operator_Role1_idx` (`role_id` ASC) VISIBLE,
  CONSTRAINT `fk_Operator_Role1`
    FOREIGN KEY (`role_id`)
    REFERENCES `sparehub`.`role` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`dispatch_status`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`dispatch_status` (
  `status` ENUM('Created', 'Sent', 'Delivered') NOT NULL,
  PRIMARY KEY (`status`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`transport_mode`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`transport_mode` (
  `type` ENUM('Air', 'Sea', 'Courier') NOT NULL,
  PRIMARY KEY (`type`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`dispatch`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`dispatch` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `origin_type` ENUM('Warehouse', 'Supplier', 'Address') NOT NULL,
  `origin_id` INT NOT NULL,
  `destination_type` ENUM('Warehouse', 'Supplier', 'Address') NOT NULL,
  `destination_id` INT NULL,
  `dispatch_status` ENUM('Created', 'Sent', 'Delivered') NOT NULL,
  `transport_mode_type` ENUM('Air', 'Sea', 'Courier') NOT NULL,
  `tracking_number` VARCHAR(45) NULL,
  `dispatch_date` DATETIME NULL,
  `delivery_date` DATETIME NULL,
  `user_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_dispatch_dispatch_status1_idx` (`dispatch_status` ASC) VISIBLE,
  INDEX `fk_dispatch_transport_mode1_idx` (`transport_mode_type` ASC) VISIBLE,
  INDEX `fk_dispatch_user1_idx` (`user_id` ASC) VISIBLE,
  CONSTRAINT `fk_dispatch_dispatch_status1`
    FOREIGN KEY (`dispatch_status`)
    REFERENCES `sparehub`.`dispatch_status` (`status`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_dispatch_transport_mode1`
    FOREIGN KEY (`transport_mode_type`)
    REFERENCES `sparehub`.`transport_mode` (`type`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_dispatch_user1`
    FOREIGN KEY (`user_id`)
    REFERENCES `sparehub`.`user` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`invoice`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`invoice` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `dispatch_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Invoice_Dispatch1_idx` (`dispatch_id` ASC) VISIBLE,
  CONSTRAINT `fk_Invoice_Dispatch1`
    FOREIGN KEY (`dispatch_id`)
    REFERENCES `sparehub`.`dispatch` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`dispatch_has_order`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`dispatch_has_order` (
  `dispatch_id` INT NOT NULL,
  `order_id` INT NOT NULL,
  PRIMARY KEY (`order_id`, `dispatch_id`),
  INDEX `fk_Dispatch_has_Order_Order1_idx` (`order_id` ASC) VISIBLE,
  INDEX `fk_Dispatch_has_Order_Dispatch1_idx` (`dispatch_id` ASC) VISIBLE,
  CONSTRAINT `fk_Dispatch_has_Order_Dispatch1`
    FOREIGN KEY (`dispatch_id`)
    REFERENCES `sparehub`.`dispatch` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Dispatch_has_Order_Order1`
    FOREIGN KEY (`order_id`)
    REFERENCES `sparehub`.`order` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`owner_has_user`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`owner_has_user` (
  `owner_id` INT NOT NULL,
  `operator_id` INT NOT NULL,
  PRIMARY KEY (`owner_id`, `operator_id`),
  INDEX `fk_owner_has_user_user1_idx` (`operator_id` ASC) VISIBLE,
  INDEX `fk_owner_has_user_owner1_idx` (`owner_id` ASC) VISIBLE,
  CONSTRAINT `fk_owner_has_user_owner1`
    FOREIGN KEY (`owner_id`)
    REFERENCES `sparehub`.`owner` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_owner_has_user_user1`
    FOREIGN KEY (`operator_id`)
    REFERENCES `sparehub`.`user` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`contact_info`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`contact_info` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NULL,
  `value` VARCHAR(45) NULL,
  `contact_type` ENUM('phone', 'email', 'mobile') NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`address_has_contact_info`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`address_has_contact_info` (
  `address_id` INT NOT NULL,
  `contact_info_id` INT NOT NULL,
  PRIMARY KEY (`address_id`, `contact_info_id`),
  INDEX `fk_address_has_contact_info_contact_info1_idx` (`contact_info_id` ASC) VISIBLE,
  INDEX `fk_address_has_contact_info_address1_idx` (`address_id` ASC) VISIBLE,
  CONSTRAINT `fk_address_has_contact_info_address1`
    FOREIGN KEY (`address_id`)
    REFERENCES `sparehub`.`address` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_address_has_contact_info_contact_info1`
    FOREIGN KEY (`contact_info_id`)
    REFERENCES `sparehub`.`contact_info` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`contact_info_has_user`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`contact_info_has_user` (
  `contact_info_id` INT NOT NULL,
  `user_id` INT NOT NULL,
  PRIMARY KEY (`contact_info_id`, `user_id`),
  INDEX `fk_contact_info_has_user_user1_idx` (`user_id` ASC) VISIBLE,
  INDEX `fk_contact_info_has_user_contact_info1_idx` (`contact_info_id` ASC) VISIBLE,
  CONSTRAINT `fk_contact_info_has_user_contact_info1`
    FOREIGN KEY (`contact_info_id`)
    REFERENCES `sparehub`.`contact_info` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_contact_info_has_user_user1`
    FOREIGN KEY (`user_id`)
    REFERENCES `sparehub`.`user` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`cost_type`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`cost_type` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `type` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`currency`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`currency` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `code` VARCHAR(3) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`financial`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`financial` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `invoice_id` INT NOT NULL,
  `cost_type_id` INT NOT NULL,
  `currency_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_financial_invoice1_idx` (`invoice_id` ASC) VISIBLE,
  INDEX `fk_financial_cost_type1_idx` (`cost_type_id` ASC) VISIBLE,
  INDEX `fk_financial_currency1_idx` (`currency_id` ASC) VISIBLE,
  CONSTRAINT `fk_financial_invoice1`
    FOREIGN KEY (`invoice_id`)
    REFERENCES `sparehub`.`invoice` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_financial_cost_type1`
    FOREIGN KEY (`cost_type_id`)
    REFERENCES `sparehub`.`cost_type` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_financial_currency1`
    FOREIGN KEY (`currency_id`)
    REFERENCES `sparehub`.`currency` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`box`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`box` (
  `id` CHAR(36) NOT NULL,
  `length` INT NOT NULL,
  `width` INT NOT NULL,
  `height` INT NOT NULL,
  `weight` DOUBLE NOT NULL,
  `order_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_box_order1_idx` (`order_id` ASC) VISIBLE,
  CONSTRAINT `fk_box_order1`
    FOREIGN KEY (`order_id`)
    REFERENCES `sparehub`.`order` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`port`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`port` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sparehub`.`vessel_at_port`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sparehub`.`vessel_at_port` (
  `vessel_id` INT NOT NULL,
  `port_id` INT NOT NULL,
  `arrival_date` DATETIME NULL,
  `departure_date` DATETIME NULL,
  PRIMARY KEY (`vessel_id`, `port_id`),
  INDEX `fk_vessel_has_port_port1_idx` (`port_id` ASC) VISIBLE,
  INDEX `fk_vessel_has_port_vessel1_idx` (`vessel_id` ASC) VISIBLE,
  INDEX `arrival_departure_dates_idx` (`arrival_date` ASC, `departure_date` ASC) VISIBLE,
  CONSTRAINT `fk_vessel_has_port_vessel1`
    FOREIGN KEY (`vessel_id`)
    REFERENCES `sparehub`.`vessel` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_vessel_has_port_port1`
    FOREIGN KEY (`port_id`)
    REFERENCES `sparehub`.`port` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
