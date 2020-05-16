using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MovieRentalAdminApi.DataAccess;
using MovieRentalAdminApi.Domain.Entities;

namespace MovieRentalAdminApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            using (var serviceScope = host.Services.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<MovieRentalDbContext>();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.UserAccount.AddRange(UserAccountEntity.CreateDumpData());
                context.Movie.AddRange(MovieEntity.CreateDumpData());
                //context.Image.AddRange(ImageEntity.CreateDumpData());
                //context.MovieLikes.AddRange(MovieLikesEntity.CreateDumpData());
                context.RentalSettings.AddRange(RentalSettingsEntity.CreateDumpData());
                context.SaveChanges();
            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
               .Build();
    }
}
