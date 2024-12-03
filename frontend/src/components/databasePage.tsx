import * as React from 'react';
import { useState, useEffect } from 'react';
import axios from "axios";
import {Link} from "react-router-dom";
import {Button} from "@mui/material";

const Databasepage: React.FC = () => {

    const [tables, setTables] = useState<string[] | null>(null);
    const [error, setError] = useState(null);

    useEffect(() => {

        axios.get(`${import.meta.env.VITE_API_URL}/api/database/tableNames`)
            .then(response => setTables(response.data))
            .catch(err => setError(err.message));

    }, []);
    return (
        <div>
            <h1>Database Tables</h1>
            {error && <p style={{ color: "red" }}>{error}</p>}
            {tables != null && tables.length > 0 ? (
                <ul>
                    {tables.map(table => (
                        <li key={table}>
                            <Button>
                                <Link to={`/database/${table}`}>{`Edit ${table}`}</Link> {}
                            </Button>

                        </li>
                    ))}
                </ul>
            ) : (
                <p>Loading tables...</p>
            )}
        </div>
    );
}
export default Databasepage;
