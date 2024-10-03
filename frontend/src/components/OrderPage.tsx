import * as React from 'react';
import { DataGrid, GridColDef, GridRowSelectionModel } from '@mui/x-data-grid';
import { Chip } from '@mui/material';

const columns: GridColDef[] = [
  { field: 'id', headerName: 'Order ID', flex: 0.4, headerAlign: 'center' },
  { field: 'owner', headerName: 'Owner', flex: 0.5 , headerAlign: 'center'},
  { field: 'vessel', headerName: 'Vessel', flex: 0.6, headerAlign: 'center' },
  { field: 'poNumber', headerName: 'Client Ref', flex: 1, headerAlign: 'center' },
  { field: 'pieces', headerName: 'Pcs', flex: 0.2 , headerAlign: 'center'},
  { field: 'weight', headerName: 'Weight', flex: 0.25 ,headerAlign: 'center' },
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
  const rows = [
    { id: 1, owner: 'V.Ships USA LLC', vessel: 'CSL Metis', poNumber: '2271-04262', pieces: 1, weight: 100, stockLocation: 'Amsterdam Warehouse', status: 'Stock' },
    { id: 2, owner: 'Flexco Ltd', vessel: 'Aurora', poNumber: '1345-673', pieces: 2, weight: 200, stockLocation: 'Amsterdam Warehouse', status: 'Inbound' },
    { id: 3, owner: 'Shell', vessel: 'Oceanic', poNumber: '4567-ABC', pieces: 3, weight: 300, stockLocation: 'Korea Warehouse', status: 'Pending' },
    { id: 4, owner: 'Maersk', vessel: 'Explorer', poNumber: '6789-XYZ', pieces: 4, weight: 400, stockLocation: 'Japan Warehouse', status: 'Stock' },
  ];

  const [selectedRows, setSelectedRows] = React.useState<GridRowSelectionModel>([]);

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
    // Check if a group header was selected
    const groupHeaders = groupedRows.filter((row) => row.isGroupHeader).map((row) => row.stockLocation);
    const selectedGroupHeader = newSelection.find((id) => groupHeaders.includes(id as string));

    if (selectedGroupHeader) {
      // Select all rows for this group header
      const groupRows = groupedRows
        .filter((row) => row.stockLocation === selectedGroupHeader && !row.isGroupHeader)
        .map((row) => row.id);
      setSelectedRows(groupRows);
    } else {
      setSelectedRows(newSelection);
    }
  };

  return (
    <div style={{ width: '100%' }}>
      <DataGrid
        rows={groupedRows}
        sx={{
          '& .MuiDataGrid-columnHeaders': {
            fontSize: '0.8rem', 
            textAlign: 'center',
          },
          '& .MuiDataGrid-cell': {
            fontSize: '0.8rem', 
            textAlign: 'center',
          },
          '& .MuiTablePagination-root': {
            fontSize: '0.875rem', 
            textAlign: 'center',
          },
        }}
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
