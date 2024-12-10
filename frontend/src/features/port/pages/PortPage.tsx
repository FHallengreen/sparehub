import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { GridRowSelectionModel } from '@mui/x-data-grid';
import PortGrid from '../components/PortGrid';
import PortFilter from '../components/PortFilter';
import { portColumns } from '../columns/PortColumns';
import { getPorts } from '../../../api/portApi';
import { Port } from '../../../interfaces/port';

const PortPage: React.FC = () => {
  const navigate = useNavigate();
  const [rows, setRows] = useState<Port[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [searchTags, setSearchTags] = useState<string[]>([]);
  const [selectionModel, setSelectionModel] = useState<GridRowSelectionModel>([]);
  const [suggestions, setSuggestions] = useState<string[]>([]);

  const fetchPorts = async () => {
    setLoading(true);
    try {
      const response = await getPorts();
      setRows(response);
      // Update suggestions based on the data
      const uniqueTerms = Array.from(
        new Set(response.map((row: Port) => row.name))
      );
      setSuggestions(uniqueTerms);
    } catch (err) {
      console.error('Error fetching ports:', err);
      setError('Failed to fetch ports.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    setLoading(true);
    fetchPorts();
  }, [searchTags]);

  return (
    <div className="w-full">
      <PortFilter
        suggestions={suggestions}
        searchTags={searchTags}
        setSearchTags={setSearchTags}
      />
      <PortGrid
        rows={rows}
        columns={portColumns}
        loading={loading}
        error={error}
        selectionModel={selectionModel}
        onRowSelectionModelChange={setSelectionModel}
        onRowDoubleClick={(params) => {
          navigate(`/ports/${params.row.id}`);
        }}
      />
    </div>
  );
};

export default PortPage; 