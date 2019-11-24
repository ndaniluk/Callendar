using Microsoft.EntityFrameworkCore;

namespace Callendar
{
    public class CallendarDbContext : DbContext
    {
        public CallendarDbContext(DbContextOptions<CallendarDbContext> options)
            : base(options)
        {
        }

        public DbSet<Absence> Absences { get; set; }
        public DbSet<TakenAbsence> TakenAbsences { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskCategory> TaskCategories { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TakenAbsence>()
                .HasKey(takenAbsence => new
                {
                    takenAbsence.UserId, takenAbsence.AbsenceId
                });

            modelBuilder.Entity<TakenAbsence>()
                .HasOne(takenAbsence => takenAbsence.User)
                .WithMany(user => user.TakenAbsences)
                .HasForeignKey(takenAbsence => takenAbsence.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TakenAbsence>()
                .HasOne(takenAbsence => takenAbsence.Absence)
                .WithMany(absence => absence.TakenAbsences)
                .HasForeignKey(takenAbsence => takenAbsence.AbsenceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(user => user.Permission)
                .WithMany(permission => permission.Users)
                .HasForeignKey(user => user.PermissionId);

            modelBuilder.Entity<Task>()
                .HasOne(task => task.TaskCategory)
                .WithMany(taskCategory => taskCategory.Tasks)
                .HasForeignKey(task => task.TaskCategoryId);

            modelBuilder.Entity<Task>()
                .HasOne(task => task.User)
                .WithMany(user => user.Tasks)
                .HasForeignKey(task => task.UserId);

            modelBuilder.Entity<User>()
                .HasOne(user => user.Team)
                .WithMany(team => team.Users)
                .HasForeignKey(user => user.TeamId);
        }
    }
}