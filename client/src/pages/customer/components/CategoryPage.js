import React, { useEffect, useState } from "react";
import axios from "axios";
import { Container, Card, CardContent, Typography, Grid } from "@mui/material";
import Navbar from "./Navbar";

const CategoryPage = () => {
  const [categories, setCategories] = useState([]);

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const response = await axios.get(
          "https://localhost:7096/api/AdminCategory"
        );
        setCategories(response.data.$values);
      } catch (error) {
        console.error("Hata:", error);
      }
    };

    fetchCategories();
  }, []);

  const groupedCategories = categories.reduce((acc, category) => {
    const { categoryId, categoryName, description, productName, unit, price } =
      category;
    if (!acc[categoryId]) {
      acc[categoryId] = {
        categoryName,
        description,
        products: [],
      };
    }
    acc[categoryId].products.push({ productName, unit, price });
    return acc;
  }, {});

  return (
    <div>
      <Navbar />
      <Container sx={{ mt: 4 }}>
        <Grid container spacing={3}>
          {Object.entries(groupedCategories).map(
            ([_, { categoryName, description, products }]) => (
              <Grid item xs={12} sm={6} md={4} key={categoryName}>
                <Card
                  sx={{
                    boxShadow: 3,
                    borderRadius: 2,
                    transition: "0.3s",
                    "&:hover": { boxShadow: 6 },
                  }}
                >
                  <CardContent>
                    <Typography
                      variant="h5"
                      component="div"
                      sx={{ fontWeight: "bold", color: "#1976d2" }}
                    >
                      {categoryName}
                    </Typography>
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      sx={{ mb: 2 }}
                    >
                      {description}
                    </Typography>
                    <Typography
                      variant="h6"
                      component="div"
                      sx={{ mt: 2, fontWeight: "bold" }}
                    >
                      Ürünler:
                    </Typography>
                    <ul style={{ paddingLeft: 20 }}>
                      {products.map((product, index) => (
                        <li key={index} style={{ padding: "8px 0" }}>
                          <Typography variant="body2" sx={{ fontWeight: "bold" }}>
                            {product.productName}
                          </Typography>
                          <Typography variant="body2">
                            {product.unit} - <strong>${product.price}</strong>
                          </Typography>
                        </li>
                      ))}
                    </ul>
                  </CardContent>
                </Card>
              </Grid>
            )
          )}
        </Grid>
      </Container>
      </div>
  );
};

export default CategoryPage;
