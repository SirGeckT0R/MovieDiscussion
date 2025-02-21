import { createContext } from 'react';
import { Role } from '../types/user';

export const AuthContext = createContext({
  role: Role.Guest,
  logout: () => {},
  authenticate: () => {},
});
