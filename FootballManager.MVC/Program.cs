using FootballManager.Data;
using FootballManager.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

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

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = scope.ServiceProvider.GetService<FootballManagerContext>();
            context.Database.Migrate();

            var a = context.Clubs.Any();

            if (!context.Clubs.Any())
            {
                context.Clubs.Add(new Club
                {
                    ClubName = "Arsenal",
                    ClubImageUrl = "https://resources.premierleague.com/premierleague/badges/t3.svg",
                    Founded = new DateTime(1886, 1, 1).Date,
                    City = "London"
                });

                context.Clubs.Add(new Club
                {
                    ClubName = "Chelsea",
                    ClubImageUrl = "https://resources.premierleague.com/premierleague/badges/t8.svg",
                    Founded = new DateTime(1905, 3, 10).Date,
                    City = "London"
                });

                context.Clubs.Add(new Club
                {
                    ClubName = "Liverpool",
                    ClubImageUrl = "https://resources.premierleague.com/premierleague/badges/t14.svg",
                    Founded = new DateTime(1892, 3, 15).Date,
                    City = "Liverpool"
                });

                context.SaveChanges();

                var ArsenalId = context.Clubs.FirstOrDefault(c => c.ClubName == "Arsenal").Id;
                var ChelseaId = context.Clubs.FirstOrDefault(c => c.ClubName == "Chelsea").Id;
                var LiverpoolId = context.Clubs.FirstOrDefault(c => c.ClubName == "Liverpool").Id;

                context.Stadiums.Add(new Stadium
                {
                    StadiumName = "Emirates Stadium",
                    StadiumImageUrl = "https://resources.premierleague.com/premierleague/photo/2016/07/21/ccade424-00e6-4310-a183-48f7101b1f5e/Arsenal_Stadium_Emirates.jpeg",
                    Capacity = 60272,
                    ClubId = ArsenalId
                });

                context.Stadiums.Add(new Stadium
                {
                    StadiumName = "Stamford Bridge",
                    StadiumImageUrl = "https://resources.premierleague.com/premierleague/photo/2016/09/22/c9c78d39-60fb-47c6-9e20-837efb242ac5/Stamford_Bridge.jpg",
                    Capacity = 41798,
                    ClubId = ChelseaId
                });

                context.Stadiums.Add(new Stadium
                {
                    StadiumName = "Anfield",
                    StadiumImageUrl = "https://resources.premierleague.com/premierleague/photo/2016/09/22/01c2bc32-7a73-41a5-81fb-a86d4fe9ef5c/Anfield.jpg",
                    Capacity = 54074,
                    ClubId = LiverpoolId
                });

                context.Coaches.Add(new Coach
                {
                    Name = "Mikel",
                    Surname = "Arteta",
                    ClubId = ArsenalId
                });

                context.Coaches.Add(new Coach
                {
                    Name = "Frank",
                    Surname = "Lampard",
                    ClubId = ChelseaId
                });

                context.Coaches.Add(new Coach
                {
                    Name = "Jürgen",
                    Surname = "Klopp",
                    ClubId = LiverpoolId
                });

                context.Footballers.AddRange(new List<Footballer>
            {
                new Footballer { Name = "Bernd", Surname = "Leno", ClubId = ArsenalId },
                new Footballer { Name = "Kieran", Surname = "Tierney", ClubId = ArsenalId },
                new Footballer { Name = "Nicolas", Surname = "Pépé", ClubId = ArsenalId },
                new Footballer { Name = "Piere-Emeric", Surname = "Aubameyang", ClubId = ArsenalId }
            });

                context.Footballers.AddRange(new List<Footballer>
            {
                new Footballer { Name = "Kepa", Surname = "Arrizabalaga", ClubId = ChelseaId },
                new Footballer { Name = "César", Surname = "Azpilicueta", ClubId = ChelseaId },
                new Footballer { Name = "Christian", Surname = "Pulisic", ClubId = ChelseaId },
                new Footballer { Name = "Tammy", Surname = "Abraham", ClubId = ChelseaId }
            });

                context.Footballers.AddRange(new List<Footballer>
            {
                new Footballer { Name = "Alisson", Surname = "", ClubId = LiverpoolId },
                new Footballer { Name = "Virgil", Surname = "van Dijk", ClubId = LiverpoolId },
                new Footballer { Name = "Jordan", Surname = "Henderson", ClubId = LiverpoolId },
                new Footballer { Name = "Sadio", Surname = "Mané", ClubId = LiverpoolId }
            });

                context.SaveChanges();

            }
        }
    }
}