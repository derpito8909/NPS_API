// src/app/core/guards/role.guard.ts
import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const roleGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const expectedRole = route.data['role'] as 'Admin' | 'Voter';
  const currentRole = authService.getRole();

  if (currentRole && expectedRole === currentRole) {
    return true;
  }

  if (authService.isLoggedIn()) {
    return currentRole === 'Admin'
      ? router.parseUrl('/nps/admin')
      : router.parseUrl('/nps/vote');
  }

  return router.parseUrl('/login');
};
