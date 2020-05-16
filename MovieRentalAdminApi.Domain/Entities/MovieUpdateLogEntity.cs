namespace MovieRentalAdminApi.Domain.Entities
{
    public class MovieUpdateLogEntity : LogEntity
    {
        public int IdMovie { get; set; }
        public double RentalPrice { get; set; }
        public double SalePrice { get; set; }
        public string Title { get; set; }
    }
}
