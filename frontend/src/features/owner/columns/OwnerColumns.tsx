import { GridColDef } from '@mui/x-data-grid';

export const ownerColumns: GridColDef[] = [
  { field: 'id', headerName: 'ID', flex: 0.1, headerAlign: 'center', align: 'center' },
  { field: 'name', headerName: 'Name', flex: 0.3, headerAlign: 'center', align: 'center' },
];
