namespace MovieRentalAdminApi.Dto
{
    public class AddMovieDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public double RentalPrice { get; set; }
        public double SalePrice { get; set; }
        public int Availability { get; set; }

    }
}