import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NpsService } from '../../../core/services/nps.service';
import { VoteNpsResponse } from '../../../core/models/nps/Nps.models';

@Component({
  standalone: true,
  selector: 'app-nps-voter',
  imports: [CommonModule],
  templateUrl: './voter.component.html',
  styleUrl: './voter.component.scss',
})
export class VoterComponent implements OnInit {
  question = '';
  loadingQuestion = false;

  scores: number[] = Array.from({ length: 11 }, (_, i) => i);

  selectedScore: number | null = null;
  submitting = false;
  successMessage: string | null = null;
  errorMessage: string | null = null;

  hasVoted = false;

  constructor(private npsService: NpsService) {}

  ngOnInit(): void {
    this.loadQuestion();
  }

  private loadQuestion(): void {
    this.loadingQuestion = true;
    this.errorMessage = null;
    this.successMessage = null;

    this.npsService.getActiveQuestion().subscribe({
      next: (res) => {
        this.loadingQuestion = false;
        this.question = res.question;
      },
      error: (err) => {
        this.loadingQuestion = false;
        this.errorMessage =
          err?.error?.detail ?? 'No se pudo cargar la pregunta NPS.';
      },
    });
  }

  selectScore(score: number): void {
    if (this.hasVoted || this.submitting) {
      return;
    }
    this.selectedScore = score;
    this.errorMessage = null;
  }

  submitVote(): void {
    if (this.selectedScore === null || this.hasVoted) {
      this.errorMessage = 'Debe seleccionar un valor entre 0 y 10.';
      return;
    }

    this.submitting = true;
    this.errorMessage = null;
    this.successMessage = null;

    this.npsService.vote(this.selectedScore).subscribe({
      next: (res: VoteNpsResponse) => {
        this.submitting = false;
        this.hasVoted = true;
        this.successMessage = `${res.message}`;
      },
      error: (err) => {
        this.submitting = false;
        this.errorMessage =
          err?.error?.detail ?? 'No se pudo registrar el voto.';
      },
    });
  }
}
