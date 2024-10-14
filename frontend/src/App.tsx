import { useState } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Sidebar from './components/SideBar';
import OrderPage from './components/OrderTable';
import OrderDetailPage from './components/OrderDetailPage';
import { GiHamburgerMenu } from 'react-icons/gi';

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
          className="fixed top-3 left-4 z-50 md:hidden bg-white text-black w-9 h-9 p-1 rounded text-4xl shadow-lg"
        >
          <GiHamburgerMenu size={26} />
        </button>

        <Sidebar isOpen={isSidebarOpen} onClose={closeSidebar} />

        <div className="flex-1 p-4 ml-0 md:ml-16 transition-all duration-300">
          <Routes>
            <Route path="/orders" element={<OrderPage />} />
            <Route path="/" element={<OrderPage />} />
            <Route path="/orders/:id" element={<OrderDetailPage />} />
          </Routes>
        </div>
      </div>
    </Router>
  );
}

export default App;
