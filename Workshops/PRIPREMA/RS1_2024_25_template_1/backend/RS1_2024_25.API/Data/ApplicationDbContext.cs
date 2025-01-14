using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul1_Auth;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul2_Basic;
using RS1_2024_25.API.Helper;
using RS1_2024_25.API.Helper.BaseClasses;
using RS1_2024_25.API.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace RS1_2024_25.API.Data;

public class ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : DbContext(options)
{
   
    public DbSet<AcademicYear> AcademicYears { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Municipality> Municipalities { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Tenant> Tenants { get; set; }

    public DbSet<MyAppUser> MyAppUsersAll { get; set; }
    public DbSet<MyAuthenticationToken> MyAuthenticationTokensAll { get; set; }
    public DbSet<Department> DepartmentsAll { get; set; }
    public DbSet<Faculty> FacultiesAll { get; set; }
    public DbSet<Professor> ProfessorsAll { get; set; }
    public DbSet<Student> StudentsAll { get; set; }

    // IQueryable umjesto DbSet
    public IQueryable<MyAppUser> MyAppUsers => Set<MyAppUser>().Where(e => e.TenantId == CurrentTenantIdThrowIfFail);
    public IQueryable<MyAuthenticationToken> MyAuthenticationTokens => Set<MyAuthenticationToken>();
    public IQueryable<Department> Departments => Set<Department>().Where(e => e.TenantId == CurrentTenantIdThrowIfFail);
    public IQueryable<Faculty> Faculties => Set<Faculty>().Where(e => e.TenantId == CurrentTenantIdThrowIfFail);
    public IQueryable<Professor> Professors => Set<Professor>().Where(e => e.TenantId == CurrentTenantIdThrowIfFail);
    public IQueryable<Student> Students => Set<Student>().Where(e => e.TenantId == CurrentTenantIdThrowIfFail);

    #region METHODS
    public int? _CurrentTenantId = null;

    public int CurrentTenantIdThrowIfFail
    {
        get
        {
            var result = CurrentTenantId;
            if (result == null || result == 0)
            {
                throw new UnauthorizedAccessException();
            }

            return result.Value;
        }
    }
    public int? CurrentTenantId
    {
        get
        {
            if (_CurrentTenantId == null)
            {
                MyAuthInfo myAuthInfo = MyAuthServiceHelper.GetAuthInfoFromRequest(this, httpContextAccessor);
                _CurrentTenantId = myAuthInfo.TenantId;
            }
            return _CurrentTenantId;
        }
        set
        {
            _CurrentTenantId = value;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.NoAction;
        }

        // opcija kod nasljeđivanja
        // modelBuilder.Entity<NekaBaznaKlasa>().UseTpcMappingStrategy();

        // Iteracija kroz sve entitete u modelu
        // U EF-u defaultno naziv tabele je jednak nazivu dbseta.
        // S obzirom što smo izmjenili nazive dbsetova zbog tenanata i zbog dodatnih queryable
        // onda u narednoj petlji postavljamo da nazivi tabela budu nazivi atributa "table"
        // Ako nema atributa "table" onda se koristi naziv klase.

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            // Provjera da li postoji [Table("TblNekoIme")] atribut
            var tableAttribute = clrType.GetCustomAttributes(typeof(TableAttribute), inherit: false)
                                        .FirstOrDefault() as TableAttribute;

            if (tableAttribute == null)
            {
                // Ako nema TableAttribute, automatski pluralizuj naziv tabele
                var tableName = clrType.Name.Pluralize();
                modelBuilder.Entity(clrType).ToTable(tableName);
            }
            else
            {
                // Ako postoji TableAttribute, koristi navedeni naziv tabele
                modelBuilder.Entity(clrType).ToTable(tableAttribute.Name);
            }
        }
    }
 
    public override int SaveChanges()
    {
        AddTenantIdToNewEntities();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddTenantIdToNewEntities();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void AddTenantIdToNewEntities()
    {
        // Iteracija kroz sve promjene u DbContext
        foreach (var entry in ChangeTracker.Entries()
                     .Where(e => e.State == EntityState.Added && e.Entity is TenantSpecificTable))
        {
            // Postavljanje TenantId za nove entitete
            var entity = (TenantSpecificTable)entry.Entity;
            entity.TenantId = CurrentTenantIdThrowIfFail;
        }
    }
    #endregion
}
