
export interface LoginResponse {
  isAuthenticated: boolean;
  address?: string;
  token?: string;
  refreshToken?: string;
  expiresIn?: Date;
  Erros: string[];
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  "email": string;
  "password": string;
  "confirmPassword": string;
}

  