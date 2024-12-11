export interface VesselAtPort {
    id: string;
    vesselId: string; // Reference to the Vessel
    portId: string; // Reference to the Port
    arrivalDate: string; // Date when the vessel arrived at the port
    departureDate?: string; // Date when the vessel departed from the port
    status: string; // Status of the vessel at the port (e.g., "Docked", "Departed")
}

// If you need a type for vessel-at-port details
export interface VesselAtPortDetail extends VesselAtPort {
    vesselId: string; // Reference to the Vessel
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