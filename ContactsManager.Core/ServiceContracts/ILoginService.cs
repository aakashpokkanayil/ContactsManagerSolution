using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts
{
    public interface ILoginService
    {
        public Task<LoginResponseDto> LoginAsync(LoginDTO loginDTO);
    }
}
