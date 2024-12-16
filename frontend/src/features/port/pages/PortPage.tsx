import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { GridRowSelectionModel } from '@mui/x-data-grid';
import PortGrid from '../components/PortGrid';
import PortFilter from '../components/PortFilter';
import { portColumns } from '../columns/PortColumns';
import { getPorts } from '../../../api/portApi';
import { Port } from '../../../interfaces/port';
import { Typography } from '@mui/material';

const PortPage: React.FC = () => {
  const navigate = useNavigate();
  const [rows, setRows] = useState<Port[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [selectionModel, setSelectionModel] = useState<GridRowSelectionModel>([]);

  const fetchPorts = async () => {
    setLoading(true);
    try {
      const response = await getPorts();
      setRows(response);
    } catch (err) {
      console.error('Error fetching ports:', err);
      setError('Failed to fetch ports.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPorts();
  }, []);

  const filteredRows = rows.filter((row) =>
    row.name.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div className="w-full">
      
      <PortFilter
        suggestions={rows.map(row => row.name)}
        searchTerm={searchTerm}
        setSearchTerm={setSearchTerm}
      />
      {loading ? (
        <Typography>Loading...</Typography>
      ) : error ? (
        <Typography color="error">{error}</Typography>
      ) : (
        <PortGrid
          rows={filteredRows}
          columns={portColumns}
          loading={loading}
          error={error}
          selectionModel={selectionModel}
          onRowSelectionModelChange={setSelectionModel}
          onRowDoubleClick={(params: any) => {
            navigate(`/ports/${params.row.id}`);
          }}
        />
      )}
    </div>
  );
};

export default PortPage;