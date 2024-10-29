import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { LoginRequest, LoginResponse } from '../../Interfaces/LoginResponse';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }
  
  login(username: string, password: string)
  {
    const body: LoginRequest =
    {
      email: username,
      password: password
    };

    return this.http.post<LoginResponse>(`${this.apiUrl}api/login`, body);
  }
}
