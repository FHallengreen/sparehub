import { GridColDef } from '@mui/x-data-grid';

export const vesselAtPortColumns: GridColDef[] = [
    { field: 'vesselName', headerName: 'Vessel Name', flex: 0.3, headerAlign: 'center', align: 'center', renderCell: (params) => params.row.vessels[0].name },
    { field: 'portName', headerName: 'Port Name', flex: 0.3, headerAlign: 'center', align: 'center' },
    { field: 'arrivalDate', headerName: 'Arrival Date', flex: 0.3, headerAlign: 'center', align: 'center' },
    { field: 'departureDate', headerName: 'Departure Date', flex: 0.3, headerAlign: 'center', align: 'center' },
];