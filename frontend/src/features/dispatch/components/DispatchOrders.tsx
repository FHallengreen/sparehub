import React from 'react';
import { Typography } from '@mui/material';

interface DispatchOrdersProps {
  orders: any[];
}

const DispatchOrders: React.FC<DispatchOrdersProps> = ({ orders }) => {
  if (!orders || orders.length === 0) {
    return (
      <Typography variant="body2" color="textSecondary">
        No related orders.
      </Typography>
    );
  }

  return (
    <div className="shadow-lg p-6 rounded-md bg-white">
      <Typography variant="h5" className="font-bold mb-4 pb-4">
        RELATED ORDERS:
      </Typography>
      <div className="flex flex-wrap gap-2">
        {orders.map((order, index) => (
          <Typography
            key={order.id || index} // Use `id` if available, fallback to `index`
            variant="body1"
            className="bg-gray-100 px-4 py-2 rounded-md"
          >
            {/* Adjust rendering based on the structure of `order` */}
            {typeof order === 'string' || typeof order === 'number'
              ? order
              : `Order ID: ${order.id}, Status: ${order.status || 'N/A'}`}
          </Typography>
        ))}
      </div>
    </div>
  );
};

export default DispatchOrders;
