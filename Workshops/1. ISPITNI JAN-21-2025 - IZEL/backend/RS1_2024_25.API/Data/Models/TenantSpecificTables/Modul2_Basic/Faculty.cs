using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Helper.BaseClasses;

namespace RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul2_Basic;

public class Faculty : TenantSpecificTable
{
    [StringLength(100)]
    public string Name { get; set; } = null!;
    public virtual List<Department> Departments { get; set; } = new List<Department>();

}
