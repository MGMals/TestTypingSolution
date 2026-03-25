using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TestTypingApi.Models.DB;

namespace TestTypingApi.Models.Context;

public partial class TestTypingSpeedsDbContext : DbContext
{
    public TestTypingSpeedsDbContext()
    {
    }

    public TestTypingSpeedsDbContext(DbContextOptions<TestTypingSpeedsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TestTypeUser> TestTypeUsers { get; set; }

    public virtual DbSet<TypingTest> TypingTests { get; set; }

    public virtual DbSet<UserSession> UserSessions { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=Danga_rgb_ps\\RGB_PS_MSSQLSERV;Initial Catalog=TestTypingSpeedsDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestTypeUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestType__3214EC07624F2E0A");

            entity.ToTable("TestTypeUser");

            entity.HasIndex(e => e.Email, "IX_TestTypeUsers_Email").IsUnique();

            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
        });

        modelBuilder.Entity<TypingTest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TypingTe__3214EC07D3E455C8");

            entity.Property(e => e.Accuracy).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Difficulty).HasMaxLength(50);
            entity.Property(e => e.TestDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Wpm).HasColumnName("WPM");

            entity.HasOne(d => d.User).WithMany(p => p.TypingTests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TypingTests_Users");
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserSess__3214EC07580225DE");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Token).HasMaxLength(500);

            entity.HasOne(d => d.User).WithMany(p => p.UserSessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserSessi__UserI__4222D4EF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
