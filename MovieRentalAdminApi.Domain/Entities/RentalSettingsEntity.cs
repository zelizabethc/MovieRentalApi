using System.Collections.Generic;

namespace MovieRentalAdminApi.Domain.Entities
{
    public class RentalSettingsEntity
    {
        public int Id { get; set; }
        public double Penalty { get; set; }
        public int DaysForRent { get; set; }
        public int Status { get; set; }

        public static List<RentalSettingsEntity> CreateDumpData()
        {
            return new List<RentalSettingsEntity>
            {
                new RentalSettingsEntity()
                {
                    DaysForRent = 2,
                    Penalty = 10.50,
                    Status = 1
                },
                new RentalSettingsEntity()
                {
                    DaysForRent = 7,
                    Penalty = 9.00,
                    Status = 0
                },
            };
        }
    }
}
