import React, { useEffect, useState } from 'react';
import OwnerGrid from '../components/OwnerGrid';
import OwnerFilter from '../components/OwnerFilter';
import { getOwners } from '../../../api/ownerApi';
import { ownerColumns } from '../columns/OwnerColumns';
import { Owner } from '../../../interfaces/owner';
import { useNavigate } from 'react-router-dom';
import { Typography } from '@mui/material';
import { GridRowSelectionModel } from '@mui/x-data-grid';

const OwnerPage: React.FC = () => {
    const navigate = useNavigate();
    const [owners, setOwners] = useState<Owner[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [selectionModel, setSelectionModel] = useState<GridRowSelectionModel>([]);

    const fetchOwners = async () => {
        setLoading(true);
        try {
            const response = await getOwners();
            setOwners(response);
        } catch (err) {
            console.error('Error fetching owners:', err);
            setError('Failed to fetch owners.');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchOwners();
    }, []);

    const filteredRows = owners.filter((row) =>
        row.name.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <div className="w-full">
            <Typography variant="h4" className="text-2xl font-bold mb-4">
                Owners
            </Typography>
            <OwnerFilter
                suggestions={owners.map(owner => owner.name)}
                searchTerm={searchTerm}
                setSearchTerm={setSearchTerm}
            />
            {loading ? (
                <Typography>Loading...</Typography>
            ) : error ? (
                <Typography color="error">{error}</Typography>
            ) : (
                <OwnerGrid
                    rows={filteredRows}
                    columns={ownerColumns}
                    loading={loading}
                    error={error}
                    selectionModel={selectionModel}
                    onRowSelectionModelChange={setSelectionModel}
                    onRowDoubleClick={(params: any) => {
                        navigate(`/owners/${params.row.id}`);
                    }}
                />
            )}
        </div>
    );
};

export default OwnerPage;
