import React, { useState } from 'react';
import { TextField, IconButton, Button, Typography } from '@mui/material';
import { Delete as DeleteIcon, Edit as EditIcon } from '@mui/icons-material';
import { Box as OrderBox } from '../../../interfaces/order';
import api from '../../../api/api.ts';

interface BoxListProps {
  initialBoxes: OrderBox[];
  orderId: number | null;
  onBoxesUpdate: (updatedBoxes: OrderBox[]) => void;
  showSnackbar: (message: string, type: 'success' | 'error' | 'warning') => void;
}

const BoxList: React.FC<BoxListProps> = ({
  initialBoxes,
  orderId,
  onBoxesUpdate,
  showSnackbar,
}) => {
  const [boxes, setBoxes] = useState<OrderBox[]>(initialBoxes);
  const [editableBoxes, setEditableBoxes] = useState<boolean[]>(
    Array(initialBoxes.length).fill(false)
  );

  const updateBoxField = (index: number, field: keyof OrderBox, value: number) => {
    const updatedBoxes = boxes.map((box, i) =>
      i === index ? { ...box, [field]: value } : box
    );
    setBoxes(updatedBoxes);
    onBoxesUpdate(updatedBoxes);
  };

  const toggleBoxEdit = (index: number) => {
    const updatedEditableBoxes = [...editableBoxes];
    updatedEditableBoxes[index] = !updatedEditableBoxes[index];
    setEditableBoxes(updatedEditableBoxes);
  };

  const handleRemoveBox = async (id: number | undefined) => {
    if (!id || !orderId) {
      showSnackbar('Unable to remove box. Order or boxes not found.', 'error');
      return;
    }

    try {
      await api.delete(`${import.meta.env.VITE_API_URL}/api/order/${orderId}/box/${id}`);
      const updatedBoxes = boxes.filter((box) => box.id !== id);
      setBoxes(updatedBoxes);
      setEditableBoxes((prev) => prev.slice(0, updatedBoxes.length));
      onBoxesUpdate(updatedBoxes);
      showSnackbar('Box deleted successfully!', 'success');
    } catch (error) {
      showSnackbar('Failed to delete box. Please try again.', 'error');
    }
  };

  const handleAddBox = () => {
    const newBox: OrderBox = {
      id: undefined,
      length: 0,
      width: 0,
      height: 0,
      weight: 0,
    };
    const updatedBoxes = [...boxes, newBox];
    setBoxes(updatedBoxes);
    setEditableBoxes((prev) => [...prev, true]);
    onBoxesUpdate(updatedBoxes);
  };

  return (
    <>
      <Typography variant="h6" className="text-xl font-semibold mt-6 mb-4 pb-5">
        Boxes
      </Typography>
      {boxes.map((box, index) => (
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
      <Button onClick={handleAddBox} variant="outlined" className="mt-4">
        Add Box
      </Button>
    </>
  );
};

export default BoxList;
