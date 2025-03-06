import {
  ForgotPasswordRequest,
  LoginRequest,
  ResetPasswordRequest,
  User,
} from '../types/user';
import { axiosInstance } from './global';

export const fetchLogin = async (body: LoginRequest) => {
  const response = await axiosInstance
    .postForm('/api/auth/login', body)
    .then((response) => response?.data);

  return response;
};

export const fetchRegister = async (body: LoginRequest) => {
  const response = await axiosInstance
    .postForm('/api/auth/register', body)
    .then((response) => response?.data);

  return response;
};

export async function fetchUser() {
  const response: User = await axiosInstance
    .get('/api/user')
    .then((response) => response?.data);

  return response;
}

export async function sendConfirmationEmail() {
  const response = await axiosInstance
    .post('/api/auth/confirm')
    .then((response) => response?.data);

  return response;
}

export async function forgotPassword(body: ForgotPasswordRequest) {
  const response = await axiosInstance
    .postForm('/api/auth/forgot', body)
    .then((response) => response?.data);

  return response;
}

export async function changePassword() {
  const response = await axiosInstance
    .post('/api/auth/change')
    .then((response) => response?.data);

  return response;
}

export async function resetPassword(body: ResetPasswordRequest) {
  const response = await axiosInstance
    .postForm('/api/auth/reset', body)
    .then((response) => response?.data);

  return response;
}

export async function confirmEmail(email: string | null, token: string | null) {
  console.log(email, token);
  const response = await axiosInstance
    .get(`/api/auth/confirm?email=${email}&token=${token}`)
    .then((response) => response?.data);

  return response;
}

export async function fetchLogout() {
  const response = await axiosInstance.post('/api/auth/logout');

  return response;
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
        console.log('Refreshing...');

        await axiosInstance.post('/api/auth/refresh', {});

        return axiosInstance(originalRequest);
      } catch (refreshError) {
        console.warn('Token refresh failed:', refreshError);

        return Promise.reject(refreshError);
      }
    }
    return Promise.reject(error);
  }
);
