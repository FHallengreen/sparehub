import * as React from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { CircularProgress, Typography, Button, TextField, IconButton } from '@mui/material';
import axios from 'axios';
import { Delete as DeleteIcon, Edit as EditIcon } from '@mui/icons-material';
import { OrderDetail, Box as OrderBox } from '../interfaces/order';

const OrderDetailPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [order, setOrder] = React.useState<OrderDetail | null>(null);
    const [loading, setLoading] = React.useState<boolean>(true);
    const [error, setError] = React.useState<string | null>(null);
    const [editableBoxes, setEditableBoxes] = React.useState<boolean[]>([]);
    const [statuses, setStatuses] = React.useState<string[]>([]);

    React.useEffect(() => {
        const fetchOrder = async () => {
            try {
                const orderResponse = await axios.get<OrderDetail>(`${import.meta.env.VITE_API_URL}/api/orders/${id}`);
                const statusResponse = await axios.get<string[]>(`${import.meta.env.VITE_API_URL}/api/orders/statuses`);
    
                const orderData = orderResponse.data;
                setOrder({
                    ...orderData,
                    boxes: orderData.boxes ?? []
                });
                setStatuses(statusResponse.data);
                setEditableBoxes(Array((orderData.boxes ?? []).length).fill(false));
            } catch (err) {
                setError('Failed to fetch order.');
            } finally {
                setLoading(false);
            }
        };
        fetchOrder();
    }, [id]);
    

    const handleAddBox = () => {
        const newBox: OrderBox = { length: 0, width: 0, height: 0, weight: 0 };
        setOrder((prevOrder) => prevOrder ? { ...prevOrder, boxes: [...(prevOrder.boxes ?? []), newBox] } : null);
        setEditableBoxes((prev) => [...prev, true]);
    };

    const handleBoxChange = (index: number, field: keyof OrderBox, value: number) => {
        if (order) {
            const updatedBoxes = [...(order.boxes ?? [])];
            updatedBoxes[index] = { ...updatedBoxes[index], [field]: value };
            setOrder({ ...order, boxes: updatedBoxes });
        }
    };

    const handleRemoveBox = (index: number) => {
        if (order) {
            const updatedBoxes = [...(order.boxes ?? [])];
            updatedBoxes.splice(index, 1);
            setOrder({ ...order, boxes: updatedBoxes });
            setEditableBoxes((prev) => prev.filter((_, i) => i !== index));
        }
    };

    const toggleBoxEdit = (index: number) => {
        setEditableBoxes((prev) => {
            const updatedEditableBoxes = [...prev];
            updatedEditableBoxes[index] = !updatedEditableBoxes[index];
            return updatedEditableBoxes;
        });
    };

    const handleSave = async () => {
        try {
            await axios.put(`${import.meta.env.VITE_API_URL}/api/orders/${id}`, order);
            navigate('/orders');
        } catch (err) {
            setError('Failed to save order.');
        }
    };

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
                    <Typography variant="h4" className="text-2xl font-bold mb-6 pb-4">
                        Order / {order.id}
                    </Typography>

                    <div className="grid grid-cols-2 gap-6 mb-6">
                        <div className="shadow-lg p-6 rounded-md bg-white">
                            <Typography variant="h5" className="font-bold mb-4 pb-4">ORDER FOR:</Typography>
                            <div className="flex flex-col gap-4">
                                <TextField
                                    label="Vessel Name"
                                    value={order.vessel.name}
                                    className="w-full"
                                    onChange={(e) => setOrder({ ...order, vessel: { ...order.vessel, name: e.target.value } })}
                                />
                                <TextField label="Owner Name" value={order.owner.name} className="w-full" disabled />
                            </div>
                        </div>

                        <div className="shadow-lg p-6 rounded-md bg-white">
                            <Typography variant="h5" className="font-bold mb-4 pb-4">SUPPLIER:</Typography>
                            <div className="flex flex-col gap-4">
                                <TextField
                                    label="Supplier Name"
                                    value={order.supplier.name}
                                    className="w-full"
                                    onChange={(e) => setOrder({ ...order, supplier: { ...order.supplier, name: e.target.value } })}
                                />
                                <TextField
                                    label="Supplier Order Number"
                                    value={order.supplierOrderNumber}
                                    className="w-full"
                                    onChange={(e) => setOrder({ ...order, supplierOrderNumber: e.target.value })}
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
                                    className="w-full"
                                    onChange={(e) => setOrder({ ...order, orderNumber: e.target.value })}
                                />
                                <TextField
                                    label="Warehouse Name"
                                    value={order.warehouse.name}
                                    className="w-full"
                                    onChange={(e) => setOrder({ ...order, warehouse: { ...order.warehouse, name: e.target.value } })}
                                />
                                <TextField
                                    label="Agent Name"
                                    value={order.agent.name}
                                    className="w-full"
                                    onChange={(e) => setOrder({ ...order, agent: { ...order.agent, name: e.target.value } })}
                                />

                                <div className="w-full">
                                    <label className="block text-gray-700 text-sm font-bold mb-2">Order Status</label>
                                    <select
                                        value={order.orderStatus}
                                        onChange={(e) => setOrder({ ...order, orderStatus: e.target.value })}
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
                        <div key={index} className="grid grid-cols-11 gap-3 mb-4">
                            <TextField label="Length" value={box.length} onChange={(e) => handleBoxChange(index, 'length', parseFloat(e.target.value))} disabled={!editableBoxes[index]} className="w-20" />
                            <TextField label="Width" value={box.width} onChange={(e) => handleBoxChange(index, 'width', parseFloat(e.target.value))} disabled={!editableBoxes[index]} className="w-20" />
                            <TextField label="Height" value={box.height} onChange={(e) => handleBoxChange(index, 'height', parseFloat(e.target.value))} disabled={!editableBoxes[index]} className="w-20" />
                            <TextField label="Weight" value={box.weight} onChange={(e) => handleBoxChange(index, 'weight', parseFloat(e.target.value))} disabled={!editableBoxes[index]} className="w-24" />
                            <div>
                                <IconButton onClick={() => toggleBoxEdit(index)} className="text-gray-600">
                                    <EditIcon color={editableBoxes[index] ? 'primary' : 'inherit'} />
                                </IconButton>
                                <IconButton onClick={() => handleRemoveBox(index)} className="text-gray-600">
                                    <DeleteIcon />
                                </IconButton>
                            </div>
                        </div>
                    ))}

                    <Button onClick={handleAddBox} variant="outlined" className="mt-4">Add Box</Button>

                    <div className="mt-8 gap-2">
                        <Button onClick={handleSave} variant="contained" color="primary" className="mr-2 pr-5">Save</Button>
                        <Button onClick={() => navigate('/orders')} variant="outlined">Cancel</Button>
                    </div>

                </>
            )}
        </div>
    );
};

export default OrderDetailPage;
