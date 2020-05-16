namespace MovieRentalAdminApi.Dto
{
    public class GetMoviesResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public double RentalPrice { get; set; }
        public double SalePrice { get; set; }
        public int Availability { get; set; }
        public int Likes { get; set; }
    }
}