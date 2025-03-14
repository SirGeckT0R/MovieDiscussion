export interface Genre {
  id: string;
  name: string;
}

export interface CreateGenreRequest {
  name: string;
}

export interface UpdateGenreRequest {
  id: string;
  name: string;
}

export interface DeleteGenreRequest {
  id: string;
}
