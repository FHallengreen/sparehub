import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { CircularProgress, Typography, Button } from '@mui/material';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import OwnerDetailForm from '../components/OwnerDetailForm.tsx';
import { createOwner } from '../../../api/ownerApi.ts';
import { OwnerRequest } from '../../../interfaces/owner';

const NewOwnerPage: React.FC = () => {
    const navigate = useNavigate();
    const showSnackbar = useSnackbar();

    const [owner, setOwner] = useState<OwnerRequest>({
        name: '',
        email: '',
        phone: '',
    });

    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const handleInputChange = (field: keyof OwnerRequest, value: string) => {
        setOwner((prev) => ({
            ...prev,
            [field]: value,
        }));
    };

    const handleSave = async () => {
        setLoading(true);
        setError(null);

        try {
            await createOwner(owner);
            showSnackbar('Owner created successfully!', 'success');
            navigate('/owners');
        } catch (err) {
            console.error('Error creating owner:', err);
            showSnackbar('Failed to create owner.', 'error');
            setError('Failed to create owner.');
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
                Create New Owner
            </Typography>

            {error && <Typography color="error">{error}</Typography>}

            <OwnerDetailForm
                title="Create Owner"
                fields={[
                    { label: 'Name', value: owner.name, onChange: (value) => handleInputChange('name', value) },
                    { label: 'Email', value: owner.email, onChange: (value) => handleInputChange('email', value) },
                    { label: 'Phone', value: owner.phone, onChange: (value) => handleInputChange('phone', value) },
                ]}
            />

            <div className="mt-8 gap-2 flex">
                <Button onClick={handleSave} variant="contained" color="primary" className="mr-2">
                    Save
                </Button>
                <Button onClick={() => navigate('/owners')} variant="outlined">
                    Back
                </Button>
            </div>
        </div>
    );
};

export default NewOwnerPage;
