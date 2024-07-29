// pages/login/LoginHome.js
import React, {useState } from "react";
import axios from 'axios';
import { Container, TextField, Button, Typography, Box } from '@mui/material';
import { useNavigate } from 'react-router-dom';

function LoginHome() {
  const [username, setUsername] = useState(''); // Kullanıcı adı
  const [password, setPassword] = useState(''); // Şifre
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();

    try {
      const response = await axios.post('https://localhost:7096/api/users/login', {
        username: username,
        password: password,
      });

      const user = response.data;

      // Kullanıcının rolüne göre yönlendir
      switch (user.role) {
        case 'Admin':
          navigate('/admin');
          break;
        case 'Employee':
          navigate('/employee');
          break;
        case 'Customer':
          navigate('/customer');
          break;
        default:
          alert('Geçersiz rol');
      }
    } catch (error) {
      alert('Kullanıcı adı veya şifre yanlış');
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
          Giriş Yap
        </Typography>
        <Box component="form" onSubmit={handleSubmit} sx={{ mt: 1 }}>
          <TextField
            margin="normal"
            required
            fullWidth
            id="username"
            label="Kullanıcı Adı"
            name="username"
            autoComplete="given-name"
            autoFocus
            value={username}
            onChange={(e) => setUsername(e.target.value)}
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
