import { GridColDef } from '@mui/x-data-grid';
import { Chip } from '@mui/material';

export const columns: GridColDef[] = [
  { field: 'id', headerName: 'Id', flex: 0.1, headerAlign: 'center', align: 'center' },
  { field: 'owner', headerName: 'Owner', flex: 0.5, headerAlign: 'center', align: 'center' },
  { field: 'vessel', headerName: 'Vessel', flex: 0.6, headerAlign: 'center', align: 'center' },
  { field: 'supplier', headerName: 'Supplier', flex: 0.6, headerAlign: 'center', align: 'center' },
  { field: 'poNumber', headerName: 'Client Ref', flex: 0.7, headerAlign: 'center', align: 'center' },
  {
    field: 'status',
    headerName: 'Status',
    flex: 0.5,
    renderCell: (params) => (
      <Chip
        label={params.value}
        variant="outlined"
        color={
          params.value === 'Inbound'
            ? 'warning'
            : params.value === 'Stock'
            ? 'success'
            : params.value === 'Cancelled'
            ? 'error'
            : 'default'
        }
      />
    ),
  },
];
