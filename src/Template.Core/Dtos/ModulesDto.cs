namespace Template.Core.Dtos
{
    public record ModulesDto : BaseDto
    {
        public string ModuleName { get; set; } = string.Empty;
    }
}
