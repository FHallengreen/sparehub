import React from 'react';
import TableRow from './TableRow';

interface TableComponentProps {
    data: any[];
    onEditObject: (object: any) => void;
    onCellClick?: (column: string) => void; // Optional handler for cell clicks
}

const TableComponent: React.FC<TableComponentProps> = ({ data, onEditObject, onCellClick }) => {
    const headers = data.length > 0 ? Object.keys(data[0]) : [];

    return (
        <table>
            <thead>
            <tr>
                {headers.map((key) => (
                    <th key={key}>{key}</th>
                ))}
            </tr>
            </thead>
            <tbody>
            {data.map((row) => (
                <TableRow
                    key={row.id}
                    row={row}
                    onEditObject={onEditObject}
                    onCellClick={onCellClick} // Pass the handler to TableRow
                />
            ))}
            </tbody>
        </table>
    );
};

export default TableComponent;
