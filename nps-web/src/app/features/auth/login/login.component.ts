// src/app/features/auth/login/login.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  standalone: true,
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  loading = false;
  error: string | null = null;
  form;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  submit(): void {
    if (this.form.invalid) {
      return;
    }

    this.loading = true;
    this.error = null;

    this.authService.login(this.form.value as any).subscribe({
      next: (res) => {
        this.loading = false;
        if (res.role === 'Admin') {
          this.router.navigate(['/nps/admin']);
        } else {
          this.router.navigate(['/nps/vote']);
        }
      },
      error: (err) => {
        this.loading = false;
        this.error = err?.error?.detail ?? 'Error al iniciar sesión.';
      },
    });
  }
}
