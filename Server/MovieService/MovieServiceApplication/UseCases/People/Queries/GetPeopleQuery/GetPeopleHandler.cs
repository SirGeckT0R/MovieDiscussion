using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.PersonSpecifications;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.People.Queries.GetAllPeopleQuery
{
    public class GetPeopleHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetPeopleQuery, PaginatedCollection<PersonDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<PaginatedCollection<PersonDto>> Handle(GetPeopleQuery request, CancellationToken cancellationToken)
        {
            var specification = new PeopleByNameSpecification(request.Name);
            var (people, totalPages) = await _unitOfWork.People.GetPaginatedWithSpecificationAsync(
                                                                                    specification,
                                                                                    request.PageIndex,
                                                                                    request.PageSize,
                                                                                    cancellationToken
                                                                                    );
            cancellationToken.ThrowIfCancellationRequested();
            var peopleDtoCollection = _mapper.Map<ICollection<PersonDto>>(people);

            var paginatedCollection = new PaginatedCollection<PersonDto>(peopleDtoCollection, request.PageIndex, totalPages);

            return paginatedCollection;
        }
    }
}
