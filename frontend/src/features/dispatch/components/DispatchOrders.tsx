import React from 'react';
import { Typography } from '@mui/material';

interface DispatchOrdersProps {
  orders: (string | number)[];
}

const DispatchOrders: React.FC<DispatchOrdersProps> = ({ orders }) => {
  return (
    <div className="shadow-lg p-6 rounded-md bg-white">
      <Typography variant="h5" className="font-bold mb-4 pb-4">RELATED ORDERS:</Typography>
      <div className="flex flex-wrap gap-2">
        {orders.length > 0 ? (
          orders.map((order) => (
            <Typography
              key={order.toString()}
              variant="body1"
              className="bg-gray-100 px-4 py-2 rounded-md"
            >
              {order}
            </Typography>
          ))
        ) : (
          <Typography variant="body2">No related orders.</Typography>
        )}
      </div>
    </div>
  );
};

export default DispatchOrders;
