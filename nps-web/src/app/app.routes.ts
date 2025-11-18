import { NgModule } from '@angular/core';
import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/rol.guard';
import { LoginComponent } from './features/auth/login/login.component';
import { VoterComponent } from './features/nps/voter/voter.component';
import { AdminComponent } from './features/nps/admin/admin.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: 'nps/vote',
    component: VoterComponent,
    canActivate: [authGuard, roleGuard],
    data: { role: 'Voter' },
  },
  {
    path: 'nps/admin',
    component: AdminComponent,
    canActivate: [authGuard, roleGuard],
    data: { role: 'Admin' },
  },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' },
];
