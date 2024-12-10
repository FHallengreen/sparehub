import { GridColDef } from '@mui/x-data-grid';

export const vesselAtPortColumns: GridColDef[] = [
    { field: 'id', headerName: 'ID', flex: 0.1, headerAlign: 'center', align: 'center' },
    //{ field: 'vesselId', headerName: 'Vessel ID', flex: 0.3, headerAlign: 'center', align: 'center' },
    { field: 'vesselName', headerName: 'Vessel Name', flex: 0.3, headerAlign: 'center', align: 'center' },
    //{ field: 'portId', headerName: 'Port ID', flex: 0.3, headerAlign: 'center', align: 'center' },
    { field: 'portName', headerName: 'Port Name', flex: 0.3, headerAlign: 'center', align: 'center' },
    { field: 'arrivalDate', headerName: 'Arrival Date', flex: 0.3, headerAlign: 'center', align: 'center' },
    { field: 'departureDate', headerName: 'Departure Date', flex: 0.3, headerAlign: 'center', align: 'center' },
    { field: 'status', headerName: 'Status', flex: 0.3, headerAlign: 'center', align: 'center' },
]; 