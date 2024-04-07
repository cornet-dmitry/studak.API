using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using studak.spbrtk.API.Models;

namespace studak.spbrtk.API.Context;

public partial class IcawqbetContext : DbContext
{
    public IcawqbetContext()
    {
    }

    public IcawqbetContext(DbContextOptions<IcawqbetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Direction> Directions { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Involvement> Involvements { get; set; }

    public virtual DbSet<Involvementstatus> Involvementstatuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserStatus> UserStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=snuffleupagus.db.elephantsql.com;Database=icawqbet;Username=icawqbet;password=ELYj1GJkFJsrPBTU9zmJmstWMSFqV8Tz");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("btree_gin")
            .HasPostgresExtension("btree_gist")
            .HasPostgresExtension("citext")
            .HasPostgresExtension("cube")
            .HasPostgresExtension("dblink")
            .HasPostgresExtension("dict_int")
            .HasPostgresExtension("dict_xsyn")
            .HasPostgresExtension("earthdistance")
            .HasPostgresExtension("fuzzystrmatch")
            .HasPostgresExtension("hstore")
            .HasPostgresExtension("intarray")
            .HasPostgresExtension("ltree")
            .HasPostgresExtension("pg_stat_statements")
            .HasPostgresExtension("pg_trgm")
            .HasPostgresExtension("pgcrypto")
            .HasPostgresExtension("pgrowlocks")
            .HasPostgresExtension("pgstattuple")
            .HasPostgresExtension("tablefunc")
            .HasPostgresExtension("unaccent")
            .HasPostgresExtension("uuid-ossp")
            .HasPostgresExtension("xml2");

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("admin_pkey");

            entity.ToTable("admin");

            entity.Property(e => e.Userid)
                .ValueGeneratedNever()
                .HasColumnName("userid");
            entity.Property(e => e.Userlogin)
                .HasMaxLength(128)
                .HasColumnName("userlogin");
            entity.Property(e => e.Userpasswordhash).HasColumnName("userpasswordhash");
            entity.Property(e => e.Userpasswordsalt).HasColumnName("userpasswordsalt");

            entity.HasOne(d => d.User).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("admin_userid_fkey");
        });

        modelBuilder.Entity<Direction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("directions_pkey");

            entity.ToTable("directions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DirectionLongName).HasColumnName("direction_long_name");
            entity.Property(e => e.DirectionShortName).HasColumnName("direction_short_name");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("events_pkey");

            entity.ToTable("events");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Direction).HasColumnName("direction");
            entity.Property(e => e.EndDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("end_date");
            entity.Property(e => e.EndTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("end_time");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.Responsible).HasColumnName("responsible");
            entity.Property(e => e.StartDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_date");
            entity.Property(e => e.StartTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_time");

            entity.HasOne(d => d.DirectionNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.Direction)
                .HasConstraintName("events_direction_fkey");

            entity.HasOne(d => d.ResponsibleNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.Responsible)
                .HasConstraintName("events_responsible_fkey");
        });

        modelBuilder.Entity<Involvement>(entity =>
        {
            entity.HasKey(e => new { e.Eventid, e.Userid }).HasName("involvement_pkey");

            entity.ToTable("involvement");

            entity.Property(e => e.Eventid).HasColumnName("eventid");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Createtime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createtime");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Event).WithMany(p => p.Involvements)
                .HasForeignKey(d => d.Eventid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("involvement_eventid_fkey");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Involvements)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("involvement_status_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Involvements)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("involvement_userid_fkey");
        });

        modelBuilder.Entity<Involvementstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("involvementstatus_pkey");

            entity.ToTable("involvementstatus");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.InvolvementName)
                .HasColumnType("character varying")
                .HasColumnName("involvement_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateBirth)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_birth");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Group).HasColumnName("group");
            entity.Property(e => e.Kpi).HasColumnName("kpi");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.OrderNumber).HasColumnName("order_number");
            entity.Property(e => e.Patronymic).HasColumnName("patronymic");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.StartDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_date");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Surname).HasColumnName("surname");
            entity.Property(e => e.TgLink).HasColumnName("tg_link");
            entity.Property(e => e.VkLink).HasColumnName("vk_link");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Status)
                .HasConstraintName("users_fk");
        });

        modelBuilder.Entity<UserStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_status_pkey");

            entity.ToTable("user_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StatusName).HasColumnName("status_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
