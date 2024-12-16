import { Vessel } from "./order";

export interface VesselAtPort {
    portName: string;
    portId: string;
    vessels: Vessel[];
    arrivalDate: string;
    departureDate?: string;
}


export interface VesselAtPortDetail extends VesselAtPort {
    vessels: Vessel[];
    portId: string;
    arrivalDate: string;
    departureDate?: string;
}


export interface VesselAtPortRequest {
    vesselId: string;
    portId: string;
    arrivalDate: string;
    departureDate?: string;
} 