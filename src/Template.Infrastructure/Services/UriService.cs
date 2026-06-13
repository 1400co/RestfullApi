using Template.Infrastructure.Interfaces;
using System;

namespace Template.Infrastructure.Services
{
    public class UriService(string baseUrl) : IUriService
    {
        public Uri GetPageUri(int pageNumber, int pageSize, string? filter, string actionUrl)
        {
            var fullUrl = $"{baseUrl}{actionUrl}";
            var query = $"PageNumber={pageNumber}&PageSize={pageSize}";
            if (!string.IsNullOrWhiteSpace(filter))
                query += $"&Filter={Uri.EscapeDataString(filter)}";
            return new Uri($"{fullUrl}?{query}");
        }
    }
}
