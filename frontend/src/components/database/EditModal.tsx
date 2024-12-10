import React, { useState, useEffect, useRef } from "react";
import { supportedTables } from "../../helpers/FrontendDatabase.tsx";

interface EditModalProps {
    open: boolean;
    object: any;
    onClose: () => void;
    onSave: (updatedObject: any) => void;
}

const EditModal: React.FC<EditModalProps> = ({ open, object, onClose, onSave }) => {
    const [formData, setFormData] = useState<any>(object);
    const modalRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        if (object) {
            setFormData(object);
        }
    }, [object]);

    useEffect(() => {
        if (open) {
            const handleKeyDown = (e: KeyboardEvent) => {
                if (e.key === "Escape") {
                    onClose();
                }
            };
            document.addEventListener("keydown", handleKeyDown);

            if (modalRef.current) {
                modalRef.current.focus(); // Focus the modal container when it opens
            }

            return () => document.removeEventListener("keydown", handleKeyDown);
        }
    }, [open, onClose]);

    const handleInputChange = (key: string, value: string) => {
        setFormData((prev: any) => ({
            ...prev,
            [key]: value,
        }));
    };

    const handleSave = () => {
        const updatedObject = appendIdToForeignKeys(formData);
        onSave(updatedObject);
    };

    const appendIdToForeignKeys = (data: any) => {
        const updatedData: any = {};
        Object.entries(data).forEach(([key, value]) => {
            if (value && typeof value === "object" && value.id) {
                updatedData[`${key}Id`] = value.id;
            } else {
                updatedData[key] = value;
            }
        });
        return updatedData;
    };

    const renderFieldValue = (key: string, value: any) => {
        if (value && typeof value === "object" && value.id) {
            return value.id;
        }
        return value;
    };

    if (!open || !formData) return null;

    return (
        <div
            ref={modalRef}
            role="dialog"
            aria-modal="true"
            aria-labelledby="modal-title"
            tabIndex={0} // Makes the outer container focusable
            className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50"
            onClick={onClose}
            onKeyDown={(e) => {
                if (e.key === "Enter" || e.key === " ") {
                    onClose();
                }
            }}
        >
            <div
                role="document" // Indicates this is the modal's content
                aria-labelledby="modal-title"
                tabIndex={-1} // Allows programmatic focus
                className="bg-white rounded-lg shadow-lg p-6 w-full max-w-md"
                onClick={(e) => e.stopPropagation()} // Prevent close on modal content click
            >
                <h2 id="modal-title" className="text-xl font-bold text-gray-800 mb-4">
                    Edit Entry
                </h2>
                {Object.entries(formData).map(([key, value]) => (
                    <div key={key} className="mb-4">
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            {key}
                        </label>
                        <input
                            type="text"
                            className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                            value={renderFieldValue(key, value)}
                            onChange={(e) => handleInputChange(key, e.target.value)}
                            disabled={supportedTables.includes(key)}
                        />
                    </div>
                ))}
                <div className="flex justify-end space-x-2 mt-6">
                    <button
                        onClick={handleSave}
                        className="bg-blue-600 text-white px-4 py-2 rounded-md shadow hover:bg-blue-700"
                    >
                        Save
                    </button>
                    <button
                        onClick={onClose}
                        className="bg-gray-300 text-gray-700 px-4 py-2 rounded-md shadow hover:bg-gray-400"
                    >
                        Cancel
                    </button>
                </div>
            </div>
        </div>
    );
};

export default EditModal;
