using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProductPersistenceServices.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TableProduct> TableProducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("User Id=PHUONG;Password=123456;Data Source=localhost:1521/mypdb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("PHUONG")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<TableProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008448");

            entity.ToTable("TABLE_PRODUCT");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("PRICE");
            entity.Property(e => e.Quantity)
                .HasColumnType("NUMBER")
                .HasColumnName("QUANTITY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
