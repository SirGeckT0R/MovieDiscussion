export interface Movie {
  id: string;
  title: string;
  description: string;
  releaseDate: string;
  rating: number;
  genres: Array<string>;
  crewMembers: Array<CrewMember>;
  image: string;
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

export interface CrewMember {
  personId: string;
  role: CrewRole;
  fullName: string | null;
}

export enum CrewRole {
  None = 0,
  Actor = 1,
  Director = 2,
  Producer = 3,
  Screenwriter = 4,
}
export const crewRoles = [
  CrewRole.None,
  CrewRole.Actor,
  CrewRole.Director,
  CrewRole.Producer,
  CrewRole.Screenwriter,
];
