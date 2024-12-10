import { Owner } from './owner';

export interface Vessel {
    id: string;
    owner_id: string;
    name: string;
    imoNumber: string;
    flag: string;
    owner?: Owner;
}

export interface VesselDetail extends Vessel {
    createdAt?: string;
    updatedAt?: string;
}

export interface VesselRequest {
    owner_id: string;
    name: string;
    imoNumber: string;
    flag: string;
} 