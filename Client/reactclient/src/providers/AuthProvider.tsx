import { ReactNode, useEffect, useState } from 'react';
import { Role } from '../types/user';
import { fetchLogout, fetchRole } from '../api/userService';
import { AuthContext } from './AuthContext';

export function AuthProvider({ children }: { children: ReactNode }) {
  const [role, setRole] = useState<Role | null>(null);
  const logout = () => {
    fetchLogout();
    setRole(Role.Guest);
  };

  const authenticate = async () => {
    await updateRole();
  };

  const updateRole = () => {
    return fetchRole()
      .then((role) => {
        setRole(role);
      })
      .catch(() => {
        logout();
      });
  };

  useEffect(() => {
    updateRole();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  if (role === null) {
    return null;
  }

  const value = {
    role,
    logout,
    authenticate,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
