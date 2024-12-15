import React, { useState, useEffect } from 'react';
import { TextField, Typography } from '@mui/material';
import { Warehouse, Agent } from '../../../interfaces/order'
import { useDebounce } from '../../../hooks/useDebounce';

interface OrderDetailsCardProps {
    orderNumber: string;
    warehouse: Warehouse;
    agent: Agent | null | undefined;
    orderStatus: string;
    statuses: string[];
    fetchWarehouseOptions: (query: string) => Promise<Warehouse[]>;
    fetchAgentOptions: (query: string) => Promise<Agent[]>;
    onOrderNumberChange: (orderNumber: string) => void;
    onWarehouseChange: (warehouse: Warehouse) => void;
    onAgentChange: (agent: Agent) => void;
    onOrderStatusChange: (status: string) => void;
}

const OrderDetailsCard: React.FC<OrderDetailsCardProps> = ({
    orderNumber,
    warehouse,
    agent,
    orderStatus,
    statuses,
    fetchWarehouseOptions,
    fetchAgentOptions,
    onOrderNumberChange,
    onWarehouseChange,
    onAgentChange,
    onOrderStatusChange,
}) => {
    const [warehouseQuery, setWarehouseQuery] = useState(warehouse.name);
    const [warehouseOptions, setWarehouseOptions] = useState<Warehouse[]>([]);
    const [agentQuery, setAgentQuery] = useState(agent?.name || '');
    const [agentOptions, setAgentOptions] = useState<Agent[]>([]);
    const debouncedWarehouseQuery = useDebounce(warehouseQuery, 500);

    useEffect(() => {
        const fetchOptions = async () => {
            if (debouncedWarehouseQuery.length >= 3) {
                try {
                    const options = await fetchWarehouseOptions(debouncedWarehouseQuery);
                    setWarehouseOptions(options);
                } catch (error) {
                    console.error('Failed to fetch warehouse options:', error);
                    setWarehouseOptions([]);
                }
            } else {
                setWarehouseOptions([]);
            }
        };

        fetchOptions();
    }, [debouncedWarehouseQuery, fetchWarehouseOptions]);

    useEffect(() => {
        if (agentQuery.length >= 3 && agentQuery !== agent?.name) {
            fetchAgentOptions(agentQuery)
                .then(setAgentOptions)
                .catch((err) => {
                    console.error('Failed to fetch agent options:', err);
                    setAgentOptions([]);
                });
        } else {
            setAgentOptions([]);
        }
    }, [agentQuery, fetchAgentOptions, agent?.name]);

    const handleWarehouseSelect = (selectedWarehouse: Warehouse) => {
        setWarehouseQuery(selectedWarehouse.name);
        setWarehouseOptions([]);
        onWarehouseChange(selectedWarehouse);
      
        if (selectedWarehouse.agent) {
          setAgentQuery(selectedWarehouse.agent.name);
        } else {
          setAgentQuery('');
        }
      };

    const handleAgentSelect = (selectedAgent: Agent) => {
        setAgentQuery(selectedAgent.name);
        setAgentOptions([]);
        onAgentChange(selectedAgent);
    };

    return (
        <div className="shadow-lg p-6 rounded-md bg-white">
            <Typography variant="h5" className="font-bold mb-4 pb-4">ORDER DETAILS:</Typography>
            <div className="flex flex-col gap-4">
    
                <div className="relative">
                    <TextField
                        label="Order Number"
                        value={orderNumber}
                        onChange={(e) => onOrderNumberChange(e.target.value)}
                        required
                        className="w-full"
                    />
                </div>
    
                <div className="relative">
                    <TextField
                        label="Warehouse Name"
                        value={warehouseQuery}
                        onChange={(e) => setWarehouseQuery(e.target.value)}
                        className="w-full"
                        required
                        autoComplete="off"
                    />
                    {warehouseQuery.length > 0 && warehouseQuery !== warehouse.name && warehouseOptions.length > 0 && (
                        <div className="absolute bg-white border border-gray-300 rounded-md shadow-lg mt-1 z-50 w-full max-h-40 overflow-y-auto">
                            {warehouseOptions.map((option) => (
                                <button
                                    key={option.id}
                                    onClick={() => handleWarehouseSelect(option)}
                                    className="px-4 py-2 cursor-pointer hover:bg-blue-100 text-left w-full"
                                >
                                    {option.name}
                                </button>
                            ))}
                        </div>
                    )}
                </div>
    
                <div className="relative">
                    <TextField
                        label="Agent Name"
                        value={agentQuery}
                        onChange={(e) => setAgentQuery(e.target.value)}
                        className="w-full"
                        disabled
                    />
                    {agentQuery.length > 0 && agentQuery !== agent?.name && agentOptions.length > 0 && (
                        <div className="absolute bg-white border border-gray-300 rounded-md shadow-lg mt-1 z-50 w-full max-h-40 overflow-y-auto">
                            {agentOptions.map((option) => (
                                <button
                                    key={option.id}
                                    onClick={() => handleAgentSelect(option)}
                                    className="px-4 py-2 cursor-pointer hover:bg-blue-100 text-left w-full"
                                >
                                    {option.name}
                                </button>
                            ))}
                        </div>
                    )}
                </div>
    
                <div className="relative">
                    <label htmlFor="orderStatus" className="block text-gray-700 text-sm font-bold mb-2">
                        Order Status
                    </label>
                    <select
                        id="orderStatus"
                        value={orderStatus}
                        onChange={(e) => onOrderStatusChange(e.target.value)}
                        className="w-full px-3 py-2 border rounded-md text-gray-700"
                    >
                        {statuses.map((status) => (
                            <option key={status} value={status}>
                                {status}
                            </option>
                        ))}
                    </select>
                </div>
    
            </div>
        </div>
    );
    
};


export default OrderDetailsCard;
