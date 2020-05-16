using MovieRentalAdminApi.Domain.Events;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MovieRentalAdminApi.Domain.Entities
{
    public class MovieEntity : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; protected set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public double RentalPrice { get; set; }
        public double SalePrice { get; set; }
        public int Availability { get; set; }
        public int Likes { get; protected set; }
        public ICollection<MovieLikesEntity> UserAccountLikes { get; set; }
        public ICollection<ImageEntity> Images { get; set; }

        public MovieEntity Create(string title, string description, int stock, double rentalPrice, double salePrice, int availability)
        {
            if (string.IsNullOrEmpty(title.Trim()) || string.IsNullOrEmpty(description.Trim()) 
                || stock < 0 || rentalPrice < 0.0 || salePrice < 0.0)
                return null;
            return new MovieEntity()
            {
                Availability = availability,
                Description = description,
                Likes = 0,
                RentalPrice = rentalPrice,
                SalePrice = salePrice,
                Stock = stock,
                Title = title
            };
        }

        public bool Update(string title, string description, int stock, double rentalPrice, double salePrice, int availability)
        {
            if (string.IsNullOrEmpty(title.Trim()) || string.IsNullOrEmpty(description.Trim())
                || stock < 0 || rentalPrice < 0.0 || salePrice < 0.0)
                return false;

            MovieEntity oldMovie = new MovieEntity()
            {
                Title = Title,
                RentalPrice = RentalPrice,
                SalePrice = SalePrice,
                Id = Id
            };
            Description = description;
            RentalPrice = rentalPrice;
            SalePrice = salePrice;
            Stock = stock;
            Title = title;
            Availability = availability;

            if(oldMovie.Title != Title || oldMovie.RentalPrice != RentalPrice || oldMovie.SalePrice != SalePrice)
                AddDomainEvent(new MovieUpdated() { Movie = oldMovie });

            return true;
        }

        public bool Rent(int quantity, int daysForRent, double penalty)
        {
            if (quantity < 1)
                return false;
            if (Stock < quantity)
                return false;
            if (Availability == 0)
                return false;
            Stock -= quantity;
            if (Stock == 0)
                Availability = 0;
            AddDomainEvent(new MovieAction() { Movie = this, Quantity = quantity, Action = "RENT", DaysForRent = daysForRent, Penalty = penalty });
            return true;
        }

        public bool Buy(int quantity)
        {
            if (quantity < 1)
                return false;
            if (Stock < quantity)
                return false;
            if (Availability == 0)
                return false;
            Stock -= quantity;
            if (Stock == 0)
                Availability = 0;
            AddDomainEvent(new MovieAction() { Movie = this, Quantity = quantity, Action = "SOLD" });
            return true;
        }

        public void Like(UserAccountEntity user)
        {
            var userAccount = UserAccountLikes.FirstOrDefault(a => a.UserAccountId == user.Id);
            if (userAccount != null)
            {
                Likes--;
                UserAccountLikes.Remove(userAccount);
            }
            else
            {
                Likes++;
                UserAccountLikes.Add(new MovieLikesEntity
                {
                    UserAccountId = user.Id,
                    MovieId = Id
                });
            }
        }

        public void UploadImage(byte[] image, string imageTile)
        {
            var images = Images.FirstOrDefault(a => a.MovieId == Id);
            Images.Add(new ImageEntity
            {
                ImageData = image, 
                ImageTitle = imageTile
            });
        }

        public static List<MovieEntity> CreateDumpData()
        {
            return new List<MovieEntity>
            {
                new MovieEntity()
                {
                    Availability = 1,
                    Description = "An insomniac office worker and a devil-may-care soapmaker form an underground fight club that evolves into something much, much more.",
                    RentalPrice = 2.99,
                    SalePrice = 29.99,
                    Stock = 10,
                    Title = "Fight Club",
                    Likes = 2
                },
                new MovieEntity()
                {
                    Availability = 1,
                    RentalPrice = 2.99,
                    SalePrice = 19.99,
                    Stock = 3,
                    Description = "A grieving family is haunted by tragic and disturbing occurrences.",
                    Title = "Hereditary",
                    Likes = 1
                },
                new MovieEntity()
                {
                    Availability = 0,
                    Description = "Allied soldiers from Belgium, the British Empire, and France are surrounded by the German Army, and evacuated during a fierce battle in World War II.",
                    RentalPrice = 2.99,
                    SalePrice = 29.99,
                    Stock = 10,
                    Title = "Dunkirk",
                    Likes = 0
                },
                new MovieEntity()
                {
                    Availability = 1,
                    Description = "The drug-induced utopias of four Coney Island people are shattered when their addictions run deep.",
                    RentalPrice = 8.99,
                    SalePrice = 130,
                    Stock = 3,
                    Title = "Requiem for a Dream",
                    Likes = 0
                },
                new MovieEntity()
                {
                    Availability = 1,
                    Description = "In Nazi-occupied France during World War II, a plan to assassinate Nazi leaders by a group of Jewish U.S. soldiers coincides with a theatre owner's vengeful plans for the same.",
                    RentalPrice = 8.99,
                    SalePrice = 130,
                    Stock = 3,
                    Title = "Inglourious Basterds",
                    Likes = 0
                },
                new MovieEntity()
                {
                    Availability = 1,
                    Description = "A former neo-nazi skinhead tries to prevent his younger brother from going down the same wrong path that he did.",
                    RentalPrice = 8.99,
                    SalePrice = 130,
                    Stock = 3,
                    Title = "American History X",
                    Likes = 0
                },
            };
        }
    }
}
