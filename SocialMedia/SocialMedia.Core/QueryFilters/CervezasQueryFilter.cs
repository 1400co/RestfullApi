namespace SocialMedia.Core.QueryFilters
{
    public class CervezasQueryFilter : BaseQueryFilter
    {
        public string? Nombre { get; set; }
        public decimal? GradosAlcoholMin { get; set; }
        public decimal? GradosAlcoholMax { get; set; }
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }
    }
}
