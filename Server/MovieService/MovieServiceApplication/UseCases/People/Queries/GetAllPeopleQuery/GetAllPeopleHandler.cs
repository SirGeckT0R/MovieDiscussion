using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;

namespace MovieServiceApplication.UseCases.People.Queries.GetAllPeopleQuery
{
    public class GetAllPeopleHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetAllPeopleQuery, ICollection<PersonDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<ICollection<PersonDto>> Handle(GetAllPeopleQuery request, CancellationToken cancellationToken)
        {
            var people = await _unitOfWork.People.GetAllAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<ICollection<PersonDto>>(people);
        }
    }
}
