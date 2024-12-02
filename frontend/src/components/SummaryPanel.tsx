import React from 'react';
import { Typography, Button } from '@mui/material';
import { OrderRow, StockLocationSummary } from '../interfaces/order';

interface SummaryPanelProps {
  selectedRows: Set<string>;
  allRows: OrderRow[];
  dimensionalFactor?: number; // Add dimensional factor
}

type StockLocationData = {
    [location: string]: StockLocationSummary;
};

const SummaryPanel: React.FC<SummaryPanelProps> = ({
 selectedRows,
 allRows,
 dimensionalFactor = 6000, // Default to 6,000 cm³/kg
}) => {
  const selectedData = allRows.filter((row) => selectedRows.has(row.id));

  console.log('Selected Data:', selectedData);

  const stockLocationData = selectedData.reduce((acc: StockLocationData, row) => {
    const loc = row.stockLocation || 'Unknown';
    if (!acc[loc]) {
      acc[loc] = {
        orders: 0,
        pieces: 0,
        weight: 0,
        volume: 0,
        volumetricWeight: 0,
      };
    }
    acc[loc].orders += 1;
    acc[loc].pieces += row.pieces || 0;
    acc[loc].weight += row.weight || 0;
    acc[loc].volume += row.volume || 0;
    acc[loc].volumetricWeight += row.volume
      ? (row.volume * 1_000_000) / dimensionalFactor
      : 0;
    return acc;
  }, {} as StockLocationData);

  return (
    <div className="p-4 bg-gray-100 border rounded">
      <Typography variant="h6">Selected Orders by Stock Location:</Typography>
      {Object.entries(stockLocationData).map(([location, data]) => (
        <Typography key={location}>
          <strong>{location}</strong>: {data.orders} Orders | {data.pieces} Pieces | {data.weight} kg
          {data.volume > 0 && ` | ${new Intl.NumberFormat('en-US').format(data.volume)} m³`}
          {data.volumetricWeight > 0 &&
            ` | ${new Intl.NumberFormat('en-US', { maximumFractionDigits: 2 }).format(data.volumetricWeight)} kg Volumetric`}
        </Typography>
      ))}

      <Button
        variant="contained"
        color="primary"
        className="mt-4"
        onClick={() => {
          alert('Dispatch action triggered!');
        }}
      >
        Create Dispatch
      </Button>
    </div>
  );
};

export default SummaryPanel;
