using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts
{
    public interface IPersonsGetterService
    {
        Task<List<PersonResponseDto>?> GetAllPerson();
        Task<PersonResponseDto?> GetPersonById(Guid? personId);
        Task<List<PersonResponseDto>?> GetPersonByFilter(string searchBy, string? searchString);
    }
}
