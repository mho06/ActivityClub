using Microsoft.EntityFrameworkCore;
using ActivityClub.Models;

namespace ActivityClub.Data
{
    public class BOS : DbContext
    {
        public BOS(DbContextOptions<BOS> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Guide> Guides { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<EventMembers> EventMembers { get; set; }
        public DbSet<EventGuides> EventGuides { get; set; }
        public DbSet<Lookup> Lookups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .Property(e => e.Cost)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<EventMembers>()
                .HasKey(em => new { em.EventID, em.MemberID });

            modelBuilder.Entity<EventGuides>()
                .HasKey(eg => new { eg.EventID, eg.GuideID });

            modelBuilder.Entity<EventMembers>()
                .HasOne(em => em.Event)
                .WithMany(e => e.EventMembers)
                .HasForeignKey(em => em.EventID);

            modelBuilder.Entity<EventMembers>()
                .HasOne(em => em.Member)
                .WithMany(u => u.EventMembers)
                .HasForeignKey(em => em.MemberID);

            modelBuilder.Entity<EventGuides>()
                .HasOne(eg => eg.Event)
                .WithMany(e => e.EventGuides)
                .HasForeignKey(eg => eg.EventID);

            modelBuilder.Entity<EventGuides>()
                .HasOne(eg => eg.Guide)
                .WithMany(g => g.EventGuides)
                .HasForeignKey(eg => eg.GuideID);

            modelBuilder.Entity<EventGuides>()
                .HasOne(eg => eg.User)
                .WithMany()
                .HasForeignKey(eg => eg.UserID);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.CategoryLookup)
                .WithMany()
                .HasForeignKey(e => e.CategoryLookupID);
        }

    }
}
