using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS1_2024_25.API.Helper.BaseClasses;

namespace RS1_2024_25.API.Data.Models.SharedTables;

// regija - Regije
public class Region : SharedTableBase
{
    public string Name { get; set; } = string.Empty; // Naziv regije
    public int CountryId { get; set; } // FK na državu
    [ForeignKey(nameof(CountryId))]
    public Country? Country { get; set; } // Navigaciona veza na državu

    [InverseProperty(nameof(City.Region))]
    public List<City> Cities{ get; set; } = new();

}
