import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { getOwners, deleteOwner } from './services/ownerService';

const OwnerList: React.FC = () => {
    const [owners, setOwners] = useState<any[]>([]);

    useEffect(() => {
        const fetchOwners = async () => {
            try {
                const response = await getOwners();
                setOwners(response.data);
            } catch (error) {
                console.error('Error fetching owners:', error);
            }
        };

        fetchOwners();
    }, []);

    const handleDelete = async (ownerId: string) => {
        try {
            await deleteOwner(ownerId);
            setOwners(owners.filter((owner) => owner.id !== ownerId));
        } catch (error) {
            console.error('Error deleting owner:', error);
        }
    };

    return (
        <div className="p-4">
            <h1 className="text-2xl font-bold mb-4">Owner List</h1>
            <Link to="/create-owner" className="text-blue-500">
                Create New Owner
            </Link>
            <ul className="mt-4 space-y-2">
                {owners.map((owner) => (
                    <li key={owner.id} className="p-2 border rounded shadow">
                        {owner.name} - {owner.email}
                        <div className="space-x-2">
                            <Link to={`/owner/${owner.id}`} className="text-blue-500">
                                View
                            </Link>
                            <Link to={`/owner/edit/${owner.id}`} className="text-yellow-500">
                                Edit
                            </Link>
                            <button
                                onClick={() => handleDelete(owner.id)}
                                className="text-red-500"
                            >
                                Delete
                            </button>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default OwnerList;
