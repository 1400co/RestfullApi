using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastructure.Interfaces;
using System;

namespace SocialMedia.Infrastructure.Services
{
    public class UriService(string baseUrl) : IUriService
    {

        public Uri GetPostPaginationUri(PostQueryFilter filter, string actionUrl)
        {
            var fullUrl = $"{baseUrl}{actionUrl}";
            return new Uri(fullUrl);
        }
    }
}
