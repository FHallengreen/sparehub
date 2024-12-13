import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Button, CircularProgress, TextField, Typography } from '@mui/material';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import { getVesselById, updateVessel, deleteVessel } from '../../../api/vesselApi';
import { VesselDetail, VesselRequest } from '../../../interfaces/vessel';
import { getOwners } from '../../../api/ownerApi.ts';


const VesselDetailsPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const showSnackbar = useSnackbar();
    const [vessel, setVessel] = useState<VesselDetail | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [ownerId, setOwnerId] = useState<string | null>(null);
    const [owners, setOwners] = useState<{ id: string; name: string }[]>([]);

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

    useEffect(() => {
        const fetchVessel = async () => {
            try {
                const data = await getVesselById(id!);
                console.log('Fetched Vessel:', data);
                setVessel(data);
                setOwnerId(data.owner?.id?.toString() || '');
            } catch (err) {
                console.error('Error fetching vessel:', err);
                setError('Failed to fetch vessel details.');
            } finally {
                setLoading(false);
            }
        };

        fetchVessel();
    }, [id]);


    const handleSave = async () => {
        console.log('Vessel:', vessel);
        console.log('Owner ID:', ownerId);

        if (!vessel || !ownerId) {
            showSnackbar('Owner ID is required.', 'error');
            return;
        }

        const updatedVessel: VesselRequest = {
            name: vessel.name,
            imoNumber: vessel.imoNumber,
            flag: vessel.flag,
            ownerId: ownerId
        };

        console.log('Updated Vessel:', updatedVessel); // Debugging: log the payload
        try {
            console.log('Attempting to update vessel with ID:', vessel.id);
            await updateVessel(vessel.id, updatedVessel);
            console.log('Vessel updated successfully');
            showSnackbar('Vessel updated successfully!', 'success');
            navigate('/vessels');
        } catch (err) {
            console.error('Error updating vessel:', err);
            showSnackbar('Failed to update vessel.', 'error');
        }
    };


    const handleDelete = async () => {
        if (!vessel) {
            showSnackbar('No vessel found to delete.', 'error');
            return;
        }

        if (window.confirm('Are you sure you want to delete this vessel?')) {
            try {
                await deleteVessel(vessel.id);
                showSnackbar('Vessel deleted successfully!', 'success');
                navigate('/vessels');
            } catch (err) {
                console.error('Error deleting vessel:', err);
                showSnackbar('Failed to delete vessel.', 'error');
            }
        }
    };

    if (loading) return <CircularProgress />;
    if (error) return <Typography color="error">{error}</Typography>;
    if (!vessel) return <Typography>No vessel found.</Typography>;

    return (
        <div className="container mx-auto p-6">
            <Typography variant="h4" className="text-2xl font-bold mb-6">
                Vessel Details
            </Typography>

            <TextField
                label="Vessel Name"
                value={vessel.name}
                onChange={(e) => setVessel({ ...vessel, name: e.target.value })}
                fullWidth
                className="mb-4"
            />
            <TextField
                label="IMO Number"
                value={vessel.imoNumber}
                onChange={(e) => setVessel({ ...vessel, imoNumber: e.target.value })}
                fullWidth
                className="mb-4"
            />
            <TextField
                label="Flag"
                value={vessel.flag}
                onChange={(e) => setVessel({ ...vessel, flag: e.target.value })}
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
                <Button onClick={handleDelete} variant="outlined" color="error">
                    Delete
                </Button>
                <Button onClick={() => navigate('/vessels')} variant="outlined">
                    Back
                </Button>
            </div>
        </div>
    );
};

export default VesselDetailsPage; 