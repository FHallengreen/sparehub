import * as React from 'react';
import {useParams, useNavigate} from 'react-router-dom';
import {FaLink} from 'react-icons/fa6';
import {CircularProgress, Typography, Button, TextField, IconButton} from '@mui/material';
import {Delete as DeleteIcon, Edit as EditIcon} from '@mui/icons-material';
import {
  OrderDetail,
  Box as OrderBox,
  VesselOption,
  OrderRequest,
  SupplierOption,
  Warehouse,
  Agent, Box
} from '../../../interfaces/order.ts';
import {useSnackbar} from '../../../context/SnackbarContext.tsx';
import {useDebounce} from '../../../hooks/useDebounce.ts';
import api from '../../../api/api.ts';
import axios from 'axios';
import {useEffect} from "react";

const OrderDetailPage: React.FC = () => {
  const {id} = useParams<{ id: string }>();
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

  const [transporters] = React.useState<string[]>(['DHL', 'FEDEX', 'GLS']);
  const [selectedTransporter, setSelectedTransporter] = React.useState<string>('');
  const [trackingNumber, setTrackingNumber] = React.useState<string>('');
  const [trackingStatus, setTrackingStatus] = React.useState<{
    currentStep: string;
    statusDescription: string;
    location: string;
    timestamp: string;
    estimatedDelivery: string;
  } | null>(null);

  useEffect(() => {
    const fetchOrder = async () => {
      try {
        const statusResponse = await api.get<string[]>(`${import.meta.env.VITE_API_URL}/api/order/status`);
        setStatuses(statusResponse.data);

        if (!isNewOrder) {
          const orderResponse = await api.get<OrderDetail>(`${import.meta.env.VITE_API_URL}/api/order/${id}`);

          const fetchedOrder = orderResponse.data;

          setOrder({
            ...fetchedOrder,
            boxes: fetchedOrder.boxes ?? []
          });

          setSelectedTransporter(fetchedOrder.transporter || '');
          setTrackingNumber(fetchedOrder.trackingNumber || '');

          setEditableBoxes(Array(fetchedOrder.boxes?.length ?? 0).fill(false));
        } else {
          setOrder({
            id: 0,
            orderNumber: '',
            supplierOrderNumber: '',
            expectedReadiness: undefined,
            actualReadiness: undefined,
            expectedArrival: undefined,
            actualArrival: undefined,
            supplier: {id: 0, name: ''},
            vessel: {id: 0, name: '', owner: {id: 0, name: ''}},
            warehouse: {id: 0, name: '', agent: {id: 0, name: ''}},
            orderStatus: 'Pending',
            boxes: [],
            transporter: '',
            trackingNumber: '',
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
      setOrder({...order, [field]: sanitizeInput(value)});
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
      return {...prevOrder, boxes: updatedBoxes};
    });

    setEditableBoxes((prev) => [...prev, true]);
  };

  const deleteOrder = async (orderId: number) => {
    try {
      await api.delete(`${import.meta.env.VITE_API_URL}/api/order/${orderId}`);
      showSnackbar('Order deleted successfully!', 'success');
      navigate('/orders');
    } catch (err) {
      if (axios.isAxiosError(err) && err.response) {
        const {status} = err.response;
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
        id: box.id ?? undefined,
        length: box.length,
        width: box.width,
        height: box.height,
        weight: box.weight,
      })) || [],
      transporter: selectedTransporter,
      trackingNumber: trackingNumber,
    };

    try {
      if (isNewOrder) {
        const response = await api.post(`${import.meta.env.VITE_API_URL}/api/order`, sanitizedOrder);
        setOrder(response.data);
        setEditableBoxes(Array(response.data.boxes?.length ?? 0).fill(false));
        showSnackbar('Order created successfully!', 'success');
      } else {
        await api.put(`${import.meta.env.VITE_API_URL}/api/order/${id}`, sanitizedOrder);

        const response = await api.get<OrderDetail>(`${import.meta.env.VITE_API_URL}/api/order/${id}`);
        setOrder(response.data);
        setEditableBoxes(Array(response.data.boxes?.length ?? 0).fill(false));

        showSnackbar('Order updated successfully!', 'success');
      }
    } catch (err) {
      showSnackbar('Failed to save order.', 'error');
      console.error('Failed to save order:', err);
    }
  };
  const refreshTrackingStatus = async () => {
    if (!selectedTransporter || !trackingNumber) {
      showSnackbar('Please select a transporter and enter a tracking number.', 'warning');
      return;
    }

    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/api/order/${order?.id}/tracking/${trackingNumber}`);

      setTrackingStatus(response.data);
      showSnackbar('Tracking status updated.', 'success');
    } catch (error) {
      console.error('Failed to fetch tracking status', error);
      showSnackbar('Failed to fetch tracking status. Please try again.', 'error');
    }
  };

  const getDhlTrackingUrl = (trackingNumber: string) =>
    `https://www.dhl.com/us-en/home/tracking.html?tracking-id=${trackingNumber}&submit=1&inputsource=flyout`;

  useEffect(() => {
    if (debouncedVesselQuery.length >= 3) {
      api.get<VesselOption[]>(`${import.meta.env.VITE_API_URL}/api/vessel/query`, {
        params: {searchQuery: debouncedVesselQuery}
      })
        .then(response => {
          setVesselOptions(response.data);
        })
        .catch(error => {
          console.error('Failed to fetch vessels', error);
        });
    }
  }, [debouncedVesselQuery]);


  useEffect(() => {
    if (debouncedSupplierQuery.length >= 3) {
      api.get<SupplierOption[]>(`${import.meta.env.VITE_API_URL}/api/supplier`, {
        params: {searchQuery: debouncedSupplierQuery}
      }).then(response => {
        setSupplierOptions(response.data);
      }).catch(error => {
        console.error('Failed to fetch suppliers', error);
      });
    }
  }, [debouncedSupplierQuery]);

  useEffect(() => {
    if (debouncedWarehouseQuery.length >= 3) {
      api.get<Warehouse[]>(`${import.meta.env.VITE_API_URL}/api/warehouse/search`, {
        params: {searchQuery: debouncedWarehouseQuery}
      }).then(response => {
        setWarehouseOptions(response.data);
      }).catch(error => {
        console.error('Failed to fetch warehouses', error);
      });
    }
  }, [debouncedWarehouseQuery]);

  useEffect(() => {
    if (debouncedAgentQuery.length >= 3) {
      api.get<Agent[]>(`${import.meta.env.VITE_API_URL}/api/agent/search`, {
        params: {searchQuery: debouncedAgentQuery}
      }).then(response => {
        setAgentOptions(response.data);
      }).catch(error => {
        console.error('Failed to fetch agents', error);
      });
    }
  }, [debouncedAgentQuery]);

  const updateBoxField = (index: number, field: keyof Box, value: number) => {
    setOrder((prevOrder) => {
      if (!prevOrder) return null;
      const updatedBoxes = prevOrder.boxes?.map((b, i) =>
        i === index ? { ...b, [field]: value } : b
      );
      return { ...prevOrder, boxes: updatedBoxes ?? [] };
    });
  };

  const toggleBoxEdit = (index: number) => {
    setEditableBoxes((prev) => {
      const updatedEditableBoxes = [...prev];
      updatedEditableBoxes[index] = !updatedEditableBoxes[index];
      return updatedEditableBoxes;
    });
  };

  const handleRemoveBox = async (id: number | undefined) => {
    if (!order?.boxes || !id) {
      showSnackbar('Unable to remove box. Order or boxes not found.', 'error');
      return;
    }

    console.log('Deleting box with ID:', id);

    if (id && id !== 0) {
      try {
        await api.delete(`${import.meta.env.VITE_API_URL}/api/order/${order.id}/box/${id}`);
        showSnackbar('Box deleted successfully!', 'success');
      } catch (error) {
        showSnackbar('Failed to delete box. Please try again.', 'error');
        console.error('Failed to delete box:', error);
        return;
      }
    }

    const updatedBoxes = order.boxes.filter((box) => box.id !== id);
    setOrder({...order, boxes: updatedBoxes});

    setEditableBoxes((prev) => prev.slice(0, updatedBoxes.length));
  };

  if (loading) {
    return <CircularProgress/>;
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
                  <DeleteIcon/>
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
                  <div
                    className="absolute bg-white border border-gray-300 rounded-md shadow-lg mt-12 max-h-40 overflow-y-auto z-50 w-1/3">
                    {vesselOptions.map((option) => (
                      <button
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
                      </button>
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
                  <div
                    className="absolute bg-white border border-gray-300 rounded-md shadow-lg mt-12 max-h-40 overflow-y-auto z-50 w-1/3">
                    {supplierOptions.map((option) => (
                      <button
                        key={option.id}
                        onClick={() => {
                          setOrder((prevOrder) => {
                            if (prevOrder) {
                              return {
                                ...prevOrder,
                                supplier: {id: option.id, name: option.name},
                              };
                            } else {
                              return prevOrder;
                            }
                          });
                          setSupplierQuery('');
                          setSupplierOptions([]);
                        }}
                        className="px-4 py-2 cursor-pointer hover:bg-blue-100 text-left w-full"
                      >
                        {option.name}
                      </button>
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
                  <div
                    className="absolute bg-white border border-gray-300 rounded-md shadow-lg mt-12 max-h-40 overflow-y-auto z-50 w-1/3">
                    {warehouseOptions.map((option) => (
                      <button
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
                      </button>
                    ))}
                  </div>
                )}


                <TextField
                  label="Agent Name"
                  value={order.warehouse.agent?.name ?? ''}
                  className="w-full"
                  disabled
                />


                {agentOptions.length > 0 && (
                  <div
                    className="absolute bg-white border border-gray-300 rounded-md shadow-lg mt-12 max-h-40 overflow-y-auto z-50 w-1/3">
                    {agentOptions.map((option) => (
                      <button
                        key={option.id}
                        onClick={() => {
                          setOrder((prevOrder) => {
                            if (prevOrder) {
                              return {
                                ...prevOrder,
                                agent: {id: option.id, name: option.name}
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
                      </button>
                    ))}
                  </div>
                )}

                <div className="w-full">
                  <label
                    htmlFor="orderStatus"
                    className="block text-gray-700 text-sm font-bold mb-2"
                  >
                    Order Status
                  </label>
                  <select
                    id="orderStatus"
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
            <div className="shadow-lg p-6 rounded-md bg-white">
              <Typography variant="h5" className="font-bold mb-4 pb-4 text-gray-800">
                Transport and Tracking
              </Typography>
              <div className="flex flex-col gap-6">
                <div className="w-full">

                  <select
                    value={selectedTransporter}
                    onChange={(e) => setSelectedTransporter(e.target.value)}
                    className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring focus:border-blue-500"
                  >
                    <option value="" disabled>
                      Select Transporter
                    </option>
                    {transporters.map((transporter) => (
                      <option key={transporter} value={transporter}>
                        {transporter}
                      </option>
                    ))}
                  </select>
                </div>

                <div className="w-full">

                  <div className="flex items-center">
                    <TextField
                      label="Tracking Number"
                      value={trackingNumber}
                      onChange={(e) => setTrackingNumber(e.target.value)}
                      fullWidth
                      variant="outlined"
                    />
                    {selectedTransporter === 'DHL' && trackingNumber && (
                      <a
                        href={getDhlTrackingUrl(trackingNumber)}
                        target="_blank"
                        rel="noopener noreferrer"
                        className="ml-2 text-blue-500 hover:text-blue-700"
                        aria-label="Track package on DHL"
                      >
                        <FaLink size={20}/>
                      </a>
                    )}
                  </div>
                </div>


                <div className="flex justify-start mt-4">
                  <Button
                    onClick={refreshTrackingStatus}
                    variant="contained"
                    color="primary"
                  >
                    Refresh Status
                  </Button>
                </div>
              </div>

              {trackingStatus && (
                <div className="mt-6 p-4 border border-gray-300 rounded-md bg-gray-50">
                  <Typography variant="h6" className="font-bold text-gray-700 mb-3">
                    Current Status
                  </Typography>
                  <Typography className="text-gray-800">
                    <strong>Status:</strong> {trackingStatus.statusDescription}
                  </Typography>
                  <Typography className="text-gray-800">
                    <strong>Location:</strong> {trackingStatus.location}
                  </Typography>
                  <Typography className="text-gray-800">
                    <strong>Estimated Delivery:</strong> {trackingStatus.estimatedDelivery}
                  </Typography>
                </div>
              )}
            </div>
          </div>

          <Typography variant="h6" className="text-xl font-semibold mt-6 mb-4 pb-5">Boxes</Typography>

          {order?.boxes?.map((box, index) => (
            <div key={`${box.id}-${index}`} className="grid grid-cols-11 gap-3 mb-4">
              <TextField
                label="Length"
                value={box.length}
                onChange={(e) => updateBoxField(index, 'length', parseFloat(e.target.value))}
                disabled={!editableBoxes[index]}
                className="w-20"
              />
              <TextField
                label="Width"
                value={box.width}
                onChange={(e) => updateBoxField(index, 'width', parseFloat(e.target.value))}
                disabled={!editableBoxes[index]}
                className="w-20"
              />
              <TextField
                label="Height"
                value={box.height}
                onChange={(e) => updateBoxField(index, 'height', parseFloat(e.target.value))}
                disabled={!editableBoxes[index]}
                className="w-20"
              />
              <TextField
                label="Weight (kg)"
                value={box.weight}
                onChange={(e) => updateBoxField(index, 'weight', parseFloat(e.target.value))}
                disabled={!editableBoxes[index]}
                className="w-24"
                type="number"
              />

              <div>
                <IconButton onClick={() => toggleBoxEdit(index)} className="text-gray-600">
                  <EditIcon color={editableBoxes[index] ? 'primary' : 'inherit'} />
                </IconButton>
                <IconButton onClick={() => handleRemoveBox(box.id)} className="text-gray-600">
                  <DeleteIcon />
                </IconButton>
              </div>
            </div>
          ))}

          <Button onClick={handleAddBox} variant="outlined" className="mt-4" disabled={!order?.id || order.id === 0}>Add
            Box</Button>

          <div className="mt-8 gap-2 flex">
            <Button onClick={handleSave} variant="contained" color="primary" className="mr-2 pr-5">Save</Button>
            <Button onClick={() => navigate('/orders')} variant="outlined">Back</Button>
          </div>
        </>
      )
      }
    </div>
  );
};


export default OrderDetailPage;
