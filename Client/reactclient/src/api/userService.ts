import { LoginRequest, Role } from '../types/user';
import { axiosInstance } from './global';

export const fetchLogin = async (body: LoginRequest) => {
  const response = await axiosInstance.postForm('/api/auth/login', body);

  return response;
};

export const fetchRegister = async (body: LoginRequest) => {
  const response = await axiosInstance.postForm('/api/auth/register', body);

  return response;
};

export async function fetchRole() {
  const response: Role = await axiosInstance
    .get('/api/role')
    .then((response) => response?.data);

  return response ?? Role.Guest;
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
