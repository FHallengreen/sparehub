import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, CircularProgress, TextField, Typography } from '@mui/material';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import { createVessel } from '../../../api/vesselApi';
import { VesselRequest } from '../../../interfaces/vessel';
import { getOwners } from '../../../api/ownerApi.ts';
import axios from 'axios';

const NewVesselPage: React.FC = () => {
    const navigate = useNavigate();
    const showSnackbar = useSnackbar();
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [owners, setOwners] = useState<{ id: string; name: string }[]>([]);
    const [ownerId, setOwnerId] = useState<string | null>(null);
    const [vessel, setVessel] = useState<VesselRequest>({
        name: '',
        imoNumber: '',
        flag: '',
        ownerId: '',
    });

    useEffect(() => {
            // Fetch owners data from API or define it statically
            const fetchOwners = async () => {
                try {
                    const data = await getOwners(); // Replace with actual API call
                    setOwners(data.map(owner => ({ id: owner.id.toString(), name: owner.name })));
                } catch (err) {
                    console.error('Error fetching owners:', err);
                }
            };
    
            fetchOwners();
        }, []);

    const handleInputChange = (field: keyof VesselRequest, value: string) => {
        setVessel({ ...vessel, [field]: value });
    };

    const handleSave = async () => {
        setLoading(true);
        setError(null);

        console.log('Vessel data being sent:', vessel);

        try {
            await createVessel(vessel);
            showSnackbar('Vessel created successfully!', 'success');
            navigate('/vessels');
        } catch (err: unknown) {
            if (axios.isAxiosError(err)) {
                console.error('Error creating vessel:', err.response?.data || err.message);
                showSnackbar('Failed to create vessel: ' + (err.response?.data || err.message), 'error');
            } else {
                console.error('Unexpected error:', err);
                showSnackbar('Failed to create vessel due to an unexpected error.', 'error');
            }
            setError('Failed to create vessel.');
        } finally {
            setLoading(false);
        }
    };

    if (loading) {
        return <CircularProgress />;
    }

    return (
        <div className="container mx-auto p-6">
            <Typography variant="h4" className="text-2xl font-bold mb-6">
                Create New Vessel
            </Typography>

            {error && <Typography color="error">{error}</Typography>}

            <TextField
                label="Vessel Name"
                value={vessel.name}
                onChange={(e) => handleInputChange('name', e.target.value)}
                fullWidth
                className="mb-4"
            />
            <TextField
                label="IMO Number"
                value={vessel.imoNumber}
                onChange={(e) => handleInputChange('imoNumber', e.target.value)}
                fullWidth
                className="mb-4"
            />
            <TextField
                label="Flag"
                value={vessel.flag}
                onChange={(e) => handleInputChange('flag', e.target.value)}
                fullWidth
                className="mb-4"
            />
            <TextField
                select
                label="Owner"
                value={ownerId}
                onChange={(e) => setOwnerId(e.target.value)}
                fullWidth
                className="mb-4"
                SelectProps={{
                    native: true,
                }}
                InputLabelProps={{
                    shrink: true,
                }}
            >
                {owners.map((owner) => (
                    <option key={owner.id} value={owner.id}>
                        {owner.name}
                    </option>
                ))}
            </TextField>

            <div className="mt-8 gap-2 flex">
                <Button onClick={handleSave} variant="contained" color="primary" className="mr-2">
                    Save
                </Button>
                <Button onClick={() => navigate('/vessels')} variant="outlined">
                    Back
                </Button>
            </div>
        </div>
    );
};

export default NewVesselPage; 