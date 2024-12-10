export interface Owner {
    id: string;
    name: string;
    email: string;
    phone: string;
    // Add any other properties that your owner object might have
}

// If you need a type for creating a new owner (without id)
export interface OwnerRequest {
    name: string;
    email: string;
    phone: string;
    // Add any other properties needed for creating an owner
}

// If you need a type for owner details
export interface OwnerDetail extends Owner {
    // Add any additional properties that might be needed for detailed view
    createdAt?: string;
    updatedAt?: string;
}
