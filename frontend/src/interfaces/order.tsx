export interface Box {
    length: number;
    width: number;
    height: number;
    weight: number;
  }
  
  export interface OrderDetail {
    id: number;
    orderNumber: string;
    supplierOrderNumber: string;
    supplier: Supplier;
    vessel: Vessel;
    warehouse: Warehouse;
    agent: Agent;
    orderStatus: string;
    owner: Owner;
    expectedReadiness: Date;
    actualReadiness?: Date;
    expectedArrival?: Date;
    actualArrival?: Date;
    boxes: Box[] | null;
  }
  
  export interface Order {
    id: number;
    supplier: Supplier;
    vessel: Vessel;
    orderNumber: string;
    warehouse: Warehouse;
    orderStatus: string;
    owner: Owner;
    totalWeight: number;
    boxes: number;
  }
  
  export interface Supplier {
    id: number;
    name: string;
  }
  
  export interface Vessel {
    id: number;
    name: string;
  }
  
  export interface Warehouse {
    id: number;
    name: string;
  }
  
  export interface Owner {
    id: number;
    name: string;
  }
  
  export interface Agent {
    id: number;
    name: string;
  }
  