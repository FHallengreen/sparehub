export interface Vessel {
    id: string;
    name: string;
    type: string;
    ownerId: string; // Assuming a relationship with the owner
    imoNumber: string;
}

// If you need a type for vessel details
export interface VesselDetail extends Vessel {
    createdAt?: string;
    updatedAt?: string;
}

// If you need a type for creating/updating vessels
export interface VesselRequest {
    name: string;
    type: string;
    ownerId: string;
    imoNumber: string;
} 