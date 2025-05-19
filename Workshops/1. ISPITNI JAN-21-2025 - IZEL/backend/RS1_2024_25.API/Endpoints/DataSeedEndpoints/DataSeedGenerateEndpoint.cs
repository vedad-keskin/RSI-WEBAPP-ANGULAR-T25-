namespace RS1_2024_25.API.Endpoints.DataSeedEndpoints;

using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Enums;
using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul1_Auth;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[Route("data-seed")]
public class DataSeedGenerateEndpoint(ApplicationDbContext db)
    : MyEndpointBaseAsync
    .WithoutRequest
    .WithResult<string>
{
    [HttpPost]
    public override async Task<string> HandleAsync(CancellationToken cancellationToken = default)
    {
        if (db.MyAppUsersAll.Any())
        {
            return "Podaci su vec generisani";
        }

        // Kreiraj faker za imena
        var faker = new Faker("en");

        #region COUNTRY_REGION_CITY
        // Kreiranje država
        var countries = new List<Country>
{
    // Bosna i Hercegovina
    new Country
    {
        Name = "Bosnia and Herzegovina",
        Regions =
        [
            new Region
            {
                Name = "Hercegovačko-neretvanski kanton (HNK)",
                Cities =
                [
                    new City
                    {
                        Name = "Mostar",
                        Municipalities = [ new Municipality { Name = "Mostar Stari Grad" }, new Municipality { Name = "Mostar Sjever" } ]
                    },
                    new City
                    {
                        Name = "Čapljina",
                        Municipalities = [ new Municipality { Name = "Čapljina Centar" } ]
                    },
                    new City
                    {
                        Name = "Konjic",
                        Municipalities = [ new Municipality { Name = "Konjic Stari Grad" } ]
                    }
                ]
            },
            new Region
            {
                Name = "Sarajevo Canton",
                Cities =
                [
                    new City
                    {
                        Name = "Sarajevo",
                        Municipalities = [ new Municipality { Name = "Stari Grad" }, new Municipality { Name = "Novi Grad" } ]
                    },
                    new City
                    {
                        Name = "Ilidža",
                        Municipalities = [ new Municipality { Name = "Ilidža Centar" } ]
                    },
                    new City
                    {
                        Name = "Vogošća",
                        Municipalities = [ new Municipality { Name = "Vogošća Stari Grad" } ]
                    }
                ]
            },
            new Region
            {
                Name = "Republika Srpska",
                Cities =
                [
                    new City
                    {
                        Name = "Banja Luka",
                        Municipalities = [ new Municipality { Name = "Banja Luka Centar" } ]
                    },
                    new City
                    {
                        Name = "Prijedor",
                        Municipalities = [ new Municipality { Name = "Prijedor Stari Grad" } ]
                    },
                    new City
                    {
                        Name = "Doboj",
                        Municipalities = [ new Municipality { Name = "Doboj Jug" } ]
                    }
                ]
            }
        ]
    },

    // Hrvatska
    new Country
    {
        Name = "Croatia",
        Regions =
        [
            new Region
            {
                Name = "Istria",
                Cities =
                [
                    new City
                    {
                        Name = "Pula",
                        Municipalities = [ new Municipality { Name = "Pula Centar" } ]
                    },
                    new City
                    {
                        Name = "Rovinj",
                        Municipalities = [ new Municipality { Name = "Rovinj Stari Grad" } ]
                    },
                    new City
                    {
                        Name = "Umag",
                        Municipalities = [ new Municipality { Name = "Umag Zapad" } ]
                    }
                ]
            },
            new Region
            {
                Name = "Dalmatia",
                Cities =
                [
                    new City
                    {
                        Name = "Split",
                        Municipalities = [ new Municipality { Name = "Split Centar" }, new Municipality { Name = "Split Zapad" } ]
                    },
                    new City
                    {
                        Name = "Zadar",
                        Municipalities = [ new Municipality { Name = "Zadar Centar" } ]
                    },
                    new City
                    {
                        Name = "Dubrovnik",
                        Municipalities = [ new Municipality { Name = "Dubrovnik Stari Grad" } ]
                    }
                ]
            },
            new Region
            {
                Name = "Zagreb County",
                Cities =
                [
                    new City
                    {
                        Name = "Zagreb",
                        Municipalities = [ new Municipality { Name = "Zagreb Novi Grad" }, new Municipality { Name = "Zagreb Stari Grad" } ]
                    },
                    new City
                    {
                        Name = "Velika Gorica",
                        Municipalities = [ new Municipality { Name = "Velika Gorica Centar" } ]
                    },
                    new City
                    {
                        Name = "Samobor",
                        Municipalities = [ new Municipality { Name = "Samobor Centar" } ]
                    }
                ]
            }
        ]
    },

    // Njemačka
    new Country
    {
        Name = "Germany",
        Regions =
        [
            new Region
            {
                Name = "Bavaria",
                Cities =
                [
                    new City
                    {
                        Name = "Munich",
                        Municipalities = [ new Municipality { Name = "Munich Central" } ]
                    },
                    new City
                    {
                        Name = "Nuremberg",
                        Municipalities = [ new Municipality { Name = "Nuremberg Central" } ]
                    },
                    new City
                    {
                        Name = "Augsburg",
                        Municipalities = [ new Municipality { Name = "Augsburg Central" } ]
                    }
                ]
            },
            new Region
            {
                Name = "Baden-Württemberg",
                Cities =
                [
                    new City
                    {
                        Name = "Stuttgart",
                        Municipalities = [ new Municipality { Name = "Stuttgart Mitte" } ]
                    },
                    new City
                    {
                        Name = "Heidelberg",
                        Municipalities = [ new Municipality { Name = "Heidelberg Altstadt" } ]
                    },
                    new City
                    {
                        Name = "Karlsruhe",
                        Municipalities = [ new Municipality { Name = "Karlsruhe Süd" } ]
                    }
                ]
            },
            new Region
            {
                Name = "North Rhine-Westphalia",
                Cities =
                [
                    new City
                    {
                        Name = "Cologne",
                        Municipalities = [ new Municipality { Name = "Cologne Zentrum" } ]
                    },
                    new City
                    {
                        Name = "Düsseldorf",
                        Municipalities = [ new Municipality { Name = "Düsseldorf Altstadt" } ]
                    },
                    new City
                    {
                        Name = "Dortmund",
                        Municipalities = [ new Municipality { Name = "Dortmund Nord" } ]
                    }
                ]
            }
        ]
    }
};

        #endregion

        // Kreiranje tenant-a (univerziteta)
        var tenants = new List<Tenant>
        {
            new Tenant { Name = "University Džemal Bijedić, Mostar", DatabaseConnection = "db_conn_mostar", ServerAddress = "192.168.1.10" },
            new Tenant { Name = "University of Sarajevo", DatabaseConnection = "db_conn_sarajevo", ServerAddress = "192.168.1.11" },
        };

        await db.Countries.AddRangeAsync(countries, cancellationToken);
        await db.Tenants.AddRangeAsync(tenants, cancellationToken);

        await db.SaveChangesAsync(cancellationToken);

        db.CurrentTenantId = tenants[0].ID;


        // Kreiranje fakulteta
        var faculties = new List<Faculty>
        {
            new Faculty { Name = "Faculty of Computer Science" },
            new Faculty { Name = "Faculty of Mathematics" }
        };

        foreach (var x in faculties)
        {
            x.Tenant = tenants[0];
        }

        // Kreiranje akademskih godina
        var academicYears = new List<AcademicYear>
        {
            new AcademicYear { StartDate = new DateOnly(2024, 9, 1), EndDate = new DateOnly(2025, 6, 30), Description = "Academic Year 2024/2025" },
            new AcademicYear { StartDate = new DateOnly(2023, 9, 1), EndDate = new DateOnly(2024, 6, 30), Description = "Academic Year 2023/2024" }
        };

        // Kreiranje korisnika s ulogama
        var users = new List<MyAppUser>
        {
            new MyAppUser
            {
                Email = "admin",
                FirstName = "Admin",
                LastName = "One",
                IsAdmin = true,
                IsDean = false
            },
            new MyAppUser
            {
                Email = "manager",
                FirstName = "Manager",
                LastName = "One",
                IsAdmin = false,
                IsDean = true
            },
            new MyAppUser
            {
                Email = "user1",
                FirstName = "User",
                LastName = "One",
                IsAdmin = false,
                IsDean = false
            },
            new MyAppUser
            {
                Email = "user2",
                FirstName = "User",
                LastName = "Two",
                IsAdmin = false,
                IsDean = false
            }
        };

        foreach (var x in users)
        {
            x.SetPassword("test");
            x.Tenant = tenants[0];
        }

        // Kreiranje profesora
        var professors = new List<Professor>
        {
            new Professor
            {
                User = users[0], // Povezano s adminom
                Title = "Dr.",
                Department = "Računarstvo",
                HireDate = DateTime.Now.AddYears(-10),
                Biography = "Iskusan profesor računarstva."
            },
            new Professor
            {
                User = users[1], // Povezano s managerom
                Title = "Prof.",
                Department = "Matematika",
                HireDate = DateTime.Now.AddYears(-15),
                Biography = "Ekspert za matematiku."
            }
        };

        foreach (var x in professors)
        {
            x.Tenant = tenants[0];
        }

        List<Student> students = new List<Student>();
        // Kreiranje studenata
        for (int i = 0; i < 100; i++)
        {
            // Generiši nasumično ime i prezime
            string firstName = faker.Name.FirstName();
            string lastName = faker.Name.LastName();

            var s = new Student
            {
                User = new MyAppUser
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = (firstName + "." + lastName).ToLower() + "@edu.fit.ba",
                    Tenant = tenants[0]
                },
                ParentName = faker.Name.FirstName(),
                BirthDate = new DateOnly(2000, 5, 15),
                Gender = Gender.Male,
                Citizenship = countries[0],
                BirthPlace = "Sarajevo",
                StudentNumber = "IB200"+ i.ToString("D3"),
                ContactMobilePhone = faker.Phone.PhoneNumber(),
                ContactPrivateEmail = faker.Internet.Email(firstName, lastName)
            };
            s.User.SetPassword("test");
            students.Add(s);
        }

        Municipality defaultOpstina = countries[0].Regions[0].Cities[0].Municipalities[0];

        foreach (var x in students)
        {
            x.Tenant = tenants[0];
            x.BirthMunicipality = defaultOpstina;
        }

        // Dodavanje podataka u bazu    
        await db.AddRangeAsync(faculties, cancellationToken);
        await db.AddRangeAsync(academicYears, cancellationToken);
        await db.AddRangeAsync(users, cancellationToken);
        await db.AddRangeAsync(professors, cancellationToken);
        await db.AddRangeAsync(students, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return "Data generation completed successfully.";
    }
}
