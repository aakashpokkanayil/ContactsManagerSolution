using ContactsManager.Core.Domain.Entities;
using System.Linq.Expressions;
namespace ContactsManager.Core.Domain.RepositoryContracts
{
    public interface IPersonRepository
    {
        Task<Person> AddPerson(Person person);
        Task<List<Person>> GetAllPersons();
        Task<Person?> GetPersonById(Guid? personId);
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person,bool>> predicate);
        Task<bool> DeletePersonById(Guid? personId);
        Task<Person?> UpdatePerson(Person person);
    }
}
