import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { createOwner, getOwnerById, updateOwner } from "../Api";

const OwnerForm: React.FC = () => {
    const { ownerId } = useParams<{ ownerId: string }>();
    const navigate = useNavigate();
    const [formData, setFormData] = useState({
        name: "",
        email: "",
        phone: "",
    });

    useEffect(() => {
        if (ownerId) {
            const fetchOwner = async () => {
                try {
                    const response = await getOwnerById(ownerId);
                    setFormData(response.data);
                } catch (error) {
                    console.error("Error fetching owner:", error);
                }
            };
            fetchOwner();
        }
    }, [ownerId]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            if (ownerId) {
                await updateOwner(ownerId, formData);
            } else {
                await createOwner(formData);
            }
            navigate("/");
        } catch (error) {
            console.error("Error saving owner:", error);
        }
    };

    return (
        <form onSubmit={handleSubmit} className="p-4 space-y-4">
            <h1 className="text-xl font-bold">{ownerId ? "Edit Owner" : "Create Owner"}</h1>
            <input
                type="text"
                name="name"
                value={formData.name}
                onChange={handleChange}
                placeholder="Name"
                className="p-2 border rounded w-full"
                required
            />
            <input
                type="email"
                name="email"
                value={formData.email}
                onChange={handleChange}
                placeholder="Email"
                className="p-2 border rounded w-full"
                required
            />
            <input
                type="text"
                name="phone"
                value={formData.phone}
                onChange={handleChange}
                placeholder="Phone"
                className="p-2 border rounded w-full"
                required
            />
            <button
                type="submit"
                className="px-4 py-2 bg-blue-500 text-white rounded shadow"
            >
                {ownerId ? "Update Owner" : "Create Owner"}
            </button>
        </form>
    );
};

export default OwnerForm;
