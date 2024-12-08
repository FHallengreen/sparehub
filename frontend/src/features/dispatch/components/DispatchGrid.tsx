import React from 'react';
import { DataGrid, GridColDef, GridRowSelectionModel } from '@mui/x-data-grid';
import { CircularProgress, Typography } from '@mui/material';

interface DispatchGridProps {
  rows: any[];
  columns: GridColDef[];
  loading: boolean;
  error: string | null;
  selectionModel: GridRowSelectionModel;
  onSelectionModelChange: (newSelection: GridRowSelectionModel) => void;
  onRowDoubleClick: (params: any) => void;
}

const DispatchGrid: React.FC<DispatchGridProps> = ({
  rows,
  columns,
  loading,
  error,
  selectionModel,
  onSelectionModelChange,
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
      disableColumnFilter
      disableColumnSorting
      disableColumnResize
      disableRowSelectionOnClick
      showCellVerticalBorder
      rowSelectionModel={selectionModel}
      onRowSelectionModelChange={onSelectionModelChange}
      onRowDoubleClick={onRowDoubleClick}
      autoHeight
    />
  );
};

export default DispatchGrid;
