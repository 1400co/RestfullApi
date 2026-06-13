using Template.Core.Dtos;
using System;
using System.Collections.Generic;

namespace Template.Core.Interfaces
{
    public interface ISessionService
    {
        Guid GetUserId();
        string? GetUserName();
        List<string> GetRoles();
        UserModelDto? GetCurrentUser();
    }
}