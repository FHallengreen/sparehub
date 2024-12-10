import { Routes, Route, useLocation } from 'react-router-dom';
import { useState } from 'react';
import { GiHamburgerMenu } from 'react-icons/gi';
import OrderPage from './features/order/pages/OrderPage.tsx';
import OrderDetailPage from './features/order/pages/OrderDetailPage.tsx';
import Sidebar from './components/SideBar';
import { SnackbarProvider } from './context/SnackbarContext.tsx';
import DispatchPage from "./features/dispatch/pages/DispatchPage.tsx";
import DispatchDetailPage from "./features/dispatch/pages/DispatchDetailPage.tsx";
import NewDispatchPage from "./features/dispatch/pages/NewDispatchPage.tsx";
import ProtectedRoute from './components/ProtectedRoute';
import LoginPage from './pages/LoginPage.tsx';
import NotFoundPage from './pages/NotFoundPage.tsx';
import { AuthProvider } from './context/AuthContext';
import OwnerList from './features/owner/components/OwnerList.tsx';
import NewOwnerPage from './features/owner/pages/NewOwnerPage.tsx';
import OwnerDetailsPage from './features/owner/pages/OwnerDetailsPage.tsx';
import PortPage from './features/port/pages/PortPage.tsx';
import NewPortPage from './features/port/pages/NewPortPage.tsx';
import PortDetailsPage from './features/port/pages/PortDetailsPage.tsx';
import VesselPage from './features/vessel/pages/VesselPage.tsx';
import NewVesselPage from './features/vessel/pages/NewVesselPage.tsx';
import VesselDetailsPage from './features/vessel/pages/VesselDetailsPage.tsx';
import VesselAtPortPage from './features/vesselAtPort/pages/VesselAtPortPage.tsx';
import NewVesselAtPortPage from './features/vesselAtPort/pages/NewVesselAtPortPage.tsx';
import VesselAtPortDetailsPage from './features/vesselAtPort/pages/VesselAtPortDetailsPage.tsx';

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
                path="/dispatches/new"
                element={
                  <ProtectedRoute>
                    <NewDispatchPage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/dispatches/:id"
                element={
                  <ProtectedRoute>
                    <DispatchDetailPage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/dispatches"
                element={
                  <ProtectedRoute>
                    <DispatchPage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/owners/new"
                element={
                  <ProtectedRoute>
                    <NewOwnerPage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/owners/:id"
                element={
                  <ProtectedRoute>
                    <OwnerDetailsPage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/owners"
                element={
                  <ProtectedRoute>
                    <OwnerList />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/ports/new"
                element={
                  <ProtectedRoute>
                    <NewPortPage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/ports/:id"
                element={
                  <ProtectedRoute>
                    <PortDetailsPage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/ports"
                element={
                  <ProtectedRoute>
                    <PortPage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/vessels/new"
                element={
                  <ProtectedRoute>
                    <NewVesselPage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/vessels/:id"
                element={
                  <ProtectedRoute>
                    <VesselDetailsPage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/vessels"
                element={
                  <ProtectedRoute>
                    <VesselPage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/vessels-at-ports/new"
                element={
                  <ProtectedRoute>
                    <NewVesselAtPortPage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/vessels-at-ports/:id"
                element={
                  <ProtectedRoute>
                    <VesselAtPortDetailsPage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/vessels-at-ports"
                element={
                  <ProtectedRoute>
                    <VesselAtPortPage />
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
