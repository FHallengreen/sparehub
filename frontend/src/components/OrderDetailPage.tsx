import * as React from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { CircularProgress, Typography, Button, TextField, IconButton } from '@mui/material';
import axios from 'axios';
import { Delete as DeleteIcon, Edit as EditIcon } from '@mui/icons-material';
import { OrderDetail, Box as OrderBox, VesselOption, OrderRequest, SupplierOption, Warehouse, Agent } from '../interfaces/order.ts';
import { useSnackbar } from './SnackbarContext';
import { useDebounce } from '../hooks/useDebounce';

const OrderDetailPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const isNewOrder = id === 'new';
    const navigate = useNavigate();
    const [order, setOrder] = React.useState<OrderDetail | null>(null);
    const [loading, setLoading] = React.useState<boolean>(true);
    const [error, setError] = React.useState<string | null>(null);
    const [editableBoxes, setEditableBoxes] = React.useState<boolean[]>([]);
    const [statuses, setStatuses] = React.useState<string[]>([]);
    const showSnackbar = useSnackbar();

    const [vesselOptions, setVesselOptions] = React.useState<VesselOption[]>([]);
    const [supplierOptions, setSupplierOptions] = React.useState<SupplierOption[]>([]);
    const [warehouseOptions, setWarehouseOptions] = React.useState<Warehouse[]>([]);
    const [agentOptions, setAgentOptions] = React.useState<Agent[]>([]);

    const [vesselQuery, setVesselQuery] = React.useState('');
    const [supplierQuery, setSupplierQuery] = React.useState('');
    const [warehouseQuery, setWarehouseQuery] = React.useState('');
    const [agentQuery, setAgentQuery] = React.useState('');

    const debouncedVesselQuery = useDebounce(vesselQuery, 300);
    const debouncedSupplierQuery = useDebounce(supplierQuery, 300);
    const debouncedWarehouseQuery = useDebounce(warehouseQuery, 300);
    const debouncedAgentQuery = useDebounce(agentQuery, 300);

    React.useEffect(() => {
        const fetchOrder = async () => {
            try {
                const statusResponse = await axios.get<string[]>(`${import.meta.env.VITE_API_URL}/api/order/status`);
                setStatuses(statusResponse.data);

                if (!isNewOrder) {
                    const orderResponse = await axios.get<OrderDetail>(`${import.meta.env.VITE_API_URL}/api/order/${id}`);

                    setOrder({
                        ...orderResponse.data,
                        boxes: orderResponse.data.boxes ?? []
                    });
                    setEditableBoxes(Array(orderResponse.data.boxes?.length ?? 0).fill(false));

                } else {
                    setOrder({
                        id: 0,
                        orderNumber: '',
                        supplierOrderNumber: '',
                        expectedReadiness: undefined,
                        actualReadiness: undefined,
                        expectedArrival: undefined,
                        actualArrival: undefined,
                        supplier: { id: 0, name: '' },
                        vessel: { id: 0, name: '', owner: { id: 0, name: '' } },
                        warehouse: { id: 0, name: '', agent: { id: 0, name: '' } },
                        orderStatus: 'Pending',
                        boxes: [],
                    });
                    setEditableBoxes([]);
                }
            } catch (error) {
                if (axios.isAxiosError(error) && error.response?.status === 404) {
                    showSnackbar('Order not found.', 'error');
                    navigate('/orders');
                } else {
                    setError('Failed to fetch order.');
                }
            } finally {
                setLoading(false);
            }
        };

        fetchOrder();
    }, [id, isNewOrder]);



    const sanitizeInput = (value: string) => {
        return value.replace(/[^a-zA-Z0-9\s]/g, '');
    };

    const handleInputChange = (field: keyof OrderDetail, value: string) => {
        if (order) {
            setOrder({ ...order, [field]: sanitizeInput(value) });
        }
    };

    const handleAddBox = () => {
        if (!order) return;

        const newBox: OrderBox = {
            id: undefined,
            length: 0,
            width: 0,
            height: 0,
            weight: 0
        };

        setOrder((prevOrder) => {
            if (!prevOrder) return null;
            const updatedBoxes = [...(prevOrder.boxes ?? []), newBox];
            return { ...prevOrder, boxes: updatedBoxes };
        });

        setEditableBoxes((prev) => [...prev, true]);
    };

    const handleRemoveBox = async (id: number) => {
        if (!order || !order.boxes) {
            showSnackbar('Unable to remove box. Order or boxes not found.', 'error');
            return;
        }

        console.log('Deleting box with ID:', id);

        if (id && id !== 0) {
            try {
                await axios.delete(`${import.meta.env.VITE_API_URL}/api/order/${order.id}/box/${id}`);
                showSnackbar('Box deleted successfully!', 'success');
            } catch (err) {
                showSnackbar('Failed to delete box. Please try again.', 'error');
                return;
            }
        }

        const updatedBoxes = order.boxes.filter((box) => box.id !== id);
        setOrder({ ...order, boxes: updatedBoxes });

        setEditableBoxes((prev) => prev.slice(0, updatedBoxes.length));
    };

    const toggleBoxEdit = (index: number) => {
        setEditableBoxes((prev) => {
            const updatedEditableBoxes = [...prev];
            updatedEditableBoxes[index] = !updatedEditableBoxes[index];
            return updatedEditableBoxes;
        });
    };

    const deleteOrder = async (orderId: number) => {
        try {
            await axios.delete(`${import.meta.env.VITE_API_URL}/api/order/${orderId}`);
            showSnackbar('Order deleted successfully!', 'success');
            navigate('/orders');
        } catch (err) {
            if (axios.isAxiosError(err) && err.response) {
                const { status } = err.response;
                if (status === 404) {
                    showSnackbar('Order not found.', 'error');
                } else {
                    showSnackbar('Failed to delete order.', 'error');
                }
            } else {
                showSnackbar('Unexpected error occurred.', 'error');
            }
        }
    };

    const handleSave = async () => {
        if (!order) return;

        const sanitizedOrder: OrderRequest = {
            orderNumber: sanitizeInput(order.orderNumber),
            supplierOrderNumber: sanitizeInput(order.supplierOrderNumber || ""),
            expectedReadiness: order.expectedReadiness || new Date(),
            actualReadiness: order.actualReadiness || undefined,
            expectedArrival: order.expectedArrival || undefined,
            actualArrival: order.actualArrival || undefined,
            supplierId: order.supplier.id,
            vesselId: order.vessel.id,
            warehouseId: order.warehouse.id,
            orderStatus: sanitizeInput(order.orderStatus),
            boxes: order.boxes?.map(box => ({
                id: box.id || undefined,
                length: box.length,
                width: box.width,
                height: box.height,
                weight: box.weight,
            })) || [],
        };

        try {
            if (isNewOrder) {
                const response = await axios.post(`${import.meta.env.VITE_API_URL}/api/order`, sanitizedOrder);
                setOrder(response.data);
                setEditableBoxes(Array(response.data.boxes?.length ?? 0).fill(false));
                showSnackbar('Order created successfully!', 'success');
            } else {
                await axios.put(`${import.meta.env.VITE_API_URL}/api/order/${id}`, sanitizedOrder);

                const response = await axios.get<OrderDetail>(`${import.meta.env.VITE_API_URL}/api/order/${id}`);
                setOrder(response.data);
                setEditableBoxes(Array(response.data.boxes?.length ?? 0).fill(false));

                showSnackbar('Order updated successfully!', 'success');
            }
        } catch (err) {
            showSnackbar('Failed to save order.', 'error');
        }
    };


    React.useEffect(() => {
        if (debouncedVesselQuery.length >= 3) {
            axios.get<VesselOption[]>(`${import.meta.env.VITE_API_URL}/api/vessel`, {
                params: { searchQuery: debouncedVesselQuery }
            })
                .then(response => {
                    setVesselOptions(response.data);
                })
                .catch(error => {
                    console.error('Failed to fetch vessels', error);
                });
        }
    }, [debouncedVesselQuery]);


    React.useEffect(() => {
        if (debouncedSupplierQuery.length >= 3) {
            axios.get<SupplierOption[]>(`${import.meta.env.VITE_API_URL}/api/supplier`, {
                params: { searchQuery: debouncedSupplierQuery }
            }).then(response => {
                setSupplierOptions(response.data);
            }).catch(error => {
                console.error('Failed to fetch suppliers', error);
            });
        }
    }, [debouncedSupplierQuery]);

    React.useEffect(() => {
        if (debouncedWarehouseQuery.length >= 3) {
            axios.get<Warehouse[]>(`${import.meta.env.VITE_API_URL}/api/warehouse/search`, {
                params: { searchQuery: debouncedWarehouseQuery }
            }).then(response => {
                setWarehouseOptions(response.data);
            }).catch(error => {
                console.error('Failed to fetch warehouses', error);
            });
        }
    }, [debouncedWarehouseQuery]);

    React.useEffect(() => {
        if (debouncedAgentQuery.length >= 3) {
            axios.get<Agent[]>(`${import.meta.env.VITE_API_URL}/api/agent/search`, {
                params: { searchQuery: debouncedAgentQuery }
            }).then(response => {
                setAgentOptions(response.data);
            }).catch(error => {
                console.error('Failed to fetch agents', error);
            });
        }
    }, [debouncedAgentQuery]);


    if (loading) {
        return <CircularProgress />;
    }

    if (error) {
        return <Typography color="error">{error}</Typography>;
    }

    return (
        <div className="container mx-auto p-6">
            {order && (
                <>
                    <div className="mt-8 gap-2 flex items-center">
                        {order.id !== 0 && (
                            <>
                                <Typography variant="h4" className="text-2xl font-bold mb-6 pb-4">
                                    Order ID: {order.id}
                                </Typography>
                                <IconButton
                                    onClick={() => {
                                        if (window.confirm("Are you sure you want to delete this order?")) {
                                            deleteOrder(order.id);
                                        }
                                    }}
                                    className="text-red-500"
                                >
                                    <DeleteIcon />
                                </IconButton>
                            </>
                        )}
                    </div>
                    <div className="grid grid-cols-2 gap-6 mb-6">
                        <div className="shadow-lg p-6 rounded-md bg-white">
                            <Typography variant="h5" className="font-bold mb-4 pb-4">ORDER FOR:</Typography>
                            <div className="flex flex-col gap-4">
                                <TextField
                                    label="Vessel Name"
                                    value={order.vessel.name}
                                    className="w-full"
                                    required
                                    onChange={(e) => {
                                        handleInputChange('vessel', e.target.value);
                                        setVesselQuery(e.target.value);
                                    }}
                                    onFocus={() => setVesselOptions([])}
                                    autoComplete="off"
                                />
                                {vesselOptions.length > 0 && (
                                    <div className="absolute bg-white border border-gray-300 rounded-md shadow-lg mt-12 max-h-40 overflow-y-auto z-50 w-1/3">
                                        {vesselOptions.map((option) => (
                                            <div
                                                key={option.id}
                                                onClick={() => {
                                                    setOrder((prevOrder) => {
                                                        if (prevOrder) {
                                                            return {
                                                                ...prevOrder,
                                                                vessel: {
                                                                    id: option.id,
                                                                    name: option.name,
                                                                    owner: option.owner,
                                                                },
                                                            };
                                                        } else {
                                                            return prevOrder;
                                                        }
                                                    });
                                                    setVesselQuery('');
                                                    setVesselOptions([]);
                                                }}

                                                className="px-4 py-2 cursor-pointer hover:bg-blue-100"
                                            >
                                                {option.name}
                                            </div>
                                        ))}
                                    </div>
                                )}

                                <TextField
                                    label="Owner Name"
                                    value={order.vessel.owner?.name || ''}
                                    className="w-full"
                                    disabled
                                />
                            </div>
                        </div>

                        <div className="shadow-lg p-6 rounded-md bg-white">
                            <Typography variant="h5" className="font-bold mb-4 pb-4">SUPPLIER:</Typography>
                            <div className="flex flex-col gap-4">
                                <TextField
                                    label="Supplier Name"
                                    value={order.supplier.name}
                                    className="w-full"
                                    required
                                    onChange={(e) => {
                                        handleInputChange('supplier', e.target.value);
                                        setSupplierQuery(e.target.value);
                                    }}
                                    onFocus={() => setSupplierOptions([])}
                                    autoComplete="off"
                                />
                                {supplierOptions.length > 0 && (
                                    <div className="absolute bg-white border border-gray-300 rounded-md shadow-lg mt-12 max-h-40 overflow-y-auto z-50 w-1/3">
                                        {supplierOptions.map((option) => (
                                            <div
                                                key={option.id}
                                                onClick={() => {
                                                    setOrder((prevOrder) => {
                                                        if (prevOrder) {
                                                            return {
                                                                ...prevOrder,
                                                                supplier: { id: option.id, name: option.name }
                                                            };
                                                        } else {
                                                            return prevOrder;
                                                        }
                                                    });
                                                    setSupplierQuery('');
                                                    setSupplierOptions([]);
                                                }}
                                                className="px-4 py-2 cursor-pointer hover:bg-blue-100"
                                            >
                                                {option.name}
                                            </div>
                                        ))}
                                    </div>
                                )}
                                <TextField
                                    label="Supplier Order Number"
                                    value={order.supplierOrderNumber}
                                    className="w-full"
                                    onChange={(e) => handleInputChange('supplierOrderNumber', e.target.value)}
                                />
                            </div>
                        </div>
                    </div>

                    <div className="grid grid-cols-2 gap-6 mb-6">
                        <div className="shadow-lg p-6 rounded-md bg-white">
                            <Typography variant="h5" className="font-bold mb-4 pb-4">ORDER DETAILS:</Typography>
                            <div className="flex flex-col gap-4">
                                <TextField
                                    label="Order Number"
                                    value={order.orderNumber}
                                    required
                                    className="w-full"
                                    onChange={(e) => handleInputChange('orderNumber', e.target.value)}
                                />

                                <TextField
                                    label="Warehouse Name"
                                    value={order.warehouse.name}
                                    required
                                    className="w-full"
                                    onChange={(e) => {
                                        handleInputChange('warehouse', e.target.value);
                                        setWarehouseQuery(e.target.value);
                                    }}
                                    onFocus={() => setWarehouseOptions([])}
                                    autoComplete="off"
                                />
                                {warehouseOptions.length > 0 && (
                                    <div className="absolute bg-white border border-gray-300 rounded-md shadow-lg mt-12 max-h-40 overflow-y-auto z-50 w-1/3">
                                        {warehouseOptions.map((option) => (
                                            <div
                                                key={option.id}
                                                onClick={() => {
                                                    setOrder((prevOrder) => {
                                                        if (prevOrder) {
                                                            return {
                                                                ...prevOrder,
                                                                warehouse: {
                                                                    id: option.id,
                                                                    name: option.name,
                                                                    agent: option.agent,
                                                                },
                                                            };
                                                        }
                                                        return prevOrder;
                                                    });
                                                    setWarehouseQuery('');
                                                    setWarehouseOptions([]);
                                                }}
                                                className="px-4 py-2 cursor-pointer hover:bg-blue-100"
                                            >
                                                {option.name}
                                            </div>
                                        ))}
                                    </div>
                                )}


                                <TextField
                                    label="Agent Name"
                                    value={order.warehouse.agent?.name || ''}
                                    className="w-full"
                                    disabled
                                />


                                {agentOptions.length > 0 && (
                                    <div className="absolute bg-white border border-gray-300 rounded-md shadow-lg mt-12 max-h-40 overflow-y-auto z-50 w-1/3">
                                        {agentOptions.map((option) => (
                                            <div
                                                key={option.id}
                                                onClick={() => {
                                                    setOrder((prevOrder) => {
                                                        if (prevOrder) {
                                                            return {
                                                                ...prevOrder,
                                                                agent: { id: option.id, name: option.name }
                                                            };
                                                        }
                                                        return prevOrder;
                                                    });
                                                    setAgentQuery('');
                                                    setAgentOptions([]);
                                                }}
                                                className="px-4 py-2 cursor-pointer hover:bg-blue-100"
                                            >
                                                {option.name}
                                            </div>
                                        ))}
                                    </div>
                                )}

                                <div className="w-full">
                                    <label className="block text-gray-700 text-sm font-bold mb-2">Order Status</label>
                                    <select
                                        value={order.orderStatus}
                                        onChange={(e) => handleInputChange('orderStatus', e.target.value)}
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
                    </div>
                    <Typography variant="h6" className="text-xl font-semibold mt-6 mb-4 pb-5">Boxes</Typography>

                    {order.boxes && order.boxes.map((box, index) => (
                        <div key={`${box.id}-${index}`} className="grid grid-cols-11 gap-3 mb-4">
                            <TextField
                                label="Length"
                                value={box.length}
                                onChange={(e) =>
                                    setOrder((prevOrder) => {
                                        if (!prevOrder) return null;
                                        const updatedBoxes = prevOrder.boxes?.map((b, i) =>
                                            i === index ? { ...b, length: parseFloat(e.target.value) } : b
                                        );
                                        return { ...prevOrder, boxes: updatedBoxes ?? [] };
                                    })
                                }
                                disabled={!editableBoxes[index]}
                                className="w-20"
                            />
                            <TextField
                                label="Width"
                                value={box.width}
                                onChange={(e) =>
                                    setOrder((prevOrder) => {
                                        if (!prevOrder) return null;
                                        const updatedBoxes = prevOrder.boxes?.map((b, i) =>
                                            i === index ? { ...b, width: parseFloat(e.target.value) } : b
                                        );
                                        return { ...prevOrder, boxes: updatedBoxes ?? [] };
                                    })
                                }
                                disabled={!editableBoxes[index]}
                                className="w-20"
                            />
                            <TextField
                                label="Height"
                                value={box.height}
                                onChange={(e) =>
                                    setOrder((prevOrder) => {
                                        if (!prevOrder) return null;
                                        const updatedBoxes = prevOrder.boxes?.map((b, i) =>
                                            i === index ? { ...b, height: parseFloat(e.target.value) } : b
                                        );
                                        return { ...prevOrder, boxes: updatedBoxes ?? [] };
                                    })
                                }
                                disabled={!editableBoxes[index]}
                                className="w-20"
                            />
                            <TextField
                                label="Weight (kg)"
                                value={box.weight}
                                onChange={(e) => {
                                    const value = parseFloat(e.target.value);
                                    setOrder((prevOrder) => {
                                        if (!prevOrder) return null;
                                        const updatedBoxes = prevOrder.boxes?.map((b, i) =>
                                            i === index ? { ...b, weight: value } : b
                                        );
                                        return { ...prevOrder, boxes: updatedBoxes ?? [] };
                                    });
                                }}
                                disabled={!editableBoxes[index]}
                                className="w-24"
                                inputProps={{
                                    type: "number",
                                    step: "0.1",
                                }}
                            />

                            <div>
                                <IconButton onClick={() => toggleBoxEdit(index)} className="text-gray-600">
                                    <EditIcon color={editableBoxes[index] ? 'primary' : 'inherit'} />
                                </IconButton>
                                <IconButton onClick={() => {
                                    if (box.id !== undefined) {
                                        handleRemoveBox(box.id);
                                    }
                                }} className="text-gray-600">
                                    <DeleteIcon />
                                </IconButton>
                            </div>
                        </div>
                    ))}

                    <Button onClick={handleAddBox} variant="outlined" className="mt-4" disabled={!order?.id || order.id === 0}>Add Box</Button>

                    <div className="mt-8 gap-2 flex">
                        <Button onClick={handleSave} variant="contained" color="primary" className="mr-2 pr-5">Save</Button>
                        <Button onClick={() => navigate('/orders')} variant="outlined">Back</Button>
                    </div>
                </>
            )
            }
        </div >
    );
};


export default OrderDetailPage;
