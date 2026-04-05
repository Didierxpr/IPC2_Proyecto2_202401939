using IPC2_Proyecto2_202401939.DataStructures;
using IPC2_Proyecto2_202401939.Domain;
using System.Text;

namespace IPC2_Proyecto2_202401939.Services;

/// <summary>
/// Servicio para generar visualizaciones usando Graphviz.
/// Genera archivos en formato DOT que pueden convertirse a imágenes PNG o SVG.
/// </summary>
public class GeneradorGraphviz
{
    /// <summary>
    /// Genera un diagrama DOT para visualizar un sistema de drones (tabla dron x altura).
    /// </summary>
    public string GenerarDiagramaSistema(SistemaDrones sistema)
    {
        if (sistema == null)
        {
            return string.Empty;
        }

        StringBuilder dot = new();

        dot.AppendLine("digraph SistemaDrones {");
        dot.AppendLine("  rankdir=LR;");
        dot.AppendLine("  node [shape=box, style=filled, fillcolor=lightblue];");
        dot.AppendLine($"  graph [label=\"Sistema: {sistema.Nombre}\", labelloc=top, fontsize=14];");
        dot.AppendLine();

        dot.AppendLine("  subgraph cluster_tabla {");
        dot.AppendLine("    label=\"Mapeo Dron-Altura-Letra\";");
        dot.AppendLine("    style=filled;");
        dot.AppendLine("    fillcolor=lightgrey;");
        dot.AppendLine();

        MapaDronAltura[] mapas = ObtenerMapasDelSistema(sistema);

        if (mapas.Length == 0)
        {
            dot.AppendLine("    nodata [label=\"Sin mapeos\"];");
        }
        else
        {
            foreach (var mapa in mapas)
            {
                string nodoId = $"mapa_{mapa.NombreDron}_{mapa.Altura}";
                string etiqueta = $"{mapa.NombreDron}\\n@{mapa.Altura}m\\n'{mapa.Simbolo}'";
                dot.AppendLine($"    {nodoId} [label=\"{etiqueta}\"];");
            }
        }

        dot.AppendLine("  }");
        dot.AppendLine();
        dot.AppendLine("}");

        return dot.ToString();
    }

    
    /// Genera un diagrama DOT para visualizar las instrucciones de un mensaje en forma de timeline.
    /// Muestra los pasos que ejecuta cada dron.
    
    public string GenerarDiagramaInstrucciones(MensajeOptimizado mensaje)
    {
        if (mensaje == null || mensaje.InstruccionesPorTiempo.Length == 0)
        {
            return string.Empty;
        }

        StringBuilder dot = new();

        dot.AppendLine("digraph Instrucciones {");
        dot.AppendLine("  rankdir=TB;");
        dot.AppendLine("  node [shape=box, style=filled];");
        dot.AppendLine($"  graph [label=\"Mensaje: {mensaje.Nombre}\\nTiempo: {mensaje.TiempoOptimo}s\\nMensaje: {mensaje.MensajeRecibido}\", labelloc=top, fontsize=12];");
        dot.AppendLine();

        for (int t = 0; t < mensaje.InstruccionesPorTiempo.Length; t++)
        {
            var instruccion = mensaje.InstruccionesPorTiempo[t];
            var acciones = instruccion.Acciones.ObtenerTodas();

            dot.AppendLine($"  subgraph cluster_tiempo_{t} {{");
            dot.AppendLine($"    label=\"Segundo {instruccion.Tiempo}\";");
            dot.AppendLine("    style=filled;");
            dot.AppendLine("    fillcolor=lightyellow;");
            dot.AppendLine();

            foreach (var (nombreDron, accion) in acciones)
            {
                string nodoId = $"t{t}_{nombreDron}";
                string color = accion == "Emitir luz" ? "lightgreen" : "lightblue";
                dot.AppendLine($"    {nodoId} [label=\"{nombreDron}\\n{accion}\", fillcolor={color}];");
            }

            dot.AppendLine("  }");
            dot.AppendLine();
        }

        dot.AppendLine("}");

        return dot.ToString();
    }

