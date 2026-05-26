using System;

namespace SocialMedia.Infrastructure.Interfaces
{
    public interface IUriService
    {
        Uri GetPageUri(int pageNumber, int pageSize, string? filter, string actionUrl);
    }
}
