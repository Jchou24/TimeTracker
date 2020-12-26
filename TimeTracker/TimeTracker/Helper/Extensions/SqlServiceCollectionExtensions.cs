using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;
using TimeTracker.DAL;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.Extensions.Configuration;

namespace TimeTracker.Helper.Extensions
{
    public static class SqlServiceCollectionExtensions
    {
        public enum DBTypes
        {
            MS,
            PG
        }

        /// <summary>
        /// DBType: 'MS', 'PG'
        /// </summary>
        /// <param name="Configuration"></param>
        public static IServiceCollection InitDB(this IServiceCollection services, IConfiguration Configuration)
        {
            var dbType = Configuration.GetSection("DBType").Value;

            DBTypes db;
            switch (dbType)
            {
                case nameof(DBTypes.PG):
                    db = DBTypes.PG;
                    break;
                case nameof(DBTypes.MS):
                default:
                    db = DBTypes.MS;
                    break;
            }

            var connectionString = services.GetConnectioonString(Configuration, db);

            switch (db)
            {
                case DBTypes.PG:
                    return services.AddConfiguredNpgSqlDbContext(connectionString);
                case DBTypes.MS:
                default:
                    return services.AddConfiguredMsSqlDbContext(connectionString);
            }
        }

        public static string GetConnectioonString(this IServiceCollection services, IConfiguration Configuration, DBTypes dbTypes = DBTypes.MS)
        {
            var connectionString = Environment.GetEnvironmentVariable("TIMETRACKER_CONNECTIONNSTRING");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = Configuration.GetConnectionString(dbTypes.ToString());
            }

            return connectionString;
        }

        public static IServiceCollection AddConfiguredMsSqlDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<ApplicationDbContext>((serviceProvider, optionsBuilder) =>
                    optionsBuilder
                        .UseSqlServer(
                            connectionString,
                            sqlServerOptionsBuilder =>
                            {
                                sqlServerOptionsBuilder
                                    .CommandTimeout((int)TimeSpan.FromSeconds(30).TotalSeconds)
                                    .EnableRetryOnFailure()
                                    .MigrationsAssembly(typeof(SqlServiceCollectionExtensions).Assembly.FullName);
                            })
                        .AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>()));
            return services;
        }

        public static IServiceCollection AddConfiguredNpgSqlDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<ApplicationDbContext>((serviceProvider, optionsBuilder) =>
                    optionsBuilder
                        .UseNpgsql(
                            connectionString,
                            sqlServerOptionsBuilder =>
                            {
                                sqlServerOptionsBuilder
                                    .CommandTimeout((int)TimeSpan.FromSeconds(30).TotalSeconds)
                                    .EnableRetryOnFailure()
                                    .MigrationsAssembly(typeof(SqlServiceCollectionExtensions).Assembly.FullName);
                            })
                        .AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>()));
            return services;
        }



    }    
}
