import * as React from 'react';
import { useState, useEffect } from 'react';
import axios from "axios";
import { useParams } from "react-router-dom";
import { Table } from "../../interfaces/database";
import { Button, IconButton } from "@mui/material";

import EditIcon from "@mui/icons-material/Edit";
import SaveIcon from "@mui/icons-material/Save";
import CancelIcon from "@mui/icons-material/Cancel";

const MySqlTable: React.FC = () => {
    const { table } = useParams<{ table: string }>();
    const [tableData, setTableData] = useState<any[] | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [editRowId, setEditRowId] = useState<number | null>(null);
    const [formData, setFormData] = useState<Record<string, any>>({});

    const endpoint = tableApiMethodMapping(table);
    if (endpoint === null) {
        return <div>Table not implemented yet</div>;
    }
    useEffect(() => {
        // Fetch the table data from the backend
        axios.get(`${import.meta.env.VITE_API_URL}/${endpoint}`)
            .then(response => setTableData(response.data))
            .catch(err => setError(err.message));

    }, [table]);

    const handleEditClick = (row: any) => {
        setEditRowId(row.id); // Set the ID of the row being edited
        setFormData({ ...row }); // Copy the row's data into the form state
    };

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>, key: string) => {
        setFormData({
            ...formData,
            [key]: e.target.value, // Update the specific field in the form data
        });
    };

    const handleSaveClick = () => {
        // Call the backend API to save changes
        console.log("Saving data:", formData);

        // Simulate a successful save by updating local state
        setTableData((prev) => {
            if (!prev) return prev;
            const updatedData = prev.data.map((row) =>
                row.id === formData.id ? { ...formData } : row
            );
            return { ...prev, data: updatedData };
        });

        // Exit edit mode
        setEditRowId(null);
    };

    const handleCancelClick = () => {
        // Exit edit mode without saving changes
        setEditRowId(null);
        setFormData({});
    };

    return (
        <div>
            <h1>Table: {table}</h1>

            {error && <p style={{ color: "red" }}>{error}</p>}
            {tableData != null && tableData.length > 0 ? (
                <table>
                    <thead>
                    <tr>
                        {Object.keys(tableData[0]).map((key, index) => (
                            <th key={index}>{key}</th>
                        ))}
                        <th>Actions</th>
                    </tr>
                    </thead>
                    <tbody>
                    {tableData.map((row) => (
                        <tr key={row.id}>
                            {Object.entries(row).map(([key, value], index) => (
                                <td key={index}>
                                    {editRowId === row.id && key !== "id" ? (
                                        <input
                                            type="text"
                                            value={formData[key] || ""}
                                            onChange={(e) => handleInputChange(e, key)}
                                        />
                                    ) : (
                                        value.toString()
                                    )}
                                </td>
                            ))}
                            <td>
                                {editRowId === row.id ? (
                                    <>
                                        <IconButton onClick={handleSaveClick}>
                                            <SaveIcon />
                                        </IconButton>
                                        <IconButton onClick={handleCancelClick}>
                                            <CancelIcon />
                                        </IconButton>
                                    </>
                                ) : (
                                    <IconButton onClick={() => handleEditClick(row)}>
                                        <EditIcon />
                                    </IconButton>
                                )}
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            ) : (
                <p>Loading table...</p>
            )}
        </div>
    );
};

function tableApiMethodMapping(table: string | undefined): string | null{

    switch (table) {
        case "address":
            return "api/address/search";
        case "agent":
            return "api/agent/search";
        case "box":
            return "api/box/search";
        case "dispatch":
            return "api/dispatch";
        case "order":
            return "api/order";
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
export default MySqlTable;
