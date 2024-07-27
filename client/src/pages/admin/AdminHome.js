import React from 'react';
import { Typography } from '@mui/material';
import Navbar from './components/Navbar';

function AdminHome() {
  return (
    <>
      <Navbar />
      <div
        style={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          justifyContent: 'center',
          height: '100vh',
          backgroundColor: '#f5f5f5',
          padding: '20px', // Padding ekledim
        }}
      >
        <div style={{ textAlign: 'center' }}>
          <Typography variant="h4" gutterBottom>
            Yönetim Paneline Hoş Geldiniz!
          </Typography>
          <Typography variant="body1" style={{ marginBottom: '20px' }}>
            Bu panel, sistemdeki farklı varlıkları yönetmek için kullanılır. 
            Navbar üzerinden yönetmek istediğiniz varlıkları seçebilirsiniz.
          </Typography>
        </div>
      </div>
    </>
  );
}

export default AdminHome;
