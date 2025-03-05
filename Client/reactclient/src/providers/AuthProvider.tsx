import { ReactNode, useEffect, useState } from 'react';
import { Role, User } from '../types/user';
import { fetchLogout, fetchUser } from '../api/userService';
import { AuthContext } from './AuthContext';

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const logout = () => {
    fetchLogout();
    setUser({ role: Role.Guest });
  };

  const authenticate = async () => {
    await updateUser();
  };

  const updateUser = () => {
    return fetchUser()
      .then((user) => {
        setUser(user);
      })
      .catch(() => {
        logout();
      });
  };

  useEffect(() => {
    updateUser();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  if (user === null) {
    return null;
  }

  const value = {
    user,
    logout,
    authenticate,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
