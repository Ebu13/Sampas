// components/Navbar.js
import React from 'react';
import { AppBar, Toolbar, Typography, Button } from '@mui/material';
import { Link } from 'react-router-dom';

function Navbar() {
  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h6" style={{ flexGrow: 1 }}>
          Customer
        </Typography> 
        <Button color="inherit" component={Link} to="/customer">Home</Button>
        <Button color="inherit" component={Link} to="/customer/categories">Categories</Button>
        <Button color="inherit" component={Link} to="/customer/chat">ChatPage</Button>
        <Button color="inherit" component={Link} to="/customer/orderdetails">Order Details</Button>
        <Button color="inherit" component={Link} to="/customer/products">Products</Button>
        <Button color="inherit" component={Link} to="/customer/shippers">Shippers</Button>
        <Button color="inherit" component={Link} to="/customer/suppliers">Suppliers</Button>
        <Button color="inherit" component={Link} to="/customer/profile">Profile</Button>
        <Button color="inherit" component={Link} to="/">Çıkış Yap</Button>
      </Toolbar>
    </AppBar>
  );
}

export default Navbar;
