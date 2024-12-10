import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { GridRowSelectionModel } from '@mui/x-data-grid';
import VesselGrid from '../components/VesselGrid';
import { vesselColumns } from '../columns/VesselColumns';
import { getVessels } from '../../../api/vesselApi';
import { Vessel } from '../../../interfaces/vessel';
import VesselFilter from '../components/VesselFilter';
import { Typography } from '@mui/material';

const VesselPage: React.FC = () => {
    const navigate = useNavigate();
    const [rows, setRows] = useState<Vessel[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [selectionModel, setSelectionModel] = useState<GridRowSelectionModel>([]);
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [suggestions, setSuggestions] = useState<string[]>([]);

    const fetchVessels = async () => {
        setLoading(true);
        try {
            const response = await getVessels();
            const modifiedRows = response.map((vessel) => ({
                id: vessel.id,
                name: vessel.name,
                imoNumber: vessel.imoNumber,
                flag: vessel.flag,
                owner_id: String(vessel.owner?.id || ''),
                ownerName: vessel.owner?.name || '',
            }));
            setRows(modifiedRows);
            const uniqueNames = Array.from(new Set(modifiedRows.map(v => v.name)));
            setSuggestions(uniqueNames);
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
        row.name.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <div className="w-full">
            <Typography variant="h4" className="text-2xl font-bold mb-4">
                Vessel List
            </Typography>
            <VesselFilter
                suggestions={suggestions}
                searchTerm={searchTerm}
                setSearchTerm={setSearchTerm}
            />
            <VesselGrid
                rows={filteredRows}
                columns={vesselColumns}
                loading={loading}
                error={error}
                selectionModel={selectionModel}
                onRowSelectionModelChange={setSelectionModel}
                onRowDoubleClick={(params) => {
                    navigate(`/vessels/${params.row.id}`);
                }}
            />
        </div>
    );
};

export default VesselPage; 