import { Routes, Route, useLocation } from 'react-router-dom';
import { useState } from 'react';
import { GiHamburgerMenu } from 'react-icons/gi';
import OrderPage from './components/OrderTable';
import OrderDetailPage from './components/OrderDetailPage';
import Sidebar from './components/SideBar';
import { SnackbarProvider } from './components/SnackbarContext';
import DispatchPage from "./components/DispatchPage.tsx";
import DispatchDetailPage from "./components/DispatchDetailPage.tsx";
import NewDispatchPage from "./components/NewDispatchPage.tsx";
import ProtectedRoute from './components/ProtectedRoute';
import LoginPage from './components/LoginPage';
import NotFoundPage from './components/NotFoundPage';
import { SnackbarProvider } from './components/SnackbarContext';
import { AuthProvider } from './context/AuthContext';

function App() {
  const [isSidebarOpen, setSidebarOpen] = useState(false);

  const toggleSidebar = () => {
    setSidebarOpen(!isSidebarOpen);
  };

  const closeSidebar = () => {
    setSidebarOpen(false);
  };

  const location = useLocation();
  const hideSidebar = location.pathname === '/login';

  return (
    <AuthProvider>
      <SnackbarProvider>
        <div className="flex h-screen">
          {!hideSidebar && (
            <>
              <button
                onClick={toggleSidebar}
                className="fixed top-3 left-4 z-50 md:hidden bg-white text-black w-9 h-9 p-1 rounded text-4xl shadow-lg"
              >
                <GiHamburgerMenu size={26} />
              </button>
              <Sidebar isOpen={isSidebarOpen} onClose={closeSidebar} />
            </>
          )}
          <div
            className={`flex-1 p-4 ${
              hideSidebar ? '' : 'ml-0 md:ml-16'
            } transition-all duration-300`}
          >
            <Routes>
              <Route path="/login" element={<LoginPage />} />
              <Route
                path="/orders/:id"
                element={
                  <ProtectedRoute>
                    <OrderDetailPage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/orders"
                element={
                  <ProtectedRoute>
                    <OrderPage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/"
                element={
                  <ProtectedRoute>
                    <OrderPage />
                  </ProtectedRoute>
                }
              />
              <Route path="*" element={<NotFoundPage />} />
            </Routes>
          </div>
        </div>
      </SnackbarProvider>
    </AuthProvider>
  );
}

export default App;
