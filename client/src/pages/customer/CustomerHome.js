// src/App.js
import Navbar from './components/Navbar';
import React, { useEffect, useState } from "react";
import axios from 'axios';
import { Container, Grid, Card, CardContent, Typography, Button, List, ListItem, Divider } from '@mui/material';
import { styled } from '@mui/material/styles';

// Stil tanımları
const StyledCard = styled(Card)(({ theme }) => ({
    boxShadow: theme.shadows[5],
    borderRadius: theme.shape.borderRadius,
}));

const StyledButton = styled(Button)(({ theme }) => ({
    margin: theme.spacing(0.5),
}));



const CustomerHome = () => {
    const [categories, setCategories] = useState([]);

    useEffect(() => {
        axios.get('https://localhost:7096/api/category')
            .then(response => {
                setCategories(response.data.$values);
            })
            .catch(error => {
                console.error('Error fetching data:', error);
            });
    }, []);

    return (
        <div>
            <Navbar/>
            <Container sx={{ mt: 4 }}>
                <Grid container spacing={4}>
                    <Grid item md={6}>
                        <StyledCard>
                            <CardContent>
                                <Typography variant="h4" gutterBottom>Kategoriler</Typography>
                                <Divider sx={{ mb: 2 }} />
                                <List>
                                    {categories.map(category => (
                                        <ListItem key={category.categoryId} sx={{ borderBottom: '1px solid', borderColor: 'divider' }}>
                                            {category.categoryName}
                                        </ListItem>
                                    ))}
                                </List>
                            </CardContent>
                        </StyledCard>
                    </Grid>
                    <Grid item md={6}>
                        <StyledCard>
                            <CardContent>
                                <Typography variant="h4" gutterBottom>İletişim Bilgileri</Typography>
                                <Divider sx={{ mb: 2 }} />
                                <Typography>Telefon: +90 553 165 1382</Typography>
                                <Typography>E-posta: <a href="mailto:info@firmaadi.com">fulldevstudios@gmail.com</a></Typography>
                                <Typography>Adres: Tomtom, Kıvrıntılı Sk. No:4, 34433 Beyoğlu/İstanbul, Türkiye</Typography>
                            </CardContent>
                        </StyledCard>
                        <StyledCard sx={{ mt: 4 }}>
                            <CardContent>
                                <Typography variant="h4" gutterBottom>Müşteri Yorumları ve Referanslar</Typography>
                                <Divider sx={{ mb: 2 }} />
                                <Typography variant="body2">"Bu firmadan çok memnun kaldım. Ürünler harika!" - Ahmet Y.</Typography>
                                <Typography variant="body2">"Hızlı kargo ve kaliteli hizmet. Kesinlikle tavsiye ederim." - Ayşe K.</Typography>
                            </CardContent>
                        </StyledCard>
                        <StyledCard sx={{ mt: 4 }}>
                            <CardContent>
                                <Typography variant="h4" gutterBottom>Sosyal Medya</Typography>
                                <Divider sx={{ mb: 2 }} />
                                <div>
                                    <StyledButton href="http://facebook.com/firmaadi" target="_blank" variant="contained" color="primary">
                                        Facebook
                                    </StyledButton>
                                    <StyledButton href="http://twitter.com/firmaadi" target="_blank" variant="contained" color="info">
                                        Twitter
                                    </StyledButton>
                                    <StyledButton href="http://instagram.com/firmaadi" target="_blank" variant="contained" color="error">
                                        Instagram
                                    </StyledButton>
                                </div>
                            </CardContent>
                        </StyledCard>
                    </Grid>
                </Grid>
            </Container>

            <Container sx={{ bgcolor: 'black', color: 'white', py: 3 }}>
                <Typography variant="body2" align="center">
                    İletişim Bilgileri | Gizlilik Politikası | Kullanım Koşulları
                </Typography>
            </Container>
        </div>
    );
};

export default CustomerHome;
