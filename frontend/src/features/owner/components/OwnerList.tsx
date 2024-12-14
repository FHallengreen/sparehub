import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { getOwners } from '../../../api/ownerApi';
import { ownerColumns } from '../columns/OwnerColumns';
import OwnerGrid from './OwnerGrid';
import { GridRowSelectionModel } from '@mui/x-data-grid';
import { CircularProgress, Typography } from '@mui/material';

const OwnerList: React.FC = () => {
    const navigate = useNavigate();
    const [owners, setOwners] = useState<any[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [selectionModel, setSelectionModel] = useState<GridRowSelectionModel>([]);

    useEffect(() => {
        const fetchOwners = async () => {
            try {
                const response = await getOwners();
                setOwners(response);
            } catch (error) {
                console.error('Error fetching owners:', error);
                setError('Failed to fetch owners.');
            } finally {
                setLoading(false);
            }
        };

        fetchOwners();
    }, []);

    const handleRowDoubleClick = (params: any) => {
        navigate(`/owners/${params.row.id}`);
    };

    return (
        <div className="p-4">
            <h1 className="text-2xl font-bold mb-4">Owner List</h1>
            <Link to="/owners/new" className="text-blue-500">Create New Owner</Link>
            {loading ? (
                <CircularProgress />
            ) : error ? (
                <Typography color="error">{error}</Typography>
            ) : (
                <OwnerGrid
                    rows={owners}
                    columns={ownerColumns}
                    loading={loading}
                    error={error}
                    selectionModel={selectionModel}
                    onRowSelectionModelChange={setSelectionModel}
                    onRowDoubleClick={handleRowDoubleClick}
                />
            )}
        </div>
    );
};

export default OwnerList;