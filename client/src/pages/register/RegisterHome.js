// pages/register/RegisterHome.js
import React, { useState } from 'react';
import axios from 'axios';
import { Container, TextField, Button, Typography, Box } from '@mui/material';
import { useNavigate } from 'react-router-dom';

function RegisterHome() {
  const [customerName, setCustomerName] = useState('');
  const [contactName, setContactName] = useState('');
  const [address, setAddress] = useState('');
  const [city, setCity] = useState('');
  const [postalCode, setPostalCode] = useState('');
  const [country, setCountry] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();

    const newCustomer = {
      customerName: customerName, // string
      contactName: contactName,   // string
      address: address,           // string
      city: city,                 // string
      postalCode: postalCode,     // string
      country: country,           // string
      password: password          // string
    };

    try {
      const response = await axios.post('https://localhost:7096/api/customer', newCustomer, {
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        }
      });
      console.log('Response:', response.data);
      alert('Kayıt başarılı');
      navigate('/login');
    } catch (error) {
      console.error('Error registering customer:', error);
      if (error.response) {
        console.error('Response error data:', error.response.data);
        console.error('Response error status:', error.response.status);
        console.error('Response error headers:', error.response.headers);
      } else {
        console.error('Error message:', error.message);
      }
      alert('Kayıt sırasında bir hata oluştu');
    }
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
          Müşteri Kayıt Sistemi
        </Typography>
        <Box component="form" onSubmit={handleSubmit} sx={{ mt: 1 }}>
          <TextField
            margin="normal"
            required
            fullWidth
            id="customerName"
            label="Müşteri Adı"
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
            id="contactName"
            label="İletişim Adı"
            name="contactName"
            autoComplete="contact-name"
            value={contactName}
            onChange={(e) => setContactName(e.target.value)}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            id="address"
            label="Adres"
            name="address"
            autoComplete="address"
            value={address}
            onChange={(e) => setAddress(e.target.value)}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            id="city"
            label="Şehir"
            name="city"
            autoComplete="city"
            value={city}
            onChange={(e) => setCity(e.target.value)}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            id="postalCode"
            label="Posta Kodu"
            name="postalCode"
            autoComplete="postal-code"
            value={postalCode}
            onChange={(e) => setPostalCode(e.target.value)}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            id="country"
            label="Ülke"
            name="country"
            autoComplete="country"
            value={country}
            onChange={(e) => setCountry(e.target.value)}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            id="password"
            label="Şifre"
            name="password"
            type="password"
            autoComplete="current-password"
            value={password}
            onChange={(e) => setPassword(e.target.value.toString())}
          />
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            Kayıt Ol
          </Button>
        </Box>
      </Box>
    </Container>
  );
}

export default RegisterHome;
