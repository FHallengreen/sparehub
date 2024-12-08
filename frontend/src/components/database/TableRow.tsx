import React from 'react';
import { Button, IconButton } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';

interface TableRowProps {
    row: any;
    onEditObject: (object: any) => void;
    onCellClick?: (column: string) => void; // Optional handler for cell clicks
}

const TableRow: React.FC<TableRowProps> = ({ row, onEditObject, onCellClick }) => {
    return (
        <tr>
            {Object.entries(row).map(([key, value], index) => (
                <td
                    key={index}
                    onClick={() => onCellClick && onCellClick(key)} // Pass column and row to handler
                    style={{ cursor: onCellClick ? 'pointer' : 'default' }} // Add a pointer cursor if clickable
                >
                    {typeof value === 'object' && value !== null ? (
                        <Button onClick={() => onEditObject(value)}>
                            {value.id}
                        </Button>
                    ) : (
                        value?.toString()
                    )}
                </td>
            ))}
            <td>
                <IconButton onClick={() => onEditObject(row)}>
                    <EditIcon />
                </IconButton>
            </td>
        </tr>
    );
};

export default TableRow;
