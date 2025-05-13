using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul1_Auth;
using RS1_2024_25.API.Helper.BaseClasses;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models.SharedTables;

public class MyAuthenticationToken : SharedTableBase
{
    public required string Value { get; set; } // Token string

    public string IpAddress { get; set; } = string.Empty;// IP address of the client

    public DateTime RecordedAt { get; set; } // Timestamp of token creation

    // Foreign key to link the token to a specific user
    [ForeignKey(nameof(MyAppUser))]
    public int MyAppUserId { get; set; }

    public MyAppUser? MyAppUser { get; set; } // Navigation property to the user
    public int TenantId { get; set; }
    [ForeignKey(nameof(TenantId))]
    public Tenant? Tenant { get; set; }
}
