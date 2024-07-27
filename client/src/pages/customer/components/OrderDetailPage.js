import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Navbar from './Navbar';
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TablePagination,
  Paper,
  Typography,
  CircularProgress,
  Box,
  Snackbar,
  Alert,
} from '@mui/material';
import { createTheme, ThemeProvider } from '@mui/material/styles';

const theme = createTheme({
  palette: {
    primary: {
      main: '#1976d2', // Mavi Renk
    },
    secondary: {
      main: '#424242', // Gri Renk
    },
  },
});

const OrderDetailPage = () => {
  const [orderDetails, setOrderDetails] = useState([]);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(20);
  const [totalCount, setTotalCount] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchOrderDetails = async () => {
      try {
        const response = await axios.get('https://localhost:7096/api/ComprehensiveOrderDetail');
        setOrderDetails(response.data.$values);
        setTotalCount(response.data.$values.length);
      } catch (error) {
        setError('Error fetching order details');
      } finally {
        setLoading(false);
      }
    };

    fetchOrderDetails();
  }, []);

  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  return (
    <ThemeProvider theme={theme}>
      <Navbar />
      <Paper sx={{ padding: 3, margin: 3, backgroundColor: '#f5f5f5', boxShadow: 3 }}>
        <Typography variant="h4" component="h1" gutterBottom align="center" sx={{ color: '#ffffff', backgroundColor: '#1976d2', padding: '16px', borderRadius: '8px' }}>
          Order Details
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
          <TableContainer component={Paper} elevation={3} sx={{ borderRadius: '8px' }}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell sx={{ backgroundColor: '#1976d2', color: '#ffffff' }}>Order Date</TableCell>
                  <TableCell sx={{ backgroundColor: '#1976d2', color: '#ffffff' }}>Customer Name</TableCell>
                  <TableCell sx={{ backgroundColor: '#1976d2', color: '#ffffff' }}>Employee Name</TableCell>
                  <TableCell sx={{ backgroundColor: '#1976d2', color: '#ffffff' }}>Quantity</TableCell>
                  <TableCell sx={{ backgroundColor: '#1976d2', color: '#ffffff' }}>Product Name</TableCell>
                  <TableCell sx={{ backgroundColor: '#1976d2', color: '#ffffff' }}>Price</TableCell>
                  <TableCell sx={{ backgroundColor: '#1976d2', color: '#ffffff' }}>Total Price</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {orderDetails
                  .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                  .map((order) => (
                    <TableRow key={order.$id}>
                      <TableCell>{new Date(order.orderDate).toLocaleDateString()}</TableCell>
                      <TableCell>{order.customerName}</TableCell>
                      <TableCell>{order.employeeFirstName} {order.employeeLastName}</TableCell>
                      <TableCell>{order.quantity}</TableCell>
                      <TableCell>{order.productName}</TableCell>
                      <TableCell>{order.price}</TableCell>
                      <TableCell>$ {order.totalPrice}</TableCell>
                    </TableRow>
                  ))}
              </TableBody>
            </Table>
          </TableContainer>
        )}
        <TablePagination
          rowsPerPageOptions={[20]}
          component="div"
          count={totalCount}
          rowsPerPage={rowsPerPage}
          page={page}
          onPageChange={handleChangePage}
          onRowsPerPageChange={handleChangeRowsPerPage}
        />
      </Paper>
    </ThemeProvider>
  );
};

export default OrderDetailPage;
