export interface Discussion {
  id: string;
  title: string;
  description: string;
  startAt: string;
  createdBy: string;
  isActive: boolean;
  subscribers: unknown;
}

export interface Message {
  text: string;
  username: string;
  sentAt: string;
}
