import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { useState } from 'react';
import { GiHamburgerMenu } from 'react-icons/gi';
import OrderPage from './components/OrderTable';
import OrderDetailPage from './components/OrderDetailPage';
import Sidebar from './components/SideBar';
import { SnackbarProvider } from './components/SnackbarContext';
import DatabasePage from './components/DatabasePage';
import MySQLTable from "./components/database/MySQLTable";

function App() {
  const [isSidebarOpen, setSidebarOpen] = useState(false);

  const toggleSidebar = () => {
    setSidebarOpen(!isSidebarOpen);
  };

  const closeSidebar = () => {
    setSidebarOpen(false);
  };

  return (
    <SnackbarProvider>
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
              <Route path="/orders/:id" element={<OrderDetailPage />} />
              <Route path="/orders" element={<OrderPage />} />
              <Route path="/" element={<OrderPage />} />
              <Route path="/database" element={<DatabasePage />} />
              <Route path="/database/:table" element={<MySQLTable />} />
            </Routes>
          </div>
        </div>
      </Router>
    </SnackbarProvider>
  );
}

export default App;
