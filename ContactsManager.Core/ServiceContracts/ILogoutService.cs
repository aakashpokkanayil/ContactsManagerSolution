using ContactsManager.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.ServiceContracts
{
    public interface ILogoutService
    {
        public Task<LogoutResponseDto> LogoutAsync();
    }
}
