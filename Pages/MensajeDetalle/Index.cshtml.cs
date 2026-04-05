using IPC2_Proyecto2_202401939.Domain;
using IPC2_Proyecto2_202401939.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proyecto2_202401939.Pages.MensajeDetalle;


/// Página que muestra las instrucciones detalladas de un mensaje optimizado.
/// Permite visualizar el tiempo óptimo, el mensaje decodificado y la tabla de acciones.

public class IndexModel : PageModel
{
    private readonly AppStateService _appState;
    private readonly OptimizadorMensaje _optimizador;
    private readonly GeneradorGraphviz _generador;

    public IndexModel(AppStateService appState, OptimizadorMensaje optimizador, GeneradorGraphviz generador)
    {
        _appState = appState;
        _optimizador = optimizador;
        _generador = generador;
        MensajeActual = null;
        InstruccionesPorTiempo = [];
        SvgGrafica = string.Empty;
    }

    
    /// El mensaje seleccionado para visualizar.
    
    public MensajeOptimizado? MensajeActual { get; private set; }

   
    /// Array de instrucciones ordenadas por tiempo.
   
    public InstruccionPorTiempo[] InstruccionesPorTiempo { get; private set; }

    /// <summary>
    /// SVG de la gráfica de instrucciones para este mensaje.
    /// </summary>
    public string SvgGrafica { get; private set; }

    
    /// Carga las instrucciones del mensaje seleccionado.
    /// El nombre del mensaje llega como parámetro de query: ?nombreMensaje=NombreMensaje
    
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
            
            // Generar SVG para el diagrama
            SvgGrafica = _generador.GenerarSVGInstrucciones(MensajeActual);
        }
    }

    
    /// Obtiene el nombre de todos los drones ordenado alfabéticamente.
    /// Útil para mostrar columnas en la tabla de instrucciones.
    
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

    
    /// Obtiene la acción de un dron en un tiempo específico para mostrarla en la tabla.
    
    public string ObtenerAccion(InstruccionPorTiempo instruccion, string nombreDron)
    {
        if (instruccion == null)
        {
            return "Esperar";
        }

        return instruccion.ObtenerAccion(nombreDron);
    }
}
