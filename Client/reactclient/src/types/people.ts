export interface Person {
  id: string;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  name: string;
}
export interface PaginatedPerson {
  items: Person[];
  pageIndex: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface CreatePersonRequest {
  firstName: string;
  lastName: string;
  dateOfBirth: string;
}
