import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Navbar from './Navbar';
import {
  Box,
  Grid,
  Card,
  CardContent,
  Typography,
  List,
  ListItem,
  ListItemText,
  IconButton,
  Divider
} from '@mui/material';
import { Phone, LocationOn } from '@mui/icons-material';

const SupplierPage = () => {
  const [suppliers, setSuppliers] = useState([]);

  useEffect(() => {
    const fetchSuppliers = async () => {
      try {
        const response = await axios.get('https://localhost:7096/api/SupplierDetail');
        setSuppliers(response.data.$values);
      } catch (error) {
        console.error('Error fetching suppliers:', error);
      }
    };

    fetchSuppliers();
  }, []);

  const groupedSuppliers = suppliers.reduce((acc, supplier) => {
    const { supplierID, supplierName, contactName, address, city, postalCode, country, phone } = supplier;
    if (!acc[supplierID]) {
      acc[supplierID] = {
        supplierID,
        supplierName,
        contactName,
        address,
        city,
        postalCode,
        country,
        phone,
        products: [],
      };
    }
    acc[supplierID].products.push({
      productName: supplier.productName,
      categoryName: supplier.categoryName,
    });
    return acc;
  }, {});

  const supplierList = Object.values(groupedSuppliers);

  return (
    <>
      <Navbar />
      <Box sx={{ flexGrow: 1, padding: 2 }}>
        <Grid container spacing={2}>
          {supplierList.map((supplier) => (
            <Grid item xs={12} sm={6} md={4} key={supplier.supplierID}>
              <Card sx={{ boxShadow: 3, borderRadius: 2 }}>
                <CardContent>
                  <Typography variant="h5" component="div" sx={{ fontWeight: 'bold' }}>
                    {supplier.supplierName}
                  </Typography>
                  <Typography color="text.secondary">
                    <IconButton size="small"><Phone fontSize="inherit" /></IconButton>
                    {supplier.contactName}
                  </Typography>
                  <Typography color="text.secondary">
                    <IconButton size="small"><LocationOn fontSize="inherit" /></IconButton>
                    {supplier.address}, {supplier.city}, {supplier.postalCode}, {supplier.country}
                  </Typography>
                  <Typography color="text.secondary">
                    <IconButton size="small"><Phone fontSize="inherit" /></IconButton>
                    Phone: {supplier.phone}
                  </Typography>
                  <Divider sx={{ margin: '16px 0' }} />
                  <Typography variant="h6" sx={{ marginTop: 2, fontWeight: 'bold' }}>
                    Ürünler
                  </Typography>
                  <List>
                    {supplier.products.map((product, index) => (
                      <ListItem key={index}>
                        <ListItemText
                          primary={product.productName}
                          secondary={product.categoryName}
                        />
                      </ListItem>
                    ))}
                  </List>
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>
      </Box>
    </>
  );
};

export default SupplierPage;
