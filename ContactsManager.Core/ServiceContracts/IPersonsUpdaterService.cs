﻿using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts
{
    public interface IPersonsUpdaterService
    {
        Task<PersonResponseDto?> UpdatePerson(PersonUpdateRequestDto? personUpdateRequestDto);
    }
}
