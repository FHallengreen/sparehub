import React from 'react';
import { DataGrid, GridColDef, GridRowSelectionModel } from '@mui/x-data-grid';
import { CircularProgress, Typography } from '@mui/material';

interface OwnerGridProps {
  rows: any[];
  columns: GridColDef[];
  loading: boolean;
  error: string | null;
  selectionModel: GridRowSelectionModel;
  onRowSelectionModelChange: (newSelection: GridRowSelectionModel) => void;
  onRowDoubleClick: (params: any) => void;
}

const OwnerGrid: React.FC<OwnerGridProps> = ({
  rows,
  columns,
  loading,
  error,
  selectionModel,
  onRowSelectionModelChange,
  onRowDoubleClick,
}) => {
  if (loading) {
    return <CircularProgress />;
  }

  if (error) {
    return <Typography color="error">{error}</Typography>;
  }

  return (
    <DataGrid
      rows={rows}
      columns={columns}
      getRowId={(row) => row.id}
      pageSizeOptions={[25, 50, 100]}
      pagination
      checkboxSelection
      rowSelectionModel={selectionModel}
      onRowSelectionModelChange={onRowSelectionModelChange}
      onRowDoubleClick={onRowDoubleClick}
      autoHeight
    />
  );
};

export default OwnerGrid;
