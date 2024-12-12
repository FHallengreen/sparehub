import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { GridRowSelectionModel } from '@mui/x-data-grid';
import VesselAtPortGrid from '../components/VesselAtPortGrid';
import VesselAtPortFilter from '../components/VesselAtPortFilter';
import { vesselAtPortColumns } from '../columns/VesselAtPortColumns';
import { getVesselsAtPorts } from '../../../api/vesselAtPortApi';
import { VesselAtPort } from '../../../interfaces/vesselAtPort';
import { Typography } from '@mui/material';

const VesselAtPortPage: React.FC = () => {
    const navigate = useNavigate();
    const [rows, setRows] = useState<VesselAtPort[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [searchTags, setSearchTags] = useState<string[]>([]);
    const [selectionModel, setSelectionModel] = useState<GridRowSelectionModel>([]);
    const [suggestions, setSuggestions] = useState<string[]>([]);

    const fetchVesselsAtPorts = async () => {
        setLoading(true);
        try {
            const response = await getVesselsAtPorts();
            setRows(response);
            const uniqueTerms = Array.from(new Set(response.map((row: VesselAtPort) => row.vesselId)));
            setSuggestions(uniqueTerms);
        } catch (err) {
            console.error('Error fetching vessels at ports:', err);
            setError('Failed to fetch vessels at ports.');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchVesselsAtPorts();
    }, [searchTags]);

    return (
        <div className="w-full">
            <Typography variant="h4" className="text-2xl font-bold mb-4">
                Vessels At Ports
            </Typography>
            <VesselAtPortFilter
                suggestions={suggestions}
                searchTags={searchTags}
                setSearchTags={setSearchTags}
            />
            <VesselAtPortGrid
                rows={rows}
                columns={vesselAtPortColumns}
                loading={loading}
                error={error}
                selectionModel={selectionModel}
                onRowSelectionModelChange={setSelectionModel}
                onRowDoubleClick={(params: any) => {
                    navigate(`/vessels-at-ports/${params.row.id}`);
                }}
            />
        </div>
    );
};

export default VesselAtPortPage;