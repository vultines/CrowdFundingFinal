using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Actual.Models;

public partial class CrowdFundingContext : DbContext
{
    public CrowdFundingContext()
    {
    }

    public CrowdFundingContext(DbContextOptions<CrowdFundingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Campaña> Campañas { get; set; }

    public virtual DbSet<CampañaIntegrante> CampañaIntegrantes { get; set; }

    public virtual DbSet<CategoriaAsesoramiento> CategoriaAsesoramientos { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Imagen> Imagens { get; set; }

    public virtual DbSet<Integrante> Integrantes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LUNA\\SQLEXPRESS2;Database=CrowdFunding;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CI_AS");

        modelBuilder.Entity<Admin>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Admin");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Campaña>(entity =>
        {
            entity.ToTable("Campaña");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DescripcionGeneral)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Enlace)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("enlace");
            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaCierre)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Cierre");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Inicio");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.IdCategoriaAsesoramiento).HasColumnName("idCategoriaAsesoramiento");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.NombreCampaña)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Objetivos)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Riesgos)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Vision)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Campañas)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("FK_Campaña_Categoria");

            entity.HasOne(d => d.IdCategoriaAsesoramientoNavigation).WithMany(p => p.Campañas)
                .HasForeignKey(d => d.IdCategoriaAsesoramiento)
                .HasConstraintName("FK_Campaña_Categoria_Asesoramiento");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Campañas)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_Campaña_Usuario");
        });

        modelBuilder.Entity<CampañaIntegrante>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Campaña_integrante");

            entity.Property(e => e.IdCampaña).HasColumnName("id_campaña");
            entity.Property(e => e.IdIntegrante).HasColumnName("id_integrante");

            entity.HasOne(d => d.IdCampañaNavigation).WithMany()
                .HasForeignKey(d => d.IdCampaña)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Campaña_integrante_Campaña");

            entity.HasOne(d => d.IdIntegranteNavigation).WithMany()
                .HasForeignKey(d => d.IdIntegrante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Campaña_integrante_Integrantes");
        });

        modelBuilder.Entity<CategoriaAsesoramiento>(entity =>
        {
            entity.ToTable("Categoria_Asesoramiento");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreAsesoramiento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Nombre_Asesoramiento");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Imagen>(entity =>
        {
            entity.ToTable("Imagen");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdCampaña).HasColumnName("idCampaña");

            entity.HasOne(d => d.IdCampañaNavigation).WithMany(p => p.Imagens)
                .HasForeignKey(d => d.IdCampaña)
                .HasConstraintName("FK_Imagen_Campaña");
        });

        modelBuilder.Entity<Integrante>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Nombre_Completo");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Contraseña).IsUnicode(false);
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Rol)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Token).HasMaxLength(150);
            entity.Property(e => e.TokenExpiracion).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
