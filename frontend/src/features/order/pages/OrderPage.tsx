import React, { useState } from 'react';
import {Button, CircularProgress, Typography} from '@mui/material';
import OrderFilter from '../components/OrderFilter';
import OrderTable from '../components/OrderTable';
import {useNavigate} from "react-router-dom";

const OrderPage: React.FC = () => {
  const [loading, setLoading] = useState(true);
  const [error] = useState<string | null>(null);
  const [searchTags, setSearchTags] = useState<string[]>([]);
  const [suggestions, setSuggestions] = useState<string[]>([]);

  const navigate = useNavigate();

  // Fake loading for example purposes
  React.useEffect(() => {
    setTimeout(() => setLoading(false), 1000); // Replace with real data fetching
  }, []);

  if (loading) {
    return <CircularProgress />;
  }

  if (error) {
    return <Typography color="error">{error}</Typography>;
  }

  return (
    <div className="w-full">
      <div className="flex items-center space-x-2 mb-5">
        {/* Search bar */}
        <OrderFilter
          suggestions={suggestions}
          searchTags={searchTags}
          setSearchTags={setSearchTags}
        />
        {/* New Order button */}
        <Button onClick={() => navigate(`/orders/new`)} variant="contained" color="primary" className="pr-5">
          New Order
        </Button>
      </div>

      <OrderTable searchTags={searchTags} setSuggestions={setSuggestions}/>
    </div>
  );
};

export default OrderPage;
