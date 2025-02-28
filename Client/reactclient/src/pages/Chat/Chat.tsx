import { useEffect, useRef, useState } from 'react';
import * as signalR from '@microsoft/signalr';
import {
  Box,
  Button,
  List,
  ListItem,
  ListItemText,
  Paper,
  TextField,
} from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import { getMessagesQuery } from '../../queries/messagesQueries';
import { useParams } from 'react-router-dom';
import { Message } from '../../types/discussion';

export function Chat() {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null
  );
  const { id } = useParams();
  const { data: history, isSuccess } = useQuery(getMessagesQuery(id));

  const messagesEndRef = useRef(null);
  const [messages, setMessages] = useState<Array<Message>>([]);

  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${import.meta.env.VITE_IMAGES_HOST}/discussion-hub`)
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection != null) {
      connection.start().then(() => {
        connection
          .invoke('JoinChat', id)
          .then((res) => console.log('here'))
          .catch((response) => console.error(response));
      });

      connection.on(
        'ReceiveMessage',
        function (username: string, text: string, sentAt: string) {
          setMessages((prevMovies) => [
            ...prevMovies,
            { text, username, sentAt },
          ]);
        }
      );
    }
  }, [connection]);

  useEffect(() => setMessages(history), [isSuccess]);

  const [input, setInput] = useState('');

  const handleSend = () => {
    if (input.trim()) {
      connection?.invoke('SendMessage', input.trim());
      setInput('');
    }
  };

  useEffect(() => {
    if (messagesEndRef.current) {
      messagesEndRef.current.scrollIntoView({ behavior: 'smooth' });
    }
  }, [messages]);

  return (
    <Box
      sx={{ display: 'flex', flexDirection: 'column', height: '100vh', p: 2 }}>
      <Paper sx={{ flexGrow: 1, overflow: 'auto', mb: 2 }}>
        <List>
          {messages?.map((message, index) => (
            <ListItem key={index}>
              <ListItemText
                primary={message.text}
                secondary={message.username}
                sx={{
                  textAlign: message.username,
                  //    === 'user' ? 'right' : 'left',
                }}
              />
            </ListItem>
          ))}
        </List>
        <div ref={messagesEndRef} />
      </Paper>
      <Box sx={{ display: 'flex' }}>
        <TextField
          fullWidth
          variant='outlined'
          value={input}
          onChange={(e) => setInput(e.target.value)}
          onKeyPress={(e) => e.key === 'Enter' && handleSend()}
        />
        <Button variant='contained' onClick={handleSend} sx={{ ml: 2 }}>
          Send
        </Button>
      </Box>
    </Box>
  );
}
