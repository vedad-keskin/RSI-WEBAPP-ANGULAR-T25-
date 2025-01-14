using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Helper;
using RS1_2024_25.API.Helper.BaseClasses;

namespace RS1_2024_25.API.Data.Models.SharedTables;

public partial class AcademicYear : SharedTableBase
{

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Description { get; set; } = string.Empty;

}
