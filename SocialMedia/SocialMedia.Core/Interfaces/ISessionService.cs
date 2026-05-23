using SocialMedia.Core.Dtos;
using System;
using System.Collections.Generic;

namespace SocialMedia.Core.Interfaces
{
    public interface ISessionService
    {
        Guid GetUserId();
        string GetUserName();
        List<string> GetRoles();
        UserModelDto GetCurrentUser();
    }
}