export interface Dispatch {
  id: string;
  originType: string;
  originId: number;
  destinationType: string;
  destinationId?: number | null;
  dispatchStatus: string;
  transportModeType: string;
  trackingNumber?: string | null;
  dispatchDate?: Date | null;
  deliveryDate?: Date | null;
  userId: number;
  orderIds: number[];
}

export interface DispatchDetail {
  id: number;
  originType: string;
  originId: number;
  destinationType: string;
  destinationId: number | null;
  dispatchStatus: string;
  transportModeType: string;
  trackingNumber: string | null;
  dispatchDate: Date | null;
  deliveryDate: Date | null;
  userId: number;
  orderNumbers: string[];
}

export interface DispatchRequest {
  destinationType: 'Warehouse' | 'Vessel' | 'Supplier' | 'Address' | null;
  destinationId: number | null;
  transportModeType: 'Air' | 'Sea' | 'Courier';
  status: 'Created' | 'Sent' | 'Delivered';
  userId: number;
  orderIds: number[];
}
