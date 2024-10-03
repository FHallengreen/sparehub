import { useState } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Sidebar from './components/SideBar';
import OrderPage from './components/OrderPage'; // Replace with actual component imports

function App() {
  const [isSidebarOpen, setSidebarOpen] = useState(false);

  const toggleSidebar = () => {
    setSidebarOpen(!isSidebarOpen);
  };

  const closeSidebar = () => {
    setSidebarOpen(false);
  };

  return (
    <Router>
      <div className="flex h-screen">
        <button
          onClick={toggleSidebar}
          className="fixed top-4 left-4 z-50 md:hidden text-white bg-[#1d2b31] p-2 rounded"
        >
          &#9776;
        </button>

        <Sidebar isOpen={isSidebarOpen} onClose={closeSidebar} />

        <div className="flex-1 p-4 ml-0 md:ml-16 transition-all duration-300">
          <Routes>
            <Route path="/orders" element={<OrderPage />} />
            <Route path="/" element={<OrderPage />} />
          </Routes>
        </div>
      </div>
    </Router>
  );
}

export default App;
