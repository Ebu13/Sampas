// src/pages/EmployeePage.js
import React, { useEffect, useState } from "react";
import axios from "axios";
import Navbar from "./Navbar";
import {
  Container,
  Grid,
  Card,
  CardContent,
  CardMedia,
  Typography,
  Box,
} from "@mui/material";

const EmployeePage = () => {
  const [employees, setEmployees] = useState([]);

  useEffect(() => {
    axios
      .get("https://localhost:7096/api/employee")
      .then((response) => {
        setEmployees(response.data.$values);
      })
      .catch((error) => {
        console.error("Error fetching data:", error);
      });
  }, []);

  const getImage = (employeeId) => {
    try {
      return require(`../../../photos/employee/${employeeId}.jpg`);
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
            Employee List
          </Typography>
        </Box>
        <Grid container spacing={4}>
          {employees.map((employee) => (
            <Grid item key={employee.employeeId} xs={12} sm={6} md={4}>
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
                  image={getImage(employee.employeeId)} // Get the dynamic image path
                  alt={`${employee.firstName} ${employee.lastName}`}
                />
                <CardContent>
                  <Typography variant="h5" component="div">
                    {employee.firstName} {employee.lastName}
                  </Typography>
                  <Typography variant="body2" color="textSecondary">
                    Birth Date: {employee.birthDate}
                  </Typography>
                  <Typography variant="body2" color="textSecondary">
                    Notes: {employee.notes}
                  </Typography>
                  <Typography variant="body2" color="textSecondary">
                    Password: {employee.password}
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

export default EmployeePage;
