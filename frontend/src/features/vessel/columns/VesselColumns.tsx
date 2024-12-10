import { GridColDef } from '@mui/x-data-grid';

export const vesselColumns: GridColDef[] = [
    { field: 'id', headerName: 'ID', flex: 0.1, headerAlign: 'center', align: 'center' },
    { field: 'name', headerName: 'Name', flex: 0.5, headerAlign: 'center', align: 'center' },
    { field: 'type', headerName: 'Type', flex: 0.3, headerAlign: 'center', align: 'center' },
    { field: 'imoNumber', headerName: 'IMO Number', flex: 0.5, headerAlign: 'center', align: 'center' },
]; 