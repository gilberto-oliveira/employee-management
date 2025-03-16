import { Injectable } from '@angular/core';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode'; 
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private authUrl: string = `${environment.apiUrl}employees/auth`; 

  constructor() {}

  async authenticate(username: string, password: string): Promise<any> {
    try {
      const response = await axios.post(this.authUrl, {
        docNumber: username,
        password: password,
      });

      if (response.data && response.data.accessToken) {
        this.storeToken(response.data.accessToken);
        return response.data;
      } else {
        throw new Error('Authentication failed. No token received.');
      }
    } catch (error) {
      console.error('Authentication error:', error);
      throw error;
    }
  }

  private storeToken(token: string): void {
    localStorage.setItem('auth_token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('auth_token');
  }

  decodeToken(token: string): any {
    try {
      return jwtDecode(token);  // Decode and return the payload
    } catch (error) {
      console.error('Error decoding token:', error);
      return null;  // Return null if token is invalid
    }
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    if (token) {
      const decodedToken = this.decodeToken(token);
      if (decodedToken && decodedToken.exp) {
        const currentTime = Date.now() / 1000;
        if (decodedToken.exp > currentTime) {
          return true;  // Token is valid and not expired
        } else {
          this.logout();  // Token expired, log out the user
        }
      }
    }
    return false;  // No token or invalid token
  }

  getDecodedToken(): any {
    const token = this.getToken();
    if (token) {
      return this.decodeToken(token);  // Return the decoded token (payload)
    }
    return null;  // No token in localStorage
  }

  logout(): void {
    localStorage.removeItem('auth_token');
  }
}
