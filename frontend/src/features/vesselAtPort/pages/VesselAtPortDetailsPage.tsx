import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { CircularProgress, Typography, Button } from '@mui/material';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import { getVesselAtPort, updateVesselAtPort, deleteVesselAtPort } from '../../../api/vesselAtPortApi.ts';
import { VesselAtPortDetail } from '../../../interfaces/vesselAtPort.ts';
import { getPorts } from '../../../api/portApi.ts';
import VesselAtPortDetailForm from '../components/VesselAtPortDetailForm.tsx';

const VesselAtPortDetailsPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const showSnackbar = useSnackbar();
    const [vesselAtPort, setVesselAtPort] = useState<VesselAtPortDetail | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [ports, setPorts] = useState<{ id: string; name: string }[]>([]);

    useEffect(() => {
        const fetchPorts = async () => {
            try {
                const data = await getPorts();
                setPorts(data.map(port => ({ id: port.id.toString(), name: port.name })));
            } catch (err) {
                console.error('Error fetching ports:', err);
            }
        };

        fetchPorts();
    }, []);

    useEffect(() => {
        const fetchVesselAtPort = async () => {
            try {
                const data = await getVesselAtPort(id!);
                setVesselAtPort(data);
                setError(null);
            } catch (err) {
                console.error('Error fetching vessel at port details:', err);
                setError('Failed to fetch vessel at port details.');
            } finally {
                setLoading(false);
            }
        };

        fetchVesselAtPort();
    }, [id]);

    const handleInputChange = (field: keyof VesselAtPortDetail, value: string) => {
        if (vesselAtPort) {
            setVesselAtPort({ ...vesselAtPort, [field]: value });
        }
    };

    const handleSave = async () => {
        if (!vesselAtPort) return;

        try {
            await updateVesselAtPort(
                {
                    vesselId: vesselAtPort.vessels[0].id.toString(),
                    portId: vesselAtPort.portId,
                    arrivalDate: vesselAtPort.arrivalDate,
                    departureDate: vesselAtPort.departureDate
                });
            showSnackbar('Vessel at Port updated successfully!', 'success');
            navigate('/vessels-at-ports');
        } catch (err) {
            console.error('Error updating vessel at port:', err);
            showSnackbar('Failed to update vessel at port.', 'error');
        }
    };

    const handleDelete = async () => {
        if (window.confirm('Are you sure you want to delete this vessel at port?')) {
            try {
                await deleteVesselAtPort(id!);
                showSnackbar('Vessel at Port deleted successfully!', 'success');
                navigate('/vessels-at-ports');
            } catch (err) {
                console.error('Error deleting vessel at port:', err);
                showSnackbar('Failed to delete vessel at port.', 'error');
            }
        }
    };

    if (loading) return <CircularProgress />;
    if (error) return <Typography color="error">{error}</Typography>;
    if (!vesselAtPort) return <Typography>No vessel at port found.</Typography>;

    return (
        <div className="container mx-auto p-6">
            <VesselAtPortDetailForm
                title='Vessel At Port Information'
                fields={[
                    {
                        label: 'Vessel',
                        value: vesselAtPort.vessels[0].name,
                        select: false,
                        InputLabelProps: { readOnly: true },
                        onChange: () => {}
                    },
                    {
                        label: 'Port Name',
                        value: vesselAtPort.portId,
                        select: true,
                        options: ports.map(port => ({ label: port.name, value: port.id })),
                        onChange: (value: string) => handleInputChange('portId', value)
                    },
                    {
                        label: 'Arrival Date',
                        value: vesselAtPort.arrivalDate,
                        type: 'date',
                        onChange: (value: string) => handleInputChange('arrivalDate', value)
                    },
                    {
                        label: 'Departure Date',
                        value: vesselAtPort.departureDate || '',
                        type: 'date',
                        onChange: (value: string) => handleInputChange('departureDate', value)
                    }
                ]}
            />

            <div className="mt-8 gap-2 flex">
                <Button onClick={handleSave} variant="contained" color="primary" className="mr-2">
                    Save
                </Button>
                <Button onClick={handleDelete} variant="outlined" color="error">
                    Delete
                </Button>
                <Button onClick={() => navigate('/vessels-at-ports')} variant="outlined">
                    Back
                </Button>
            </div>
        </div>
    );
};

export default VesselAtPortDetailsPage;