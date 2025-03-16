import axios from 'axios';
import { environment } from '../../../../environments/environment';

const apiClient = axios.create({
  baseURL: environment.apiUrl,
  headers: {
    'Content-Type': 'application/json',
  },
});

apiClient.interceptors.request.use(
  (config) => {
    const accessToken = localStorage.getItem('auth_token'); 

    if (accessToken) {
      config.headers['Authorization'] = `Bearer ${accessToken}`;
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default apiClient;
