import React from 'react';
import { Link } from 'react-router-dom';

const Header = ({ onLogout }) => {
  return (
    <header className="header">
      <div className="header-content">
        <h1>📚 Student Management System</h1>
        <nav className="nav-links">
          <Link to="/">Students</Link>
          <Link to="/add">Add Student</Link>
          <button
            className="btn btn-sm btn-danger"
            onClick={onLogout}
          >
            Logout
          </button>
        </nav>
      </div>
    </header>
  );
};

export default Header;
