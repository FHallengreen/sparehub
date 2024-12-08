import React, { useState, useEffect } from 'react';
import { useParams } from "react-router-dom";
import TableComponent from './Table';
import EditModal from './EditModal';
import {supportedTables, tableApiMethodMapping} from "../../helpers/FrontendDatabase";
import api from "../../Api";

const MySqlTable: React.FC = () => {
    const { table } = useParams<{ table: string }>();
    const [tableData, setTableData] = useState<any[] | null>(null);
    const [modalOpen, setModalOpen] = useState(false);
    const [selectedObject, setSelectedObject] = useState<any>(null);
    const [entityRef, setEntityRef] = useState<string>();

    const endpoint = tableApiMethodMapping(table);

    if (!endpoint) {
        return <div>Table not implemented yet</div>;
    }

    const fetchData = async () => {
        try {
            const response = await api.get<any[]>(`${import.meta.env.VITE_API_URL}/api/${endpoint}`);
            setTableData(response.data);
            setEntityRef(endpoint);
        } catch (error) {
            console.error("Error fetching table data:", error);
        }
    };

    useEffect(() => {
        fetchData();
    }, [endpoint]);

    const handleEditObject = (object: any) => {
        setSelectedObject(object);
        setModalOpen(true);
    };

    const handleCellClick = (column: string) => {

        if (supportedTables.includes(column)) {
            setEntityRef(column)
        }


        // Additional logic based on the column or row can go here
    };

    const handleModalClose = () => {
        setModalOpen(false);
        setSelectedObject(null);
    };

    const handleSaveObject = async (updatedObject: any) => {

        console.log(updatedObject)
        try {
            const updateEndpoint = `${import.meta.env.VITE_API_URL}/api/${tableApiMethodMapping(entityRef)}/${updatedObject.id}`;
            await api.put(updateEndpoint, updatedObject);
            await fetchData();
            handleModalClose();
        } catch (error) {
            //console.error("Error saving object:", error);
            alert("Failed to save changes. Please try again.");
        }
    };

    return (
        <div>
            <h1>Table: {table}</h1>
            {tableData ? (
                <TableComponent
                    data={tableData}
                    onEditObject={handleEditObject}
                    onCellClick={handleCellClick} // Pass the cell click handler
                />
            ) : (
                <p>Loading table...</p>
            )}
            <EditModal
                open={modalOpen}
                object={selectedObject}
                onClose={handleModalClose}
                onSave={handleSaveObject}
            />
        </div>
    );
};

export default MySqlTable;
