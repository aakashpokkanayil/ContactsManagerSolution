using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.DTO
{
    public class LoginResponseDto
    {
        public bool IsSucceeded { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
