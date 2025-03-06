import { CrewMember } from '../types/movie';

export const groupOnRole = (crew: CrewMember[]): Array<CrewMember[]> =>
  Object.values(
    crew
      ?.sort((x) => x.role)
      ?.reduce((r, o) => {
        (r[o.role] = r[o.role] || []).push(o);
        return r;
      }, Object.create(null))
  );
