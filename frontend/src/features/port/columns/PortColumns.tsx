import { GridColDef } from '@mui/x-data-grid';

export const portColumns: GridColDef[] = [
  {
    field: 'id',
    headerName: 'ID',
    flex: 0.1,
    headerAlign: 'center',
    align: 'center',
  },
  {
    field: 'name',
    headerName: 'Name',
    flex: 0.5,
    headerAlign: 'center',
    align: 'center',
  }
]; 