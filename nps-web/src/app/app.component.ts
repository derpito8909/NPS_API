import { Component, inject } from '@angular/core';
import { RouterOutlet, Router } from '@angular/router';
import { NgIf, AsyncPipe } from '@angular/common';
import { AuthService } from './core/services/auth.service';
import { SessionActivityService } from './core/services/session-activity.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NgIf, AsyncPipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  private sessionService = inject(SessionActivityService);

  currentUser$ = this.authService.currentUser$;
  sessionWarning$ = this.sessionService.warning$;

  logout(): void {
    this.authService.logout();

    this.router.navigate(['/login']);
  }
  continueSession(): void {
    this.sessionService.confirmExtendSession();
  }

  logoutFromWarning(): void {
    this.sessionService.cancelAndLogout();
  }
}
