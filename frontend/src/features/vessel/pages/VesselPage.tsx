import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { GridRowSelectionModel } from '@mui/x-data-grid';
import VesselGrid from '../components/VesselGrid';
import VesselFilter from '../components/VesselFilter';
import { vesselColumns } from '../columns/VesselColumns';
import { getVessels } from '../../../api/vesselApi';
import { Vessel } from '../../../interfaces/vessel';

const VesselPage: React.FC = () => {
    const navigate = useNavigate();
    const [rows, setRows] = useState<Vessel[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [searchTags, setSearchTags] = useState<string[]>([]);
    const [selectionModel, setSelectionModel] = useState<GridRowSelectionModel>([]);
    const [suggestions, setSuggestions] = useState<string[]>([]);

    const fetchVessels = async () => {
        setLoading(true);
        try {
            const response = await getVessels();
            setRows(response);

            // Update suggestions based on the data
            const uniqueTerms = Array.from(
                new Set(response.map((row: Vessel) => row.name))
            );
            setSuggestions(uniqueTerms);
        } catch (err) {
            console.error('Error fetching vessels:', err);
            setError('Failed to fetch vessels.');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchVessels();
    }, [searchTags]);

    return (
        <div className="w-full">
            <VesselFilter
                suggestions={suggestions}
                searchTags={searchTags}
                setSearchTags={setSearchTags}
            />
            <VesselGrid
                rows={rows}
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