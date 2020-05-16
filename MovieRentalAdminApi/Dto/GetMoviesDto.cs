using System.ComponentModel;

namespace MovieRentalAdminApi.Controllers
{
    public class GetMoviesDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 25;
        public Sorter SortBy { get; set; } = Sorter.Title;
        public Order Order { get; set; } = Order.Asc;
    }
    public enum Sorter
    {
        Title = 0,
        Likes = 1
    }

    public enum Filter
    {
        Ununavailable = 0,
        Available = 1,
        [Browsable(false)] All = 2
    }
    public enum Order
    {
        Asc = 0,
        Desc = 1
    }

}