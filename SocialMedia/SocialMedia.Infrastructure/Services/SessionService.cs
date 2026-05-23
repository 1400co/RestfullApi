using Microsoft.AspNetCore.Http;
using SocialMedia.Core.Dtos;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SocialMedia.Infrastructure.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public Guid GetUserId()
        {
            var claim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return claim != null ? Guid.Parse(claim) : Guid.Empty;
        }

        public string GetUserName()
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

        public UserModelDto GetCurrentUser()
        {
            if (User == null || !User.Identity.IsAuthenticated)
                return null;

            return new UserModelDto
            {
                Id = GetUserId(),
                UserName = GetUserName(),
                Email = GetUserName(),
                Roles = GetRoles()
            };
        }
    }
}