using Microsoft.EntityFrameworkCore;
using qcmAuditoriasIatf.Models.Catalogos;

namespace qcmAuditoriasIatf.Data;

public static class SeedData
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
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
            ("MX-6000-D1","CALIDAD PROCESO"),
            ("MX-7000-D1","PRODUCCIÓN"),
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

        // ===== UNIDADES DE NEGOCIO (solo para PRODUCCIÓN) =====
        var prod = await db.Procesos.FirstOrDefaultAsync(x => x.Codigo == "MX-7000-D1");
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
}