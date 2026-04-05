using IPC2_Proyecto2_202401939.Domain;
using IPC2_Proyecto2_202401939.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proyecto2_202401939.Pages.DescargarSalida;

/// <summary>
/// Controlador para generar y descargar archivos XML de salida con los resultados
/// de la optimización de mensajes.
/// </summary>
public class IndexModel : PageModel
{
    private readonly AppStateService _appState;
    private readonly OptimizadorMensaje _optimizador;
    private readonly XmlOutputService _xmlOutputService;

    public IndexModel(
        AppStateService appState,
        OptimizadorMensaje optimizador,
        XmlOutputService xmlOutputService)
    {
        _appState = appState;
        _optimizador = optimizador;
        _xmlOutputService = xmlOutputService;
        Mensajes = [];
    }

    public MensajeConfig[] Mensajes { get; private set; }

    /// <summary>
    /// Carga la página con la lista de mensajes disponibles.
    /// </summary>
    public void OnGet()
    {
        Mensajes = _appState.ObtenerMensajes();
    }

    /// <summary>
    /// Descarga un archivo XML con los resultados optimizados de todos los mensajes.
    /// </summary>
    public IActionResult OnGetDescargar()
    {
        try
        {
            MensajeConfig[] mensajes = _appState.ObtenerMensajes();
            if (mensajes.Length == 0)
            {
                return new ContentResult
                {
                    Content = "No hay mensajes para descargar.",
                    StatusCode = 400
                };
            }

            List<MensajeOptimizado> mensajesOptimizados = new();

            foreach (var mensaje in mensajes)
            {
                MensajeOptimizado optimizado = _appState.GenerarInstruccionesOptimizadas(mensaje, _optimizador);
                mensajesOptimizados.Add(optimizado);
            }

            string xmlSalida = _xmlOutputService.GenerarXmlSalida(mensajesOptimizados.ToArray());

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(xmlSalida);
            string nombreArchivo = $"salida_{DateTime.Now:yyyyMMdd_HHmmss}.xml";

            return File(buffer, "application/xml", nombreArchivo);
        }
        catch (Exception ex)
        {
            return new ContentResult
            {
                Content = $"Error al generar archivo: {ex.Message}",
                StatusCode = 500
            };
        }
    }
}
