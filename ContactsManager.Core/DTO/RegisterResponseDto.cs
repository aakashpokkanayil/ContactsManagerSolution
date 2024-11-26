using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.DTO
{
    public class RegisterResponseDto
    {
        public bool IsSucceeded { get; set; }
        public IEnumerable<IdentityError>? ErrorMessage { get; set; }
    }
}
