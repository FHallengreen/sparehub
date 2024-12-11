import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";

import api from "../../api/api";
import TableComponent from "./Table";
import EditModal from "./EditModal";
import CreateModal from "./CreateModal.tsx";
import {
    supportedTables,
    tableApiMethodMapping,
    getCreateModalInitialData,
} from "../../helpers/FrontendDatabase";

const MySqlTable: React.FC = () => {
    const { table } = useParams<{ table: string }>();
    const [tableData, setTableData] = useState<any[] | null>(null);
    const [modalOpen, setModalOpen] = useState(false);
    const [createModalOpen, setCreateModalOpen] = useState(false);
    const [selectedObject, setSelectedObject] = useState<any>(null);
    const [entityRef, setEntityRef] = useState<string>();
    const baseUrl = import.meta.env.VITE_API_URL;

    const endpoint = tableApiMethodMapping(table);

    const fetchData = async () => {
        try {
            const response = await api.get<any[]>(`${baseUrl}/api/${endpoint}`);
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
            setEntityRef(column);
        }
    };

    const handleModalClose = () => {
        setModalOpen(false);
        setSelectedObject(null);
    };

    const handleCreateModalOpen = () => {
        setCreateModalOpen(true);
    };

    const handleCreateModalClose = () => {
        setCreateModalOpen(false);
    };

    const handleSaveObject = async (updatedObject: any) => {
        try {
            const updateEndpoint = `${baseUrl}/api/${tableApiMethodMapping(entityRef)}/${updatedObject.id}`;
            await api.put(updateEndpoint, updatedObject);
            await fetchData();
            handleModalClose();
        } catch (error) {
            alert("Failed to save changes. Please try again.");
        }
    };

    const handleCreateSave = async (newObject: any) => {
        try {
            const createEndpoint = `${baseUrl}/api/${endpoint}`;
            await api.post(createEndpoint, newObject);
            await fetchData();
            handleCreateModalClose();
        } catch (error) {
            alert("Failed to create a new entry. Please try again.");
        }
    };

    return (
        <div className="p-6">
            <h1 className="text-2xl font-bold text-gray-800 mb-6">Table: {table}</h1>
            {tableData ? (
                <>
                    <TableComponent
                        data={tableData}
                        onEditObject={handleEditObject}
                        onCellClick={handleCellClick}
                    />
                    <div className="mt-4 text-right">
                        <button
                            onClick={handleCreateModalOpen}
                            className="px-6 py-2 text-white bg-blue-600 hover:bg-blue-700 rounded-md shadow-md font-medium"
                        >
                            Create New Entry
                        </button>
                    </div>
                </>
            ) : (
                <p className="text-gray-500 text-center">Loading table...</p>
            )}
            <EditModal
                open={modalOpen}
                object={selectedObject}
                onClose={handleModalClose}
                onSave={handleSaveObject}
            />
            <CreateModal
                open={createModalOpen}
                initialData={getCreateModalInitialData(table)}
                onClose={handleCreateModalClose}
                onSave={handleCreateSave}
                table={table}
            />
        </div>
    );
};

export default MySqlTable;
