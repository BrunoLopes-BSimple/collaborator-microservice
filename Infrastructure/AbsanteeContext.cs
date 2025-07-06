using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AbsanteeContext : DbContext
    {
        public virtual DbSet<CollaboratorDataModel> Collaborators { get; set; }
        public virtual DbSet<UserDataModel> Users { get; set; }
        public virtual DbSet<CollaboratorWithoutUserDataModel> CollaboratorsTemp { get; set; }

        public AbsanteeContext(DbContextOptions<AbsanteeContext> options) : base(options)
        {
            Console.WriteLine($"[DEBUG] DB Connection: {Database.GetDbConnection().ConnectionString}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CollaboratorDataModel>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<CollaboratorDataModel>()
                .OwnsOne(a => a.PeriodDateTime);

            modelBuilder.Entity<UserDataModel>().HasKey(v => v.Id);

            modelBuilder.Entity<CollaboratorWithoutUserDataModel>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<CollaboratorWithoutUserDataModel>()
                .OwnsOne(a => a.PeriodDateTime);

            base.OnModelCreating(modelBuilder);
        }
    }
}