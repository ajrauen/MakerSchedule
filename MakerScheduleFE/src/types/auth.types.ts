export interface UserLogin {
  email: string;
  password?: string;
}

export interface LoginResponse {
  accessToken: string;
}
