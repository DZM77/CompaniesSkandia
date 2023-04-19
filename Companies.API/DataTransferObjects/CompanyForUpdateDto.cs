namespace Companies.API.DataTransferObjects
{
    public record CompanyForUpdateDto : CompanyForManipulationDto
    {
        public Guid Id { get; init; }
    }
}
