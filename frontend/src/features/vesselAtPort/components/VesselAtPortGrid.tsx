import React from 'react';
import { DataGrid, GridColDef, GridRowSelectionModel } from '@mui/x-data-grid';
import { CircularProgress, Typography } from '@mui/material';

interface VesselAtPortGridProps {
    rows: any[];
    columns: GridColDef[];
    loading: boolean;
    error: string | null;
    selectionModel: GridRowSelectionModel;
    onRowSelectionModelChange: (newSelection: GridRowSelectionModel) => void;
}

const VesselAtPortGrid: React.FC<VesselAtPortGridProps> = ({
    rows,
    columns,
    loading,
    error,
    selectionModel,
    onRowSelectionModelChange,
}) => {
    if (loading) return <CircularProgress />;
    if (error) return <Typography color="error">{error}</Typography>;

    return (
        <DataGrid
            rows={rows}
            columns={columns}
            rowSelectionModel={selectionModel}
            onRowSelectionModelChange={onRowSelectionModelChange}
            autoHeight
            getRowId={(row) => `${row.portId}-${row.vessels[0].id}`}
        />
    );
};

export default VesselAtPortGrid; 