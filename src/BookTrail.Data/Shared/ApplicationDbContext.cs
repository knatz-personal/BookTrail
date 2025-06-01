using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookTrail.Data.Shared
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Genre> Genre { get; set; }
        public DbSet<BookGenre> BookGenre { get; set; }

        public DbSet<Author> Author { get; set; }
        public DbSet<BookAuthor> BookAuthor { get; set; }

        public DbSet<Review> Reviews { get; set; }
        public DbSet<Recommendation> Recomendation { get; set; }
        public DbSet<ReadLog> ReadLog { get; set; }
        public DbSet<BookShelf> BookShelf { get; set; }
        public DbSet<BookShelfItem> BookShelfItem { get; set; }
        public DbSet<Recommendation> Recommendation { get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity schema
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole<Guid>>().ToTable("Roles");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

            // Apply entity configurations from assembly
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
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
            DateTime now = DateTime.UtcNow;

            foreach (EntityEntry entry in ChangeTracker.Entries())
            {
                // Update CreatedAt for added entities that have such property
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity.GetType().GetProperty("CreatedAt") != null)
                    {
                        entry.Property("CreatedAt").CurrentValue = now;
                    }
                }

                // Update UpdatedAt for modified entities that have such property
                if (entry.State == EntityState.Modified)
                {
                    if (entry.Entity.GetType().GetProperty("UpdatedAt") != null)
                    {
                        entry.Property("UpdatedAt").CurrentValue = now;
                    }
                }
            }
        }
    }
}