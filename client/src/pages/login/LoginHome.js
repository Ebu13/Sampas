import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { TextField, Button, Grid, Paper, Typography } from '@mui/material';

const LoginHome = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        setError('');

        try {
            const response = await axios.post('https://localhost:7096/api/users/login', {
                username,
                password,
            });

            const user = response.data;

            // Kullanıcının rolüne göre yönlendirme
            if (user.role === 'Admin') {
                navigate('/admin');
            } else if (user.role === 'Employee') {
                navigate('/employee');
            } else if (user.role === 'Customer') {
                navigate('/customer');
            }
        } catch (err) {
            setError(err.response ? err.response.data : 'Giriş başarısız.');
        }
    };

    return (
        <Grid container justifyContent="center" alignItems="center" style={{ height: '100vh' }}>
            <Paper elevation={3} style={{ padding: '20px', width: '300px' }}>
                <Typography variant="h4" align="center">Giriş Yap</Typography>
                <form onSubmit={handleLogin}>
                    <TextField
                        label="Kullanıcı Adı"
                        variant="outlined"
                        fullWidth
                        margin="normal"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                    />
                    <TextField
                        label="Şifre"
                        type="password"
                        variant="outlined"
                        fullWidth
                        margin="normal"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                    <Button type="submit" variant="contained" color="primary" fullWidth>
                        Giriş Yap
                    </Button>
                    {error && <Typography color="error" align="center" style={{ marginTop: '10px' }}>{error}</Typography>}
                </form>
            </Paper>
        </Grid>
    );
};

export default LoginHome;
