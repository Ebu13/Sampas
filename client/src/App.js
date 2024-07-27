import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";

import AdminHome from "./pages/admin/AdminHome";
import CategoryPage from "./pages/admin/components/CategoryPage";
import CustomerPage from "./pages/admin/components/CustomerPage";
import EmployeePage from "./pages/admin/components/EmployeePage";
import MessagePage from "./pages/admin/components/MessagePage";
import OrderDetailPage from "./pages/admin/components/OrderDetailPage";
import ProductPage from "./pages/admin/components/ProductPage";
import ShipperPage from "./pages/admin/components/ShipperPage";
import SupplierPage from "./pages/admin/components/SupplierPage";

import ClientHome from "./pages/client/ClientHome";


import CustomerHome from "./pages/customer/CustomerHome";
import CustomerChatPage from "./pages/customer/components/ChatPage";
import CustomerCategoryPage from "./pages/customer/components/CategoryPage";
import CustomerOrderDetailPage from "./pages/customer/components/OrderDetailPage";
import CustomerProductPage from "./pages/customer/components/ProductPage";
import CustomerShipperPage from "./pages/customer/components/ShipperPage";
import CustomerSupplierPage from "./pages/customer/components/SupplierPage";
import CustomerProfilePage from "./pages/customer/components/ProfilePage";

import EmployeeHome from "./pages/employee/EmployeeHome";
import EmployeeCategoryPage from "./pages/employee/components/CategoryPage";
import EmployeeEmployeePage from "./pages/employee/components/EmployeePage";
import EmployeeOrderDetailPage from "./pages/employee/components/OrderDetailPage";
import EmployeeProductPage from "./pages/employee/components/ProductPage";
import EmployeeShipperPage from "./pages/employee/components/ShipperPage";
import EmployeeSupplierPage from "./pages/employee/components/SupplierPage";
import EmployeeProfilePage from "./pages/employee/components/ProfilePage";

import LoginHome from "./pages/login/LoginHome";

import RegisterHome from "./pages/register/RegisterHome";



function App() {
  return (
    <Router>
      <div className="App">
        <Routes>
          <Route path="/" element={<ClientHome />} />

          <Route path="/admin" element={<AdminHome />} />
          <Route path="/admin/categories" element={<CategoryPage />} />
          <Route path="/admin/customers" element={<CustomerPage />} />
          <Route path="/admin/employees" element={<EmployeePage />} />
          <Route path="/admin/messages" element={<MessagePage />} />
          <Route path="/admin/orderdetails" element={<OrderDetailPage />} />
          <Route path="/admin/products" element={<ProductPage />} />
          <Route path="/admin/shippers" element={<ShipperPage />} />
          <Route path="/admin/suppliers" element={<SupplierPage />} />

          <Route path="/customer" element={<CustomerHome />} />
          <Route path="/customer/chat" element={<CustomerChatPage />} />
          <Route path="/customer/categories" element={<CustomerCategoryPage />} />
          <Route path="/customer/orderdetails" element={<CustomerOrderDetailPage />} />
          <Route path="/customer/products" element={<CustomerProductPage />} />
          <Route path="/customer/shippers" element={<CustomerShipperPage />} />
          <Route path="/customer/suppliers" element={<CustomerSupplierPage />} />
          <Route path="/customer/profile" element={<CustomerProfilePage />} />


          <Route path="/employee" element={<EmployeeHome />} />
          <Route path="/employee/categories" element={<EmployeeCategoryPage />} />
          <Route path="/employee/employees" element={<EmployeeEmployeePage />} />
          <Route path="/employee/orderdetails" element={<EmployeeOrderDetailPage />} />
          <Route path="/employee/products" element={<EmployeeProductPage />} />
          <Route path="/employee/shippers" element={<EmployeeShipperPage />} />
          <Route path="/employee/suppliers" element={<EmployeeSupplierPage />} />
          <Route path="/employee/profile" element={<EmployeeProfilePage />} />
          
          
          <Route path="/login" element={<LoginHome />} />
          
          <Route path="/register" element={<RegisterHome />} />


        </Routes>
      </div>
    </Router>
  );
}

export default App;
