export interface Owner {
    id: number;
    name: string;
}


export interface OwnerRequest {
    name: string;
}


export interface OwnerDetail extends Owner {
    createdAt?: string;
    updatedAt?: string;
}
