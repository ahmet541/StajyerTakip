
using Microsoft.EntityFrameworkCore;
using StajyerTakip.Models;

namespace StajyerTakip.Data
{
    public class StajyerContext: DbContext
    {
        public StajyerContext(DbContextOptions<StajyerContext> options)
            : base(options)
        {
        }

        public DbSet<StajyerTakip.Models.StajyerModel> ListOfStajyers { get; set; } = default!;
        public DbSet<StajyerTakip.Models.StajBasvuru> StajBasvurusu { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StajyerModel>().ToTable("StajyerModel");


        }
    }
}
