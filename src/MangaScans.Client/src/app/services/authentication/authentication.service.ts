import { Injectable, signal } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { LoginRequest, LoginResponse, RegisterRequest } from "../../Interfaces/LoginResponse";
import { Observable, of, catchError, tap, BehaviorSubject } from "rxjs";
import { flush } from "@angular/core/testing";
import { ActionResultResponse } from "../../Interfaces/ActionResultResponse";

@Injectable({
  providedIn: "root",
})
export class AuthService {
  private apiUrl = environment.apiUrl;

  private authenticated = new BehaviorSubject<boolean>(false);

  isAuthenticated = this.authenticated.asObservable();
  errors = signal<string[]>([]);

  constructor(private http: HttpClient) {}

  GetSession(): Promise<boolean> {

    return new Promise((resolve) => {

      if (!!this.authenticated)
        return resolve(true);

      const token = sessionStorage.getItem("token");
  
      if (token) {
  
        const header = new HttpHeaders({
          Authorization: `Bearer ${token}`,
        });
  
        // Primeira tentativa de validação com o token
        this.http.get<LoginResponse>(`${this.apiUrl}validate`, { headers: header }).subscribe({
          next: (response) => {
            this.authenticated.next(response.isAuthenticated);
            resolve(!!this.isAuthenticated);
          },
          error: (error) => {
            this.tryRefreshToken().then(resolve).catch(() => resolve(false));
          },
        });
      } else {
        this.tryRefreshToken().then(resolve).catch(() => resolve(false));
      }
    });
  }

  logout() {
    this.authenticated.next(false);
    this.ClearSession();
  }
  
  private tryRefreshToken(): Promise<boolean> {
    return new Promise((resolve, reject) => {
      const refreshToken = localStorage.getItem("refreshToken");
      const address = localStorage.getItem("address");
  
      if (refreshToken && address) {
        const headers = new HttpHeaders({
          Authorization: `Bearer ${refreshToken}`,
          address: address,
        });
  
        this.http.post<LoginResponse>(`${this.apiUrl}refresh-login`, {}, { headers }).subscribe({
          next: (response) => {
            this.authenticated.next(response.isAuthenticated);
            if (response.isAuthenticated) {
              // Atualiza os tokens e valores da sessão
              this.SetSessionValues(response);
            }
            resolve(!!this.isAuthenticated);
          },
          error: (error) => {
            this.authenticated.next(false);
            this.ClearSession();
            reject(false);
          }
        });
      } else {
        reject(false);
      }
    });
  }
  

  Login(username: string, password: string): Observable<LoginResponse | null> {
    const body: LoginRequest = {
      email: username,
      password: password,
    };
    console.log("Iniciando login com body:", body);

    return this.http.post<LoginResponse>(`${this.apiUrl}login`, body).pipe(
      tap((response) => {
        console.log("Login bem-sucedido, resposta:", response);
        this.SetSessionValues(response);
      }),
      catchError((error) => {
        console.log("Erro ao tentar logar:", error);
        this.errors.set(["Erro ao tentar logar."]);
        return of(null);
      }),
    );
  }

  // Método privado para definir valores da sessão
  private SetSessionValues(response: LoginResponse) {
    console.log("Setando valores da sessão com resposta:", response);

    if (response.isAuthenticated) {
      this.authenticated.next(true);

      if (response.address) {
        localStorage.setItem('address', response.address);
      }
      if (response.refreshToken) {
        localStorage.setItem("refreshToken", response.refreshToken);
      }
      if (response.token) {
        sessionStorage.setItem("token", response.token);
      }
      if (response.expiresIn) {
        localStorage.setItem("expiresIn", response.expiresIn.toString());
      }

      this.errors.set([]);
    } else {
      this.authenticated.next(false);
      this.ClearSession();
      this.errors.set(response.Erros || ["Erro de autenticação."]);
    }
  }

  // Método para limpar a sessão
  private ClearSession() {
    sessionStorage.removeItem("token");
    localStorage.removeItem("refreshToken");
    localStorage.removeItem("expiresIn");
    localStorage.removeItem("address");
  }

  // Método para registrar um novo usuário
  Register(username: string, password: string, confirmPassword: string): Observable<ActionResultResponse> {
    const body: RegisterRequest = {
      email: username,
      password: password,
      confirmPassword: confirmPassword,
    };

    return this.http.post<ActionResultResponse>(`${this.apiUrl}Register`, body);
  }
}
