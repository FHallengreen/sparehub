import * as React from 'react';
import { DataGrid, GridColDef, GridRowSelectionModel } from '@mui/x-data-grid';
import { Chip, CircularProgress, Typography } from '@mui/material';
import axios from 'axios';
import { TextField, Button } from '@mui/material';

const columns: GridColDef[] = [
  { field: 'id', headerName: 'Order ID', flex: 0.4, headerAlign: 'center' },
  { field: 'owner', headerName: 'Owner', flex: 0.5, headerAlign: 'center' },
  { field: 'vessel', headerName: 'Vessel', flex: 0.6, headerAlign: 'center' },
  { field: 'poNumber', headerName: 'Client Ref', flex: 1, headerAlign: 'center' },
  { field: 'pieces', headerName: 'Pcs', flex: 0.2, headerAlign: 'center' },
  { field: 'weight', headerName: 'Weight', flex: 0.25, headerAlign: 'center' },
  {
    field: 'stockLocation',
    headerName: 'Stock Location',
    flex: 0.7,
    headerAlign: 'center'
  },
  {
    field: 'status',
    headerName: 'Status',
    flex: 0.5,
    headerAlign: 'center',
    renderCell: (params) => {
      if (params.row.isGroupHeader) {
        return null;
      }
      return (
        <Chip
          label={params.value}
          variant="outlined"
          color={
            params.value === 'Inbound' ? 'warning' :
              params.value === 'Stock' ? 'success' :
                params.value === 'Pending' ? 'default' : 'error'
          }
          sx={{
            minWidth: '100%',
            justifyContent: 'center',
          }}
        />
      );
    },
  },
];

const OrderTable: React.FC = () => {
  const [rows, setRows] = React.useState<any[]>([]);
  const [loading, setLoading] = React.useState<boolean>(true);
  const [error, setError] = React.useState<string | null>(null);
  const [searchTerm, setSearchTerm] = React.useState<string>('');  // New state for search term
  const [selectedRows, setSelectedRows] = React.useState<GridRowSelectionModel>([]);

  // Fetch orders from the API with optional search term
  const fetchOrders = async (query: string = '') => {
    try {
      const response = await axios.get(`${import.meta.env.VITE_API_URL}/api/orders`, {
        params: { search: query },  // Send search term to backend
      });

      const mappedRows = response.data.map(order => ({
        id: order.id,
        owner: order.supplierName,
        vessel: order.vesselName,
        poNumber: order.orderNumber,
        pieces: 1,
        weight: 100,
        stockLocation: order.warehouseName,
        status: order.orderStatus
      }));

      setRows(mappedRows);
    } catch (err) {
      setError('Failed to fetch orders.');
    } finally {
      setLoading(false);
    }
  };

  React.useEffect(() => {
    fetchOrders();
  }, []);

  const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(event.target.value);  // Update the search term
  };

  // Trigger search when the search button is clicked
  const handleSearchClick = () => {
    setLoading(true); // Set loading state while searching
    fetchOrders(searchTerm);  // Query backend with search term
  };

  // Group rows by stockLocation
  const groupedRows = React.useMemo(() => {
    const groupHeaders: any[] = [];
    const rowGroups: Record<string, any[]> = {};

    rows.forEach((row) => {
      const location = row.stockLocation;
      if (!rowGroups[location]) {
        rowGroups[location] = [];
      }
      rowGroups[location].push(row);
    });

    Object.entries(rowGroups).forEach(([location, locationRows]) => {
      groupHeaders.push({
        stockLocation: location,
        isGroupHeader: true,
      });
      groupHeaders.push(...locationRows);
    });

    return groupHeaders;
  }, [rows]);

  // Handle selection changes
  const handleSelectionChange = (newSelection: GridRowSelectionModel) => {
    const groupHeaders = groupedRows.filter((row) => row.isGroupHeader).map((row) => row.stockLocation);
    const selectedGroupHeader = newSelection.find((id) => groupHeaders.includes(id as string));

    if (selectedGroupHeader) {
      const groupRows = groupedRows
        .filter((row) => row.stockLocation === selectedGroupHeader && !row.isGroupHeader)
        .map((row) => row.id);
      setSelectedRows(groupRows);
    } else {
      setSelectedRows(newSelection);
    }
  };

  if (loading) {
    return <CircularProgress />;
  }

  if (error) {
    return <Typography color="error">{error}</Typography>;
  }

  return (
    <div style={{ width: '100%' }}>
      <div style={{ display: 'flex', marginBottom: '20px' }}>
        <TextField
          label="Search Orders"
          variant="outlined"
          value={searchTerm}
          onChange={handleSearchChange}
          onKeyDown={(event) => {
            if (event.key === 'Enter') {
              handleSearchClick();
            }
          }}
          style={{ marginRight: '10px' }}
        />
        <Button variant="contained" onClick={handleSearchClick}>
          Search
        </Button>
      </div>


      <DataGrid
        rows={rows}
        columns={columns}
        getRowId={(row) => row.id || row.stockLocation}
        pageSizeOptions={[50, 100, 250]}
        pagination
        checkboxSelection
        disableColumnResize
        disableColumnSorting
        disableColumnMenu
        disableRowSelectionOnClick
        showCellVerticalBorder
        initialState={{
          pagination: {
            paginationModel: { pageSize: 50 },
          },
        }}
        rowSelectionModel={selectedRows}
        onRowSelectionModelChange={handleSelectionChange}
        getRowClassName={(params) =>
          params.row.isGroupHeader ? 'bg-gray-100 font-bold' : ''
        }
        autoHeight
      />
    </div>
  );
};

export default OrderTable;
