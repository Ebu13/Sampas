import React, { useEffect, useState } from "react";
import axios from "axios";
import {
  Container,
  Grid,
  Card,
  CardContent,
  CardMedia,
  Typography,
  Box
} from "@mui/material";
import Navbar from "./Navbar";

const CustomerPage = () => {
  const [customers, setCustomers] = useState([]);

  useEffect(() => {
    axios
      .get("https://localhost:7096/api/customer")
      .then((response) => {
        setCustomers(response.data.$values);
      })
      .catch((error) => {
        console.error("Error fetching data:", error);
      });
  }, []);

  const getImage = (customerId) => {
    try {
      return require(`../../../photos/customer/${customerId}.jpg`);
    } catch (error) {
      console.error("Error loading image:", error);
      return null; // Return null if the image is not found
    }
  };

  return (
    <div>
      <Navbar />
      <Container>
      <Box my={4}>
          <Typography variant="h4" gutterBottom>
            Customer List
          </Typography>
        </Box>
        <Grid container spacing={4}>
          {customers.map((customer) => (
            <Grid item xs={12} sm={6} md={3} key={customer.customerId}>
              <Card>
              <CardMedia
                  component="img"
                  sx={{
                    height: 360,
                    width: "100%",
                    objectFit: "cover",
                    borderRadius: "50%", // Yuvarlak köşeler için
                    display: "flex",
                    justifyContent: "center", // İçeriği yatayda ortala
                    alignItems: "center", // İçeriği dikeyde ortala
                  }}
                  image={getImage(customer.customerId)} // Get the dynamic image path
                  alt={`${customer.customerName}`}
                />

                <CardContent>
                  <Typography variant="h6" component="h2">
                    {customer.customerName}
                  </Typography>
                  <Typography color="textSecondary">
                    {customer.contactName}
                  </Typography>
                  <Typography variant="body2" component="p">
                    <strong>ID:</strong> {customer.customerId}
                  </Typography>
                  <Typography variant="body2" component="p">
                    <strong>Address:</strong> {customer.address}
                  </Typography>
                  <Typography variant="body2" component="p">
                    <strong>City:</strong> {customer.city}
                  </Typography>
                  <Typography variant="body2" component="p">
                    <strong>Postal Code:</strong> {customer.postalCode}
                  </Typography>
                  <Typography variant="body2" component="p">
                    <strong>Country:</strong> {customer.country}
                  </Typography>
                  <Typography variant="body2" component="p">
                    <strong>Password:</strong> {customer.password}
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>
      </Container>
    </div>
  );
};

export default CustomerPage;
