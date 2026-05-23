namespace TransforSerPu.Core.Dtos
{
    public record TokenDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
