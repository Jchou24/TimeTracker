using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Reflection;
using TimeTracker.DAL.Attributes;
using TimeTracker.DAL.DBModels;
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
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<MapUserRole> MapUserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            InitSqlDefaultValue(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.GuId)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.GuId)
                .ValueGeneratedOnAdd()
                .HasValueGenerator<GuidValueGenerator>();

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

            modelBuilder.Entity<UserRole>()
                .HasIndex(u => u.CodeName)
                .IsUnique();

            SeedData(modelBuilder);
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

        protected void SeedData(ModelBuilder modelBuilder)
        {
            // add admin, user
            modelBuilder
                .Entity<User>()
                .HasData(new User("admin@auth.com", "@Dmin")
                {
                    Id = 1,
                    Name = "Admin",
                    AccountStatus = AccountStatus.Approved
                });

            modelBuilder
                .Entity<User>()
                .HasData(new User("user@auth.com", "user1234")
                {
                    Id = 2,
                    Name = "User",
                    AccountStatus = AccountStatus.Approved
                });

            // add role
            modelBuilder
                .Entity<UserRole>()
                .HasData(new UserRole
                {
                    Id = (int)Models.UserRoles.Admin,
                    CodeName = Models.UserRoles.Admin.ToString(),
                    DisplayName = Models.UserRoles.Admin.ToString(),
                });

            modelBuilder
                .Entity<DBModels.UserRole>()
                .HasData(new DBModels.UserRole
                {
                    Id = (int)Models.UserRoles.User,
                    CodeName = Models.UserRoles.User.ToString(),
                    DisplayName = Models.UserRoles.User.ToString()
                });

            // add role of user
            modelBuilder
                .Entity<MapUserRole>()
                .HasData(new MapUserRole
                {
                    UserId = 1,
                    UserRolesId = (int)Models.UserRoles.Admin
                });

            modelBuilder
                .Entity<MapUserRole>()
                .HasData(new MapUserRole
                {
                    UserId = 2,
                    UserRolesId = (int)Models.UserRoles.User
                });
        }
    }
}
