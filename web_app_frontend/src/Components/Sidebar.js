// Sidebar.js
import React from 'react';
import '../css/sidebar.css';  // Optional, if you want to add specific styles

const Sidebar = () => {
  return (
    <div className="sidebar">
      <ul>
        <li><a href="#dashboard">EMS</a></li>
        <li><a href="#profile">Employees</a></li>
        <li><a href="#settings">Departments</a></li>
      </ul>
    </div>
  );
};

export default Sidebar;
