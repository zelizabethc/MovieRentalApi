using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRentalAdminApi.Domain.Entities
{
    public class ImageEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        public MovieEntity Movie { get; set; }
        public string ImageTitle { get; set; }
        public byte[] ImageData { get; set; }

        public static List<ImageEntity> CreateDumpData()
        {
            return new List<ImageEntity>
            {
                new ImageEntity()
                {
                    MovieId = 1,
                    ImageTitle = "pic.jpg"
                },
            };
        }
    }
}
