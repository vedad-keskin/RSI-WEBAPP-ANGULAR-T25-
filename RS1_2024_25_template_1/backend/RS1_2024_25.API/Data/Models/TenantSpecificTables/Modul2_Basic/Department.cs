using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Helper;
using RS1_2024_25.API.Helper.BaseClasses;

namespace RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul2_Basic;

public class Department : TenantSpecificTable
{
    [StringLength(50)]
    public string Name { get; set; } = null!;

    public int FacultyId { get; set; }
    [ForeignKey(nameof(FacultyId))]
    public virtual Faculty Faculty { get; set; } = null!;
}
