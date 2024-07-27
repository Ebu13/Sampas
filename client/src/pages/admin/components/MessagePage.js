import React, { useEffect, useState } from "react";
import axios from "axios";
import {
  Typography,
  Grid,
  Card,
  CardContent,
  Avatar,
  Divider,
  Box,
} from "@mui/material";
import Navbar from "./Navbar";

const API_MESSAGE_URL = "https://localhost:7096/api/messages";

const getImage = (employeeId) => {
  try {
    return require(`../../../photos/employee/${employeeId}.jpg`);
  } catch (error) {
    console.error("Error loading image:", error);
    return null; // Return null if the image is not found
  }
};

const MessagePage = () => {
  const [messages, setMessages] = useState([]);

  useEffect(() => {
    // Mesajları al
    axios
      .get(API_MESSAGE_URL)
      .then((response) => {
        setMessages(response.data.$values);
      })
      .catch((error) => {
        console.error("Error fetching messages:", error);
      });
  }, []);

  return (
    <div>
      <Navbar />
      <Typography variant="h4" gutterBottom sx={{ margin: 2 }}>
        Messages
      </Typography>
      <Grid container spacing={3} sx={{ padding: 2 }}>
        {messages.map((message) => (
          <Grid item xs={12} sm={6} md={4} key={message.messageId}>
            <Card>
              <Box display="flex" alignItems="center" padding={2}>
                <Avatar
                  src={getImage(message.senderId)}
                  alt={message.senderId}
                  sx={{ marginRight: 2 }}
                >
                  {!getImage(message.senderId) && message.senderId[0]}
                </Avatar>
                <Box flexGrow={1}>
                  <Typography variant="body1">
                    <strong>From:</strong> {message.senderFirstName_LastName}
                  </Typography>
                  <Typography variant="body1" sx={{ textAlign: 'right' }}>
                    <strong>To:</strong> {message.receiverFirstName_LastName}
                  </Typography>
                </Box>
                <Avatar
                  src={getImage(message.receiverId)}
                  alt={message.receiverId}
                  sx={{ marginLeft: 2 }}
                >
                  {!getImage(message.receiverId) && message.receiverId[0]}
                </Avatar>
              </Box>
              <Divider />
              <CardContent>
                <Typography variant="body2" color="textSecondary">
                  Sent: {new Date(message.sentDate).toLocaleString()}
                </Typography>
                <Typography variant="body1" sx={{ marginTop: 1 }}>
                  {message.messageText}
                </Typography>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
    </div>
  );
};

export default MessagePage;
