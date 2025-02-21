export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
}

export enum Role {
  Guest = 0,
  User = 1,
  Admin = 2,
}
