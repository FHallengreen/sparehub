export interface Port {
    id: string;
    name: string;
    country: string;
    unlocode: string;
}

// If you need a type for port details
export interface PortDetail extends Port {
    createdAt?: string;
    updatedAt?: string;
}

// If you need a type for creating/updating ports
export interface PortRequest {
    name: string;
    country: string;
    unlocode: string;
} 