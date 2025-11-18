export interface StoredTokens {
  accessToken: string;
  refreshToken: string;
  role: 'Admin' | 'Voter';
  expiresAt: string;
}
