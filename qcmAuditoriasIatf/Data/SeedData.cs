using Microsoft.EntityFrameworkCore;
using qcmAuditoriasIatf.Models.Catalogos;

namespace qcmAuditoriasIatf.Data;

public static class SeedData
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        // El seed solo debe correr una vez, para inicializar un catálogo vacío.
        // Si ya hay procesos (por poco que sea), es porque el catálogo ya lo administran
        // los usuarios: no se debe volver a insertar ni reactivar nada en cada arranque.
        if (await db.Procesos.AnyAsync())
            return;

        // ===== PROCESOS (upsert por Codigo) =====
        var procesos = new List<(string Codigo, string Nombre)>
        {
            ("MX-0100-D1","ALTA DIRECCIÓN"),
            ("MX-6100-D1","AUDITORIAS INTERNAS"),
            ("MX-6700-D1","SATISFACCIÓN AL CLIENTE / QUEJAS DEL CLIENTE"),
            ("MX-6600-D1","NO CONFORMIDADES, ACCIONES CORRECTIVAS Y PREVENTIVAS"),
            ("MX-3000-D1","VENTAS"),
            ("MX-6200-D1","APQP"),
            ("MX-7100-D1","PLANEACIÓN DE LA PRODUCCIÓN"),
            ("MX-4000-D1","COMPRAS / SELECCIÓN Y DESARROLLO DE PROVEEDORES"),
            ("MX-9000-D1","RECIBO, ALMACENAJE, SURTIMIENTO, PRODUCTO TERMINADO Y EMBARQUES"),
            ("MX-7000-D1 / MX-6000-D1","PRODUCCIÓN / CALIDAD PROCESO"),
            ("MX-8000-D1","INGENIERIA"),
            ("MX-6300-D1","CALIDAD PROVEEDORES"),
            ("MX-6500-D1","GESTION DE CAMBIOS"),
            ("MX-1000-D1","RECURSOS HUMANOS Y MOTIVACIÓN DEL PERSONAL"),
            ("MX-1300-D1","ENTRENAMIENTO"),
            ("MX-6400-D1","CONTROL DE EQUIPOS E INSTRUMENTOS DE MEDICIÓN"),
        };

        foreach (var p in procesos)
        {
            var existente = await db.Procesos.FirstOrDefaultAsync(x => x.Codigo == p.Codigo);
            if (existente == null)
            {
                db.Procesos.Add(new Proceso
                {
                    Codigo = p.Codigo,
                    Nombre = p.Nombre,
                    Area = "QMS",
                    Activo = true
                });
            }
            else
            {
                // si ya existe, actualiza nombre/activo/area (opcional)
                existente.Nombre = p.Nombre;
                existente.Area = string.IsNullOrWhiteSpace(existente.Area) ? "QMS" : existente.Area;
                existente.Activo = true;
            }
        }

        await db.SaveChangesAsync();

        // ===== UNIDADES DE NEGOCIO (solo para PRODUCCIÓN / CALIDAD PROCESO) =====
        var prod = await db.Procesos.FirstOrDefaultAsync(x => x.Codigo == "MX-7000-D1 / MX-6000-D1");
        if (prod != null)
        {
            var unidades = new List<(string Codigo, string Nombre)>
            {
                ("ALT","Alternador"),
                ("MAR","Marchas"),
                ("CME","Car Mechatronics"),
                ("EPS","EPS"),
                ("MUL","Multimedia"),
                ("CEL","Car Electronics"),
            };

            foreach (var u in unidades)
            {
                var existeUN = await db.UnidadesNegocio
                    .AnyAsync(x => x.ProcesoId == prod.ProcesoId && x.Codigo == u.Codigo);

                if (!existeUN)
                {
                    db.UnidadesNegocio.Add(new UnidadNegocio
                    {
                        ProcesoId = prod.ProcesoId,
                        Codigo = u.Codigo,
                        Nombre = u.Nombre,
                        Activo = true
                    });
                }
            }

            await db.SaveChangesAsync();
        }
    }

    // Guardado aparte de SeedAsync: ese método ya no vuelve a correr una vez que
    // el catálogo de Procesos existe (ver comentario arriba), así que la matriz de
    // competencias (MX-6100-F7) necesita su propio candado independiente para
    // poblarse la primera vez, sin importar cuándo se agregue este catálogo.
    public static async Task SeedCompetenciasAuditorAsync(ApplicationDbContext db)
    {
        if (await db.CompetenciasAuditor.AnyAsync())
            return;

        // (Nombre, Grupo, Obligatoria **, Orden) — según MX-6100-F7 rev. D.
        var competencias = new List<(string Nombre, string Grupo, bool Obligatoria, int Orden)>
        {
            ("Requisitos específicos de cliente", "Conocimientos específicos", true, 1),
            ("INTERPRETACIÓN ISO 9001 E IATF 16949", "Conocimientos específicos", true, 2),
            ("Auditor Interno ISO 19011", "Conocimientos específicos", true, 3),
            ("Core Tools - AMEF", "Conocimientos específicos", true, 4),
            ("Core Tools - APQP", "Conocimientos específicos", true, 5),
            ("Core Tools - PPAP", "Conocimientos específicos", true, 6),
            ("Core Tools - MSA", "Conocimientos específicos", true, 7),
            ("Core Tools - SPC", "Conocimientos específicos", true, 8),
            ("Core Tools - CP", "Conocimientos específicos", true, 9),
            ("GD&T", "Conocimientos específicos", false, 10),
            ("8D's", "Conocimientos específicos", false, 11),
            ("CQI-8 LPA", "Conocimientos específicos", false, 12),
            ("Gestión de Riesgos ISO 31000:2018", "Conocimientos específicos", true, 13),
            ("CQI-14", "Conocimientos CSR", false, 14),
            ("CQI-15", "Conocimientos CSR", false, 15),
            ("Soldadura IPC610 / 7711", "Conocimientos CSR", false, 16),
        };

        foreach (var c in competencias)
        {
            db.CompetenciasAuditor.Add(new CompetenciaAuditor
            {
                Nombre = c.Nombre,
                Grupo = c.Grupo,
                Obligatoria = c.Obligatoria,
                Orden = c.Orden,
                Activo = true
            });
        }

        await db.SaveChangesAsync();
    }
}