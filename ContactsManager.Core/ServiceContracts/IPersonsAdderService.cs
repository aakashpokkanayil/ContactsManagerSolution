using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts
{
    public interface IPersonsAdderService
    {
        Task<PersonResponseDto?> AddPerson(PersonAddRequestDto? personAddRequestDto);

    }
}
