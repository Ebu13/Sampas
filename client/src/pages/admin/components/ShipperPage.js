import React, { useEffect, useState } from "react";
import axios from 'axios';
import Navbar from './Navbar';
import { Grid, Card, CardContent, Typography, Container, CircularProgress, Box, Snackbar, Alert } from '@mui/material';
import { createTheme, ThemeProvider } from '@mui/material/styles';

const theme = createTheme({
  palette: {
    primary: {
      main: '#1976d2',
    },
    secondary: {
      main: '#424242',
    },
  },
});

const ShipperPage = () => {
  const [shippers, setShippers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    axios.get('https://localhost:7096/api/shipper')
      .then(response => {
        setShippers(response.data.$values);
        setLoading(false);
      })
      .catch(error => {
        setError('Error fetching data');
        setLoading(false);
      });
  }, []);

  return (
    <ThemeProvider theme={theme}>
      <Navbar />
      <Container>
        <Typography variant="h4" component="h1" gutterBottom align="center" sx={{ margin: '16px 0' }}>
          Shipper List
        </Typography>
        {loading ? (
          <Box display="flex" justifyContent="center" alignItems="center" minHeight="400px">
            <CircularProgress />
          </Box>
        ) : error ? (
          <Snackbar open={true} autoHideDuration={6000}>
            <Alert severity="error" sx={{ width: '100%' }}>
              {error}
            </Alert>
          </Snackbar>
        ) : (
          <Grid container spacing={3}>
            {shippers.map((shipper) => (
              <Grid item xs={12} sm={6} md={4} key={shipper.shipperId}>
                <Card>
                  <CardContent>
                    <Typography variant="h6" component="h2">
                      {shipper.shipperName}
                    </Typography>
                    <Typography variant="body2" color="textSecondary">
                      {shipper.phone}
                    </Typography>
                  </CardContent>
                </Card>
              </Grid>
            ))}
          </Grid>
        )}
      </Container>
    </ThemeProvider>
  );
};

export default ShipperPage;
