using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataGenerator.DB;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ganre> Ganres { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<MigrationHead> MigrationHeads { get; set; }

    public virtual DbSet<MigrationHistory> MigrationHistories { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MoviesImage> MoviesImages { get; set; }

    public virtual DbSet<MoviesStatistic> MoviesStatistics { get; set; }

    public virtual DbSet<MoviesView> MoviesViews { get; set; }

    public virtual DbSet<ObjectsStore> ObjectsStores { get; set; }

    public virtual DbSet<Top10MoviesView> Top10MoviesViews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("User Id=video_rent;Password=video_rent;Data Source=localhost:1521/XEPDB1;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("VIDEO_RENT")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Ganre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008226");

            entity.ToTable("GANRES");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008224");

            entity.ToTable("LANGUAGES");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<MigrationHead>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("MIGRATION_HEAD");

            entity.Property(e => e.Head)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("HEAD");
        });

        modelBuilder.Entity<MigrationHistory>(entity =>
        {
            entity.HasKey(e => e.Migration).HasName("SYS_C008222");

            entity.ToTable("MIGRATION_HISTORY");

            entity.Property(e => e.Migration)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("MIGRATION");
            entity.Property(e => e.EndDate)
                .HasPrecision(6)
                .HasColumnName("END_DATE");
            entity.Property(e => e.StartDate)
                .HasPrecision(6)
                .HasColumnName("START_DATE");
            entity.Property(e => e.Status)
                .HasColumnType("NUMBER(1)")
                .HasColumnName("STATUS");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008228");

            entity.ToTable("MOVIES");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSDATE")
                .HasColumnType("DATE")
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DELETED_BY");
            entity.Property(e => e.DeletedDate)
                .HasColumnType("DATE")
                .HasColumnName("DELETED_DATE");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Language)
                .HasColumnType("NUMBER")
                .HasColumnName("LANGUAGE");
            entity.Property(e => e.Photo)
                .HasColumnType("BLOB")
                .HasColumnName("PHOTO");
            entity.Property(e => e.ReleaseDate)
                .HasColumnType("DATE")
                .HasColumnName("RELEASE_DATE");
            entity.Property(e => e.RunTime)
                .HasColumnType("INTERVAL DAY(2) TO SECOND(6)")
                .HasColumnName("RUN_TIME");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TITLE");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("SYSDATE\r\n")
                .HasColumnType("DATE")
                .HasColumnName("UPDATED_AT");

            entity.HasOne(d => d.LanguageNavigation).WithMany(p => p.Movies)
                .HasForeignKey(d => d.Language)
                .HasConstraintName("SYS_C008229");

            entity.HasMany(d => d.Ganres).WithMany(p => p.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MoviesGanre",
                    r => r.HasOne<Ganre>().WithMany()
                        .HasForeignKey("GanreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C008231"),
                    l => l.HasOne<Movie>().WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C008230"),
                    j =>
                    {
                        j.HasKey("MovieId", "GanreId");
                        j.ToTable("MOVIES_GANRES");
                        j.IndexerProperty<decimal>("MovieId")
                            .HasColumnType("NUMBER")
                            .HasColumnName("MOVIE_ID");
                        j.IndexerProperty<decimal>("GanreId")
                            .HasColumnType("NUMBER")
                            .HasColumnName("GANRE_ID");
                    });
        });

        modelBuilder.Entity<MoviesImage>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("MOVIES_IMAGES");

            entity.Property(e => e.ImageGuid).HasColumnName("IMAGE_GUID");
            entity.Property(e => e.MovieId)
                .HasColumnType("NUMBER")
                .HasColumnName("MOVIE_ID");

            entity.HasOne(d => d.Movie).WithMany()
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("SYS_C008234");
        });

        modelBuilder.Entity<MoviesStatistic>(entity =>
        {
            entity.HasKey(e => e.MovieId).HasName("SYS_C008232");

            entity.ToTable("MOVIES_STATISTICS");

            entity.Property(e => e.MovieId)
                .HasColumnType("NUMBER")
                .HasColumnName("MOVIE_ID");
            entity.Property(e => e.ViewsCount)
                .HasColumnType("NUMBER")
                .HasColumnName("VIEWS_COUNT");

            entity.HasOne(d => d.Movie).WithOne(p => p.MoviesStatistic)
                .HasForeignKey<MoviesStatistic>(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008233");
        });

        modelBuilder.Entity<MoviesView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("MOVIES_VIEW");

            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Language)
                .HasColumnType("NUMBER")
                .HasColumnName("LANGUAGE");
            entity.Property(e => e.Photo)
                .HasColumnType("BLOB")
                .HasColumnName("PHOTO");
            entity.Property(e => e.ReleaseDate)
                .HasColumnType("DATE")
                .HasColumnName("RELEASE_DATE");
            entity.Property(e => e.RunTime)
                .HasColumnType("INTERVAL DAY(2) TO SECOND(6)")
                .HasColumnName("RUN_TIME");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<ObjectsStore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008235");

            entity.ToTable("OBJECTS_STORE");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("sys_guid() ")
                .HasColumnName("ID");
            entity.Property(e => e.Obj)
                .HasColumnType("BLOB")
                .HasColumnName("OBJ");
        });

        modelBuilder.Entity<Top10MoviesView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("TOP_10_MOVIES_VIEW");

            entity.Property(e => e.Id)
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Photo)
                .HasColumnType("BLOB")
                .HasColumnName("PHOTO");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TITLE");
            entity.Property(e => e.ViewsCount)
                .HasColumnType("NUMBER")
                .HasColumnName("VIEWS_COUNT");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
