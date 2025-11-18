import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NpsService } from '../../../core/services/nps.service';
import { NpsResult } from '../../../core/models/nps/Nps.models';

@Component({
  standalone: true,
  selector: 'app-nps-admin',
  imports: [CommonModule],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss',
})
export class AdminComponent implements OnInit {
  loading = false;
  errorMessage: string | null = null;
  result: NpsResult | null = null;

  constructor(private npsService: NpsService) {}

  ngOnInit(): void {
    this.loadResult();
  }

  loadResult(): void {
    this.loading = true;
    this.errorMessage = null;

    this.npsService.getResult().subscribe({
      next: (res) => {
        this.loading = false;
        this.result = res;
      },
      error: (err) => {
        this.loading = false;
        this.errorMessage =
          err?.error?.detail ?? 'No se pudo obtener el resultado NPS.';
      },
    });
  }

  getNpsColor(): string {
    if (!this.result) {
      return 'gray';
    }

    if (this.result.nps >= 50) {
      return 'green';
    }
    if (this.result.nps >= 0) {
      return 'orange';
    }
    return 'red';
  }
}
