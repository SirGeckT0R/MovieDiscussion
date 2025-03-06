export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
}

export interface ForgotPasswordRequest {
  email: string;
}

export interface ResetPasswordRequest {
  email: string | null;
  token: string | null;
  newPassword: string;
}

export interface User {
  id?: string;
  username?: string;
  role?: Role;
  isEmailConfirmed?: boolean;
}

export enum Role {
  Guest = 0,
  User = 1,
  Admin = 2,
}
