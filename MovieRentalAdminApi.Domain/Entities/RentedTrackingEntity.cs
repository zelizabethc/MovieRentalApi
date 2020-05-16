using System;

namespace MovieRentalAdminApi.Domain.Entities
{
    public class RentedTrackingEntity
    {
        public int Id { get; set; }
        public int IdMovie { get; set; }
        public int Quantity { get; set; }
        public int DaysForRent { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        private DateTime _duedate { get; set; }
        public DateTime DueDate
        {
            get => _duedate;
            set { _duedate = Date.AddDays(DaysForRent); }
        }
        public double Penalty { get; set; }
    }
}