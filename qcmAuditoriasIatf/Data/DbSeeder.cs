using Microsoft.EntityFrameworkCore;
using qcmAuditoriasIatf.Models.Catalogos;

namespace qcmAuditoriasIatf.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        // 1) Procesos (inserta los que falten por Codigo)
        var procesosSeed = new List<Proceso>
        {
            new() { Codigo="MX-0100-D1", Nombre="ALTA DIRECCIÓN", Area="QMS", Activo=true },
            new() { Codigo="MX-6100-D1", Nombre="AUDITORIAS INTERNAS", Area="QMS", Activo=true },
            new() { Codigo="MX-6700-D1", Nombre="SATISFACCIÓN AL CLIENTE / QUEJAS DEL CLIENTE", Area="QMS", Activo=true },
            new() { Codigo="MX-6600-D1", Nombre="NO CONFORMIDADES, ACCIONES CORRECTIVAS Y PREVENTIVAS", Area="QMS", Activo=true },
            new() { Codigo="MX-3000-D1", Nombre="VENTAS", Area="QMS", Activo=true },
            new() { Codigo="MX-6200-D1", Nombre="APQP", Area="QMS", Activo=true },
            new() { Codigo="MX-7100-D1", Nombre="PLANEACIÓN DE LA PRODUCCIÓN", Area="QMS", Activo=true },
            new() { Codigo="MX-4000-D1", Nombre="COMPRAS / SELECCIÓN Y DESARROLLO DE PROVEEDORES", Area="QMS", Activo=true },
            new() { Codigo="MX-9000-D1", Nombre="RECIBO, ALMACENAJE, SURTIMIENTO, PRODUCTO TERMINADO Y EMBARQUES", Area="QMS", Activo=true },
            new() { Codigo="MX-6000-D1", Nombre="CALIDAD PROCESO", Area="QMS", Activo=true },
            new() { Codigo="MX-7000-D1", Nombre="PRODUCCIÓN", Area="QMS", Activo=true },
            new() { Codigo="MX-8000-D1", Nombre="INGENIERIA", Area="QMS", Activo=true },
            new() { Codigo="MX-6300-D1", Nombre="CALIDAD PROVEEDORES", Area="QMS", Activo=true },
            new() { Codigo="MX-6500-D1", Nombre="GESTION DE CAMBIOS", Area="QMS", Activo=true },
            new() { Codigo="MX-1000-D1", Nombre="RECURSOS HUMANOS Y MOTIVACIÓN DEL PERSONAL", Area="QMS", Activo=true },
            new() { Codigo="MX-1300-D1", Nombre="ENTRENAMIENTO", Area="QMS", Activo=true },
            new() { Codigo="MX-6400-D1", Nombre="CONTROL DE EQUIPOS E INSTRUMENTOS DE MEDICIÓN", Area="QMS", Activo=true },
        };

        var existentes = await db.Procesos.Select(p => p.Codigo).ToListAsync();
        var faltantes = procesosSeed.Where(p => !existentes.Contains(p.Codigo)).ToList();
        if (faltantes.Count > 0)
        {
            db.Procesos.AddRange(faltantes);
            await db.SaveChangesAsync();
        }

        // 2) Unidades de negocio (solo para MX-7000-D1)
        var produccion = await db.Procesos.FirstOrDefaultAsync(p => p.Codigo == "MX-7000-D1");
        if (produccion != null)
        {
            var unidadesSeed = new List<UnidadNegocio>
            {
                new() { ProcesoId = produccion.ProcesoId, Codigo="ALT", Nombre="Alternador", Activo=true },
                new() { ProcesoId = produccion.ProcesoId, Codigo="MAR", Nombre="Marchas", Activo=true },
                new() { ProcesoId = produccion.ProcesoId, Codigo="CME", Nombre="Car Mechatronics", Activo=true },
                new() { ProcesoId = produccion.ProcesoId, Codigo="EPS", Nombre="EPS", Activo=true },
                new() { ProcesoId = produccion.ProcesoId, Codigo="MUL", Nombre="Multimedia", Activo=true },
                new() { ProcesoId = produccion.ProcesoId, Codigo="CEL", Nombre="Car Electronics", Activo=true },
            };

            var ya = await db.UnidadesNegocio
                .Where(u => u.ProcesoId == produccion.ProcesoId)
                .Select(u => u.Codigo)
                .ToListAsync();

            var faltanUnidades = unidadesSeed.Where(u => !ya.Contains(u.Codigo)).ToList();
            if (faltanUnidades.Count > 0)
            {
                db.UnidadesNegocio.AddRange(faltanUnidades);
                await db.SaveChangesAsync();
            }
        }
    }
}