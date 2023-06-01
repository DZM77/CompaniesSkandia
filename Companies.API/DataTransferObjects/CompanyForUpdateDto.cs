namespace Companies.API.DataTransferObjects
{
    public record CompanyForUpdateDto : CompanyForManipulationDto, IKey
    {
        public Guid Id { get; init; }
    }
}
