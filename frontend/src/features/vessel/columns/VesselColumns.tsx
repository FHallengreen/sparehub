import { GridColDef } from '@mui/x-data-grid';

export const vesselColumns: GridColDef[] = [
    { field: 'ownerName', headerName: 'Owner Name', flex: 0.5, headerAlign: 'center', align: 'center' },
    { field: 'name', headerName: 'Vessel Name', flex: 0.5, headerAlign: 'center', align: 'center' },
    { field: 'imoNumber', headerName: 'IMO Number', flex: 0.5, headerAlign: 'center', align: 'center' },
    { field: 'flag', headerName: 'Flag', flex: 0.5, headerAlign: 'center', align: 'center' },
];