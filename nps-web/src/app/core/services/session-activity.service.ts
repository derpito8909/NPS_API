import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class SessionActivityService {
  private readonly inactivityMs = 5 * 60 * 1000;

  private readonly warningGraceMs = 10 * 1000;

  private inactivityTimerId: any = null;
  private warningTimerId: any = null;

  private warningSubject = new BehaviorSubject<boolean>(false);
  warning$ = this.warningSubject.asObservable();

  constructor(private authService: AuthService, private router: Router) {
    this.authService.currentUser$.subscribe((user) => {
      if (user) {
        this.startInactivityTimer();
      } else {
        this.clearAllTimers();
        this.warningSubject.next(false);
      }
    });
  }

  registerBusinessActivity(): void {
    if (!this.authService.isLoggedIn()) {
      return;
    }

    this.startInactivityTimer();
  }

  confirmExtendSession(): void {
    this.clearWarningTimer();

    this.authService.refreshToken().subscribe({
      next: () => {
        this.warningSubject.next(false);
        this.startInactivityTimer();
      },
      error: () => {
        this.forceLogout();
      },
    });
  }

  cancelAndLogout(): void {
    this.forceLogout();
  }

  private startInactivityTimer(): void {
    this.clearInactivityTimer();

    this.warningSubject.next(false);

    this.inactivityTimerId = setTimeout(() => {
      this.triggerWarning();
    }, this.inactivityMs);
  }

  private triggerWarning(): void {
    this.warningSubject.next(true);

    this.startWarningTimer();
  }

  private startWarningTimer(): void {
    this.clearWarningTimer();

    this.warningTimerId = setTimeout(() => {
      if (this.authService.isLoggedIn()) {
        this.forceLogout();
      }
    }, this.warningGraceMs);
  }

  private forceLogout(): void {
    this.warningSubject.next(false);
    this.clearAllTimers();
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  private clearInactivityTimer(): void {
    if (this.inactivityTimerId) {
      clearTimeout(this.inactivityTimerId);
      this.inactivityTimerId = null;
    }
  }

  private clearWarningTimer(): void {
    if (this.warningTimerId) {
      clearTimeout(this.warningTimerId);
      this.warningTimerId = null;
    }
  }

  private clearAllTimers(): void {
    this.clearInactivityTimer();
    this.clearWarningTimer();
  }
}
