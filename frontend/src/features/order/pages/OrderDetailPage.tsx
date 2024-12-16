import * as React from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { CircularProgress, Typography, Button, IconButton } from '@mui/material';
import { Delete as DeleteIcon } from '@mui/icons-material';
import {
  OrderDetail,
  OrderRequest,
} from '../../../interfaces/order.ts';
import { useSnackbar } from '../../../context/SnackbarContext.tsx';
import api from '../../../api/api.ts';
import { useEffect } from "react";
import VesselCard from '../components/VesselCard.tsx';
import SupplierCard from '../components/SupplierCard.tsx';
import TrackingCard from '../components/TrackingCard.tsx';
import BoxList from '../components/BoxList.tsx';
import OrderDetailsCard from '../components/OrderDetailsCard.tsx';
import {
  fetchVesselOptions, fetchSupplierOptions, fetchWarehouseOptions, fetchAgentOptions
  , deleteOrderById, fetchOrderById, saveOrder, fetchTrackingStatus
} from '../../../api/orderApi.ts';

const OrderDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const isNewOrder = id === 'new';
  const navigate = useNavigate();
  const [order, setOrder] = React.useState<OrderDetail | null>(null);
  const [loading, setLoading] = React.useState<boolean>(true);
  const [statuses, setStatuses] = React.useState<string[]>([]);
  const showSnackbar = useSnackbar();

  useEffect(() => {
    const loadOrder = async () => {
      try {
        const statusResponse = await api.get<string[]>(`${import.meta.env.VITE_API_URL}/api/order/status`);
        setStatuses(statusResponse.data);

        if (isNewOrder) {
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
            transporter: '',
            trackingNumber: '',
          });
        } else {
          const fetchedOrder = await fetchOrderById(id ?? '');
          if (!fetchedOrder) {
            showSnackbar('Order not found.', 'error');
            navigate('/orders');
          } else {
            setOrder({
              ...fetchedOrder,
              boxes: fetchedOrder.boxes ?? [],
            });
          }
        }
      } catch (error) {
        showSnackbar('An error occurred.', 'error');
      } finally {
        setLoading(false);
      }
    };

    loadOrder();
  }, [id, isNewOrder]);


  const sanitizeInput = (value: string) => {
    return value.replace(/[^a-zA-Z0-9\s]/g, '');
  };

  const handleVesselChange = (updatedVessel: { id: number; name: string; owner?: { id: number; name: string } }) => {
    setOrder((prev) => {
      if (!prev) return null;

      return {
        ...prev,
        vessel: {
          id: updatedVessel.id,
          name: updatedVessel.name,
          owner: updatedVessel.owner ?? prev.vessel.owner,
        },
      };
    });
  };

  const handleSupplierChange = (updatedSupplier: { id: number; name: string }) => {
    setOrder((prev) => {
      if (!prev) return null;

      return {
        ...prev,
        supplier: {
          id: updatedSupplier.id,
          name: updatedSupplier.name,
        },
      };
    });
  };

  const handleOrderNumberChange = (updatedOrderNumber: string) => {
    setOrder((prev) => {
      if (!prev) return null;
      return { ...prev, orderNumber: updatedOrderNumber };
    });
  };

  const handleWarehouseChange = (updatedWarehouse: { id: number; name: string; agent?: { id: number; name: string } | null }) => {
    setOrder((prev) => {
      if (!prev) return null;

      return {
        ...prev,
        warehouse: { ...updatedWarehouse, agent: updatedWarehouse.agent || null },
      };
    });
  };

  const handleAgentChange = (updatedAgent: { id: number; name: string }) => {
    setOrder((prev) => {
      if (!prev) return null;
      return {
        ...prev,
        warehouse: { ...prev.warehouse, agent: updatedAgent },
      };
    });
  };

  const handleOrderStatusChange = (updatedStatus: string) => {
    setOrder((prev) => {
      if (!prev) return null;
      return { ...prev, orderStatus: updatedStatus };
    });
  };


  const handleSupplierOrderNumberChange = (updatedOrderNumber: string) => {
    setOrder((prev) => {
      if (!prev) return null;

      return {
        ...prev,
        supplierOrderNumber: updatedOrderNumber,
      };
    });
  };

  const handleTrackingUpdate = (transporter: string, trackingNumber: string) => {
    setOrder((prevOrder) => {
      if (!prevOrder) return null;
      return {
        ...prevOrder,
        transporter,
        trackingNumber,
      };
    });
  };


  const deleteOrder = async (orderId: number) => {
    try {
      await deleteOrderById(orderId);
      showSnackbar('Order deleted successfully!', 'success');
      navigate('/orders');
    } catch (error) {
      showSnackbar('Failed to delete order.', 'error');
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
      boxes: order.boxes?.map((box) => ({
        id: box.id ?? undefined,
        length: box.length,
        width: box.width,
        height: box.height,
        weight: box.weight,
      })) || [],
      transporter: order.transporter || "",
      trackingNumber: sanitizeInput(order.trackingNumber || ""),
    };

    try {
      const savedOrder = await saveOrder(sanitizedOrder, isNewOrder, id);
      setOrder(savedOrder);
      showSnackbar('Order saved successfully!', 'success');
    } catch (error) {
      showSnackbar('Failed to save order.', 'error');
    }
  };

  if (loading) {
    return <CircularProgress />;
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
            <VesselCard
              vessel={order.vessel}
              fetchVesselOptions={(query) => fetchVesselOptions(query)}
              onVesselChange={handleVesselChange}
            />

            <SupplierCard
              supplier={order.supplier}
              supplierOrderNumber={order.supplierOrderNumber}
              fetchSupplierOptions={fetchSupplierOptions}
              onSupplierChange={handleSupplierChange}
              onOrderNumberChange={handleSupplierOrderNumberChange}
            />


            <OrderDetailsCard
              orderNumber={order.orderNumber}
              warehouse={order.warehouse}
              agent={order.warehouse.agent}
              orderStatus={order.orderStatus}
              statuses={statuses}
              fetchWarehouseOptions={fetchWarehouseOptions}
              fetchAgentOptions={fetchAgentOptions}
              onOrderNumberChange={handleOrderNumberChange}
              onWarehouseChange={handleWarehouseChange}
              onAgentChange={handleAgentChange}
              onOrderStatusChange={handleOrderStatusChange}
            />


            <TrackingCard
              transporter={order.transporter}
              trackingNumber={order.trackingNumber}
              transporters={['DHL', 'FEDEX', 'GLS']}
              fetchTrackingStatus={(trackingNumber, transporter) =>
                fetchTrackingStatus(order?.id, transporter, trackingNumber)
              }
              showSnackbar={showSnackbar}
              onTrackingUpdate={handleTrackingUpdate}
            />


          </div>

          <BoxList
            initialBoxes={order?.boxes || []}
            orderId={order?.id || null}
            onBoxesUpdate={(updatedBoxes) =>
              setOrder((prevOrder) => (prevOrder ? { ...prevOrder, boxes: updatedBoxes } : null))
            }
            showSnackbar={showSnackbar}
          />


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