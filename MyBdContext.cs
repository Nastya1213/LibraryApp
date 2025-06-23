using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp;

public partial class MyBdContext : DbContext
{
    public MyBdContext()
    {
    }

    public MyBdContext(DbContextOptions<MyBdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Книга> Книгаs { get; set; }

    public virtual DbSet<Пользователь> Пользовательs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-9146J70;Database=MyBd;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Книга>(entity =>
        {
            entity.HasKey(e => e.Артикул);

            entity.ToTable("Книга");

            entity.Property(e => e.Артикул)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Жанр)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Название)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Описание)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Статус)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Читатель)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.ЧитательNavigation).WithMany(p => p.Книгаs)
                .HasForeignKey(d => d.Читатель)
                .HasConstraintName("FK_Книга_Пользователь");
        });

        modelBuilder.Entity<Пользователь>(entity =>
        {
            entity.HasKey(e => e.Логин);

            entity.ToTable("Пользователь");

            entity.Property(e => e.Логин)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.НомерТелефона)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Пароль)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Фио)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("ФИО");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
