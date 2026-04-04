using IPC2_Proyecto2_202401939.Domain;
using IPC2_Proyecto2_202401939.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proyecto2_202401939.Pages.MensajeDetalle;

/// <summary>
/// Página que muestra las instrucciones detalladas de un mensaje optimizado.
/// Permite visualizar el tiempo óptimo, el mensaje decodificado y la tabla de acciones.
/// </summary>
public class IndexModel : PageModel
{
    private readonly AppStateService _appState;
    private readonly OptimizadorMensaje _optimizador;

    public IndexModel(AppStateService appState, OptimizadorMensaje optimizador)
    {
        _appState = appState;
        _optimizador = optimizador;
        MensajeActual = null;
        InstruccionesPorTiempo = [];
    }

    /// <summary>
    /// El mensaje seleccionado para visualizar.
    /// </summary>
    public MensajeOptimizado? MensajeActual { get; private set; }

    /// <summary>
    /// Array de instrucciones ordenadas por tiempo.
    /// </summary>
    public InstruccionPorTiempo[] InstruccionesPorTiempo { get; private set; }

    /// <summary>
    /// Carga las instrucciones del mensaje seleccionado.
    /// El nombre del mensaje llega como parámetro de query: ?nombreMensaje=NombreMensaje
    /// </summary>
    public void OnGet(string nombreMensaje)
    {
        if (string.IsNullOrWhiteSpace(nombreMensaje))
        {
            return;
        }

        // Obtener el mensaje del estado
        MensajeConfig? mensaje = _appState.ObtenerMensajePorNombre(nombreMensaje);
        if (mensaje == null)
        {
            return;
        }

        // Generar instrucciones optimizadas
        MensajeActual = _appState.GenerarInstruccionesOptimizadas(mensaje, _optimizador);

        // Obtener instrucciones por tiempo
        if (MensajeActual != null && MensajeActual.InstruccionesPorTiempo != null)
        {
            InstruccionesPorTiempo = MensajeActual.InstruccionesPorTiempo;
        }
    }

    /// <summary>
    /// Obtiene el nombre de todos los drones ordenado alfabéticamente.
    /// Útil para mostrar columnas en la tabla de instrucciones.
    /// </summary>
    public string[] ObtenerNombresDrones()
    {
        Dron[] drones = _appState.ObtenerDrones();
        string[] nombres = new string[drones.Length];
        for (int i = 0; i < drones.Length; i++)
        {
            nombres[i] = drones[i].Nombre;
        }
        return nombres;
    }

    /// <summary>
    /// Obtiene la acción de un dron en un tiempo específico para mostrarla en la tabla.
    /// </summary>
    public string ObtenerAccion(InstruccionPorTiempo instruccion, string nombreDron)
    {
        if (instruccion == null)
        {
            return "Esperar";
        }

        return instruccion.ObtenerAccion(nombreDron);
    }
}
