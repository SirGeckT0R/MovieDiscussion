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
import { useAuth } from '../../hooks/useAuth';

export function Chat() {
  const { id } = useParams();
  const { user } = useAuth();

  const messagesQuery = getMessagesQuery(id);
  const { data: history, isSuccess } = useQuery(messagesQuery);
  const [messages, setMessages] = useState<Message[]>([]);

  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null
  );

  const messagesEndRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${import.meta.env.VITE_IMAGES_HOST}/discussion-hub`)
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  // eslint-disable-next-line react-hooks/exhaustive-deps
  useEffect(() => history && setMessages(history), [isSuccess]);

  useEffect(() => {
    if (connection != null) {
      connection.start().then(() => {
        connection
          .invoke('JoinChat', id)
          .catch((response) => console.error(response));
      });

      connection.on(
        'ReceiveMessage',
        function (
          userId: string,
          username: string,
          text: string,
          sentAt: string
        ) {
          setMessages((prevMovies) => [
            ...prevMovies,
            { userId, text, username, sentAt },
          ]);
        }
      );

      return () => {
        connection.off('ReceiveMessage');
        connection.stop();
      };
    }
  }, [connection, id]);

  useEffect(() => {
    if (messagesEndRef.current) {
      messagesEndRef.current.scrollIntoView({ behavior: 'smooth' });
    }
  }, [messages]);

  const [input, setInput] = useState('');

  const handleSend = () => {
    if (input.trim()) {
      connection?.invoke('SendMessage', input.trim());
      setInput('');
    }
  };

  return (
    <Box
      sx={{ display: 'flex', flexDirection: 'column', height: '60vh', p: 2 }}>
      <Paper sx={{ flexGrow: 1, overflow: 'auto', mb: 2 }}>
        <List>
          {messages?.map((message, index) => {
            let textAlignment = 'left';

            if (message.username === 'Admin') {
              textAlignment = 'center';
            }

            if (message.userId === user.id) {
              textAlignment = 'right';
            }

            return (
              <ListItem key={index}>
                <ListItemText
                  primary={message.text}
                  secondary={message.username}
                  sx={{
                    textAlign: textAlignment,
                  }}
                />
              </ListItem>
            );
          })}
        </List>
        <div ref={messagesEndRef} />
      </Paper>
      <Box sx={{ display: 'flex' }}>
        <TextField
          fullWidth
          variant='outlined'
          value={input}
          onChange={(e) => setInput(e.target.value)}
          onKeyDown={(e) => e.key === 'Enter' && handleSend()}
        />
        <Button variant='contained' onClick={handleSend} sx={{ ml: 2 }}>
          Send
        </Button>
      </Box>
    </Box>
  );
}
