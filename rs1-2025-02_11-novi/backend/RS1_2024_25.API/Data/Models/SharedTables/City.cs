using RS1_2024_25.API.Helper;
using RS1_2024_25.API.Helper.BaseClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models.SharedTables;

public class City : SharedTableBase
{
    public string Name { get; set; }

    public int RegionId { get; set; } // FK na regiju
    [ForeignKey(nameof(RegionId))]
    public Region? Region { get; set; } // Navigaciona veza na regiju

    [InverseProperty(nameof(Municipality.City))]
    public List<Municipality> Municipalities { get; set; } = new();
}