import React, { useEffect, useState, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { GridRowSelectionModel } from '@mui/x-data-grid';
import OwnerGrid from '../components/OwnerGrid';
import OwnerFilter from '../components/OwnerFilter';
import { Owner } from '../../../interfaces/owner';
import { ownerColumns } from '../columns/OwnerColumns';
import { getOwners } from '../../../api/ownerApi';

const OwnerPage: React.FC = () => {
  const navigate = useNavigate();
  const [rows, setRows] = useState<Owner[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [searchTags, setSearchTags] = useState<string[]>([]);
  const [selectionModel, setSelectionModel] = useState<GridRowSelectionModel>([]);
  const [suggestions, setSuggestions] = useState<string[]>([]);
  const searchBoxRef = useRef<HTMLInputElement>(null);

  const fetchOwners = async () => {
    try {
      const response = await getOwners();
      setRows(response);
    } catch (err) {
      console.error('Error fetching owners:', err);
      setError('Failed to fetch owners.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (searchBoxRef.current) {
      searchBoxRef.current.focus();
    }
  }, []);

  useEffect(() => {
    setLoading(true);
    fetchOwners();
  }, [searchTags]);

  useEffect(() => {
    if (rows.length > 0) {
      const uniqueTerms = Array.from(
        new Set(
          rows.flatMap((row) => [
            row.name,
            row.email,
            row.phone,
          ]).filter(Boolean)
        )
      );
      setSuggestions(uniqueTerms);
    }
  }, [rows]);

  return (
    <div className="w-full">
      <OwnerFilter
        suggestions={suggestions}
        searchTags={searchTags}
        setSearchTags={setSearchTags}
        searchBoxRef={searchBoxRef}
      />
      <OwnerGrid
        rows={rows}
        columns={ownerColumns}
        loading={loading}
        error={error}
        selectionModel={selectionModel}
        onRowSelectionModelChange={setSelectionModel}
        onRowDoubleClick={(params) => {
          navigate(`/owners/${params.row.id}`);
        }}
      />
    </div>
  );
};

export default OwnerPage;
