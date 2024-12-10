import React from "react";
import TableRow from "./TableRow";

interface TableComponentProps {
    data: any[];
    onEditObject: (object: any) => void;
    onCellClick?: (column: string) => void;
}

const TableComponent: React.FC<TableComponentProps> = ({
                                                           data,
                                                           onEditObject,
                                                           onCellClick,
                                                       }) => {
    const headers = data.length > 0 ? Object.keys(data[0]) : [];

    return (
        <div className="overflow-x-auto">
            <table className="table-auto w-full border-collapse border border-gray-200">
                <thead>
                <tr className="bg-gray-100">
                    {headers.map((key) => (
                        <th
                            key={key}
                            className="border border-gray-300 px-4 py-2 text-left text-gray-700 font-medium"
                        >
                            {key}
                        </th>
                    ))}
                </tr>
                </thead>
                <tbody>
                {data.map((row) => (
                    <TableRow
                        key={row.id}
                        row={row}
                        onEditObject={onEditObject}
                        onCellClick={onCellClick}
                    />
                ))}
                </tbody>
            </table>
        </div>
    );
};

export default TableComponent;
