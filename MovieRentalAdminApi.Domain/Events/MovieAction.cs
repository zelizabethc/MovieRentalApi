using MovieRentalAdminApi.Domain.Entities;
using System;

namespace MovieRentalAdminApi.Domain.Events
{
    public class MovieAction : IDomainEvent
    {
        public MovieEntity Movie { get; set; }
        public int Quantity { get; set; }
        public string Action { get; set; }
        public int DaysForRent { get; set; }
        public double Penalty { get; set; }
        public DateTime DueDate { get; set; }
    }
}