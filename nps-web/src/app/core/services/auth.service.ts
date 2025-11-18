import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { LoginRequest, LoginResponse } from '../models/auth/login.models';
import { StoredTokens } from '../models/auth/storeTokens.models';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = 'http://localhost:5237/api';
  private readonly storageKey = 'nps_auth';

  private currentUserSubject: BehaviorSubject<StoredTokens | null>;
  currentUser$;

  constructor(private http: HttpClient) {
    const initial = this.loadFromStorage();
    this.currentUserSubject = new BehaviorSubject<StoredTokens | null>(initial);
    this.currentUser$ = this.currentUserSubject.asObservable();
  }

  login(payload: LoginRequest): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.apiUrl}/auth/login`, payload)
      .pipe(tap((res) => this.saveTokens(res)));
  }

  refreshToken(): Observable<LoginResponse> {
    const current = this.currentUserSubject.value;
    if (!current) {
      throw new Error('No hay sesión para refrescar');
    }

    return this.http
      .post<LoginResponse>(`${this.apiUrl}/auth/refresh`, {
        refreshToken: current.refreshToken,
      })
      .pipe(tap((res) => this.saveTokens(res)));
  }

  logout(): void {
    localStorage.removeItem(this.storageKey);
    this.currentUserSubject.next(null);
  }

  getAccessToken(): string | null {
    return this.currentUserSubject.value?.accessToken ?? null;
  }

  getRole(): 'Admin' | 'Voter' | null {
    return this.currentUserSubject.value?.role ?? null;
  }

  isLoggedIn(): boolean {
    return !!this.currentUserSubject.value;
  }

  private saveTokens(res: LoginResponse) {
    const stored: StoredTokens = {
      accessToken: res.accessToken,
      refreshToken: res.refreshToken,
      role: res.role,
      expiresAt: res.accessTokenExpiresAt,
    };

    localStorage.setItem(this.storageKey, JSON.stringify(stored));
    this.currentUserSubject.next(stored);
  }

  private loadFromStorage(): StoredTokens | null {
    const data = localStorage.getItem(this.storageKey);
    if (!data) {
      return null;
    }

    try {
      return JSON.parse(data) as StoredTokens;
    } catch {
      localStorage.removeItem(this.storageKey);
      return null;
    }
  }
}
