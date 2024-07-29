import React, { useEffect, useState } from 'react';
import axios from 'axios';
import {
  Box,
  Typography,
  Paper,
  CircularProgress,
  Button,
} from '@mui/material';

const UserProfile = () => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchUserProfile = async () => {
      try {
        const response = await axios.get('https://localhost:7096/api/Users/profile', {
          headers: {
            accept: '*/*',
            Authorization: `Bearer YOUR_ACCESS_TOKEN`, // Buraya token'ınızı ekleyin
          },
        });
        setUser(response.data);
      } catch (err) {
        if (err.response) {
          // Sunucudan dönen hata mesajını JSON formatında al
          console.error('Response error:', err.response.data);
          setError(`Hata: ${JSON.stringify(err.response.data)}`);
        } else if (err.request) {
          // İstek yapıldı ama yanıt alınamadı
          console.error('Request error:', err.request);
          setError('İstek yapıldı ama yanıt alınamadı.');
        } else {
          // Hatanın sebebi başka bir şey
          console.error('General error:', err.message);
          setError(err.message);
        }
      } finally {
        setLoading(false);
      }
    };

    fetchUserProfile();
  }, []);

  if (loading) {
    return <CircularProgress />;
  }

  if (error) {
    return <Typography color="error">Bir hata oluştu: {error}</Typography>;
  }

  return (
    <Box sx={{ padding: 2, maxWidth: 600, margin: 'auto' }}>
      <Paper elevation={3} sx={{ padding: 3 }}>
        <Typography variant="h4" gutterBottom>
          Profil Bilgileri
        </Typography>
        <Typography variant="h6">Kullanıcı ID: {user.userId}</Typography>
        <Typography variant="h6">Kullanıcı Adı: {user.username}</Typography>
        <Typography variant="h6">Rol: {user.role}</Typography>
        <Typography variant="h6">Person ID: {user.personId}</Typography>
        <Button variant="contained" color="primary" sx={{ marginTop: 2 }}>
          Düzenle
        </Button>
      </Paper>
    </Box>
  );
};

export default UserProfile;
