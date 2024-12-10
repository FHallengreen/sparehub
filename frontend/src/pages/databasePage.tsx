import * as React from 'react';
import {Link} from "react-router-dom";
import {Button} from "@mui/material";
import { supportedTables } from "../helpers/FrontendDatabase.tsx"

const Databasepage: React.FC = () => {

    return (
        <div>
            <h1>Database Tables</h1>
            {supportedTables.length > 0 ? (
                <ul>
                    {supportedTables.map(table => (
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
