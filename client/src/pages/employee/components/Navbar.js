// components/Navbar.js
import React from 'react';
import { AppBar, Toolbar, Typography, Button } from '@mui/material';
import { Link } from 'react-router-dom';

function Navbar() {
  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h6" style={{ flexGrow: 1 }}>
          Employee
        </Typography>
        <Button color="inherit" component={Link} to="/employee">Home</Button>
        <Button color="inherit" component={Link} to="/employee/categories">Categories</Button>
        <Button color="inherit" component={Link} to="/employee/employees">Employees</Button>
        <Button color="inherit" component={Link} to="/employee/orderdetails">Order Details</Button>
        <Button color="inherit" component={Link} to="/employee/products">Products</Button>
        <Button color="inherit" component={Link} to="/employee/shippers">Shippers</Button>
        <Button color="inherit" component={Link} to="/employee/suppliers">Suppliers</Button>
        <Button color="inherit" component={Link} to="/employee/profile">Profile</Button>
        <Button color="inherit" component={Link} to="/">Çıkış Yap</Button>
      </Toolbar>
    </AppBar>
  );
}

export default Navbar;
