import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { MenuItem, Select, FormControl, InputLabel, Typography, TextField, Button } from '@mui/material';
import Navbar from './Navbar';

const API_EMPLOYEE_URL = 'https://localhost:7096/api/employee';
const API_MESSAGE_URL = 'https://localhost:7096/api/message';

const ChatPage = () => {
  const [employees, setEmployees] = useState([]);
  const [messages, setMessages] = useState([]);
  const [selectedSenderEmployee, setSelectedSenderEmployee] = useState('');
  const [selectedReceiverEmployee, setSelectedReceiverEmployee] = useState('');
  const [newMessage, setNewMessage] = useState({
    senderId: '',
    receiverId: '',
    messageText: ''
  });

  useEffect(() => {
    // Çalışanları al
    axios.get(API_EMPLOYEE_URL)
      .then(response => {
        setEmployees(response.data.$values);
      })
      .catch(error => {
        console.error('Error fetching employees:', error);
      });
  }, []);

  useEffect(() => {
    // Mesajları al
    axios.get(API_MESSAGE_URL)
      .then(response => {
        setMessages(response.data.$values);
      })
      .catch(error => {
        console.error('Error fetching messages:', error);
      });
  }, []);

  const handleSenderChange = (event) => {
    setSelectedSenderEmployee(event.target.value);
    setNewMessage({ ...newMessage, senderId: event.target.value });
  };

  const handleReceiverChange = (event) => {
    setSelectedReceiverEmployee(event.target.value);
    setNewMessage({ ...newMessage, receiverId: event.target.value });
  };

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setNewMessage({ ...newMessage, [name]: value });
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    axios.post(API_MESSAGE_URL, newMessage)
      .then(() => {
        // Yeni mesaj gönderildiğinde formu sıfırla ve mesajları güncelle
        setNewMessage({ senderId: '', receiverId: '', messageText: '' });
        setSelectedSenderEmployee('');
        setSelectedReceiverEmployee('');
        return axios.get(API_MESSAGE_URL);
      })
      .then(response => {
        setMessages(response.data.$values);
      })
      .catch(error => {
        console.error('Error creating message:', error);
      });
  };

  const selectedReceiverDetails = employees.find(
    (employee) => employee.employeeId.toString() === selectedReceiverEmployee
  );

  const selectedSenderDetails = employees.find(
    (employee) => employee.employeeId.toString() === selectedSenderEmployee
  );

  return (
    <div>
      <Navbar />
      <Typography variant="h4" gutterBottom>
        Messages
      </Typography>
      <ul>
        {messages.map((message) => (
          <li key={message.messageId}>
            <div>
              <strong>From:</strong> {message.senderId}
            </div>
            <div>
              <strong>To:</strong> {message.receiverId}
            </div>
            <div>
              <strong>Sent:</strong> {new Date(message.sentDate).toLocaleString()}
            </div>
            <div>
              <strong>Message:</strong> {message.messageText}
            </div>
          </li>
        ))}
      </ul>

      <Typography variant="h4" gutterBottom>
        Add a New Message
      </Typography>

      <form onSubmit={handleSubmit}>
        <FormControl fullWidth variant="outlined" margin="normal">
          <InputLabel id="sender-select-label">Select Sender</InputLabel>
          <Select
            labelId="sender-select-label"
            value={selectedSenderEmployee}
            onChange={handleSenderChange}
            label="Select Sender"
          >
            <MenuItem value="">
              <em>None</em>
            </MenuItem>
            {employees.map((employee) => (
              <MenuItem key={employee.employeeId} value={employee.employeeId.toString()}>
                {employee.firstName} {employee.lastName}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        <FormControl fullWidth variant="outlined" margin="normal">
          <InputLabel id="receiver-select-label">Select Receiver</InputLabel>
          <Select
            labelId="receiver-select-label"
            value={selectedReceiverEmployee}
            onChange={handleReceiverChange}
            label="Select Receiver"
          >
            <MenuItem value="">
              <em>None</em>
            </MenuItem>
            {employees.map((employee) => (
              <MenuItem key={employee.employeeId} value={employee.employeeId.toString()}>
                {employee.firstName} {employee.lastName}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        <TextField
          name="messageText"
          label="Message Text"
          fullWidth
          multiline
          rows={4}
          variant="outlined"
          margin="normal"
          value={newMessage.messageText}
          onChange={handleInputChange}
        />

        <Button type="submit" variant="contained" color="primary">
          Send Message
        </Button>
      </form>

      {selectedSenderDetails && (
        <div>
          <Typography variant="h6">Selected Sender:</Typography>
          <p>
            {selectedSenderDetails.firstName} {selectedSenderDetails.lastName} ({selectedSenderDetails.employeeId})
          </p>
        </div>
      )}

      {selectedReceiverDetails && (
        <div>
          <Typography variant="h6">Selected Receiver:</Typography>
          <p>
            {selectedReceiverDetails.firstName} {selectedReceiverDetails.lastName} ({selectedReceiverDetails.employeeId})
          </p>
        </div>
      )}
    </div>
  );
};

export default ChatPage;
