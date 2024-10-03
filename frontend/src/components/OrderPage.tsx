import * as React from 'react';
import { DataGrid, GridColDef, GridToolbar } from '@mui/x-data-grid';
import { Chip, Dialog, DialogTitle, DialogContent, DialogActions, Button, TextField } from '@mui/material';

const columns: GridColDef[] = [
  { field: 'id', headerName: 'Order ID', flex: 0.4 },
  { field: 'owner', headerName: 'Owner', flex: 0.8 },
  { field: 'vessel', headerName: 'Vessel', flex: 1 },
  { field: 'poNumber', headerName: 'Client Ref', flex: 1 },
  { field: 'pieces', headerName: 'Pcs', flex: 0.25 },
  { field: 'weight', headerName: 'Weight', flex: 0.4 },
  { field: 'stockLocation', headerName: 'Stock Location', flex: 1 },
  {
    field: 'status',
    headerName: 'Status',
    flex: 0.5,
    renderCell: (params) => (
      <Chip
        label={params.value}
        color={params.value === 'Pending' ? 'success' : 'default'}
        variant="outlined"
      />
    ),
  },
];

const OrderTable: React.FC = () => {
  // Sample data to use without a backend
  const initialRows = [
    { id: 1, owner: 'V.Ships USA LLC', vessel: 'CSL Metis', poNumber: '2271-04262', pieces: 1, weight: 100, stockLocation: 'Amsterdam Warehouse', status: 'PFD' },
    { id: 2, owner: 'Flexco Ltd', vessel: 'Aurora', poNumber: '1345-673', pieces: 2, weight: 200, stockLocation: 'Amsterdam Warehouse', status: 'Pending' },
  ];

  const [rows, setRows] = React.useState<any[]>(initialRows);
  const [searchText, setSearchText] = React.useState('');
  const [open, setOpen] = React.useState(false);
  const [selectedOrder, setSelectedOrder] = React.useState<any>(null);

  const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchText(event.target.value);
  };

  const handleRowClick = (params: any) => {
    setSelectedOrder(params.row);
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
    setSelectedOrder(null);
  };

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setSelectedOrder((prev: any) => ({ ...prev, [name]: value }));
  };

  const handleSave = () => {
    // TODO:  Update the local state instead of making an API call
    setRows((prevRows) =>
      prevRows.map((row) => (row.id === selectedOrder.id ? selectedOrder : row))
    );
    setOpen(false);
  };

  // Filtered data based on search
  const filteredRows = rows.filter((row) =>
    row.owner?.toLowerCase().includes(searchText.toLowerCase()) ||
    row.vessel?.toLowerCase().includes(searchText.toLowerCase()) ||
    row.poNumber?.toLowerCase().includes(searchText.toLowerCase()) ||
    row.status?.toLowerCase().includes(searchText.toLowerCase()) ||
    row.id.toString().includes(searchText) ||
    row.pieces?.toString().includes(searchText) ||
    row.weight?.toString().includes(searchText)
  );

  return (
    <div style={{ height: '100vh', width: '100%' }}>
      <div className="mb-4">
        <input
          type="text"
          placeholder="Search orders..."
          value={searchText}
          onChange={handleSearchChange}
          className="p-2 border border-gray-300 rounded"
        />
      </div>

      <DataGrid
        rows={filteredRows}
        columns={columns}
        initialState={{
          pagination: {
            paginationModel: { pageSize: 100 },
          },
        }}
        pageSizeOptions={[100, 200, 500]}
        pagination
        checkboxSelection
        slots={{ toolbar: GridToolbar }}
        disableColumnFilter={false}
        disableColumnMenu={true}
        disableColumnSorting={true}
        disableRowSelectionOnClick={true}
        autoHeight
        onRowClick={handleRowClick}
      />

      <Dialog open={open} onClose={handleClose}>
        <DialogTitle>Edit Order</DialogTitle>
        <DialogContent>
          {selectedOrder && (
            <div>
              <TextField
                name="owner"
                label="Owner"
                value={selectedOrder.owner}
                onChange={handleInputChange}
                fullWidth
                margin="dense"
              />
              <TextField
                name="vessel"
                label="Vessel"
                value={selectedOrder.vessel}
                onChange={handleInputChange}
                fullWidth
                margin="dense"
              />
              <TextField
                name="poNumber"
                label="Client Ref"
                value={selectedOrder.poNumber}
                onChange={handleInputChange}
                fullWidth
                margin="dense"
              />
              <TextField
                name="pieces"
                label="Pieces"
                type="number"
                value={selectedOrder.pieces}
                onChange={handleInputChange}
                fullWidth
                margin="dense"
              />
              <TextField
                name="weight"
                label="Weight"
                type="number"
                value={selectedOrder.weight}
                onChange={handleInputChange}
                fullWidth
                margin="dense"
              />
              <TextField
                name="stockLocation"
                label="Stock Location"
                value={selectedOrder.stockLocation}
                onChange={handleInputChange}
                fullWidth
                margin="dense"
              />
              <TextField
                name="status"
                label="Status"
                value={selectedOrder.status}
                onChange={handleInputChange}
                fullWidth
                margin="dense"
              />
            </div>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose} color="primary">Cancel</Button>
          <Button onClick={handleSave} color="primary">Save</Button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default OrderTable;
