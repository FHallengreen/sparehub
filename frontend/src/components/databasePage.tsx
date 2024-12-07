import * as React from 'react';
import { useState } from 'react';
import {Link} from "react-router-dom";
import {Button} from "@mui/material";
import { supportedTables} from "../helpers/FrontendDatabase"

const Databasepage: React.FC = () => {

    const tables = supportedTables
    return (
        <div>
            <h1>Database Tables</h1>
            {tables.length > 0 ? (
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
