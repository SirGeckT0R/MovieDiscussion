using AutoMapper;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.People.Queries.GetPersonByIdQuery
{
    public class GetPersonByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPersonByIdHandler> logger) : IQueryHandler<GetPersonByIdQuery, PersonDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetPersonByIdHandler> _logger = logger;

        public async Task<PersonDto> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            var person = await _unitOfWork.People.GetByIdAsync(request.Id, cancellationToken);

            if (person == null)
            {
                _logger.LogError("Get person by id {Id} command failed: person not found", request.Id);

                throw new NotFoundException("Person not found");
            }

            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<PersonDto>(person);
        }
    }
}
