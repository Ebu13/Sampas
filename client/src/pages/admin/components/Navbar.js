// components/Navbar.js
import React from 'react';
import { AppBar, Toolbar, Typography, Button } from '@mui/material';
import { Link } from 'react-router-dom';

function Navbar() {
  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h6" style={{ flexGrow: 1 }}>
          Admin
        </Typography>
        <Button color="inherit" component={Link} to="/admin">Home</Button>
        <Button color="inherit" component={Link} to="/admin/categories">Categories</Button>
        <Button color="inherit" component={Link} to="/admin/employees">Employees</Button>
        <Button color="inherit" component={Link} to="/admin/customers">Customers</Button>
        <Button color="inherit" component={Link} to="/admin/messages">Messages</Button>
        <Button color="inherit" component={Link} to="/admin/orderdetails">Order Details</Button>
        <Button color="inherit" component={Link} to="/admin/products">Products</Button>
        <Button color="inherit" component={Link} to="/admin/shippers">Shippers</Button>
        <Button color="inherit" component={Link} to="/admin/suppliers">Suppliers</Button>
        <Button color="inherit" component={Link} to="/">Çıkış Yap</Button>
      </Toolbar>
    </AppBar>
  );
}

export default Navbar;
