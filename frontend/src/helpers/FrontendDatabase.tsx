export const supportedTables = [
    'address',
    'agent',
    'port',
    'supplier',
    'warehouse',
    'vessel'
]

export function tableApiMethodMapping(table: string | undefined): string | null{

    switch (table) {
        case "address":
            return "api/address/search";
        case "agent":
            return "api/agent/search";
        case "port":
            return "api/port";
        case "warehouse":
            return "api/warehouse/search";
        case "supplier":
            return "api/supplier";
        case "vessel":
            return "api/vessel";
        default:
            return null;
    }
}