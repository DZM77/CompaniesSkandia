namespace Companies.API.Paging
{
    public class ResourceParameters
    {
        private int pageSize = 2;
        const int maxPageSize = 20;
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > maxPageSize ? maxPageSize : value;
        }
    }
}
