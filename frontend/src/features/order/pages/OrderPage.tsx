import React, { useState } from 'react';
import { CircularProgress, Typography } from '@mui/material';
import OrderFilter from '../components/OrderFilter';
import OrderTable from '../components/OrderTable';

const OrderPage: React.FC = () => {
  const [loading, setLoading] = useState(true);
  const [error] = useState<string | null>(null);
  const [searchTags, setSearchTags] = useState<string[]>([]);
  const [suggestions, setSuggestions] = useState<string[]>([]);

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
      <OrderFilter
        suggestions={suggestions}
        searchTags={searchTags}
        setSearchTags={setSearchTags}
      />
      <OrderTable searchTags={searchTags} setSuggestions={setSuggestions} />
    </div>
  );
};

export default OrderPage;
