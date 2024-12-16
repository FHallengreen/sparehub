import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, CircularProgress, Typography } from '@mui/material';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import { createVessel } from '../../../api/vesselApi';
import { VesselRequest } from '../../../interfaces/vessel';
import { getOwners } from '../../../api/ownerApi.ts';
import axios from 'axios';
import VesselDetailForm from '../components/VesselDetailForm.tsx';

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
        const fetchOwners = async () => {
            try {
                const data = await getOwners(); 
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

    const handleOwnerChange = (value: string) => {
        setOwnerId(value);
        handleInputChange('ownerId', value);
    };

    const handleSave = async () => {
        setLoading(true);
        setError(null);

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
            {error && <Typography color="error">{error}</Typography>}

            <VesselDetailForm
                title="Create Vessel"
                fields={[
                    { label: 'Vessel Name', value: vessel.name, onChange: (value) => handleInputChange('name', value) },
                    { label: 'IMO Number', value: vessel.imoNumber, onChange: (value) => handleInputChange('imoNumber', value) },
                    { label: 'Flag', value: vessel.flag, onChange: (value) => handleInputChange('flag', value) },
                    { 
                        label: 'Owner', 
                        value: ownerId || '', 
                        onChange: handleOwnerChange,
                        select: true,
                        options: owners.map(owner => ({ value: owner.id, label: owner.name }))
                    },
                ]}
            />

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