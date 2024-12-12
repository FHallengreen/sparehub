import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { GridRowSelectionModel } from '@mui/x-data-grid';
import VesselAtPortGrid from '../components/VesselAtPortGrid';
import VesselAtPortFilter from '../components/VesselAtPortFilter';
import { vesselAtPortColumns } from '../columns/VesselAtPortColumns';
import { getVesselsAtPorts } from '../../../api/vesselAtPortApi';
import { VesselAtPort } from '../../../interfaces/vesselAtPort';
import { Typography } from '@mui/material';

// VesselAtPortPage component definition
const VesselAtPortPage: React.FC = () => {
    const navigate = useNavigate(); // Hook to programmatically navigate to different routes
    const [rows, setRows] = useState<VesselAtPort[]>([]); // State to store fetched data
    const [loading, setLoading] = useState<boolean>(true); // State to indicate loading status
    const [error, setError] = useState<string | null>(null); // State to store error messages
    const [searchTags, setSearchTags] = useState<string[]>([]); // State to manage search filter tags
    const [selectionModel, setSelectionModel] = useState<GridRowSelectionModel>([]); // State to handle selected rows in the grid
    const [suggestions, setSuggestions] = useState<string[]>([]); // State to store search suggestions

    // Function to fetch vessels at ports data from the API
    const fetchVesselsAtPorts = async () => {
        setLoading(true); // Set loading state to true before making the API call
        try {
            const response = await getVesselsAtPorts(); // Fetch data from the API
            console.log(response);
            setRows(response); // Update rows state with the fetched data

            // Update suggestions based on the data
            const uniqueTerms = Array.from(
                new Set(response.map((row: VesselAtPort) => row.vesselId)) // Generate unique search suggestions based on vesselId
            );
            setSuggestions(uniqueTerms); // Update suggestions state
        } catch (err) {
            console.error('Error fetching vessels at ports:', err); // Log error to the console
            setError('Failed to fetch vessels at ports.'); // Update error state
        } finally {
            setLoading(false); // Set loading state to false after the API call
        }
    };

    // useEffect hook to call fetchVesselsAtPorts whenever searchTags state changes
    useEffect(() => {
        fetchVesselsAtPorts();
    }, [searchTags]);

    return (
        <div className="w-full">
            <Typography variant="h4" className="text-2xl font-bold mb-4">
                Vessels At Ports
            </Typography>
            {/* VesselAtPortFilter component for filtering functionality */}
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
            />
        </div>
    );
};

export default VesselAtPortPage;