import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { GridRowSelectionModel } from '@mui/x-data-grid';
import VesselGrid from '../components/VesselGrid';
import VesselFilter from '../components/VesselFilter';
import { getVessels } from '../../../api/vesselApi';
import { Vessel } from '../../../interfaces/vessel';
import { Typography } from '@mui/material';
import { vesselColumns } from '../columns/VesselColumns';

const VesselPage: React.FC = () => {
    const navigate = useNavigate();
    const [rows, setRows] = useState<Vessel[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [selectionModel, setSelectionModel] = useState<GridRowSelectionModel>([]);

    const fetchVessels = async () => {
        setLoading(true);
        try {
            const response = await getVessels();
            setRows(response);
        } catch (err) {
            console.error('Error fetching vessels:', err);
            setError('Failed to fetch vessels.');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchVessels();
    }, []);

    const filteredRows = rows.filter((row) =>
        row.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        row.imoNumber.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <div className="w-full">
            <Typography variant="h4" className="text-2xl font-bold mb-4">
                Vessels
            </Typography>
            <VesselFilter
                suggestions={rows.map(row => row.name)}
                searchTerm={searchTerm}
                setSearchTerm={setSearchTerm}
            />
            {loading ? (
                <Typography>Loading...</Typography>
            ) : error ? (
                <Typography color="error">{error}</Typography>
            ) : (
                <VesselGrid
                    rows={filteredRows}
                    columns={vesselColumns}
                    loading={loading}
                    error={error}
                    selectionModel={selectionModel}
                    onRowSelectionModelChange={setSelectionModel}
                    onRowDoubleClick={(params: any) => {
                        navigate(`/vessels/${params.row.id}`);
                    }}
                />
            )}
        </div>
    );
};

export default VesselPage;