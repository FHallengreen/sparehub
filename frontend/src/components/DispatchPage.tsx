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
import axios from 'axios';
import qs from 'qs';
import { useNavigate } from 'react-router-dom';
import { Dispatch } from '../interfaces/dispatch';
import { format } from 'date-fns';

const columns: GridColDef[] = [
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
          {orders.map((order, index) => (
            <Chip key={index} label={order} variant="outlined" sx={{ margin: 0.5 }} />
          ))}
        </div>
      ) : (
        'No Orders'
      );
    },
  },
];

const DispatchTable: React.FC = () => {
  const [rows, setRows] = React.useState<Dispatch[]>([]);
  const [loading, setLoading] = React.useState<boolean>(true);
  const [error, setError] = React.useState<string | null>(null);
  const [searchTags, setSearchTags] = React.useState<string[]>([]);
  const [selectionModel, setSelectionModel] = React.useState<GridRowSelectionModel>([]);
  const [suggestions, setSuggestions] = React.useState<string[]>([]);
  const searchBoxRef = React.useRef<HTMLInputElement>(null);
  const navigate = useNavigate();

  const fetchDispatches = async (tags: string[] = []) => {
    try {
      const response = await axios.get<Dispatch[]>(`${import.meta.env.VITE_API_URL}/api/dispatch`, {
        params: { searchTerms: tags },
        paramsSerializer: (params) => qs.stringify(params, { arrayFormat: 'repeat' }),
      });

      setRows(response.data);
    } catch (err) {
      console.error('Error fetching dispatches:', err);
      setError('Failed to fetch dispatches.');
    } finally {
      setLoading(false);
    }
  };

  React.useEffect(() => {
    if (searchBoxRef.current) {
      searchBoxRef.current.focus();
    }
  }, []);

  React.useEffect(() => {
    setLoading(true);
    fetchDispatches(searchTags);
  }, [searchTags]);

  React.useEffect(() => {
    if (rows.length > 0) {
      const uniqueTerms = Array.from(
        new Set(
          rows
            .flatMap((row) => [
              row.originType,
              row.destinationType,
              row.dispatchStatus,
              row.transportModeType,
            ])
            .filter(Boolean)
        )
      );

      setSuggestions(uniqueTerms);
    }
  }, [rows]);

  const handleSelectionChange = (newSelection: GridRowSelectionModel) => {
    setSelectionModel(newSelection);
  };

  const handleRowDoubleClick = (params: any) => {
    navigate(`/dispatches/${params.id}`);
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
          onChange={(_, newValue) => setSearchTags(newValue)}
          renderTags={(value, getTagProps) =>
            value.map((option, index) => {
              const { key, ...tagProps } = getTagProps({ index }); // Extract `key` from `getTagProps`
              return <Chip key={key} variant="outlined" label={option} {...tagProps} />;
            })
          }
          renderInput={(params) => (
            <TextField
              {...params}
              inputRef={searchBoxRef}
              variant="outlined"
              label="Search Dispatches"
              placeholder="Add a tag"
            />
          )}
          style={{ width: '40vw' }}
        />
        <Button onClick={() => navigate(`/dispatches/new`)} variant="contained" color="primary">
          New Dispatch
        </Button>
      </div>

      <DataGrid
        rows={rows}
        columns={columns}
        getRowId={(row) => row.id}
        pageSizeOptions={[25, 50, 100]}
        pagination
        checkboxSelection
        disableColumnFilter
        disableColumnSorting
        disableColumnResize
        disableRowSelectionOnClick
        showCellVerticalBorder
        rowSelectionModel={selectionModel}
        onRowSelectionModelChange={handleSelectionChange}
        onRowDoubleClick={handleRowDoubleClick} // Handle double-click navigation
        autoHeight
      />
    </div>
  );
};

export default DispatchTable;
