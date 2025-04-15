using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data.Enums;
using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul1_Auth;
using RS1_2024_25.API.Helper;
using RS1_2024_25.API.Helper.BaseClasses;

namespace RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul2_Basic;

public class Student : TenantSpecificTable
{
    #region MATICNI_PODACI
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public MyAppUser User { get; set; }
    public string? ParentName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public Gender Gender { get; set; }
    public int? CitizenshipId { get; set; }
    [ForeignKey(nameof(CitizenshipId))]
    public Country? Citizenship { get; set; }//državljanstvo
    public string? BirthPlace { get; set; } //mjesto rodjenja
    public int? BirthMunicipalityId { get; set; } //opština rodjenja
    [ForeignKey(nameof(BirthMunicipalityId))]
    public Municipality? BirthMunicipality { get; set; } //opština rodjenja
    #endregion

    #region CIPS_INFO
    public string? PermanentAddressStreet { get; set; }
    public int? PermanentMunicipalityId { get; set; }
    [ForeignKey(nameof(PermanentMunicipalityId))]
    public Municipality? PermanentMunicipality { get; set; }
    #endregion

    public string StudentNumber { get; set; } = string.Empty;
    public string? ContactMobilePhone { get; set; }
    public string? ContactPrivateEmail { get; set; }
    public bool IsDeleted { get; internal set; }
}
