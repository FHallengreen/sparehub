import { GridColDef } from '@mui/x-data-grid';
import { Chip } from '@mui/material';
import { format } from 'date-fns';

export const dispatchColumns: GridColDef[] = [
  { field: 'id', headerName: 'ID', flex: 0.1, headerAlign: 'center', align: 'center' },
  { field: 'originType', headerName: 'Origin Type', flex: 0.3, headerAlign: 'center', align: 'center' },
  { field: 'originId', headerName: 'Origin ID', flex: 0.2, headerAlign: 'center', align: 'center' },
  { field: 'destinationType', headerName: 'Destination Type', flex: 0.3, headerAlign: 'center', align: 'center' },
  { field: 'destinationId', headerName: 'Destination ID', flex: 0.2, headerAlign: 'center', align: 'center' },
  { field: 'dispatchStatus', headerName: 'Status', flex: 0.3, headerAlign: 'center', align: 'center' },
  { field: 'transportModeType', headerName: 'Transport Mode', flex: 0.3, headerAlign: 'center', align: 'center' },
  { field: 'trackingNumber', headerName: 'Tracking #', flex: 0.3, headerAlign: 'center', align: 'center' },
  {
    field: 'dispatchDate',
    headerName: 'Dispatch Date',
    flex: 0.3,
    headerAlign: 'center',
    align: 'center',
    renderCell: (params) => format(new Date(params.value as string), 'yyyy-MM-dd'),
  },
  {
    field: 'deliveryDate',
    headerName: 'Delivery Date',
    flex: 0.3,
    headerAlign: 'center',
    align: 'center',
    renderCell: (params) => format(new Date(params.value as string), 'yyyy-MM-dd'),
  },
  { field: 'userId', headerName: 'User ID', flex: 0.2, headerAlign: 'center', align: 'center' },
  {
    field: 'orderIds',
    headerName: 'Orders',
    flex: 0.5,
    headerAlign: 'center',
    align: 'center',
    renderCell: (params) => {
      const orders = params.value as number[];
      return orders.length > 0 ? (
        <div>
          {orders.map((order) => (
            <Chip key={order.toString()} label={order} variant="outlined" sx={{ margin: 0.5 }} />
          ))}
        </div>
      ) : (
        'No Orders'
      );
    },
  },
];
