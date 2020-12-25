using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Reflection;
using TimeTracker.DAL.Attributes;
using TimeTracker.DAL.DBModels.Auth;
using TimeTracker.DAL.DBModels.Task;
using TimeTracker.DAL.Models;

namespace TimeTracker.DAL
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<UserImage> UserImage { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<MapUserRole> MapUserRoles { get; set; }
        
        public DbSet<Period> Period { get; set; }
        public DbSet<TaskDay> TaskDay { get; set; }
        public DbSet<Task> Task { get; set; }
        public DbSet<TaskTimeRange> TaskTimeRange { get; set; }
        public DbSet<TaskType> TaskType { get; set; }
        public DbSet<TaskSource> TaskSource { get; set; }
        public DbSet<NonWorkDays> NonWorkDays { get; set; }
        public DbSet<DayWorkLimitTime> DayWorkLimitTime { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            InitSqlDefaultValue(modelBuilder);
            InitUnique(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.Guid)
                .ValueGeneratedOnAdd()
                .HasValueGenerator<GuidValueGenerator>();

            modelBuilder.Entity<UserImage>()
                .HasOne(a => a.User);

            modelBuilder.Entity<User>()
                .HasMany(p => p.UserRoles)
                .WithMany(p => p.Users)
                .UsingEntity<MapUserRole>(
                    j => j
                        .HasOne(pt => pt.UserRoles)
                        .WithMany(t => t.MapUserRoles)
                        .HasForeignKey(pt => pt.UserRolesId),
                    j => j
                        .HasOne(pt => pt.User)
                        .WithMany(p => p.MapUserRoles)
                        .HasForeignKey(pt => pt.UserId),
                    j =>
                    {
                        j.HasKey(t => new { t.UserId, t.UserRolesId });
                    });

            modelBuilder.Entity<User>()
                .HasMany(t => t.TaskDay)
                .WithOne(d => d.User)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TaskDay>()
                .HasMany(t => t.Task)
                .WithOne(d => d.TaskDay)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TaskDay>()
                .HasMany(t => t.TaskTimeRange)
                .WithOne(d => d.TaskDay)
                .OnDelete(DeleteBehavior.SetNull);


            SeedData.Init(modelBuilder);
        }

        protected void InitSqlDefaultValue(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    var memberInfo = property.PropertyInfo ?? (MemberInfo)property.FieldInfo;
                    if (memberInfo == null) continue;
                    var defaultValue = Attribute.GetCustomAttribute(memberInfo, typeof(SqlDefaultValueAttribute)) as SqlDefaultValueAttribute;
                    if (defaultValue == null) continue;

                    property.SetDefaultValueSql(defaultValue.DefaultValue);
                }
            }
        }

        protected void InitUnique(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    var memberInfo = property.PropertyInfo ?? (MemberInfo)property.FieldInfo;
                    if (memberInfo == null) continue;
                    var defaultValue = Attribute.GetCustomAttribute(memberInfo, typeof(UniqueAttribute)) as UniqueAttribute;
                    if (defaultValue == null) continue;

                    entityType.AddIndex(property).IsUnique = true;
                }
            }
        }

    }
}
