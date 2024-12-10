export interface Port {
    id: string;
    name: string;
    country: string;
    unlocode: string;
}


export interface PortDetail extends Port {
    id: string;
    name: string;
    country: string;
    unlocode: string;
}


export interface PortRequest {
    name: string;
    country: string;
    unlocode: string;
} 