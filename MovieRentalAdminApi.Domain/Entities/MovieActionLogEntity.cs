namespace MovieRentalAdminApi.Domain.Entities
{
    public class MovieActionLogEntity : LogEntity
    {
        public int IdMovie { get; set; }
        public int Quantity { get; set; }
        public string Action { get; set; }

    }
}
