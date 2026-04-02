using IPC2_Proyecto2_202401939.Domain;
using IPC2_Proyecto2_202401939.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proyecto2_202401939.Pages.Mensajes;

public class IndexModel : PageModel
{
    private readonly AppStateService _appState;

    public IndexModel(AppStateService appState)
    {
        _appState = appState;
        Mensajes = [];
    }

    public MensajeConfig[] Mensajes { get; private set; }

    // Obtiene los mensajes cargados para mostrarlos en la interfaz.
    public void OnGet()
    {
        Mensajes = _appState.ObtenerMensajes();
    }
}
