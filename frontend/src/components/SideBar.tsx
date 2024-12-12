import React from 'react';
import { NavLink, useNavigate } from 'react-router-dom';
import { FaBox, FaTruck, FaDatabase, FaSignOutAlt, FaUsers, FaShip, FaAnchor } from 'react-icons/fa';
import { GiHarborDock } from "react-icons/gi";
import sparehubLogo from '../assets/Sparehublogo_white_noname.svg';
import { useAuth } from '../context/AuthContext';
import axios from 'axios';

interface SidebarProps {
  isOpen: boolean;
  onClose: () => void;
}

const Sidebar: React.FC<SidebarProps> = ({ isOpen, onClose }) => {
  const { logout, token } = useAuth();
  const navigate = useNavigate();

  const handleLogout = async () => {
    try {
      await axios.post(
        `${import.meta.env.VITE_API_URL}/api/auth/logout`,
        {},
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      logout();
      navigate('/login');
    } catch (error) {
      console.error('Error during logout:', error);
      logout();
      navigate('/login');
    }
  };

  return (
    <div
      className={`fixed top-0 left-0 h-full bg-[#1d2b31] text-white flex flex-col items-center py-4 z-50 transform transition-transform duration-300 ease-in-out ${isOpen ? 'translate-x-0' : '-translate-x-full'
        } md:translate-x-0 sm:w-16 md:w-14`}
    >
      <button onClick={onClose} className="md:hidden mb-5 text-2xl">
        &times;
      </button>

      <NavLink to="/" className="mb-5">
        <img src={sparehubLogo} alt="SpareHub Logo" className="w-12 h-12" />
      </NavLink>

      <NavLink to="/owners" className="mb-5" title="Owners">
        <FaUsers size={28} />
      </NavLink>
      <NavLink to="/vessels" className="mb-5" title="Vessels">
        <FaShip size={28} />
      </NavLink>
      <NavLink to="/ports" className="mb-5" title="Ports">
        <FaAnchor size={28} />
      </NavLink>
      <NavLink to="/vessels-at-ports" className="mb-5" title="Vessels at Ports">
        <GiHarborDock size={28} />
      </NavLink>
      <NavLink to="/orders" className="mb-5" title="Orders">
        <FaBox size={28} />
      </NavLink>
      <NavLink to="/dispatches" className="mb-5" title="Dispatches">
        <FaTruck size={28} />
      </NavLink>
      <NavLink to="/database" className="mb-5">
        <FaDatabase size={28} />
      </NavLink>

      <button
        className="mb-5 flex flex-col items-center text-white hover:text-gray-300 focus:outline-none"
        onClick={handleLogout}
      >
        <FaSignOutAlt size={28} />
      </button>
    </div>
  );
};

export default Sidebar;
