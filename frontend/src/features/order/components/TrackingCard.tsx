import React from 'react';
import { TextField, Typography, Button } from '@mui/material';
import { FaLink } from 'react-icons/fa';

interface TrackingCardProps {
  transporters: string[];
  fetchTrackingStatus: (trackingNumber: string, transporter: string) => Promise<any>;
  showSnackbar: (message: string, severity: 'success' | 'error' | 'warning') => void;
  transporter: string; 
  trackingNumber: string;
  onTrackingUpdate: (transporter: string, trackingNumber: string) => void;
}

const TrackingCard: React.FC<TrackingCardProps> = ({
  transporters,
  fetchTrackingStatus,
  showSnackbar,
  transporter,
  trackingNumber,
  onTrackingUpdate,
}) => {
  const [trackingStatus, setTrackingStatus] = React.useState<{
    currentStep: string;
    statusDescription: string;
    location: string;
    timestamp: string;
    estimatedDelivery: string;
  } | null>(null);

  const getDhlTrackingUrl = (trackingNumber: string) =>
    `https://www.dhl.com/us-en/home/tracking.html?tracking-id=${trackingNumber}&submit=1&inputsource=flyout`;

  const refreshTrackingStatus = async () => {
    if (!transporter || !trackingNumber) {
      showSnackbar('Please select a transporter and enter a tracking number.', 'warning');
      return;
    }

    try {
      const trackingData = await fetchTrackingStatus(trackingNumber, transporter);
      setTrackingStatus(trackingData);
      showSnackbar('Tracking status updated.', 'success');
    } catch (error) {
      console.error('Failed to fetch tracking status:', error);
      showSnackbar('Failed to fetch tracking status. Please try again.', 'error');
    }
  };

  return (
    <div className="shadow-lg p-6 rounded-md bg-white">
      <Typography variant="h5" className="font-bold mb-4 pb-4 text-gray-800">
        Transport and Tracking
      </Typography>
      <div className="flex flex-col gap-6">
        <div className="w-full">
          <select
            value={transporter}
            onChange={(e) => onTrackingUpdate(e.target.value, trackingNumber)}
            className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring focus:border-blue-500"
          >
            <option value="" disabled>
              Select Transporter
            </option>
            {transporters.map((trans) => (
              <option key={trans} value={trans}>
                {trans}
              </option>
            ))}
          </select>
        </div>

        <TextField
          label="Tracking Number"
          value={trackingNumber}
          onChange={(e) => onTrackingUpdate(transporter, e.target.value)}
          fullWidth
          variant="outlined"
        />
        {transporter === 'DHL' && trackingNumber && (
          <a
            href={getDhlTrackingUrl(trackingNumber)}
            target="_blank"
            rel="noopener noreferrer"
            className="ml-2 text-blue-500 hover:text-blue-700"
            aria-label="Track package on DHL"
          >
            <FaLink size={20} />
          </a>
        )}

        {trackingNumber && (
          <div className="flex justify-start mt-4">
            <Button
              onClick={refreshTrackingStatus}
              variant="contained"
              color="primary"
            >
              Refresh Status
            </Button>
          </div>
        )}
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
  );
};

export default TrackingCard;
