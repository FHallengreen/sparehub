db = db.getSiblingDB('sparehub');

db.OrderBoxCollection.insertMany([
  {
    OrderId: 5,
    Boxes: [
      { Length: 100, Width: 50, Height: 40, Weight: 200 },
      { Length: 80, Width: 40, Height: 30, Weight: 150 }
    ]
  },
  {
    OrderId: 7,
    Boxes: [
      { Length: 120, Width: 60, Height: 45, Weight: 250 }
    ]
  },
  {
    OrderId: 9,
    Boxes: [
      { Length: 110, Width: 55, Height: 35, Weight: 220 }
    ]
  },
  {
    OrderId: 12,
    Boxes: [
      { Length: 90, Width: 45, Height: 40, Weight: 180 }
    ]
  },
  {
    OrderId: 13,
    Boxes: [
      { Length: 115, Width: 65, Height: 50, Weight: 260 }
    ]
  },
  {
    OrderId: 15,
    Boxes: [
      { Length: 105, Width: 60, Height: 45, Weight: 240 }
    ]
  },
  {
    OrderId: 16,
    Boxes: [
      { Length: 95, Width: 50, Height: 40, Weight: 190 }
    ]
  },
  {
    OrderId: 17,
    Boxes: [
      { Length: 110, Width: 55, Height: 35, Weight: 220 }
    ]
  },
  {
    OrderId: 19,
    Boxes: [
      { Length: 125, Width: 70, Height: 55, Weight: 280 }
    ]
  },
  {
    OrderId: 24,
    Boxes: [
      { Length: 130, Width: 75, Height: 60, Weight: 300 }
    ]
  },
  {
    OrderId: 25,
    Boxes: [
      { Length: 115, Width: 65, Height: 50, Weight: 260 }
    ]
  },
  {
    OrderId: 33,
    Boxes: [
      { Length: 110, Width: 55, Height: 35, Weight: 220 }
    ]
  },
  {
    OrderId: 17,
    Boxes: [
      { Length: 105, Width: 60, Height: 45, Weight: 240 }
    ]
  }
]);
