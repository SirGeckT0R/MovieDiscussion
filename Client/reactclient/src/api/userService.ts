import {
  ForgotPasswordRequest,
  LoginRequest,
  ResetPasswordRequest,
} from '../types/user';
import { axiosInstance } from './global';

export const fetchLogin = async (body: LoginRequest) => {
  const { data } = await axiosInstance.postForm('/api/auth/login', body);

  return data;
};

export const fetchRegister = async (body: LoginRequest) => {
  const { data } = await axiosInstance.postForm('/api/auth/register', body);

  return data;
};

export async function fetchUser() {
  const { data } = await axiosInstance.get('/api/user');

  return data;
}

export async function sendConfirmationEmail() {
  const { data } = await axiosInstance.post('/api/auth/confirm');

  return data;
}

export async function forgotPassword(body: ForgotPasswordRequest) {
  const { data } = await axiosInstance.postForm('/api/auth/forgot', body);

  return data;
}

export async function changePassword() {
  const { data } = await axiosInstance.post('/api/auth/change');

  return data;
}

export async function resetPassword(body: ResetPasswordRequest) {
  const { data } = await axiosInstance.postForm('/api/auth/reset', body);

  return data;
}

export async function confirmEmail(email: string | null, token: string | null) {
  const { data } = await axiosInstance.get(
    `/api/auth/confirm?email=${email}&token=${token}`
  );

  return data;
}

export async function fetchLogout() {
  const { data } = await axiosInstance.post('/api/auth/logout');

  return data;
}

axiosInstance.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    if (
      error.response.status === 401 &&
      !error.response.request.responseURL.includes('refresh') &&
      !originalRequest._retry
    ) {
      originalRequest._retry = true;

      try {
        await axiosInstance.post('/api/auth/refresh', {});

        return axiosInstance(originalRequest);
      } catch (refreshError) {
        return Promise.reject(refreshError);
      }
    }
    return Promise.reject(error);
  }
);
