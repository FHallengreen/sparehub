db = db.getSiblingDB('sparehub');

db.OrderBoxCollection.insertMany([
  {
    OrderId: 5,
    Boxes: [
      { BoxId: 1, Length: 100, Width: 50, Height: 40, Weight: 200 },
      { BoxId: 2, Length: 80, Width: 40, Height: 30, Weight: 150 }
    ]
  },
  {
    OrderId: 7,
    Boxes: [
      { BoxId: 3, Length: 120, Width: 60, Height: 45, Weight: 250 }
    ]
  },
  {
    OrderId: 9,
    Boxes: [
      { BoxId: 4, Length: 110, Width: 55, Height: 35, Weight: 220 }
    ]
  },
  {
    OrderId: 12,
    Boxes: [
      { BoxId: 5, Length: 90, Width: 45, Height: 40, Weight: 180 }
    ]
  },
  {
    OrderId: 13,
    Boxes: [
      { BoxId: 6, Length: 115, Width: 65, Height: 50, Weight: 260 }
    ]
  },
  {
    OrderId: 15,
    Boxes: [
      { BoxId: 7, Length: 105, Width: 60, Height: 45, Weight: 240 }
    ]
  },
  {
    OrderId: 16,
    Boxes: [
      { BoxId: 8, Length: 95, Width: 50, Height: 40, Weight: 190 }
    ]
  },
  {
    OrderId: 17,
    Boxes: [
      { BoxId: 9, Length: 110, Width: 55, Height: 35, Weight: 220 }
    ]
  },
  {
    OrderId: 19,
    Boxes: [
      { BoxId: 10, Length: 125, Width: 70, Height: 55, Weight: 280 }
    ]
  },
  {
    OrderId: 24,
    Boxes: [
      { BoxId: 11, Length: 130, Width: 75, Height: 60, Weight: 300 }
    ]
  },
  {
    OrderId: 25,
    Boxes: [
      { BoxId: 12, Length: 115, Width: 65, Height: 50, Weight: 260 }
    ]
  },
  {
    OrderId: 33,
    Boxes: [
      { BoxId: 13, Length: 110, Width: 55, Height: 35, Weight: 220 }
    ]
  },
  {
    OrderId: 17,
    Boxes: [
      { BoxId: 14, Length: 105, Width: 60, Height: 45, Weight: 240 }
    ]
  }
]);
