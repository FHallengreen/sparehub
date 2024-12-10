export interface Port {
    id: string;
    name: string;
    country: string;
    unlocode: string;
}


export interface PortDetail extends Port {
    createdAt?: string;
    updatedAt?: string;
}


export interface PortRequest {
    name: string;
    country: string;
    unlocode: string;
} 