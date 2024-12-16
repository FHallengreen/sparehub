import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, CircularProgress, Typography } from '@mui/material';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import { createVesselAtPort, getVesselsAtPorts } from '../../../api/vesselAtPortApi.ts';
import { VesselAtPortRequest } from '../../../interfaces/vesselAtPort.ts';
import { getPorts } from '../../../api/portApi.ts';
import { getVessels } from '../../../api/vesselApi.ts';
import VesselAtPortDetailForm from '../components/VesselAtPortDetailForm.tsx';

const NewVesselAtPortPage: React.FC = () => {
    const navigate = useNavigate();
    const showSnackbar = useSnackbar();
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [ports, setPorts] = useState<{ id: string; name: string }[]>([]);
    const [vessels, setVessels] = useState<{ id: string; name: string }[]>([]);
    const [vesselAtPort, setVesselAtPort] = useState<VesselAtPortRequest>({
        vesselId: '',
        portId: '',
        arrivalDate: '',
        departureDate: '',
    });

    useEffect(() => {
        const fetchVessels = async () => {
            try {
                const vesselsData = await getVessels();
                const vesselsAtPortData = await getVesselsAtPorts();

                const vesselsAtPortIds = vesselsAtPortData.map(vesselAtPort => vesselAtPort.vessels[0].id.toString());
                const availableVessels = vesselsData.filter(vessel => !vesselsAtPortIds.includes(vessel.id.toString()));

                setVessels(availableVessels.map(vessel => ({ id: vessel.id.toString(), name: vessel.name })));
            } catch (err) {
                console.error('Error fetching vessels:', err);
            }
        };

        fetchVessels();
    }, []);

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

    const handleInputChange = (field: keyof VesselAtPortRequest, value: string) => {
        setVesselAtPort({ ...vesselAtPort, [field]: value });
    };

    const handleSave = async () => {
        if (!vesselAtPort.vesselId || !vesselAtPort.portId) {
            setError('Vessel and Port must be selected.');
            return;
        }

        setLoading(true);
        setError(null);

        try {
            await createVesselAtPort(vesselAtPort);
            showSnackbar('Vessel at Port created successfully!', 'success');
            navigate('/vessels-at-ports');
        } catch (err) {
            console.error('Error creating vessel at port:', err);
            showSnackbar('Failed to create vessel at port.', 'error');
            setError('Failed to create vessel at port.');
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

            <VesselAtPortDetailForm
                title="Add Vessel To Port"
                fields={[
                    {
                        label: 'Vessel',
                        value: vesselAtPort.vesselId,
                        onChange: (value) => handleInputChange('vesselId', value),
                        select: true,
                        options: vessels.map(vessel => ({ value: vessel.id, label: vessel.name })),
                    },
                    {
                        label: 'Port',
                        value: vesselAtPort.portId,
                        onChange: (value) => handleInputChange('portId', value),
                        select: true,
                        options: ports.map(port => ({ value: port.id, label: port.name })),
                    },
                    {
                        label: 'Arrival Date',
                        value: vesselAtPort.arrivalDate,
                        onChange: (value) => handleInputChange('arrivalDate', value),
                        type: 'date',
                        InputLabelProps: { shrink: true },
                    },
                    {
                        label: 'Departure Date',
                        value: vesselAtPort.departureDate || '',
                        onChange: (value) => handleInputChange('departureDate', value),
                        type: 'date',
                        InputLabelProps: { shrink: true },
                    },
                ]}
            />

            <div className="mt-8 gap-2 flex">
                <Button onClick={handleSave} variant="contained" color="primary" className="mr-2">
                    Save
                </Button>
                <Button onClick={() => navigate('/vessels-at-ports')} variant="outlined">
                    Back
                </Button>
            </div>
        </div>
    );
};

export default NewVesselAtPortPage;