import React from 'react';
import { NavLink } from 'react-router-dom';
import { FaBox, FaTruck, FaFileInvoice, FaCogs, FaDatabase } from 'react-icons/fa';
import sparehubLogo from '../assets/Sparehublogo_white_noname.svg';

interface SidebarProps {
    isOpen: boolean;
    onClose: () => void;
}

const Sidebar: React.FC<SidebarProps> = ({ isOpen, onClose }) => {
    return (
        <div
            className={`fixed top-0 left-0 h-full bg-[#1d2b31] text-white flex flex-col items-center py-4 z-50 transform transition-transform duration-300 ease-in-out ${isOpen ? 'translate-x-0' : '-translate-x-full'
                } md:translate-x-0 sm:w-16 md:w-16`}
        >
            {/* Close button for mobile */}
            <button onClick={onClose} className="md:hidden mb-5 text-2xl">
                &times;
            </button>

            {/* Logo */}
            <NavLink to="/" className="mb-5">
                <img src={sparehubLogo} alt="SpareHub Logo" className="w-12 h-12" />
            </NavLink>

            {/* Navigation Links */}
            <NavLink to="/orders" className="mb-5">
                <FaBox size={24} />
            </NavLink>
            <NavLink to="/dispatches" className="mb-5">
                <FaTruck size={24} />
            </NavLink>
            <NavLink to="/invoices" className="mb-5">
                <FaFileInvoice size={24} />
            </NavLink>
            <NavLink to="/settings" className="mb-5">
                <FaCogs size={24} />
            </NavLink>
            <NavLink to="/database" className="mb-5">
                <FaDatabase size={24} />
            </NavLink>
        </div>
    );
};

export default Sidebar;
