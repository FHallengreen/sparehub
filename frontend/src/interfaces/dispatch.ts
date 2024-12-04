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
}
