import { CrewMember } from '../types/movie';

export const groupOnRole = (crew: CrewMember[]): CrewMember[][] =>
  Object.values(
    crew
      ?.toSorted((x) => x.role)
      ?.reduce((r, o) => {
        (r[o.role] = r[o.role] || []).push(o);

        return r;
      }, Object.create(null))
  );
