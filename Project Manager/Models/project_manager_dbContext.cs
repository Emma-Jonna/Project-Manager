﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project_Manager.Models;

public partial class project_manager_dbContext : DbContext
{
    public project_manager_dbContext()
    {
    }

    public project_manager_dbContext(DbContextOptions<project_manager_dbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Category { get; set; }

    public virtual DbSet<Material> Material { get; set; }

    public virtual DbSet<Project> Project { get; set; }

    public virtual DbSet<Status> Status { get; set; }

    public virtual DbSet<Type> Type { get; set; }

    public virtual DbSet<User> User { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=project_manager_db;Integrated Security=True; Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__category__3213E83F5689E9E5");

            entity.ToTable("category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__material__3213E83F93F4E82E");

            entity.ToTable("material");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Acquired).HasColumnName("acquired");
            entity.Property(e => e.Amount)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("amount");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");

            entity.HasOne(d => d.Project).WithMany(p => p.Material)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__material__projec__44FF419A");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__project__3213E83FE43B34E1");

            entity.ToTable("project");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AfterImage)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("after_image");
            entity.Property(e => e.BeforeImage)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("before_image");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("end_date");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PatternLink)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("pattern_link");
            entity.Property(e => e.Sketch)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("sketch");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("start_date");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Project)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__project__categor__412EB0B6");

            entity.HasOne(d => d.Status).WithMany(p => p.Project)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__project__status___403A8C7D");

            entity.HasOne(d => d.Type).WithMany(p => p.Project)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK__project__type_id__4222D4EF");

            entity.HasOne(d => d.User).WithMany(p => p.Project)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__project__user_id__3F466844");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__status__3213E83FF9AFB53D");

            entity.ToTable("status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__type__3213E83F376561BE");

            entity.ToTable("type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user__3213E83FF4B750D4");

            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}