    /// <summary>
    /// Genera una tabla HTML que muestra el sistema de drones (alternativa visual).
    /// </summary>
    public string GenerarTablaHTMLSistema(SistemaDrones sistema)
    {
        if (sistema == null)
        {
            return "<p>Sistema no encontrado</p>";
        }

        StringBuilder html = new();

        html.AppendLine("<table class=\"tabla-sistema\">");
        html.AppendLine("  <thead>");
        html.AppendLine("    <tr>");
        html.AppendLine("      <th>Altura (m)</th>");

        var mapas = ObtenerMapasDelSistema(sistema);
        var dronesUnicos = ObtenerDronesUnicos(mapas);

        foreach (var dron in dronesUnicos)
        {
            html.AppendLine($"      <th>{dron}</th>");
        }

        html.AppendLine("    </tr>");
        html.AppendLine("  </thead>");
        html.AppendLine("  <tbody>");

        for (int altura = sistema.AlturaMaxima; altura >= 1; altura--)
        {
            html.AppendLine("    <tr>");
            html.AppendLine($"      <td><strong>{altura}</strong></td>");

            foreach (var dron in dronesUnicos)
            {
                var simbolo = BuscarSimbolo(mapas, dron, altura) ?? "-";
                html.AppendLine($"      <td>{simbolo}</td>");
            }

            html.AppendLine("    </tr>");
        }

        html.AppendLine("  </tbody>");
        html.AppendLine("</table>");

        return html.ToString();
    }

    /// <summary>
    /// Obtiene todos los mapas del sistema como arreglo.
    /// </summary>
    private MapaDronAltura[] ObtenerMapasDelSistema(SistemaDrones sistema)
    {
        if (sistema?.Mapas == null)
        {
            return [];
        }

        return sistema.Mapas.ConvertirAArreglo();
    }

    /// <summary>
    /// Obtiene la lista única de drones en los mapas.
    /// </summary>
    private string[] ObtenerDronesUnicos(MapaDronAltura[] mapas)
    {
        var dronesSet = new System.Collections.Generic.HashSet<string>();
        foreach (var mapa in mapas)
        {
            dronesSet.Add(mapa.NombreDron);
        }

        var drones = new List<string>(dronesSet);
        drones.Sort();
        return drones.ToArray();
    }

    /// <summary>
    /// Busca el símbolo para un dron en una altura específica.
    /// </summary>
    private string BuscarSimbolo(MapaDronAltura[] mapas, string nombreDron, int altura)
    {
        foreach (var mapa in mapas)
        {
            if (mapa.NombreDron == nombreDron && mapa.Altura == altura)
            {
                return mapa.Simbolo;
            }
        }

        return null;
    }

    /// <summary>
    /// Codifica un string DOT a Base64 URL-safe para Kroki.io usando compresión
    /// </summary>
    private string CodificarParaKroki(string dotContent)
    {
        // Usar codificación simple para Kroki: base64 + URL encoding
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(dotContent);
        string base64 = System.Convert.ToBase64String(bytes);
        
        // Kroki usa base64url sin padding
        string base64Url = base64
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
        
        return base64Url;
    }

