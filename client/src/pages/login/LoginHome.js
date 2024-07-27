// pages/login/LoginHome.js
import React, { useEffect, useState } from "react";
import axios from 'axios';
import { Container, TextField, Button, Typography, Box } from '@mui/material';
import { useNavigate } from 'react-router-dom';

function LoginHome() {
  const [customers, setCustomers] = useState([]);
  const [employees, setEmployees] = useState([]);
  const [customerName, setCustomerName] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    axios.get('https://localhost:7096/api/customer')
      .then(response => {
        setCustomers(response.data.$values);
      })
      .catch(error => {
        console.error('Error fetching customers:', error);
      });

    axios.get('https://localhost:7096/api/employee')
      .then(response => {
        setEmployees(response.data.$values);
      })
      .catch(error => {
        console.error('Error fetching employees:', error);
      });
  }, []);

  const handleSubmit = (event) => {
    event.preventDefault();

    // Check if customer
    const customer = customers.find(cust => cust.customerName === customerName && cust.password === password);
    if (customer) {
      navigate('/customer');
      return;
    }

    // Check if employee
    const employee = employees.find(emp => emp.firstName === customerName && emp.password === password);
    if (employee) {
      if (employee.employeeId === 1) {
        navigate('/admin');
      } else {
        navigate('/employee');
      }
      return;
    }

    // If neither found
    alert('Kullanıcı adı veya şifre yanlış');
  };

  return (
    <Container maxWidth="xs">
      <Box
        sx={{
          marginTop: 8,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
        }}
      >
        <Typography component="h1" variant="h5">
          Giriş Yap
        </Typography>
        <Box component="form" onSubmit={handleSubmit} sx={{ mt: 1 }}>
          <TextField
            margin="normal"
            required
            fullWidth
            id="customerName"
            label="İsim"
            name="customerName"
            autoComplete="given-name"
            autoFocus
            value={customerName}
            onChange={(e) => setCustomerName(e.target.value)}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            name="password"
            label="Şifre"
            type="password"
            id="password"
            autoComplete="current-password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            Giriş Yap
          </Button>
        </Box>
      </Box>
    </Container>
  );
}

export default LoginHome;
