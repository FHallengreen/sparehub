import React, { useEffect, useState, useRef } from 'react';
import { GridRowSelectionModel } from '@mui/x-data-grid';
import DispatchFilter from '../components/DispatchFilter';
import DispatchGrid from '../components/DispatchGrid';
import { Dispatch } from '../../../interfaces/dispatch';
import { dispatchColumns } from '../columns/DispatchColumns.tsx';
import {getDispatches} from "../../../api/dispatchApi.ts";
import {useNavigate} from "react-router-dom";

const DispatchPage: React.FC = () => {
  const [rows, setRows] = useState<Dispatch[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [searchTags, setSearchTags] = useState<string[]>([]);
  const [selectionModel, setSelectionModel] = useState<GridRowSelectionModel>([]);
  const [suggestions, setSuggestions] = useState<string[]>([]);
  const searchBoxRef = useRef<HTMLInputElement>(null);

  const navigate = useNavigate();

  const fetchDispatches = async (tags: string[] = []) => {
    try {
      const response = await getDispatches(tags);

      setRows(response);
    } catch (err) {
      console.error('Error fetching dispatches:', err);
      setError('Failed to fetch dispatches.');
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
    fetchDispatches(searchTags);
  }, [searchTags]);

  useEffect(() => {
    if (rows.length > 0) {
      const uniqueTerms = Array.from(
        new Set(
          rows
            .flatMap((row) => [
              row.originType,
              row.destinationType,
              row.dispatchStatus,
              row.transportModeType,
            ])
            .filter(Boolean)
        )
      );

      setSuggestions(uniqueTerms);
    }
  }, [rows]);

  return (
    <div className="w-full">
      <DispatchFilter
        suggestions={suggestions}
        searchTags={searchTags}
        setSearchTags={setSearchTags}
        searchBoxRef={searchBoxRef}
      />
      <DispatchGrid
        rows={rows}
        columns={dispatchColumns} // Use the imported columns
        loading={loading}
        error={error}
        selectionModel={selectionModel}
        onSelectionModelChange={setSelectionModel}
        onRowDoubleClick={(params) => navigate(`/dispatches/${params.id}`)}
      />
    </div>
  );
};

export default DispatchPage;
