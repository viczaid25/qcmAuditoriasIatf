using System.Globalization;
using System.Text;
using ClosedXML.Excel;

namespace qcmAuditoriasIatf.Services;

public class ChecklistPreguntaParseada
{
    public string Numero { get; set; } = "";
    public string? RequisitoTexto { get; set; }
    public string PreguntaTexto { get; set; } = "";
}

public class ChecklistExcelParseResult
{
    public string? ProcesoTexto { get; set; }
    public List<ChecklistPreguntaParseada> Preguntas { get; set; } = new();
}

public static class ChecklistExcelParser
{
    private static readonly string[] FooterMarkers =
    {
        "elaborado por",
        "prepared by",
        "firma de aprobacion",
        "signature of approval"
    };

    public static ChecklistExcelParseResult Parse(Stream fileStream)
    {
        using var workbook = new XLWorkbook(fileStream);

        var sheet = workbook.Worksheets.FirstOrDefault(w =>
            string.Equals(w.Name.Trim(), "Checklist", StringComparison.OrdinalIgnoreCase))
            ?? workbook.Worksheets.First();

        var result = new ChecklistExcelParseResult
        {
            ProcesoTexto = BuscarProcesoTexto(sheet)
        };

        var headerRow = BuscarFilaEncabezado(sheet);
        if (headerRow is null)
            return result;

        var usedRows = sheet.LastRowUsed()?.RowNumber() ?? headerRow.Value;

        for (var row = headerRow.Value + 1; row <= usedRows; row++)
        {
            var numero = sheet.Cell(row, 2).GetString().Trim(); // columna B
            var requisito = sheet.Cell(row, 3).GetString().Trim(); // columna C
            var pregunta = sheet.Cell(row, 10).GetString().Trim(); // columna J

            if (string.IsNullOrWhiteSpace(numero) && string.IsNullOrWhiteSpace(pregunta))
                continue;

            if (EsMarcadorDePie(numero) || EsMarcadorDePie(pregunta))
                break;

            if (string.IsNullOrWhiteSpace(pregunta))
                continue;

            result.Preguntas.Add(new ChecklistPreguntaParseada
            {
                Numero = string.IsNullOrWhiteSpace(numero) ? (result.Preguntas.Count + 1).ToString() : numero,
                RequisitoTexto = string.IsNullOrWhiteSpace(requisito) ? null : requisito,
                PreguntaTexto = pregunta
            });
        }

        return result;
    }

    private static string? BuscarProcesoTexto(IXLWorksheet sheet)
    {
        var usedRows = sheet.LastRowUsed()?.RowNumber() ?? 0;

        for (var row = 1; row <= usedRows; row++)
        {
            var etiqueta = sheet.Cell(row, 1).GetString(); // columna A
            if (etiqueta.Contains("Proceso a auditar", StringComparison.OrdinalIgnoreCase))
            {
                return sheet.Cell(row, 7).GetString().Trim(); // columna G
            }
        }

        return null;
    }

    private static int? BuscarFilaEncabezado(IXLWorksheet sheet)
    {
        var usedRows = sheet.LastRowUsed()?.RowNumber() ?? 0;

        for (var row = 1; row <= usedRows; row++)
        {
            var valor = sheet.Cell(row, 2).GetString().Trim(); // columna B
            if (valor == "#")
                return row;
        }

        return null;
    }

    private static bool EsMarcadorDePie(string valor)
    {
        var normalizado = QuitarAcentos(valor.Trim()).ToLowerInvariant();
        return FooterMarkers.Any(m => normalizado.Contains(m));
    }

    private static string QuitarAcentos(string texto)
    {
        var descompuesto = texto.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder();

        foreach (var c in descompuesto)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                builder.Append(c);
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}
