export interface Box {
  id?: number;
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
  orderStatus: string;
  expectedReadiness?: Date | null;
  actualReadiness?: Date | null;
  expectedArrival?: Date | null;
  actualArrival?: Date | null;
  boxes: Box[] | null;
  transporter: string;
  trackingNumber: string;
}



export interface OrderRequest {
  orderNumber: string;
  supplierOrderNumber?: string;
  expectedReadiness: Date;
  actualReadiness?: Date;
  expectedArrival?: Date;
  actualArrival?: Date;
  supplierId: number;
  vesselId: number;
  warehouseId: number;
  orderStatus: string;
  boxes?: Box[];
  transporter?: string;
  trackingNumber?: string;
}


export interface Order {
  id: number;
  supplierName: Supplier;
  vesselName: Vessel;
  orderNumber: string;
  warehouseName: Warehouse;
  orderStatus: string;
  ownerName: Owner;
  totalWeight: number;
  boxes: number;
  totalVolume: number;
}

export interface Supplier {
  id: number;
  name: string;
}

export interface VesselOption {
  id: number;
  name: string;
  owner: Owner;
}

export interface SupplierOption {
  id: number;
  name: string;
}


export interface Vessel {
  id: number;
  name: string;
  owner: Owner;
  imoNumber?: string;
  flag?: string;
}


export interface Warehouse {
  id: number;
  name: string;
  agent?: Agent | null;
}


export interface Owner {
  id: number;
  name: string;
}

export interface Agent {
  id: number;
  name: string;
}

export interface OrderRowBase {
  id: string;
  stockLocation: string;
  isGroupHeader?: boolean;
}

export interface OrderRowData extends OrderRowBase {
  pieces: number | null;
  weight: number | null;
  volume: number | null;
  status: string;
  owner: string;
  vessel: string;
  supplier: string;
  poNumber: string;
}

export type OrderRow = OrderRowData | OrderRowBase;


export interface StockLocationSummary {
  orders: number;
  pieces: number;
  weight: number;
  volume: number;
  volumetricWeight: number;
}

export interface LoginRequest {
  email: string,
  password: string,
}

export interface LoginResponse {
  token: string;
  user: {
    id: number;
    email: string;
    name: string;
    role: string;
  }
}
