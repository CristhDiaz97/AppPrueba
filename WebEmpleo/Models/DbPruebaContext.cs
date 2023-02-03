using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebEmpleo.Models;

public partial class DbPruebaContext : DbContext
{
    public DbPruebaContext()
    {
    }

    public DbPruebaContext(DbContextOptions<DbPruebaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<Experiencia> Experiencias { get; set; }

    public virtual DbSet<Industrium> Industria { get; set; }

    public virtual DbSet<NivelesEducativo> NivelesEducativos { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Postulacion> Postulacions { get; set; }

    public virtual DbSet<Vacante> Vacantes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-NB23DOD;Initial Catalog=dbPrueba;Persist Security Info=False;User ID=dbaraque;Password=Araque1234;Connection Timeout=30;Encrypt=False;TrustServerCertificate=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.IdEmpresa).HasName("PK__Empresas__5EF4033E4DF741B8");

            entity.Property(e => e.Ciudad)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.FchCreate).HasColumnType("date");
            entity.Property(e => e.FchUpdate).HasColumnType("date");
            entity.Property(e => e.IdUsuarios).HasMaxLength(450);
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUsuariosNavigation).WithMany(p => p.Empresas)
                .HasForeignKey(d => d.IdUsuarios)
                .HasConstraintName("FK_Empresas_AspNetUsers");

            entity.HasOne(d => d.IndustriaNavigation).WithMany(p => p.Empresas)
                .HasForeignKey(d => d.Industria)
                .HasConstraintName("fk_empresas_indutria");
        });

        modelBuilder.Entity<Experiencia>(entity =>
        {
            entity.HasKey(e => e.IdExperienciaLaboral).HasName("PK__Experien__FAB3C8EC19D13D7A");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.FchCreate).HasColumnType("date");
            entity.Property(e => e.FchFin).HasColumnType("date");
            entity.Property(e => e.FchInicio).HasColumnType("date");
            entity.Property(e => e.FchUpdate).HasColumnType("date");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Experiencia)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Person_Experiencias");
        });

        modelBuilder.Entity<Industrium>(entity =>
        {
            entity.HasKey(e => e.IdIndustria).HasName("PK__Industri__9FAF2A4FDF2A72FC");

            entity.Property(e => e.IdIndustria).HasColumnName("idIndustria");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FchCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fchCreacion");
        });

        modelBuilder.Entity<NivelesEducativo>(entity =>
        {
            entity.HasKey(e => e.IdNivelEducativo).HasName("PK__NivelesE__5035CA167DC8284B");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.FchCreate).HasColumnType("date");
            entity.Property(e => e.FchUpdate).HasColumnType("date");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.IdPersona).HasName("PK__Person__2EC8D2ACD9AF6373");

            entity.ToTable("Person");

            entity.Property(e => e.Apellidos)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.FchCreate).HasColumnType("date");
            entity.Property(e => e.FchUpdate).HasColumnType("date");
            entity.Property(e => e.IdUsuarios).HasMaxLength(450);
            entity.Property(e => e.Nombres)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Notas)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdNivelEducativoNavigation).WithMany(p => p.People)
                .HasForeignKey(d => d.IdNivelEducativo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Person_NivelesEducativos");

            entity.HasOne(d => d.IdUsuariosNavigation).WithMany(p => p.People)
                .HasForeignKey(d => d.IdUsuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Personas_AspNetUsers");
        });

        modelBuilder.Entity<Postulacion>(entity =>
        {
            entity.HasKey(e => e.IdPostulacion).HasName("PK__Postulac__E95628FBA18A53DE");

            entity.ToTable("Postulacion");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FchCreate).HasColumnType("date");
            entity.Property(e => e.FchUpdate).HasColumnType("date");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Postulacions)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Postulacion_Person");
        });

        modelBuilder.Entity<Vacante>(entity =>
        {
            entity.HasKey(e => e.IdVacante).HasName("PK__Vacante__A58A8FA8974A7A9E");

            entity.ToTable("Vacante");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Estudios)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Experiencia)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FchCreate).HasColumnType("date");
            entity.Property(e => e.FchUpdate).HasColumnType("date");
            entity.Property(e => e.FchVencimiento).HasColumnType("date");
            entity.Property(e => e.Localizacion)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.TipoContrato)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
