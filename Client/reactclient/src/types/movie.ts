import { Genre } from './genre';

export interface Movie {
  id: string;
  title: string;
  description: string;
  releaseDate: string;
  rating: number;
  genres: Array<Genre>;
  crewMembers: Array<CrewMember>;
  image: string;
}

export interface MovieFilters {
  name?: string;
  genres?: string[];
  crewMember?: string;
}

export interface PaginatedMovies {
  items: Movie[];
  pageIndex: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface CreateMovieRequest {
  title: string;
  description: string;
  releaseDate: string;
  genres: Array<string>;
  crewMembers: Array<CrewMember>;
  file: Blob | null;
  image: string | null;
}

export interface UpdateMovieRequest {
  id: string;
  title: string;
  description: string;
  releaseDate: string;
  genres: Array<string>;
  crewMembers: Array<CrewMember>;
  file: Blob | null;
  image: string | null;
}

export interface DeleteMovieRequest {
  id: string;
  image: string | null;
}

export interface CrewMember {
  personId: string;
  role: CrewRole;
  fullName: string | null;
}

export enum CrewRole {
  None = 0,
  Director = 1,
  Producer = 2,
  Screenwriter = 3,
  Actor = 4,
}

export const crewRoles = [
  CrewRole.None,
  CrewRole.Actor,
  CrewRole.Director,
  CrewRole.Producer,
  CrewRole.Screenwriter,
];

export interface ManageMovieApprovalRequest {
  movieId: string;
  shouldApprove: boolean;
}
