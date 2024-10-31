import { Injectable, signal } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { LoginRequest, LoginResponse, RegisterRequest } from "../../Interfaces/LoginResponse";
import { Observable, of, catchError, tap } from "rxjs";
import { flush } from "@angular/core/testing";

@Injectable({
  providedIn: "root",
})
export class AuthService {
  private apiUrl = environment.apiUrl;

  isAuthenticated = signal<boolean>(false);
  errors = signal<string[]>([]);

  constructor(private http: HttpClient) {}

  GetSession(): Promise<boolean> {
    return new Promise((resolve) => {
      const token = sessionStorage.getItem("token");
  
      if (token) {
  
        const header = new HttpHeaders({
          Authorization: `Bearer ${token}`,
        });
  
        // Primeira tentativa de validação com o token
        this.http.get<LoginResponse>(`${this.apiUrl}validate`, { headers: header }).subscribe({
          next: (response) => {
            this.isAuthenticated.set(response.isAuthenticated);
            resolve(this.isAuthenticated());
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
            this.isAuthenticated.set(response.isAuthenticated);
            if (response.isAuthenticated) {
              // Atualiza os tokens e valores da sessão
              this.SetSessionValues(response);
            }
            resolve(this.isAuthenticated());
          },
          error: (error) => {
            this.isAuthenticated.set(false);
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
      this.isAuthenticated.set(true);
      console.log("Usuário autenticado, salvando informações da sessão");

      if (response.address) {
        console.log("Endereço salvo:", response.address);
        localStorage.setItem('address', response.address);
      }
      if (response.refreshToken) {
        console.log("Refresh token salvo");
        localStorage.setItem("refreshToken", response.refreshToken);
      }
      if (response.token) {
        console.log("Token salvo");
        sessionStorage.setItem("token", response.token);
      }
      if (response.expiresIn) {
        console.log("ExpiresIn salvo:", response.expiresIn);
        localStorage.setItem("expiresIn", response.expiresIn.toString());
      }

      this.errors.set([]);
    } else {
      console.log("Autenticação falhou, limpando sessão");
      this.isAuthenticated.set(false);
      this.ClearSession();
      this.errors.set(response.Erros || ["Erro de autenticação."]);
    }
  }

  // Método para limpar a sessão
  private ClearSession() {
    console.log("Limpando sessão");
    sessionStorage.removeItem("token");
    localStorage.removeItem("refreshToken");
    localStorage.removeItem("expiresIn");
    localStorage.removeItem("address");
  }

  // Método para registrar um novo usuário
  Register(username: string, password: string, confirmPassword: string): Observable<any> {
    const body: RegisterRequest = {
      email: username,
      password: password,
      confirmPassword: confirmPassword,
    };
    console.log("Iniciando registro com body:", body);

    return this.http.post(`${this.apiUrl}Register`, body).pipe(
      catchError((error) => {
        console.log("Erro ao registrar:", error);
        this.errors.set(["Erro ao registrar."]);
        return of(null);
      }),
    );
  }
}
