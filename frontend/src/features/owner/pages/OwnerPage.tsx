import React, { useEffect, useState } from 'react';
import OwnerGrid from '../components/OwnerGrid';
import OwnerFilter from '../components/OwnerFilter';
import { getOwners } from '../../../api/ownerApi';
import { ownerColumns } from '../columns/OwnerColumns';
import { Owner } from '../../../interfaces/owner';

const OwnerPage: React.FC = () => {
    const [owners, setOwners] = useState<Owner[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [searchTags, setSearchTags] = useState<string[]>([]);
    const [suggestions, setSuggestions] = useState<string[]>([]);

    const fetchOwners = async () => {
        setLoading(true);
        try {
            const response = await getOwners();
            setOwners(response);
            const uniqueNames = Array.from(new Set(response.map(owner => owner.name)));
            setSuggestions(uniqueNames);
        } catch (err) {
            console.error('Error fetching owners:', err);
            setError('Failed to fetch owners.');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchOwners();
    }, []);

    return (
        <div>
            <h1 className="text-2xl font-bold mb-4">Owner List</h1>
            <OwnerFilter
                suggestions={suggestions}
                searchTags={searchTags}
                setSearchTags={setSearchTags}
                searchBoxRef={React.createRef<HTMLInputElement>()}
            />
            <OwnerGrid
                rows={owners}
                columns={ownerColumns}
                loading={loading}
                error={error}
                selectionModel={[]}
                onRowSelectionModelChange={() => {}}
                onRowDoubleClick={() => {}}
            />
        </div>
    );
};

export default OwnerPage;
