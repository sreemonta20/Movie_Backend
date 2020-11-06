using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Movie.BackEnd.Common.Utilities;
using Movie.BackEnd.Persistence.DBModel;

namespace Movie.BackEnd.Persistence.DBContext
{
    public partial class MovieDBContext : DbContext
    {
        public MovieDBContext()
        {
        }

        public MovieDBContext(DbContextOptions<MovieDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LoginUser> LoginUser { get; set; }
        public virtual DbSet<MovieBooking> MovieBooking { get; set; }
        public virtual DbSet<MovieDetails> MovieDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(ConnectionParameter.MainConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginUser>(entity =>
            {
                entity.HasKey(e => e.UserCode);

                entity.Property(e => e.UserCode).ValueGeneratedNever();

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword).IsUnicode(false);
            });

            modelBuilder.Entity<MovieBooking>(entity =>
            {
                //entity.HasNoKey();
                entity.HasKey(e => e.BookCode);
                entity.Property(e => e.UserCode).HasColumnType("int");
                entity.Property(e => e.ImdbCode).HasColumnType("int");
            });

            modelBuilder.Entity<MovieDetails>(entity =>
            {
                entity.HasKey(e => e.ImdbCode);

                entity.Property(e => e.ImdbCode).ValueGeneratedNever();

                entity.Property(e => e.ImdbId)
                    .HasColumnName("ImdbID")
                    .HasMaxLength(100);

                entity.Property(e => e.ImdbRating).HasMaxLength(100);

                entity.Property(e => e.Language).HasMaxLength(100);

                entity.Property(e => e.ListingType).HasMaxLength(100);

                entity.Property(e => e.Location).HasMaxLength(100);

                entity.Property(e => e.Poster).HasMaxLength(250);

                entity.Property(e => e.SoundEffects).HasMaxLength(100);

                entity.Property(e => e.Stills).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
