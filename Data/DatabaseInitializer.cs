using StajyerTakip.Data;
using StajyerTakip.Models;

namespace KendoAppProgress.Data
{
    public class DataBaseInitializer
    {
        public static void Initialize(StajyerContext context)
        {
            // Look for any students.
            if (context.ListOfStajyers.Any())
            {
                return;   // DB has been seeded
            }

            var userModels = new StajyerModel[]
            {
                new StajyerModel {Id = 0, Name = "Ahmet Şahin", IdentificationNumber = "33333333333", StartingDate = new DateTime(2022, 8, 1), EndingDate = new DateTime(2022, 8, 11), LocationOfCV= "/wwwroot/CVs/..." }

            };

            context.ListOfStajyers.AddRange(userModels);
            context.SaveChanges();

        }
    }

}
