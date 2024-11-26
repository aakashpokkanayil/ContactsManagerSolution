using ContactsManager.Core.DTO;
using ContactsManager.Core.Enum;

namespace ContactsManager.Core.ServiceContracts
{
    public interface IPersonsSorterService
    {
        List<PersonResponseDto>? GetSortedPersons(List<PersonResponseDto> persons, string sortby, SortOderOption order);

    }
}
