import { Vessel } from "./order";

export interface VesselAtPort {
    portName: string; // Name of the Port
    portId: string; // Reference to the Port
    vessels: Vessel[]; // Vessel details
    arrivalDate: string; // Date when the vessel arrived at the port
    departureDate?: string; // Date when the vessel departed from the port
}

// If you need a type for vessel-at-port details
export interface VesselAtPortDetail extends VesselAtPort {
    vessels: Vessel[]; // Vessel details
    portId: string; // Reference to the Port
    arrivalDate: string; // Date when the vessel arrived at the port
    departureDate?: string; // Date when the vessel departed from the port
}

// If you need a type for creating/updating vessel-at-port records
export interface VesselAtPortRequest {
    vesselId: string;
    portId: string;
    arrivalDate: string;
    departureDate?: string;
} 