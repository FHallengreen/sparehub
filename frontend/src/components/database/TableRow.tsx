import React from "react";

interface TableRowProps {
    row: any;
    onEditObject: (object: any) => void;
    onCellClick?: (column: string) => void;
}

const TableRow: React.FC<TableRowProps> = ({ row, onEditObject, onCellClick }) => {
    return (
        <tr className="hover:bg-gray-100">
            {Object.entries(row).map(([key, value], index) => (
                <td
                    key={index}
                    onClick={() => onCellClick && onCellClick(key)} // Pass column to handler if clickable
                    className={`border border-gray-300 px-4 py-2 ${
                        onCellClick ? "cursor-pointer text-blue-600" : ""
                    }`}
                >
                    {typeof value === "object" && value !== null ? (
                        <button
                            className="text-blue-500 hover:text-blue-700"
                            onClick={() => onEditObject(value)}
                        >
                            {value.id}
                        </button>
                    ) : (
                        value?.toString()
                    )}
                </td>
            ))}
            <td className="border border-gray-300 px-4 py-2 text-center">
                <button
                    onClick={() => onEditObject(row)}
                    className="text-blue-500 hover:text-blue-700"
                >
                    ✏️
                </button>
            </td>
        </tr>
    );
};

export default TableRow;
