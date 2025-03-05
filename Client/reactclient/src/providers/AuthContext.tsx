import { createContext } from 'react';
import { Role } from '../types/user';

export const AuthContext = createContext({
  user: { id: null, role: Role.Guest, username: null, isEmailConfirmed: false },
  logout: () => {},
  authenticate: () => {},
});
