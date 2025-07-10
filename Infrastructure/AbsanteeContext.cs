using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AbsanteeContext : DbContext
    {
        public virtual DbSet<CollaboratorDataModel> Collaborators { get; set; }
        public DbSet<UserDataModel> ValidUserIds { get; set; }
        public virtual DbSet<CollaboratorWithoutUserDataModel> TempCollaborators { get; set; } 
        public AbsanteeContext(DbContextOptions<AbsanteeContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CollaboratorDataModel>().OwnsOne(a => a.PeriodDateTime);
            modelBuilder.Entity<UserDataModel>().HasKey(v => v.Id);
            modelBuilder.Entity<CollaboratorWithoutUserDataModel>().HasKey(t => t.Id);
            modelBuilder.Entity<CollaboratorWithoutUserDataModel>().OwnsOne(a => a.PeriodDateTime);

            base.OnModelCreating(modelBuilder);
        }
    }
}