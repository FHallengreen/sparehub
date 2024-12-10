export const supportedTables = [
    'address',
    'agent',
    'port',
    'supplier',
    'warehouse',
    'vessel',
    'owner'
]

export function tableApiMethodMapping(table: string | undefined): string | null{

    switch (table) {
        case "address":
            return "address";
        case "agent":
            return "agent";
        case "port":
            return "port";
        case "warehouse":
            return "warehouse";
        case "supplier":
            return "supplier";
        case "vessel":
            return "vessel";
        case "owner":
            return "owner";
        default:
            return null;
    }
}

export function getForeignKeyTable(field: string): string | null{

    // Map foreign key fields to corresponding table names.
    switch (field) {
        case "addressId":
            return "address";
        case "agentId":
            return "agent";
        case "warehouseId":
            return "warehouse";
        case "vesselId":
            return "vessel";
        case "supplierId":
            return "supplier";
        case "portId":
            return "port";
        case "ownerId":
            return "owner";

        default:
            return "";
    }
}

//Function to return the form format of the specified table when creating a new entry
export function getCreateModalInitialData(table: string)  {

    switch (table) {
        case 'address':
            return {
                addressLine: '',
                postalCode: '',
                country: ''
            }
        case 'agent':
            return {
                name: '',
            }
        case 'warehouse':
            return {
                name: '',
                addressId: '',
                agentId: '',
            }
        case 'vessel':
            return {
                name: '',
                imoNumber: '',
                flag: '',
                ownerId: ''
            }
        case 'owner':
            return {
                name: '',
            }
        case 'supplier':
            return {
                name: '',
                addressId: '',
            }
        default:
            return null;

    }
}