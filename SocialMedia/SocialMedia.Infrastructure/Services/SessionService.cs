using Microsoft.AspNetCore.Http;
using SocialMedia.Core.Dtos;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SocialMedia.Infrastructure.Services
{
    public class SessionService(IHttpContextAccessor httpContextAccessor) : ISessionService
    {

        private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

        public Guid GetUserId()
        {
            var claim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return claim != null ? Guid.Parse(claim) : Guid.Empty;
        }

        public string? GetUserName()
        {
            return User?.Identity?.Name;
        }

        public List<string> GetRoles()
        {
            return User?.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList() ?? new List<string>();
        }

        public UserModelDto? GetCurrentUser()
        {
            var user = User;
            if (user == null || user.Identity?.IsAuthenticated != true)
                return null;

            return new UserModelDto
            {
                Id = GetUserId(),
                UserName = GetUserName() ?? string.Empty,
                Email = GetUserName() ?? string.Empty,
                Roles = GetRoles()
            };
        }
    }
}