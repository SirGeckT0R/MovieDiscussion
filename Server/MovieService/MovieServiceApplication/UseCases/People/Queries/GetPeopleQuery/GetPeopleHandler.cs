using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.PersonSpecifications;

namespace MovieServiceApplication.UseCases.People.Queries.GetAllPeopleQuery
{
    public class GetPeopleHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetPeopleQuery, ICollection<PersonDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<ICollection<PersonDto>> Handle(GetPeopleQuery request, CancellationToken cancellationToken)
        {
            var specification = new PeopleByNameSpecification(request.Name);
            var people = await _unitOfWork.People.GetPaginatedWithSpecificationAsync(
                                                                                    specification,
                                                                                    request.PageIndex,
                                                                                    request.PageSize,
                                                                                    cancellationToken
                                                                                    );

            return _mapper.Map<ICollection<PersonDto>>(people);
        }
    }
}
