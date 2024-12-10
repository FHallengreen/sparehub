import React from 'react';
import { DataGrid, GridColDef, GridRowSelectionModel } from '@mui/x-data-grid';
import { CircularProgress, Typography } from '@mui/material';

interface VesselGridProps {
    rows: any[];
    columns: GridColDef[];
    loading: boolean;
    error: string | null;
    selectionModel: GridRowSelectionModel;
    onRowSelectionModelChange: (newSelection: GridRowSelectionModel) => void;
    onRowDoubleClick: (params: any) => void;
}

const VesselGrid: React.FC<VesselGridProps> = ({
    rows,
    columns,
    loading,
    error,
    selectionModel,
    onRowSelectionModelChange,
    onRowDoubleClick,
}) => {
    if (loading) return <CircularProgress />;
    if (error) return <Typography color="error">{error}</Typography>;

    return (
        <DataGrid
            rows={rows}
            columns={columns}
            checkboxSelection
            rowSelectionModel={selectionModel}
            onRowSelectionModelChange={onRowSelectionModelChange}
            onRowDoubleClick={onRowDoubleClick}
            autoHeight
        />
    );
};

export default VesselGrid; 