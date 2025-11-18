import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  VoteNpsRequest,
  VoteNpsResponse,
  NpsResult,
} from '../models/nps/Nps.models';
import { SessionActivityService } from './session-activity.service';

@Injectable({
  providedIn: 'root',
})
export class NpsService {
  private readonly apiUrl = 'http://localhost:5237/api';

  constructor(
    private http: HttpClient,
    private sessionActivity: SessionActivityService
  ) {}

  /**
   * Obtiene la pregunta NPS activa para mostrar al votante.
   * GET /api/nps/active
   */
  getActiveQuestion(): Observable<{ question: string }> {
    return this.http.get<{ question: string }>(`${this.apiUrl}/Nps/active`);
  }

  /**
   * Registra el voto NPS del usuario actual.
   * POST /api/nps/vote
   */
  vote(score: number): Observable<VoteNpsResponse> {
    const body: VoteNpsRequest = { score };
    this.sessionActivity.registerBusinessActivity();
    return this.http.post<VoteNpsResponse>(`${this.apiUrl}/Nps/vote`, body);
  }

  /**
   * Obtiene el resultado calculado del NPS
   * (solo para usuarios con rol Admin).
   * GET /api/nps/result
   */
  getResult(): Observable<NpsResult> {
    this.sessionActivity.registerBusinessActivity();
    return this.http.get<NpsResult>(`${this.apiUrl}/Nps/result`);
  }
}
