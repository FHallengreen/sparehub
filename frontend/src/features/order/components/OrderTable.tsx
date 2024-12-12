import React, { useEffect, useState } from 'react';
import { DataGrid, GridRowSelectionModel } from '@mui/x-data-grid';
import { useNavigate } from 'react-router-dom';
import SummaryPanel from '../../../components/SummaryPanel';
import { fetchOrders } from '../../../api/orderApi.ts';
import { columns } from '../columns/OrderColumns.tsx';
import { OrderRow, OrderRowData } from "../../../interfaces/order.ts";

interface OrderTableProps {
  searchTags: string[];
  setSuggestions: (suggestions: string[]) => void;
}

const OrderTable: React.FC<OrderTableProps> = ({ searchTags, setSuggestions }) => {
  const [rows, setRows] = useState<OrderRow[]>([]);
  const [groupedRows, setGroupedRows] = useState<OrderRow[]>([]);
  const [selectionModel, setSelectionModel] = useState<GridRowSelectionModel>([]);
  const [selectedRows, setSelectedRows] = useState<OrderRow[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    const loadOrders = async () => {
      try {
        const data = await fetchOrders(searchTags);
        const mappedRows = data.map((order) => ({
          id: order.id.toString(),
          owner: order.ownerName.toString(),
          vessel: order.vesselName.toString(),
          supplier: order.supplierName.toString(),
          poNumber: order.orderNumber?.toString() ?? null,
          pieces: order.boxes ?? null,
          weight: order.totalWeight ?? null,
          volume: order.totalVolume ?? null,
          stockLocation: order.warehouseName.toString(),
          status: order.orderStatus,
        }));
        setRows(mappedRows);

        const allSuggestions = [...new Set(data.map((row) => row.warehouseName.toString()))];
        setSuggestions(allSuggestions);
      } catch (err) {
        console.error('Error fetching orders:', err);
      }
    };
    loadOrders();
  }, [searchTags]);

  useEffect(() => {
    const groupRows = (rows: OrderRow[]) => {
      const groupHeaders: OrderRow[] = [];
      const rowGroups: Record<string, Record<string, OrderRow[]>> = {};

      const isOrderRowData = (row: OrderRow): row is OrderRowData => {
        return (row as OrderRowData).status !== undefined;
      };

      rows.forEach((row) => {
        if (!isOrderRowData(row)) {
          console.warn(`Row with id ${row.id} is missing status or other OrderRowData properties.`);
          return;
        }

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
        groupHeaders.push({
          id: `header-${location}`,
          stockLocation: location,
          isGroupHeader: true,
        });

        Object.entries(statusGroups)
          .sort(([statusA], [statusB]) => statusB.localeCompare(statusA))
          .forEach(([, statusRows]) => {
            groupHeaders.push(...statusRows);
          });
      });


      return groupHeaders;
    };
    setGroupedRows(groupRows(rows));
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

        childRowIds.forEach((childId) => newSelectionSet.add(childId));
      } else {
        newSelectionSet.add(id as string);
      }
    });

    setSelectionModel(Array.from(newSelectionSet));

    const selectedDataRowIds = new Set(
      Array.from(newSelectionSet).filter(
        (id) => !groupHeaderIds.includes(id)
      )
    );

    const selectedData = groupedRows.filter(
      (row) => selectedDataRowIds.has(row.id) && !row.isGroupHeader
    );

    setSelectedRows(selectedData);
  };

  return (
    <>
      {selectedRows.length > 0 && (
        <div className="mt-5">
          <SummaryPanel selectedRows={new Set(selectedRows.map((row) => row.id))} allRows={rows} />
        </div>
      )}
      <DataGrid
        rows={groupedRows}
        columns={columns}
        checkboxSelection
        disableColumnSorting
        disableRowSelectionOnClick 
        onRowDoubleClick={(params) => {
          if (!params.row.isGroupHeader) {
            navigate(`/orders/${params.id}`);
          }
        }}
        rowSelectionModel={selectionModel}
        onRowSelectionModelChange={handleSelectionChange}
        getRowClassName={(params) =>
          params.row.isGroupHeader
            ? 'bg-gray-200 text-gray-700 font-semibold'
            : ''
        }
        autoHeight
      />
    </>
  );
};

export default OrderTable;
