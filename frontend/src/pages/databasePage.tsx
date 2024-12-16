import React from 'react';
import { Link } from 'react-router-dom';
import { supportedTables } from '../helpers/FrontendDatabase.tsx';

const Databasepage: React.FC = () => {
    return (
        <div className="min-h-screen bg-gray-100 p-6 flex flex-col items-center">
            <h1 className="text-3xl font-bold text-gray-800 mb-8">Database Tables</h1>
            {supportedTables.length > 0 ? (
                <div className="grid grid-cols-3 gap-6 w-full max-w-4xl">
                    {supportedTables.map((table) => (
                        <Link
                            key={table}
                            to={`/database/${table}`}
                            className="flex items-center justify-center bg-blue-600 text-white text-lg font-semibold h-40 rounded-md shadow-md hover:bg-blue-700 transition-colors"
                        >
                            {`Edit ${table}`}
                        </Link>
                    ))}
                </div>
            ) : (
                <p className="text-gray-600">Loading tables...</p>
            )}
        </div>
    );
};

export default Databasepage;
