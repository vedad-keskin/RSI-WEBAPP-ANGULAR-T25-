using RS1_2024_25.API.Helper.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS1_2024_25.API.Data.Models.SharedTables;

// tenants - Lista tenant-a
// univerziteti
public class Tenant : SharedTableBase
{
    public string Name { get; set; } = string.Empty; // Naziv tenant-a
    public string DatabaseConnection { get; set; } = string.Empty; // Konekcija na bazu podataka
    public string ServerAddress { get; set; } = string.Empty; // Adresa tenant-a
}
