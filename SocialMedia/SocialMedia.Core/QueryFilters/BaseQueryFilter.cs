
namespace SocialMedia.Core.QueryFilters
{
    public abstract class BaseQueryFilter
    {
        public string Filter { get; set; } = string.Empty;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
