import React from 'react';
import { DataGrid, GridRowSelectionModel } from '@mui/x-data-grid';
import { useNavigate } from 'react-router-dom';
import SummaryPanel from '../../../components/SummaryPanel';
import { fetchOrders } from '../../../api/orderApi.ts';
import { columns } from '../columns/OrderColumns.tsx';

interface OrderTableProps {
  searchTags: string[];
  setSuggestions: (suggestions: string[]) => void;
}

const OrderTable: React.FC<OrderTableProps> = ({ searchTags, setSuggestions }) => {
  const [rows, setRows] = React.useState<any[]>([]);
  const [selectionModel, setSelectionModel] = React.useState<GridRowSelectionModel>([]);
  const navigate = useNavigate();

  React.useEffect(() => {
    const loadOrders = async () => {
      try {
        const data = await fetchOrders(searchTags);
        setRows(data);
        const allSuggestions = [...new Set(data.map((row) => row.stockLocation))];
        setSuggestions(allSuggestions);
      } catch (err) {
        console.error('Error fetching orders:', err);
      }
    };
    loadOrders();
  }, [searchTags, setSuggestions]);

  return (
    <>
      <DataGrid
        rows={rows}
        columns={columns}
        checkboxSelection
        onRowDoubleClick={(params) => navigate(`/orders/${params.id}`)}
        rowSelectionModel={selectionModel}
        onRowSelectionModelChange={(newSelection) => setSelectionModel(newSelection)}
        autoHeight
      />
      {selectionModel.length > 0 && (
        <SummaryPanel selectedRows={new Set(selectionModel)} allRows={rows} />
      )}
    </>
  );
};

export default OrderTable;
