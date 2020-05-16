namespace MovieRentalAdminApi.CrossCutting.Dto
{
    public class PaginationDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public string Order { get; set; }
        public int Filter { get; set; }
    }
}