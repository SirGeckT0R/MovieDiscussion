import { Navigate } from 'react-router-dom';
import { ReactNode } from 'react';
import { useAuth } from '../../hooks/useAuth';
import { Role } from '../../types/user';

type Props = {
  children: ReactNode;
  allowedRoles: Role[];
};

export function ProtectedRoute({ children, allowedRoles }: Props) {
  const { user } = useAuth();

  if (!allowedRoles.includes(user.role)) {
    return <Navigate to='/login' />;
  }

  return children;
}
