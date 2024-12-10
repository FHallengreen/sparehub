import React, { useState } from "react";
import { Link } from "react-router-dom";
import { getForeignKeyTable } from "../../helpers/FrontendDatabase.tsx";

interface CreateModalProps {
    open: boolean;
    initialData: Record<string, any> | null;
    onClose: () => void;
    onSave: (data: any) => void;
    table: string;
}

const CreateModal: React.FC<CreateModalProps> = ({
                                                     open,
                                                     initialData,
                                                     onClose,
                                                     onSave,
                                                     table,
                                                 }) => {
    const [formData, setFormData] = useState<Record<string, any>>(initialData || {});

    if (!open) return null;

    const handleInputChange = (field: string, value: string) => {
        setFormData((prev) => ({
            ...prev,
            [field]: value,
        }));
    };

    const isForeignKeyField = (field: string): boolean => {
        return field.endsWith("Id");
    };

    const handleSubmit = () => {
        onSave(formData);
    };

    return (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
            <div className="bg-white rounded-lg shadow-lg p-6 w-full max-w-md overflow-y-auto">
                <h2 className="text-xl font-semibold text-gray-800 mb-4">
                    Create New {table}
                </h2>
                <form className="space-y-4">
                    {initialData &&
                        Object.keys(initialData).map((field) => (
                            <div key={field} className="space-y-1">
                                <label
                                    className="block text-sm font-medium text-gray-700"
                                    htmlFor={field}
                                >
                                    {field}
                                </label>
                                <input
                                    id={field}
                                    type="text"
                                    value={formData[field] || ""}
                                    onChange={(e) => handleInputChange(field, e.target.value)}
                                    className="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                                />
                                {isForeignKeyField(field) && (
                                    <div className="mt-1">
                                        <Link
                                            to={`/database/${getForeignKeyTable(field)}`}
                                            target="_blank"
                                            className="text-blue-600 hover:text-blue-800 text-sm underline"
                                        >
                                            Go to {getForeignKeyTable(field)} Table
                                        </Link>
                                    </div>
                                )}
                            </div>
                        ))}
                </form>
                <div className="mt-6 flex justify-end space-x-3">
                    <button
                        onClick={onClose}
                        className="px-4 py-2 text-sm font-medium text-gray-700 bg-gray-200 hover:bg-gray-300 rounded-md"
                    >
                        Cancel
                    </button>
                    <button
                        onClick={handleSubmit}
                        className="px-4 py-2 text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 rounded-md"
                    >
                        Save
                    </button>
                </div>
            </div>
        </div>
    );
};

export default CreateModal;
