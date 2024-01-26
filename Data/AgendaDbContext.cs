using System.Reflection.Emit;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
//using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using Ikigenda.Models;

namespace Ikigenda.Data;

public class AgendaDbContext : DbContext
{
    public AgendaDbContext()
    {
    }

    public AgendaDbContext(DbContextOptions<AgendaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Historial> Historials { get; set; }

    public virtual DbSet<Planin> Planins { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; }

    public virtual DbSet<TipoServicio> TipoServicios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

   /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;user=neo;password=Matrix68;database=Agenda", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.6.12-mariadb"));*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Documento).HasName("PRIMARY");

            entity.ToTable("Cliente");

            entity.HasIndex(e => e.ClienteId, "ClienteId_UNIQUE").IsUnique();

            entity.HasIndex(e => e.TipoDocumentoId, "FK_TipoDocumento");

            entity.Property(e => e.Documento).HasMaxLength(45);
            entity.Property(e => e.Apellidos).HasMaxLength(60);
            entity.Property(e => e.ClienteId)
                .ValueGeneratedOnAdd()
                .HasColumnType("int(11)");
            entity.Property(e => e.CodPostal).HasMaxLength(5);
            entity.Property(e => e.Correo).HasMaxLength(255);
            entity.Property(e => e.Direccion).HasMaxLength(75);
            entity.Property(e => e.Nombre).HasMaxLength(45);
            entity.Property(e => e.Observaciones).HasMaxLength(255);
            entity.Property(e => e.Poblacion).HasMaxLength(45);
            entity.Property(e => e.Provincia).HasMaxLength(45);
            entity.Property(e => e.Telefono).HasMaxLength(9);
            entity.Property(e => e.TipoDocumentoId).HasColumnType("int(11)");

            entity.HasOne(d => d.TipoDocumento).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.TipoDocumentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TipoDocumento");
        });

        modelBuilder.Entity<Historial>(entity =>
        {
            entity.HasKey(e => e.HistorialId).HasName("PRIMARY");

            entity.ToTable("Historial");

            entity.HasIndex(e => e.ClienteId, "fk_Historial_Cliente_idx");

            entity.HasIndex(e => e.PlaninId, "fk_Historial_Planin_idx");

            entity.HasIndex(e => e.TipoServicioId, "fk_Historial_TipoServicio_idx");

            entity.Property(e => e.HistorialId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.ClienteId).HasColumnType("int(11)");
            entity.Property(e => e.DateLocal).HasColumnType("datetime");
            entity.Property(e => e.Descripcion).HasMaxLength(100);
            entity.Property(e => e.Observaciones).HasMaxLength(1000);
            entity.Property(e => e.PlaninId).HasColumnType("int(11)");
            entity.Property(e => e.TipoServicioId).HasColumnType("int(11)");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Historials)
                .HasPrincipalKey(p => p.ClienteId)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Historial_Cliente");

            entity.HasOne(d => d.Planin).WithMany(p => p.Historials)
                .HasForeignKey(d => d.PlaninId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Historial_Planin");

            entity.HasOne(d => d.TipoServicio).WithMany(p => p.Historials)
                .HasForeignKey(d => d.TipoServicioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Historial_TipoServicio");
        });

        modelBuilder.Entity<Planin>(entity =>
        {
            entity.HasKey(e => e.PlaninId).HasName("PRIMARY");

            entity.ToTable("Planin");

            entity.Property(e => e.PlaninId).HasColumnType("int(11)");
            entity.Property(e => e.Disponible)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(4)");
            entity.Property(e => e.FechaLocal)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PRIMARY");

            entity.ToTable("Rol");

            entity.Property(e => e.RolId).HasColumnType("int(11)");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .HasDefaultValueSql("'Staff'");
        });

        modelBuilder.Entity<TipoDocumento>(entity =>
        {
            entity.HasKey(e => e.TipoDocumentoId).HasName("PRIMARY");

            entity.ToTable("TipoDocumento", tb => tb.HasComment("Tabla de los tipos de documentos admitidos"));

            entity.HasIndex(e => e.Nombre, "Nombre_UNIQUE").IsUnique();

            entity.Property(e => e.TipoDocumentoId).HasColumnType("int(11)");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<TipoServicio>(entity =>
        {
            entity.HasKey(e => e.TipoServicioId).HasName("PRIMARY");

            entity.ToTable("TipoServicio");

            entity.Property(e => e.TipoServicioId).HasColumnType("int(11)");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PRIMARY");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Correo, "Correo_UNIQUE").IsUnique();

            entity.HasIndex(e => e.NombreUsuario, "NombreUsuario_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Telefono, "Telefono_UNIQUE").IsUnique();

            entity.HasIndex(e => e.RolId, "fk_Usuario_Rol_idx");

            entity.Property(e => e.UsuarioId).HasColumnType("int(11)");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(4)");
            entity.Property(e => e.Contrasenya).HasMaxLength(25);
            entity.Property(e => e.Nombre).HasMaxLength(45);
            entity.Property(e => e.NombreUsuario).HasMaxLength(45);
            entity.Property(e => e.RolId).HasColumnType("int(11)");
            entity.Property(e => e.Telefono).HasMaxLength(9);

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Usuario_Rol");
        });
             
       
 

        //OnModelCreatingPartial(modelBuilder);
    }

    //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
