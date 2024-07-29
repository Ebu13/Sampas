import React, { useEffect, useState } from 'react';
import axios from 'axios';
import {
  Box,
  Typography,
  Paper,
  CircularProgress,
  Button,
  TextField,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
} from '@mui/material';

const UserProfile = () => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [editMode, setEditMode] = useState(false);
  const [updatedUser, setUpdatedUser] = useState({
    username: '',
    role: '',
  });

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
        setUpdatedUser({
          username: response.data.username,
          role: response.data.role,
        });
      } catch (err) {
        if (err.response) {
          console.error('Response error:', err.response.data);
          setError(`Hata: ${JSON.stringify(err.response.data)}`);
        } else if (err.request) {
          console.error('Request error:', err.request);
          setError('İstek yapıldı ama yanıt alınamadı.');
        } else {
          console.error('General error:', err.message);
          setError(err.message);
        }
      } finally {
        setLoading(false);
      }
    };

    fetchUserProfile();
  }, []);

  const handleEdit = () => {
    setEditMode(true);
  };

  const handleClose = () => {
    setEditMode(false);
  };

  const handleSave = async () => {
    try {
      await axios.put(`https://localhost:7096/api/Users/${user.userId}`, updatedUser, {
        headers: {
          accept: '*/*',
          Authorization: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwidW5pcXVlX25hbWUiOiJFYnViZWtpciBTxLFkZMSxayBOYXpsxLEiLCJyb2xlIjoiQWRtaW4iLCJuYmYiOjE3MjIyNTg2NTUsImV4cCI6MTcyMjI2MjI1NSwiaWF0IjoxNzIyMjU4NjU1fQ.Hq52WjpTcZGeF-fObQBaDtfwBhfV4iLAKaCP8iwJEz0`, // Buraya token'ınızı ekleyin
        },
      });
      setUser({ ...user, ...updatedUser });
      setEditMode(false);
    } catch (err) {
      console.error('Update error:', err.message);
      setError('Profil güncellenirken bir hata oluştu.');
    }
  };

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
        <Button variant="contained" color="primary" sx={{ marginTop: 2 }} onClick={handleEdit}>
          Düzenle
        </Button>
      </Paper>

      <Dialog open={editMode} onClose={handleClose}>
        <DialogTitle>Profil Düzenle</DialogTitle>
        <DialogContent>
          <TextField
            autoFocus
            margin="dense"
            label="Kullanıcı Adı"
            type="text"
            fullWidth
            value={updatedUser.username}
            onChange={(e) => setUpdatedUser({ ...updatedUser, username: e.target.value })}
          />
          <TextField
            margin="dense"
            label="Rol"
            type="text"
            fullWidth
            value={updatedUser.role}
            onChange={(e) => setUpdatedUser({ ...updatedUser, role: e.target.value })}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose} color="secondary">
            İptal
          </Button>
          <Button onClick={handleSave} color="primary">
            Kaydet
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default UserProfile;
