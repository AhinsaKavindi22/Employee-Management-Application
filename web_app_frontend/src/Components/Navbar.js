// Navbar.js
import React from 'react';
import '../css/navbar.css';  // Optional, if you want to add specific styles

const Navbar = () => {
  return (
    <div className="navbar">
      <div className="logo">MyApp</div>
      <div className="nav-links">
        <a href="#home">Home</a>
        <a href="#about">About</a>
        <a href="#services">Services</a>
        <a href="#contact">Contact</a>
      </div>
    </div>
  );
};

export default Navbar;