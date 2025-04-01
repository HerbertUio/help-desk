using Infrastructure.Database.EntityFramework.Entities;
using Infrastructure.Database.EntityFramework.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.EntityFramework.Context;

public class HelpDeskDbContext: DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    
    public HelpDeskDbContext(DbContextOptions<HelpDeskDbContext> options): base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure the UserEntity
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.ToTable("Users", "CTR");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(50)").IsRequired();
            entity.Property(e => e.LastName).HasColumnName("last_name").HasColumnType("varchar(50)").IsRequired();
            entity.Property(e => e.PhoneNumber).HasColumnName("phone_number").HasColumnType("varchar(20)").IsRequired();
            entity.Property(e => e.Email).HasColumnName("email").HasColumnType("varchar(100)").IsRequired();
            entity.Property(e => e.Password).HasColumnName("password").HasColumnType("varchar(500)").IsRequired();
            entity.Property(e => e.DepartmentId).HasColumnName("department_id").IsRequired();
            entity.Property(e => e.Role).HasColumnName("role").HasColumnType("varchar(50)").IsRequired();
            entity.Property(e => e.Active).HasColumnName("active").HasColumnType("boolean").IsRequired();
        });
    }
    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }
    
    private void UpdateAuditFields()
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = GetCurrentUserId();
                    entry.Entity.LastModifiedByAt = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy= GetCurrentUserId();
                    break;

                case EntityState.Modified:
                    entry.Property(nameof(BaseEntity.CreatedAt)).IsModified = false;
                    entry.Property(nameof(BaseEntity.CreatedBy)).IsModified = false;
                    entry.Entity.LastModifiedByAt = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = 104;
                    break;
            }
        }
    }
    
    private int GetCurrentUserId()
    {
        return 123;
    }
}