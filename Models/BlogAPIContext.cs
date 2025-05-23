using BlogAPIWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAPIWebApp.Models
{
    public class BlogAPIContext : DbContext
    {
        public virtual DbSet<BlogUser> BlogUsers { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }

        public virtual DbSet<Like> Likes { get; set; }

        public virtual DbSet<Post> Posts { get; set; }

        public virtual DbSet<PostTag> PostTags { get; set; }

        public virtual DbSet<Subscription> Subscriptions { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }

        public BlogAPIContext(DbContextOptions<BlogAPIContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var fk in entityType.GetForeignKeys())
                {
                    fk.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }


            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Reader)
                .WithMany(u => u.SubscriptionsAsReader)
                .HasForeignKey(s => s.ReaderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Author)
                .WithMany(u => u.SubscriptionsAsAuthor)
                .HasForeignKey(s => s.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
