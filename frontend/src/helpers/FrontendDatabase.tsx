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
        default:
            return null;
    }
}