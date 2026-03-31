using Microsoft.EntityFrameworkCore;
using qcmAuditoriasIatf.Models.External;
using qcmAuditoriasIatf.Models.Auditorias;
using qcmAuditoriasIatf.Models.Catalogos;
using qcmAuditoriasIatf.Models.Evidencias;
using qcmAuditoriasIatf.Models.Hallazgos;

namespace qcmAuditoriasIatf.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Catálogos
    public DbSet<Proceso> Procesos => Set<Proceso>();
    public DbSet<ClausulaIATF> ClausulasIATF => Set<ClausulaIATF>();
    public DbSet<ChecklistPregunta> ChecklistPreguntas => Set<ChecklistPregunta>();
    public DbSet<TipoHallazgo> TiposHallazgo => Set<TipoHallazgo>();
    public DbSet<UnidadNegocio> UnidadesNegocio => Set<UnidadNegocio>();
    public DbSet<Auditor> Auditores => Set<Auditor>();
    public DbSet<MeaxAllUser> MeaxAllUsers => Set<MeaxAllUser>();

    // Auditoría
    public DbSet<Auditoria> Auditorias => Set<Auditoria>();
    public DbSet<AuditoriaProceso> AuditoriaProcesos => Set<AuditoriaProceso>();
    public DbSet<AuditoriaChecklist> AuditoriaChecklists => Set<AuditoriaChecklist>();

    // Hallazgos
    public DbSet<Hallazgo> Hallazgos => Set<Hallazgo>();
    public DbSet<AccionCorrectiva> AccionesCorrectivas => Set<AccionCorrectiva>();
    public DbSet<HallazgoSeguimiento> HallazgoSeguimientos => Set<HallazgoSeguimiento>();
    public DbSet<HallazgoCincoPorQue> HallazgosCincoPorQue => Set<HallazgoCincoPorQue>();


    // Evidencias / Historial
    public DbSet<Evidencia> Evidencias => Set<Evidencia>();
    public DbSet<HistorialCambio> HistorialCambios => Set<HistorialCambio>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ===== Evitar "multiple cascade paths" en SQL Server =====
        modelBuilder.Entity<ChecklistPregunta>()
            .HasOne(x => x.Proceso)
            .WithMany()
            .HasForeignKey(x => x.ProcesoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChecklistPregunta>()
            .HasOne(x => x.Clausula)
            .WithMany(c => c.ChecklistPreguntas)
            .HasForeignKey(x => x.ClausulaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AuditoriaProceso>()
            .HasOne(x => x.Auditoria)
            .WithMany(a => a.AuditoriaProcesos)
            .HasForeignKey(x => x.AuditoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AuditoriaProceso>()
            .HasOne(x => x.Proceso)
            .WithMany(p => p.AuditoriaProcesos)
            .HasForeignKey(x => x.ProcesoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AuditoriaChecklist>()
            .HasOne(x => x.AuditoriaProceso)
            .WithMany() // si luego quieres colección en AuditoriaProceso, lo cambiamos
            .HasForeignKey(x => x.AuditoriaProcesoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AuditoriaChecklist>()
            .HasIndex(x => new { x.AuditoriaProcesoId, x.PreguntaId })
            .IsUnique();

        modelBuilder.Entity<Hallazgo>()
            .HasOne(x => x.Auditoria)
            .WithMany(a => a.Hallazgos)
            .HasForeignKey(x => x.AuditoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Hallazgo>()
            .HasOne(x => x.Proceso)
            .WithMany()
            .HasForeignKey(x => x.ProcesoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Hallazgo>()
            .HasOne(x => x.Clausula)
            .WithMany()
            .HasForeignKey(x => x.ClausulaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Hallazgo>()
            .HasOne(x => x.TipoHallazgo)
            .WithMany(t => t.Hallazgos)
            .HasForeignKey(x => x.TipoHallazgoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Hallazgo>()
            .HasOne(h => h.AuditoriaProceso)
            .WithMany()
            .HasForeignKey(h => h.AuditoriaProcesoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Hallazgo>()
            .HasOne(h => h.Pregunta)
            .WithMany()
            .HasForeignKey(h => h.PreguntaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Hallazgo>()
            .HasIndex(h => new { h.AuditoriaProcesoId, h.PreguntaId })
            .IsUnique()
            .HasFilter("[PreguntaId] IS NOT NULL");

        modelBuilder.Entity<AccionCorrectiva>()
            .HasOne(x => x.Hallazgo)
            .WithMany(h => h.AccionesCorrectivas)
            .HasForeignKey(x => x.HallazgoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UnidadNegocio>()
            .HasOne(x => x.Proceso)
            .WithMany() // (si quieres colección en Proceso, lo cambiamos)
            .HasForeignKey(x => x.ProcesoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AuditoriaProceso>()
            .HasOne(x => x.UnidadNegocio)
            .WithMany()
            .HasForeignKey(x => x.UnidadNegocioId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HallazgoSeguimiento>()
            .HasOne(s => s.Hallazgo)
            .WithMany(h => h.Seguimientos)
            .HasForeignKey(s => s.HallazgoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HallazgoSeguimiento>()
            .HasIndex(s => new { s.HallazgoId, s.FechaRegistro });

        modelBuilder.Entity<MeaxAllUser>(eb =>
        {
            eb.HasNoKey();
            eb.ToView("meax_all_user", "dbo");      // <- evita que EF la migre
            eb.Metadata.SetIsTableExcludedFromMigrations(true);
        });

        modelBuilder.Entity<MeaxAllUser>()
            .HasNoKey()
            .ToTable("meax_all_user"); // solo lectura

        modelBuilder.Entity<Auditor>()
            .HasIndex(x => x.MeaxUserId)
            .HasDatabaseName("IX_qmcAudAuditor_MeaxUserId");

        modelBuilder.Entity<Auditor>()
            .HasIndex(x => x.PcLoginId)
            .HasDatabaseName("IX_qmcAudAuditor_PcLoginId");

        modelBuilder.Entity<HallazgoCincoPorQue>()
            .HasOne(x => x.Hallazgo)
            .WithOne(h => h.CincoPorQue)
            .HasForeignKey<HallazgoCincoPorQue>(x => x.HallazgoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HallazgoCincoPorQue>()
            .HasIndex(x => x.HallazgoId)
            .IsUnique();

    }
}