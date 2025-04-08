using RS1_2024_25.API.Helper.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS1_2024_25.API.Data.Models.SharedTables;

// drzava - Države
public class Country : SharedTableBase
{
    public string Name { get; set; } = string.Empty; // Naziv države
    public string IsoCode { get; set; } = string.Empty; // ISO kod države

    [InverseProperty(nameof(Region.Country))] // Veza ka Region.Country
    public List<Region> Regions { get; set; } = new();
}
