using FootballManager.Data;
using FootballManager.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace FootballManager.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            PrepareDatabase(host);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void PrepareDatabase(IHost host)
        {
            var config = host.Services.GetRequiredService<IConfiguration>();
            var databaseConfig = config.GetConnectionString(nameof(FootballManagerContext));
            MigrateDatabase(databaseConfig);
        }

        private static void MigrateDatabase(string databaseConfig)
        {
            var services = new ServiceCollection();
            services.AddDbContext<FootballManagerContext>(options =>
                options.UseSqlServer(databaseConfig)
                    .EnableSensitiveDataLogging());

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    using (var context = scope.ServiceProvider.GetService<FootballManagerContext>())
                    {
                        context.Database.Migrate();

                        //context.Clubs.Add(new Club
                        //{
                        //    Id = 1,
                        //    ClubName = "Arsenal",
                        //    ClubImageUrl = "https://resources.premierleague.com/premierleague/badges/t3.svg",
                        //    Founded = new DateTime(1886, 1, 1).Date,
                        //    City = "London"
                        //});

                        //context.Clubs.Add(new Club
                        //{
                        //    Id = 2,
                        //    ClubName = "Chelsea",
                        //    ClubImageUrl = "https://resources.premierleague.com/premierleague/badges/t8.svg",
                        //    Founded = new DateTime(1905, 3, 10).Date,
                        //    City = "London"
                        //});

                        //context.Stadiums.Add(new Stadium
                        //{
                        //    Id = 1,
                        //    StadiumName = "Emirates Stadium",
                        //    StadiumImageUrl = "https://resources.premierleague.com/premierleague/photo/2016/07/21/ccade424-00e6-4310-a183-48f7101b1f5e/Arsenal_Stadium_Emirates.jpeg",
                        //    Capacity = 60272,
                        //    ClubId = 1
                        //});

                        //context.Stadiums.Add(new Stadium
                        //{
                        //    Id = 2,
                        //    StadiumName = "Stamford Bridge",
                        //    StadiumImageUrl = "https://resources.premierleague.com/premierleague/photo/2016/09/22/c9c78d39-60fb-47c6-9e20-837efb242ac5/Stamford_Bridge.jpg",
                        //    Capacity = 41798,
                        //    ClubId = 2
                        //});

                        //context.Coaches.Add(new Coach
                        //{
                        //    Id = 1,
                        //    Name = "Mikel",
                        //    Surname = "Arteta",
                        //    ClubId = 1
                        //});

                        //context.Coaches.Add(new Coach
                        //{
                        //    Id = 2,
                        //    Name = "Frank",
                        //    Surname = "Lampard",
                        //    ClubId = 2
                        //});

                        //context.Footballers.AddRange(new List<Footballer>
                        //{
                        //    new Footballer { Id = 1, Name = "Bernd", Surname = "Leno", ClubId = 1 },
                        //    new Footballer { Id = 2, Name = "Kieran", Surname = "Tierney", ClubId = 1 },
                        //    new Footballer { Id = 3, Name = "Nicolas", Surname = "Pépé", ClubId = 1 },
                        //    new Footballer { Id = 4, Name = "Piere-Emeric", Surname = "Aubameyang", ClubId = 1 }
                        //});

                        //context.Footballers.AddRange(new List<Footballer>
                        //{
                        //    new Footballer { Id = 5, Name = "Kepa", Surname = "Arrizabalaga", ClubId = 2 },
                        //    new Footballer { Id = 6, Name = "César", Surname = "Azpilicueta", ClubId = 2 },
                        //    new Footballer { Id = 7, Name = "Christian", Surname = "Pulisic", ClubId = 2 },
                        //    new Footballer { Id = 8, Name = "Tammy", Surname = "Abraham", ClubId = 2 }
                        //});
                    }
                }
            }
        }
    }
}