    /// <summary>
    /// Genera la URL de Kroki.io para visualizar un diagrama DOT del sistema.
    /// </summary>
    public string GenerarUrlKrokiSistema(SistemaDrones sistema)
    {
        if (sistema == null)
            return string.Empty;

        string dot = GenerarDiagramaSistema(sistema);
        if (string.IsNullOrEmpty(dot))
            return string.Empty;

        try
        {
            // Usar la sintaxis correcta de Kroki
            string encoded = CodificarParaKroki(dot);
            return $"https://kroki.io/graphviz/svg/{encoded}";
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Genera la URL de Kroki.io para visualizar un diagrama DOT de instrucciones.
    /// </summary>
    public string GenerarUrlKrokiInstrucciones(MensajeOptimizado mensaje)
    {
        if (mensaje == null)
            return string.Empty;

        string dot = GenerarDiagramaInstrucciones(mensaje);
        if (string.IsNullOrEmpty(dot))
            return string.Empty;

        try
        {
            string encoded = CodificarParaKroki(dot);
            return $"https://kroki.io/graphviz/svg/{encoded}";
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Genera un string SVG simplificado del sistema para visualizar sin dependencias externas.
    /// </summary>
    public string GenerarSVGSistema(SistemaDrones sistema)
    {
        if (sistema == null || sistema.Mapas == null)
            return string.Empty;

        var mapas = ObtenerMapasDelSistema(sistema);
        if (mapas.Length == 0)
            return string.Empty;

        StringBuilder svg = new();
        svg.AppendLine("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"800\" height=\"600\" style=\"border: 1px solid #ddd; border-radius: 4px;\">");
        svg.AppendLine("  <rect width=\"800\" height=\"600\" fill=\"#f9f9f9\"/>");
        svg.AppendLine($"  <text x=\"400\" y=\"30\" text-anchor=\"middle\" font-size=\"20\" font-weight=\"bold\">{sistema.Nombre}</text>");
        
        int x = 50;
        int y = 80;
        int rectWidth = 120;
        int rectHeight = 40;
        int spacing = 150;

        foreach (var mapa in mapas)
        {
            svg.AppendLine($"  <rect x=\"{x}\" y=\"{y}\" width=\"{rectWidth}\" height=\"{rectHeight}\" fill=\"#e3f2fd\" stroke=\"#1976d2\" stroke-width=\"2\" rx=\"4\"/>");
            svg.AppendLine($"  <text x=\"{x + rectWidth / 2}\" y=\"{y + 20}\" text-anchor=\"middle\" font-size=\"12\" font-weight=\"bold\">{mapa.NombreDron}</text>");
            svg.AppendLine($"  <text x=\"{x + rectWidth / 2}\" y=\"{y + 35}\" text-anchor=\"middle\" font-size=\"11\">@{mapa.Altura}m</text>");
            
            x += spacing;
            if (x > 650)
            {
                x = 50;
                y += 100;
            }
        }

        svg.AppendLine("</svg>");
        return svg.ToString();
    }

    /// <summary>
    /// Genera un SVG de timeline para visualizar las instrucciones de un mensaje.
    /// </summary>
    public string GenerarSVGInstrucciones(MensajeOptimizado mensaje)
    {
        if (mensaje == null || mensaje.InstruccionesPorTiempo.Length == 0)
            return string.Empty;

        StringBuilder svg = new();
        int width = Math.Max(1000, 200 + mensaje.InstruccionesPorTiempo.Length * 120);
        int height = 400;

        svg.AppendLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{width}\" height=\"{height}\" style=\"border: 1px solid #ddd; border-radius: 4px; overflow: auto;\">");
        svg.AppendLine($"  <rect width=\"{width}\" height=\"{height}\" fill=\"#fafafa\"/>");
        svg.AppendLine($"  <text x=\"20\" y=\"30\" font-size=\"16\" font-weight=\"bold\">Timeline: {mensaje.Nombre}</text>");
        svg.AppendLine($"  <text x=\"20\" y=\"55\" font-size=\"12\" fill=\"#666\">Tiempo Total: {mensaje.TiempoOptimo} segundos</text>");

        int timelineY = 100;
        int timelineX = 50;
        int boxWidth = 100;
        int boxHeight = 50;
        int spacing = 120;

        for (int t = 0; t < mensaje.InstruccionesPorTiempo.Length; t++)
        {
            var instruccion = mensaje.InstruccionesPorTiempo[t];
            var acciones = instruccion.Acciones.ObtenerTodas();

            // Línea vertical del timeline
            svg.AppendLine($"  <line x1=\"{timelineX + t * spacing + boxWidth / 2}\" y1=\"80\" x2=\"{timelineX + t * spacing + boxWidth / 2}\" y2=\"{timelineY}\" stroke=\"#ccc\" stroke-width=\"1\"/>");
            
            // Número de tiempo
            svg.AppendLine($"  <text x=\"{timelineX + t * spacing + boxWidth / 2}\" y=\"90\" text-anchor=\"middle\" font-size=\"12\" font-weight=\"bold\">T{instruccion.Tiempo}s</text>");

            int y = timelineY;
            foreach (var (nombreDron, accion) in acciones)
            {
                string fillColor = accion == "Emitir luz" ? "#90EE90" : "#87CEEB";
                string strokeColor = accion == "Emitir luz" ? "#228B22" : "#4682B4";

                svg.AppendLine($"  <rect x=\"{timelineX + t * spacing}\" y=\"{y}\" width=\"{boxWidth}\" height=\"{boxHeight}\" fill=\"{fillColor}\" stroke=\"{strokeColor}\" stroke-width=\"1\" rx=\"3\"/>");
                svg.AppendLine($"  <text x=\"{timelineX + t * spacing + boxWidth / 2}\" y=\"{y + 20}\" text-anchor=\"middle\" font-size=\"10\" font-weight=\"bold\">{nombreDron}</text>");
                svg.AppendLine($"  <text x=\"{timelineX + t * spacing + boxWidth / 2}\" y=\"{y + 40}\" text-anchor=\"middle\" font-size=\"9\">{accion}</text>");

                y += boxHeight + 10;
            }
        }

        svg.AppendLine("</svg>");
        return svg.ToString();
    }

    /// <summary>
    /// Guarda un contenido DOT en un archivo .dot.
    /// </summary>
    public bool GuardarArchivoDot(string rutaArchivo, string contenidoDot)
    {
        try
        {
            System.IO.File.WriteAllText(rutaArchivo, contenidoDot);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Convierte un archivo DOT a PNG usando Graphviz (requiere Graphviz instalado).
    /// </summary>
    public bool ConvertirDotAPng(string rutaDot, string rutaPng)
    {
        try
        {
            var processInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "dot",
                Arguments = $"-Tpng -o \"{rutaPng}\" \"{rutaDot}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(processInfo))
            {
                process?.WaitForExit(5000);
                return true;
            }
        }
        catch
        {
            return false;
        }
    }
}
