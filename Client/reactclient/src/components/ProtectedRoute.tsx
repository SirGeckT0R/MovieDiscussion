import { Navigate } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import { ReactNode } from 'react';
import { Role } from '../types/user';

export function ProtectedRoute({
  children,
  allowedRoles,
}: {
  children: ReactNode;
  allowedRoles: Role[];
}) {
  const { user } = useAuth();

  if (!allowedRoles.includes(user.role)) {
    return <Navigate to='/login' />;
  }

  return children;
}
