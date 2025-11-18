export interface NpsResult {
  question: string;
  totalVotes: number;
  promoters: number;
  neutrals: number;
  detractors: number;
  nps: number;
}

export interface VoteNpsRequest {
  score: number;
}

export interface VoteNpsResponse {
  message: string;
  score: number;
  classification: string;
}
