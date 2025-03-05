export interface Discussion {
  id: string;
  title: string;
  description: string;
  startAt: string;
  createdBy: string;
  isActive: boolean;
  subscribers: string[];
}

export interface CreateDiscussionRequest {
  createdBy: string | null;
  title: string;
  description: string;
  startAt: string;
}

export interface UpdateDiscussionRequest {
  id: string;
  updatedBy: string | null;
  title: string;
  description: string;
}

export interface Message {
  userId: string;
  text: string;
  username: string;
  sentAt: string;
}
