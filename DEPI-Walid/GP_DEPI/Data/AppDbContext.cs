using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CustomIdentity.Models; // Assuming your models are in this namespace

namespace CustomIdentity.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet properties for your entities
        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }
        public DbSet<Penalty> Penalties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base method to apply Identity configurations
            base.OnModelCreating(modelBuilder);

            // Configuring the Penalty entity
            modelBuilder.Entity<Penalty>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18, 2)"); // Specify the column type with precision and scale

            // Configuring Checkout relationships
            modelBuilder.Entity<Checkout>()
                .HasOne(c => c.Book)
                .WithMany(b => b.Checkouts)
                .HasForeignKey(c => c.BookId);

            modelBuilder.Entity<Checkout>()
                .HasOne(c => c.Member)
                .WithMany(m => m.Checkouts)
                .HasForeignKey(c => c.MemberId);

            // Configuring Penalty relationships
            modelBuilder.Entity<Penalty>()
                .HasOne(p => p.Checkout)
                .WithMany(c => c.Penalties)
                .HasForeignKey(p => p.CheckoutId);
        }
    }
}
