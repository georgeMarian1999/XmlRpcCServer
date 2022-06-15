using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using WebService.model;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace WebService.modelContext;

public partial class ModelContext: DbContext
{
    
    public virtual DbSet Author { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=ec2-52-200-215-149.compute-1.amazonaws.com;Username=nbwavzyshzkuiy;Password=5ce75fe8818ecb2e397d77d0749ef05d630b38c732e7cbfb6eeaf7eba775a90f;Database=ds75uhlqkbe92");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity
                .HasKey(e => e.AuthorId)
                .HasName("");

            entity.ToTable("Author", "public");

            entity.Property(e => e.AuthorId).HasColumnName("authorId");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Age).HasColumnName("age");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity
                .HasKey(e => e.BookId)
                .HasName("");

            entity.ToTable("Book", "public");

            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Year).HasColumnName("year");
            entity.Property(e => e.AuthorId).HasColumnName("authorId");


        });
    }
}