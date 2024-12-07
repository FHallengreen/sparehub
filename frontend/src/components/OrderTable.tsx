import * as React from 'react';
import {
  DataGrid,
  GridColDef,
  GridRowSelectionModel,
} from '@mui/x-data-grid';
import {
  CircularProgress,
  Typography,
  Autocomplete,
  TextField,
  Button,
  Chip,
} from '@mui/material';
import api from '../Api';
import qs from 'qs';
import { useNavigate } from 'react-router-dom';
import { Order } from '../interfaces/order.ts';
import SummaryPanel from './SummaryPanel';

const columns: GridColDef[] = [
  {
    field: 'id',
    headerName: 'Id',
    flex: 0.1,
    headerAlign: 'center',
    align: 'center',
    renderCell: (params) => {
      if (params.row.isGroupHeader) {
        return '';
      }
      return params.value;
    },
  },
  { field: 'owner', headerName: 'Owner', flex: 0.5, headerAlign: 'center', align: 'center', },
  { field: 'vessel', headerName: 'Vessel', flex: 0.6, headerAlign: 'center', align: 'center', },
  { field: 'supplier', headerName: 'Supplier', flex: 0.6, headerAlign: 'center', align: 'center', },
  { field: 'poNumber', headerName: 'Client Ref', flex: 0.7, headerAlign: 'center', align: 'center', },
  {
    field: 'pieces',
    headerName: 'Pieces',
    flex: 0.25,
    headerAlign: 'center',
    align: 'center',
  },
  {
    field: 'weight',
    headerName: 'Weight',
    flex: 0.25,
    headerAlign: 'center',
    align: 'center',
    renderCell: (params) => {
      if (params.row.isGroupHeader) {
        return null;
      }
      return params.value !== null ? params.value : '';
    },
  },
  {
    field: 'stockLocation',
    headerName: 'Stock Location',
    flex: 0.8,
    headerAlign: 'center',
    align: 'center',
  },
  {
    field: 'status',
    headerName: 'Status',
    flex: 0.45,
    headerAlign: 'center',
    align: 'center',
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
  const [searchTags, setSearchTags] = React.useState<string[]>(() => {
    const savedFilters = localStorage.getItem('savedSearchTags');
    return savedFilters ? JSON.parse(savedFilters) : [];
  });

  const [selectionModel, setSelectionModel] = React.useState<GridRowSelectionModel>([]);
  const [selectedDataRowIds, setSelectedDataRowIds] = React.useState<Set<string>>(new Set());

  const [suggestions, setSuggestions] = React.useState<string[]>([]);
  const searchBoxRef = React.useRef<HTMLInputElement>(null);
  const navigate = useNavigate();

  const fetchOrders = async (tags: string[] = []) => {
    try {
      const response = await api.get<Order[]>(`${import.meta.env.VITE_API_URL}/api/order`, {
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
        pieces: order.boxes && order.boxes > 0 ? order.boxes : null,
        weight: order.totalWeight && order.totalWeight > 0 ? order.totalWeight : null,
        volume: order.totalVolume && order.totalVolume > 0 ? order.totalVolume : null, // Map volume here
        stockLocation: order.warehouseName,
        status: order.orderStatus,
      }));

      setRows(mappedRows);
    } catch (err) {
      console.error('Error fetching orders:', err);
      setError('Failed to fetch orders.');
    } finally {
      setLoading(false);
    }
  };

  // Effects
  React.useEffect(() => {
    if (searchBoxRef.current) {
      searchBoxRef.current.focus();
    }
  }, []);

  React.useEffect(() => {
    setLoading(true);
    fetchOrders(searchTags);
  }, [searchTags]);

  React.useEffect(() => {
    localStorage.setItem('savedSearchTags', JSON.stringify(searchTags));
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
    const rowGroups: Record<string, Record<string, any[]>> = {};

    rows.forEach((row) => {
      const location = row.stockLocation;
      const status = row.status;

      if (!rowGroups[location]) {
        rowGroups[location] = {};
      }
      if (!rowGroups[location][status]) {
        rowGroups[location][status] = [];
      }
      rowGroups[location][status].push(row);
    });

    Object.entries(rowGroups).forEach(([location, statusGroups]) => {
      // Add group header for the stock location
      groupHeaders.push({
        id: `header-${location}`,
        stockLocation: location,
        isGroupHeader: true,
      });

      // Sort statuses in reverse order and add rows for each status
      Object.entries(statusGroups)
        .sort(([statusA], [statusB]) => statusB.localeCompare(statusA)) // Reverse the order
        .forEach(([, statusRows]) => {
          if (statusRows.length > 0) {
            groupHeaders.push(...statusRows);
          }
        });
    });

    return groupHeaders;
  }, [rows]);

  const handleSelectionChange = (newSelection: GridRowSelectionModel) => {
    const groupHeaderIds = groupedRows
      .filter((row) => row.isGroupHeader)
      .map((row) => row.id);

    const newSelectionSet = new Set<string>();

    newSelection.forEach((id) => {
      if (groupHeaderIds.includes(id as string)) {
        const stockLocation = (id as string).replace('header-', '');
        const childRowIds = groupedRows
          .filter(
            (row) =>
              row.stockLocation === stockLocation && !row.isGroupHeader
          )
          .map((row) => row.id);
        childRowIds.forEach((childId) => {
          newSelectionSet.add(childId);
        });
        newSelectionSet.add(id as string);
      } else {
        newSelectionSet.add(id as string);
      }
    });

    setSelectionModel(Array.from(newSelectionSet));

    const selectedDataRowIds = new Set(
      Array.from(newSelectionSet).filter(
        (id) => !groupHeaderIds.includes(id as string)
      )
    );
    setSelectedDataRowIds(selectedDataRowIds);
  };


  if (loading) {
    return <CircularProgress />;
  }

  if (error) {
    return <Typography color="error">{error}</Typography>;
  }

  return (
    <div className="w-full">
      <div className="flex items-center space-x-2 mb-5">
        <Autocomplete
          multiple
          freeSolo
          options={suggestions}
          value={searchTags}
          onChange={(_, newValue) => {
            setSearchTags(newValue);
          }}
          onInputChange={(_, inputValue) => {
            if (inputValue.length >= 3) {
              setSuggestions(
                suggestions.filter((option) =>
                  option.toLowerCase().includes(inputValue.toLowerCase())
                )
              );
            } else {
              setSuggestions([]);
            }
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
            <TextField
              {...params}
              inputRef={searchBoxRef}
              autoFocus
              variant="outlined"
              label="Search Orders"
              placeholder="Add a tag"
            />
          )}
          style={{ width: "40vw" }}
        />

        <Button onClick={() => navigate(`/orders/new`)} variant="contained" color="primary" className="pr-5">
          New Order
        </Button>
      </div>

      {selectedDataRowIds.size > 0 && (
        <div className="mt-5">
          <SummaryPanel selectedRows={selectedDataRowIds} allRows={rows} />
        </div>
      )}

      <DataGrid
        rows={groupedRows}
        columns={columns}
        onRowDoubleClick={(params) => navigate(`/orders/${params.id}`)}
        getRowId={(row) => row.id}
        pageSizeOptions={[100, 250, 500]}
        pagination
        checkboxSelection
        disableColumnFilter
        disableColumnSorting
        disableColumnResize
        disableRowSelectionOnClick
        showCellVerticalBorder
        initialState={{
          pagination: {
            paginationModel: { pageSize: 100 },
          },
        }}
        rowSelectionModel={selectionModel}
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
