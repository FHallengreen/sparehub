import * as React from 'react';
import { DataGrid, GridColDef, GridRowSelectionModel } from '@mui/x-data-grid';
import { CircularProgress, Typography, Autocomplete, TextField, Chip } from '@mui/material';
import axios from 'axios';
import qs from 'qs';

interface Order {
  id: number;
  supplierName: string;
  vesselName: string;
  orderNumber: string;
  warehouseName: string;
  orderStatus: string;
  ownerName: string;
}

const columns: GridColDef[] = [
  { field: 'id', headerName: 'Order ID', flex: 0.4, headerAlign: 'center' },
  { field: 'owner', headerName: 'Owner', flex: 0.5, headerAlign: 'center' },
  { field: 'vessel', headerName: 'Vessel', flex: 0.6, headerAlign: 'center' },
  { field: 'supplier', headerName: 'Supplier', flex: 0.6, headerAlign: 'center' },
  { field: 'poNumber', headerName: 'Client Ref', flex: 0.7, headerAlign: 'center' },
  { field: 'pieces', headerName: 'Pcs', flex: 0.2, headerAlign: 'center' },
  { field: 'weight', headerName: 'Weight', flex: 0.25, headerAlign: 'center' },
  {
    field: 'stockLocation',
    headerName: 'Stock Location',
    flex: 0.9,
    headerAlign: 'center',
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
                params.value === 'Pending' ? 'default' :
                  params.value === 'Ready' ? 'primary' :
                    params.value === 'Cancelled' ? 'error' : 'default'
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
  const [searchTags, setSearchTags] = React.useState<string[]>([]);
  const [selectedRows, setSelectedRows] = React.useState<GridRowSelectionModel>([]);
  const [suggestions, setSuggestions] = React.useState<string[]>([]);

  const fetchOrders = async (tags: string[] = []) => {
    try {
      const response = await axios.get<Order[]>(`${import.meta.env.VITE_API_URL}/api/orders`, {
        params: { searchTerms: tags },
        paramsSerializer: (params) => {
          return qs.stringify(params, { arrayFormat: 'repeat' });
        },
      });

      const mappedRows = response.data.map((order: Order) => ({
        id: order.id,
        owner: order.ownerName,
        vessel: order.vesselName,
        supplier: order.supplierName,
        poNumber: order.orderNumber,
        pieces: 1,
        weight: 100,
        stockLocation: order.warehouseName,
        status: order.orderStatus,
      }));

      setRows(mappedRows);
    } catch (err) {
      setError('Failed to fetch orders.');
    } finally {
      setLoading(false);
    }
  };

  React.useEffect(() => {
    setLoading(true);
    fetchOrders(searchTags);
  }, [searchTags]);

  React.useEffect(() => {
    if (rows.length > 0) {
      const warehouseNames = rows.map((row) => row.stockLocation);
      const vesselNames = rows.map((row) => row.vessel);
      const ownerNames = rows.map((row) => row.owner);
      const orderNumbers = rows.map((row) => row.poNumber);
      const statuses = rows.map((row) => row.status);

      const allTerms = [
        ...warehouseNames,
        ...vesselNames,
        ...ownerNames,
        ...orderNumbers,
        ...statuses,
        "Cancelled"
      ];

      const uniqueTerms = Array.from(new Set(allTerms.filter(Boolean)));

      setSuggestions(uniqueTerms);
    }
  }, [rows]);

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
      <Autocomplete
        multiple
        freeSolo
        options={suggestions}
        value={searchTags}
        onChange={(_, newValue) => {
          setSearchTags(newValue);
        }}
        renderTags={(value: string[], getTagProps) =>
          value.map((option: string, index: number) => {
            const { key, ...restTagProps } = getTagProps({ index });

            return (
              <Chip
                key={index}
                variant="outlined"
                label={option}
                {...restTagProps}
              />
            );
          })
        }
        renderInput={(params) => (
          <TextField {...params} variant="outlined" label="Search Orders" placeholder="Add a tag" />
        )}
        style={{ marginBottom: '20px', width: '100%' }}
      />

      <DataGrid
        rows={groupedRows} 
        columns={columns}
        getRowId={(row) => row.id || `header-${row.stockLocation}`}
        pageSizeOptions={[100, 250, 500]}
        pagination
        checkboxSelection
        disableColumnResize
        disableRowSelectionOnClick
        showCellVerticalBorder
        initialState={{
          pagination: {
            paginationModel: { pageSize: 100 },
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
