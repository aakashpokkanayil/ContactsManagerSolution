using AutoMapper;
using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO;
using ContactsManager.Core.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace ContactsManager.Core.Services
{
    public class PersonsUpdaterServices:IPersonsUpdaterService
    {
        private readonly IMapper _mapper;
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonsAdderServices> _logger;

        public PersonsUpdaterServices(IMapper mapper, IPersonRepository personRepository, ILogger<PersonsAdderServices> logger)
        {

            _mapper = mapper;
            _personRepository = personRepository;
            _logger = logger;
        }

        public async Task<PersonResponseDto?> UpdatePerson(PersonUpdateRequestDto? personUpdateRequestDto)
        {
            if (personUpdateRequestDto == null) throw new ArgumentNullException(nameof(PersonUpdateRequestDto));
            if (string.IsNullOrEmpty(personUpdateRequestDto.PersonName))
                throw new ArgumentException("Name cant be null");

            Person? matchingPerson = await _personRepository.GetPersonById(personUpdateRequestDto.PersonId);
            if (matchingPerson == null) throw new ArgumentException("Person dont exists");

            await _personRepository.UpdatePerson(matchingPerson);

            return _mapper.Map<PersonResponseDto>(matchingPerson);

        }


    }
}
