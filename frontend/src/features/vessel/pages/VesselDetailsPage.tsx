import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Button, CircularProgress, Typography } from '@mui/material';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import { getVesselById, updateVessel, deleteVessel } from '../../../api/vesselApi';
import { VesselDetail, VesselRequest } from '../../../interfaces/vessel';
import { getOwners } from '../../../api/ownerApi.ts';
import VesselDetailForm from '../components/VesselDetailForm.tsx';


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

    useEffect(() => {
        const fetchVessel = async () => {
            try {
                const data = await getVesselById(id!);
                
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

        
        try {
         
            await updateVessel(vessel.id, updatedVessel);
          
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
           
            {error && <Typography color="error">{error}</Typography>}

            <VesselDetailForm
                title='Vessel Information'
                fields={[
                    {
                        label: 'Vessel Name',
                        value: vessel.name,
                        onChange: (value: string) => setVessel({ ...vessel, name: value }),
                    },
                    {
                        label: 'IMO Number',
                        value: vessel.imoNumber,
                        onChange: (value: string) => setVessel({ ...vessel, imoNumber: value }),
                    },
                    {
                        label: 'Flag',
                        value: vessel.flag,
                        onChange: (value: string) => setVessel({ ...vessel, flag: value }),
                    },
                    {
                        label: 'Owner',
                        value: ownerId,
                        onChange: (value: string) => setOwnerId(value),
                        select: true,
                        options: owners.map(owner => ({ value: owner.id, label: owner.name })),
                    },
                ]}
                />


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