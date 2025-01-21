using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.People.Queries.GetPersonByIdQuery
{
    public class GetPersonByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetPersonByIdQuery, PersonDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<PersonDto> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            var person = await _unitOfWork.People.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Person not found");

            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<PersonDto>(person);
        }
    }
}
